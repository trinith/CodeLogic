using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArbitraryPixel.CodeLogic.Common.Scenes;
using ArbitraryPixel.CodeLogic.Common.Model;
using ArbitraryPixel.Platform2D.Engine;
using NSubstitute;
using ArbitraryPixel.CodeLogic.Common;
using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Platform2D.Layer;
using ArbitraryPixel.Platform2D.Entity;
using ArbitraryPixel.Common.Drawing;
using Microsoft.Xna.Framework;
using ArbitraryPixel.Platform2D.Text;
using ArbitraryPixel.Common.Graphics.Factory;
using ArbitraryPixel.CodeLogic.Common.Entities;
using ArbitraryPixel.Platform2D.UI;
using System.Collections.Generic;
using ArbitraryPixel.Platform2D.Scene;
using ArbitraryPixel.Common;
using System.Linq;
using ArbitraryPixel.CodeLogic.Common.Config;
using ArbitraryPixel.Common.Audio;
using ArbitraryPixel.CodeLogic.Common.Controllers;

namespace ArbitraryPixel.CodeLogic.Common_Tests.Scenes
{
    [TestClass]
    public class MissionDebriefingScene_Tests
    {
        private MissionDebriefingScene _sut;
        private IEngine _mockEngine;
        private IDeviceModel _mockDeviceModel;
        private IGameStatsController _mockGameStatsController;

        private GameObjectFactory _mockGameObjectFactory;

        private ICodeLogicSettings _mockSettings;

        private ILayer _mockRootLayer;
        private ILayer _mockBGLayer;
        private ILayer _mockTextLayer;
        private ILayer _mockMarksLayer;
        private ILayer _mockLightOverlayLayer;
        private ILayer _mockUILayer;

        [TestInitialize]
        public void Initialize()
        {
            _mockEngine = Substitute.For<IEngine>();
            _mockDeviceModel = Substitute.For<IDeviceModel>();
            _mockGameStatsController = Substitute.For<IGameStatsController>();

            _mockSettings = Substitute.For<ICodeLogicSettings>();
            _mockEngine.GetComponent<ICodeLogicSettings>().Returns(_mockSettings);

            _mockGameObjectFactory = Substitute.For<GameObjectFactory>();
            GameObjectFactory.SetInstance(_mockGameObjectFactory);

            _mockBGLayer = Substitute.For<ILayer>();
            _mockGameObjectFactory.CreateGenericLayer(Arg.Any<IEngine>(), Arg.Any<ISpriteBatch>()).Returns(
                _mockRootLayer = Substitute.For<ILayer>(),
                _mockBGLayer = Substitute.For<ILayer>(),
                _mockTextLayer = Substitute.For<ILayer>(),
                _mockMarksLayer = Substitute.For<ILayer>(),
                _mockLightOverlayLayer = Substitute.For<ILayer>(),
                _mockUILayer = Substitute.For<ILayer>(),

                Substitute.For<ILayer>() // Remainder
            );

            _sut = new MissionDebriefingScene(_mockEngine, _mockDeviceModel, _mockGameStatsController);
        }

        #region Constructor Tests
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullParameterShouldThrowException_Model()
        {
            _sut = new MissionDebriefingScene(_mockEngine, null, _mockGameStatsController);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullParameterShouldThrowException_GameStatsController()
        {
            _sut = new MissionDebriefingScene(_mockEngine, _mockDeviceModel, null);
        }

        [TestMethod]
        public void ConstructShouldAttachToEngineExternalActionOccurredEvent()
        {
            _mockEngine.Received(1).ExternalActionOccurred += Arg.Any<EventHandler<ExternalActionEventArgs>>();
        }
        #endregion

        #region Initialize Tests
        [TestMethod]
        public void InitializeShouldCreateRequiredLayers()
        {
            ISpriteBatch mockSpriteBatch = Substitute.For<ISpriteBatch>();
            _mockEngine.GrfxFactory.SpriteBatchFactory.Create(Arg.Any<IGrfxDevice>()).Returns(mockSpriteBatch);
            ILayer mockLayer = Substitute.For<ILayer>();
            mockLayer.MainSpriteBatch.Returns(mockSpriteBatch);
            _mockGameObjectFactory.CreateGenericLayer(Arg.Any<IEngine>(), Arg.Any<ISpriteBatch>()).Returns(mockLayer, Substitute.For<ILayer>());

            _sut.Initialize();

            _mockGameObjectFactory.Received(6).CreateGenericLayer(_mockEngine, mockSpriteBatch);
        }

        [TestMethod]
        public void InitializeShouldAddLayersToRoot()
        {
            _sut.Initialize();

            Received.InOrder(
                () =>
                {
                    _mockRootLayer.AddEntity(_mockBGLayer);
                    _mockRootLayer.AddEntity(_mockTextLayer);
                    _mockRootLayer.AddEntity(_mockLightOverlayLayer);
                    _mockRootLayer.AddEntity(_mockUILayer);
                }
            );
        }

        [TestMethod]
        public void InitializeShouldAddRootLayerToEntities()
        {
            _sut.Initialize();

            Assert.IsTrue(_sut.Entities.Contains(_mockRootLayer));
        }

        [TestMethod]
        public void InitializeShouldAddTextureEntityToBackgroundLayer()
        {
            ITexture2D mockTexture = Substitute.For<ITexture2D>();
            _mockEngine.AssetBank.Get<ITexture2D>("MissionDebriefingBackground").Returns(mockTexture);

            ITextureEntity mockEntity = Substitute.For<ITextureEntity>();
            _mockGameObjectFactory.CreateTextureEntity(_mockEngine, new RectangleF(0, 0, 600, 500), Arg.Any<ISpriteBatch>(), mockTexture, Color.White).Returns(mockEntity);

            _mockEngine.ScreenManager.World.Returns(new Point(600, 500));

            _sut.Initialize();

            _mockBGLayer.Received(1).AddEntity(mockEntity);
        }

        [TestMethod]
        public void InitializeShouldAddTextureEntityToLightOverlayLayer()
        {
            ITexture2D mockTexture = Substitute.For<ITexture2D>();
            _mockEngine.AssetBank.Get<ITexture2D>("MissionDebriefingLightOverlay").Returns(mockTexture);

            _mockEngine.ScreenManager.World.Returns(new Point(600, 500));

            _sut.Initialize();

            Received.InOrder(
                () =>
                {
                    _mockGameObjectFactory.CreateTextureEntity(_mockEngine, new RectangleF(0, 0, 600, 500), Arg.Any<ISpriteBatch>(), mockTexture, Color.White);
                    _mockLightOverlayLayer.AddEntity(Arg.Any<IEntity>());
                }
            );
        }

        [TestMethod]
        public void InitializeShouldSetUpTextRendering()
        {
            RectangleF expectedBounds = new RectangleF(662, 62, 464, 600);
            expectedBounds.Inflate(-25, -25);

            ITextObjectBuilder mockTextBuilder = Substitute.For<ITextObjectBuilder>();
            _mockGameObjectFactory.CreateTextObjectBuilder(Arg.Any<ITextFormatProcessor>(), Arg.Any<ITextObjectFactory>()).Returns(mockTextBuilder);

            _sut.Initialize();

            Received.InOrder(
                () =>
                {
                    _mockEngine.GrfxFactory.SpriteBatchFactory.Create(Arg.Any<IGrfxDevice>()); // For root layer.

                    _mockGameObjectFactory.CreateTextFormatValueHandlerManager();
                    _mockGameObjectFactory.CreateTextFormatProcessor(Arg.Any<ITextFormatValueHandlerManager>());
                    _mockGameObjectFactory.CreateTextObjectFactory();
                    _mockGameObjectFactory.CreateTextObjectBuilder(Arg.Any<ITextFormatProcessor>(), Arg.Any<ITextObjectFactory>());

                    _mockEngine.AssetBank.Get<ISpriteFont>("TypewriterNormalFont");
                    mockTextBuilder.RegisterFont("Normal", Arg.Any<ISpriteFont>());

                    _mockEngine.AssetBank.Get<ISpriteFont>("TypewriterSmallFont");
                    mockTextBuilder.RegisterFont("Small", Arg.Any<ISpriteFont>());

                    _mockEngine.AssetBank.Get<ISpriteFont>("TypewriterTitleFont");
                    mockTextBuilder.RegisterFont("Title", Arg.Any<ISpriteFont>());

                    mockTextBuilder.GetRegisteredFont("Normal");
                    mockTextBuilder.DefaultFont = Arg.Any<ISpriteFont>();
                }
            );
        }

        [TestMethod]
        public void InitializeShouldSetupOkButton()
        {
            _mockEngine.ScreenManager.World.Returns(new Point(600, 500));

            RectangleF expectedBounds = new RectangleF(600 - 10 - 200, 500 - 10 - 75, 200, 75);

            ISimpleButton mockButton = Substitute.For<ISimpleButton>();
            _mockGameObjectFactory.CreateSimpleButton(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>(), Arg.Any<ISpriteFont>()).Returns(mockButton);

            _sut.Initialize();

            Received.InOrder(
                () =>
                {
                    _mockEngine.AssetBank.Get<ISpriteFont>("MainButtonFont");
                    _mockGameObjectFactory.CreateSimpleButton(_mockEngine, expectedBounds, Arg.Any<ISpriteBatch>(), Arg.Any<ISpriteFont>());
                    mockButton.Text = "Ok";
                    mockButton.BackColour = new Color(32, 32, 32, 225);
                    mockButton.Tapped += Arg.Any<EventHandler<ButtonEventArgs>>();

                    _mockUILayer.AddEntity(mockButton);
                }
            );
        }

        [TestMethod]
        public void InitializeShouldCreateMissionDebriefHistoryMarksBuilder()
        {
            _sut.Initialize();

            Received.InOrder(
                () =>
                {
                    _mockGameObjectFactory.CreateRandom();
                    _mockGameObjectFactory.CreateTextureEntityFactory();

                    _mockGameObjectFactory.CreateMissionDebriefAttemptRecordMarksBuilder(_mockEngine, Arg.Any<IRandom>(), Arg.Any<ITextureEntityFactory>());

                    _mockGameObjectFactory.CreateMissionDebriefHistoryMarksBuilder(_mockEngine, Arg.Any<IRandom>(), Arg.Any<ITextureEntityFactory>(), Arg.Any<IMissionDebriefAttemptRecordMarksBuilder>());
                }
            );
        }

        [TestMethod]
        public void InitializeShouldSetUILayerEnabledToFalse()
        {
            _sut.Initialize();

            _mockUILayer.Received(1).Enabled = false;
        }

        [TestMethod]
        public void InitializeShouldSetUILayerVisibleToFalse()
        {
            _sut.Initialize();

            _mockUILayer.Received(1).Visible = false;
        }

        [TestMethod]
        public void InitializeShouldCreatePhotoOnBackgroundLayer_Device()
        {
            Vector2 pos = new Vector2(409, 279);
            SizeF size = new SizeF(171, 170);
            ITexture2D mockTexture = Substitute.For<ITexture2D>();
            _mockEngine.AssetBank.Get<ITexture2D>("DebriefPhoto_Default_Device").Returns(mockTexture);

            ITextureEntity mockEntity = Substitute.For<ITextureEntity>();
            _mockGameObjectFactory.CreateTextureEntity(_mockEngine, new RectangleF(pos, size), _mockBGLayer.MainSpriteBatch, mockTexture, Color.White).Returns(mockEntity);

            _sut.Initialize();

            _mockBGLayer.Received(1).AddEntity(mockEntity);
        }

        [TestMethod]
        public void InitializeShouldCreatePhotoOnBackgroundLayer_Objective()
        {
            Vector2 pos = new Vector2(182, 417);
            SizeF size = new SizeF(171, 170);
            ITexture2D mockTexture = Substitute.For<ITexture2D>();
            _mockEngine.AssetBank.Get<ITexture2D>("DebriefPhoto_Default_Objective").Returns(mockTexture);

            ITextureEntity mockEntity = Substitute.For<ITextureEntity>();
            _mockGameObjectFactory.CreateTextureEntity(_mockEngine, new RectangleF(pos, size), _mockBGLayer.MainSpriteBatch, mockTexture, Color.White).Returns(mockEntity);

            _sut.Initialize();

            _mockBGLayer.Received(1).AddEntity(mockEntity);
        }

        [TestMethod]
        public void InitializeShouldCreatePhotoOnBackgroundLayer_Setting()
        {
            Vector2 pos = new Vector2(179, 91);
            SizeF size = new SizeF(171, 170);
            ITexture2D mockTexture = Substitute.For<ITexture2D>();
            _mockEngine.AssetBank.Get<ITexture2D>("DebriefPhoto_Default_Setting").Returns(mockTexture);

            ITextureEntity mockEntity = Substitute.For<ITextureEntity>();
            _mockGameObjectFactory.CreateTextureEntity(_mockEngine, new RectangleF(pos, size), _mockBGLayer.MainSpriteBatch, mockTexture, Color.White).Returns(mockEntity);

            _sut.Initialize();

            _mockBGLayer.Received(1).AddEntity(mockEntity);
        }
        #endregion

        #region Reset Tests
        [TestMethod]
        public void ResetShouldSetUpConsoleWindow()
        {
            RectangleF expectedBounds = new RectangleF(662, 62, 464, 600);
            expectedBounds.Inflate(-25, -25);

            ITextObjectBuilder mockTextBuilder = Substitute.For<ITextObjectBuilder>();
            _mockGameObjectFactory.CreateTextObjectBuilder(Arg.Any<ITextFormatProcessor>(), Arg.Any<ITextObjectFactory>()).Returns(mockTextBuilder);

            ITextObjectRenderer mockTextRenderer = Substitute.For<ITextObjectRenderer>();
            _mockGameObjectFactory.CreateTextObjectRenderer(Arg.Any<IRenderTargetFactory>(), Arg.Any<IGrfxDevice>(), Arg.Any<ISpriteBatch>(), Arg.Any<Rectangle>()).Returns(mockTextRenderer);

            IConsoleWindow mockConsoleWindow = Substitute.For<IConsoleWindow>();
            _mockGameObjectFactory.CreateConsoleWindow(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>(), Arg.Any<ITextObjectBuilder>(), Arg.Any<ITextObjectRenderer>(), Arg.Any<bool>()).Returns(mockConsoleWindow);

            _sut.Initialize();
            _mockGameObjectFactory.ClearReceivedCalls();
            _mockTextLayer.ClearReceivedCalls();
            _mockEngine.GrfxFactory.SpriteBatchFactory.ClearReceivedCalls();
            mockConsoleWindow.ClearReceivedCalls();
            mockTextRenderer.ClearReceivedCalls();

            _sut.Reset();

            Received.InOrder(
                () =>
                {
                    _mockTextLayer.ClearEntities();

                    _mockEngine.GrfxFactory.SpriteBatchFactory.Create(Arg.Any<IGrfxDevice>());
                    _mockGameObjectFactory.CreateTextObjectRenderer(_mockEngine.GrfxFactory.RenderTargetFactory, _mockEngine.Graphics.GraphicsDevice, Arg.Any<ISpriteBatch>(), (Rectangle)expectedBounds);

                    _mockGameObjectFactory.CreateConsoleWindow(_mockEngine, expectedBounds, Arg.Any<ISpriteBatch>(), mockTextBuilder, mockTextRenderer, false);

                    mockConsoleWindow.AutoAdvanceOnTap = false;
                    mockConsoleWindow.ShowBackground = false;
                    mockConsoleWindow.Tapped += Arg.Any<EventHandler<ButtonEventArgs>>();
                    mockConsoleWindow.WindowStateChanged += Arg.Any<EventHandler<StateChangedEventArgs<WindowState>>>();

                    mockConsoleWindow.SetTextFormat(Arg.Any<string>());

                    _mockTextLayer.AddEntity(mockConsoleWindow);
                }
            );
        }

        [TestMethod]
        public void ResetShouldClearMarksLayer()
        {
            _sut.Initialize();

            _sut.Reset();

            _mockMarksLayer.Received(1).ClearEntities();
        }

        [TestMethod]
        public void ResetShouldSetMarksLayerToInvisible()
        {
            _sut.Initialize();

            _sut.Reset();

            _mockMarksLayer.Received(1).Visible = false;
        }

        [TestMethod]
        public void ResetShouldCreateAttemptHistoryMarks()
        {
            IMissionDebriefHistoryMarksBuilder mockBuilder = Substitute.For<IMissionDebriefHistoryMarksBuilder>();
            _mockGameObjectFactory.CreateMissionDebriefHistoryMarksBuilder(Arg.Any<IEngine>(), Arg.Any<IRandom>(), Arg.Any<ITextureEntityFactory>(), Arg.Any<IMissionDebriefAttemptRecordMarksBuilder>()).Returns(mockBuilder);
            ISpriteBatch mockSpriteBatch = Substitute.For<ISpriteBatch>();
            _mockMarksLayer.MainSpriteBatch.Returns(mockSpriteBatch);

            _sut.Initialize();

            _sut.Reset();

            mockBuilder.Received(1).CreateAttemptHistoryMarks(_mockDeviceModel, new Vector2(717, 308), mockSpriteBatch);
        }

        [TestMethod]
        public void ResetShouldAddAttemptHistoryMarksToMarksLayer()
        {
            IMissionDebriefHistoryMarksBuilder mockBuilder = Substitute.For<IMissionDebriefHistoryMarksBuilder>();
            _mockGameObjectFactory.CreateMissionDebriefHistoryMarksBuilder(Arg.Any<IEngine>(), Arg.Any<IRandom>(), Arg.Any<ITextureEntityFactory>(), Arg.Any<IMissionDebriefAttemptRecordMarksBuilder>()).Returns(mockBuilder);
            ISpriteBatch mockSpriteBatch = Substitute.For<ISpriteBatch>();
            _mockMarksLayer.MainSpriteBatch.Returns(mockSpriteBatch);
            _sut.Initialize();

            ITextureEntity mockEntityA = Substitute.For<ITextureEntity>();
            ITextureEntity mockEntityB = Substitute.For<ITextureEntity>();
            mockBuilder.CreateAttemptHistoryMarks(Arg.Any<IDeviceModel>(), Arg.Any<Vector2>(), Arg.Any<ISpriteBatch>()).Returns(
                new ITextureEntity[] { mockEntityA, mockEntityB }
            );

            _sut.Reset();

            Received.InOrder(
                () =>
                {
                    _mockMarksLayer.AddEntity(mockEntityA);
                    _mockMarksLayer.AddEntity(mockEntityB);
                }
            );
        }

        [TestMethod]
        public void ResetShouldCreateFinalCodeMarks()
        {
            IMissionDebriefHistoryMarksBuilder mockBuilder = Substitute.For<IMissionDebriefHistoryMarksBuilder>();
            _mockGameObjectFactory.CreateMissionDebriefHistoryMarksBuilder(Arg.Any<IEngine>(), Arg.Any<IRandom>(), Arg.Any<ITextureEntityFactory>(), Arg.Any<IMissionDebriefAttemptRecordMarksBuilder>()).Returns(mockBuilder);
            ISpriteBatch mockSpriteBatch = Substitute.For<ISpriteBatch>();
            _mockMarksLayer.MainSpriteBatch.Returns(mockSpriteBatch);

            _sut.Initialize();

            _sut.Reset();

            mockBuilder.Received(1).CreateFinalCodeMarks(_mockDeviceModel, new Vector2(805, 444), mockSpriteBatch);
        }

        [TestMethod]
        public void ResetShouldAddFinalCodeMarksToMarksLayer()
        {
            IMissionDebriefHistoryMarksBuilder mockBuilder = Substitute.For<IMissionDebriefHistoryMarksBuilder>();
            _mockGameObjectFactory.CreateMissionDebriefHistoryMarksBuilder(Arg.Any<IEngine>(), Arg.Any<IRandom>(), Arg.Any<ITextureEntityFactory>(), Arg.Any<IMissionDebriefAttemptRecordMarksBuilder>()).Returns(mockBuilder);

            ITextureEntity mockEntityA = Substitute.For<ITextureEntity>();
            ITextureEntity mockEntityB = Substitute.For<ITextureEntity>();
            mockBuilder.CreateFinalCodeMarks(Arg.Any<IDeviceModel>(), Arg.Any<Vector2>(), Arg.Any<ISpriteBatch>()).Returns(
                new ITextureEntity[] { mockEntityA, mockEntityB }
            );

            _sut.Initialize();

            _sut.Reset();

            Received.InOrder(
                () =>
                {
                    _mockMarksLayer.AddEntity(mockEntityA);
                    _mockMarksLayer.AddEntity(mockEntityB);
                }
            );
        }

        [TestMethod]
        public void ResetShouldCreateSignatureTextureEntity()
        {
            _sut.Initialize();

            ITexture2D mockTexture = Substitute.For<ITexture2D>();
            _mockEngine.AssetBank.Get<ITexture2D>("MissionDebriefSignature").Returns(mockTexture);
            mockTexture.Width.Returns(100);
            mockTexture.Height.Returns(50);

            ISpriteBatch mockSpriteBatch = Substitute.For<ISpriteBatch>();
            _mockMarksLayer.MainSpriteBatch.Returns(mockSpriteBatch);

            ITextureEntity mockTextureEntity = Substitute.For<ITextureEntity>();
            _mockGameObjectFactory.CreateTextureEntity(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>(), mockTexture, Arg.Any<Color>()).Returns(mockTextureEntity);

            _sut.Reset();

            Received.InOrder(
                () =>
                {
                    _mockEngine.AssetBank.Get<ITexture2D>("MissionDebriefSignature");
                    _mockGameObjectFactory.CreateTextureEntity(
                        _mockEngine,
                        new RectangleF(1014 - 100 / 2, 540.5f - 50 / 2, 100, 50),
                        mockSpriteBatch,
                        mockTexture,
                        Color.White
                    );
                    _mockMarksLayer.AddEntity(mockTextureEntity);
                }
            );
        }

        [TestMethod]
        public void ResetShouldSetUILayerEnabledToFalse()
        {
            _sut.Initialize();
            _mockUILayer.ClearReceivedCalls();

            _sut.Reset();

            _mockUILayer.Received(1).Enabled = false;
        }

        [TestMethod]
        public void ResetShouldSetUILayerVisibleToFalse()
        {
            _sut.Initialize();
            _mockUILayer.ClearReceivedCalls();

            _sut.Reset();

            _mockUILayer.Received(1).Visible = false;
        }
        #endregion

        #region Ok Button Tap
        [TestMethod]
        public void OkButtonTappedShouldCreateTransitionScene()
        {
            IScene mockScene = Substitute.For<IScene>();
            Dictionary<string, IScene> scenes = new Dictionary<string, IScene>();
            scenes.Add("MainMenu", mockScene);
            _mockEngine.Scenes.Returns(scenes);
            ISimpleButton mockButton = Substitute.For<ISimpleButton>();
            _mockGameObjectFactory.CreateSimpleButton(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>(), Arg.Any<ISpriteFont>()).Returns(mockButton);

            _sut.Initialize();

            mockButton.Tapped += Raise.Event<EventHandler<ButtonEventArgs>>(mockButton, new ButtonEventArgs(Vector2.Zero));

            _mockGameObjectFactory.Received(1).CreateFadeSceneTransition(_mockEngine, _sut, mockScene, FadeSceneTransitionMode.Out, CodeLogicEngine.Constants.FadeSceneTransitionTime);
        }

        [TestMethod]
        public void OkButtonTappedShouldChangeSceneToMainMenuTransition()
        {
            IScene mockScene = Substitute.For<IScene>();
            Dictionary<string, IScene> scenes = new Dictionary<string, IScene>();
            scenes.Add("MainMenu", mockScene);
            _mockEngine.Scenes.Returns(scenes);
            ISimpleButton mockButton = Substitute.For<ISimpleButton>();
            _mockGameObjectFactory.CreateSimpleButton(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>(), Arg.Any<ISpriteFont>()).Returns(mockButton);

            IScene mockTransitionScene = Substitute.For<IScene>();
            _mockGameObjectFactory.CreateFadeSceneTransition(_mockEngine, _sut, mockScene, FadeSceneTransitionMode.Out, CodeLogicEngine.Constants.FadeSceneTransitionTime).Returns(mockTransitionScene);

            _sut.Initialize();

            mockButton.Tapped += Raise.Event<EventHandler<ButtonEventArgs>>(mockButton, new ButtonEventArgs(Vector2.Zero));

            Assert.AreEqual<IScene>(mockTransitionScene, _sut.NextScene);
        }

        [TestMethod]
        public void OkButtonTappedShouldSetSceneComplete()
        {
            IScene mockScene = Substitute.For<IScene>();
            Dictionary<string, IScene> scenes = new Dictionary<string, IScene>();
            scenes.Add("MainMenu", mockScene);
            _mockEngine.Scenes.Returns(scenes);
            ISimpleButton mockButton = Substitute.For<ISimpleButton>();
            _mockGameObjectFactory.CreateSimpleButton(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>(), Arg.Any<ISpriteFont>()).Returns(mockButton);
            _sut.Initialize();

            mockButton.Tapped += Raise.Event<EventHandler<ButtonEventArgs>>(mockButton, new ButtonEventArgs(Vector2.Zero));

            Assert.IsTrue(_sut.SceneComplete);
        }

        [TestMethod]
        public void OkButtonTappedShouldPlayExpectedSound()
        {
            ISoundResource mockSound = Substitute.For<ISoundResource>();
            _mockEngine.AssetBank.Get<ISoundResource>("ButtonPress").Returns(mockSound);

            IScene mockScene = Substitute.For<IScene>();
            Dictionary<string, IScene> scenes = new Dictionary<string, IScene>();
            scenes.Add("MainMenu", mockScene);
            _mockEngine.Scenes.Returns(scenes);
            ISimpleButton mockButton = Substitute.For<ISimpleButton>();
            _mockGameObjectFactory.CreateSimpleButton(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>(), Arg.Any<ISpriteFont>()).Returns(mockButton);

            _sut.Initialize();

            mockButton.Tapped += Raise.Event<EventHandler<ButtonEventArgs>>(mockButton, new ButtonEventArgs(Vector2.Zero));

            mockSound.Received(1).Play();
        }
        #endregion

        #region Window State Changed Tests
        [TestMethod]
        public void ConsoleWindowStateChangedNotProcessingToWaitingShouldNotSetMarksLayerVisibleToTrue()
        {
            // Just do one test here... should be good enough.
            IConsoleWindow mockConsoleWindow = Substitute.For<IConsoleWindow>();
            _mockGameObjectFactory.CreateConsoleWindow(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>(), Arg.Any<ITextObjectBuilder>(), Arg.Any<ITextObjectRenderer>(), Arg.Any<bool>()).Returns(mockConsoleWindow);

            _sut.Initialize();
            _sut.Reset();
            _mockMarksLayer.ClearReceivedCalls();

            mockConsoleWindow.WindowStateChanged += Raise.Event<EventHandler<StateChangedEventArgs<WindowState>>>(mockConsoleWindow, new StateChangedEventArgs<WindowState>(WindowState.Opening, WindowState.Processing));

            _mockMarksLayer.Received(0).Visible = true;
        }

        [TestMethod]
        public void ConsoleWindowStateChangedFromProcessingToWaitingShouldSetMarksLayerVisibleToTrue()
        {
            IConsoleWindow mockConsoleWindow = Substitute.For<IConsoleWindow>();
            _mockGameObjectFactory.CreateConsoleWindow(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>(), Arg.Any<ITextObjectBuilder>(), Arg.Any<ITextObjectRenderer>(), Arg.Any<bool>()).Returns(mockConsoleWindow);

            _sut.Initialize();
            _sut.Reset();
            _mockMarksLayer.ClearReceivedCalls();

            mockConsoleWindow.WindowStateChanged += Raise.Event<EventHandler<StateChangedEventArgs<WindowState>>>(mockConsoleWindow, new StateChangedEventArgs<WindowState>(WindowState.Processing, WindowState.Waiting));

            _mockMarksLayer.Received(1).Visible = true;
        }

        [TestMethod]
        public void ConsoleWindowStateChangedFromProcessingToWaitingShouldSetUILayerVisibleToTrue()
        {
            IConsoleWindow mockConsoleWindow = Substitute.For<IConsoleWindow>();
            _mockGameObjectFactory.CreateConsoleWindow(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>(), Arg.Any<ITextObjectBuilder>(), Arg.Any<ITextObjectRenderer>(), Arg.Any<bool>()).Returns(mockConsoleWindow);

            _sut.Initialize();
            _sut.Reset();
            _mockMarksLayer.ClearReceivedCalls();

            mockConsoleWindow.WindowStateChanged += Raise.Event<EventHandler<StateChangedEventArgs<WindowState>>>(mockConsoleWindow, new StateChangedEventArgs<WindowState>(WindowState.Processing, WindowState.Waiting));

            _mockUILayer.Received(1).Visible = true;
        }

        [TestMethod]
        public void ConsoleWindowStateChangedFromProcessingToWaitingShouldSetUILayerEnabledToTrue()
        {
            IConsoleWindow mockConsoleWindow = Substitute.For<IConsoleWindow>();
            _mockGameObjectFactory.CreateConsoleWindow(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>(), Arg.Any<ITextObjectBuilder>(), Arg.Any<ITextObjectRenderer>(), Arg.Any<bool>()).Returns(mockConsoleWindow);

            _sut.Initialize();
            _sut.Reset();
            _mockMarksLayer.ClearReceivedCalls();

            mockConsoleWindow.WindowStateChanged += Raise.Event<EventHandler<StateChangedEventArgs<WindowState>>>(mockConsoleWindow, new StateChangedEventArgs<WindowState>(WindowState.Processing, WindowState.Waiting));

            _mockUILayer.Received(1).Enabled = true;
        }
        #endregion

        #region Start Tests
        [TestMethod]
        public void StartShouldBeginPlayingMusic()
        {
            ISong mockSong = Substitute.For<ISong>();
            _mockEngine.AssetBank.Get<ISong>("Debriefing").Returns(mockSong);

            ICodeLogicSettings mockSettings = Substitute.For<ICodeLogicSettings>();
            mockSettings.MusicVolume.Returns(0.123f);
            _mockEngine.GetComponent<ICodeLogicSettings>().Returns(mockSettings);

            _sut.Start();

            Received.InOrder(
                () =>
                {
                    _mockEngine.AudioManager.MusicController.VolumeAttenuation = 0f;
                    _mockEngine.AudioManager.MusicController.Play(mockSong);
                    _mockEngine.AudioManager.MusicController.FadeVolumeAttenuation(0.123f, CodeLogicEngine.Constants.FadeSceneTransitionTime);
                }
            );
        }
        #endregion

        #region End Tests
        [TestMethod]
        public void EndShouldCallAudioManagerFadeMusicVolume()
        {
            _sut.End();

            _mockEngine.AudioManager.Received(1).MusicController.FadeVolumeAttenuation(0, CodeLogicEngine.Constants.FadeSceneTransitionTime);
        }

        [TestMethod]
        public void EndWithOutstandingConfigChangesShouldPersistCache()
        {
            _mockSettings.CacheChanged.Returns(true);

            _sut.End();

            _mockSettings.Received(1).PersistCache();
        }

        [TestMethod]
        public void EndWithoutOutstandingConfigChangesShouldNotPersistCache()
        {
            _mockSettings.CacheChanged.Returns(false);

            _sut.End();

            _mockSettings.Received(0).PersistCache();
        }
        #endregion

        #region External Action Occurred Event Tests
        [TestMethod]
        public void ExternalActionOccurredWithTextNotWaitingShouldCallConsoleWindowFlushText()
        {
            _mockEngine.CurrentScene.Returns(_sut);
            IConsoleWindow mockConsoleWindow = Substitute.For<IConsoleWindow>();
            mockConsoleWindow.WindowState.Returns(WindowState.Processing);
            _mockGameObjectFactory.CreateConsoleWindow(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>(), Arg.Any<ITextObjectBuilder>(), Arg.Any<ITextObjectRenderer>(), Arg.Any<bool>()).Returns(mockConsoleWindow);
            _sut.Initialize();
            _sut.Reset();

            _mockEngine.ExternalActionOccurred += Raise.Event<EventHandler<ExternalActionEventArgs>>(_mockEngine, new ExternalActionEventArgs(CodeLogicEngine.Constants.ExternalActions.BackPressed));

            mockConsoleWindow.Received(1).FlushText();
        }

        [TestMethod]
        public void ExternalActionOccurredWithTextWaitingShouldChangeScene()
        {
            _mockEngine.CurrentScene.Returns(_sut);
            IConsoleWindow mockConsoleWindow = Substitute.For<IConsoleWindow>();
            mockConsoleWindow.WindowState.Returns(WindowState.Waiting);
            _mockGameObjectFactory.CreateConsoleWindow(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>(), Arg.Any<ITextObjectBuilder>(), Arg.Any<ITextObjectRenderer>(), Arg.Any<bool>()).Returns(mockConsoleWindow);
            _sut.Initialize();
            _sut.Reset();

            Dictionary<string, IScene> scenes = new Dictionary<string, IScene>();
            scenes.Add("MainMenu", Substitute.For<IScene>());
            _mockEngine.Scenes.Returns(scenes);

            IScene mockTransitionScene = Substitute.For<IScene>();
            _mockGameObjectFactory.CreateFadeSceneTransition(_mockEngine, _sut, scenes["MainMenu"], FadeSceneTransitionMode.Out, CodeLogicEngine.Constants.FadeSceneTransitionTime).Returns(mockTransitionScene);

            _mockEngine.ExternalActionOccurred += Raise.Event<EventHandler<ExternalActionEventArgs>>(_mockEngine, new ExternalActionEventArgs(CodeLogicEngine.Constants.ExternalActions.BackPressed));

            Assert.AreSame(mockTransitionScene, _sut.NextScene);
            Assert.IsTrue(_sut.SceneComplete);
        }

        [TestMethod]
        public void ExternalActionOccurredShouldCheckConsoleWindowWindowState()
        {
            _mockEngine.CurrentScene.Returns(_sut);
            IConsoleWindow mockConsoleWindow = Substitute.For<IConsoleWindow>();
            mockConsoleWindow.WindowState.Returns(WindowState.Processing);
            _mockGameObjectFactory.CreateConsoleWindow(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>(), Arg.Any<ITextObjectBuilder>(), Arg.Any<ITextObjectRenderer>(), Arg.Any<bool>()).Returns(mockConsoleWindow);
            _sut.Initialize();
            _sut.Reset();

            _mockEngine.ExternalActionOccurred += Raise.Event<EventHandler<ExternalActionEventArgs>>(_mockEngine, new ExternalActionEventArgs(CodeLogicEngine.Constants.ExternalActions.BackPressed));

            var x = mockConsoleWindow.Received(1).WindowState;
        }

        [TestMethod]
        public void ExternalActionOccurredWithoutBackPressedShouldNotCheckWindowState()
        {
            _mockEngine.CurrentScene.Returns(_sut);
            IConsoleWindow mockConsoleWindow = Substitute.For<IConsoleWindow>();
            mockConsoleWindow.WindowState.Returns(WindowState.Processing);
            _mockGameObjectFactory.CreateConsoleWindow(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>(), Arg.Any<ITextObjectBuilder>(), Arg.Any<ITextObjectRenderer>(), Arg.Any<bool>()).Returns(mockConsoleWindow);
            _sut.Initialize();
            _sut.Reset();

            _mockEngine.ExternalActionOccurred += Raise.Event<EventHandler<ExternalActionEventArgs>>(_mockEngine, new ExternalActionEventArgs("NOT BACK PRESSED"));

            var x = mockConsoleWindow.Received(0).WindowState;
        }

        [TestMethod]
        public void ExternalActionOccurredWhenNotCurrentSceneShouldNotCheckWindowState()
        {
            _mockEngine.CurrentScene.Returns(Substitute.For<IScene>());
            IConsoleWindow mockConsoleWindow = Substitute.For<IConsoleWindow>();
            mockConsoleWindow.WindowState.Returns(WindowState.Processing);
            _mockGameObjectFactory.CreateConsoleWindow(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>(), Arg.Any<ITextObjectBuilder>(), Arg.Any<ITextObjectRenderer>(), Arg.Any<bool>()).Returns(mockConsoleWindow);
            _sut.Initialize();
            _sut.Reset();

            _mockEngine.ExternalActionOccurred += Raise.Event<EventHandler<ExternalActionEventArgs>>(_mockEngine, new ExternalActionEventArgs(CodeLogicEngine.Constants.ExternalActions.BackPressed));

            var x = mockConsoleWindow.Received(0).WindowState;
        }
        #endregion
    }
}
