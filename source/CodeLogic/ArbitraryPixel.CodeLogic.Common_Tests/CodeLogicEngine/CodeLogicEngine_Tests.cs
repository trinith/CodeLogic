using ArbitraryPixel.CodeLogic.Common.Config;
using ArbitraryPixel.Platform2D.Engine;
using ArbitraryPixel.Platform2D.Scene;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;
using NSubstitute;
using System;

namespace ArbitraryPixel.CodeLogic.Common_Tests
{
    [TestClass]
    public class CodeLogicEngine_Tests : CodeLogicEngineTestBase
    {
        #region Exit Tests
        [TestMethod]
        public void ExitWithOutstandingConfigChangesShouldPersist()
        {
            ICodeLogicSettings mockConfigStore = Substitute.For<ICodeLogicSettings>();
            _mockContainer.GetComponent<ICodeLogicSettings>().Returns(mockConfigStore);

            mockConfigStore.CacheChanged.Returns(true);

            _sut.Exit();

            mockConfigStore.Received(1).PersistCache();
        }

        [TestMethod]
        public void ExitWithoutOutstandingConfigChangesShouldNotPersist()
        {
            ICodeLogicSettings mockConfigStore = Substitute.For<ICodeLogicSettings>();
            _mockContainer.GetComponent<ICodeLogicSettings>().Returns(mockConfigStore);

            mockConfigStore.CacheChanged.Returns(false);

            _sut.Exit();

            mockConfigStore.Received(0).PersistCache();
        }
        #endregion

        #region Suspend Tests
        [TestMethod]
        public void SuspendShouldCallSuspendOnAudioPlayerMusicController()
        {
            _sut.Suspend();

            _mockAudioManager.MusicController.Received(1).Suspend();
        }
        #endregion

        #region Suspend Tests
        [TestMethod]
        public void SuspendShouldCallStopwatchManagerStop()
        {
            _sut.Suspend();

            _mockStopwatchManager.Received(1).Stop();
        }
        #endregion

        #region Resume Tests
        [TestMethod]
        public void ResumeShouldCallStopwatchManagerStart()
        {
            _sut.Resume();

            _mockStopwatchManager.Received(1).Start();
        }
        #endregion

        #region Current Scene Tests
        [TestMethod]
        public void CurrentSceneShouldDefaultToNull()
        {
            Assert.IsNull(_sut.CurrentScene);
        }

        [TestMethod]
        public void CurrentSceneShouldChangeToExpectedSceneAfterLoadContent()
        {
            IScene mockScene = Substitute.For<IScene>();
            _mockGameObjectFactory.CreateGameStartupScene(Arg.Any<IEngine>(), Arg.Any<IScene>()).Returns(mockScene);

            _sut.LoadContent();

            Assert.AreSame(mockScene, _sut.CurrentScene);
        }

        [TestMethod]
        public void CurrentSceneShouldUpdateAfterSceneChange()
        {
            IScene mockScene = Substitute.For<IScene>();
            _mockGameObjectFactory.CreateGameStartupScene(Arg.Any<IEngine>(), Arg.Any<IScene>()).Returns(mockScene);

            IScene mockNextScene = Substitute.For<IScene>();
            mockScene.NextScene.Returns(mockNextScene);
            mockScene.SceneComplete.Returns(true);

            _sut.LoadContent();
            _sut.Update(new GameTime());

            Assert.AreSame(mockNextScene, _sut.CurrentScene);
        }
        #endregion

        #region TriggerExternalAction Tests
        [TestMethod]
        public void TriggerExternalActionShouldFireExternalActionOccurredEvent_TestA()
        {
            EventHandler<ExternalActionEventArgs> subscriber = Substitute.For<EventHandler<ExternalActionEventArgs>>();
            _sut.ExternalActionOccurred += subscriber;

            _sut.TriggerExternalAction("TestString");

            subscriber.Received(1)(_sut, Arg.Is<ExternalActionEventArgs>(x => (string)x.Data == "TestString"));
        }

        [TestMethod]
        public void TriggerExternalActionShouldFireExternalActionOccurredEvent_TestB()
        {
            EventHandler<ExternalActionEventArgs> subscriber = Substitute.For<EventHandler<ExternalActionEventArgs>>();
            _sut.ExternalActionOccurred += subscriber;

            _sut.TriggerExternalAction(Color.Red);

            subscriber.Received(1)(_sut, Arg.Is<ExternalActionEventArgs>(x => (Color)x.Data == Color.Red));
        }
        #endregion
    }
}
