using System;
using ArbitraryPixel.CodeLogic.Common.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArbitraryPixel.CodeLogic.Common_Tests.Model
{
    [TestClass]
    public class GameStatsModel_Tests
    {
        private GameStatsModel _sut;

        [TestInitialize]
        public void Initialize()
        {
        }

        private void Construct()
        {
            _sut = new GameStatsModel();
        }

        private void AssertDefault<T>(T value)
        {
            Assert.AreEqual<T>(default(T), value);
        }

        #region Reset Tests
        [TestMethod]
        public void ResetShouldResetPropertyToDefault_GamesPlayed()
        {
            Construct();
            _sut.GamesPlayed = 1234;

            _sut.Reset();

            AssertDefault<ulong>(_sut.GamesPlayed);
        }

        [TestMethod]
        public void ResetShouldResetPropertyToDefault_GamesWon()
        {
            Construct();
            _sut.GamesWon = 1234;

            _sut.Reset();

            AssertDefault<ulong>(_sut.GamesWon);
        }

        [TestMethod]
        public void ResetShouldResetPropertyToDefault_TotalGuesses()
        {
            Construct();
            _sut.TotalGuesses = 1234;

            _sut.Reset();

            AssertDefault<ulong>(_sut.TotalGuesses);
        }

        [TestMethod]
        public void ResetShouldResetPropertyToDefault_LeastGuessesToWin()
        {
            Construct();
            _sut.LeastGuessesToWin = 1234;

            _sut.Reset();

            AssertDefault<ulong>(_sut.LeastGuessesToWin);
        }

        [TestMethod]
        public void ResetShouldResetPropertyToDefault_CurrentWinStreak()
        {
            Construct();
            _sut.CurrentWinStreak = 1234;

            _sut.Reset();

            AssertDefault<ulong>(_sut.CurrentWinStreak);
        }

        [TestMethod]
        public void ResetShouldResetPropertyToDefault_CurrentLossStreak()
        {
            Construct();
            _sut.CurrentLossStreak = 1234;

            _sut.Reset();

            AssertDefault<ulong>(_sut.CurrentLossStreak);
        }

        [TestMethod]
        public void ResetShouldResetPropertyToDefault_TotalGameTime()
        {
            Construct();
            _sut.TotalGameTime = TimeSpan.FromSeconds(1234);

            _sut.Reset();

            AssertDefault<TimeSpan>(_sut.TotalGameTime);
        }

        [TestMethod]
        public void ResetShouldResetPropertyToDefault_FastestWin()
        {
            Construct();
            _sut.FastestWin = TimeSpan.FromSeconds(1234);

            _sut.Reset();

            AssertDefault<TimeSpan>(_sut.FastestWin);
        }

        [TestMethod]
        public void ResetShouldResetPropertyToDefault_SlowestWin()
        {
            Construct();
            _sut.SlowestWin = TimeSpan.FromSeconds(1234);

            _sut.Reset();

            AssertDefault<TimeSpan>(_sut.SlowestWin);
        }
        #endregion
    }
}
