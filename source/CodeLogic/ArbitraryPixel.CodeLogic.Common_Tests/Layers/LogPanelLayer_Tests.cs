using ArbitraryPixel.CodeLogic.Common;
using ArbitraryPixel.CodeLogic.Common.Layers;
using ArbitraryPixel.CodeLogic.Common.Model;
using ArbitraryPixel.CodeLogic.Common.Theme;
using ArbitraryPixel.CodeLogic.Common.Theme.Device;
using ArbitraryPixel.Common.Drawing;
using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Platform2D.Engine;
using ArbitraryPixel.Platform2D.Layer;
using ArbitraryPixel.Platform2D.Theme;
using ArbitraryPixel.Platform2D.UI;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NSubstitute;
using System;
using System.Linq;

namespace ArbitraryPixel.CodeLogic.Common_Tests.Layers
{
    [TestClass]
    public class LogPanelLayer_Tests
    {
        private LogPanelLayer _sut;
        private IEngine _mockEngine;
        private ISpriteBatch _mockSpriteBatch;
        private IDeviceModel _mockDeviceModel;
        private ILogPanelModel _mockPanelModel;

        private IThemeManagerCollection _mockThemeCollection;

        private IGenericButton _mockButton;

        private GameObjectFactory _mockGOFactory;

        [TestInitialize]
        public void Initialize()
        {
            _mockEngine = Substitute.For<IEngine>();
            _mockSpriteBatch = Substitute.For<ISpriteBatch>();
            _mockDeviceModel = Substitute.For<IDeviceModel>();
            _mockPanelModel = Substitute.For<ILogPanelModel>();

            _mockThemeCollection = Substitute.For<IThemeManagerCollection>();
            _mockEngine.GetComponent<IThemeManagerCollection>().Returns(_mockThemeCollection);

            _mockGOFactory = Substitute.For<GameObjectFactory>();
            GameObjectFactory.SetInstance(_mockGOFactory);

            _mockButton = Substitute.For<IGenericButton>();
            _mockGOFactory.CreateGenericButton(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>()).Returns(_mockButton);
        }

        private void Construct()
        {
            _sut = new LogPanelLayer(_mockEngine, _mockSpriteBatch, _mockDeviceModel, _mockPanelModel);
        }

        private void RaiseButtonTappedEvent()
        {
            _mockButton.Tapped += Raise.Event<EventHandler<ButtonEventArgs>>(_mockButton, new ButtonEventArgs(Vector2.Zero));
        }

        #region Constructor Tests
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullParameterShouldThrowException_DeviceModel()
        {
            _sut = new LogPanelLayer(_mockEngine, _mockSpriteBatch, null, _mockPanelModel);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullParameterShouldThrowException_PanelModel()
        {
            _sut = new LogPanelLayer(_mockEngine, _mockSpriteBatch, _mockDeviceModel, null);
        }

        [TestMethod]
        public void ConstructShouldRequestCurrentTheme()
        {
            Construct();

            _mockThemeCollection[ThemeObjectType.Device].Received(1).GetCurrentTheme<IDeviceTheme>();
        }

        [TestMethod]
        public void ConstructShouldCreateButton()
        {
            _mockPanelModel.WorldBounds.Returns(new SizeF(500, 300));

            IGenericButton mockButton = Substitute.For<IGenericButton>();
            _mockGOFactory.CreateGenericButton(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>()).Returns(mockButton);

            ISpriteFont mockFont = Substitute.For<ISpriteFont>();
            _mockEngine.AssetBank.Get<ISpriteFont>("SceneChangeButtonFont").Returns(mockFont);

            IDeviceTheme mockTheme = Substitute.For<IDeviceTheme>();
            _mockThemeCollection[ThemeObjectType.Device].GetCurrentTheme<IDeviceTheme>().Returns(mockTheme);
            mockTheme.SceneChangeBackgroundMask.Returns(Color.Red);
            mockTheme.SceneChangeBorderNormalMask.Returns(Color.Green);
            mockTheme.SceneChangeBorderHighlightMask.Returns(Color.Blue);

            Construct();

            Received.InOrder(
                () =>
                {
                    _mockGOFactory.CreateButtonObjectDefinitionFactory();
                    _mockGOFactory.CreateGenericButton(
                        _mockEngine,
                        new RectangleF(500 / 2f - 200 / 2f, 300 - 100, 200, 100),
                        _mockSpriteBatch
                    );
                }
            );
        }

        [TestMethod]
        public void ConstructShouldAddButtonToEntities()
        {
            IGenericButton mockButton = Substitute.For<IGenericButton>();
            _mockGOFactory.CreateGenericButton(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>()).Returns(mockButton);

            Construct();

            Assert.IsTrue(_sut.Entities.Contains(mockButton));
        }

        [TestMethod]
        public void ConstructShouldCreateButtonFill()
        {
            IButtonObjectDefinitionFactory mockBODFactory = Substitute.For<IButtonObjectDefinitionFactory>();
            _mockGOFactory.CreateButtonObjectDefinitionFactory().Returns(mockBODFactory);

            IGenericButton mockButton = Substitute.For<IGenericButton>();
            _mockGOFactory.CreateGenericButton(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>()).Returns(mockButton);

            ITexture2D mockTexture = Substitute.For<ITexture2D>();
            _mockEngine.AssetBank.Get<ITexture2D>("LogButtonFill").Returns(mockTexture);

            IButtonTextureDefinition mockTextureDefinition = Substitute.For<IButtonTextureDefinition>();
            mockBODFactory.CreateButtonTextureDefinition(Arg.Any<ITexture2D>(), Arg.Any<Color>(), Arg.Any<SpriteEffects>()).Returns(mockTextureDefinition);

            IDeviceTheme mockTheme = Substitute.For<IDeviceTheme>();
            _mockThemeCollection[ThemeObjectType.Device].GetCurrentTheme<IDeviceTheme>().Returns(mockTheme);
            mockTheme.SceneChangeBackgroundMask.Returns(Color.Red);
            mockTheme.SceneChangeBorderNormalMask.Returns(Color.Green);
            mockTheme.SceneChangeBorderHighlightMask.Returns(Color.Blue);

            Construct();

            Received.InOrder(
                () =>
                {
                    _mockEngine.AssetBank.Get<ITexture2D>("LogButtonFill");
                    mockBODFactory.CreateButtonTextureDefinition(mockTexture, Color.Red, SpriteEffects.None);
                    mockButton.AddButtonObject(mockTextureDefinition);
                }
            );
        }

        [TestMethod]
        public void ConstructShouldCreateButtonBorder()
        {
            IButtonObjectDefinitionFactory mockBODFactory = Substitute.For<IButtonObjectDefinitionFactory>();
            _mockGOFactory.CreateButtonObjectDefinitionFactory().Returns(mockBODFactory);

            IGenericButton mockButton = Substitute.For<IGenericButton>();
            _mockGOFactory.CreateGenericButton(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>()).Returns(mockButton);

            ITexture2D mockTexture = Substitute.For<ITexture2D>();
            _mockEngine.AssetBank.Get<ITexture2D>("LogButtonBorder").Returns(mockTexture);

            IButtonTextureDefinition mockTextureDefinition = Substitute.For<IButtonTextureDefinition>();
            mockBODFactory.CreateButtonTextureDefinition(Arg.Any<ITexture2D>(), Arg.Any<Color>(), Arg.Any<Color>(), Arg.Any<SpriteEffects>()).Returns(mockTextureDefinition, Substitute.For<IButtonTextureDefinition>());

            IDeviceTheme mockTheme = Substitute.For<IDeviceTheme>();
            _mockThemeCollection[ThemeObjectType.Device].GetCurrentTheme<IDeviceTheme>().Returns(mockTheme);
            mockTheme.SceneChangeBackgroundMask.Returns(Color.Red);
            mockTheme.SceneChangeBorderNormalMask.Returns(Color.Green);
            mockTheme.SceneChangeBorderHighlightMask.Returns(Color.Blue);

            Construct();

            Received.InOrder(
                () =>
                {
                    _mockEngine.AssetBank.Get<ITexture2D>("LogButtonBorder");
                    mockBODFactory.CreateButtonTextureDefinition(mockTexture, Color.Green, Color.Blue, SpriteEffects.None);
                    mockButton.AddButtonObject(mockTextureDefinition);
                }
            );
        }

        [TestMethod]
        public void ConstructShouldCreateButtonIcon()
        {
            IButtonObjectDefinitionFactory mockBODFactory = Substitute.For<IButtonObjectDefinitionFactory>();
            _mockGOFactory.CreateButtonObjectDefinitionFactory().Returns(mockBODFactory);

            IGenericButton mockButton = Substitute.For<IGenericButton>();
            _mockGOFactory.CreateGenericButton(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>()).Returns(mockButton);

            ITexture2D mockTexture = Substitute.For<ITexture2D>();
            _mockEngine.AssetBank.Get<ITexture2D>("IconLog").Returns(mockTexture);

            IButtonTextureDefinition mockTextureDefinition = Substitute.For<IButtonTextureDefinition>();
            mockBODFactory.CreateButtonTextureDefinition(Arg.Any<ITexture2D>(), Arg.Any<Color>(), Arg.Any<Color>(), Arg.Any<SpriteEffects>()).Returns(Substitute.For<IButtonTextureDefinition>(), mockTextureDefinition);

            IDeviceTheme mockTheme = Substitute.For<IDeviceTheme>();
            _mockThemeCollection[ThemeObjectType.Device].GetCurrentTheme<IDeviceTheme>().Returns(mockTheme);
            mockTheme.SceneChangeBackgroundMask.Returns(Color.Red);
            mockTheme.SceneChangeBorderNormalMask.Returns(Color.Green);
            mockTheme.SceneChangeBorderHighlightMask.Returns(Color.Blue);
            mockTheme.SceneChangeIconOffset.Returns(new Vector2(-7, 5));

            Construct();

            Received.InOrder(
                () =>
                {
                    _mockEngine.AssetBank.Get<ITexture2D>("IconLog");
                    mockBODFactory.CreateButtonTextureDefinition(mockTexture, Color.Green, Color.Blue, SpriteEffects.None);
                    mockTextureDefinition.GlobalOffset = new Vector2(5, 7);
                    mockButton.AddButtonObject(mockTextureDefinition);
                }
            );
        }

        [TestMethod]
        public void ConstructShouldCreateLogPanelContentLayer()
        {
            Construct();

            _mockGOFactory.Received(1).CreateLogPanelContentLayer(_mockEngine, _mockSpriteBatch, _mockDeviceModel, _mockPanelModel);
        }

        [TestMethod]
        public void ConstructShouldAddLogPanelContentLayerToEntities()
        {
            ILayer mockPanelContent = Substitute.For<ILayer>();
            _mockGOFactory.CreateLogPanelContentLayer(Arg.Any<IEngine>(), Arg.Any<ISpriteBatch>(), Arg.Any<IDeviceModel>(), Arg.Any<ILogPanelModel>()).Returns(mockPanelContent);

            Construct();

            Assert.IsTrue(_sut.Entities.Contains(mockPanelContent));
        }

        [TestMethod]
        public void ConstructShouldAddEventHandlerToPanelModelModelResetEvent()
        {
            Construct();

            _mockPanelModel.Received(1).ModelReset += Arg.Any<EventHandler<EventArgs>>();
        }

        [TestMethod]
        public void ConstructShouldUpdateEntityPositionForModelOffset()
        {
            _mockButton.Bounds.Returns(new RectangleF(400, 400, 100, 100));
            _mockPanelModel.CurrentOffset.Returns(new Vector2(123, 321));
            Construct();

            _mockButton.Received(1).Bounds = new RectangleF(
                400 - 123,
                400 - 321,
                100,
                100
            );
        }
        #endregion

        #region Button Press Tests
        // NOTE: Unless specified, ButtonTapped tests assume NextMode is null.

        [TestMethod]
        public void ButtonTappedShouldSetModelPreviousOffset()
        {
            Construct();
            _mockPanelModel.NextMode.Returns((LogPanelMode?)null);
            _mockPanelModel.CurrentOffset = new Vector2(100, 200);
            RaiseButtonTappedEvent();

            _mockPanelModel.Received(1).PreviousOffset = new Vector2(100, 200);
        }

        [TestMethod]
        public void ButtonTapepdWithNextModeAlreadySetShouldNotSetModelPreviousOffset()
        {
            // NOTE: Not going to do the negative for all of these... this is enough :)

            Construct();
            _mockPanelModel.NextMode.Returns(LogPanelMode.Closed);
            _mockPanelModel.CurrentOffset = new Vector2(100, 200);
            RaiseButtonTappedEvent();

            _mockPanelModel.Received(0).PreviousOffset = new Vector2(100, 200);
        }

        [TestMethod]
        public void ButtonTappedShouldSetModelProgressValueToZero()
        {
            Construct();
            _mockPanelModel.NextMode.Returns((LogPanelMode?)null);
            RaiseButtonTappedEvent();

            _mockPanelModel.Received(1).ProgressValue = Vector2.Zero;
        }

        [TestMethod]
        public void ButtonTappedShouldSetModelProgressSpeedToExpectedValue()
        {
            Construct();
            _mockPanelModel.ProgressTarget.Returns(new Vector2(0, 1));
            _mockPanelModel.NextMode.Returns((LogPanelMode?)null);
            RaiseButtonTappedEvent();

            _mockPanelModel.Received(1).ProgressSpeed = new Vector2(0, 1f / 0.5f);
        }

        [TestMethod]
        public void ButtonTappedShouldSetNextMode_WithCurrentMode_Closed()
        {
            Construct();
            _mockPanelModel.NextMode.Returns((LogPanelMode?)null);
            _mockPanelModel.CurrentMode.Returns(LogPanelMode.Closed);
            RaiseButtonTappedEvent();

            _mockPanelModel.Received(1).NextMode = LogPanelMode.PartialView;
        }

        [TestMethod]
        public void ButtonTappedShouldSetTargetOffset_WithCurrentMode_Closed()
        {
            Construct();
            _mockPanelModel.NextMode.Returns((LogPanelMode?)null);
            _mockPanelModel.CurrentMode.Returns(LogPanelMode.Closed);
            _mockPanelModel.PartialSize.Returns(new SizeF(123, 456));
            RaiseButtonTappedEvent();

            _mockPanelModel.Received(1).TargetOffset = new Vector2(123, 456);
        }

        [TestMethod]
        public void ButtonTappedShouldSetNextMode_WithCurrentMode_PartialView()
        {
            Construct();
            _mockPanelModel.NextMode.Returns((LogPanelMode?)null);
            _mockPanelModel.CurrentMode.Returns(LogPanelMode.PartialView);
            RaiseButtonTappedEvent();

            _mockPanelModel.Received(1).NextMode = LogPanelMode.FullView;
        }

        [TestMethod]
        public void ButtonTappedShouldSetTargetOffset_WithCurrentMode_PartialView()
        {
            Construct();
            _mockPanelModel.NextMode.Returns((LogPanelMode?)null);
            _mockPanelModel.CurrentMode.Returns(LogPanelMode.PartialView);
            _mockPanelModel.FullSize.Returns(new SizeF(123, 456));
            RaiseButtonTappedEvent();

            _mockPanelModel.Received(1).TargetOffset = new Vector2(123, 456);
        }

        [TestMethod]
        public void ButtonTappedShouldSetNextMode_WithCurrentMode_FullView()
        {
            Construct();
            _mockPanelModel.NextMode.Returns((LogPanelMode?)null);
            _mockPanelModel.CurrentMode.Returns(LogPanelMode.FullView);
            RaiseButtonTappedEvent();

            _mockPanelModel.Received(1).NextMode = LogPanelMode.Closed;
        }

        [TestMethod]
        public void ButtonTappedShouldSetTargetOffset_WithCurrentMode_FullView()
        {
            Construct();
            _mockPanelModel.NextMode.Returns((LogPanelMode?)null);
            _mockPanelModel.CurrentMode.Returns(LogPanelMode.FullView);
            _mockPanelModel.ClosedSize.Returns(new SizeF(123, 456));
            RaiseButtonTappedEvent();

            _mockPanelModel.Received(1).TargetOffset = new Vector2(123, 456);
        }

        [TestMethod]
        public void ButtonTappedShouldFireModeChangeStartedEvent()
        {
            Construct();
            _mockPanelModel.NextMode.Returns((LogPanelMode?)null);
            _mockPanelModel.CurrentOffset = new Vector2(100, 200);
            EventHandler<EventArgs> subscriber = Substitute.For<EventHandler<EventArgs>>();
            _sut.ModeChangeStarted += subscriber;

            RaiseButtonTappedEvent();

            subscriber.Received(1)(_sut, Arg.Any<EventArgs>());
        }

        [TestMethod]
        public void ButtonTapped_WithCurrentMode_ShouldNotFireModeChangeStartedEvent()
        {
            Construct();
            _mockPanelModel.NextMode.Returns(LogPanelMode.PartialView);
            _mockPanelModel.CurrentOffset = new Vector2(100, 200);
            EventHandler<EventArgs> subscriber = Substitute.For<EventHandler<EventArgs>>();
            _sut.ModeChangeStarted += subscriber;

            RaiseButtonTappedEvent();

            subscriber.Received(0)(Arg.Any<object>(), Arg.Any<EventArgs>());
        }
        #endregion

        #region Update Tests
        [TestMethod]
        public void UpdateShouldCallPanelModelUpdate()
        {
            Construct();
            GameTime expected = new GameTime();

            _sut.Update(expected);

            _mockPanelModel.Received(1).Update(expected);
        }

        [TestMethod]
        public void UpdateShouldUpdateButtonPositionBasedOnPanelModelOffset_TestA()
        {
            _mockPanelModel.NextMode.Returns(LogPanelMode.Closed);
            _mockPanelModel.CurrentOffset = new Vector2(100, 200);
            _mockButton.Bounds.Returns(new RectangleF(200, 100, 400, 300));

            Construct();
            _mockButton.ClearReceivedCalls();

            _sut.Update(new GameTime());

            _mockButton.Received(1).Bounds = new RectangleF(
                200 - 100,
                100 - 200,
                400,
                300
            );
        }

        [TestMethod]
        public void UpdateShouldUpdateButtonPositionBasedOnPanelModelOffset_TestB()
        {
            _mockPanelModel.NextMode.Returns(LogPanelMode.Closed);
            _mockPanelModel.CurrentOffset = new Vector2(120, 75);
            _mockButton.Bounds.Returns(new RectangleF(200, 100, 400, 300));

            Construct();
            _mockButton.ClearReceivedCalls();

            _sut.Update(new GameTime());

            _mockButton.Received(1).Bounds = new RectangleF(
                200 - 120,
                100 - 75,
                400,
                300
            );
        }

        [TestMethod]
        public void UpdateWhenNextModeNullShouldNotUpdatePositions()
        {
            Construct();
            _mockPanelModel.NextMode.Returns((LogPanelMode?)null);
            _mockButton.ClearReceivedCalls();

            _sut.Update(new GameTime());

            _mockButton.Received(0).Bounds = Arg.Any<RectangleF>();
        }
        #endregion

        #region Draw Tests - Negative Tests
        // NOTE: Not going to test all the negative conditions, just use a spritebatch draw call to confirm that something wasn't done.
        [TestMethod]
        public void DrawWithModeClosedAndNextModeNullShouldNotCallSpriteBatchDraw()
        {
            Construct();
            _mockPanelModel.CurrentMode.Returns(LogPanelMode.Closed);
            _mockPanelModel.NextMode.Returns((LogPanelMode?)null);

            _sut.Draw(new GameTime());

            _mockSpriteBatch.Received(0).Draw(Arg.Any<ITexture2D>(), Arg.Any<RectangleF>(), Arg.Any<Color>());
        }

        [TestMethod]
        public void DrawWithModeClosedAndNextModeNotNullShouldCallSpriteBatchDraw()
        {
            Construct();
            _mockPanelModel.CurrentMode.Returns(LogPanelMode.Closed);
            _mockPanelModel.NextMode.Returns(LogPanelMode.PartialView);

            _sut.Draw(new GameTime());

            _mockSpriteBatch.Received(2).Draw(Arg.Any<ITexture2D>(), Arg.Any<RectangleF>(), Arg.Any<Color>());
        }

        [TestMethod]
        public void DrawWithModePartialViewAndNextModeNullShouldCallSpriteBatchDraw()
        {
            Construct();
            _mockPanelModel.CurrentMode.Returns(LogPanelMode.PartialView);
            _mockPanelModel.NextMode.Returns((LogPanelMode?)null);

            _sut.Draw(new GameTime());

            _mockSpriteBatch.Received(2).Draw(Arg.Any<ITexture2D>(), Arg.Any<RectangleF>(), Arg.Any<Color>());
        }

        [TestMethod]
        public void DrawWithModeFullViewAndNextModeNullShouldCallSpriteBatchDraw()
        {
            Construct();
            _mockPanelModel.CurrentMode.Returns(LogPanelMode.FullView);
            _mockPanelModel.NextMode.Returns((LogPanelMode?)null);

            _sut.Draw(new GameTime());

            _mockSpriteBatch.Received(2).Draw(Arg.Any<ITexture2D>(), Arg.Any<RectangleF>(), Arg.Any<Color>());
        }
        #endregion

        #region Draw Tests
        // Test when draw stuff actually happens :)
        [TestMethod]
        public void DrawShouldRequestCurrentTheme()
        {
            Construct();
            _mockThemeCollection[ThemeObjectType.Device].ClearReceivedCalls();
            _mockPanelModel.CurrentMode.Returns(LogPanelMode.PartialView);

            _sut.Draw(new GameTime());

            _mockThemeCollection[ThemeObjectType.Device].Received(1).GetCurrentTheme<IDeviceTheme>();
        }

        [TestMethod]
        public void DrawShouldRequestPixelTexture()
        {
            Construct();
            _mockPanelModel.CurrentMode.Returns(LogPanelMode.PartialView);

            _sut.Draw(new GameTime());

            _mockEngine.AssetBank.Received(1).Get<ITexture2D>("Pixel");
        }

        [TestMethod]
        public void DrawShouldDrawBorder()
        {
            Construct();
            _mockPanelModel.CurrentMode.Returns(LogPanelMode.PartialView);

            IDeviceTheme mockTheme = Substitute.For<IDeviceTheme>();
            mockTheme.SceneChangeBorderNormalMask.Returns(Color.Pink);
            _mockThemeCollection[ThemeObjectType.Device].GetCurrentTheme<IDeviceTheme>().Returns(mockTheme);

            ITexture2D mockTexture = Substitute.For<ITexture2D>();
            _mockEngine.AssetBank.Get<ITexture2D>("Pixel").Returns(mockTexture);

            _mockButton.Bounds.Returns(new RectangleF(200, 100, 400, 300));
            _mockPanelModel.WorldBounds.Returns(new SizeF(1000, 500));

            RectangleF expectedBounds = new RectangleF(
                0,
                400,
                1000,
                100
            );

            _sut.Draw(new GameTime());

            _mockSpriteBatch.Received(1).Draw(mockTexture, expectedBounds, Color.Pink);
        }

        [TestMethod]
        public void DrawShouldDrawBackground()
        {
            Construct();
            _mockPanelModel.CurrentMode.Returns(LogPanelMode.PartialView);

            IDeviceTheme mockTheme = Substitute.For<IDeviceTheme>();
            mockTheme.SceneChangeBackgroundMask.Returns(Color.Pink);
            _mockThemeCollection[ThemeObjectType.Device].GetCurrentTheme<IDeviceTheme>().Returns(mockTheme);

            ITexture2D mockTexture = Substitute.For<ITexture2D>();
            _mockEngine.AssetBank.Get<ITexture2D>("Pixel").Returns(mockTexture);

            _mockButton.Bounds.Returns(new RectangleF(200, 100, 400, 300));
            _mockPanelModel.WorldBounds.Returns(new SizeF(1000, 500));

            RectangleF expectedBounds = new RectangleF(
                0,
                400 + 2,
                1000,
                100
            );

            _sut.Draw(new GameTime());

            _mockSpriteBatch.Received(1).Draw(mockTexture, expectedBounds, Color.Pink);
        }
        #endregion

        #region LogPanelModel Reset Tests
        [TestMethod]
        public void ModelResetEventShouldUpdateButtonPosition()
        {
            _mockButton.Bounds.Returns(new RectangleF(200, 100, 400, 300));
            _mockPanelModel.CurrentOffset.Returns(new Vector2(123, 456));
            Construct();
            _mockButton.ClearReceivedCalls();

            _mockPanelModel.ModelReset += Raise.Event<EventHandler<EventArgs>>(_mockPanelModel, new EventArgs());

            _mockButton.Received(1).Bounds = new RectangleF(
                200 - 123,
                100 - 456,
                400,
                300
            );
        }
        #endregion
    }
}
