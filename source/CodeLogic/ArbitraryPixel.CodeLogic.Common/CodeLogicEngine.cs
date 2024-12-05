using ArbitraryPixel.CodeLogic.Common.Config;
using ArbitraryPixel.CodeLogic.Common.Controllers;
using ArbitraryPixel.CodeLogic.Common.Model;
using ArbitraryPixel.CodeLogic.Common.UI;
using ArbitraryPixel.Common.Drawing;
using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Common.Screen;
using ArbitraryPixel.Common.ValueConversion;
using ArbitraryPixel.Platform2D.Engine;
using ArbitraryPixel.Platform2D.Layer;
using ArbitraryPixel.Platform2D.Scene;
using ArbitraryPixel.Platform2D.Text;
using ArbitraryPixel.Platform2D.Theme;
using ArbitraryPixel.Platform2D.Time;
using Microsoft.Xna.Framework;

namespace ArbitraryPixel.CodeLogic.Common
{
    public class CodeLogicEngine : EngineBase
    {
        #region Constants Declaration
        public static class Constants
        {
            public enum ExternalActions
            {
                BackPressed,
            }

            public const string DialogText_ExitConfirmation = "{TPC:0}{C:White}Are you sure you wish to exit the game?";
            public const string DialogText_ResetStatisticsConfirmation = "{TPC:0}{C:White}Are you sure you wish reset all game statistics?";
            public const string DialogText_NoStoragePermissions = ""
                + "{TPC:0}{Alignment:Centre}{Color:White}"
                + "Information\n"
                + "\n"
                + "{Alignment:Left}{Color:128,128,128}"
                + "Storage permissions were not granted to CodeLogic so settings and\n"
                + "gameplay statistics can\'t be saved for next time. If this functionality\n"
                + "is desired, exit CodeLogic completely, restart, and accept the\n"
                + "permission request prompt on start up.\n"
                + "\n"
                + "Tap outside this dialog to close.";

            public const int MaximumTrials = 10;
            public const double DeviceSceneTransitionTime = 0.125;
            public const float FadeSceneTransitionTime = 0.5f;
            public const float FadeSceneTransitionTimeHalf = FadeSceneTransitionTime / 2f;
            public const float SplashScreenFadeTime = 1f;
            public const float SplashScreenFadeInDelay = 1f;
            public const double SplashScreenViewTime = 2.0;

            public const float MenuItemHeight = 100f;

            public static readonly SizeF MenuButtonSize = new SizeF(200, 75);
            public static readonly SizeF AdProgressBarSize = new SizeF(600, 25);
            public static readonly SizeF TextWindowPadding = new SizeF(10f);
            public static readonly SizeF TextWindowBorderSize = new SizeF(2f);

            public static readonly Color ClrMenuBG = new Color(0, 0, 0, 192);
            public static readonly Color ClrMenuBGHigh = new Color(128, 128, 128, 128);
            public static readonly Color ClrMenuBGMid = new Color(128, 128, 128, 64);
            public static readonly Color ClrMenuBGLow = new Color(64, 64, 64, 64);
            public static readonly Color ClrMenuTextSelected = new Color(255, 255, 255);
            public static readonly Color ClrMenuTextNormal = new Color(128, 128, 128);
            public static readonly Color ClrMenuFGHigh = new Color(255, 255, 255);
            public static readonly Color ClrMenuFGMid = new Color(128, 128, 128);
            public static readonly Color ClrMenuFGLow = new Color(32, 32, 32);
        }
        #endregion

        #region Private Members
        private IDeviceModel _deviceModel = null;
        private ILogPanelModel _logPanelModel = null;
        private ILayer _overlayLayer = null;
        private IBuildInfoStore _buildInfoStore = null;
        #endregion

        #region Public Properties
        public bool RenderBuildInfoOverlay { get; set; } = false;
        #endregion

        #region Constructor
        public CodeLogicEngine(IComponentContainer componentContainer)
            : base(componentContainer)
        {
            if (GameObjectFactory.Instance == null)
                throw new GameObjectFactoryInstanceNullException();

            this.RegisterComponent<IThemeManagerCollection>(GameObjectFactory.Instance.CreateThemeManagerCollection());

            _buildInfoStore = this.GetComponent<IBuildInfoStore>() ?? throw new RequiredComponentMissingException();
            var chkCodeLogicSettings = this.GetComponent<ICodeLogicSettings>() ?? throw new RequiredComponentMissingException();
            var chkStopwatchManager = this.GetComponent<IStopwatchManager>() ?? throw new RequiredComponentMissingException();
            var chkValueConverterManager = this.GetComponent<IValueConverterManager>() ?? throw new RequiredComponentMissingException();

#if RENDER_BUILDINFO_OVERLAY // || DEBUG
            this.RenderBuildInfoOverlay = true;
#endif
        }
        #endregion

        #region Override Methods
        protected override void OnInitialize()
        {
            ICodeLogicSettings settings = this.GetComponent<ICodeLogicSettings>();

            this.AudioManager.SoundController.Enabled = settings.SoundEnabled;
            this.AudioManager.SoundController.Volume = settings.SoundVolume;

            this.AudioManager.MusicController.IsRepeating = true;
            this.AudioManager.MusicController.Enabled = settings.MusicEnabled;
            this.AudioManager.MusicController.VolumeAttenuation = 0f;
            this.AudioManager.MusicController.Volume = settings.MusicVolume;

            base.OnInitialize();
        }

        protected override void OnLoadContent()
        {
            // Create a general pixel texture and store in asset bank for general use.
            ITexture2D pixelTexture = this.GrfxFactory.Texture2DFactory.Create(this.Graphics.GraphicsDevice, 1, 1);
            pixelTexture.SetData<Color>(new Color[] { Color.White });
            this.AssetBank.Put<ITexture2D>("Pixel", pixelTexture);

            // Setup screen manager background textures.
            ScreenManagerOptions screenManagerOptions = new ScreenManagerOptions(this.GrfxFactory.SpriteBatchFactory.Create(this.Graphics.GraphicsDevice));
            screenManagerOptions.ScreenBackground = new BackgroundTextureDefinition(pixelTexture, Color.Black);
            screenManagerOptions.WorldBackground = new BackgroundTextureDefinition(pixelTexture, Color.CornflowerBlue);
            this.ScreenManager.Options = screenManagerOptions;

            // Add a debug font for general use.
            this.AssetBank.Put<ISpriteFont>("Debug", this.GrfxFactory.SpriteFontFactory.Create(this.Content, @"Fonts\Debug"));

            ITextObjectBuilder textBuilder = GameObjectFactory.Instance.CreateTextObjectBuilder(
                GameObjectFactory.Instance.CreateTextFormatProcessor(GameObjectFactory.Instance.CreateTextFormatValueHandlerManager()),
                GameObjectFactory.Instance.CreateTextObjectFactory()
            );
            textBuilder.DefaultFont = this.GrfxFactory.SpriteFontFactory.Create(this.Content, @"Fonts\OverlayFont");

            _overlayLayer = GameObjectFactory.Instance.CreateBuildInfoOverlayLayer(
                this,
                this.GrfxFactory.SpriteBatchFactory.Create(this.Graphics.GraphicsDevice),
                GameObjectFactory.Instance.CreateBuildInfoOverlayModel(_buildInfoStore, textBuilder),
                GameObjectFactory.Instance.CreateRandom()
            );

            // Create a model for the device.
            _deviceModel = GameObjectFactory.Instance.CreateDeviceModel(GameObjectFactory.Instance.CreateRandom(), this.GetComponent<IStopwatchManager>());

            // Create a model for the log panel.
            _logPanelModel = GameObjectFactory.Instance.CreateLogPanelModel(this.GetComponent<ICodeLogicSettings>(), (SizeF)this.ScreenManager.World);
            _logPanelModel.ClosedSize = new SizeF(_logPanelModel.WorldBounds.Width, 0);
            _logPanelModel.PartialSize = new SizeF(_logPanelModel.WorldBounds.Width, 44);
            _logPanelModel.FullSize = new SizeF(_logPanelModel.WorldBounds.Width, 118);
            _logPanelModel.SetOffsetForMode();

            // Clear scenes and then build new ones.
            this.Scenes.Clear();

            // Splash Screen
            this.Scenes.Add("SplashScreen", GameObjectFactory.Instance.CreateSplashScreenScene(this, GameObjectFactory.Instance.CreateUIObjectFactory()));

            // Main Menu
            IMenuFactory menuFactory = GameObjectFactory.Instance.CreateMenuFactory();
            IMenuContentMap menuContentMap = GameObjectFactory.Instance.CreateMenuContentMap();
            IMainMenuContentLayerFactory contentFactory = GameObjectFactory.Instance.CreateMainMenuContentLayerFactory();
            IMainMenuModel mainMenu = GameObjectFactory.Instance.CreateMainMenuModel(this, menuFactory, menuContentMap, contentFactory);
            this.Scenes.Add("MainMenu", GameObjectFactory.Instance.CreateMainMenuScene(this, mainMenu, menuFactory, this.GetComponent<IGameStatsData>()));

            // Device Scenes
            this.Scenes.Add("DeviceAssetLoad", GameObjectFactory.Instance.CreateDeviceAssetLoadScene(this));
            this.Scenes.Add("DeviceBoot", GameObjectFactory.Instance.CreateDeviceBootScene(this, _deviceModel, _logPanelModel));
            this.Scenes.Add("DeviceMain", GameObjectFactory.Instance.CreateDeviceMainScene(this, _deviceModel, _logPanelModel));
            this.Scenes.Add("DeviceMenu", GameObjectFactory.Instance.CreateDeviceMenuScene(this, _deviceModel));

            // Mission Debriefing Scene
            IGameStatsController gameStatsController = GameObjectFactory.Instance.CreateGameStatsController(this.GetComponent<IGameStatsData>().Model);
            gameStatsController.Updated += (sender, e) => this.GetComponent<IGameStatsData>().SaveData();
            this.Scenes.Add("MissionDebriefing", GameObjectFactory.Instance.CreateMissionDebriefingScene(this, _deviceModel, gameStatsController));

            // PreGameAd Scenes
            this.Scenes.Add("PreGameAd", GameObjectFactory.Instance.CreatePreGameAdScene(this));
            this.Scenes.Add("NoAdMessage", GameObjectFactory.Instance.CreateNoAdMessageScene(this));

            // StartScene
            this.Scenes.Add("GameStart", GameObjectFactory.Instance.CreateGameStartupScene(this, this.Scenes["SplashScreen"]));

            foreach (IScene scene in this.Scenes.Values)
            {
                scene.Initialize();
            }

            // Start scene the game.
            this.CurrentScene = this.Scenes["GameStart"];

            // Set the next scene to whatever... for testing purposes :)
            //_scene.NextScene = this.Scenes["DeviceAssetLoad"];
            //this.Scenes["DeviceAssetLoad"].NextScene = this.Scenes["DeviceMain"];

            base.OnLoadContent();
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            if (this.RenderBuildInfoOverlay)
                _overlayLayer.Update(gameTime);

            base.OnUpdate(gameTime);
        }

        protected override void OnDraw(GameTime gameTime)
        {
            base.OnDraw(gameTime);

            if (this.RenderBuildInfoOverlay)
                _overlayLayer.Draw(gameTime);
        }

        protected override void OnExit()
        {
            ICodeLogicSettings settings = this.GetComponent<ICodeLogicSettings>();

            if (settings.CacheChanged)
                settings.PersistCache();

            base.OnExit();
        }

        protected override void OnSuspend()
        {
            this.GetComponent<IStopwatchManager>().Stop();

            base.OnSuspend();
        }

        protected override void OnResume()
        {
            this.GetComponent<IStopwatchManager>().Start();

            base.OnResume();
        }
        #endregion
    }
}
