using ArbitraryPixel.CodeLogic.Common;
using ArbitraryPixel.CodeLogic.Common.Config;
using ArbitraryPixel.CodeLogic.Common.Model;
using ArbitraryPixel.CodeLogic.Common.Scenes;
using ArbitraryPixel.CodeLogic.Common.UI;
using ArbitraryPixel.Common.Audio;
using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Platform2D.Assets;
using ArbitraryPixel.Platform2D.Engine;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace ArbitraryPixel.CodeLogic.Common_Tests.Scenes
{
    [TestClass]
    public class MainMenuScene_AssetLoad_Tests
    {
        private MainMenuScene _sut;
        private IEngine _mockEngine;
        private IMainMenuModel _mockModel;
        private IMenuFactory _mockMenuFactory;
        private IGameStatsData _mockGameStatsData;

        private GameObjectFactory _mockGameObjectFactory;

        [TestInitialize]
        public void Initialize()
        {
            _mockEngine = Substitute.For<IEngine>();
            _mockModel = Substitute.For<IMainMenuModel>();
            _mockMenuFactory = Substitute.For<IMenuFactory>();
            _mockGameStatsData = Substitute.For<IGameStatsData>();

            _mockGameObjectFactory = Substitute.For<GameObjectFactory>();
            GameObjectFactory.SetInstance(_mockGameObjectFactory);

            IAssetLoader mockLoader = Substitute.For<IAssetLoader>();
            AssetLoadMethod loadMethod = null;
            _mockEngine.AssetBank.CreateLoader().Returns(mockLoader);
            mockLoader.When(x => x.RegisterLoadMethod(Arg.Any<AssetLoadMethod>())).Do(x => loadMethod = x[0] as AssetLoadMethod);
            mockLoader.When(x => x.LoadBank()).Do(x => loadMethod(_mockEngine.AssetBank));

            _sut = new MainMenuScene(_mockEngine, _mockModel, _mockMenuFactory, _mockGameStatsData);
        }

        #region Textures - General
        [TestMethod]
        public void InitializeShouldLoadTexture_Plane()
        {
            string target = "Plane";
            string assetPath = "Textures\\" + target;
            ITexture2D mockTexture = Substitute.For<ITexture2D>();
            _mockEngine.GrfxFactory.Texture2DFactory.Create(_mockEngine.Content, assetPath).Returns(mockTexture);

            _sut.Initialize();

            Received.InOrder(
                () =>
                {
                    _mockEngine.GrfxFactory.Texture2DFactory.Create(_mockEngine.Content, assetPath);
                    _mockEngine.AssetBank.Put<ITexture2D>(target, mockTexture);
                }
            );
        }

        [TestMethod]
        public void InitializeShouldLoadTexture_MainMenuBackground()
        {
            string target = "MainMenuBackground";
            string assetPath = "Textures\\" + target;
            ITexture2D mockTexture = Substitute.For<ITexture2D>();
            _mockEngine.GrfxFactory.Texture2DFactory.Create(_mockEngine.Content, assetPath).Returns(mockTexture);

            _sut.Initialize();

            Received.InOrder(
                () =>
                {
                    _mockEngine.GrfxFactory.Texture2DFactory.Create(_mockEngine.Content, assetPath);
                    _mockEngine.AssetBank.Put<ITexture2D>(target, mockTexture);
                }
            );
        }

        [TestMethod]
        public void InitializeShouldLoadTexture_Cloud0()
        {
            string target = "cloud0";
            string assetPath = "Textures\\" + target;
            ITexture2D mockTexture = Substitute.For<ITexture2D>();
            _mockEngine.GrfxFactory.Texture2DFactory.Create(_mockEngine.Content, assetPath).Returns(mockTexture);

            _sut.Initialize();

            Received.InOrder(
                () =>
                {
                    _mockEngine.GrfxFactory.Texture2DFactory.Create(_mockEngine.Content, assetPath);
                    _mockEngine.AssetBank.Put<ITexture2D>(target, mockTexture);
                }
            );
        }

        [TestMethod]
        public void InitializeShouldLoadTexture_Cloud1()
        {
            string target = "cloud1";
            string assetPath = "Textures\\" + target;
            ITexture2D mockTexture = Substitute.For<ITexture2D>();
            _mockEngine.GrfxFactory.Texture2DFactory.Create(_mockEngine.Content, assetPath).Returns(mockTexture);

            _sut.Initialize();

            Received.InOrder(
                () =>
                {
                    _mockEngine.GrfxFactory.Texture2DFactory.Create(_mockEngine.Content, assetPath);
                    _mockEngine.AssetBank.Put<ITexture2D>(target, mockTexture);
                }
            );
        }
        #endregion

        #region Textures - Briefing
        [TestMethod]
        public void InitializeShouldLoadTexture_CX4Logo()
        {
            string target = "CX4Logo";
            string assetPath = "Textures\\Briefing\\" + target;
            ITexture2D mockTexture = Substitute.For<ITexture2D>();
            _mockEngine.GrfxFactory.Texture2DFactory.Create(_mockEngine.Content, assetPath).Returns(mockTexture);

            _sut.Initialize();

            Received.InOrder(
                () =>
                {
                    _mockEngine.GrfxFactory.Texture2DFactory.Create(_mockEngine.Content, assetPath);
                    _mockEngine.AssetBank.Put<ITexture2D>(target, mockTexture);
                }
            );
        }

        [TestMethod]
        public void InitializeShouldLoadTexture_BP2_BootupImage()
        {
            string target = "BP2_BootupImage";
            string assetPath = "Textures\\Briefing\\" + target;
            ITexture2D mockTexture = Substitute.For<ITexture2D>();
            _mockEngine.GrfxFactory.Texture2DFactory.Create(_mockEngine.Content, assetPath).Returns(mockTexture);

            _sut.Initialize();

            Received.InOrder(
                () =>
                {
                    _mockEngine.GrfxFactory.Texture2DFactory.Create(_mockEngine.Content, assetPath);
                    _mockEngine.AssetBank.Put<ITexture2D>(target, mockTexture);
                }
            );
        }

        [TestMethod]
        public void InitializeShouldLoadTexture_BP3_Overview()
        {
            string target = "BP3_Overview";
            string assetPath = "Textures\\Briefing\\" + target;
            ITexture2D mockTexture = Substitute.For<ITexture2D>();
            _mockEngine.GrfxFactory.Texture2DFactory.Create(_mockEngine.Content, assetPath).Returns(mockTexture);

            _sut.Initialize();

            Received.InOrder(
                () =>
                {
                    _mockEngine.GrfxFactory.Texture2DFactory.Create(_mockEngine.Content, assetPath);
                    _mockEngine.AssetBank.Put<ITexture2D>(target, mockTexture);
                }
            );
        }

        [TestMethod]
        public void InitializeShouldLoadTexture_BP4_CurrentAttempt()
        {
            string target = "BP4_CurrentAttempt";
            string assetPath = "Textures\\Briefing\\" + target;
            ITexture2D mockTexture = Substitute.For<ITexture2D>();
            _mockEngine.GrfxFactory.Texture2DFactory.Create(_mockEngine.Content, assetPath).Returns(mockTexture);

            _sut.Initialize();

            Received.InOrder(
                () =>
                {
                    _mockEngine.GrfxFactory.Texture2DFactory.Create(_mockEngine.Content, assetPath);
                    _mockEngine.AssetBank.Put<ITexture2D>(target, mockTexture);
                }
            );
        }

        [TestMethod]
        public void InitializeShouldLoadTexture_CodeInput()
        {
            string target = "CodeInput";
            string assetPath = "Textures\\Briefing\\" + target;
            ITexture2D mockTexture = Substitute.For<ITexture2D>();
            _mockEngine.GrfxFactory.Texture2DFactory.Create(_mockEngine.Content, assetPath).Returns(mockTexture);

            _sut.Initialize();

            Received.InOrder(
                () =>
                {
                    _mockEngine.GrfxFactory.Texture2DFactory.Create(_mockEngine.Content, assetPath);
                    _mockEngine.AssetBank.Put<ITexture2D>(target, mockTexture);
                }
            );
        }

        [TestMethod]
        public void InitializeShouldLoadTexture_CodeInput_Example()
        {
            string target = "CodeInput_Example";
            string assetPath = "Textures\\Briefing\\" + target;
            ITexture2D mockTexture = Substitute.For<ITexture2D>();
            _mockEngine.GrfxFactory.Texture2DFactory.Create(_mockEngine.Content, assetPath).Returns(mockTexture);

            _sut.Initialize();

            Received.InOrder(
                () =>
                {
                    _mockEngine.GrfxFactory.Texture2DFactory.Create(_mockEngine.Content, assetPath);
                    _mockEngine.AssetBank.Put<ITexture2D>(target, mockTexture);
                }
            );
        }

        [TestMethod]
        public void InitializeShouldLoadTexture_HistoryFull()
        {
            string target = "HistoryFull";
            string assetPath = "Textures\\Briefing\\" + target;
            ITexture2D mockTexture = Substitute.For<ITexture2D>();
            _mockEngine.GrfxFactory.Texture2DFactory.Create(_mockEngine.Content, assetPath).Returns(mockTexture);

            _sut.Initialize();

            Received.InOrder(
                () =>
                {
                    _mockEngine.GrfxFactory.Texture2DFactory.Create(_mockEngine.Content, assetPath);
                    _mockEngine.AssetBank.Put<ITexture2D>(target, mockTexture);
                }
            );
        }

        [TestMethod]
        public void InitializeShouldLoadTexture_HistoryZoom()
        {
            string target = "HistoryZoom";
            string assetPath = "Textures\\Briefing\\" + target;
            ITexture2D mockTexture = Substitute.For<ITexture2D>();
            _mockEngine.GrfxFactory.Texture2DFactory.Create(_mockEngine.Content, assetPath).Returns(mockTexture);

            _sut.Initialize();

            Received.InOrder(
                () =>
                {
                    _mockEngine.GrfxFactory.Texture2DFactory.Create(_mockEngine.Content, assetPath);
                    _mockEngine.AssetBank.Put<ITexture2D>(target, mockTexture);
                }
            );
        }

        [TestMethod]
        public void InitializeShouldLoadTexture_SubmitButton()
        {
            string target = "SubmitButton";
            string assetPath = "Textures\\Briefing\\" + target;
            ITexture2D mockTexture = Substitute.For<ITexture2D>();
            _mockEngine.GrfxFactory.Texture2DFactory.Create(_mockEngine.Content, assetPath).Returns(mockTexture);

            _sut.Initialize();

            Received.InOrder(
                () =>
                {
                    _mockEngine.GrfxFactory.Texture2DFactory.Create(_mockEngine.Content, assetPath);
                    _mockEngine.AssetBank.Put<ITexture2D>(target, mockTexture);
                }
            );
        }

        [TestMethod]
        public void InitializeShouldLoadTexture_MenuButton()
        {
            string target = "MenuButton";
            string assetPath = "Textures\\Briefing\\" + target;
            ITexture2D mockTexture = Substitute.For<ITexture2D>();
            _mockEngine.GrfxFactory.Texture2DFactory.Create(_mockEngine.Content, assetPath).Returns(mockTexture);

            _sut.Initialize();

            Received.InOrder(
                () =>
                {
                    _mockEngine.GrfxFactory.Texture2DFactory.Create(_mockEngine.Content, assetPath);
                    _mockEngine.AssetBank.Put<ITexture2D>(target, mockTexture);
                }
            );
        }

        [TestMethod]
        public void InitializeShouldLoadTexture_ReturnButton()
        {
            string target = "ReturnButton";
            string assetPath = "Textures\\Briefing\\" + target;
            ITexture2D mockTexture = Substitute.For<ITexture2D>();
            _mockEngine.GrfxFactory.Texture2DFactory.Create(_mockEngine.Content, assetPath).Returns(mockTexture);

            _sut.Initialize();

            Received.InOrder(
                () =>
                {
                    _mockEngine.GrfxFactory.Texture2DFactory.Create(_mockEngine.Content, assetPath);
                    _mockEngine.AssetBank.Put<ITexture2D>(target, mockTexture);
                }
            );
        }

        [TestMethod]
        public void InitializeShouldLoadTexture_MenuScreen()
        {
            string target = "MenuScreen";
            string assetPath = "Textures\\Briefing\\" + target;
            ITexture2D mockTexture = Substitute.For<ITexture2D>();
            _mockEngine.GrfxFactory.Texture2DFactory.Create(_mockEngine.Content, assetPath).Returns(mockTexture);

            _sut.Initialize();

            Received.InOrder(
                () =>
                {
                    _mockEngine.GrfxFactory.Texture2DFactory.Create(_mockEngine.Content, assetPath);
                    _mockEngine.AssetBank.Put<ITexture2D>(target, mockTexture);
                }
            );
        }

        [TestMethod]
        public void InitializeShouldLoadTexture_Example_Code()
        {
            string target = "Example_Code";
            string assetPath = "Textures\\Briefing\\" + target;
            ITexture2D mockTexture = Substitute.For<ITexture2D>();
            _mockEngine.GrfxFactory.Texture2DFactory.Create(_mockEngine.Content, assetPath).Returns(mockTexture);

            _sut.Initialize();

            Received.InOrder(
                () =>
                {
                    _mockEngine.GrfxFactory.Texture2DFactory.Create(_mockEngine.Content, assetPath);
                    _mockEngine.AssetBank.Put<ITexture2D>(target, mockTexture);
                }
            );
        }

        [TestMethod]
        public void InitializeShouldLoadTexture_Example_Results1()
        {
            string target = "Example_Results1";
            string assetPath = "Textures\\Briefing\\" + target;
            ITexture2D mockTexture = Substitute.For<ITexture2D>();
            _mockEngine.GrfxFactory.Texture2DFactory.Create(_mockEngine.Content, assetPath).Returns(mockTexture);

            _sut.Initialize();

            Received.InOrder(
                () =>
                {
                    _mockEngine.GrfxFactory.Texture2DFactory.Create(_mockEngine.Content, assetPath);
                    _mockEngine.AssetBank.Put<ITexture2D>(target, mockTexture);
                }
            );
        }

        [TestMethod]
        public void InitializeShouldLoadTexture_Example_Results2()
        {
            string target = "Example_Results2";
            string assetPath = "Textures\\Briefing\\" + target;
            ITexture2D mockTexture = Substitute.For<ITexture2D>();
            _mockEngine.GrfxFactory.Texture2DFactory.Create(_mockEngine.Content, assetPath).Returns(mockTexture);

            _sut.Initialize();

            Received.InOrder(
                () =>
                {
                    _mockEngine.GrfxFactory.Texture2DFactory.Create(_mockEngine.Content, assetPath);
                    _mockEngine.AssetBank.Put<ITexture2D>(target, mockTexture);
                }
            );
        }

        [TestMethod]
        public void InitializeShouldLoadTexture_Example_Results3()
        {
            string target = "Example_Results3";
            string assetPath = "Textures\\Briefing\\" + target;
            ITexture2D mockTexture = Substitute.For<ITexture2D>();
            _mockEngine.GrfxFactory.Texture2DFactory.Create(_mockEngine.Content, assetPath).Returns(mockTexture);

            _sut.Initialize();

            Received.InOrder(
                () =>
                {
                    _mockEngine.GrfxFactory.Texture2DFactory.Create(_mockEngine.Content, assetPath);
                    _mockEngine.AssetBank.Put<ITexture2D>(target, mockTexture);
                }
            );
        }

        [TestMethod]
        public void InitializeShouldLoadTexture_Example_Results4()
        {
            string target = "Example_Results4";
            string assetPath = "Textures\\Briefing\\" + target;
            ITexture2D mockTexture = Substitute.For<ITexture2D>();
            _mockEngine.GrfxFactory.Texture2DFactory.Create(_mockEngine.Content, assetPath).Returns(mockTexture);

            _sut.Initialize();

            Received.InOrder(
                () =>
                {
                    _mockEngine.GrfxFactory.Texture2DFactory.Create(_mockEngine.Content, assetPath);
                    _mockEngine.AssetBank.Put<ITexture2D>(target, mockTexture);
                }
            );
        }

        [TestMethod]
        public void InitializeShouldLoadTexture_Results_Equal()
        {
            string target = "Results_Equal";
            string assetPath = "Textures\\Briefing\\" + target;
            ITexture2D mockTexture = Substitute.For<ITexture2D>();
            _mockEngine.GrfxFactory.Texture2DFactory.Create(_mockEngine.Content, assetPath).Returns(mockTexture);

            _sut.Initialize();

            Received.InOrder(
                () =>
                {
                    _mockEngine.GrfxFactory.Texture2DFactory.Create(_mockEngine.Content, assetPath);
                    _mockEngine.AssetBank.Put<ITexture2D>(target, mockTexture);
                }
            );
        }

        [TestMethod]
        public void InitializeShouldLoadTexture_Results_Partial()
        {
            string target = "Results_Partial";
            string assetPath = "Textures\\Briefing\\" + target;
            ITexture2D mockTexture = Substitute.For<ITexture2D>();
            _mockEngine.GrfxFactory.Texture2DFactory.Create(_mockEngine.Content, assetPath).Returns(mockTexture);

            _sut.Initialize();

            Received.InOrder(
                () =>
                {
                    _mockEngine.GrfxFactory.Texture2DFactory.Create(_mockEngine.Content, assetPath);
                    _mockEngine.AssetBank.Put<ITexture2D>(target, mockTexture);
                }
            );
        }

        [TestMethod]
        public void InitializeShouldLoadTexture_Results_NotEqual()
        {
            string target = "Results_NotEqual";
            string assetPath = "Textures\\Briefing\\" + target;
            ITexture2D mockTexture = Substitute.For<ITexture2D>();
            _mockEngine.GrfxFactory.Texture2DFactory.Create(_mockEngine.Content, assetPath).Returns(mockTexture);

            _sut.Initialize();

            Received.InOrder(
                () =>
                {
                    _mockEngine.GrfxFactory.Texture2DFactory.Create(_mockEngine.Content, assetPath);
                    _mockEngine.AssetBank.Put<ITexture2D>(target, mockTexture);
                }
            );
        }
        #endregion

        #region Fonts
        [TestMethod]
        public void InitializeShouldLoadFont_MainMenuFont()
        {
            string target = "MainMenuFont";
            string assetPath = "Fonts\\" + target;
            ISpriteFont mockFont = Substitute.For<ISpriteFont>();
            _mockEngine.GrfxFactory.SpriteFontFactory.Create(_mockEngine.Content, assetPath).Returns(mockFont);

            _sut.Initialize();

            Received.InOrder(
                () =>
                {
                    _mockEngine.GrfxFactory.SpriteFontFactory.Create(_mockEngine.Content, assetPath);
                    _mockEngine.AssetBank.Put<ISpriteFont>(target, mockFont);
                }
            );
        }

        [TestMethod]
        public void InitializeShouldLoadFont_MainMenuContentFont()
        {
            string target = "MainMenuContentFont";
            string assetPath = "Fonts\\" + target;
            ISpriteFont mockFont = Substitute.For<ISpriteFont>();
            _mockEngine.GrfxFactory.SpriteFontFactory.Create(_mockEngine.Content, assetPath).Returns(mockFont);

            _sut.Initialize();

            Received.InOrder(
                () =>
                {
                    _mockEngine.GrfxFactory.SpriteFontFactory.Create(_mockEngine.Content, assetPath);
                    _mockEngine.AssetBank.Put<ISpriteFont>(target, mockFont);
                }
            );
        }

        [TestMethod]
        public void InitializeShouldLoadFont_MainButtonFont()
        {
            string target = "MainButtonFont";
            string assetPath = "Fonts\\" + target;
            ISpriteFont mockFont = Substitute.For<ISpriteFont>();
            _mockEngine.GrfxFactory.SpriteFontFactory.Create(_mockEngine.Content, assetPath).Returns(mockFont);

            _sut.Initialize();

            Received.InOrder(
                () =>
                {
                    _mockEngine.GrfxFactory.SpriteFontFactory.Create(_mockEngine.Content, assetPath);
                    _mockEngine.AssetBank.Put<ISpriteFont>(target, mockFont);
                }
            );
        }

        [TestMethod]
        public void InitializeShouldLoadFont_CreditsTitleFont()
        {
            string target = "CreditsTitleFont";
            string assetPath = "Fonts\\" + target;
            ISpriteFont mockFont = Substitute.For<ISpriteFont>();
            _mockEngine.GrfxFactory.SpriteFontFactory.Create(_mockEngine.Content, assetPath).Returns(mockFont);

            _sut.Initialize();

            Received.InOrder(
                () =>
                {
                    _mockEngine.GrfxFactory.SpriteFontFactory.Create(_mockEngine.Content, assetPath);
                    _mockEngine.AssetBank.Put<ISpriteFont>(target, mockFont);
                }
            );
        }

        [TestMethod]
        public void InitializeShouldLoadFont_CreditsCreditFont()
        {
            string target = "CreditsCreditFont";
            string assetPath = "Fonts\\" + target;
            ISpriteFont mockFont = Substitute.For<ISpriteFont>();
            _mockEngine.GrfxFactory.SpriteFontFactory.Create(_mockEngine.Content, assetPath).Returns(mockFont);

            _sut.Initialize();

            Received.InOrder(
                () =>
                {
                    _mockEngine.GrfxFactory.SpriteFontFactory.Create(_mockEngine.Content, assetPath);
                    _mockEngine.AssetBank.Put<ISpriteFont>(target, mockFont);
                }
            );
        }

        [TestMethod]
        public void InitializeShouldLoadFont_ConsoleNormalFont()
        {
            string target = "ConsoleNormalFont";
            string assetPath = "Fonts\\" + target;
            ISpriteFont mockFont = Substitute.For<ISpriteFont>();
            _mockEngine.GrfxFactory.SpriteFontFactory.Create(_mockEngine.Content, assetPath).Returns(mockFont);

            _sut.Initialize();

            Received.InOrder(
                () =>
                {
                    _mockEngine.GrfxFactory.SpriteFontFactory.Create(_mockEngine.Content, assetPath);
                    _mockEngine.AssetBank.Put<ISpriteFont>(target, mockFont);
                }
            );
        }

        [TestMethod]
        public void InitializeShouldLoadFont_ConsoleHeadingFont()
        {
            string target = "ConsoleHeadingFont";
            string assetPath = "Fonts\\" + target;
            ISpriteFont mockFont = Substitute.For<ISpriteFont>();
            _mockEngine.GrfxFactory.SpriteFontFactory.Create(_mockEngine.Content, assetPath).Returns(mockFont);

            _sut.Initialize();

            Received.InOrder(
                () =>
                {
                    _mockEngine.GrfxFactory.SpriteFontFactory.Create(_mockEngine.Content, assetPath);
                    _mockEngine.AssetBank.Put<ISpriteFont>(target, mockFont);
                }
            );
        }

        [TestMethod]
        public void InitializeShouldLoadFont_VersionFont()
        {
            string target = "VersionFont";
            string assetPath = "Fonts\\" + target;
            ISpriteFont mockFont = Substitute.For<ISpriteFont>();
            _mockEngine.GrfxFactory.SpriteFontFactory.Create(_mockEngine.Content, assetPath).Returns(mockFont);

            _sut.Initialize();

            Received.InOrder(
                () =>
                {
                    _mockEngine.GrfxFactory.SpriteFontFactory.Create(_mockEngine.Content, assetPath);
                    _mockEngine.AssetBank.Put<ISpriteFont>(target, mockFont);
                }
            );
        }

        [TestMethod]
        public void InitializeShouldLoadFont_StatsFont()
        {
            string target = "StatsFont";
            string assetPath = "Fonts\\" + target;
            ISpriteFont mockFont = Substitute.For<ISpriteFont>();
            _mockEngine.GrfxFactory.SpriteFontFactory.Create(_mockEngine.Content, assetPath).Returns(mockFont);

            _sut.Initialize();

            Received.InOrder(
                () =>
                {
                    _mockEngine.GrfxFactory.SpriteFontFactory.Create(_mockEngine.Content, assetPath);
                    _mockEngine.AssetBank.Put<ISpriteFont>(target, mockFont);
                }
            );
        }

        [TestMethod]
        public void InitializeShouldLoadFont_BriefingNormalFont()
        {
            string target = "BriefingNormalFont";
            string assetPath = "Fonts\\Briefing\\" + target;
            ISpriteFont mockFont = Substitute.For<ISpriteFont>();
            _mockEngine.GrfxFactory.SpriteFontFactory.Create(_mockEngine.Content, assetPath).Returns(mockFont);

            _sut.Initialize();

            Received.InOrder(
                () =>
                {
                    _mockEngine.GrfxFactory.SpriteFontFactory.Create(_mockEngine.Content, assetPath);
                    _mockEngine.AssetBank.Put<ISpriteFont>(target, mockFont);
                }
            );
        }

        [TestMethod]
        public void InitializeShouldLoadFont_BriefingTitleFont()
        {
            string target = "BriefingTitleFont";
            string assetPath = "Fonts\\Briefing\\" + target;
            ISpriteFont mockFont = Substitute.For<ISpriteFont>();
            _mockEngine.GrfxFactory.SpriteFontFactory.Create(_mockEngine.Content, assetPath).Returns(mockFont);

            _sut.Initialize();

            Received.InOrder(
                () =>
                {
                    _mockEngine.GrfxFactory.SpriteFontFactory.Create(_mockEngine.Content, assetPath);
                    _mockEngine.AssetBank.Put<ISpriteFont>(target, mockFont);
                }
            );
        }
        #endregion

        #region Music
        [TestMethod]
        public void InitializeShouldLoadMusic_MainMenu()
        {
            string target = "MainMenu";
            string assetPath = "Music\\" + target;
            ISong mockSong = Substitute.For<ISong>();
            _mockEngine.AudioManager.MusicController.CreateSong(_mockEngine.Content, assetPath).Returns(mockSong);

            _sut.Initialize();

            Received.InOrder(
                () =>
                {
                    _mockEngine.AudioManager.MusicController.CreateSong(_mockEngine.Content, assetPath);
                    _mockEngine.AssetBank.Put<ISong>(target, mockSong);
                }
            );
        }
        #endregion

        #region Sounds
        [TestMethod]
        public void InitializeShouldLoadSound_WindowOpen()
        {
            string target = "WindowOpen";
            string assetPath = "Sounds\\" + target;
            ISoundResource mockSound = Substitute.For<ISoundResource>();
            _mockEngine.AudioManager.SoundController.CreateSoundResource(_mockEngine.Content, assetPath).Returns(mockSound);

            _sut.Initialize();

            Received.InOrder(
                () =>
                {
                    _mockEngine.AudioManager.SoundController.CreateSoundResource(_mockEngine.Content, assetPath);
                    _mockEngine.AssetBank.Put<ISoundResource>(target, mockSound);
                }
            );
        }

        [TestMethod]
        public void InitializeShouldLoadSound_WindowClose()
        {
            string target = "WindowClose";
            string assetPath = "Sounds\\" + target;
            ISoundResource mockSound = Substitute.For<ISoundResource>();
            _mockEngine.AudioManager.SoundController.CreateSoundResource(_mockEngine.Content, assetPath).Returns(mockSound);

            _sut.Initialize();

            Received.InOrder(
                () =>
                {
                    _mockEngine.AudioManager.SoundController.CreateSoundResource(_mockEngine.Content, assetPath);
                    _mockEngine.AssetBank.Put<ISoundResource>(target, mockSound);
                }
            );
        }

        [TestMethod]
        public void InitializeShouldLoadSound_Thunder1()
        {
            string target = "Thunder1";
            string assetPath = "Sounds\\" + target;
            ISoundResource mockSound = Substitute.For<ISoundResource>();
            _mockEngine.AudioManager.SoundController.CreateSoundResource(_mockEngine.Content, assetPath).Returns(mockSound);

            _sut.Initialize();

            Received.InOrder(
                () =>
                {
                    _mockEngine.AudioManager.SoundController.CreateSoundResource(_mockEngine.Content, assetPath);
                    _mockEngine.AssetBank.Put<ISoundResource>(target, mockSound);
                }
            );
        }

        [TestMethod]
        public void InitializeShouldLoadSound_AirplaneNormal()
        {
            string target = "AirplaneNormal";
            string assetPath = "Sounds\\" + target;
            ISoundResource mockSound = Substitute.For<ISoundResource>();
            _mockEngine.AudioManager.SoundController.CreateSoundResource(_mockEngine.Content, assetPath).Returns(mockSound);

            _sut.Initialize();

            Received.InOrder(
                () =>
                {
                    _mockEngine.AudioManager.SoundController.CreateSoundResource(_mockEngine.Content, assetPath);
                    _mockEngine.AssetBank.Put<ISoundResource>(target, mockSound);
                }
            );
        }

        [TestMethod]
        public void InitializeShouldLoadSound_AirplaneReducePowerFade()
        {
            string target = "AirplaneReducePowerFade";
            string assetPath = "Sounds\\" + target;
            ISoundResource mockSound = Substitute.For<ISoundResource>();
            _mockEngine.AudioManager.SoundController.CreateSoundResource(_mockEngine.Content, assetPath).Returns(mockSound);

            _sut.Initialize();

            Received.InOrder(
                () =>
                {
                    _mockEngine.AudioManager.SoundController.CreateSoundResource(_mockEngine.Content, assetPath);
                    _mockEngine.AssetBank.Put<ISoundResource>(target, mockSound);
                }
            );
        }
        #endregion

        #region Shaders
        [TestMethod]
        public void InitializeShouldLoadShader_LightningFlash()
        {
            string target = "LightningFlash";
            string assetPath = "Shaders\\" + target;
            IEffect mockEffect = Substitute.For<IEffect>();
            _mockEngine.GrfxFactory.EffectFactory.Create(_mockEngine.Content, assetPath).Returns(mockEffect);

            _sut.Initialize();

            Received.InOrder(
                () =>
                {
                    _mockEngine.GrfxFactory.EffectFactory.Create(_mockEngine.Content, assetPath);
                    _mockEngine.AssetBank.Put<IEffect>(target, mockEffect);
                }
            );
        }
        #endregion
    }
}
