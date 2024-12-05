using ArbitraryPixel.CodeLogic.Common.Config;
using ArbitraryPixel.CodeLogic.Common.Entities;
using ArbitraryPixel.CodeLogic.Common.UI;
using ArbitraryPixel.Common;
using ArbitraryPixel.Common.Audio;
using ArbitraryPixel.Common.Drawing;
using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Platform2D.Engine;
using ArbitraryPixel.Platform2D.Entity;
using Microsoft.Xna.Framework;
using System;

namespace ArbitraryPixel.CodeLogic.Common.Layers
{
    public interface IMenuSettingsContentLayer : IMenuContentLayer
    {
    }

    public class MenuSettingsContentLayer : MenuContentLayerBase, IMenuSettingsContentLayer
    {
        private ICheckButton _soundEnabled;
        private ISlider _soundVolume;

        private ICheckButton _musicEnabled;
        private ISlider _musicVolume;

        private SizeF _padding = new SizeF(20);
        private IAudioControlsFactory _audioControlFactory;

        public MenuSettingsContentLayer(IEngine host, ISpriteBatch mainSpriteBatch, RectangleF contentBounds, IAudioControlsFactory audioControlsFactory)
            : base(host, mainSpriteBatch, contentBounds)
        {
            _audioControlFactory = audioControlsFactory ?? throw new ArgumentNullException();

            Vector2 anchor = this.ContentBounds.Location + _padding;
            anchor = SetupSoundControls(anchor);
            anchor = SetupMusicControls(anchor);

            UpdateControls();
        }

        protected override void OnShow()
        {
            base.OnShow();

            UpdateControls();
        }

        private Vector2 SetupSoundControls(Vector2 anchor)
        {
            AudioControlsFactoryResult controls = _audioControlFactory.CreateControls(anchor, _padding, this.MainSpriteBatch, "MainMenuContentFont", "Enable Sound", "Sound Volume");
            foreach (IEntity entity in controls.Entities)
                this.AddEntity(entity);

            _soundEnabled = controls.EnableControl;
            _soundVolume = controls.VolumeControl;

            return controls.NextAnchor;
        }

        private Vector2 SetupMusicControls(Vector2 anchor)
        {
            AudioControlsFactoryResult controls = _audioControlFactory.CreateControls(anchor, _padding, this.MainSpriteBatch, "MainMenuContentFont", "Enable Music", "Music Volume");
            foreach (IEntity entity in controls.Entities)
                this.AddEntity(entity);

            _musicEnabled = controls.EnableControl;
            _musicVolume = controls.VolumeControl;

            return controls.NextAnchor;
        }

        private void Handle_SoundEnabledCheckStateChanged(object sender, EventArgs e)
        {
            this.Host.AudioManager.SoundController.Enabled = ((ICheckButton)sender).Checked;
            this.Host.GetComponent<ICodeLogicSettings>().SoundEnabled = this.Host.AudioManager.SoundController.Enabled;

            this.Host.AssetBank.Get<ISoundResource>("ButtonPress").Play();
        }

        private void Handle_SoundVolumeSliderValueChanged(object sender, StateChangedEventArgs<float> e)
        {
            this.Host.AudioManager.SoundController.Volume = e.CurrentState;
            this.Host.GetComponent<ICodeLogicSettings>().SoundVolume = this.Host.AudioManager.SoundController.Volume;
        }

        private void Handle_MusicEnabledCheckStateChanged(object sender, EventArgs e)
        {
            this.Host.AssetBank.Get<ISoundResource>("ButtonPress").Play();
            this.Host.AudioManager.MusicController.Enabled = ((ICheckButton)sender).Checked;
            this.Host.GetComponent<ICodeLogicSettings>().MusicEnabled = this.Host.AudioManager.MusicController.Enabled;
        }

        private void Handle_MusicVolumeSliderValueChanged(object sender, StateChangedEventArgs<float> e)
        {
            this.Host.AudioManager.MusicController.Volume = e.CurrentState;
            this.Host.GetComponent<ICodeLogicSettings>().MusicVolume = this.Host.AudioManager.MusicController.Volume;
        }

        private void UpdateControls()
        {
            _soundEnabled.CheckStateChanged -= Handle_SoundEnabledCheckStateChanged;
            _soundVolume.ValueChanged -= Handle_SoundVolumeSliderValueChanged;
            _musicEnabled.CheckStateChanged -= Handle_MusicEnabledCheckStateChanged;
            _musicVolume.ValueChanged -= Handle_MusicVolumeSliderValueChanged;

            ICodeLogicSettings settings = this.Host.GetComponent<ICodeLogicSettings>();

            _soundEnabled.Checked = settings.SoundEnabled;
            _soundVolume.Value = settings.SoundVolume;

            _musicEnabled.Checked = settings.MusicEnabled;
            _musicVolume.Value = settings.MusicVolume;

            _soundEnabled.CheckStateChanged += Handle_SoundEnabledCheckStateChanged;
            _soundVolume.ValueChanged += Handle_SoundVolumeSliderValueChanged;
            _musicEnabled.CheckStateChanged += Handle_MusicEnabledCheckStateChanged;
            _musicVolume.ValueChanged += Handle_MusicVolumeSliderValueChanged;
        }
    }
}
