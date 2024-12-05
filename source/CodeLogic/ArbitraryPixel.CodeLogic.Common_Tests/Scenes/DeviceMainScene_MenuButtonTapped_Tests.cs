using ArbitraryPixel.CodeLogic.Common;
using ArbitraryPixel.CodeLogic.Common.Layers;
using ArbitraryPixel.CodeLogic.Common.Model;
using ArbitraryPixel.CodeLogic.Common.Scenes;
using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Platform2D.Engine;
using ArbitraryPixel.Platform2D.Scene;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;
using NSubstitute;
using System;
using System.Collections.Generic;

namespace ArbitraryPixel.CodeLogic.Common_Tests.Scenes
{
    [TestClass]
    public class DeviceMainScene_MenuButtonTapped_Tests : UnitTestBase<DeviceMainScene>
    {
        private IEngine _mockEngine;
        private GameObjectFactory _mockGameObjectFactory;
        private IDeviceMainUILayer _mockDUILayer;
        private IScene _mockMenuScene;
        private IScene _mockTransitionScene;

        protected override void OnInitializing()
        {
            base.OnInitializing();

            _mockEngine = Substitute.For<IEngine>();
            _mockEngine.ScreenManager.World.Returns(new Point(100, 200));

            GameObjectFactory.SetInstance(_mockGameObjectFactory = Substitute.For<GameObjectFactory>());
            _mockGameObjectFactory.CreateDeviceMainUILayer(Arg.Any<IEngine>(), Arg.Any<ISpriteBatch>(), Arg.Any<IDeviceModel>(), Arg.Any<ILogPanelModel>()).Returns(_mockDUILayer = Substitute.For<IDeviceMainUILayer>());
            _mockDUILayer.Enabled.Returns(true);
            _mockDUILayer.Visible.Returns(true);

            _mockGameObjectFactory.CreatePanSceneTransition(Arg.Any<IEngine>(), Arg.Any<IScene>(), Arg.Any<IScene>(), Arg.Any<PanSceneTransitionMode>(), Arg.Any<double>()).Returns(_mockTransitionScene = Substitute.For<IScene>());
        }

        protected override DeviceMainScene OnCreateSUT()
        {
            return new DeviceMainScene(_mockEngine, Substitute.For<IDeviceModel>(), Substitute.For<ILogPanelModel>());
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            _sut.Initialize();

            _mockEngine.Scenes.Returns(new Dictionary<string, IScene>());
            _mockMenuScene = Substitute.For<IScene>();
            _mockEngine.Scenes.Add("DeviceMenu", _mockMenuScene);

            _mockDUILayer.MenuButtonTapped += Raise.Event<EventHandler<EventArgs>>();
        }

        [TestMethod]
        public void MenuButtonTappedShouldCreateHorizontalTransitionScene()
        {
            _mockGameObjectFactory.Received(1).CreatePanSceneTransition(_mockEngine, _sut, _mockMenuScene, PanSceneTransitionMode.PanRight, 0.125);
        }

        [TestMethod]
        public void MenuButtonTappedShouldSetNextSceneToDeviceDeviceMenu()
        {
            Assert.AreSame(_mockTransitionScene, _sut.NextScene);
        }

        [TestMethod]
        public void MenuButtonTappedShouldSetSceneCompleteTrue()
        {
            Assert.IsTrue(_sut.SceneComplete);
        }
    }
}
