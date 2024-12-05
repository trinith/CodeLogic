using ArbitraryPixel.CodeLogic.Common.Theme;
using ArbitraryPixel.CodeLogic.Common.Theme.Device;
using ArbitraryPixel.Common.Audio;
using ArbitraryPixel.Common.Audio.Factory;
using ArbitraryPixel.Common.ContentManagement;
using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Common.Graphics.Factory;
using ArbitraryPixel.Platform2D.Assets;
using ArbitraryPixel.Platform2D.Audio;
using ArbitraryPixel.Platform2D.Engine;
using ArbitraryPixel.Platform2D.Scene;
using ArbitraryPixel.Platform2D.Theme;

namespace ArbitraryPixel.CodeLogic.Common.Scenes
{
    /// <summary>
    /// Scene to load all the assets needed for device visuals.
    /// </summary>
    public class DeviceAssetLoadScene : SceneBase
    {
        private IDeviceTheme _theme;

        public DeviceAssetLoadScene(IEngine host)
            : base(host)
        {
            _theme = this.Host.GetComponent<IThemeManagerCollection>()[ThemeObjectType.Device].GetCurrentTheme<IDeviceTheme>();
        }

        protected override void OnLoadAssetBank(IContentManager content, IAssetBank bank)
        {
            base.OnLoadAssetBank(content, bank);

            LoadTextures(content, bank);
            LoadFonts(content, bank);
            LoadMusic(content, bank);
            LoadSounds(content, bank);
        }

        private void LoadTextures(IContentManager content, IAssetBank bank)
        {
            ITexture2DFactory textureFactory = this.Host.GrfxFactory.Texture2DFactory;
            bank.Put<ITexture2D>("DeviceBackground", textureFactory.Create(content, _theme.GetFullAssetName("DeviceBackground")));

            bank.Put<ITexture2D>("SequenceSubmitButtonForeground", textureFactory.Create(content, _theme.GetFullAssetName("SequenceSubmitButtonForeground")));
            bank.Put<ITexture2D>("SequenceSubmitButtonBackground", textureFactory.Create(content, _theme.GetFullAssetName("SequenceSubmitButtonBackground")));
            bank.Put<ITexture2D>("SequenceSubmitButtonHoldOverlay", textureFactory.Create(content, _theme.GetFullAssetName("SequenceSubmitButtonHoldOverlay")));

            bank.Put<ITexture2D>("StatusBarBackgroundFill", textureFactory.Create(content, _theme.GetFullAssetName("StatusBarBackgroundFill")));
            bank.Put<ITexture2D>("StatusBarBackgroundBorder", textureFactory.Create(content, _theme.GetFullAssetName("StatusBarBackgroundBorder")));
            bank.Put<ITexture2D>("StatusBarProgressFrame", textureFactory.Create(content, _theme.GetFullAssetName("StatusBarProgressFrame")));
            bank.Put<ITexture2D>("StatusBarProgressFill", textureFactory.Create(content, _theme.GetFullAssetName("StatusBarProgressFill")));
            bank.Put<ITexture2D>("StatusBarProgressFillBackground", textureFactory.Create(content, _theme.GetFullAssetName("StatusBarProgressFillBackground")));
            bank.Put<ITexture2D>("StatusBarCurrentTrialIndicator", textureFactory.Create(content, _theme.GetFullAssetName("StatusBarCurrentTrialIndicator")));

            bank.Put<ITexture2D>("SceneChangeButtonFill", textureFactory.Create(content, _theme.GetFullAssetName("SceneChangeButtonFill")));
            bank.Put<ITexture2D>("SceneChangeButtonBorder", textureFactory.Create(content, _theme.GetFullAssetName("SceneChangeButtonBorder")));

            bank.Put<ITexture2D>("HexButtonBorder", textureFactory.Create(content, _theme.GetFullAssetName("HexButtonBorder")));
            bank.Put<ITexture2D>("HexButtonFill", textureFactory.Create(content, _theme.GetFullAssetName("HexButtonFill")));
            bank.Put<ITexture2D>("HexButtonIcon", textureFactory.Create(content, _theme.GetFullAssetName("HexButtonIcon")));
            bank.Put<ITexture2D>("HexButtonSelectorFill", textureFactory.Create(content, _theme.GetFullAssetName("HexButtonSelectorFill")));
            bank.Put<ITexture2D>("HexButtonSelectorColours", textureFactory.Create(content, _theme.GetFullAssetName("HexButtonSelectorColours")));
            bank.Put<ITexture2D>("HexButtonSelectorBorder", textureFactory.Create(content, _theme.GetFullAssetName("HexButtonSelectorBorder")));
            bank.Put<ITexture2D>("HexButtonSelectorHighlight", textureFactory.Create(content, _theme.GetFullAssetName("HexButtonSelectorHighlight")));

            bank.Put<ITexture2D>("InputBackgroundBorder", textureFactory.Create(content, _theme.GetFullAssetName("InputBackgroundBorder")));
            bank.Put<ITexture2D>("InputBackgroundFill", textureFactory.Create(content, _theme.GetFullAssetName("InputBackgroundFill")));

            bank.Put<ITexture2D>("DeviceMenuButtonBackground", textureFactory.Create(content, _theme.GetFullAssetName("DeviceMenuButtonBackground")));
            bank.Put<ITexture2D>("DeviceMenuButtonBorder", textureFactory.Create(content, _theme.GetFullAssetName("DeviceMenuButtonBorder")));
            bank.Put<ITexture2D>("DeviceMenuButtonAbort", textureFactory.Create(content, _theme.GetFullAssetName("DeviceMenuButtonAbort")));
            bank.Put<ITexture2D>("DeviceMenuButtonSettings", textureFactory.Create(content, _theme.GetFullAssetName("DeviceMenuButtonSettings")));

            bank.Put<ITexture2D>("IconMenu", textureFactory.Create(content, _theme.GetFullAssetName("IconMenu")));
            bank.Put<ITexture2D>("IconBack", textureFactory.Create(content, _theme.GetFullAssetName("IconBack")));
            bank.Put<ITexture2D>("IconLog", textureFactory.Create(content, _theme.GetFullAssetName("IconLog")));

            bank.Put<ITexture2D>("LogButtonFill", textureFactory.Create(content, _theme.GetFullAssetName("LogButtonFill")));
            bank.Put<ITexture2D>("LogButtonBorder", textureFactory.Create(content, _theme.GetFullAssetName("LogButtonBorder")));

            bank.Put<ITexture2D>("HistoryIndexChoice", textureFactory.Create(content, _theme.GetFullAssetName("HistoryIndexChoice")));
            bank.Put<ITexture2D>("HistoryAttemptFrameBackground", textureFactory.Create(content, _theme.GetFullAssetName("HistoryAttemptFrameBackground")));
            bank.Put<ITexture2D>("HistoryAttemptFrame", textureFactory.Create(content, _theme.GetFullAssetName("HistoryAttemptFrame")));
            bank.Put<ITexture2D>("HistoryPartial", textureFactory.Create(content, _theme.GetFullAssetName("HistoryPartial")));
            bank.Put<ITexture2D>("HistoryEqual", textureFactory.Create(content, _theme.GetFullAssetName("HistoryEqual")));
        }

        private void LoadFonts(IContentManager content, IAssetBank bank)
        {
            ISpriteFontFactory fontFactory = this.Host.GrfxFactory.SpriteFontFactory;

            bank.Put<ISpriteFont>("TitleFont", fontFactory.Create(content, _theme.GetFullAssetName("TitleFont")));

            bank.Put<ISpriteFont>("SceneChangeButtonFont", fontFactory.Create(content, _theme.GetFullAssetName("SceneChangeButtonFont")));

            bank.Put<ISpriteFont>("HistoryAttemptIndexFont", fontFactory.Create(content, _theme.GetFullAssetName("HistoryAttemptIndexFont")));
        }

        private void LoadMusic(IContentManager content, IAssetBank bank)
        {
            IMusicController musicController = this.Host.AudioManager.MusicController;

            bank.Put<ISong>("Gameplay", musicController.CreateSong(content, @"Music\Gameplay"));
        }

        private void LoadSounds(IContentManager content, IAssetBank bank)
        {
            ISoundController soundController = this.Host.AudioManager.SoundController;

            bank.Put<ISoundResource>("IndexValueChanged", soundController.CreateSoundResource(content, _theme.GetFullAssetName("IndexValueChanged")));
            bank.Put<ISoundResource>("ButtonPress", soundController.CreateSoundResource(content, _theme.GetFullAssetName("ButtonPress")));
            bank.Put<ISoundResource>("SubmitSequence", soundController.CreateSoundResource(content, _theme.GetFullAssetName("SubmitSequence")));
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();

            // All we needed to do was load assets. Move to the first scene in device.
            this.SceneComplete = true;
            this.NextScene = this.Host.Scenes["PreGameAd"];
        }
    }
}
