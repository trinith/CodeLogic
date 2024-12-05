using ArbitraryPixel.CodeLogic.Common.Model;
using ArbitraryPixel.Common;
using ArbitraryPixel.Common.ValueConversion;
using ArbitraryPixel.Platform2D.Config;
using System;

namespace ArbitraryPixel.CodeLogic.Common.Config
{
    public class GameStatsDataLoadedEventArgs : EventArgs
    {
        public bool FirstLoad { get; }
        public Version FileVersion { get; }
        public Version CurrentVersion { get; }

        public GameStatsDataLoadedEventArgs(bool isFirstLoad, string fileVersion, string currentVersion)
        {
            this.FirstLoad = isFirstLoad;
            this.FileVersion = new Version(fileVersion);
            this.CurrentVersion = new Version(currentVersion);
        }
    }

    public interface IGameStatsData
    {
        IGameStatsModel Model { get; }

        string CurrentVersion { get; }
        string DataVersion { get; }
        bool IsTransient { get; set; }

        event EventHandler<StateChangedEventArgs<bool>> IsTransientChanged;
        event EventHandler<GameStatsDataLoadedEventArgs> DataLoaded;
        event EventHandler<EventArgs> DataSaved;

        void LoadData();
        void SaveData();
    }

    public class GameStatsData : IGameStatsData
    {
        private IConfigStore _configStore;
        private IBuildInfoStore _buildInfo;
        private IValueConverterManager _valueConverterManager;

        private bool _firstLoad = true;
        private string _dataVersion = null;

        public GameStatsData(IConfigStore configStore, IGameStatsModel model, IBuildInfoStore buildInfo, IValueConverterManager valueConverterManager)
        {
            _configStore = configStore ?? throw new ArgumentNullException();
            this.Model = model ?? throw new ArgumentNullException();
            _buildInfo = buildInfo ?? throw new ArgumentNullException();
            _valueConverterManager = valueConverterManager ?? throw new ArgumentNullException();
        }

        #region IGameStatsData Implementation
        public IGameStatsModel Model { get; set; }
        public string CurrentVersion => _buildInfo.Version;
        public string DataVersion => _dataVersion;
        public bool IsTransient
        {
            get { return _configStore.IsTransient; }
            set
            {
                if (value != _configStore.IsTransient)
                {
                    _configStore.IsTransient = value;

                    if (this.IsTransientChanged != null)
                        this.IsTransientChanged(this, new StateChangedEventArgs<bool>(!value, value));
                }
            }
        }

        public event EventHandler<StateChangedEventArgs<bool>> IsTransientChanged;
        public event EventHandler<GameStatsDataLoadedEventArgs> DataLoaded;
        public event EventHandler<EventArgs> DataSaved;

        private void StoreModelValuesToCache()
        {
            _configStore.Store("Version", this.CurrentVersion);

            _configStore.Store("GamesPlayed", _valueConverterManager.ConvertToString<ulong>(this.Model.GamesPlayed));
            _configStore.Store("GamesWon", _valueConverterManager.ConvertToString<ulong>(this.Model.GamesWon));
            _configStore.Store("TotalGuesses", _valueConverterManager.ConvertToString<ulong>(this.Model.TotalGuesses));
            _configStore.Store("LeastGuessesToWin", _valueConverterManager.ConvertToString<ulong>(this.Model.LeastGuessesToWin));
            _configStore.Store("CurrentWinStreak", _valueConverterManager.ConvertToString<ulong>(this.Model.CurrentWinStreak));
            _configStore.Store("CurrentLossStreak", _valueConverterManager.ConvertToString<ulong>(this.Model.CurrentLossStreak));
            _configStore.Store("TotalGameTime", _valueConverterManager.ConvertToString<TimeSpan>(this.Model.TotalGameTime));
            _configStore.Store("FastestWin", _valueConverterManager.ConvertToString<TimeSpan>(this.Model.FastestWin));
            _configStore.Store("SlowestWin", _valueConverterManager.ConvertToString<TimeSpan>(this.Model.SlowestWin));
        }

        public void LoadData()
        {
            _configStore.LoadCache();

            if (!_configStore.ContainsKey("Version"))
            {
                // Didn't find any data so reset values to default.
                this.Model.Reset();
                StoreModelValuesToCache();
            }

            _dataVersion = _configStore.Get("Version");

            this.Model.GamesPlayed = _valueConverterManager.ConvertFromString<ulong>(_configStore.Get("GamesPlayed"));
            this.Model.GamesWon = _valueConverterManager.ConvertFromString<ulong>(_configStore.Get("GamesWon"));
            this.Model.TotalGuesses = _valueConverterManager.ConvertFromString<ulong>(_configStore.Get("TotalGuesses"));
            this.Model.LeastGuessesToWin = _valueConverterManager.ConvertFromString<ulong>(_configStore.Get("LeastGuessesToWin"));
            this.Model.CurrentWinStreak = _valueConverterManager.ConvertFromString<ulong>(_configStore.Get("CurrentWinStreak"));
            this.Model.CurrentLossStreak = _valueConverterManager.ConvertFromString<ulong>(_configStore.Get("CurrentLossStreak"));
            this.Model.TotalGameTime = _valueConverterManager.ConvertFromString<TimeSpan>(_configStore.Get("TotalGameTime"));
            this.Model.FastestWin = _valueConverterManager.ConvertFromString<TimeSpan>(_configStore.Get("FastestWin"));
            this.Model.SlowestWin = _valueConverterManager.ConvertFromString<TimeSpan>(_configStore.Get("SlowestWin"));

            if (this.DataLoaded != null)
                this.DataLoaded(this, new GameStatsDataLoadedEventArgs(_firstLoad, _dataVersion, _buildInfo.Version));

            _firstLoad = false;
        }

        public void SaveData()
        {
            StoreModelValuesToCache();
            _configStore.PersistCache();

            if (this.DataSaved != null)
                this.DataSaved(this, new EventArgs());
        }
        #endregion
    }
}
