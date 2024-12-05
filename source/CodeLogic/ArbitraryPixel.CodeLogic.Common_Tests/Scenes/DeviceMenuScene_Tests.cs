using ArbitraryPixel.CodeLogic.Common;
using ArbitraryPixel.CodeLogic.Common.Entities;
using ArbitraryPixel.CodeLogic.Common.Layers;
using ArbitraryPixel.CodeLogic.Common.Model;
using ArbitraryPixel.CodeLogic.Common.Scenes;
using ArbitraryPixel.CodeLogic.Common.Theme;
using ArbitraryPixel.CodeLogic.Common.Theme.Device;
using ArbitraryPixel.CodeLogic.Common.UI;
using ArbitraryPixel.Common.Audio;
using ArbitraryPixel.Common.Drawing;
using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Platform2D.Engine;
using ArbitraryPixel.Platform2D.Layer;
using ArbitraryPixel.Platform2D.Scene;
using ArbitraryPixel.Platform2D.Text;
using ArbitraryPixel.Platform2D.Theme;
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
    public class DeviceMenuScene_Tests
    {
        private DeviceMenuScene _sut;
        private GameObjectFactory _mockGameObjectFactory;
        private IEngine _mockEngine;
        private IDeviceModel _mockDeviceModel;
        private ISpriteBatch _mockBackgroundSpriteBatch;
        private ISpriteBatch _mockUILayerSpriteBatch;
        private ISpriteBatch _mockOverlayLayerSpriteBatch;

        private ILayer _mockBGLayer;
        private IDeviceMenuUILayer _mockUILayer;
        private ILayer _mockOverlayLayer;

        private SizeF _screenSize = new SizeF(1000, 500);
        private RectangleF _abortDialogBounds = new RectangleF(125, 150, 750, 200);

        [TestInitialize]
        public void Initialize()
        {
            _mockEngine = Substitute.For<IEngine>();
            _mockDeviceModel = Substitute.For<IDeviceModel>();

            _mockGameObjectFactory = Substitute.For<GameObjectFactory>();
            GameObjectFactory.SetInstance(_mockGameObjectFactory);

            _mockEngine.GrfxFactory.SpriteBatchFactory.Create(Arg.Any<IGrfxDevice>()).Returns(
                _mockBackgroundSpriteBatch = Substitute.For<ISpriteBatch>(),
                _mockUILayerSpriteBatch = Substitute.For<ISpriteBatch>(),
                _mockOverlayLayerSpriteBatch = Substitute.For<ISpriteBatch>()
            );

            _mockEngine.ScreenManager.World.Returns((Point)_screenSize);

            _mockGameObjectFactory.CreateGenericLayer(Arg.Any<IEngine>(), Arg.Any<ISpriteBatch>(), Arg.Any<SpriteSortMode>(), Arg.Any<BlendState>(), Arg.Any<SamplerState>()).Returns(_mockBGLayer = Substitute.For<ILayer>());
            _mockGameObjectFactory.CreateDeviceMenuUILayer(Arg.Any<IEngine>(), Arg.Any<ISpriteBatch>()).Returns(_mockUILayer = Substitute.For<IDeviceMenuUILayer>());
            _mockGameObjectFactory.CreateGenericLayer(_mockEngine, _mockOverlayLayerSpriteBatch).Returns(_mockOverlayLayer = Substitute.For<ILayer>());
            _mockOverlayLayer.MainSpriteBatch.Returns(_mockOverlayLayerSpriteBatch);

            _sut = new DeviceMenuScene(_mockEngine, _mockDeviceModel);
        }

        #region Constructor Tests
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullModelShouldThrowException()
        {
            _sut = new DeviceMenuScene(_mockEngine, null);
        }

        [TestMethod]
        public void ConstructShouldAttachToHostExternalActionOccurredEvent()
        {
            _mockEngine.Received(1).ExternalActionOccurred += Arg.Any<EventHandler<ExternalActionEventArgs>>();
        }
        #endregion

        #region Reset Tests
        [TestMethod]
        public void ResetShouldSetSceneCompleteToFalse()
        {
            _sut.Initialize();
            _sut.SceneComplete = true;
            _sut.Reset();
            Assert.IsFalse(_sut.SceneComplete);
        }
        #endregion

        #region Initialize Test
        [TestMethod]
        public void InitializeShouldCreateExpectedNumberOfSpriteBatchObjects()
        {
            int expected = 3;

            _sut.Initialize();

            _mockEngine.GrfxFactory.SpriteBatchFactory.Received(expected).Create(Arg.Any<IGrfxDevice>());
        }

        [TestMethod]
        public void InitializeShouldCreateTextBuilder()
        {
            _sut.Initialize();

            Received.InOrder(
                () =>
                {
                    _mockGameObjectFactory.CreateTextFormatValueHandlerManager();
                    _mockGameObjectFactory.CreateTextFormatProcessor(Arg.Any<ITextFormatValueHandlerManager>());
                    _mockGameObjectFactory.CreateTextObjectFactory();
                    _mockGameObjectFactory.CreateTextObjectBuilderWithConsoleFonts(Arg.Any<ITextFormatProcessor>(), Arg.Any<ITextObjectFactory>(), _mockEngine.AssetBank);
                }
            );
        }

        #region Device Background
        [TestMethod]
        public void InitializeShouldCreateGenericLayerForBackground()
        {
            _sut.Initialize();

            _mockGameObjectFactory.Received(1).CreateGenericLayer(_mockEngine, _mockBackgroundSpriteBatch, SpriteSortMode.Deferred, null, SamplerState.PointClamp);
        }

        [TestMethod]
        public void InitializeShouldCreateDeviceBackground()
        {
            _mockEngine.ScreenManager.World.Returns(new Point(100, 100));
            _mockBGLayer.MainSpriteBatch.Returns(_mockBackgroundSpriteBatch);

            _sut.Initialize();

            _mockGameObjectFactory.Received(1).CreateDeviceBackground(_mockEngine, new RectangleF(0, 0, 100, 100), _mockBackgroundSpriteBatch);
        }

        [TestMethod]
        public void IniitalizeShouldAddDeviceBackgroundToBackgroundLayer()
        {
            IDeviceBackground mockDeviceBackground = Substitute.For<IDeviceBackground>();
            _mockGameObjectFactory.CreateDeviceBackground(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>()).Returns(mockDeviceBackground);

            _sut.Initialize();

            _mockBGLayer.Received(1).AddEntity(mockDeviceBackground);
        }

        [TestMethod]
        public void InitializeShouldSetDeviceBackgroundColourToThemeBackgroundImageMask()
        {
            IThemeManagerCollection mockThemeCollection = Substitute.For<IThemeManagerCollection>();
            _mockEngine.GetComponent<IThemeManagerCollection>().Returns(mockThemeCollection);

            mockThemeCollection[ThemeObjectType.Device].GetCurrentTheme<IDeviceTheme>().BackgroundImageMask.Returns(Color.Pink);
            IDeviceBackground mockDeviceBackground = Substitute.For<IDeviceBackground>();
            _mockGameObjectFactory.CreateDeviceBackground(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>()).Returns(mockDeviceBackground);

            _sut.Initialize();

            mockDeviceBackground.Received(1).Colour = Color.Pink;
        }

        [TestMethod]
        public void InitializeShouldAddDeviceBackgroundToEntities()
        {
            _sut.Initialize();

            Assert.IsTrue(_sut.Entities.Contains(_mockBGLayer));
        }
        #endregion

        #region UI Layer
        [TestMethod]
        public void InitializeShouldCreateDeviceMenuUILayer()
        {
            _sut.Initialize();

            _mockGameObjectFactory.Received(1).CreateDeviceMenuUILayer(_mockEngine, _mockUILayerSpriteBatch);
        }

        [TestMethod]
        public void InitializeShouldSubscribeToDeviceMenuUILayerReturnButtonTappedEvent()
        {
            _sut.Initialize();

            _mockUILayer.Received(1).ReturnButtonTapped += Arg.Any<EventHandler<EventArgs>>();
        }

        [TestMethod]
        public void InitializeShouldSubscribeToDeviceMenuUILayerAbortButtonTappedEvent()
        {
            _sut.Initialize();

            _mockUILayer.Received(1).AbortButtonTapped += Arg.Any<EventHandler<EventArgs>>();
        }

        [TestMethod]
        public void InitializeShouldSubscribeToDeviceMenuUILayerSettingsButtonTappedEvent()
        {
            _sut.Initialize();

            _mockUILayer.Received(1).SettingsButtonTapped += Arg.Any<EventHandler<EventArgs>>();
        }

        [TestMethod]
        public void InitializeShouldAddUILayerToEntities()
        {
            _sut.Initialize();

            Assert.IsTrue(_sut.Entities.Contains(_mockUILayer));
        }
        #endregion

        #region Abort Dialog
        [TestMethod]
        public void InitializeShouldCreateAbortDialog()
        {
            _mockEngine.ScreenManager.World.Returns(new Point(1000, 700));
            RectangleF expectedBounds = new RectangleF(125, 250, 750, 200);

            IDialog mockDialog = Substitute.For<IDialog>();
            _mockGameObjectFactory.CreateOkCancelDialog(_mockEngine, expectedBounds, Arg.Any<ITextObjectBuilder>(), Arg.Any<string>()).Returns(mockDialog, Substitute.For<IDialog>());

            _sut.Initialize();

            Received.InOrder(
                () =>
                {
                    _mockGameObjectFactory.CreateOkCancelDialog(_mockEngine, expectedBounds, Arg.Any<ITextObjectBuilder>(), Arg.Any<string>());
                    mockDialog.DialogClosed += Arg.Any<EventHandler<DialogClosedEventArgs>>();
                }
            );
        }

        [TestMethod]
        public void InitializeShouldAddAbortDialogToEntities()
        {
            _mockEngine.ScreenManager.World.Returns(new Point(1000, 700));
            RectangleF expectedBounds = new RectangleF(125, 250, 750, 200);

            IDialog mockDialog = Substitute.For<IDialog>();
            _mockGameObjectFactory.CreateOkCancelDialog(_mockEngine, expectedBounds, Arg.Any<ITextObjectBuilder>(), Arg.Any<string>()).Returns(mockDialog, Substitute.For<IDialog>());

            _sut.Initialize();

            Assert.IsTrue(_sut.Entities.Contains(mockDialog));
        }

        [TestMethod]
        public void InitializeShouldSetAbortDialogBorderColour()
        {
            IDeviceTheme mockTheme = Substitute.For<IDeviceTheme>();
            mockTheme.NormalColourMask.Returns(Color.Pink);
            IThemeManagerCollection mockThemeCollection = Substitute.For<IThemeManagerCollection>();
            mockThemeCollection[ThemeObjectType.Device].GetCurrentTheme<IDeviceTheme>().Returns(mockTheme);
            _mockEngine.GetComponent<IThemeManagerCollection>().Returns(mockThemeCollection);

            IDialog mockDialog = Substitute.For<IDialog>();
            _mockGameObjectFactory.CreateOkCancelDialog(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ITextObjectBuilder>(), Arg.Any<string>()).Returns(mockDialog, Substitute.For<IDialog>());

            _sut.Initialize();

            mockDialog.Received(1).BorderColour = Color.Pink;
        }

        [TestMethod]
        public void InitializeShouldSetAbortDialogBackgroundColour()
        {
            IDeviceTheme mockTheme = Substitute.For<IDeviceTheme>();
            mockTheme.BackgroundColourMask.Returns(Color.Pink);
            IThemeManagerCollection mockThemeCollection = Substitute.For<IThemeManagerCollection>();
            mockThemeCollection[ThemeObjectType.Device].GetCurrentTheme<IDeviceTheme>().Returns(mockTheme);
            _mockEngine.GetComponent<IThemeManagerCollection>().Returns(mockThemeCollection);

            IDialog mockDialog = Substitute.For<IDialog>();
            _mockGameObjectFactory.CreateOkCancelDialog(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ITextObjectBuilder>(), Arg.Any<string>()).Returns(mockDialog, Substitute.For<IDialog>());

            _sut.Initialize();

            mockDialog.Received(1).BackgroundColour = Color.Pink;
        }
        #endregion

        #region Settings Dialog
        [TestMethod]
        public void InitializeShouldCreateSettingsDialog()
        {
            _mockEngine.ScreenManager.World.Returns(new Point(1000, 800));
            RectangleF expectedBounds = new RectangleF(125, 100, 750, 600);

            IDialog mockDialog = Substitute.For<IDialog>();
            _mockGameObjectFactory.CreateDialog(_mockEngine, expectedBounds, Arg.Any<ITextObjectBuilder>(), Arg.Any<string>()).Returns(mockDialog);

            _sut.Initialize();

            Received.InOrder(
                () =>
                {
                    _mockGameObjectFactory.CreateDialog(_mockEngine, expectedBounds, Arg.Any<ITextObjectBuilder>(), Arg.Any<string>());
                }
            );
        }

        [TestMethod]
        public void InitializeShouldAddSettingsDialogToEntities()
        {
            _mockEngine.ScreenManager.World.Returns(new Point(1000, 800));
            RectangleF expectedBounds = new RectangleF(125, 100, 750, 600);

            IDialog mockDialog = Substitute.For<IDialog>();
            _mockGameObjectFactory.CreateDialog(_mockEngine, expectedBounds, Arg.Any<ITextObjectBuilder>(), Arg.Any<string>()).Returns(mockDialog);

            _sut.Initialize();

            Assert.IsTrue(_sut.Entities.Contains(mockDialog));
        }

        [TestMethod]
        public void InitializeShouldSetSettingsDialogBackgroundColour()
        {
            IDeviceTheme mockTheme = Substitute.For<IDeviceTheme>();
            mockTheme.BackgroundColourMask.Returns(Color.Pink);
            IThemeManagerCollection mockThemeCollection = Substitute.For<IThemeManagerCollection>();
            mockThemeCollection[ThemeObjectType.Device].GetCurrentTheme<IDeviceTheme>().Returns(mockTheme);
            _mockEngine.GetComponent<IThemeManagerCollection>().Returns(mockThemeCollection);

            _mockEngine.ScreenManager.World.Returns(new Point(1000, 800));
            RectangleF expectedBounds = new RectangleF(125, 100, 750, 600);

            IDialog mockDialog = Substitute.For<IDialog>();
            _mockGameObjectFactory.CreateDialog(_mockEngine, expectedBounds, Arg.Any<ITextObjectBuilder>(), Arg.Any<string>()).Returns(mockDialog);

            _sut.Initialize();

            mockDialog.Received(1).BackgroundColour = Color.Pink;
        }

        [TestMethod]
        public void InitializeShouldSetSettingsDialogBorderColour()
        {
            IDeviceTheme mockTheme = Substitute.For<IDeviceTheme>();
            mockTheme.NormalColourMask.Returns(Color.Pink);
            IThemeManagerCollection mockThemeCollection = Substitute.For<IThemeManagerCollection>();
            mockThemeCollection[ThemeObjectType.Device].GetCurrentTheme<IDeviceTheme>().Returns(mockTheme);
            _mockEngine.GetComponent<IThemeManagerCollection>().Returns(mockThemeCollection);

            _mockEngine.ScreenManager.World.Returns(new Point(1000, 800));
            RectangleF expectedBounds = new RectangleF(125, 100, 750, 600);

            IDialog mockDialog = Substitute.For<IDialog>();
            _mockGameObjectFactory.CreateDialog(_mockEngine, expectedBounds, Arg.Any<ITextObjectBuilder>(), Arg.Any<string>()).Returns(mockDialog);

            _sut.Initialize();

            mockDialog.Received(1).BorderColour = Color.Pink;
        }

        [TestMethod]
        public void InitializeShouldCreateMainMenuContentLayerFactory()
        {
            _sut.Initialize();

            _mockGameObjectFactory.Received(1).CreateMainMenuContentLayerFactory();
        }

        [TestMethod]
        public void InitializeShouldCreateMenuSettingsContentLayer()
        {
            IMainMenuContentLayerFactory mockContentFactory = Substitute.For<IMainMenuContentLayerFactory>();
            _mockGameObjectFactory.CreateMainMenuContentLayerFactory().Returns(mockContentFactory);

            ISpriteBatch mockSpriteBatch = Substitute.For<ISpriteBatch>();
            _mockEngine.GrfxFactory.SpriteBatchFactory.Create(Arg.Any<IGrfxDevice>()).Returns(Substitute.For<ISpriteBatch>(), Substitute.For<ISpriteBatch>(), mockSpriteBatch);

            IDialog mockDialog = Substitute.For<IDialog>();
            mockDialog.ClientRectangle.Returns(new RectangleF(200, 100, 400, 300));
            _mockGameObjectFactory.CreateDialog(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ITextObjectBuilder>(), Arg.Any<string>()).Returns(mockDialog);

            _sut.Initialize();

            mockContentFactory.Received(1).CreateMenuSettingsContentLayer(_mockEngine, mockSpriteBatch, new RectangleF(200, 100, 400, 300));
        }

        [TestMethod]
        public void InitializeShouldAddSettingsContentLayerToDialog()
        {
            IMainMenuContentLayerFactory mockContentFactory = Substitute.For<IMainMenuContentLayerFactory>();
            _mockGameObjectFactory.CreateMainMenuContentLayerFactory().Returns(mockContentFactory);

            IMenuSettingsContentLayer mockSettingsContent = Substitute.For<IMenuSettingsContentLayer>();
            mockContentFactory.CreateMenuSettingsContentLayer(Arg.Any<IEngine>(), Arg.Any<ISpriteBatch>(), Arg.Any<RectangleF>()).Returns(mockSettingsContent);

            IDialog mockDialog = Substitute.For<IDialog>();
            mockDialog.ClientRectangle.Returns(new RectangleF(200, 100, 400, 300));
            _mockGameObjectFactory.CreateDialog(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ITextObjectBuilder>(), Arg.Any<string>()).Returns(mockDialog);

            _sut.Initialize();

            mockDialog.Received(1).AddContentLayer(mockSettingsContent);
        }
        #endregion
        #endregion

        #region Return Button Tapped Tests
        [TestMethod]
        public void ReturnButtonTappedShouldSetNextSceneToDeviceMain()
        {
            IScene mockTransitionScene = Substitute.For<IScene>();
            _mockGameObjectFactory.CreatePanSceneTransition(Arg.Any<IEngine>(), Arg.Any<IScene>(), Arg.Any<IScene>(), Arg.Any<PanSceneTransitionMode>(), Arg.Any<double>()).Returns(mockTransitionScene);

            IScene mockDeviceMain = Substitute.For<IScene>();
            _mockEngine.Scenes.Returns(new Dictionary<string, IScene>());
            _mockEngine.Scenes.Add("DeviceMain", mockDeviceMain);

            _sut.Initialize();
            _mockUILayer.ReturnButtonTapped += Raise.Event();

            Assert.AreSame(mockTransitionScene, _sut.NextScene);
        }

        [TestMethod]
        public void ReturnButtonTappedShouldSetSceneCompleteTrue()
        {
            IScene mockTransitionScene = Substitute.For<IScene>();
            _mockGameObjectFactory.CreatePanSceneTransition(Arg.Any<IEngine>(), Arg.Any<IScene>(), Arg.Any<IScene>(), Arg.Any<PanSceneTransitionMode>(), Arg.Any<double>()).Returns(mockTransitionScene);

            IScene mockDeviceMain = Substitute.For<IScene>();
            _mockEngine.Scenes.Returns(new Dictionary<string, IScene>());
            _mockEngine.Scenes.Add("DeviceMain", mockDeviceMain);

            _sut.Initialize();
            _mockUILayer.ReturnButtonTapped += Raise.Event();

            Assert.IsTrue(_sut.SceneComplete);
        }

        [TestMethod]
        public void ReturnButtonTappedShouldCreateHorizontalPanTransitionScene()
        {
            IScene mockTransitionScene = Substitute.For<IScene>();
            _mockGameObjectFactory.CreatePanSceneTransition(Arg.Any<IEngine>(), Arg.Any<IScene>(), Arg.Any<IScene>(), Arg.Any<PanSceneTransitionMode>(), Arg.Any<double>()).Returns(mockTransitionScene);

            IScene mockDeviceMain = Substitute.For<IScene>();
            _mockEngine.Scenes.Returns(new Dictionary<string, IScene>());
            _mockEngine.Scenes.Add("DeviceMain", mockDeviceMain);

            _sut.Initialize();
            _mockUILayer.ReturnButtonTapped += Raise.Event();

            _mockGameObjectFactory.Received(1).CreatePanSceneTransition(_mockEngine, _sut, mockDeviceMain, PanSceneTransitionMode.PanLeft, 0.125);
        }

        [TestMethod]
        public void ReturnButtonTappedShouldPlaySound()
        {
            IScene mockTransitionScene = Substitute.For<IScene>();
            _mockGameObjectFactory.CreatePanSceneTransition(Arg.Any<IEngine>(), Arg.Any<IScene>(), Arg.Any<IScene>(), Arg.Any<PanSceneTransitionMode>(), Arg.Any<double>()).Returns(mockTransitionScene);

            IScene mockDeviceMain = Substitute.For<IScene>();
            _mockEngine.Scenes.Returns(new Dictionary<string, IScene>());
            _mockEngine.Scenes.Add("DeviceMain", mockDeviceMain);

            ISoundResource mockSound = Substitute.For<ISoundResource>();
            _mockEngine.AssetBank.Get<ISoundResource>("ButtonPress").Returns(mockSound);

            _sut.Initialize();

            _mockUILayer.ReturnButtonTapped += Raise.Event();

            mockSound.Received(1).Play();
        }
        #endregion

        #region Abort Button Tapped Tests
        [TestMethod]
        public void AbortButtonTappedShouldShowDialog()
        {
            IDialog mockDialog = Substitute.For<IDialog>();
            _mockGameObjectFactory.CreateOkCancelDialog(_mockEngine, Arg.Any<RectangleF>(), Arg.Any<ITextObjectBuilder>(), Arg.Any<string>()).Returns(mockDialog, Substitute.For<IDialog>());
            _sut.Initialize();

            _mockUILayer.AbortButtonTapped += Raise.Event<EventHandler<EventArgs>>(_mockUILayer, new EventArgs());

            mockDialog.Received(1).Show();
        }

        [TestMethod]
        public void AbortButtonTappedShouldPlaySound()
        {
            IDialog mockDialog = Substitute.For<IDialog>();
            _mockGameObjectFactory.CreateOkCancelDialog(_mockEngine, Arg.Any<RectangleF>(), Arg.Any<ITextObjectBuilder>(), Arg.Any<string>()).Returns(mockDialog);

            ISoundResource mockSound = Substitute.For<ISoundResource>();
            _mockEngine.AssetBank.Get<ISoundResource>("ButtonPress").Returns(mockSound);

            _sut.Initialize();

            _mockUILayer.AbortButtonTapped += Raise.Event<EventHandler<EventArgs>>(_mockUILayer, new EventArgs());

            mockSound.Received(1).Play();
        }
        #endregion

        #region Abort Dialog Closed Tests
        [TestMethod]
        public void AbortDialogClosedWithDialogResultOkShouldChangeScene()
        {
            IDialog mockDialog = Substitute.For<IDialog>();
            _mockGameObjectFactory.CreateOkCancelDialog(_mockEngine, Arg.Any<RectangleF>(), Arg.Any<ITextObjectBuilder>(), Arg.Any<string>()).Returns(mockDialog, Substitute.For<IDialog>());
            _sut.Initialize();

            Dictionary<string, IScene> scenes = new Dictionary<string, IScene>();
            scenes.Add("MissionDebriefing", Substitute.For<IScene>());
            _mockEngine.Scenes.Returns(scenes);

            IScene mockTransitionScene = Substitute.For<IScene>();
            _mockGameObjectFactory.CreateFadeSceneTransition(_mockEngine, _sut, scenes["MissionDebriefing"], FadeSceneTransitionMode.OutIn, CodeLogicEngine.Constants.FadeSceneTransitionTime).Returns(mockTransitionScene);

            mockDialog.DialogClosed += Raise.Event<EventHandler<DialogClosedEventArgs>>(mockDialog, new DialogClosedEventArgs(DialogResult.Ok));

            Assert.AreSame(mockTransitionScene, _sut.NextScene);
            Assert.IsTrue(_sut.SceneComplete);
        }

        [TestMethod]
        public void AbortDialogClosedWithDialogResultCancelShouldNotChangeScene()
        {
            IDialog mockDialog = Substitute.For<IDialog>();
            _mockGameObjectFactory.CreateOkCancelDialog(_mockEngine, Arg.Any<RectangleF>(), Arg.Any<ITextObjectBuilder>(), Arg.Any<string>()).Returns(mockDialog, Substitute.For<IDialog>());
            _sut.Initialize();

            Dictionary<string, IScene> scenes = new Dictionary<string, IScene>();
            scenes.Add("MissionDebriefing", Substitute.For<IScene>());
            _mockEngine.Scenes.Returns(scenes);

            IScene mockTransitionScene = Substitute.For<IScene>();
            _mockGameObjectFactory.CreateFadeSceneTransition(_mockEngine, _sut, scenes["MissionDebriefing"], FadeSceneTransitionMode.OutIn, CodeLogicEngine.Constants.FadeSceneTransitionTime).Returns(mockTransitionScene);

            mockDialog.DialogClosed += Raise.Event<EventHandler<DialogClosedEventArgs>>(mockDialog, new DialogClosedEventArgs(DialogResult.Cancel));

            Assert.IsNull(_sut.NextScene);
            Assert.IsFalse(_sut.SceneComplete);
        }
        #endregion

        #region Settings Button Tapped Tests
        [TestMethod]
        public void SettingsButtonTappedShouldShowDialog()
        {
            IDialog mockDialog = Substitute.For<IDialog>();
            _mockGameObjectFactory.CreateDialog(_mockEngine, Arg.Any<RectangleF>(), Arg.Any<ITextObjectBuilder>(), Arg.Any<string>()).Returns(mockDialog);

            _sut.Initialize();

            _mockUILayer.SettingsButtonTapped += Raise.Event<EventHandler<EventArgs>>(_mockUILayer, new EventArgs());

            mockDialog.Received(1).Show();
        }

        [TestMethod]
        public void SettingsButtonTappedShouldPlaySound()
        {
            ISoundResource mockSound = Substitute.For<ISoundResource>();
            _mockEngine.AssetBank.Get<ISoundResource>("ButtonPress").Returns(mockSound);

            _sut.Initialize();

            _mockUILayer.SettingsButtonTapped += Raise.Event<EventHandler<EventArgs>>(_mockUILayer, new EventArgs());

            mockSound.Received(1).Play();
        }

        [TestMethod]
        public void SettingsButtonTappedShouldCallShowOnSettingsContentLayer()
        {
            IMainMenuContentLayerFactory contentFactory = Substitute.For<IMainMenuContentLayerFactory>();
            _mockGameObjectFactory.CreateMainMenuContentLayerFactory().Returns(contentFactory);

            IMenuSettingsContentLayer mockContent = Substitute.For<IMenuSettingsContentLayer>();
            contentFactory.CreateMenuSettingsContentLayer(Arg.Any<IEngine>(), Arg.Any<ISpriteBatch>(), Arg.Any<RectangleF>()).Returns(mockContent);

            _sut.Initialize();

            _mockUILayer.SettingsButtonTapped += Raise.Event<EventHandler<EventArgs>>(_mockUILayer, new EventArgs());

            mockContent.Received(1).Show();
        }
        #endregion

        #region End Tests
        [TestMethod]
        public void EndAfterAbortShouldStopMusicPlayback()
        {
            Dictionary<string, IScene> scenes = new Dictionary<string, IScene>();
            scenes.Add("MissionDebriefing", Substitute.For<IScene>());
            _mockEngine.Scenes.Returns(scenes);

            IDialog mockDialog = Substitute.For<IDialog>();
            _mockGameObjectFactory.CreateOkCancelDialog(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ITextObjectBuilder>(), Arg.Any<string>()).Returns(mockDialog);
            _sut.Initialize();
            mockDialog.DialogClosed += Raise.Event<EventHandler<DialogClosedEventArgs>>(mockDialog, new DialogClosedEventArgs(DialogResult.Ok));

            _sut.End();

            _mockEngine.AudioManager.Received(1).MusicController.FadeVolumeAttenuation(0, CodeLogicEngine.Constants.FadeSceneTransitionTime);
        }

        [TestMethod]
        public void EndNormallyShouldNotStopMusicPlayback()
        {
            _sut.End();

            _mockEngine.AudioManager.Received(0).MusicController.FadeVolumeAttenuation(Arg.Any<float>(), Arg.Any<double>());
        }
        #endregion

        #region TriggerExternalActionTests
        [TestMethod]
        public void ExternalActionOccurredWithClosedDialogShouldCallDialogShow_BackPressed()
        {
            IDialog mockDialog = Substitute.For<IDialog>();
            mockDialog.IsOpen.Returns(false);
            _mockGameObjectFactory.CreateOkCancelDialog(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ITextObjectBuilder>(), Arg.Any<string>()).Returns(mockDialog);

            _mockEngine.CurrentScene.Returns(_sut);

            _sut.Initialize();

            _mockEngine.ExternalActionOccurred += Raise.Event<EventHandler<ExternalActionEventArgs>>(_mockEngine, new ExternalActionEventArgs(CodeLogicEngine.Constants.ExternalActions.BackPressed));

            mockDialog.Received(1).Show();
        }

        [TestMethod]
        public void ExternalActionOccurredWithOpenDialogShouldCallDialogClose_BackPressed()
        {
            IDialog mockDialog = Substitute.For<IDialog>();
            mockDialog.IsOpen.Returns(true);
            _mockGameObjectFactory.CreateOkCancelDialog(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ITextObjectBuilder>(), Arg.Any<string>()).Returns(mockDialog);

            _mockEngine.CurrentScene.Returns(_sut);

            _sut.Initialize();

            _mockEngine.ExternalActionOccurred += Raise.Event<EventHandler<ExternalActionEventArgs>>(_mockEngine, new ExternalActionEventArgs(CodeLogicEngine.Constants.ExternalActions.BackPressed));

            mockDialog.Received(1).Close();
        }

        [TestMethod]
        public void ExternalActionOccurredWithDifferentCurrentSceneShouldNotOpenOrCloseDialog()
        {
            IDialog mockDialog = Substitute.For<IDialog>();
            mockDialog.IsOpen.Returns(true);
            _mockGameObjectFactory.CreateOkCancelDialog(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ITextObjectBuilder>(), Arg.Any<string>()).Returns(mockDialog);

            _mockEngine.CurrentScene.Returns(Substitute.For<IScene>());

            _sut.Initialize();

            _mockEngine.ExternalActionOccurred += Raise.Event<EventHandler<ExternalActionEventArgs>>(_mockEngine, new ExternalActionEventArgs(CodeLogicEngine.Constants.ExternalActions.BackPressed));

            mockDialog.Received(0).Show();
            mockDialog.Received(0).Close();
        }

        [TestMethod]
        public void ExternalActionOccurredWithoutBackPressedDataShouldNotOpenOrCloseDialog()
        {
            IDialog mockDialog = Substitute.For<IDialog>();
            mockDialog.IsOpen.Returns(true);
            _mockGameObjectFactory.CreateOkCancelDialog(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ITextObjectBuilder>(), Arg.Any<string>()).Returns(mockDialog);

            _mockEngine.CurrentScene.Returns(_sut);

            _sut.Initialize();

            _mockEngine.ExternalActionOccurred += Raise.Event<EventHandler<ExternalActionEventArgs>>(_mockEngine, new ExternalActionEventArgs("NOT_BACK_PRESSED"));

            mockDialog.Received(0).Show();
            mockDialog.Received(0).Close();
        }

        [TestMethod]
        public void ExternalActionOccurredWithNullDataShouldNotOpenOrCloseDialog()
        {
            IDialog mockDialog = Substitute.For<IDialog>();
            mockDialog.IsOpen.Returns(true);
            _mockGameObjectFactory.CreateOkCancelDialog(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ITextObjectBuilder>(), Arg.Any<string>()).Returns(mockDialog);

            _mockEngine.CurrentScene.Returns(_sut);

            _sut.Initialize();

            _mockEngine.ExternalActionOccurred += Raise.Event<EventHandler<ExternalActionEventArgs>>(_mockEngine, new ExternalActionEventArgs(null));

            mockDialog.Received(0).Show();
            mockDialog.Received(0).Close();
        }
        #endregion
    }
}
