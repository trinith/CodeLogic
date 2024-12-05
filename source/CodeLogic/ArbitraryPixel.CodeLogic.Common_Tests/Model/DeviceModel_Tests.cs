using ArbitraryPixel.CodeLogic.Common;
using ArbitraryPixel.CodeLogic.Common.Model;
using ArbitraryPixel.Common;
using ArbitraryPixel.Platform2D.Time;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;

namespace ArbitraryPixel.CodeLogic.Common_Tests.Model
{
    [TestClass]
    public class DeviceModel_Tests
    {
        private IRandom _mockRandom;
        private ICodeSequence _mockTargetSequence;
        private ICodeValueColourMap _mockColourMap;
        private IStopwatchManager _mockStopwatchManager;

        private GameObjectFactory _mockGameObjectFactory;

        private DeviceModel _sut;

        [TestInitialize]
        public void Initialize()
        {
            int index = 0;
            CodeValue[] sequence = new CodeValue[] { CodeValue.Red, CodeValue.Green, CodeValue.Blue, CodeValue.Yellow };
            _mockRandom = Substitute.For<IRandom>();
            _mockRandom.Next(Arg.Any<int>(), Arg.Any<int>()).Returns(
                x =>
                {
                    return (int)sequence[index++];
                }
            );

            _mockColourMap = Substitute.For<ICodeValueColourMap>();
            _mockStopwatchManager = Substitute.For<IStopwatchManager>();

            _mockGameObjectFactory = Substitute.For<GameObjectFactory>();
            GameObjectFactory.SetInstance(_mockGameObjectFactory);

            _mockGameObjectFactory.CreateCodeSequence(Arg.Any<int>()).Returns(Substitute.For<ICodeSequence>(), _mockTargetSequence = Substitute.For<ICodeSequence>());

            Construct();
        }

        private void Construct()
        {
            _sut = new DeviceModel(_mockRandom, _mockColourMap, _mockStopwatchManager);
        }

        #region Constructor Tests
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullParameterShouldThrowException_Random()
        {
            _sut = new DeviceModel(null, _mockColourMap, _mockStopwatchManager);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullParameterShouldThrowException_CodeColourMap()
        {
            _sut = new DeviceModel(_mockRandom, null, _mockStopwatchManager);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullParameterShouldThrowException_StopwatchManager()
        {
            _sut = new DeviceModel(_mockRandom, _mockColourMap, null);
        }

        [TestMethod]
        public void ConstructorShouldCreateExpectedCodeSequenceObjects()
        {
            _mockGameObjectFactory.Received(2).CreateCodeSequence(4);
        }

        [TestMethod]
        public void ConstructorShouldGenerateRandomTargetSequence()
        {
            _mockTargetSequence.Received(1).GenerateRandomCode(_mockRandom);
        }

        [TestMethod]
        public void ConstructorShouldCreateSequenceAttemptCollection()
        {
            _mockGameObjectFactory.Received(1).CreateSequenceAttemptCollection();
        }

        [TestMethod]
        public void ConstructShouldCreateNewStopwatch()
        {
            _mockStopwatchManager.Received(1).Create();
        }
        #endregion

        #region GameWon Tests
        [TestMethod]
        public void GameWonShouldDefaultFalse()
        {
            Assert.IsFalse(_sut.GameWon);
        }

        [TestMethod]
        public void GameWonShouldReturnSetValue()
        {
            _sut.GameWon = true;

            Assert.IsTrue(_sut.GameWon);
        }
        #endregion

        #region CurrentTrial Tests
        [TestMethod]
        public void CurrentTrialShouldDefaultToOne()
        {
            Assert.AreEqual<int>(1, _sut.CurrentTrial);
        }

        [TestMethod]
        public void CurrentTrialShouldReturnSetValue()
        {
            _sut.CurrentTrial = 5;

            Assert.AreEqual<int>(5, _sut.CurrentTrial);
        }
        #endregion

        #region Reset Tests
        [TestMethod]
        public void ResetShouldResultProperties_CreatesCodeSequences()
        {
            _mockGameObjectFactory.ClearReceivedCalls();
            _sut.Reset();
            _mockGameObjectFactory.Received(2).CreateCodeSequence(4);
        }

        [TestMethod]
        public void ResetShouldResultProperties_CurrentTrial()
        {
            _sut.CurrentTrial = 5;
            _sut.Reset();

            Assert.AreEqual<int>(1, _sut.CurrentTrial);
        }

        [TestMethod]
        public void ResetShouldCreateSequenceAttemptCollection()
        {
            _mockGameObjectFactory.ClearReceivedCalls();

            _sut.Reset();

            _mockGameObjectFactory.Received(1).CreateSequenceAttemptCollection();
        }

        [TestMethod]
        public void ResetShouldCreateRandomCode()
        {
            ICodeSequence mockSequence = Substitute.For<ICodeSequence>();
            _mockGameObjectFactory.CreateCodeSequence(Arg.Any<int>()).Returns(Substitute.For<ICodeSequence>(), mockSequence);

            _sut.Reset();

            mockSequence.Received(1).GenerateRandomCode(_mockRandom);
        }

        [TestMethod]
        public void ResetShouldCallDisposeOnExistingStopwatch()
        {
            IStopwatch mockStopwatch = Substitute.For<IStopwatch>();
            _mockStopwatchManager.Create().Returns(mockStopwatch);

            Construct();

            _sut.Reset();

            mockStopwatch.Received(1).Dispose();
        }

        [TestMethod]
        public void ResetShouldCreateNewStopwatch()
        {
            _mockStopwatchManager.ClearReceivedCalls();

            _sut.Reset();

            _mockStopwatchManager.Received(1).Create();
        }

        [TestMethod]
        public void ResetShouldSetGameWonFalse()
        {
            _sut.GameWon = true;

            _sut.Reset();

            Assert.IsFalse(_sut.GameWon);
        }
        #endregion

        #region AlarmLevel Tests
        [TestMethod]
        public void AlarmLevelShouldReturnLowForExpectedRange_Low()
        {
            _sut.CurrentTrial = 1;
            Assert.AreEqual<AlarmLevel>(AlarmLevel.Low, _sut.AlarmLevel);
        }

        [TestMethod]
        public void AlarmLevelShouldReturnLowForExpectedRange_High()
        {
            _sut.CurrentTrial = 3;
            Assert.AreEqual<AlarmLevel>(AlarmLevel.Low, _sut.AlarmLevel);
        }

        [TestMethod]
        public void AlarmLevelShouldReturnMediumForExpectedRange_Low()
        {
            _sut.CurrentTrial = 4;
            Assert.AreEqual<AlarmLevel>(AlarmLevel.Medium, _sut.AlarmLevel);
        }

        [TestMethod]
        public void AlarmLevelShouldReturnMediumForExpectedRange_High()
        {
            _sut.CurrentTrial = 6;
            Assert.AreEqual<AlarmLevel>(AlarmLevel.Medium, _sut.AlarmLevel);
        }

        [TestMethod]
        public void AlarmLevelShouldReturnHighForExpectedRange_Low()
        {
            _sut.CurrentTrial = 7;
            Assert.AreEqual<AlarmLevel>(AlarmLevel.High, _sut.AlarmLevel);
        }

        [TestMethod]
        public void AlarmLevelShouldReturnHighForExpectedRange_High()
        {
            _sut.CurrentTrial = 9;
            Assert.AreEqual<AlarmLevel>(AlarmLevel.High, _sut.AlarmLevel);
        }

        [TestMethod]
        public void AlarmLevelShouldReturnCriticalForExpectedRange_Low()
        {
            _sut.CurrentTrial = 10;
            Assert.AreEqual<AlarmLevel>(AlarmLevel.Critical, _sut.AlarmLevel);
        }

        [TestMethod]
        public void AlarmLevelShouldReturnCriticalForExpectedRange_High()
        {
            _sut.CurrentTrial = 0;
            Assert.AreEqual<AlarmLevel>(AlarmLevel.Critical, _sut.AlarmLevel);
        }
        #endregion

        #region CodeColourMap Tests
        [TestMethod]
        public void CodeColourMapShouldReturnValueFromConstructor()
        {
            Assert.AreSame(_mockColourMap, _sut.CodeColourMap);
        }
        #endregion

        #region Stopwatch Tests
        [TestMethod]
        public void StopwatchShouldReturnExpectedObject()
        {
            IStopwatch mockStopwatch = Substitute.For<IStopwatch>();
            _mockStopwatchManager.Create().Returns(mockStopwatch);

            Construct();

            Assert.AreSame(mockStopwatch, _sut.Stopwatch);
        }
        #endregion
    }
}
