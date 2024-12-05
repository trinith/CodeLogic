using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArbitraryPixel.CodeLogic.Common.Scenes;
using ArbitraryPixel.Platform2D.Scene;
using ArbitraryPixel.Platform2D.Engine;
using NSubstitute;

namespace ArbitraryPixel.CodeLogic.Common_Tests.Scenes
{
    [TestClass]
    public class GameStartupScene_Tests
    {
        private GameStartupScene _sut;
        private IScene _mockScene;
        private IEngine _mockEngine;

        [TestInitialize]
        public void Initialize()
        {
            _mockEngine = Substitute.For<IEngine>();
            _mockScene = Substitute.For<IScene>();

            _sut = new GameStartupScene(_mockEngine, _mockScene);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullParameterShouldThrowException_StartScene()
        {
            _sut = new GameStartupScene(_mockEngine, null);
        }

        [TestMethod]
        public void InitializeShouldSetNextSceneToStartScene()
        {
            _sut.Initialize();
            Assert.AreSame(_mockScene, _sut.NextScene);
        }

        [TestMethod]
        public void InitializeShouldSetSceneCompleteTrue()
        {
            _sut.Initialize();
            Assert.IsTrue(_sut.SceneComplete);
        }
    }
}
