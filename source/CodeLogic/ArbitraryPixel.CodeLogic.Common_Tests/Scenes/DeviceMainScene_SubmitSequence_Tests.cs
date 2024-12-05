using ArbitraryPixel.CodeLogic.Common;
using ArbitraryPixel.CodeLogic.Common.Entities;
using ArbitraryPixel.CodeLogic.Common.Layers;
using ArbitraryPixel.CodeLogic.Common.Model;
using ArbitraryPixel.CodeLogic.Common.Scenes;
using ArbitraryPixel.Common;
using ArbitraryPixel.Common.Drawing;
using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Platform2D.Engine;
using ArbitraryPixel.Platform2D.Layer;
using ArbitraryPixel.Platform2D.Scene;
using ArbitraryPixel.Platform2D.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NSubstitute;
using System;
using System.Collections.Generic;

namespace ArbitraryPixel.CodeLogic.Common_Tests.Scenes
{
    [TestClass]
    public class DeviceMainScene_SubmitSequence_Tests : UnitTestBase<DeviceMainScene>
    {
        private IEngine _mockEngine;
        private IDeviceModel _mockDeviceModel;
        private ILogPanelModel _mockPanelModel;

        private GameObjectFactory _mockGameObjectFactory;
        private IDeviceMainUILayer _mockDUILayer;
        private ILayer _mockWindowLayer;

        private IConsoleWindow _mockConsoleWindow;

        private IRandom _mockRandom;
        private ISequenceComparer _mockSequenceComparer;
        private ISequenceCompareResult _mockCompareResult;
        private IScene _mockMissionDebriefingScene;
        private IScene _mockTransitionScene;

        protected override void OnInitializing()
        {
            base.OnInitializing();

            _mockEngine = Substitute.For<IEngine>();
            _mockEngine.ScreenManager.World.Returns(new Point(100, 200));

            _mockDeviceModel = Substitute.For<IDeviceModel>();
            _mockPanelModel = Substitute.For<ILogPanelModel>();

            GameObjectFactory.SetInstance(_mockGameObjectFactory = Substitute.For<GameObjectFactory>());
            _mockGameObjectFactory.CreateDeviceMainUILayer(Arg.Any<IEngine>(), Arg.Any<ISpriteBatch>(), Arg.Any<IDeviceModel>(), Arg.Any<ILogPanelModel>()).Returns(_mockDUILayer = Substitute.For<IDeviceMainUILayer>());
            _mockDUILayer.Enabled.Returns(true);
            _mockDUILayer.Visible.Returns(true);

            _mockGameObjectFactory.CreateGenericLayer(Arg.Any<IEngine>(), Arg.Any<ISpriteBatch>(), Arg.Any<SpriteSortMode>()).Returns(_mockWindowLayer = Substitute.For<ILayer>());
            _mockWindowLayer.Enabled.Returns(true);
            _mockWindowLayer.Visible.Returns(true);

            _mockGameObjectFactory.CreateRandom().Returns(_mockRandom = Substitute.For<IRandom>());

            _mockGameObjectFactory.CreateConsoleWindow(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>(), Arg.Any<ITextObjectBuilder>(), Arg.Any<ITextObjectRenderer>()).Returns(_mockConsoleWindow = Substitute.For<IConsoleWindow>());
            _mockConsoleWindow.Enabled.Returns(true);
            _mockConsoleWindow.Visible.Returns(true);

            _mockGameObjectFactory.CreateSequenceComparer().Returns(_mockSequenceComparer = Substitute.For<ISequenceComparer>());
            _mockSequenceComparer.Compare(Arg.Any<ICodeSequence>(), Arg.Any<ICodeSequence>()).Returns(_mockCompareResult = Substitute.For<ISequenceCompareResult>());
            _mockCompareResult.IsEqual.Returns(false);

            Dictionary<string, IScene> scenes = new Dictionary<string, IScene>();
            scenes.Add("MissionDebriefing", _mockMissionDebriefingScene = Substitute.For<IScene>());
            _mockEngine.Scenes.Returns(scenes);

            _mockTransitionScene = Substitute.For<IScene>();
            _mockGameObjectFactory.CreateFadeSceneTransition(Arg.Any<IEngine>(), Arg.Any<IScene>(), Arg.Any<IScene>(), Arg.Any<FadeSceneTransitionMode>(), Arg.Any<float>()).Returns(_mockTransitionScene);
        }

        protected override DeviceMainScene OnCreateSUT()
        {
            return new DeviceMainScene(_mockEngine, _mockDeviceModel, _mockPanelModel);
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            _sut.Initialize();
            _mockDUILayer.SubmitSequence += Raise.Event<EventHandler<EventArgs>>();
        }

        #region SubmitSequence Event Handling Tests
        [TestMethod]
        public void SubmitSequenceShouldAddConsoleWindowToWindowLayer()
        {
            _mockWindowLayer.Received(1).AddEntity(Arg.Any<IConsoleWindow>());
        }

        [TestMethod]
        public void SubmitSequenceShouldCreateNewConsoleWindow()
        {
            // World bounds should be 100, 200
            RectangleF expectedWindowBounds = new RectangleF(5, 10, 90, 180);
            RectangleF expectedTextBounds = expectedWindowBounds;
            expectedTextBounds.Inflate(-2 - 10, -2 - 10);

            Received.InOrder(
                () =>
                {
                    _mockGameObjectFactory.CreateTextFormatValueHandlerManager();
                    _mockGameObjectFactory.CreateTextFormatProcessor(Arg.Any<ITextFormatValueHandlerManager>());
                    _mockGameObjectFactory.CreateTextObjectFactory();
                    _mockGameObjectFactory.CreateTextObjectBuilderWithConsoleFonts(Arg.Any<ITextFormatProcessor>(), Arg.Any<ITextObjectFactory>(), _mockEngine.AssetBank);

                    _mockGameObjectFactory.CreateTextObjectRenderer(_mockEngine.GrfxFactory.RenderTargetFactory, _mockEngine.Graphics.GraphicsDevice, Arg.Any<ISpriteBatch>(), (Rectangle)expectedTextBounds);

                    _mockGameObjectFactory.CreateConsoleWindow(_mockEngine, expectedWindowBounds, _mockWindowLayer.MainSpriteBatch, Arg.Any<ITextObjectBuilder>(), Arg.Any<ITextObjectRenderer>());

                    _mockConsoleWindow.BorderSize = new SizeF(2);
                    _mockConsoleWindow.Padding = new SizeF(10);
                }
            );
        }

        [TestMethod]
        public void SubmitSequenceShouldAttachToDisposedEventHandler()
        {
            _mockConsoleWindow.Received(1).Disposed += Arg.Any<EventHandler<EventArgs>>();
        }

        [TestMethod]
        public void SubmitSequenceShouldAddConsoleWindowToEntities()
        {
            _mockWindowLayer.Received(1).AddEntity(_mockConsoleWindow);
        }

        [TestMethod]
        public void SubmitSequenceShouldCreateSequenceComparer()
        {
            _mockGameObjectFactory.Received(1).CreateSequenceComparer();
        }

        [TestMethod]
        public void SubmitSequenceShouldCallComparerCompare()
        {
            _mockSequenceComparer.Received(1).Compare(_mockDeviceModel.InputSequence, _mockDeviceModel.TargetSequence);
        }
        #endregion

        #region Text Creation Tests
        [TestMethod]
        public void SubmitSequenceShouldCreateConsoleWindowText()
        {
            // Not going to test all the string combinations... this is a weaker test set, but I don't want to get caught up in low value tests.
            // Just confirm that some text was set on the console window.
            _mockConsoleWindow.Received(1).SetTextFormat(Arg.Any<string>());
        }
        #endregion

        #region Dispose Tests
        [TestMethod]
        public void ConsoleWindowDisposeShouldIncremementModelCurrentTrialWhenLessThanTen()
        {
            _mockDeviceModel.CurrentTrial.Returns(3);

            _mockConsoleWindow.Disposed += Raise.Event<EventHandler<EventArgs>>();

            _mockDeviceModel.Received(1).CurrentTrial = 4;
        }

        [TestMethod]
        public void ConsoleWindowDisposeWhenResultEqualShouldCreateTransitionScene()
        {
            _mockDeviceModel.CurrentTrial.Returns(5);
            _mockCompareResult.IsEqual.Returns(true);

            _mockConsoleWindow.Disposed += Raise.Event<EventHandler<EventArgs>>();

            _mockGameObjectFactory.Received(1).CreateFadeSceneTransition(_mockEngine, _sut, _mockMissionDebriefingScene, FadeSceneTransitionMode.OutIn, CodeLogicEngine.Constants.FadeSceneTransitionTime);
        }

        [TestMethod]
        public void ConsoleWindowDisposeWhenResultEqualShouldSetNextSceneToTransitionScene()
        {
            _mockDeviceModel.CurrentTrial.Returns(5);
            _mockCompareResult.IsEqual.Returns(true);

            _mockConsoleWindow.Disposed += Raise.Event<EventHandler<EventArgs>>();

            Assert.AreEqual<IScene>(_mockTransitionScene, _sut.NextScene);
        }

        [TestMethod]
        public void ConsoleWindowDisposeWhenResultEqualShouldSetSceneCompleteTrue()
        {
            _mockDeviceModel.CurrentTrial.Returns(5);
            _mockCompareResult.IsEqual.Returns(true);

            _mockConsoleWindow.Disposed += Raise.Event<EventHandler<EventArgs>>();

            Assert.IsTrue(_sut.SceneComplete);
        }

        [TestMethod]
        public void ConsoleWindowDisposeWhenCurrentTrialGreaterThanMaxTrialsShouldCreateTransitionScene()
        {
            _mockDeviceModel.CurrentTrial.Returns(10);

            _mockConsoleWindow.Disposed += Raise.Event<EventHandler<EventArgs>>();

            _mockGameObjectFactory.Received(1).CreateFadeSceneTransition(_mockEngine, _sut, _mockMissionDebriefingScene, FadeSceneTransitionMode.OutIn, CodeLogicEngine.Constants.FadeSceneTransitionTime);
        }

        [TestMethod]
        public void ConsoleWindowDisposeWhenCurrentTrialGreaterThanMaxTrialsShouldSetNextSceneToTransitionScene()
        {
            _mockDeviceModel.CurrentTrial.Returns(10);

            _mockConsoleWindow.Disposed += Raise.Event<EventHandler<EventArgs>>();

            Assert.AreEqual<IScene>(_mockTransitionScene, _sut.NextScene);
        }

        [TestMethod]
        public void ConsoleWindowDisposeWhenCurrentTrialGreaterThanMaxTrialsShouldSetSceneCompleteTrue()
        {
            _mockDeviceModel.CurrentTrial.Returns(10);

            _mockConsoleWindow.Disposed += Raise.Event<EventHandler<EventArgs>>();

            Assert.IsTrue(_sut.SceneComplete);
        }

        [TestMethod]
        public void ConsoleWindowDisposedWhenGameEndingOnSuccessShouldSetDeviceModelGameWonTrue()
        {
            _mockCompareResult.IsEqual.Returns(true);
            _mockDeviceModel.CurrentTrial.Returns(10);

            _mockConsoleWindow.Disposed += Raise.Event<EventHandler<EventArgs>>();

            _mockDeviceModel.Received(1).GameWon = true;
        }

        [TestMethod]
        public void ConsoleWindowDipsosedWhenGameEndingOnFailureShouldSetDeviceModelGameWonFalse()
        {
            _mockCompareResult.IsEqual.Returns(false);
            _mockDeviceModel.CurrentTrial.Returns(10);

            _mockConsoleWindow.Disposed += Raise.Event<EventHandler<EventArgs>>();

            _mockDeviceModel.Received(1).GameWon = false;
        }
        #endregion

        #region End Tests
        // NOTE: Put these here because they will utilize the console window.
        [TestMethod]
        public void EndAfterGameoverShouldStopMusicPlayback()
        {
            _mockDeviceModel.CurrentTrial.Returns(10);
            _mockConsoleWindow.Disposed += Raise.Event<EventHandler<EventArgs>>();
            _sut.End();

            _mockEngine.AudioManager.Received(1).MusicController.FadeVolumeAttenuation(0, CodeLogicEngine.Constants.FadeSceneTransitionTime);
        }

        [TestMethod]
        public void EndAfterGameOverWithResetShouldNotStopMusicPlayback()
        {
            _mockDeviceModel.CurrentTrial.Returns(10);
            _mockConsoleWindow.Disposed += Raise.Event<EventHandler<EventArgs>>();
            _sut.Reset();
            _sut.End();

            _mockEngine.AudioManager.Received(0).MusicController.FadeVolumeAttenuation(Arg.Any<float>(), Arg.Any<double>());
        }

        [TestMethod]
        public void EndNormallyShouldNotStopMusicPlayback()
        {
            _sut.End();

            _mockEngine.AudioManager.Received(0).MusicController.FadeVolumeAttenuation(Arg.Any<float>(), Arg.Any<double>());
        }

        [TestMethod]
        public void EndAfterGameOverShouldStopDeviceModelStopwatch()
        {
            _mockDeviceModel.CurrentTrial.Returns(10);
            _mockConsoleWindow.Disposed += Raise.Event<EventHandler<EventArgs>>();
            _sut.End();

            _mockDeviceModel.Stopwatch.Received(1).Stop();
        }

        [TestMethod]
        public void EndAfterGameOverWithResetShouldNotStopDeviceModelStopwatch()
        {
            _mockDeviceModel.CurrentTrial.Returns(10);
            _mockConsoleWindow.Disposed += Raise.Event<EventHandler<EventArgs>>();
            _sut.Reset();
            _sut.End();

            _mockDeviceModel.Stopwatch.Received(0).Stop();
        }

        [TestMethod]
        public void EndNormallyShouldNotStopDeviceModelStopwatch()
        {
            _sut.End();

            _mockDeviceModel.Stopwatch.Received(0).Stop();
        }
        #endregion
    }
}
