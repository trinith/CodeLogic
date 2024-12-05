using ArbitraryPixel.CodeLogic.Common;
using ArbitraryPixel.CodeLogic.Common.Config;
using ArbitraryPixel.CodeLogic.Common.Model;
using ArbitraryPixel.Common;
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
using System.Diagnostics;
using System.IO;

namespace CodeLogic_Windows
{
#if WINDOWS || LINUX
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        private static string _appStorageDir;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            _appStorageDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ArbitraryPixel", "CodeLogic");
            AppDomain.CurrentDomain.UnhandledException += Handle_UnhandledException;

            IScreenManager screenManager = new ScreenManager();
            screenManager.World = new Point(1280, 720);
            screenManager.Screen = new Point(1280, 720);
            //screenManager.Screen = new Point(1920, 1080);
            screenManager.IsFullScreen = false;

            IComponentContainer container = new SimpleComponentContainer();
            container.RegisterComponent<ITargetPlatform>(new WindowsPlatform());
            MainGame.RegisterBasicDependencies(container);

            container.RegisterComponent<IScreenManager>(screenManager);
            container.RegisterComponent<ISurfaceInputManager>(new MouseSurfaceInputManager(new MonoGameMouseStateManager()));
            container.RegisterComponent<ICodeLogicSettings>(CreateComponent_CodeLogicSettings(container));
            container.RegisterComponent<IGameStatsData>(CreateComponent_GameStatsData(container));

            var game = new MainGame(container);
            game.GameFinished += (sender, e) => game.Exit();

            game.IsMouseVisible = true;
            game.Run();
        }

        #region Component Creation Methods
        private static ICodeLogicSettings CreateComponent_CodeLogicSettings(IComponentContainer container)
        {
            return new CodeLogicSettings(
                new JsonConfigStore(
                    container.GetComponent<ISimpleFileSystem>(),
                    container.GetComponent<IJsonConvert>(),
                    Path.Combine(_appStorageDir, "settings.json")
                )
            );
        }

        private static IGameStatsData CreateComponent_GameStatsData(IComponentContainer container)
        {
            IGameStatsData component = new GameStatsData(
                new JsonConfigStore(
                    container.GetComponent<ISimpleFileSystem>(),
                    container.GetComponent<IJsonConvert>(),
                    Path.Combine(_appStorageDir, "stats.json")
                ),
                new GameStatsModel(),
                container.GetComponent<IBuildInfoStore>(),
                container.GetComponent<IValueConverterManager>()
            );

            return component;
        }
        #endregion

        private static void Handle_UnhandledException(object sender, UnhandledExceptionEventArgs e)
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
                Debug.WriteLine("Unable to log dump file.");
                Debug.WriteLine(ex.ToString());
                Debug.WriteLine("Unhandled exception:");
                Debug.WriteLine(e.ToString());
            }
        }
    }
#endif
}
