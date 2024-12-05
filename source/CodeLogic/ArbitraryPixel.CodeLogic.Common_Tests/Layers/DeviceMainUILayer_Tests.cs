using ArbitraryPixel.CodeLogic.Common;
using ArbitraryPixel.CodeLogic.Common.Entities;
using ArbitraryPixel.CodeLogic.Common.Layers;
using ArbitraryPixel.CodeLogic.Common.Model;
using ArbitraryPixel.CodeLogic.Common.Theme;
using ArbitraryPixel.CodeLogic.Common.Theme.Device;
using ArbitraryPixel.Common.Audio;
using ArbitraryPixel.Common.Drawing;
using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Platform2D.Engine;
using ArbitraryPixel.Platform2D.Theme;
using ArbitraryPixel.Platform2D.UI;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ArbitraryPixel.CodeLogic.Common_Tests.Layers
{
    [TestClass]
    public class DeviceMainUILayer_Tests : UnitTestBase<DeviceMainUILayer>
    {
        private IEngine _mockEngine;
        private ISpriteBatch _mockSpriteBatch;
        private GameObjectFactory _mockGameObjectFactory;
        private ISpriteFont _mockSpriteFont;
        private IDeviceModel _mockDeviceModel;
        private ILogPanelModel _mockLogPanelModel;

        private IStatusIndicator _mockStatusIndicator;
        private ISequenceSubmitButton _mockSubmitButton;
        private ILogPanelLayer _mockLogPanelLayer;
        private IDeviceTheme _mockDeviceTheme;
        private IGenericButton _mockMenuButton;
        private IButtonObjectDefinitionFactory _mockBODFactory;
        private ISound _mockSubmitSound;

        protected override void OnInitializing()
        {
            base.OnInitializing();

            _mockEngine = Substitute.For<IEngine>();
            _mockSpriteBatch = Substitute.For<ISpriteBatch>();
            _mockSpriteFont = Substitute.For<ISpriteFont>();
            _mockDeviceModel = Substitute.For<IDeviceModel>();
            _mockLogPanelModel = Substitute.For<ILogPanelModel>();

            _mockEngine.AssetBank.Get<ISpriteFont>("TitleFont").Returns(_mockSpriteFont);
            _mockEngine.AssetBank.Get<ISpriteFont>("SceneChangeButtonFont").Returns(_mockSpriteFont);
            _mockEngine.ScreenManager.World.Returns(new Point(1000, 500));

            IThemeManagerCollection mockThemeManagerCollection = Substitute.For<IThemeManagerCollection>();
            _mockEngine.GetComponent<IThemeManagerCollection>().Returns(mockThemeManagerCollection);

            mockThemeManagerCollection[ThemeObjectType.Device].GetCurrentTheme<IDeviceTheme>().Returns(_mockDeviceTheme = Substitute.For<IDeviceTheme>());

            _mockGameObjectFactory = Substitute.For<GameObjectFactory>();
            GameObjectFactory.SetInstance(_mockGameObjectFactory);

            _mockGameObjectFactory.CreateStatusBar(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>(), Arg.Any<IDeviceModel>()).Returns(_mockStatusIndicator = Substitute.For<IStatusIndicator>());
            _mockGameObjectFactory.CreateSequenceSubmitButton(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>()).Returns(_mockSubmitButton = Substitute.For<ISequenceSubmitButton>());
            _mockGameObjectFactory.CreateLogPanelLayer(Arg.Any<IEngine>(), Arg.Any<ISpriteBatch>(), Arg.Any<IDeviceModel>(), Arg.Any<ILogPanelModel>()).Returns(_mockLogPanelLayer = Substitute.For<ILogPanelLayer>());
            _mockGameObjectFactory.CreateButtonObjectDefinitionFactory().Returns(_mockBODFactory = Substitute.For<IButtonObjectDefinitionFactory>());
            _mockGameObjectFactory.CreateGenericButton(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>()).Returns(_mockMenuButton = Substitute.For<IGenericButton>());

            List<IButtonObjectDefinition> buttonObjects = new List<IButtonObjectDefinition>();
            _mockMenuButton.ButtonObjects.Returns(buttonObjects);

            _mockSpriteFont.LineSpacing.Returns(20);
            _mockStatusIndicator.Bounds.Returns(new RectangleF(0, 0, 600, 100));

            mockThemeManagerCollection[ThemeObjectType.Device].GetCurrentTheme<IDeviceTheme>().HighlightColourMask.Returns(Color.Pink);
            _mockDeviceTheme.SceneChangeBackgroundMask.Returns(Color.Red);
            _mockDeviceTheme.SceneChangeBorderNormalMask.Returns(Color.Green);
            _mockDeviceTheme.SceneChangeBorderHighlightMask.Returns(Color.Blue);

            _mockSubmitSound = Substitute.For<ISound>();
            _mockEngine.AssetBank.Get<ISoundResource>("SubmitSequence").CreateInstance().Returns(_mockSubmitSound);
        }

        private void Construct()
        {
            _sut = new DeviceMainUILayer(_mockEngine, _mockSpriteBatch, _mockDeviceModel, _mockLogPanelModel);
        }

        #region Constructor Tests
        #region Status Indicator
        [TestMethod]
        public void ConstructShouldCreateNewStatusIndicator()
        {
            Construct();
            _mockGameObjectFactory.Received(1).CreateStatusBar(_mockEngine, new RectangleF(0, 0, 600, 100), _mockSpriteBatch, _mockDeviceModel);
        }

        [TestMethod]
        public void ConstructShouldSetStatusIndicatorBounds()
        {
            Construct();
            _mockStatusIndicator.Received(1).Bounds = new RectangleF(200, 0, 600, 100);
        }

        [TestMethod]
        public void ConstructShouldAddStatusIndicatorToEntities()
        {
            Construct();
            Assert.IsTrue(_sut.Entities.Contains(_mockStatusIndicator));
        }
        #endregion

        #region Sequence Submit Button
        [TestMethod]
        public void ConstructShouldCreateSequenceSubmitButton()
        {
            Construct();
            _mockGameObjectFactory.Received(1).CreateSequenceSubmitButton(_mockEngine, new RectangleF(900, 150, 100, 200), _mockSpriteBatch);
        }

        [TestMethod]
        public void ConstructShouldAddSequenceSubmitButtonToEntitiesList()
        {
            Construct();
            Assert.IsTrue(_sut.Entities.Contains(_mockSubmitButton));
        }

        [TestMethod]
        public void ConstructShouldAttachToSubmitSequenceEvent()
        {
            Construct();
            _mockSubmitButton.Received(1).SubmitSequence += Arg.Any<EventHandler<EventArgs>>();
        }

        [TestMethod]
        public void ConstructShouldAttachToSubmitButtonTouchedEvent()
        {
            Construct();
            _mockSubmitButton.Received(1).Touched += Arg.Any<EventHandler<ButtonEventArgs>>();
        }

        [TestMethod]
        public void ConstructShouldAttachToSubmitButtonReleasedEvent()
        {
            Construct();
            _mockSubmitButton.Received(1).Released += Arg.Any<EventHandler<ButtonEventArgs>>();
        }
        #endregion

        #region Menu Button
        [TestMethod]
        public void ConstructShouldCreateButtonObjectDefinitionFactory()
        {
            Construct();

            _mockGameObjectFactory.Received(1).CreateButtonObjectDefinitionFactory();
        }

        [TestMethod]
        public void ConstructShouldCreateMenuButton()
        {
            Construct();

            _mockGameObjectFactory.Received(1).CreateGenericButton(_mockEngine, new RectangleF(0, 150, 100, 200), _mockSpriteBatch);
        }

        [TestMethod]
        public void ConstructShouldSetMenuButtonFillTexture()
        {
            _mockDeviceTheme.SceneChangeBackgroundMask.Returns(Color.Pink);
            _mockDeviceTheme.SceneChangeBorderNormalMask.Returns(Color.Purple);
            _mockDeviceTheme.SceneChangeBorderHighlightMask.Returns(Color.BlueViolet);

            ITexture2D mockTexture = Substitute.For<ITexture2D>();
            _mockEngine.AssetBank.Get<ITexture2D>("SceneChangeButtonFill").Returns(mockTexture);

            IButtonTextureDefinition mockTextureDefinition = Substitute.For<IButtonTextureDefinition>();
            _mockBODFactory.CreateButtonTextureDefinition(Arg.Any<ITexture2D>(), Arg.Any<Color>(), Arg.Any<SpriteEffects>()).Returns(mockTextureDefinition);

            Construct();

            Received.InOrder(
                () =>
                {
                    _mockEngine.AssetBank.Get<ITexture2D>("SceneChangeButtonFill");
                    _mockBODFactory.CreateButtonTextureDefinition(mockTexture, Color.Pink, SpriteEffects.None);
                    _mockMenuButton.AddButtonObject(mockTextureDefinition);
                }
            );
        }

        [TestMethod]
        public void ConstructShouldSetMenuButtonBorderTexture()
        {
            _mockDeviceTheme.SceneChangeBackgroundMask.Returns(Color.Pink);
            _mockDeviceTheme.SceneChangeBorderNormalMask.Returns(Color.Purple);
            _mockDeviceTheme.SceneChangeBorderHighlightMask.Returns(Color.BlueViolet);

            ITexture2D mockTexture = Substitute.For<ITexture2D>();
            _mockEngine.AssetBank.Get<ITexture2D>("SceneChangeButtonBorder").Returns(mockTexture);

            IButtonTextureDefinition mockTextureDefinition = Substitute.For<IButtonTextureDefinition>();
            _mockBODFactory.CreateButtonTextureDefinition(Arg.Any<ITexture2D>(), Arg.Any<Color>(), Arg.Any<Color>(), Arg.Any<SpriteEffects>()).Returns(mockTextureDefinition, Substitute.For<IButtonTextureDefinition>());

            Construct();

            Received.InOrder(
                () =>
                {
                    _mockEngine.AssetBank.Get<ITexture2D>("SceneChangeButtonBorder");
                    _mockBODFactory.CreateButtonTextureDefinition(mockTexture, Color.Purple, Color.BlueViolet, SpriteEffects.None);
                    _mockMenuButton.AddButtonObject(mockTextureDefinition);
                }
            );
        }

        [TestMethod]
        public void ConstructShouldSetMenuButtonIcon()
        {
            _mockDeviceTheme.SceneChangeBackgroundMask.Returns(Color.Pink);
            _mockDeviceTheme.SceneChangeBorderNormalMask.Returns(Color.Purple);
            _mockDeviceTheme.SceneChangeBorderHighlightMask.Returns(Color.BlueViolet);
            _mockDeviceTheme.SceneChangeIconOffset.Returns(new Vector2(7, 5));

            ITexture2D mockTexture = Substitute.For<ITexture2D>();
            _mockEngine.AssetBank.Get<ITexture2D>("IconMenu").Returns(mockTexture);

            IButtonTextureDefinition mockTextureDefinition = Substitute.For<IButtonTextureDefinition>();
            _mockBODFactory.CreateButtonTextureDefinition(Arg.Any<ITexture2D>(), Arg.Any<Color>(), Arg.Any<Color>(), Arg.Any<SpriteEffects>()).Returns(Substitute.For<IButtonTextureDefinition>(), mockTextureDefinition);

            Construct();

            Received.InOrder(
                () =>
                {
                    _mockEngine.AssetBank.Get<ITexture2D>("IconMenu");
                    _mockBODFactory.CreateButtonTextureDefinition(mockTexture, Color.Purple, Color.BlueViolet, SpriteEffects.None);
                    _mockMenuButton.AddButtonObject(mockTextureDefinition);
                    mockTextureDefinition.GlobalOffset = new Vector2(7, 5);
                }
            );
        }

        [TestMethod]
        public void ConstructShouldAddEventHandlerToMenuButtonTapped()
        {
            Construct();

            _mockMenuButton.Tapped += Arg.Any<EventHandler<ButtonEventArgs>>();
        }

        [TestMethod]
        public void ConstructShouldAddReturnButtonToEntities()
        {
            Construct();

            Assert.IsTrue(_sut.Entities.Contains(_mockMenuButton));
        }
        #endregion

        #region Log Panel Layer
        [TestMethod]
        public void ConstructShouldCreateLogPanelLayer()
        {
            Construct();
            _mockGameObjectFactory.Received(1).CreateLogPanelLayer(_mockEngine, _mockSpriteBatch, _mockDeviceModel, _mockLogPanelModel);
        }

        [TestMethod]
        public void ConstructShouldAddLogPanelLayerToEntitiesList()
        {
            Construct();
            Assert.IsTrue(_sut.Entities.Contains(_mockLogPanelLayer));
        }

        [TestMethod]
        public void ConstructShouldAttachToLogPanelLayerModeChangeStartedEvent()
        {
            Construct();

            _mockLogPanelLayer.Received(1).ModeChangeStarted += Arg.Any<EventHandler<EventArgs>>();
        }
        #endregion
        #endregion

        #region Sequence Submit Button Tests
        [TestMethod]
        public void SequenceSubmitButtonSequenceSubmitEventShouldFireSequenceSubmitEvent()
        {
            Construct();
            EventHandler<EventArgs> mockSubscriber = Substitute.For<EventHandler<EventArgs>>();
            _sut.SubmitSequence += mockSubscriber;

            _mockSubmitButton.SubmitSequence += Raise.Event<EventHandler<EventArgs>>(_mockSubmitButton, new EventArgs());

            mockSubscriber.Received(1)(_sut, Arg.Any<EventArgs>());
        }

        [TestMethod]
        public void SequenceSubmitButtonSequenceSubmitShouldStopSubmitSound()
        {
            Construct();

            _mockSubmitButton.SubmitSequence += Raise.Event<EventHandler<EventArgs>>(_mockSubmitButton, new EventArgs());

            _mockSubmitSound.Received(1).Stop();
        }

        [TestMethod]
        public void SequenceSubmitButtonTouchedShouldPlaySubmitSound()
        {
            Construct();

            _mockSubmitButton.Touched += Raise.Event<EventHandler<ButtonEventArgs>>(_mockSubmitButton, new ButtonEventArgs(Vector2.Zero));

            _mockSubmitSound.Received(1).Play();
        }

        [TestMethod]
        public void SequenceSubmitButtonReleasedShouldStopSubmitSound()
        {
            Construct();

            _mockSubmitButton.Released += Raise.Event<EventHandler<ButtonEventArgs>>(_mockSubmitButton, new ButtonEventArgs(Vector2.Zero));

            _mockSubmitSound.Received(1).Stop();
        }
        #endregion

        #region Scene Change Button Tests
        [TestMethod]
        public void LeftButtonTappedShouldFireMenuButtonTappedEvent()
        {
            Construct();
            EventHandler<EventArgs> mockSubscriber = Substitute.For<EventHandler<EventArgs>>();
            _sut.MenuButtonTapped += mockSubscriber;

            _mockMenuButton.Tapped += Raise.Event<EventHandler<ButtonEventArgs>>(_mockMenuButton, new ButtonEventArgs(Vector2.Zero));

            mockSubscriber.Received(1)(_sut, Arg.Any<EventArgs>());
        }
        #endregion

        #region LogPanelLayer ModeChangedStarted Event Tests
        [TestMethod]
        public void LogPanelLayerModeChangeStartedEventShouldPlayExpectedSound()
        {
            Construct();
            ISoundResource mockSoundResource = Substitute.For<ISoundResource>();
            _mockEngine.AssetBank.Get<ISoundResource>("ButtonPress").Returns(mockSoundResource);

            _mockLogPanelLayer.ModeChangeStarted += Raise.Event<EventHandler<EventArgs>>(_mockLogPanelLayer, new EventArgs());

            mockSoundResource.Received(1).Play();
        }
        #endregion
    }
}
