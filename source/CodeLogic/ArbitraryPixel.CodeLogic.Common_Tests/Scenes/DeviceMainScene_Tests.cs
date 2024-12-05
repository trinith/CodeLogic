using ArbitraryPixel.CodeLogic.Common;
using ArbitraryPixel.CodeLogic.Common.Entities;
using ArbitraryPixel.CodeLogic.Common.Layers;
using ArbitraryPixel.CodeLogic.Common.Model;
using ArbitraryPixel.CodeLogic.Common.Scenes;
using ArbitraryPixel.CodeLogic.Common.Theme;
using ArbitraryPixel.CodeLogic.Common.Theme.Device;
using ArbitraryPixel.Common;
using ArbitraryPixel.Common.ContentManagement;
using ArbitraryPixel.Common.Drawing;
using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Platform2D.Engine;
using ArbitraryPixel.Platform2D.Entity;
using ArbitraryPixel.Platform2D.Layer;
using ArbitraryPixel.Platform2D.Scene;
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
    public class DeviceMainScene_Tests : UnitTestBase<DeviceMainScene>
    {
        private IEngine _mockEngine;
        private IDeviceModel _mockDeviceModel;
        private ILogPanelModel _mockPanelModel;

        private IGrfxDevice _mockGrfxDevice;
        private IDeviceBackground _mockDeviceBackground;
        private IContentManager _mockContentManager;
        private ISpriteBatch _mockSpriteBatch;

        private ILayer _mockBGLayer;
        private ICodeInputLayer _mockCILayer;
        private IDeviceMainUILayer _mockDUILayer;
        private ILayer _mockWindowLayer;

        private IRandom _mockRandom;

        private GameObjectFactory _mockGameObjectFactory;

        protected override void OnInitializing()
        {
            base.OnInitializing();

            _mockEngine = Substitute.For<IEngine>();
            _mockDeviceModel = Substitute.For<IDeviceModel>();
            _mockPanelModel = Substitute.For<ILogPanelModel>();

            _mockEngine.Graphics.GraphicsDevice.Returns(_mockGrfxDevice = Substitute.For<IGrfxDevice>());
            _mockEngine.Content.Returns(_mockContentManager = Substitute.For<IContentManager>());
            _mockEngine.ScreenManager.ScaleMatrix.Returns(Matrix.CreateScale(1, 2, 3));
            _mockEngine.ScreenManager.World.Returns(new Point(100, 200));
            _mockEngine.GrfxFactory.SpriteBatchFactory.Create(Arg.Any<IGrfxDevice>()).Returns(_mockSpriteBatch = Substitute.For<ISpriteBatch>());

            IThemeManagerCollection mockThemeCollection = Substitute.For<IThemeManagerCollection>();
            _mockEngine.GetComponent<IThemeManagerCollection>().Returns(mockThemeCollection);
            mockThemeCollection[ThemeObjectType.Device].GetCurrentTheme<IDeviceTheme>().BackgroundImageMask.Returns(Color.Pink);

            GameObjectFactory.SetInstance(_mockGameObjectFactory = Substitute.For<GameObjectFactory>());

            _mockGameObjectFactory.CreateDeviceBackground(_mockEngine, Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>()).Returns(_mockDeviceBackground = Substitute.For<IDeviceBackground>());
            _mockDeviceBackground.Enabled.Returns(true);
            _mockDeviceBackground.Visible.Returns(true);

            _mockGameObjectFactory.CreateGenericLayer(Arg.Any<IEngine>(), Arg.Any<ISpriteBatch>(), Arg.Any<SpriteSortMode>(), Arg.Any<BlendState>(), Arg.Any<SamplerState>()).Returns(_mockBGLayer = Substitute.For<ILayer>());
            List<IEntity> bgEntities = new List<IEntity>();
            _mockBGLayer.Entities.Returns(bgEntities);
            _mockBGLayer.When(x => x.AddEntity(Arg.Any<IEntity>())).Do(x => bgEntities.Add(x[0] as IEntity));
            _mockBGLayer.MainSpriteBatch.Returns(_mockSpriteBatch);
            _mockBGLayer.Enabled.Returns(true);
            _mockBGLayer.Visible.Returns(true);

            _mockGameObjectFactory.CreateGenericLayer(Arg.Any<IEngine>(), Arg.Any<ISpriteBatch>(), Arg.Any<SpriteSortMode>()).Returns(_mockWindowLayer = Substitute.For<ILayer>());
            _mockWindowLayer.Enabled.Returns(true);
            _mockWindowLayer.Visible.Returns(true);

            _mockGameObjectFactory.CreateCodeInputLayer(Arg.Any<IEngine>(), Arg.Any<ISpriteBatch>(), Arg.Any<IDeviceModel>()).Returns(_mockCILayer = Substitute.For<ICodeInputLayer>());
            _mockCILayer.Enabled.Returns(true);
            _mockCILayer.Visible.Returns(true);

            _mockGameObjectFactory.CreateDeviceMainUILayer(Arg.Any<IEngine>(), Arg.Any<ISpriteBatch>(), Arg.Any<IDeviceModel>(), Arg.Any<ILogPanelModel>()).Returns(_mockDUILayer = Substitute.For<IDeviceMainUILayer>());
            _mockDUILayer.Enabled.Returns(true);
            _mockDUILayer.Visible.Returns(true);

            _mockGameObjectFactory.CreateRandom().Returns(_mockRandom = Substitute.For<IRandom>());
        }

        protected override DeviceMainScene OnCreateSUT()
        {
            return new DeviceMainScene(_mockEngine, _mockDeviceModel, _mockPanelModel);
        }

        #region Constructor Tests
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullParameterShouldThrowException_DeviceModel()
        {
            _sut = new DeviceMainScene(_mockEngine, null, _mockPanelModel);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullParameterShouldThrowException_LogPanelModel()
        {
            _sut = new DeviceMainScene(_mockEngine, _mockDeviceModel, null);
        }

        [TestMethod]
        public void ConstructShouldSubscribeToEngineExternalActionOccurredEvent()
        {
            _mockEngine.Received(1).ExternalActionOccurred += Arg.Any<EventHandler<ExternalActionEventArgs>>();
        }
        #endregion

        #region Initialize Tests
        [TestMethod]
        public void InitializeShouldCreateNewSpriteBatch()
        {
            _sut.Initialize();

            _mockEngine.GrfxFactory.SpriteBatchFactory.Received(4).Create(_mockGrfxDevice);
        }

        [TestMethod]
        public void InitializeShouldCreateGenericLayer()
        {
            _sut.Initialize();

            _mockGameObjectFactory.Received(1).CreateGenericLayer(_mockEngine, _mockSpriteBatch, SpriteSortMode.Deferred, null, SamplerState.PointClamp);
        }

        [TestMethod]
        public void InitializeShouldCreateDeviceBackgroundObject()
        {
            _sut.Initialize();

            _mockGameObjectFactory.Received(1).CreateDeviceBackground(_mockEngine, new RectangleF(0, 0, 100, 200), _mockSpriteBatch);
        }

        [TestMethod]
        public void InitializeShouldAddDeviceBackgroundObjectToLayerEntities()
        {
            _sut.Initialize();

            Assert.IsTrue(_mockBGLayer.Entities.Contains(_mockDeviceBackground));
        }

        [TestMethod]
        public void InitializeShouldSetDeviceBackgroundColorToExpectedValue()
        {
            _sut.Initialize();

            _mockDeviceBackground.Received(1).Colour = Color.Pink;
        }

        [TestMethod]
        public void InitializeShouldCreateCodeInputLayer()
        {
            _sut.Initialize();

            _mockGameObjectFactory.Received(1).CreateCodeInputLayer(_mockEngine, _mockSpriteBatch, _mockDeviceModel);
        }

        [TestMethod]
        public void InitializeShouldAddCodeInputLayerToEntities()
        {
            _sut.Initialize();

            Assert.IsTrue(_sut.Entities.Contains(_mockCILayer));
        }

        [TestMethod]
        public void InitializeShouldCreateDeviceUILayer()
        {
            _sut.Initialize();

            _mockGameObjectFactory.Received(1).CreateDeviceMainUILayer(_mockEngine, _mockSpriteBatch, _mockDeviceModel, _mockPanelModel);
        }

        [TestMethod]
        public void InitializeShouldAddDeviceUILayerToEntities()
        {
            _sut.Initialize();

            Assert.IsTrue(_sut.Entities.Contains(_mockDUILayer));
        }

        [TestMethod]
        public void InitializeShouldAddEventHandlerToUILayerSubmitSequence()
        {
            _sut.Initialize();

            _mockDUILayer.Received(1).SubmitSequence += Arg.Any<EventHandler<EventArgs>>();
        }

        [TestMethod]
        public void InitializeShouldSubscribeToUILayerMenuButtonTapped()
        {
            _sut.Initialize();

            _mockDUILayer.Received(1).MenuButtonTapped += Arg.Any<EventHandler<EventArgs>>();
        }

        [TestMethod]
        public void InitializeShouldCreateGenericLayerForWindow()
        {
            _sut.Initialize();

            _mockGameObjectFactory.Received(1).CreateGenericLayer(_mockEngine, _mockSpriteBatch, SpriteSortMode.Deferred);
        }

        [TestMethod]
        public void InitializeShouldAddWindowLayerToEntities()
        {
            _sut.Initialize();

            Assert.IsTrue(_sut.Entities.Contains(_mockWindowLayer));
        }
        #endregion

        #region Update Tests
        [TestMethod]
        public void UpdateShouldCallBGLayerUpdate()
        {
            _sut.Initialize();
            GameTime expectedGT = new GameTime();

            _sut.Update(expectedGT);

            _mockBGLayer.Received(1).Update(expectedGT);
        }

        [TestMethod]
        public void UpdateShouldCallCILayerUpdate()
        {
            _sut.Initialize();
            GameTime expectedGT = new GameTime();

            _sut.Update(expectedGT);

            _mockCILayer.Received(1).Update(expectedGT);
        }

        [TestMethod]
        public void UpdateShouldCallDUILayerUpdate()
        {
            _sut.Initialize();
            GameTime expectedGT = new GameTime();

            _sut.Update(expectedGT);

            _mockDUILayer.Received(1).Update(expectedGT);
        }
        #endregion

        #region Draw Tests
        [TestMethod]
        public void DrawShouldCallBGLayerDraw()
        {
            _sut.Initialize();
            GameTime expectedGT = new GameTime();

            _sut.Draw(expectedGT);

            _mockBGLayer.Received(1).Draw(expectedGT);
        }

        [TestMethod]
        public void DrawShouldCallCILayerDraw()
        {
            _sut.Initialize();
            GameTime expectedGT = new GameTime();

            _sut.Draw(expectedGT);

            _mockCILayer.Received(1).Draw(expectedGT);
        }

        [TestMethod]
        public void DrawShouldCallDUILayerDraw()
        {
            _sut.Initialize();
            GameTime expectedGT = new GameTime();

            _sut.Draw(expectedGT);

            _mockDUILayer.Received(1).Draw(expectedGT);
        }
        #endregion

        #region Reset Tests
        [TestMethod]
        public void ResetShouldSetSceneCompleteToFalse()
        {
            _sut.SceneComplete = true;
            _sut.Reset();

            Assert.IsFalse(_sut.SceneComplete);
        }
        #endregion

        #region ExternalActionOccurred Tests
        [TestMethod]
        public void ExternalActionOccurredWithBackButtonPressedShouldCreateSceneTransition()
        {
            Dictionary<string, IScene> scenes = new Dictionary<string, IScene>();
            scenes.Add("DeviceMenu", Substitute.For<IScene>());
            _mockEngine.Scenes.Returns(scenes);
            _mockEngine.CurrentScene.Returns(_sut);

            _mockEngine.ExternalActionOccurred += Raise.Event<EventHandler<ExternalActionEventArgs>>(_mockEngine, new ExternalActionEventArgs(CodeLogicEngine.Constants.ExternalActions.BackPressed));

            _mockGameObjectFactory.Received(1).CreatePanSceneTransition(_mockEngine, _sut, scenes["DeviceMenu"], PanSceneTransitionMode.PanRight, CodeLogicEngine.Constants.DeviceSceneTransitionTime);
        }

        [TestMethod]
        public void ExternalActionOccurredWithBackButtonPressedShouldChangeScene()
        {
            Dictionary<string, IScene> scenes = new Dictionary<string, IScene>();
            scenes.Add("DeviceMenu", Substitute.For<IScene>());
            _mockEngine.Scenes.Returns(scenes);
            _mockEngine.CurrentScene.Returns(_sut);

            IScene mockTransitionScene = Substitute.For<IScene>();
            _mockGameObjectFactory.CreatePanSceneTransition(_mockEngine, _sut, scenes["DeviceMenu"], PanSceneTransitionMode.PanRight, CodeLogicEngine.Constants.DeviceSceneTransitionTime).Returns(mockTransitionScene);

            _mockEngine.ExternalActionOccurred += Raise.Event<EventHandler<ExternalActionEventArgs>>(_mockEngine, new ExternalActionEventArgs(CodeLogicEngine.Constants.ExternalActions.BackPressed));

            Assert.AreSame(mockTransitionScene, _sut.NextScene);
            Assert.IsTrue(_sut.SceneComplete);
        }

        [TestMethod]
        public void ExternalActionOccurredWhenSceneNotActiveShouldNotCreateSceneTransition()
        {
            Dictionary<string, IScene> scenes = new Dictionary<string, IScene>();
            scenes.Add("DeviceMenu", Substitute.For<IScene>());
            _mockEngine.Scenes.Returns(scenes);
            _mockEngine.CurrentScene.Returns(Substitute.For<IScene>());

            IScene mockTransitionScene = Substitute.For<IScene>();
            _mockGameObjectFactory.CreatePanSceneTransition(_mockEngine, _sut, scenes["DeviceMenu"], PanSceneTransitionMode.PanRight, CodeLogicEngine.Constants.DeviceSceneTransitionTime).Returns(mockTransitionScene);

            _mockEngine.ExternalActionOccurred += Raise.Event<EventHandler<ExternalActionEventArgs>>(_mockEngine, new ExternalActionEventArgs(CodeLogicEngine.Constants.ExternalActions.BackPressed));

            _mockGameObjectFactory.Received(0).CreatePanSceneTransition(Arg.Any<IEngine>(), Arg.Any<IScene>(), Arg.Any<IScene>(), Arg.Any<PanSceneTransitionMode>(), Arg.Any<double>());
        }

        [TestMethod]
        public void ExternalActionOccurredWithAnyOtherDataShouldNotCreateSceneTransition()
        {
            Dictionary<string, IScene> scenes = new Dictionary<string, IScene>();
            scenes.Add("DeviceMenu", Substitute.For<IScene>());
            _mockEngine.Scenes.Returns(scenes);
            _mockEngine.CurrentScene.Returns(_sut);

            IScene mockTransitionScene = Substitute.For<IScene>();
            _mockGameObjectFactory.CreatePanSceneTransition(_mockEngine, _sut, scenes["DeviceMenu"], PanSceneTransitionMode.PanRight, CodeLogicEngine.Constants.DeviceSceneTransitionTime).Returns(mockTransitionScene);

            _mockEngine.ExternalActionOccurred += Raise.Event<EventHandler<ExternalActionEventArgs>>(_mockEngine, new ExternalActionEventArgs("FAKEDATA"));

            _mockGameObjectFactory.Received(0).CreatePanSceneTransition(Arg.Any<IEngine>(), Arg.Any<IScene>(), Arg.Any<IScene>(), Arg.Any<PanSceneTransitionMode>(), Arg.Any<double>());
        }
        #endregion

        #region Start Tests
        [TestMethod]
        public void StartWhenDeviceModelStopwatchPausedShouldCallStopwatchStart()
        {
            _mockDeviceModel.Stopwatch.IsPaused.Returns(true);

            _sut.Start();

            _mockDeviceModel.Stopwatch.Received(1).Start();
        }

        [TestMethod]
        public void StartWhenDeviceModelStopwatchNotPausedShouldNotCallStopwatchStart()
        {
            _mockDeviceModel.Stopwatch.IsPaused.Returns(false);

            _sut.Start();

            _mockDeviceModel.Stopwatch.Received(0).Start();
        }
        #endregion
    }
}
