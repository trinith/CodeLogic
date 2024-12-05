using ArbitraryPixel.CodeLogic.Common.Model;
using ArbitraryPixel.Platform2D.Config;
using System;

namespace ArbitraryPixel.CodeLogic.Common.Config
{
    public interface ICodeLogicSettings : IConfigStore
    {
        bool MusicEnabled { get; set; }
        bool SoundEnabled { get; set; }
        float SoundVolume { get; set; }
        float MusicVolume { get; set; }
        LogPanelMode LogPanelMode { get; set; }
    }

    public class CodeLogicSettings : ICodeLogicSettings
    {
        private IConfigStore _configStore;

        public const string CFGMusicEnabled = "MusicEnabled";
        public const string CFGSoundEnabled = "SoundEnabled";
        public const string CFGSoundVolume = "SoundVolume";
        public const string CFGMusicVolume = "MusicVolume";
        public const string CFGLogPanelMode = "LogPanelMode";

        private const bool DEFSoundEnabled = true;
        private const float DEFSoundVolume = 0.5f;
        private const bool DEFMusicEnabled = true;
        private const float DEFMusicVolume = 0.25f;
        private const LogPanelMode DEFLogPanelMode = LogPanelMode.FullView;

        #region ICodeLogicSettings Implementation
        public bool SoundEnabled
        {
            get
            {
                AffirmValue<bool>(CFGSoundEnabled, DEFSoundEnabled);

                return bool.Parse(_configStore.Get(CFGSoundEnabled));
            }

            set
            {
                _configStore.Store(CFGSoundEnabled, value.ToString());
            }
        }

        public float SoundVolume
        {
            get
            {
                AffirmValue<float>(CFGSoundVolume, DEFSoundVolume);

                return float.Parse(_configStore.Get(CFGSoundVolume));
            }

            set
            {
                _configStore.Store(CFGSoundVolume, value.ToString());
            }
        }

        public bool MusicEnabled
        {
            get
            {
                AffirmValue<bool>(CFGMusicEnabled, DEFMusicEnabled);

                return bool.Parse(_configStore.Get(CFGMusicEnabled));
            }

            set
            {
                _configStore.Store(CFGMusicEnabled, value.ToString());
            }
        }

        public float MusicVolume
        {
            get
            {
                AffirmValue<float>(CFGMusicVolume, DEFMusicVolume);

                return float.Parse(_configStore.Get(CFGMusicVolume));
            }

            set
            {
                _configStore.Store(CFGMusicVolume, value.ToString());
            }
        }

        public LogPanelMode LogPanelMode
        {
            get
            {
                if (!_configStore.ContainsKey(CFGLogPanelMode))
                    _configStore.Store(CFGLogPanelMode, DEFLogPanelMode.ToString());

                return (LogPanelMode)Enum.Parse(typeof(LogPanelMode), _configStore.Get(CFGLogPanelMode));
            }

            set
            {
                _configStore.Store(CFGLogPanelMode, value.ToString());
            }
        }
        #endregion

        public CodeLogicSettings(IConfigStore configStore)
        {
            _configStore = configStore ?? throw new ArgumentNullException();

            Initialize();
        }

        private void Initialize()
        {
            // Load cache data.
            _configStore.LoadCache();

            // Read all properties to get their values, or load defaults.
            var chkSoundEnabled = this.SoundEnabled;
            var chkSoundVolume = this.SoundVolume;
            var chkMusicEnabled = this.MusicEnabled;
            var chkMusicVolume = this.MusicVolume;
            var chkLogPanelMode = this.LogPanelMode;
        }

        private void AffirmValue<T>(string configKey, T defaultValue)
        {
            if (!_configStore.ContainsKey(configKey))
                _configStore.Store(configKey, defaultValue.ToString());
        }

        #region IConfigStore Implementation
        public bool IsTransient
        {
            get { return _configStore.IsTransient; }
            set { _configStore.IsTransient = value; }
        }

        public bool CacheChanged => _configStore.CacheChanged;

        public void Store(string key, string value)
        {
            _configStore.Store(key, value);
        }

        public string Get(string key)
        {
            return _configStore.Get(key);
        }

        public bool ContainsKey(string key)
        {
            return _configStore.ContainsKey(key);
        }

        public void PersistCache()
        {
            _configStore.PersistCache();
        }

        public void LoadCache()
        {
            _configStore.LoadCache();
        }
        #endregion
    }
}
