using ArbitraryPixel.CodeLogic.Common;
using ArbitraryPixel.CodeLogic.Common.Layers;
using ArbitraryPixel.CodeLogic.Common.Theme;
using ArbitraryPixel.CodeLogic.Common.Theme.Device;
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
    public class DeviceMenuUILayer_Tests
    {
        private DeviceMenuUILayer _sut;
        private GameObjectFactory _mockGameObjectFactory;
        private ISpriteBatch _mockSpriteBatch;
        private IEngine _mockEngine;
        private IDeviceTheme _mockDeviceTheme;
        private ISpriteFont _mockFont;
        private ITextLabel _mockLabel;
        private IGenericButton _mockReturnButton;
        private IButtonObjectDefinitionFactory _mockBODFactory;
        private IThemeManagerCollection _mockThemeCollection;

        [TestInitialize]
        public void Initialize()
        {
            _mockEngine = Substitute.For<IEngine>();
            _mockSpriteBatch = Substitute.For<ISpriteBatch>();

            _mockGameObjectFactory = Substitute.For<GameObjectFactory>();
            GameObjectFactory.SetInstance(_mockGameObjectFactory);

            _mockThemeCollection = Substitute.For<IThemeManagerCollection>();
            _mockEngine.GetComponent<IThemeManagerCollection>().Returns(_mockThemeCollection);

            _mockEngine.ScreenManager.World.Returns(new Point(400, 400));
            _mockThemeCollection[ThemeObjectType.Device].GetCurrentTheme<IDeviceTheme>().Returns(_mockDeviceTheme = Substitute.For<IDeviceTheme>());
            _mockDeviceTheme.GetFullAssetName(Arg.Any<string>()).Returns(x => x[0].ToString());
            _mockDeviceTheme.HighlightColourMask.Returns(Color.Pink);

            _mockEngine.AssetBank.Get<ISpriteFont>("TitleFont").Returns(_mockFont = Substitute.For<ISpriteFont>());
            _mockFont.LineSpacing.Returns(25);

            _mockGameObjectFactory.CreateGenericTextLabel(Arg.Any<IEngine>(), Arg.Any<Vector2>(), Arg.Any<ISpriteBatch>(), Arg.Any<ISpriteFont>(), Arg.Any<string>(), Arg.Any<Color>()).Returns(_mockLabel = Substitute.For<ITextLabel>());
            _mockLabel.Bounds.Returns(new RectangleF(0, 0, 100, 25));

            _mockGameObjectFactory.CreateButtonObjectDefinitionFactory().Returns(_mockBODFactory = Substitute.For<IButtonObjectDefinitionFactory>());
            _mockGameObjectFactory.CreateGenericButton(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>()).Returns(_mockReturnButton = Substitute.For<IGenericButton>());

            List<IButtonObjectDefinition> buttonObjects = new List<IButtonObjectDefinition>();
            _mockReturnButton.ButtonObjects.Returns(buttonObjects);
        }

        private void Construct()
        {
            _sut = new DeviceMenuUILayer(_mockEngine, _mockSpriteBatch);
        }

        #region Constructor Tests
        [TestMethod]
        public void ConstructShouldRetrieveDeviceTheme()
        {
            Construct();
            _mockThemeCollection[ThemeObjectType.Device].Received(1).GetCurrentTheme<IDeviceTheme>();
        }

        #region Return Button
        [TestMethod]
        public void ConstructShouldCreateButtonObjectDefinitionFactory()
        {
            Construct();

            _mockGameObjectFactory.Received(1).CreateButtonObjectDefinitionFactory();
        }

        [TestMethod]
        public void ConstructShouldCreateReturnButton()
        {
            Construct();

            _mockGameObjectFactory.Received(1).CreateGenericButton(_mockEngine, new RectangleF(300, 100, 100, 200), _mockSpriteBatch);
        }

        [TestMethod]
        public void ConstructShouldSetReturnButtonFillTexture()
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
                    _mockBODFactory.CreateButtonTextureDefinition(mockTexture, Color.Pink, SpriteEffects.FlipHorizontally);
                    _mockReturnButton.AddButtonObject(mockTextureDefinition);
                }
            );
        }

        [TestMethod]
        public void ConstructShouldSetReturnButtonBorderTexture()
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
                    _mockBODFactory.CreateButtonTextureDefinition(mockTexture, Color.Purple, Color.BlueViolet, SpriteEffects.FlipHorizontally);
                    _mockReturnButton.AddButtonObject(mockTextureDefinition);
                }
            );
        }

        [TestMethod]
        public void ConstructShouldSetReturnButtonIcon()
        {
            _mockDeviceTheme.SceneChangeBackgroundMask.Returns(Color.Pink);
            _mockDeviceTheme.SceneChangeBorderNormalMask.Returns(Color.Purple);
            _mockDeviceTheme.SceneChangeBorderHighlightMask.Returns(Color.BlueViolet);
            _mockDeviceTheme.SceneChangeIconOffset.Returns(new Vector2(7, 5));

            ITexture2D mockTexture = Substitute.For<ITexture2D>();
            _mockEngine.AssetBank.Get<ITexture2D>("IconBack").Returns(mockTexture);

            IButtonTextureDefinition mockTextureDefinition = Substitute.For<IButtonTextureDefinition>();
            _mockBODFactory.CreateButtonTextureDefinition(Arg.Any<ITexture2D>(), Arg.Any<Color>(), Arg.Any<Color>(), Arg.Any<SpriteEffects>()).Returns(Substitute.For<IButtonTextureDefinition>(), mockTextureDefinition);

            Construct();

            Received.InOrder(
                () =>
                {
                    _mockEngine.AssetBank.Get<ITexture2D>("IconBack");
                    _mockBODFactory.CreateButtonTextureDefinition(mockTexture, Color.Purple, Color.BlueViolet, SpriteEffects.FlipHorizontally);
                    _mockReturnButton.AddButtonObject(mockTextureDefinition);
                    mockTextureDefinition.GlobalOffset = new Vector2(-7, 5);
                }
            );
        }

        [TestMethod]
        public void ConstructShouldAddEventHandlerToReturnButtonTapped()
        {
            Construct();

            _mockReturnButton.Tapped += Arg.Any<EventHandler<ButtonEventArgs>>();
        }

        [TestMethod]
        public void ConstructShouldAddReturnButtonToEntities()
        {
            Construct();

            Assert.IsTrue(_sut.Entities.Contains(_mockReturnButton));
        }
        #endregion

        #region Abort Button
        [TestMethod]
        public void ConstructShouldCreateDeviceMenuButtonForAbort()
        {
            ITexture2D mockTexture = Substitute.For<ITexture2D>();
            _mockEngine.AssetBank.Get<ITexture2D>("DeviceMenuButtonAbort").Returns(mockTexture);

            Construct();

            Received.InOrder(
                () =>
                {
                    _mockEngine.AssetBank.Get<ITexture2D>("DeviceMenuButtonAbort");
                    _mockGameObjectFactory.Received(1).CreateDeviceMenuButton(_mockEngine, new RectangleF(-125, 50, 300, 300), _mockSpriteBatch, mockTexture);
                }
            );
        }

        [TestMethod]
        public void ConstructShouldAddEventHandlerToAbortButtonTapped()
        {
            IButton mockButton = Substitute.For<IButton>();
            _mockGameObjectFactory.CreateDeviceMenuButton(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>(), Arg.Any<ITexture2D>()).Returns(mockButton, Substitute.For<IButton>());

            Construct();

            mockButton.Received(1).Tapped += Arg.Any<EventHandler<ButtonEventArgs>>();
        }

        [TestMethod]
        public void ConstructShouldAddAbortButtonToEntities()
        {
            IButton mockButton = Substitute.For<IButton>();
            _mockGameObjectFactory.CreateDeviceMenuButton(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>(), Arg.Any<ITexture2D>()).Returns(mockButton, Substitute.For<IButton>());

            Construct();

            Assert.IsTrue(_sut.Entities.Contains(mockButton));
        }
        #endregion

        #region Settings Button
        [TestMethod]
        public void ConstructShouldCreateDeviceMenuButtonForSettings()
        {
            ITexture2D mockTexture = Substitute.For<ITexture2D>();
            _mockEngine.AssetBank.Get<ITexture2D>("DeviceMenuButtonSettings").Returns(mockTexture);

            Construct();

            Received.InOrder(
                () =>
                {
                    _mockEngine.AssetBank.Get<ITexture2D>("DeviceMenuButtonSettings");
                    _mockGameObjectFactory.Received(1).CreateDeviceMenuButton(_mockEngine, new RectangleF(200, 50, 300, 300), _mockSpriteBatch, mockTexture);
                }
            );
        }

        [TestMethod]
        public void ConstructShouldAddEventHandlerToSettingsButtonTapped()
        {
            IButton mockButton = Substitute.For<IButton>();
            _mockGameObjectFactory.CreateDeviceMenuButton(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>(), Arg.Any<ITexture2D>()).Returns(Substitute.For<IButton>(), mockButton, Substitute.For<IButton>());

            Construct();

            mockButton.Received(1).Tapped += Arg.Any<EventHandler<ButtonEventArgs>>();
        }

        [TestMethod]
        public void ConstructShouldAddSEttingsButtonToEntities()
        {
            IButton mockButton = Substitute.For<IButton>();
            _mockGameObjectFactory.CreateDeviceMenuButton(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>(), Arg.Any<ITexture2D>()).Returns(Substitute.For<IButton>(), mockButton, Substitute.For<IButton>());

            Construct();

            Assert.IsTrue(_sut.Entities.Contains(mockButton));
        }
        #endregion
        #endregion

        #region Button Tapped Tests
        [TestMethod]
        public void ReturnButtonTappedShouldFireReturnButtonTappedEvent()
        {
            Construct();
            EventHandler<EventArgs> subscriber = Substitute.For<EventHandler<EventArgs>>();
            _sut.ReturnButtonTapped += subscriber;

            _mockReturnButton.Tapped += Raise.Event<EventHandler<ButtonEventArgs>>(_mockReturnButton, new ButtonEventArgs(Vector2.Zero));

            subscriber.Received(1)(_sut, Arg.Any<EventArgs>());
        }

        [TestMethod]
        public void AbortButtonTappedShouldFireAbortButtonTappedEvent()
        {
            IButton mockButton = Substitute.For<IButton>();
            _mockGameObjectFactory.CreateDeviceMenuButton(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>(), Arg.Any<ITexture2D>()).Returns(mockButton, Substitute.For<IButton>());
            Construct();
            EventHandler<EventArgs> subscriber = Substitute.For<EventHandler<EventArgs>>();
            _sut.AbortButtonTapped += subscriber;

            mockButton.Tapped += Raise.Event<EventHandler<ButtonEventArgs>>(mockButton, new ButtonEventArgs(Vector2.Zero));

            subscriber.Received(1)(_sut, Arg.Any<EventArgs>());
        }

        [TestMethod]
        public void SettingsButtonTappedShouldFireSettingsButtonTappedEvent()
        {
            IButton mockButton = Substitute.For<IButton>();
            _mockGameObjectFactory.CreateDeviceMenuButton(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>(), Arg.Any<ITexture2D>()).Returns(Substitute.For<IButton>(), mockButton, Substitute.For<IButton>());
            Construct();
            EventHandler<EventArgs> subscriber = Substitute.For<EventHandler<EventArgs>>();
            _sut.SettingsButtonTapped += subscriber;

            mockButton.Tapped += Raise.Event<EventHandler<ButtonEventArgs>>(mockButton, new ButtonEventArgs(Vector2.Zero));

            subscriber.Received(1)(_sut, Arg.Any<EventArgs>());
        }
        #endregion
    }
}
