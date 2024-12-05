using ArbitraryPixel.CodeLogic.Common.Model;
using ArbitraryPixel.Common;
using ArbitraryPixel.Common.Audio.MonoGame;
using ArbitraryPixel.Common.Audio.MonoGame.Factory;
using ArbitraryPixel.Common.ContentManagement;
using ArbitraryPixel.Common.ContentManagement.MonoGame;
using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Common.Graphics.Factory;
using ArbitraryPixel.Common.Graphics.MonoGame;
using ArbitraryPixel.Common.Graphics.MonoGame.Factory;
using ArbitraryPixel.Common.Json;
using ArbitraryPixel.Common.Json.JsonDotNET;
using ArbitraryPixel.Common.SimpleFileSystem;
using ArbitraryPixel.Common.SimpleFileSystem.PCLPortable;
using ArbitraryPixel.Common.ValueConversion;
using ArbitraryPixel.Platform2D.Assets;
using ArbitraryPixel.Platform2D.Audio;
using ArbitraryPixel.Platform2D.Engine;
using ArbitraryPixel.Platform2D.Logging;
using ArbitraryPixel.Platform2D.Time;
using ArbitraryPixel.Platform2D.UI.Factory;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace ArbitraryPixel.CodeLogic.Common
{
    public class MainGame : Game
    {
        private IComponentContainer _componentContainer;
        private IEngine _engine = null;
        private GraphicsDeviceManager _graphics;
        private GamePadState _previousGamePadState;

        public event EventHandler GameFinished;

        public new IComponentContainer Components => _componentContainer;

        public MainGame(IComponentContainer componentContainer)
        {
            Content.RootDirectory = "Content";

            _componentContainer = componentContainer ?? throw new ArgumentNullException();

            _graphics = new GraphicsDeviceManager(this);
        }

        #region Basic Component Registration
        public static void RegisterBasicDependencies(IComponentContainer container)
        {
            // Register some basic dependencies that our game will need. This can be called by any platform specific launchers.

            container.RegisterComponent<IDateTimeFactory>(new DotNetDateTimeFactory());
            container.RegisterComponent<IJsonConvert>(new JsonDotNETJsonConvert());
            container.RegisterComponent<ISimpleFileSystem>(new PCLStorageFileSystem());
            container.RegisterComponent<IValueConverterManager>(CreateComponent_IValueConverterManager());
            container.RegisterComponent<IBuildInfoStore>(new BuildInfoStore(container.GetComponent<ITargetPlatform>().Platform.ToString()));

            container.RegisterComponent<ILogger>(new ConsoleLogger(container.GetComponent<IDateTimeFactory>(), new DotNetDebug()) { UseTimeStamps = true });
        }

        private static IValueConverterManager CreateComponent_IValueConverterManager()
        {
            IValueConverterManager manager = new ValueConverterManager();

            manager.RegisterConverter<ulong>(new ULongValueConverter());
            manager.RegisterConverter<TimeSpan>(new TimeSpanValueConverter());

            return manager;
        }
        #endregion

        protected override void Initialize()
        {
            _graphics.SupportedOrientations = DisplayOrientation.LandscapeLeft | DisplayOrientation.LandscapeRight;
            this.Window.ClientSizeChanged += (sender, e) =>
            {
                _engine.ScreenManager.Screen = new Point(_graphics.GraphicsDevice.Viewport.Width, _graphics.GraphicsDevice.Viewport.Height);
                _engine.ScreenManager.ApplySettings(_engine.Graphics);
            };

            // Create MonoGame dependencies.
            _componentContainer.RegisterComponent<IGrfxFactory>(new MonoGameGrfxFactory());
            _componentContainer.RegisterComponent<IGrfxDeviceManager>(new MonoGameGraphicsDeviceManager(_graphics));
            _componentContainer.RegisterComponent<IContentManager>(new MonoGameContentManager(this.Content));
            _componentContainer.RegisterComponent<IAudioManager>(
                new AudioManager(
                    new MusicController(new MonoGameSongPlayer(new MonoGameSongFactory())),
                    new SoundController(new MonoGameSoundResourceFactory())
                )
            );

            // Create misc other dependencies.
            _componentContainer.RegisterComponent<IAssetBank>(new AssetBank());
            _componentContainer.RegisterComponent<IUniqueIdGenerator>(new UniqueIdGenerator(_componentContainer.GetComponent<IDateTimeFactory>()));
            _componentContainer.RegisterComponent<IStopwatchManager>(new StopwatchManager(new StopwatchFactory(_componentContainer.GetComponent<IDateTimeFactory>())));
            _componentContainer.RegisterComponent<IButtonControllerFactory>(new SingleTouchButtonControllerFactory());

            // Create a GameObjectFactory
            GameObjectFactory.SetInstance(new GameObjectFactory());

            _engine = new CodeLogicEngine(_componentContainer);
            _engine.Initialize();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _engine.LoadContent();

            base.LoadContent();
        }

        protected override void UnloadContent()
        {
            base.UnloadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            //////////////////////////////////////////////////////////////////////////////////////////////////////
            // NOTE: AndroidGameActivity.OnBackPressed doesn't work. This is the accepted MonoGame approach.
            //////////////////////////////////////////////////////////////////////////////////////////////////////
            GamePadState currentGamePadState = GamePad.GetState(0);
            if (currentGamePadState.IsButtonUp(Buttons.Back) && _previousGamePadState.IsButtonDown(Buttons.Back))
                _engine.TriggerExternalAction(CodeLogicEngine.Constants.ExternalActions.BackPressed);
            _previousGamePadState = currentGamePadState;
            //////////////////////////////////////////////////////////////////////////////////////////////////////

            _engine.InputManager.IsActive = this.IsActive;
            _engine.Update(gameTime);

            if (_engine.Finished)
            {
                if (this.GameFinished != null)
                    this.GameFinished(this, new EventArgs());
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            _engine.Draw(gameTime);

            base.Draw(gameTime);
        }

        public void Suspend()
        {
            _engine?.Suspend();
        }

        public void Resume()
        {
            _engine?.Resume();
        }
    }
}
