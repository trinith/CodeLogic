using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using Android.Views;
using ArbitraryPixel.CodeLogic.Common;
using ArbitraryPixel.CodeLogic.Common.Config;
using ArbitraryPixel.CodeLogic.Common.Model;
using ArbitraryPixel.Common;
using ArbitraryPixel.Common.Advertising;
using ArbitraryPixel.Common.Input;
using ArbitraryPixel.Common.Input.MonoGame;
using ArbitraryPixel.Common.Json;
using ArbitraryPixel.Common.Screen;
using ArbitraryPixel.Common.SimpleFileSystem;
using ArbitraryPixel.Common.ValueConversion;
using ArbitraryPixel.Platform2D.Config;
using ArbitraryPixel.Platform2D.Engine;
using ArbitraryPixel.Platform2D.Logging;
using Microsoft.Xna.Framework;
using System;
using System.IO;

namespace CodeLogic_Android
{
    [Activity(Label = "CodeLogic"
        , MainLauncher = true
        , Icon = "@drawable/icon"
        , Theme = "@style/Theme.Splash"
        , AlwaysRetainTaskState = true
        , LaunchMode = LaunchMode.SingleInstance
        , ScreenOrientation = ScreenOrientation.SensorLandscape
        , ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden | ConfigChanges.ScreenSize
    )]
    public class GameActivity : AndroidGameActivity
    {
        // App ID: ca-app-pub-4989860304002115~9385788325
        // Ad unit ID: ca-app-pub-4989860304002115/6348561444

        #region Private Members
        private const Platform PLATFORM = Platform.Android;
        private string _appStorageDir;

        private const string AD_UNIT_ID = "ca-app-pub-3940256099942544/1033173712"; // "ca-app-pub-4989860304002115/6348561444";

        private MainGame _game;
        #endregion

        #region Override Methods
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            this.Window.AddFlags(WindowManagerFlags.KeepScreenOn);

            _appStorageDir = Path.Combine(Android.OS.Environment.ExternalStorageDirectory.ToString(), "ArbitraryPixel", "CodeLogic");
            AppDomain.CurrentDomain.UnhandledException += Handle_UnhandledException;

            Android.Graphics.Point displaySize = new Android.Graphics.Point();
            this.WindowManager.DefaultDisplay.GetRealSize(displaySize);

            IScreenManager screenManager = new ScreenManager();
            screenManager.World = new Point(1280, 720);
            screenManager.Screen = new Point(displaySize.X, displaySize.Y);
            screenManager.IsFullScreen = true;

            //*/
            IAdProvider adProvider = null;
            /*/
            IAdProvider adProvider = new AndroidInterstitialAdProvider(new AndroidAdObjectFactory(), new AndroidAdContext(this), AD_UNIT_ID);
            adProvider.InitializeAds();
            adProvider.RequestAd();
            adProvider.AdClosed +=
                (sender, e) =>
                {
                    Thread t = new Thread(
                        () =>
                        {
                            Thread.Sleep(250);
                            if (CheckGameStatePostAd() == false)
                                ShowApplicationInvalidStateScreen();
                        }
                    );
                    t.Start();
                };
            //*/
            bool hasStoragePermissions = CheckStoragePermissions();

            IComponentContainer container = new SimpleComponentContainer();
            container.RegisterComponent<ITargetPlatform>(new AndroidPlatform());
            MainGame.RegisterBasicDependencies(container);

            container.RegisterComponent<IScreenManager>(screenManager);
            container.RegisterComponent<ISurfaceInputManager>(new TouchScreenSurfaceInputManager(new MonoGameTouchPanelManager()));
            container.RegisterComponent<ICodeLogicSettings>(CreateComponent_CodeLogicSettings(container, hasStoragePermissions));
            container.RegisterComponent<IGameStatsData>(CreateComponent_GameStatsData(container, hasStoragePermissions));

            if (adProvider != null)
                container.RegisterComponent<IAdProvider>(adProvider);

            _game = new MainGame(container);
            _game.GameFinished +=
                (sender, e) =>
                {
                    Process.KillProcess(Process.MyPid());
                };

            RequestRequiredPermissions(hasStoragePermissions);

            SetContentView(_game.Services.GetService<View>());
            _game.Run();
        }

        protected override void OnPause()
        {
            base.OnPause();

            _game?.Suspend();
        }

        protected override void OnResume()
        {
            base.OnResume();

            // When we resume (which also seems to happen on startup), hide the system UI to go to full screen mode.
            HideSystemUI();
        }

        protected override void OnPostResume()
        {
            base.OnPostResume();

            _game?.Resume();
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            switch (requestCode)
            {
                case REQUESTID_PERMISSION_STORAGE:
                    bool accessGranted = true;

                    // Check if any of the requested permissions came back as denied.
                    for (int i = 0; i < permissions.Length; i++)
                    {
                        if (grantResults[i] == Permission.Denied)
                        {
                            accessGranted = false;
                            break;
                        }
                    }

                    if (accessGranted)
                    {
                        // If we were granted access, update components that need this permission.

                        // CodeLogicSettings
                        ICodeLogicSettings codeLogicSettings = _game.Components.GetComponent<ICodeLogicSettings>();
                        codeLogicSettings.IsTransient = false;
                        codeLogicSettings.LoadCache();

                        // GameStatsData
                        IGameStatsData gameStatsData = _game.Components.GetComponent<IGameStatsData>();
                        gameStatsData.IsTransient = false;
                    }
                    break;
            }
        }
        #endregion

        #region Permission Check Methods
        private const int REQUESTID_PERMISSION_STORAGE = 1;
        private bool CheckStoragePermissions()
        {
            bool hasStoragePermissions = true
                && ContextCompat.CheckSelfPermission(this, Manifest.Permission.WriteExternalStorage) == Permission.Granted
                && ContextCompat.CheckSelfPermission(this, Manifest.Permission.ReadExternalStorage) == Permission.Granted
                && true;

            return hasStoragePermissions;
        }

        private void RequestRequiredPermissions(bool hasStoragePermissions)
        {
            if (!hasStoragePermissions)
                ActivityCompat.RequestPermissions(this, new string[] { Manifest.Permission.WriteExternalStorage, Manifest.Permission.ReadExternalStorage }, REQUESTID_PERMISSION_STORAGE);
        }
        #endregion

        #region Component Creation Methods
        private ICodeLogicSettings CreateComponent_CodeLogicSettings(IComponentContainer container,  bool hasStoragePermissions)
        {
            IConfigStore jsonConfigStore = new JsonConfigStore(container.GetComponent<ISimpleFileSystem>(), container.GetComponent<IJsonConvert>(), Path.Combine(_appStorageDir, "settings.json"))
            {
                IsTransient = !hasStoragePermissions   // If no storage permissions, we cannot persist the config store. We'll get them later and can update this property.
            };

            ICodeLogicSettings component = new CodeLogicSettings(jsonConfigStore);

            return component;
        }

        private IGameStatsData CreateComponent_GameStatsData(IComponentContainer container, bool hasStoragePermissions)
        {
            IConfigStore jsonConfigStore = new JsonConfigStore(container.GetComponent<ISimpleFileSystem>(), container.GetComponent<IJsonConvert>(), Path.Combine(_appStorageDir, "stats.json"))
            {
                IsTransient = !hasStoragePermissions   // If no storage permissions, we cannot persist the config store. We'll get them later and can update this property.
            };

            IGameStatsData component = new GameStatsData(
                jsonConfigStore,
                new GameStatsModel(),
                container.GetComponent<IBuildInfoStore>(),
                container.GetComponent<IValueConverterManager>()
            );

            return component;
        }
        #endregion

        #region Private Methods
        private void Handle_ProviderAdClosed(object sender, System.EventArgs e)
        {
            if (CheckGameStatePostAd() == false)
            {
                ShowApplicationInvalidStateScreen();
            }
        }

        private void HideSystemUI()
        {
            // Apparently for Android OS Kitkat and higher, you can set a full screen mode. Why this isn't on by default, or some kind
            // of simple switch, is beyond me.
            // Got this from the following forum post: http://community.monogame.net/t/blocking-the-menu-bar-from-appearing/1021/2
            if (Build.VERSION.SdkInt >= BuildVersionCodes.Kitkat)
            {
                View decorView = Window.DecorView;
                var uiOptions = (int)decorView.SystemUiVisibility;
                var newUiOptions = (int)uiOptions;

                newUiOptions |= (int)SystemUiFlags.LowProfile;
                newUiOptions |= (int)SystemUiFlags.Fullscreen;
                newUiOptions |= (int)SystemUiFlags.HideNavigation;
                newUiOptions |= (int)SystemUiFlags.ImmersiveSticky;

                decorView.SystemUiVisibility = (StatusBarVisibility)newUiOptions;

                this.Immersive = true;
            }
        }

        private bool CheckGameStatePostAd()
        {
            bool appOk = false;

            try
            {
                View gameView = _game.Services.GetService<View>();
                if (gameView.IsShown == true)
                    appOk = true;
            }
            catch { }

            return appOk;
        }

        private void ShowApplicationInvalidStateScreen()
        {
            Intent showApplicationUnrecoverableStateIntent = new Intent(this, typeof(ApplicationUnrecoverableStateActivity));
            this.StartActivity(showApplicationUnrecoverableStateIntent);
        }

        private void Handle_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
                string baseFile = "CrashLog";

                string fileName = string.Format("{0}_{1}.txt", baseFile, DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss-fff"));
                string dir = Path.Combine(_appStorageDir, "Log");
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);
                string path = Path.Combine(dir, fileName);

                File.WriteAllText(path, e.ExceptionObject.ToString());
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Unable to log dump file.");
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                System.Diagnostics.Debug.WriteLine("Unhandled exception:");
                System.Diagnostics.Debug.WriteLine(e.ToString());
            }
        }
        #endregion
    }
}