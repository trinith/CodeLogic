using System;
using ArbitraryPixel.CodeLogic.Common.Controllers;
using ArbitraryPixel.CodeLogic.Common.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace ArbitraryPixel.CodeLogic.Common_Tests.Controllers
{
    [TestClass]
    public class GameStatsController_Tests
    {
        private GameStatsController _sut;
        private IGameStatsModel _mockStatsModel;

        private IDeviceModel _mockDeviceModel;

        [TestInitialize]
        public void Initialize()
        {
            _mockStatsModel = Substitute.For<IGameStatsModel>();
            _mockDeviceModel = Substitute.For<IDeviceModel>();

            _mockDeviceModel.GameWon.Returns(true);
        }

        private void Construct()
        {
            _sut = new GameStatsController(_mockStatsModel);
        }

        #region Constructor Tests
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullParameterShouldThrowException_GameStatsModel()
        {
            _sut = new GameStatsController(null);
        }
        #endregion

        #region Update Tests
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void UpdateWithNullDeviceModelShouldThrowException()
        {
            Construct();

            _sut.Update(null);
        }

        [TestMethod]
        public void UpdateShouldIncremementModelGamesPlayed()
        {
            Construct();
            _mockStatsModel.GamesPlayed.Returns((ulong)3);

            _sut.Update(_mockDeviceModel);

            _mockStatsModel.Received(1).GamesPlayed = 4;
        }

        [TestMethod]
        public void UpdateWithGameWonTrueShouldIncrementGamesWon()
        {
            Construct();
            _mockStatsModel.GamesWon.Returns((ulong)5);

            _sut.Update(_mockDeviceModel);

            _mockStatsModel.Received(1).GamesWon = 6;
        }

        [TestMethod]
        public void UpdateWithGameWonFalseShouldNotIncremementGamesWon()
        {
            Construct();
            _mockStatsModel.GamesWon.Returns((ulong)5);
            _mockDeviceModel.GameWon.Returns(false);

            _sut.Update(_mockDeviceModel);

            _mockStatsModel.Received(0).GamesWon = Arg.Any<ulong>();
        }

        [TestMethod]
        public void UpdateShouldIncremementTotalGuessesWithDeviceModelAttemptCount()
        {
            Construct();
            _mockStatsModel.TotalGuesses.Returns((ulong)11);
            _mockDeviceModel.Attempts.Count.Returns(3);

            _sut.Update(_mockDeviceModel);

            _mockStatsModel.Received(1).TotalGuesses = 14;
        }

        [TestMethod]
        public void UpdateWithDeviceModelAttemptCountLessThanLeastGuessesToWinShouldUpdateValue()
        {
            Construct();
            _mockStatsModel.LeastGuessesToWin.Returns((ulong)5);
            _mockDeviceModel.Attempts.Count.Returns(3);

            _sut.Update(_mockDeviceModel);

            _mockStatsModel.Received(1).LeastGuessesToWin = 3;
        }

        [TestMethod]
        public void UpdateWithDeviceModelAttemptCountNotSetShouldUpdateValue()
        {
            Construct();
            _mockStatsModel.LeastGuessesToWin.Returns((ulong)0);
            _mockDeviceModel.Attempts.Count.Returns(3);

            _sut.Update(_mockDeviceModel);

            _mockStatsModel.Received(1).LeastGuessesToWin = 3;
        }

        [TestMethod]
        public void UpdateWithDeviceModelAttemptCountGreaterThanLeastGuessesToWinShouldNotUpdateValue()
        {
            Construct();
            _mockStatsModel.LeastGuessesToWin.Returns((ulong)3);
            _mockDeviceModel.Attempts.Count.Returns(5);

            _sut.Update(_mockDeviceModel);

            _mockStatsModel.Received(0).LeastGuessesToWin = Arg.Any<ulong>();
        }

        [TestMethod]
        public void UpdateWithGameWonFalseAndDeviceModelAttemptCountLessThanLeastGuessesToWinShouldNotUpdateValue()
        {
            Construct();
            _mockStatsModel.LeastGuessesToWin.Returns((ulong)5);
            _mockDeviceModel.Attempts.Count.Returns(3);
            _mockDeviceModel.GameWon.Returns(false);

            _sut.Update(_mockDeviceModel);

            _mockStatsModel.Received(0).LeastGuessesToWin = Arg.Any<ulong>();
        }

        [TestMethod]
        public void UpdateWithGameWonTrueShouldIncrementCurrentWinStreak()
        {
            Construct();
            _mockStatsModel.CurrentWinStreak.Returns((ulong)6);

            _sut.Update(_mockDeviceModel);

            _mockStatsModel.Received(1).CurrentWinStreak = 7;
        }

        [TestMethod]
        public void UpdateWithGameWonFalseShouldResetCurrentWinStreak()
        {
            Construct();
            _mockStatsModel.CurrentWinStreak.Returns((ulong)6);
            _mockDeviceModel.GameWon.Returns(false);

            _sut.Update(_mockDeviceModel);

            _mockStatsModel.Received(1).CurrentWinStreak = 0;
        }

        [TestMethod]
        public void UpdateWithGameWonTrueShouldResetCurrentLossStreak()
        {
            Construct();
            _mockStatsModel.CurrentLossStreak.Returns((ulong)5);

            _sut.Update(_mockDeviceModel);

            _mockStatsModel.Received(1).CurrentLossStreak = 0;
        }

        [TestMethod]
        public void UpdateWithGameWonFalseShouldIncremementCurrentLossStreak()
        {
            Construct();
            _mockStatsModel.CurrentLossStreak.Returns((ulong)5);
            _mockDeviceModel.GameWon.Returns(false);

            _sut.Update(_mockDeviceModel);

            _mockStatsModel.Received(1).CurrentLossStreak = 6;
        }

        [TestMethod]
        public void UpdateWithElapsedTimeLessThanFastestWinShouldSetFastestWinToStopWatchElapsedTime()
        {
            Construct();
            _mockDeviceModel.Stopwatch.ElapsedTime.Returns(TimeSpan.FromSeconds(5));
            _mockStatsModel.FastestWin.Returns(TimeSpan.FromSeconds(10));

            _sut.Update(_mockDeviceModel);

            _mockStatsModel.Received(1).FastestWin = TimeSpan.FromSeconds(5);
        }

        [TestMethod]
        public void UpdateWithElapsedTimeGreaterEqualThanFastestWinShouldNotSetFastestWin()
        {
            Construct();
            _mockDeviceModel.Stopwatch.ElapsedTime.Returns(TimeSpan.FromSeconds(15));
            _mockStatsModel.FastestWin.Returns(TimeSpan.FromSeconds(5));

            _sut.Update(_mockDeviceModel);

            _mockStatsModel.Received(0).FastestWin = Arg.Any<TimeSpan>();
        }

        [TestMethod]
        public void UpdateWithGameWonfalseAndElapsedTimeLessThanFastestWinShouldNotSetFastestWin()
        {
            Construct();
            _mockDeviceModel.Stopwatch.ElapsedTime.Returns(TimeSpan.FromSeconds(5));
            _mockStatsModel.FastestWin.Returns(TimeSpan.FromSeconds(10));
            _mockDeviceModel.GameWon.Returns(false);

            _sut.Update(_mockDeviceModel);

            _mockStatsModel.Received(0).FastestWin = Arg.Any<TimeSpan>();
        }

        [TestMethod]
        public void UpdateWithElapsedTimeGreaterThanSlowestWinShouldUpdateSlowestWin()
        {
            Construct();
            _mockDeviceModel.Stopwatch.ElapsedTime.Returns(TimeSpan.FromSeconds(15));
            _mockStatsModel.SlowestWin.Returns(TimeSpan.FromSeconds(10));

            _sut.Update(_mockDeviceModel);

            _mockStatsModel.Received(1).SlowestWin = TimeSpan.FromSeconds(15);
        }

        [TestMethod]
        public void UpdateWithElapsedTimeLessEqualThanSlowestWinShouldNotUpdateSlowestWin()
        {
            Construct();
            _mockDeviceModel.Stopwatch.ElapsedTime.Returns(TimeSpan.FromSeconds(10));
            _mockStatsModel.SlowestWin.Returns(TimeSpan.FromSeconds(15));

            _sut.Update(_mockDeviceModel);

            _mockStatsModel.Received(0).SlowestWin = Arg.Any<TimeSpan>();
        }

        [TestMethod]
        public void UpdateWithGameWonFalseAndElapsedTimeGreaterThanSlowestWinShoudNotUpdateSlowestWin()
        {
            Construct();
            _mockDeviceModel.Stopwatch.ElapsedTime.Returns(TimeSpan.FromSeconds(15));
            _mockStatsModel.SlowestWin.Returns(TimeSpan.FromSeconds(10));
            _mockDeviceModel.GameWon.Returns(false);

            _sut.Update(_mockDeviceModel);

            _mockStatsModel.Received(0).SlowestWin = Arg.Any<TimeSpan>();
        }
        #endregion

        #region Updated Event Tests
        [TestMethod]
        public void UpdatedEventShouldBeFiredWhenUpdateIsCalled()
        {
            Construct();

            var eventHandler = Substitute.For<EventHandler<EventArgs>>();
            _sut.Updated += eventHandler;

            _sut.Update(_mockDeviceModel);

            eventHandler.Received(1)(_sut, Arg.Any<EventArgs>());
        }
        #endregion
    }
}
