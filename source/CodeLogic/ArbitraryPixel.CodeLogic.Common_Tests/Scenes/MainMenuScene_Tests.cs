using ArbitraryPixel.CodeLogic.Common;
using ArbitraryPixel.CodeLogic.Common.Config;
using ArbitraryPixel.CodeLogic.Common.Controllers;
using ArbitraryPixel.CodeLogic.Common.Entities;
using ArbitraryPixel.CodeLogic.Common.Layers;
using ArbitraryPixel.CodeLogic.Common.Model;
using ArbitraryPixel.CodeLogic.Common.Scenes;
using ArbitraryPixel.CodeLogic.Common.UI;
using ArbitraryPixel.Common.Audio;
using ArbitraryPixel.Common.Drawing;
using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Common.Input;
using ArbitraryPixel.Platform2D.Animation;
using ArbitraryPixel.Platform2D.Assets;
using ArbitraryPixel.Platform2D.Engine;
using ArbitraryPixel.Platform2D.Entity;
using ArbitraryPixel.Platform2D.Layer;
using ArbitraryPixel.Platform2D.Scene;
using ArbitraryPixel.Platform2D.Text;
using ArbitraryPixel.Platform2D.UI;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ArbitraryPixel.CodeLogic.Common_Tests.Scenes
{
    [TestClass]
    public class MainMenuScene_Tests
    {
        private MainMenuScene _sut;
        private IEngine _mockEngine;
        private IMenuFactory _mockMenuFactory;
        private IGameStatsData _mockGameStatsData;

        private GameObjectFactory _mockGameObjectFactory;

        private ISpriteBatch _mockSpriteBatch;

        private ILayer _mockMenuLayer;
        private ILayer _mockContentLayer;

        private IMainMenuModel _mockMenuModel;
        private IMenuItem _mockMainMenu;
        private ITextureEntity _mockTextureEntity;
        private ISpriteFont _mockFont;

        private IMenuListView _mockMenuListView;
        private IMenuView _mockMenuTitleView;

        private IAnimationFactory<float> _mockAnimationFactory;
        private ILayerFadeController _mockFadeController;

        private RectangleF _expectedTotalBounds;
        private RectangleF _expectedTitleBounds;
        private RectangleF _expectedMenuBounds;
        private RectangleF _expectedContentBounds;

        [TestInitialize]
        public void Initialize()
        {
            _mockEngine = Substitute.For<IEngine>();
            _mockMenuFactory = Substitute.For<IMenuFactory>();
            _mockGameStatsData = Substitute.For<IGameStatsData>();

            _mockGameObjectFactory = Substitute.For<GameObjectFactory>();
            GameObjectFactory.SetInstance(_mockGameObjectFactory);

            _mockSpriteBatch = Substitute.For<ISpriteBatch>();
            _mockEngine.GrfxFactory.SpriteBatchFactory.Create(Arg.Any<IGrfxDevice>()).Returns(_mockSpriteBatch);
            _mockEngine.ScreenManager.World.Returns(new Point(1500, 1000));
            _mockEngine.AssetBank.Get<ISpriteFont>("MainMenuFont").Returns(_mockFont = Substitute.For<ISpriteFont>());

            _mockFont.LineSpacing.Returns(10);

            _mockGameObjectFactory.CreateGenericLayer(Arg.Any<IEngine>(), Arg.Any<ISpriteBatch>(), SpriteSortMode.Deferred, BlendState.NonPremultiplied).Returns(
                _mockMenuLayer = Substitute.For<ILayer>(),
                _mockContentLayer = Substitute.For<ILayer>()
            );
            _mockMenuLayer.MainSpriteBatch.Returns(_mockSpriteBatch);
            _mockContentLayer.MainSpriteBatch.Returns(_mockSpriteBatch);

            _mockGameObjectFactory.CreateTextureEntity(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>(), Arg.Any<ITexture2D>(), Arg.Any<Color>()).Returns(_mockTextureEntity = Substitute.For<ITextureEntity>());

            _mockMenuFactory.CreateMenuItem("Main Menu", CodeLogicEngine.Constants.MenuItemHeight).Returns(_mockMainMenu = Substitute.For<IMenuItem>());
            _mockMainMenu.CreateChild(Arg.Any<string>(), Arg.Any<float>()).Returns(Substitute.For<IMenuItem>());
            _mockMainMenu.Parent.Returns((IMenuItem)null);
            _mockMainMenu.Text.Returns("Main Menu");

            _mockMenuModel = Substitute.For<IMainMenuModel>();
            _mockMenuModel.RootMenu.Returns(_mockMainMenu);

            _mockMenuFactory.CreateMenuListView(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<IMenuItem>(), Arg.Any<ISpriteBatch>(), Arg.Any<ISpriteFont>(), Arg.Any<IMenuFactory>()).Returns(_mockMenuListView = Substitute.For<IMenuListView>());
            _mockMenuFactory.CreateMenuTitleView(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<IMenuItem>(), Arg.Any<ISpriteBatch>(), Arg.Any<ISpriteFont>()).Returns(_mockMenuTitleView = Substitute.For<IMenuView>());

            _mockMenuListView.ViewOf.Returns(_mockMainMenu);
            _mockMenuTitleView.ViewOf.Returns(_mockMainMenu);

            _mockAnimationFactory = Substitute.For<IAnimationFactory<float>>();
            _mockGameObjectFactory.CreateFloatAnimationFactory().Returns(_mockAnimationFactory);

            _mockFadeController = Substitute.For<ILayerFadeController>();
            _mockGameObjectFactory.CreateLayerFadeController(_mockAnimationFactory).Returns(_mockFadeController);

            // Calculate our expected bounds for each component we build.
            _expectedTotalBounds = new RectangleF(Vector2.Zero, new SizeF(1000, 650));
            _expectedTotalBounds.Location = ((SizeF)_mockEngine.ScreenManager.World).Centre - _expectedTotalBounds.Centre;

            _expectedTitleBounds = new RectangleF(_expectedTotalBounds.Location, new SizeF(_expectedTotalBounds.Width, _mockFont.LineSpacing));

            _expectedMenuBounds = new RectangleF(
                new Vector2(_expectedTitleBounds.Left, _expectedTitleBounds.Bottom),
                new SizeF(350, _expectedTotalBounds.Height - _expectedTitleBounds.Height)
            );

            _expectedContentBounds = new RectangleF(
                new Vector2(_expectedMenuBounds.Right, _expectedTitleBounds.Bottom),
                new SizeF(_expectedTotalBounds.Width - _expectedMenuBounds.Width, _expectedTotalBounds.Height - _expectedTitleBounds.Height)
            );

            _sut = new MainMenuScene(_mockEngine, _mockMenuModel, _mockMenuFactory, _mockGameStatsData);
        }

        #region Constructor Tests
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullParameterShouldThrowException_Model()
        {
            _sut = new MainMenuScene(_mockEngine, null, _mockMenuFactory, _mockGameStatsData);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullParameterShouldThrowException_MenuFactory()
        {
            _sut = new MainMenuScene(_mockEngine, _mockMenuModel, null, _mockGameStatsData);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullParameterShouldThrowException_GameStatsData()
        {
            _sut = new MainMenuScene(_mockEngine, _mockMenuModel, _mockMenuFactory, null);
        }

        [TestMethod]
        public void ConstructShouldAttachToModelStartSinglePlayerGameEvent()
        {
            _mockMenuModel.Received(1).StartSinglePlayerGame += Arg.Any<EventHandler<EventArgs>>();
        }

        [TestMethod]
        public void ConstructShouldAttachToModelExitGameEvent()
        {
            _mockMenuModel.Received(1).ExitGame += Arg.Any<EventHandler<EventArgs>>();
        }

        [TestMethod]
        public void ConstructShouldAttachToModelResetStatisticsEvent()
        {
            _mockMenuModel.Received(1).ResetStatistics += Arg.Any<EventHandler<EventArgs>>();
        }

        [TestMethod]
        public void ConstructShouldAddListenerToHostExternalActionOccurredEvent()
        {
            _mockEngine.Received(1).ExternalActionOccurred += Arg.Any<EventHandler<ExternalActionEventArgs>>();
        }

        [TestMethod]
        public void ConstructShouldCreateAnimationFactory()
        {
            _mockGameObjectFactory.Received(1).CreateFloatAnimationFactory();
        }

        [TestMethod]
        public void ConstructShouldCreateLayerFadeInController()
        {
            _mockGameObjectFactory.Received(1).CreateLayerFadeController(_mockAnimationFactory);
        }
        #endregion

        #region Initialize Tests
        [TestMethod]
        public void InitializeShouldExpectedNumberOfSpriteBatches()
        {
            _sut.Initialize();

            _mockEngine.GrfxFactory.SpriteBatchFactory.Received(3).Create(Arg.Any<IGrfxDevice>());
        }

        [TestMethod]
        public void InitializeShouldCreateMenuBackgroundLayer()
        {
            ISoundPlaybackController mockSoundController = Substitute.For<ISoundPlaybackController>();
            _mockGameObjectFactory.CreateSoundPlaybackController().Returns(mockSoundController);

            _sut.Initialize();

            _mockGameObjectFactory.Received(1).CreateMenuBackgroundLayer(Arg.Any<IEngine>(), _mockSpriteBatch, mockSoundController);
        }

        [TestMethod]
        public void InitializeShouldAddBackgroundLayerToEntities()
        {
            IMenuBackgroundLayer mockLayer = Substitute.For<IMenuBackgroundLayer>();
            _mockGameObjectFactory.CreateMenuBackgroundLayer(Arg.Any<IEngine>(), Arg.Any<ISpriteBatch>(), Arg.Any<ISoundPlaybackController>()).Returns(mockLayer);

            _sut.Initialize();

            Assert.IsTrue(_sut.Entities.Contains(mockLayer));
        }

        [TestMethod]
        public void InitializeShouldAttachToBackgroundLayerFadeOutCompleteEvent()
        {
            IMenuBackgroundLayer mockLayer = Substitute.For<IMenuBackgroundLayer>();
            _mockGameObjectFactory.CreateMenuBackgroundLayer(Arg.Any<IEngine>(), Arg.Any<ISpriteBatch>(), Arg.Any<ISoundPlaybackController>()).Returns(mockLayer);

            _sut.Initialize();

            mockLayer.Received(1).SequenceEnded += Arg.Any<EventHandler<EventArgs>>();
        }

        [TestMethod]
        public void InitializeShouldCreateExpectedNumberOfGenericLayers()
        {
            _sut.Initialize();

            _mockGameObjectFactory.Received(2).CreateGenericLayer(Arg.Any<IEngine>(), _mockSpriteBatch, SpriteSortMode.Deferred, BlendState.NonPremultiplied);
        }

        [TestMethod]
        public void InitializeShouldRequestMainMenuFontFromAssetBank()
        {
            _sut.Initialize();

            _mockEngine.AssetBank.Received(1).Get<ISpriteFont>("MainMenuFont");
        }

        [TestMethod]
        public void InitializeShouldRequestPixelTextureFromAssetBank()
        {
            _sut.Initialize();

            _mockEngine.AssetBank.Received(1).Get<ITexture2D>("Pixel");
        }

        [TestMethod]
        public void InitializeShouldCreateTextureEntity()
        {
            ISpriteBatch mockSpriteBatch = Substitute.For<ISpriteBatch>();
            ITexture2D mockPixel = Substitute.For<ITexture2D>();
            _mockContentLayer.MainSpriteBatch.Returns(mockSpriteBatch);
            _mockEngine.AssetBank.Get<ITexture2D>("Pixel").Returns(mockPixel);

            _sut.Initialize();

            _mockGameObjectFactory.Received(1).CreateTextureEntity(_mockEngine, _expectedContentBounds, mockSpriteBatch, mockPixel, CodeLogicEngine.Constants.ClrMenuBGLow);
        }

        [TestMethod]
        public void InitializeShouldAddContentBackgroundEntityToContentLayer()
        {
            _sut.Initialize();

            _mockContentLayer.Received(1).AddEntity(_mockTextureEntity);
        }

        [TestMethod]
        public void InitializeShouldCreateMenuListView()
        {
            _sut.Initialize();

            _mockMenuFactory.Received(1).CreateMenuListView(_mockEngine, _expectedMenuBounds, _mockMainMenu, _mockSpriteBatch, _mockFont, _mockMenuFactory);
        }

        [TestMethod]
        public void InitializeShouldCreateMenuTitleView()
        {
            _sut.Initialize();

            _mockMenuFactory.Received(1).CreateMenuTitleView(_mockEngine, _expectedTitleBounds, _mockMainMenu, _mockSpriteBatch, _mockFont);
        }

        [TestMethod]
        public void InitializeShouldAttachToMenuListViewEvents()
        {
            _sut.Initialize();

            Received.InOrder(
                () =>
                {
                    _mockMenuListView.MenuBackTapped += Arg.Any<EventHandler<EventArgs>>();
                    _mockMenuListView.MenuItemTapped += Arg.Any<EventHandler<MenuItemEventArgs>>();
                    _mockMenuListView.SelectedItemChanged += Arg.Any<EventHandler<MenuItemEventArgs>>();
                }
            );
        }

        [TestMethod]
        public void InitializeShouldAddExpectedEntitiesToMenuLayer()
        {
            ITexture2D mockPixel = Substitute.For<ITexture2D>();
            _mockEngine.AssetBank.Get<ITexture2D>("Pixel").Returns(mockPixel);
            ITextureEntity mockMenuBG = Substitute.For<ITextureEntity>();
            _mockGameObjectFactory.CreateTextureEntity(_mockEngine, _expectedTotalBounds, _mockSpriteBatch, mockPixel, CodeLogicEngine.Constants.ClrMenuBG).Returns(mockMenuBG);

            _sut.Initialize();

            Received.InOrder(
                () =>
                {
                    _mockMenuLayer.AddEntity(mockMenuBG);
                    _mockMenuLayer.AddEntity(_mockMenuListView);
                    _mockMenuLayer.AddEntity(_mockMenuTitleView);
                }
            );
        }

        [TestMethod]
        public void InitializeShouldCallModelBuildContentMap()
        {
            _sut.Initialize();

            _mockMenuModel.Received(1).BuildContentMap(_mockEngine, _mockSpriteBatch, _expectedContentBounds);
        }

        [TestMethod]
        public void InitializeShouldCreateExitDialog()
        {
            _mockEngine.ScreenManager.World.Returns(new Point(1000, 700));
            RectangleF expectedBounds = new RectangleF(125, 250, 750, 200);

            ITextObjectBuilder mockTextBuilder = Substitute.For<ITextObjectBuilder>();
            _mockGameObjectFactory.CreateTextObjectBuilderWithConsoleFonts(Arg.Any<ITextFormatProcessor>(), Arg.Any<ITextObjectFactory>(), _mockEngine.AssetBank).Returns(mockTextBuilder);

            IDialog mockDialog = Substitute.For<IDialog>();
            _mockGameObjectFactory.CreateOkCancelDialog(_mockEngine, expectedBounds, mockTextBuilder, Arg.Any<string>()).Returns(mockDialog, Substitute.For<IDialog>());

            _sut.Initialize();

            mockDialog.Received(1).DialogClosed += Arg.Any<EventHandler<DialogClosedEventArgs>>();
        }

        [TestMethod]
        public void InitializeShouldAddExitDialogToEntities()
        {
            _mockEngine.ScreenManager.World.Returns(new Point(1000, 700));
            RectangleF expectedBounds = new RectangleF(125, 250, 750, 200);

            ITextObjectBuilder mockTextBuilder = Substitute.For<ITextObjectBuilder>();
            _mockGameObjectFactory.CreateTextObjectBuilderWithConsoleFonts(Arg.Any<ITextFormatProcessor>(), Arg.Any<ITextObjectFactory>(), _mockEngine.AssetBank).Returns(mockTextBuilder);

            IDialog mockDialog = Substitute.For<IDialog>();
            _mockGameObjectFactory.CreateOkCancelDialog(_mockEngine, expectedBounds, mockTextBuilder, Arg.Any<string>()).Returns(mockDialog, Substitute.For<IDialog>());

            _sut.Initialize();

            Assert.IsTrue(_sut.Entities.Contains(mockDialog));
        }

        [TestMethod]
        public void InitializeShouldCreateResetStatsDialog()
        {
            _mockEngine.ScreenManager.World.Returns(new Point(1000, 700));
            RectangleF expectedBounds = new RectangleF(125, 250, 750, 200);

            ITextObjectBuilder mockTextBuilder = Substitute.For<ITextObjectBuilder>();
            _mockGameObjectFactory.CreateTextObjectBuilderWithConsoleFonts(Arg.Any<ITextFormatProcessor>(), Arg.Any<ITextObjectFactory>(), _mockEngine.AssetBank).Returns(mockTextBuilder);

            IDialog mockDialog = Substitute.For<IDialog>();
            _mockGameObjectFactory.CreateOkCancelDialog(_mockEngine, expectedBounds, mockTextBuilder, Arg.Any<string>()).Returns(Substitute.For<IDialog>(), mockDialog);

            _sut.Initialize();

            mockDialog.Received(1).DialogClosed += Arg.Any<EventHandler<DialogClosedEventArgs>>();
        }

        [TestMethod]
        public void InitializeShouldAddResetStatsDialogToEntities()
        {
            _mockEngine.ScreenManager.World.Returns(new Point(1000, 700));
            RectangleF expectedBounds = new RectangleF(125, 250, 750, 200);

            ITextObjectBuilder mockTextBuilder = Substitute.For<ITextObjectBuilder>();
            _mockGameObjectFactory.CreateTextObjectBuilderWithConsoleFonts(Arg.Any<ITextFormatProcessor>(), Arg.Any<ITextObjectFactory>(), _mockEngine.AssetBank).Returns(mockTextBuilder);

            IDialog mockDialog = Substitute.For<IDialog>();
            _mockGameObjectFactory.CreateOkCancelDialog(_mockEngine, expectedBounds, mockTextBuilder, Arg.Any<string>()).Returns(Substitute.For<IDialog>(), mockDialog);

            _sut.Initialize();

            Assert.IsTrue(_sut.Entities.Contains(mockDialog));
        }

        [TestMethod]
        public void InitializeShouldAddLayerToLayerFadeInController_ContentLayer()
        {
            _sut.Initialize();

            _mockFadeController.Received(1).AddLayer(_mockContentLayer);
        }

        [TestMethod]
        public void InitializeShouldAddLayerToLayerFadeInController_MenuLayer()
        {
            _sut.Initialize();

            _mockFadeController.Received(1).AddLayer(_mockMenuLayer);
        }

        [TestMethod]
        public void InitializeShouldResetLayerFadeInController()
        {
            _sut.Initialize();

            _mockFadeController.Received(1).Reset();
        }

        [TestMethod]
        public void InitializeShouldCreateGenericButtonAsInputBlocker()
        {
            _sut.Initialize();

            _mockGameObjectFactory.Received(1).CreateButtonBase(_mockEngine, new RectangleF(0, 0, 1500, 1000));
        }

        [TestMethod]
        public void InitializeShouldSetInputBlockerButtonVisibleToFalse()
        {
            IButton mockButton = Substitute.For<IButton>();
            _mockGameObjectFactory.CreateButtonBase(_mockEngine, Arg.Any<RectangleF>()).Returns(mockButton);

            _sut.Initialize();

            mockButton.Received(1).Visible = false;
        }

        [TestMethod]
        public void InitializeShouldSetInputBlockerButtonEnabledToFalse()
        {
            IButton mockButton = Substitute.For<IButton>();
            _mockGameObjectFactory.CreateButtonBase(_mockEngine, Arg.Any<RectangleF>()).Returns(mockButton);

            _sut.Initialize();

            mockButton.Received(1).Enabled = false;
        }

        [TestMethod]
        public void InitializeShouldSetInputBlockerButtonToLastEntity()
        {
            IButton mockButton = Substitute.For<IButton>();
            _mockGameObjectFactory.CreateButtonBase(_mockEngine, Arg.Any<RectangleF>()).Returns(mockButton);

            _sut.Initialize();

            Assert.AreSame(mockButton, _sut.Entities[_sut.Entities.Count - 1]);
        }

        [TestMethod]
        public void InitializeShouldAddLogoToMenuLayer()
        {
            ISpriteBatch mockSpriteBatch = Substitute.For<ISpriteBatch>();
            _mockMenuLayer.MainSpriteBatch.Returns(mockSpriteBatch);

            SizeF textureSize = new SizeF(200, 100);
            ITexture2D mockTexture = Substitute.For<ITexture2D>();
            mockTexture.Width.Returns((int)textureSize.Width);
            mockTexture.Height.Returns((int)textureSize.Height);
            _mockEngine.AssetBank.Get<ITexture2D>("APLogo").Returns(mockTexture);

            SizeF expectedSize = new SizeF(textureSize.Width / 2f, textureSize.Height / 2f);
            RectangleF expectedBounds = new RectangleF(new Vector2(1500, 1000) - expectedSize - CodeLogicEngine.Constants.TextWindowPadding, expectedSize);

            ITextureEntity mockEntity = Substitute.For<ITextureEntity>();
            _mockGameObjectFactory.CreateTextureEntity(_mockEngine, expectedBounds, mockSpriteBatch, mockTexture, Color.White * 0.5f).Returns(mockEntity);

            _sut.Initialize();

            _mockMenuLayer.Received(1).AddEntity(mockEntity);
        }

        [TestMethod]
        public void InitializeWithTransientSettingsShouldAddDelayedDialogControllerToEntities()
        {
            _mockEngine.GetComponent<ICodeLogicSettings>().IsTransient.Returns(true);
            _mockEngine.ScreenManager.World.Returns(new Point(1000, 800));

            RectangleF expectedDialogBounds = new RectangleF(125, 300, 750, 200);

            ITextObjectBuilder mockTextBuilder = Substitute.For<ITextObjectBuilder>();
            _mockGameObjectFactory.CreateTextObjectBuilderWithConsoleFonts(Arg.Any<ITextFormatProcessor>(), Arg.Any<ITextObjectFactory>(), Arg.Any<IAssetBank>()).Returns(mockTextBuilder);

            IDialog mockDialog = Substitute.For<IDialog>();
            _mockGameObjectFactory.CreateDialog(_mockEngine, expectedDialogBounds, mockTextBuilder, CodeLogicEngine.Constants.DialogText_NoStoragePermissions).Returns(mockDialog);

            IEntity mockDialogController = Substitute.For<IEntity>();
            _mockGameObjectFactory.CreateDelayedDialogController(_mockEngine, mockDialog, 1.0).Returns(mockDialogController);

            _sut.Initialize();

            Assert.IsTrue(_sut.Entities.ToList().Contains(mockDialogController));
        }

        [TestMethod]
        public void InitializeWithTransientSettingsShouldAddEventHandlerToDelayedDialogControllerDisposedEvent()
        {
            _mockEngine.GetComponent<ICodeLogicSettings>().IsTransient.Returns(true);
            _mockEngine.ScreenManager.World.Returns(new Point(1000, 800));

            RectangleF expectedDialogBounds = new RectangleF(125, 300, 750, 200);

            ITextObjectBuilder mockTextBuilder = Substitute.For<ITextObjectBuilder>();
            _mockGameObjectFactory.CreateTextObjectBuilderWithConsoleFonts(Arg.Any<ITextFormatProcessor>(), Arg.Any<ITextObjectFactory>(), Arg.Any<IAssetBank>()).Returns(mockTextBuilder);

            IDialog mockDialog = Substitute.For<IDialog>();
            _mockGameObjectFactory.CreateDialog(_mockEngine, expectedDialogBounds, mockTextBuilder, CodeLogicEngine.Constants.DialogText_NoStoragePermissions).Returns(mockDialog);

            IEntity mockDialogController = Substitute.For<IEntity>();
            _mockGameObjectFactory.CreateDelayedDialogController(_mockEngine, mockDialog, 1.0).Returns(mockDialogController);

            _sut.Initialize();

            mockDialogController.Received(1).Disposed += Arg.Any<EventHandler<EventArgs>>();
        }

        [TestMethod]
        public void InitializeWithoutTransientSettingsShouldNotAddDelayDialogControllerToEntities()
        {
            _mockEngine.GetComponent<ICodeLogicSettings>().IsTransient.Returns(false);
            IEntity mockDialogController = Substitute.For<IEntity>();
            _mockGameObjectFactory.CreateDelayedDialogController(Arg.Any<IEngine>(), Arg.Any<IDialog>(), Arg.Any<double>()).Returns(mockDialogController);

            _sut.Initialize();

            Assert.IsFalse(_sut.Entities.ToList().Contains(mockDialogController));
        }
        #endregion

        #region Update Tests
        [TestMethod]
        public void UpdateShouldCallUpdateOnLayerFadeController()
        {
            GameTime gameTime = new GameTime();
            _sut.Update(gameTime);

            _mockFadeController.Received(1).Update(gameTime);
        }

        [TestMethod]
        public void UpdateWhenConditionsMetShouldStartAnimationOnFadeController()
        {
            _mockFadeController.IsAnimating.Returns(false);
            _mockFadeController.IsAnimationComplete(FadeMode.FadeIn).Returns(false);
            _mockEngine.InputManager.IsActive.Returns(true);
            _mockEngine.InputManager.GetSurfaceState().Returns(new SurfaceState(Vector2.Zero, true));

            _sut.Initialize();

            _sut.Update(new GameTime());

            _mockFadeController.Received(1).StartAnimation(FadeMode.FadeIn);
        }

        [TestMethod]
        public void UpdateWhenConditionsMetShouldHideTextOnBackgroundLayer()
        {
            // Not going to test the negative conditions of this one since we test it on the fade controller and the logic is the same. Change this if we ever change the logic ;)

            _mockFadeController.IsAnimating.Returns(false);
            _mockFadeController.IsAnimationComplete(FadeMode.FadeIn).Returns(false);
            _mockEngine.InputManager.IsActive.Returns(true);
            _mockEngine.InputManager.GetSurfaceState().Returns(new SurfaceState(Vector2.Zero, true));

            IMenuBackgroundLayer mockBGLayer = Substitute.For<IMenuBackgroundLayer>();
            _mockGameObjectFactory.CreateMenuBackgroundLayer(Arg.Any<IEngine>(), Arg.Any<ISpriteBatch>(), Arg.Any<ISoundPlaybackController>()).Returns(mockBGLayer);

            _sut.Initialize();

            _sut.Update(new GameTime());

            mockBGLayer.Received(1).HideText();
        }

        [TestMethod]
        public void UpdateWhenConditionsNotMetShouldNotStartAnimationOnFadeController_ControllerIsAnimating_True()
        {
            _mockFadeController.IsAnimating.Returns(true);
            _mockFadeController.IsAnimationComplete(FadeMode.FadeIn).Returns(false);
            _mockEngine.InputManager.IsActive.Returns(true);
            _mockEngine.InputManager.GetSurfaceState().Returns(new SurfaceState(Vector2.Zero, true));

            _sut.Update(new GameTime());

            _mockFadeController.Received(0).StartAnimation(Arg.Any<FadeMode>());
        }

        [TestMethod]
        public void UpdateWhenConditionsNotMetShouldNotStartAnimationOnFadeController_IsAnimationCompleteFadeIn_True()
        {
            _mockFadeController.IsAnimating.Returns(false);
            _mockFadeController.IsAnimationComplete(FadeMode.FadeIn).Returns(true);
            _mockEngine.InputManager.IsActive.Returns(true);
            _mockEngine.InputManager.GetSurfaceState().Returns(new SurfaceState(Vector2.Zero, true));

            _sut.Update(new GameTime());

            _mockFadeController.Received(0).StartAnimation(Arg.Any<FadeMode>());
        }

        [TestMethod]
        public void UpdateWhenConditionsNotMetShouldNotStartAnimationOnFadeController_InputManagerIsActive_False()
        {
            _mockFadeController.IsAnimating.Returns(false);
            _mockFadeController.IsAnimationComplete(FadeMode.FadeIn).Returns(false);
            _mockEngine.InputManager.IsActive.Returns(false);
            _mockEngine.InputManager.GetSurfaceState().Returns(new SurfaceState(Vector2.Zero, true));

            _sut.Update(new GameTime());

            _mockFadeController.Received(0).StartAnimation(Arg.Any<FadeMode>());
        }

        [TestMethod]
        public void UpdateWhenConditionsNotMetShouldNotStartAnimationOnFadeController_InputManagerGetSurfaceStateIsTouched_False()
        {
            _mockFadeController.IsAnimating.Returns(false);
            _mockFadeController.IsAnimationComplete(FadeMode.FadeIn).Returns(false);
            _mockEngine.InputManager.IsActive.Returns(true);
            _mockEngine.InputManager.GetSurfaceState().Returns(new SurfaceState(Vector2.Zero, false));

            _sut.Update(new GameTime());

            _mockFadeController.Received(0).StartAnimation(Arg.Any<FadeMode>());
        }
        #endregion

        #region MenuListView Event Tests
        [TestMethod]
        public void MenuBackTappedWithParentShouldSetListViewViewOfToParent()
        {
            IMenuItem parentItem = Substitute.For<IMenuItem>();
            parentItem.Text.Returns("Parent");
            _mockMainMenu.Parent.Returns(parentItem);
            _sut.Initialize();

            _mockMenuListView.MenuBackTapped += Raise.Event<EventHandler<EventArgs>>(_mockMenuListView, new EventArgs());

            _mockMenuListView.Received(1).ViewOf = parentItem;
        }

        [TestMethod]
        public void MenuBackTappedWithoutParentShouldNotSetListViewViewOf()
        {
            _sut.Initialize();

            _mockMenuListView.MenuBackTapped += Raise.Event<EventHandler<EventArgs>>(_mockMenuListView, new EventArgs());

            _mockMenuListView.Received(0).ViewOf = Arg.Any<IMenuItem>();
        }

        [TestMethod]
        public void MenuItemTappedWithNoChildItemsShouldSetListViewSelectedItemToEventArgItem()
        {
            IMenuItem mockItem = Substitute.For<IMenuItem>();
            _sut.Initialize();

            _mockMenuListView.MenuItemTapped += Raise.Event<EventHandler<MenuItemEventArgs>>(_mockMenuListView, new MenuItemEventArgs(mockItem));

            _mockMenuListView.Received(1).SelectedItem = mockItem;
        }

        [TestMethod]
        public void MenuItemTappedWithChildItemsShouldSetListViewViewOfToEventArgItem()
        {
            IMenuItem mockItem = Substitute.For<IMenuItem>();
            mockItem.Items.Returns(new IMenuItem[] { Substitute.For<IMenuItem>() });
            _sut.Initialize();

            _mockMenuListView.MenuItemTapped += Raise.Event<EventHandler<MenuItemEventArgs>>(_mockMenuListView, new MenuItemEventArgs(mockItem));

            _mockMenuListView.Received(1).ViewOf = mockItem;
        }

        [TestMethod]
        public void MenuItemTappedWithConfigChangesShouldPersist()
        {
            IMenuItem mockItem = Substitute.For<IMenuItem>();
            mockItem.Items.Returns(new IMenuItem[] { Substitute.For<IMenuItem>() });
            ICodeLogicSettings mockConfigStore = Substitute.For<ICodeLogicSettings>();
            mockConfigStore.CacheChanged.Returns(true);
            _mockEngine.GetComponent<ICodeLogicSettings>().Returns(mockConfigStore);
            _sut.Initialize();

            _mockMenuListView.MenuItemTapped += Raise.Event<EventHandler<MenuItemEventArgs>>(_mockMenuListView, new MenuItemEventArgs(mockItem));

            mockConfigStore.Received(1).PersistCache();
        }

        [TestMethod]
        public void MenuItemTappedWithoutConfigChangesShouldNotPersist()
        {
            IMenuItem mockItem = Substitute.For<IMenuItem>();
            mockItem.Items.Returns(new IMenuItem[] { Substitute.For<IMenuItem>() });
            ICodeLogicSettings mockConfigStore = Substitute.For<ICodeLogicSettings>();
            mockConfigStore.CacheChanged.Returns(false);
            _mockEngine.GetComponent<ICodeLogicSettings>().Returns(mockConfigStore);
            _sut.Initialize();

            _mockMenuListView.MenuItemTapped += Raise.Event<EventHandler<MenuItemEventArgs>>(_mockMenuListView, new MenuItemEventArgs(mockItem));

            mockConfigStore.Received(0).PersistCache();
        }

        [TestMethod]
        public void SelectedItemChangedWithNoSelectedItemShouldSetTitleViewViewOfToListViewViewOf()
        {
            IMenuItem mockOther = Substitute.For<IMenuItem>();
            _mockMenuListView.ViewOf = mockOther;
            _mockMenuListView.SelectedItem.Returns((IMenuItem)null);
            _sut.Initialize();

            _mockMenuListView.SelectedItemChanged += Raise.Event<EventHandler<MenuItemEventArgs>>(_mockMenuListView, new MenuItemEventArgs(Substitute.For<IMenuItem>()));

            _mockMenuTitleView.Received(1).ViewOf = mockOther;
        }

        [TestMethod]
        public void SelectedItemChangedWithSelectedItemShouldSetTitleViewViewOfToListViewSElectedItem()
        {
            IMenuItem mockOther = Substitute.For<IMenuItem>();
            _mockMenuListView.SelectedItem.Returns((IMenuItem)mockOther);
            _sut.Initialize();

            _mockMenuListView.SelectedItemChanged += Raise.Event<EventHandler<MenuItemEventArgs>>(_mockMenuListView, new MenuItemEventArgs(Substitute.For<IMenuItem>()));

            _mockMenuTitleView.Received(1).ViewOf = mockOther;
        }

        [TestMethod]
        public void SelectedItemChangedShouldCallContentLayerClearEntities()
        {
            _sut.Initialize();

            _mockMenuListView.SelectedItemChanged += Raise.Event<EventHandler<MenuItemEventArgs>>(_mockMenuListView, new MenuItemEventArgs(Substitute.For<IMenuItem>()));

            _mockContentLayer.Received(1).ClearEntities();
        }

        [TestMethod]
        public void SelectedItemChangedShouldCallContentLayerAddEntityWithContentBackground()
        {
            _sut.Initialize();
            _mockContentLayer.ClearReceivedCalls();

            _mockMenuListView.SelectedItemChanged += Raise.Event<EventHandler<MenuItemEventArgs>>(_mockMenuListView, new MenuItemEventArgs(Substitute.For<IMenuItem>()));

            _mockContentLayer.Received(1).AddEntity(_mockTextureEntity);
        }

        [TestMethod]
        public void SelectedItemChangedShouldCheckIfItemHasMappedLayer()
        {
            IMenuItem mockItem = Substitute.For<IMenuItem>();
            _sut.Initialize();

            _mockMenuListView.SelectedItemChanged += Raise.Event<EventHandler<MenuItemEventArgs>>(_mockMenuListView, new MenuItemEventArgs(mockItem));

            _mockMenuModel.ContentMap.Received(1).HasMappedLayer(mockItem);
        }

        [TestMethod]
        public void SelectedItemChangedWithNullItemShouldNotCheckForMappedLayer()
        {
            _sut.Initialize();

            _mockMenuListView.SelectedItemChanged += Raise.Event<EventHandler<MenuItemEventArgs>>(_mockMenuListView, new MenuItemEventArgs(null));

            _mockMenuModel.ContentMap.Received(0).HasMappedLayer(Arg.Any<IMenuItem>());
        }

        [TestMethod]
        public void SelectedItemChangedWithMappedItemShouldAddMappedLayerToContentLayer()
        {
            IMenuItem mockItem = Substitute.For<IMenuItem>();
            IMenuContentLayer mockLayer = Substitute.For<IMenuContentLayer>();
            _sut.Initialize();
            _mockContentLayer.ClearReceivedCalls();

            _mockMenuModel.ContentMap.GetLayer(mockItem).Returns(mockLayer);
            _mockMenuModel.ContentMap.HasMappedLayer(mockItem).Returns(true);

            _mockMenuListView.SelectedItemChanged += Raise.Event<EventHandler<MenuItemEventArgs>>(_mockMenuListView, new MenuItemEventArgs(mockItem));

            _mockContentLayer.Received(1).AddEntity(mockLayer);
        }

        [TestMethod]
        public void SelectedItemChangedWithMappedItemShouldCallShowOnContentLayer()
        {
            IMenuItem mockItem = Substitute.For<IMenuItem>();
            IMenuContentLayer mockLayer = Substitute.For<IMenuContentLayer>();
            _sut.Initialize();
            _mockContentLayer.ClearReceivedCalls();

            _mockMenuModel.ContentMap.GetLayer(mockItem).Returns(mockLayer);
            _mockMenuModel.ContentMap.HasMappedLayer(mockItem).Returns(true);

            _mockMenuListView.SelectedItemChanged += Raise.Event<EventHandler<MenuItemEventArgs>>(_mockMenuListView, new MenuItemEventArgs(mockItem));

            mockLayer.Received(1).Show();
        }

        [TestMethod]
        public void SelectedItemChangedWithExistingContentLayerShouldCallHideOnPreviousContentLayer()
        {
            IMenuItem mockItem = Substitute.For<IMenuItem>();
            IMenuItem mockNextItem = Substitute.For<IMenuItem>();
            IMenuContentLayer mockLayer = Substitute.For<IMenuContentLayer>();
            IMenuContentLayer mockNextLayer = Substitute.For<IMenuContentLayer>();
            _sut.Initialize();
            _mockContentLayer.ClearReceivedCalls();

            _mockMenuModel.ContentMap.GetLayer(mockItem).Returns(mockLayer);
            _mockMenuModel.ContentMap.HasMappedLayer(mockItem).Returns(true);

            _mockMenuModel.ContentMap.GetLayer(mockNextItem).Returns(mockNextLayer);
            _mockMenuModel.ContentMap.HasMappedLayer(mockNextItem).Returns(true);

            _mockMenuListView.SelectedItemChanged += Raise.Event<EventHandler<MenuItemEventArgs>>(_mockMenuListView, new MenuItemEventArgs(mockItem));
            _mockMenuListView.SelectedItemChanged += Raise.Event<EventHandler<MenuItemEventArgs>>(_mockMenuListView, new MenuItemEventArgs(mockNextItem));

            mockLayer.Received(1).Hide();
        }

        [TestMethod]
        public void SelectedItemChangedShouldPlayExpectedSound()
        {
            ISoundResource mockSound = Substitute.For<ISoundResource>();
            _mockEngine.AssetBank.Get<ISoundResource>("ButtonPress").Returns(mockSound);
            _sut.Initialize();

            _mockMenuListView.SelectedItemChanged += Raise.Event<EventHandler<MenuItemEventArgs>>(_mockMenuListView, new MenuItemEventArgs(Substitute.For<IMenuItem>()));

            mockSound.Received(1).Play();
        }

        [TestMethod]
        public void SelectedItemChangedShouldNotPlaySoundWhenItemIsNull()
        {
            ISoundResource mockSound = Substitute.For<ISoundResource>();
            _mockEngine.AssetBank.Get<ISoundResource>("ButtonPress").Returns(mockSound);
            _sut.Initialize();

            _mockMenuListView.SelectedItemChanged += Raise.Event<EventHandler<MenuItemEventArgs>>(_mockMenuListView, new MenuItemEventArgs(null));

            mockSound.Received(0).Play();
        }
        #endregion

        #region MenuBackgroundLayer Event Tests
        [TestMethod]
        public void BackgroundLayerFadeOutCompleteShouldSetNextSceneToDeviceAssetLoad()
        {
            IMenuBackgroundLayer mockBGLayer = Substitute.For<IMenuBackgroundLayer>();
            _mockGameObjectFactory.CreateMenuBackgroundLayer(Arg.Any<IEngine>(), Arg.Any<ISpriteBatch>(), Arg.Any<ISoundPlaybackController>()).Returns(mockBGLayer);
            _sut.Initialize();

            IScene mockScene = Substitute.For<IScene>();
            Dictionary<string, IScene> scenes = new Dictionary<string, IScene>();
            scenes.Add("DeviceAssetLoad", mockScene);
            _mockEngine.Scenes.Returns(scenes);

            mockBGLayer.SequenceEnded += Raise.Event<EventHandler<EventArgs>>(mockBGLayer, new EventArgs());

            Assert.AreSame(mockScene, _sut.NextScene);
        }

        [TestMethod]
        public void BackgroundLayerFadeOutCompleteShouldSetSceneCompleteTrue()
        {
            IMenuBackgroundLayer mockBGLayer = Substitute.For<IMenuBackgroundLayer>();
            _mockGameObjectFactory.CreateMenuBackgroundLayer(Arg.Any<IEngine>(), Arg.Any<ISpriteBatch>(), Arg.Any<ISoundPlaybackController>()).Returns(mockBGLayer);
            _sut.Initialize();

            IScene mockScene = Substitute.For<IScene>();
            Dictionary<string, IScene> scenes = new Dictionary<string, IScene>();
            scenes.Add("DeviceAssetLoad", mockScene);
            _mockEngine.Scenes.Returns(scenes);

            mockBGLayer.SequenceEnded += Raise.Event<EventHandler<EventArgs>>(mockBGLayer, new EventArgs());

            Assert.IsTrue(_sut.SceneComplete);
        }
        #endregion

        #region Model Event Tests
        [TestMethod]
        public void ModelStartSinglePlayerGameShouldCallBackgroundLayerStartFadeOut()
        {
            IMenuBackgroundLayer mockBGLayer = Substitute.For<IMenuBackgroundLayer>();
            _mockGameObjectFactory.CreateMenuBackgroundLayer(Arg.Any<IEngine>(), Arg.Any<ISpriteBatch>(), Arg.Any<ISoundPlaybackController>()).Returns(mockBGLayer);
            _sut.Initialize();

            _mockMenuModel.StartSinglePlayerGame += Raise.Event<EventHandler<EventArgs>>(_mockMenuModel, new EventArgs());

            mockBGLayer.Received(1).StartEndSequence();
        }

        [TestMethod]
        public void ModelStartSinglePlayerGameShouldCallLayerFadeControllerStartAnimationWithExpectedValue()
        {
            _sut.Initialize();

            _mockMenuModel.StartSinglePlayerGame += Raise.Event<EventHandler<EventArgs>>(_mockMenuModel, new EventArgs());

            _mockFadeController.Received(1).StartAnimation(FadeMode.FadeOut);
        }

        [TestMethod]
        public void ModelStartSinglePlayerGameShouldSetInputBlockerButtonEnabledToTrue()
        {
            IButton mockButton = Substitute.For<IButton>();
            _mockGameObjectFactory.CreateButtonBase(_mockEngine, Arg.Any<RectangleF>()).Returns(mockButton);

            _sut.Initialize();

            _mockMenuModel.StartSinglePlayerGame += Raise.Event<EventHandler<EventArgs>>(_mockMenuModel, new EventArgs());

            mockButton.Received(1).Enabled = true;
        }

        [TestMethod]
        public void ModelExitGameEventShouldSetHostFinishedToTrue()
        {
            _mockMenuModel.ExitGame += Raise.Event<EventHandler<EventArgs>>(_mockMenuModel, new EventArgs());

            _mockEngine.Received(1).Exit();
        }

        [TestMethod]
        public void ModelResetStatisticsWithClosedDialogShouldCallDialogShow()
        {
            IDialog mockDialog = Substitute.For<IDialog>();
            mockDialog.IsOpen.Returns(false);
            _mockGameObjectFactory.CreateOkCancelDialog(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ITextObjectBuilder>(), Arg.Any<string>()).Returns(Substitute.For<IDialog>(), mockDialog);

            _sut.Initialize();

            _mockMenuModel.ResetStatistics += Raise.Event<EventHandler<EventArgs>>(_mockMenuModel, new EventArgs());

            mockDialog.Received(1).Show();
        }

        [TestMethod]
        public void ModelResetStatisticsWithDialogOpenShouldNotCallDialogShow()
        {
            IDialog mockDialog = Substitute.For<IDialog>();
            mockDialog.IsOpen.Returns(true);
            _mockGameObjectFactory.CreateOkCancelDialog(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ITextObjectBuilder>(), Arg.Any<string>()).Returns(Substitute.For<IDialog>(), mockDialog);

            _sut.Initialize();

            _mockMenuModel.ResetStatistics += Raise.Event<EventHandler<EventArgs>>(_mockMenuModel, new EventArgs());

            mockDialog.Received(0).Show();
        }
        #endregion

        #region Reset Tests
        [TestMethod]
        public void ResetShouldSetNextSceneNull()
        {
            _sut.Initialize();
            _sut.NextScene = Substitute.For<IScene>();

            _sut.Reset();

            Assert.IsNull(_sut.NextScene);
        }

        [TestMethod]
        public void ResetShouldSetSceneCompleteFalse()
        {
            _sut.Initialize();
            _sut.SceneComplete = true;

            _sut.Reset();

            Assert.IsFalse(_sut.SceneComplete);
        }

        [TestMethod]
        public void ResetShouldSetTitleViewOfToModelRootMenu()
        {
            _sut.Initialize();

            IMenuItem mockItem = Substitute.For<IMenuItem>();
            _mockMenuModel.RootMenu.Returns(mockItem);

            _sut.Reset();

            _mockMenuTitleView.Received(1).ViewOf = mockItem;
        }

        [TestMethod]
        public void ResetShouldSetMainListViewSelectedItemNull()
        {
            _sut.Initialize();

            _sut.Reset();

            _mockMenuListView.Received(1).SelectedItem = null;
        }

        [TestMethod]
        public void ResetShouldSetMainListViewOfToModelRootMenu()
        {
            _sut.Initialize();

            IMenuItem mockItem = Substitute.For<IMenuItem>();
            _mockMenuModel.RootMenu.Returns(mockItem);

            _sut.Reset();

            _mockMenuListView.Received(1).ViewOf = mockItem;
        }

        [TestMethod]
        public void ResetShouldCallUpdateOnListViewAfterResettingValues()
        {
            _sut.Initialize();

            IMenuItem mockItem = Substitute.For<IMenuItem>();
            _mockMenuModel.RootMenu.Returns(mockItem);

            _sut.Reset();

            Received.InOrder(
                () =>
                {
                    // NOTE: Order of the next two lines doesn't actually matter... I just need both of them to be done before we call update.
                    _mockMenuListView.SelectedItem = null;
                    _mockMenuListView.ViewOf = mockItem;

                    _mockMenuListView.Update(Arg.Any<GameTime>()); // Don't care what the gametime is, just need any update.
                }
            );
        }

        [TestMethod]
        public void ResetShouldCallResetOnBackgroundLayer()
        {
            IMenuBackgroundLayer mockBackgroundLayer = Substitute.For<IMenuBackgroundLayer>();
            _mockGameObjectFactory.CreateMenuBackgroundLayer(_mockEngine, Arg.Any<ISpriteBatch>(), Arg.Any<ISoundPlaybackController>()).Returns(mockBackgroundLayer);
            _sut.Initialize();

            _sut.Reset();

            mockBackgroundLayer.Received(1).Reset();
        }

        [TestMethod]
        public void ResetShouldSetInputBlockerButtonEnabledToFalse()
        {
            IButton mockButton = Substitute.For<IButton>();
            _mockGameObjectFactory.CreateButtonBase(_mockEngine, Arg.Any<RectangleF>()).Returns(mockButton);
            _sut.Initialize();
            mockButton.ClearReceivedCalls();

            _sut.Reset();

            mockButton.Received(1).Enabled = false;
        }
        #endregion

        #region Start Tests
        [TestMethod]
        public void StartShouldStartPlayingMusic()
        {
            ISong mockSong = Substitute.For<ISong>();
            _mockEngine.AssetBank.Get<ISong>("MainMenu").Returns(mockSong);

            _sut.Start();

            Received.InOrder(
                () =>
                {
                    _mockEngine.AudioManager.MusicController.VolumeAttenuation = 0;
                    _mockEngine.AudioManager.MusicController.Play(mockSong);
                    _mockEngine.AudioManager.MusicController.FadeVolumeAttenuation(1f, CodeLogicEngine.Constants.FadeSceneTransitionTime, 0.25);
                }
            );
        }

        [TestMethod]
        public void StartShouldResetLayerFadeController()
        {
            _sut.Start();

            _mockFadeController.Received(1).Reset();
        }

        [TestMethod]
        public void StartShouldCallGameStatsDataLoadData()
        {
            _sut.Initialize();

            _sut.Start();

            _mockGameStatsData.Received(1).LoadData();
        }

        [TestMethod]
        public void StartOnSubsequentCallShouldNotcallGameStatsDataLoadData()
        {
            _sut.Initialize();
            _sut.Start();
            _mockGameStatsData.ClearReceivedCalls();

            _sut.Start();

            _mockGameStatsData.DidNotReceive().LoadData();
        }
        #endregion

        #region End Tests
        [TestMethod]
        public void EndShouldStopPlayingMusic()
        {
            _sut.Initialize();
            _sut.End();

            _mockEngine.AudioManager.Received(1).MusicController.FadeVolumeAttenuation(0, CodeLogicEngine.Constants.FadeSceneTransitionTimeHalf);
        }

        [TestMethod]
        public void EndShouldCallFinishOnBackgroundLayer()
        {
            IMenuBackgroundLayer mockBackgroundLayer = Substitute.For<IMenuBackgroundLayer>();
            _mockGameObjectFactory.CreateMenuBackgroundLayer(Arg.Any<IEngine>(), Arg.Any<ISpriteBatch>(), Arg.Any<ISoundPlaybackController>()).Returns(mockBackgroundLayer);
            _sut.Initialize();

            _sut.End();

            mockBackgroundLayer.Received(1).StopSounds();
        }
        #endregion

        #region Exit Dialog DialogClosed Event Tests
        [TestMethod]
        public void ExitDialogClosedWithOkShouldCallHostExit()
        {
            IDialog mockDialog = Substitute.For<IDialog>();
            _mockGameObjectFactory.CreateOkCancelDialog(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ITextObjectBuilder>(), Arg.Any<string>()).Returns(mockDialog, Substitute.For<IDialog>());
            _sut.Initialize();

            mockDialog.DialogClosed += Raise.Event<EventHandler<DialogClosedEventArgs>>(mockDialog, new DialogClosedEventArgs(DialogResult.Ok));

            _mockEngine.Received(1).Exit();
        }

        [TestMethod]
        public void ExitDialogClosedWithCancelShouldNotCallHostExit()
        {
            IDialog mockDialog = Substitute.For<IDialog>();
            _mockGameObjectFactory.CreateOkCancelDialog(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ITextObjectBuilder>(), Arg.Any<string>()).Returns(mockDialog, Substitute.For<IDialog>());
            _sut.Initialize();

            mockDialog.DialogClosed += Raise.Event<EventHandler<DialogClosedEventArgs>>(mockDialog, new DialogClosedEventArgs(DialogResult.Cancel));

            _mockEngine.Received(0).Exit();
        }
        #endregion

        #region Reset Stats DialogClosed Event Tests
        [TestMethod]
        public void ResetStatsDialogClosedWithOkShouldMakeExpectedCalls()
        {
            IDialog mockDialog = Substitute.For<IDialog>();
            _mockGameObjectFactory.CreateOkCancelDialog(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ITextObjectBuilder>(), Arg.Any<string>()).Returns(Substitute.For<IDialog>(), mockDialog);
            _sut.Initialize();

            mockDialog.DialogClosed += Raise.Event<EventHandler<DialogClosedEventArgs>>(mockDialog, new DialogClosedEventArgs(DialogResult.Ok));

            Received.InOrder(
                () =>
                {
                    _mockGameStatsData.Model.Reset();
                    _mockGameStatsData.SaveData();
                }
            );
        }

        [TestMethod]
        public void ResetStatsDialogClosedWithCancelShouldNotCallGameStatsDataModelReset()
        {
            IDialog mockDialog = Substitute.For<IDialog>();
            _mockGameObjectFactory.CreateOkCancelDialog(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ITextObjectBuilder>(), Arg.Any<string>()).Returns(Substitute.For<IDialog>(), mockDialog);
            _sut.Initialize();

            mockDialog.DialogClosed += Raise.Event<EventHandler<DialogClosedEventArgs>>(mockDialog, new DialogClosedEventArgs(DialogResult.Cancel));

            _mockGameStatsData.Model.DidNotReceive().Reset();
        }

        [TestMethod]
        public void ResetStatsDialogClosedWithCancelShouldNotCallGameStatsDataSaveData()
        {
            IDialog mockDialog = Substitute.For<IDialog>();
            _mockGameObjectFactory.CreateOkCancelDialog(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ITextObjectBuilder>(), Arg.Any<string>()).Returns(Substitute.For<IDialog>(), mockDialog);
            _sut.Initialize();

            mockDialog.DialogClosed += Raise.Event<EventHandler<DialogClosedEventArgs>>(mockDialog, new DialogClosedEventArgs(DialogResult.Cancel));

            _mockGameStatsData.DidNotReceive().SaveData();
        }
        #endregion

        #region ExternalActionOccurred Event Tests
        [TestMethod]
        public void ExternalActionOccurredWithClosedDialogShouldCallDialogShow()
        {
            _mockEngine.CurrentScene.Returns(_sut);
            IDialog mockDialog = Substitute.For<IDialog>();
            mockDialog.IsOpen.Returns(false);
            _mockGameObjectFactory.CreateOkCancelDialog(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ITextObjectBuilder>(), Arg.Any<string>()).Returns(mockDialog);
            _sut.Initialize();

            _mockEngine.ExternalActionOccurred += Raise.Event<EventHandler<ExternalActionEventArgs>>(_mockEngine, new ExternalActionEventArgs(CodeLogicEngine.Constants.ExternalActions.BackPressed));

            mockDialog.Received(1).Show();
        }

        [TestMethod]
        public void ExternalActionOccurredWithDialogOpenShouldCallDialogClose()
        {
            _mockEngine.CurrentScene.Returns(_sut);
            IDialog mockDialog = Substitute.For<IDialog>();
            mockDialog.IsOpen.Returns(true);
            _mockGameObjectFactory.CreateOkCancelDialog(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ITextObjectBuilder>(), Arg.Any<string>()).Returns(mockDialog);
            _sut.Initialize();

            _mockEngine.ExternalActionOccurred += Raise.Event<EventHandler<ExternalActionEventArgs>>(_mockEngine, new ExternalActionEventArgs(CodeLogicEngine.Constants.ExternalActions.BackPressed));

            mockDialog.Received(1).Close();
        }

        [TestMethod]
        public void ExternalActionOccurredWithoutBackPressedShouldNotOpenOrCloseDialog()
        {
            _mockEngine.CurrentScene.Returns(_sut);
            IDialog mockDialog = Substitute.For<IDialog>();
            mockDialog.IsOpen.Returns(true);
            _mockGameObjectFactory.CreateOkCancelDialog(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ITextObjectBuilder>(), Arg.Any<string>()).Returns(mockDialog);
            _sut.Initialize();

            _mockEngine.ExternalActionOccurred += Raise.Event<EventHandler<ExternalActionEventArgs>>(_mockEngine, new ExternalActionEventArgs("NOT BACK PRESSED"));

            mockDialog.Received(0).Show();
            mockDialog.Received(0).Close();
        }

        [TestMethod]
        public void ExternalActionOccurredWhenNotCurrentSceneShouldNotOpenOrCloseDialog()
        {
            _mockEngine.CurrentScene.Returns(Substitute.For<IScene>());
            IDialog mockDialog = Substitute.For<IDialog>();
            mockDialog.IsOpen.Returns(true);
            _mockGameObjectFactory.CreateOkCancelDialog(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ITextObjectBuilder>(), Arg.Any<string>()).Returns(mockDialog);
            _sut.Initialize();

            _mockEngine.ExternalActionOccurred += Raise.Event<EventHandler<ExternalActionEventArgs>>(_mockEngine, new ExternalActionEventArgs(CodeLogicEngine.Constants.ExternalActions.BackPressed));

            mockDialog.Received(0).Show();
            mockDialog.Received(0).Close();
        }
        #endregion
    }
}
