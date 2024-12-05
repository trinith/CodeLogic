using ArbitraryPixel.CodeLogic.Common;
using ArbitraryPixel.CodeLogic.Common.Config;
using ArbitraryPixel.CodeLogic.Common.Entities;
using ArbitraryPixel.CodeLogic.Common.Model;
using ArbitraryPixel.CodeLogic.Common.Scenes;
using ArbitraryPixel.CodeLogic.Common.Theme;
using ArbitraryPixel.CodeLogic.Common.Theme.Device;
using ArbitraryPixel.Common.Audio;
using ArbitraryPixel.Common.Drawing;
using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Common.Graphics.Factory;
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

namespace ArbitraryPixel.CodeLogic.Common_Tests.Scenes
{
    [TestClass]
    public class DeviceBootScene_Tests : UnitTestBase<DeviceBootScene>
    {
        private IEngine _mockEngine;
        private IDeviceModel _mockDeviceModel;
        private ILogPanelModel _mockPanelModel;
        private IThemeManagerCollection _mockThemeCollection;
        private GameObjectFactory _mockGameObjectFactory;

        private ILayer _mockLayer;
        private ISpriteBatch _mockLayerSpriteBatch;
        private ISpriteBatch _mockTextRendererSpriteBatch;
        
        protected override void OnInitializing()
        {
            base.OnInitializing();

            _mockEngine = Substitute.For<IEngine>();
            _mockDeviceModel = Substitute.For<IDeviceModel>();
            _mockPanelModel = Substitute.For<ILogPanelModel>();

            _mockThemeCollection = Substitute.For<IThemeManagerCollection>();
            _mockEngine.GetComponent<IThemeManagerCollection>().Returns(_mockThemeCollection);

            _mockGameObjectFactory = Substitute.For<GameObjectFactory>();
            GameObjectFactory.SetInstance(_mockGameObjectFactory);

            _mockEngine.GrfxFactory.SpriteBatchFactory.Create(Arg.Any<IGrfxDevice>()).Returns(
                _mockLayerSpriteBatch = Substitute.For<ISpriteBatch>(),
                _mockTextRendererSpriteBatch = Substitute.For<ISpriteBatch>()
            );

            _mockGameObjectFactory.CreateGenericLayer(Arg.Any<IEngine>(), Arg.Any<ISpriteBatch>(), Arg.Any<SpriteSortMode>(), Arg.Any<BlendState>(), Arg.Any<SamplerState>()).Returns(
                _mockLayer = Substitute.For<ILayer>()
            );
            _mockLayer.MainSpriteBatch.Returns(_mockLayerSpriteBatch);
        }

        protected override DeviceBootScene OnCreateSUT()
        {
            return new DeviceBootScene(_mockEngine, _mockDeviceModel, _mockPanelModel);
        }

        #region Constructor Tests
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullParameterShouldThrowException_DeviceModel()
        {
            _sut = new DeviceBootScene(_mockEngine, null, _mockPanelModel);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullParameterShouldThrowException_PanelModel()
        {
            _sut = new DeviceBootScene(_mockEngine, _mockDeviceModel, null);
        }

        [TestMethod]
        public void ConstructShouldRequestCurrentTheme()
        {
            _mockThemeCollection[ThemeObjectType.Device].Received(1).GetCurrentTheme<IDeviceTheme>();
        }
        #endregion

        #region Console Window Dispose Tests
        [TestMethod]
        public void CreatedConsoleWindowDisposeShouldSetNextScene()
        {
            IConsoleWindow mockConsoleWindow = Substitute.For<IConsoleWindow>();
            _mockGameObjectFactory.CreateConsoleWindow(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>(), Arg.Any<ITextObjectBuilder>(), Arg.Any<ITextObjectRenderer>()).Returns(mockConsoleWindow);
            _sut.Reset();

            IScene mockScene = Substitute.For<IScene>();
            Dictionary<string, IScene> mockScenes = new Dictionary<string, IScene>();
            mockScenes.Add("DeviceMain", mockScene);
            _mockEngine.Scenes.Returns(mockScenes);

            mockConsoleWindow.Disposed += Raise.Event<EventHandler<EventArgs>>();

            Assert.AreSame(mockScene, _sut.NextScene);
        }

        [TestMethod]
        public void CreatedConsoleWindowDisposeShouldSceneCompleteToTrue()
        {
            IConsoleWindow mockConsoleWindow = Substitute.For<IConsoleWindow>();
            _mockGameObjectFactory.CreateConsoleWindow(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>(), Arg.Any<ITextObjectBuilder>(), Arg.Any<ITextObjectRenderer>()).Returns(mockConsoleWindow);
            _sut.Reset();

            IScene mockScene = Substitute.For<IScene>();
            Dictionary<string, IScene> mockScenes = new Dictionary<string, IScene>();
            mockScenes.Add("DeviceMain", mockScene);
            _mockEngine.Scenes.Returns(mockScenes);

            mockConsoleWindow.Disposed += Raise.Event<EventHandler<EventArgs>>();

            Assert.IsTrue(_sut.SceneComplete);
        }
        #endregion

        #region Reset Tests
        [TestMethod]
        public void ResetShouldCallModelReset()
        {
            _sut.Reset();

            _mockDeviceModel.Received(1).Reset();
        }

        [TestMethod]
        public void ResetShouldCreateDeviceBackground()
        {
            _mockEngine.ScreenManager.World.Returns(new Point(100, 100));

            _sut.Reset();

            _mockGameObjectFactory.Received(1).CreateDeviceBackground(_mockEngine, new RectangleF(0, 0, 100, 100), _mockLayerSpriteBatch);
        }

        [TestMethod]
        public void ResetShouldCallAddEntityWithDeviceBackground()
        {
            IDeviceBackground mockBackground = Substitute.For<IDeviceBackground>();
            _mockGameObjectFactory.CreateDeviceBackground(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>()).Returns(mockBackground);

            _sut.Reset();

            _mockLayer.Received(1).AddEntity(mockBackground);
        }

        [TestMethod]
        public void ResetShouldCreateConsoleWindow()
        {
            IConsoleWindow mockWindow = Substitute.For<IConsoleWindow>();
            _mockGameObjectFactory.CreateConsoleWindow(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>(), Arg.Any<ITextObjectBuilder>(), Arg.Any<ITextObjectRenderer>()).Returns(mockWindow);

            _mockEngine.ScreenManager.World.Returns(new Point(100, 100));
            _sut.Reset();

            // Confirm call chain going into CreateConsoleWindow
            Received.InOrder(
                () =>
                {
                    // Dependencies for TextFormatProcessor
                    _mockGameObjectFactory.CreateTextFormatValueHandlerManager();

                    // Dependencies for TextObjectBuilder
                    _mockGameObjectFactory.CreateTextFormatProcessor(Arg.Any<ITextFormatValueHandlerManager>());
                    _mockGameObjectFactory.CreateTextObjectFactory();

                    // Dependency for CreateConsoleWindow
                    _mockGameObjectFactory.CreateTextObjectBuilderWithConsoleFonts(Arg.Any<ITextFormatProcessor>(), Arg.Any<ITextObjectFactory>(), _mockEngine.AssetBank);

                    // Dependency for CreateConsoleWindow
                    _mockGameObjectFactory.Received(1).CreateTextObjectRenderer(Arg.Any<IRenderTargetFactory>(), Arg.Any<IGrfxDevice>(), _mockTextRendererSpriteBatch, new Rectangle(17, 17, 66, 66));

                    _mockGameObjectFactory.Received(1).CreateConsoleWindow(
                        _mockEngine,
                        new RectangleF(5, 5, 90, 90),
                        _mockLayerSpriteBatch,
                        Arg.Any<ITextObjectBuilder>(),
                        Arg.Any<ITextObjectRenderer>()
                    );

                    mockWindow.BorderSize = new SizeF(2);
                    mockWindow.Padding = new SizeF(10);
                }
            );
        }

        [TestMethod]
        public void ResetShouldCreateExpectedNumberOfSpriteBatchObjects()
        {
            /* Expected SpriteBatch Objects:
             *  1 - Layer
             *  2 - TextObjectRenderer
             */

            _sut.Reset();

            _mockEngine.GrfxFactory.SpriteBatchFactory.Received(2).Create(Arg.Any<IGrfxDevice>());
        }

        [TestMethod]
        public void ResetShouldCreateLayerWithExpectedParameters()
        {
            _sut.Reset();

            _mockGameObjectFactory.Received(1).CreateGenericLayer(_mockEngine, _mockLayerSpriteBatch, SpriteSortMode.Deferred, null, SamplerState.PointClamp);
        }

        [TestMethod]
        public void ResetShouldSetDeviceBackgroundColourToThemeBackgroundImageMask()
        {
            IDeviceBackground mockBackground = Substitute.For<IDeviceBackground>();
            _mockGameObjectFactory.CreateDeviceBackground(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>()).Returns(mockBackground);

            _mockThemeCollection[ThemeObjectType.Device].GetCurrentTheme<IDeviceTheme>().BackgroundImageMask.Returns(Color.Pink);

            _sut.Reset();

            mockBackground.Received(1).Colour = Color.Pink;
        }

        [TestMethod]
        public void ResetShouldCreateEventHandlerForConsoleWindowDisposed()
        {
            IConsoleWindow mockConsoleWindow = Substitute.For<IConsoleWindow>();
            _mockGameObjectFactory.CreateConsoleWindow(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>(), Arg.Any<ITextObjectBuilder>(), Arg.Any<ITextObjectRenderer>()).Returns(mockConsoleWindow);

            _sut.Reset();

            mockConsoleWindow.Received(1).Disposed += Arg.Any<EventHandler<EventArgs>>();
        }

        [TestMethod]
        public void ResetShouldSetTextFormatOnConsoleWindow()
        {
            IConsoleWindow mockConsoleWindow = Substitute.For<IConsoleWindow>();
            _mockGameObjectFactory.CreateConsoleWindow(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>(), Arg.Any<ITextObjectBuilder>(), Arg.Any<ITextObjectRenderer>()).Returns(mockConsoleWindow);

            _sut.Reset();

            // Don't care what the contents are, really... just make sure that we set some text.
            mockConsoleWindow.Received(1).SetTextFormat(Arg.Any<string>());
        }
        #endregion

        #region Start Tests
        [TestMethod]
        public void StartShouldPlayGameplayMusic()
        {
            ISong mockSong = Substitute.For<ISong>();
            _mockEngine.AssetBank.Get<ISong>("Gameplay").Returns(mockSong);

            _sut.Start();

            Received.InOrder(
                () =>
                {
                    _mockEngine.AudioManager.MusicController.VolumeAttenuation = 1f;
                    _mockEngine.AudioManager.MusicController.Play(mockSong);
                }
            );
        }
        #endregion
    }
}
