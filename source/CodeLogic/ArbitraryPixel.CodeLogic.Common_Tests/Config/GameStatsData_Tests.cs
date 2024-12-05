using System;
using ArbitraryPixel.CodeLogic.Common.Config;
using ArbitraryPixel.CodeLogic.Common.Model;
using ArbitraryPixel.Common.ValueConversion;
using ArbitraryPixel.Platform2D.Config;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace ArbitraryPixel.CodeLogic.Common_Tests.Config
{
    [TestClass]
    public class GameStatsData_Tests
    {
        private GameStatsData _sut;

        private IConfigStore _mockConfigStore;
        private IGameStatsModel _mockModel;
        private IBuildInfoStore _mockBuildInfo;
        private IValueConverterManager _mockValueConverterManager;

        [TestInitialize]
        public void Initialize()
        {
            _mockConfigStore = Substitute.For<IConfigStore>();
            _mockModel = Substitute.For<IGameStatsModel>();
            _mockBuildInfo = Substitute.For<IBuildInfoStore>();
            _mockValueConverterManager = Substitute.For<IValueConverterManager>();
        }

        private void Construct()
        {
            _sut = new GameStatsData(_mockConfigStore, _mockModel, _mockBuildInfo, _mockValueConverterManager);
        }

        #region Constructor Tests
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullParameterShouldThrowException_ConfigStore()
        {
            _sut = new GameStatsData(null, _mockModel, _mockBuildInfo, _mockValueConverterManager);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullParameterShouldThrowException_Model()
        {
            _sut = new GameStatsData(_mockConfigStore, null, _mockBuildInfo, _mockValueConverterManager);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullParameterShouldThrowException_BuildInfo()
        {
            _sut = new GameStatsData(_mockConfigStore, _mockModel, null, _mockValueConverterManager);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullParameterShouldThrowException_ValueConverterManager()
        {
            _sut = new GameStatsData(_mockConfigStore, _mockModel, _mockBuildInfo, null);
        }
        #endregion

        #region Property Tests
        [TestMethod]
        public void CurrentVersionReturnsExpectedValue_TestA()
        {
            Construct();
            _mockBuildInfo.Version.Returns("asdf");

            Assert.AreEqual<string>("asdf", _sut.CurrentVersion);
        }

        [TestMethod]
        public void CurrentVersionReturnsExpectedValue_TestB()
        {
            Construct();
            _mockBuildInfo.Version.Returns("123a");

            Assert.AreEqual<string>("123a", _sut.CurrentVersion);
        }

        [TestMethod]
        public void DataVersionBeforeLoadDataShouldReturnNull()
        {
            Construct();

            Assert.IsNull(_sut.DataVersion);
        }

        [TestMethod]
        public void DataVersionReturnsExpectedValue_TestA()
        {
            Construct();
            _mockConfigStore.Get("Version").Returns("asdf");
            _sut.LoadData();

            Assert.AreEqual<string>("asdf", _sut.DataVersion);
        }

        [TestMethod]
        public void DataVersionReturnsExpectedValue_TestB()
        {
            Construct();
            _mockConfigStore.Get("Version").Returns("abc123");
            _sut.LoadData();

            Assert.AreEqual<string>("abc123", _sut.DataVersion);
        }
        #endregion

        #region Load Data Tests
        [TestMethod]
        public void LoadDataShouldCallConfigStoreLoadCache()
        {
            Construct();

            _sut.LoadData();

            _mockConfigStore.Received(1).LoadCache();
        }

        [TestMethod]
        public void LoadDataShouldSetModelFieldToExpectedValue_GamesPlayed()
        {
            Construct();

            _mockConfigStore.Get("GamesPlayed").Returns("asdf");
            _mockValueConverterManager.ConvertFromString<ulong>("asdf").Returns((ulong)3);

            _sut.LoadData();

            _mockModel.Received(1).GamesPlayed = 3;
        }

        [TestMethod]
        public void LoadDataShouldSetModelFieldToExpectedValue_GamesWon()
        {
            Construct();

            _mockConfigStore.Get("GamesWon").Returns("asdf");
            _mockValueConverterManager.ConvertFromString<ulong>("asdf").Returns((ulong)6);

            _sut.LoadData();

            _mockModel.Received(1).GamesWon = 6;
        }

        [TestMethod]
        public void LoadDataShouldSetModelFieldToExpectedValue_TotalGuesses()
        {
            Construct();

            _mockConfigStore.Get("TotalGuesses").Returns("asdf");
            _mockValueConverterManager.ConvertFromString<ulong>("asdf").Returns((ulong)3);

            _sut.LoadData();

            _mockModel.Received(1).TotalGuesses = 3;
        }

        [TestMethod]
        public void LoadDataShouldSetModelFieldToExpectedValue_LeastGuessesToWin()
        {
            Construct();

            _mockConfigStore.Get("LeastGuessesToWin").Returns("asdf");
            _mockValueConverterManager.ConvertFromString<ulong>("asdf").Returns((ulong)3);

            _sut.LoadData();

            _mockModel.Received(1).LeastGuessesToWin = 3;
        }

        [TestMethod]
        public void LoadDataShouldSetModelFieldToExpectedValue_CurrentWinStreak()
        {
            Construct();

            _mockConfigStore.Get("CurrentWinStreak").Returns("asdf");
            _mockValueConverterManager.ConvertFromString<ulong>("asdf").Returns((ulong)3);

            _sut.LoadData();

            _mockModel.Received(1).CurrentWinStreak = 3;
        }

        [TestMethod]
        public void LoadDataShouldSetModelFieldToExpectedValue_CurrentLossStreak()
        {
            Construct();

            _mockConfigStore.Get("CurrentLossStreak").Returns("asdf");
            _mockValueConverterManager.ConvertFromString<ulong>("asdf").Returns((ulong)3);

            _sut.LoadData();

            _mockModel.Received(1).CurrentLossStreak = 3;
        }

        [TestMethod]
        public void LoadDataShouldSetModelFieldToExpectedValue_TotalGameTime()
        {
            Construct();

            _mockConfigStore.Get("TotalGameTime").Returns("asdf");
            _mockValueConverterManager.ConvertFromString<TimeSpan>("asdf").Returns(TimeSpan.FromSeconds(3));

            _sut.LoadData();

            _mockModel.Received(1).TotalGameTime = TimeSpan.FromSeconds(3);
        }

        [TestMethod]
        public void LoadDataShouldSetModelFieldToExpectedValue_FastestWin()
        {
            Construct();

            _mockConfigStore.Get("FastestWin").Returns("asdf");
            _mockValueConverterManager.ConvertFromString<TimeSpan>("asdf").Returns(TimeSpan.FromSeconds(3));

            _sut.LoadData();

            _mockModel.Received(1).FastestWin = TimeSpan.FromSeconds(3);
        }

        [TestMethod]
        public void LoadDataShouldSetModelFieldToExpectedValue_SlowestWin()
        {
            Construct();

            _mockConfigStore.Get("SlowestWin").Returns("asdf");
            _mockValueConverterManager.ConvertFromString<TimeSpan>("asdf").Returns(TimeSpan.FromSeconds(3));

            _sut.LoadData();

            _mockModel.Received(1).SlowestWin = TimeSpan.FromSeconds(3);
        }

        [TestMethod]
        public void LoadDataShouldGetVersionFromConfigStore()
        {
            Construct();

            _sut.LoadData();

            _mockConfigStore.Received(1).Get("Version");
        }

        [TestMethod]
        public void LoadDataWithNoDataShouldResetModelAndStoreDefaultValues()
        {
            Construct();

            _sut.LoadData();

            Received.InOrder(
                () =>
                {
                    _mockModel.Reset();
                    _mockConfigStore.Store("Version", Arg.Any<string>());
                    _mockConfigStore.Get("Version");
                }
            );
        }
        #endregion

        #region Save Data Tests
        [TestMethod]
        public void SaveDataShouldStoreBuildInfoVersion()
        {
            Construct();
            _mockBuildInfo.Version.Returns("asdf");

            _sut.SaveData();

            _mockConfigStore.Received(1).Store("Version", "asdf");
        }

        [TestMethod]
        public void SaveDataShouldStoreModelField_GamesPlayed()
        {
            Construct();
            _mockModel.GamesPlayed.Returns((ulong)3);
            _mockValueConverterManager.ConvertToString<ulong>((ulong)3).Returns("asdf");

            _sut.SaveData();

            _mockConfigStore.Received(1).Store("GamesPlayed", "asdf");
        }

        [TestMethod]
        public void SaveDataShouldStoreModelField_GamesWon()
        {
            Construct();
            _mockModel.GamesWon.Returns((ulong)3);
            _mockValueConverterManager.ConvertToString<ulong>((ulong)3).Returns("asdf");

            _sut.SaveData();

            _mockConfigStore.Received(1).Store("GamesWon", "asdf");
        }

        [TestMethod]
        public void SaveDataShouldStoreModelField_TotalGuesses()
        {
            Construct();
            _mockModel.TotalGuesses.Returns((ulong)3);
            _mockValueConverterManager.ConvertToString<ulong>((ulong)3).Returns("asdf");

            _sut.SaveData();

            _mockConfigStore.Received(1).Store("TotalGuesses", "asdf");
        }

        [TestMethod]
        public void SaveDataShouldStoreModelField_LeastGuessesToWin()
        {
            Construct();
            _mockModel.LeastGuessesToWin.Returns((ulong)3);
            _mockValueConverterManager.ConvertToString<ulong>((ulong)3).Returns("asdf");

            _sut.SaveData();

            _mockConfigStore.Received(1).Store("LeastGuessesToWin", "asdf");
        }

        [TestMethod]
        public void SaveDataShouldStoreModelField_CurrentWinStreak()
        {
            Construct();
            _mockModel.CurrentWinStreak.Returns((ulong)3);
            _mockValueConverterManager.ConvertToString<ulong>((ulong)3).Returns("asdf");

            _sut.SaveData();

            _mockConfigStore.Received(1).Store("CurrentWinStreak", "asdf");
        }

        [TestMethod]
        public void SaveDataShouldStoreModelField_CurrentLossStreak()
        {
            Construct();
            _mockModel.CurrentLossStreak.Returns((ulong)3);
            _mockValueConverterManager.ConvertToString<ulong>((ulong)3).Returns("asdf");

            _sut.SaveData();

            _mockConfigStore.Received(1).Store("CurrentLossStreak", "asdf");
        }

        [TestMethod]
        public void SaveDataShouldStoreModelField_TotalGameTime()
        {
            Construct();
            _mockModel.TotalGameTime.Returns(TimeSpan.FromSeconds(3));
            _mockValueConverterManager.ConvertToString<TimeSpan>(TimeSpan.FromSeconds(3)).Returns("asdf");

            _sut.SaveData();

            _mockConfigStore.Received(1).Store("TotalGameTime", "asdf");
        }

        [TestMethod]
        public void SaveDataShouldStoreModelField_FastestWin()
        {
            Construct();
            _mockModel.FastestWin.Returns(TimeSpan.FromSeconds(3));
            _mockValueConverterManager.ConvertToString<TimeSpan>(TimeSpan.FromSeconds(3)).Returns("asdf");

            _sut.SaveData();

            _mockConfigStore.Received(1).Store("FastestWin", "asdf");
        }

        [TestMethod]
        public void SaveDataShouldStoreModelField_SlowestWin()
        {
            Construct();
            _mockModel.SlowestWin.Returns(TimeSpan.FromSeconds(3));
            _mockValueConverterManager.ConvertToString<TimeSpan>(TimeSpan.FromSeconds(3)).Returns("asdf");

            _sut.SaveData();

            _mockConfigStore.Received(1).Store("SlowestWin", "asdf");
        }

        [TestMethod]
        public void SaveDataShouldCallConfigStorePersistCache()
        {
            Construct();

            _sut.SaveData();

            _mockConfigStore.Received(1).PersistCache();
        }
        #endregion

        #region DataLoaded Event Tests
        [TestMethod]
        public void LoadDataFirstTimeShouldTriggerDataLoadedEventWithExpectedArguments_TestA()
        {
            Construct();
            EventHandler<GameStatsDataLoadedEventArgs> mockHandler = Substitute.For<EventHandler<GameStatsDataLoadedEventArgs>>();
            _sut.DataLoaded += mockHandler;

            _mockBuildInfo.Version.Returns("1.2.3.4");
            _mockConfigStore.Get("Version").Returns("2.3.4.5");

            _sut.LoadData();

            mockHandler.Received(1)(
                _sut,
                Arg.Is<GameStatsDataLoadedEventArgs>(
                    x => true
                        && x.FirstLoad == true
                        && x.FileVersion == new Version("2.3.4.5")
                        && x.CurrentVersion == new Version("1.2.3.4")
                )
            );
        }

        [TestMethod]
        public void LoadDataFirstTimeShouldTriggerDataLoadedEventWithExpectedArguments_TestB()
        {
            Construct();
            EventHandler<GameStatsDataLoadedEventArgs> mockHandler = Substitute.For<EventHandler<GameStatsDataLoadedEventArgs>>();
            _sut.DataLoaded += mockHandler;

            _mockBuildInfo.Version.Returns("5.2.4.3");
            _mockConfigStore.Get("Version").Returns("8.8.9.2");

            _sut.LoadData();

            mockHandler.Received(1)(
                _sut,
                Arg.Is<GameStatsDataLoadedEventArgs>(
                    x => true
                        && x.FirstLoad == true
                        && x.FileVersion == new Version("8.8.9.2")
                        && x.CurrentVersion == new Version("5.2.4.3")
                )
            );
        }

        [TestMethod]
        public void LoadDataSubsequentTimeShouldTriggerDataLoadedEventWithExpectedArguments()
        {
            Construct();
            EventHandler<GameStatsDataLoadedEventArgs> mockHandler = Substitute.For<EventHandler<GameStatsDataLoadedEventArgs>>();
            _sut.DataLoaded += mockHandler;

            _mockBuildInfo.Version.Returns("5.2.4.3");
            _mockConfigStore.Get("Version").Returns("8.8.9.2");

            _sut.LoadData();
            mockHandler.ClearReceivedCalls();

            _sut.LoadData();

            mockHandler.Received(1)(
                _sut,
                Arg.Is<GameStatsDataLoadedEventArgs>(
                    x => true
                        && x.FirstLoad == false
                        && x.FileVersion == new Version("8.8.9.2")
                        && x.CurrentVersion == new Version("5.2.4.3")
                )
            );
        }
        #endregion

        #region DataSaved Event Tests
        [TestMethod]
        public void SaveDataShouldTriggerDataSavedEvent()
        {
            Construct();
            var eventHandler = Substitute.For<EventHandler<EventArgs>>();
            _sut.DataSaved += eventHandler;

            _sut.SaveData();

            eventHandler.Received(1)(_sut, Arg.Any<EventArgs>());
        }
        #endregion
    }
}
