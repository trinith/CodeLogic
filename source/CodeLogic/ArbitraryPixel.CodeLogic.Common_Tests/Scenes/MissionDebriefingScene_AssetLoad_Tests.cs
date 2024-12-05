using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArbitraryPixel.CodeLogic.Common.Scenes;
using ArbitraryPixel.CodeLogic.Common.Model;
using ArbitraryPixel.Platform2D.Engine;
using NSubstitute;
using ArbitraryPixel.Platform2D.Assets;
using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.CodeLogic.Common;
using ArbitraryPixel.Common.Audio;
using ArbitraryPixel.CodeLogic.Common.Controllers;

namespace ArbitraryPixel.CodeLogic.Common_Tests.Scenes
{
    [TestClass]
    public class MissionDebriefingScene_AssetLoad_Tests
    {
        private MissionDebriefingScene _sut;
        private IEngine _mockEngine;
        private IDeviceModel _mockDeviceModel;
        private IGameStatsController _mockGameStatsController;

        [TestInitialize]
        public void Initialize()
        {
            _mockEngine = Substitute.For<IEngine>();
            _mockDeviceModel = Substitute.For<IDeviceModel>();
            _mockGameStatsController = Substitute.For<IGameStatsController>();

            IAssetLoader mockLoader = Substitute.For<IAssetLoader>();
            AssetLoadMethod loadMethod = null;
            _mockEngine.AssetBank.CreateLoader().Returns(mockLoader);
            mockLoader.When(x => x.RegisterLoadMethod(Arg.Any<AssetLoadMethod>())).Do(x => loadMethod = x[0] as AssetLoadMethod);
            mockLoader.When(x => x.LoadBank()).Do(x => loadMethod(_mockEngine.AssetBank));

            GameObjectFactory mockGameObjectFactory = Substitute.For<GameObjectFactory>();
            GameObjectFactory.SetInstance(mockGameObjectFactory);

            _sut = new MissionDebriefingScene(_mockEngine, _mockDeviceModel, _mockGameStatsController);
        }

        #region Textures
        [TestMethod]
        public void InitializeShouldLoadTexture_MissionDebriefingBackground()
        {
            string target = "MissionDebriefingBackground";
            ITexture2D mockTexture = Substitute.For<ITexture2D>();
            _mockEngine.GrfxFactory.Texture2DFactory.Create(_mockEngine.Content, "Textures\\" + target).Returns(mockTexture);

            _sut.Initialize();

            Received.InOrder(
                () =>
                {
                    _mockEngine.GrfxFactory.Texture2DFactory.Create(_mockEngine.Content, "Textures\\" + target);
                    _mockEngine.AssetBank.Put<ITexture2D>(target, mockTexture);
                }
            );
        }

        [TestMethod]
        public void InitializeShouldLoadTexture_MissionDebriefingLightOverlay()
        {
            string target = "MissionDebriefingLightOverlay";
            ITexture2D mockTexture = Substitute.For<ITexture2D>();
            _mockEngine.GrfxFactory.Texture2DFactory.Create(_mockEngine.Content, "Textures\\" + target).Returns(mockTexture);

            _sut.Initialize();

            Received.InOrder(
                () =>
                {
                    _mockEngine.GrfxFactory.Texture2DFactory.Create(_mockEngine.Content, "Textures\\" + target);
                    _mockEngine.AssetBank.Put<ITexture2D>(target, mockTexture);
                }
            );
        }

        [TestMethod]
        public void InitializeShouldLoadTexture_MissionDebriefMarks()
        {
            string target = "MissionDebriefMarks";
            ITexture2D mockTexture = Substitute.For<ITexture2D>();
            _mockEngine.GrfxFactory.Texture2DFactory.Create(_mockEngine.Content, "Textures\\" + target).Returns(mockTexture);

            _sut.Initialize();

            Received.InOrder(
                () =>
                {
                    _mockEngine.GrfxFactory.Texture2DFactory.Create(_mockEngine.Content, "Textures\\" + target);
                    _mockEngine.AssetBank.Put<ITexture2D>(target, mockTexture);
                }
            );
        }

        [TestMethod]
        public void InitializeShouldLoadTexture_MissionDebriefEqualityMarks()
        {
            string target = "MissionDebriefEqualityMarks";
            ITexture2D mockTexture = Substitute.For<ITexture2D>();
            _mockEngine.GrfxFactory.Texture2DFactory.Create(_mockEngine.Content, "Textures\\" + target).Returns(mockTexture);

            _sut.Initialize();

            Received.InOrder(
                () =>
                {
                    _mockEngine.GrfxFactory.Texture2DFactory.Create(_mockEngine.Content, "Textures\\" + target);
                    _mockEngine.AssetBank.Put<ITexture2D>(target, mockTexture);
                }
            );
        }

        [TestMethod]
        public void InitializeShouldLoadTexture_MissionDebriefSignature()
        {
            string target = "MissionDebriefSignature";
            ITexture2D mockTexture = Substitute.For<ITexture2D>();
            _mockEngine.GrfxFactory.Texture2DFactory.Create(_mockEngine.Content, "Textures\\" + target).Returns(mockTexture);

            _sut.Initialize();

            Received.InOrder(
                () =>
                {
                    _mockEngine.GrfxFactory.Texture2DFactory.Create(_mockEngine.Content, "Textures\\" + target);
                    _mockEngine.AssetBank.Put<ITexture2D>(target, mockTexture);
                }
            );
        }

        [TestMethod]
        public void InitializeShouldLoadTexture_DebriefPhoto_Default_Device()
        {
            string target = "DebriefPhoto_Default_Device";
            ITexture2D mockTexture = Substitute.For<ITexture2D>();
            _mockEngine.GrfxFactory.Texture2DFactory.Create(_mockEngine.Content, "Textures\\" + target).Returns(mockTexture);

            _sut.Initialize();

            Received.InOrder(
                () =>
                {
                    _mockEngine.GrfxFactory.Texture2DFactory.Create(_mockEngine.Content, "Textures\\" + target);
                    _mockEngine.AssetBank.Put<ITexture2D>(target, mockTexture);
                }
            );
        }

        [TestMethod]
        public void InitializeShouldLoadTexture_DebriefPhoto_Default_Objective()
        {
            string target = "DebriefPhoto_Default_Objective";
            ITexture2D mockTexture = Substitute.For<ITexture2D>();
            _mockEngine.GrfxFactory.Texture2DFactory.Create(_mockEngine.Content, "Textures\\" + target).Returns(mockTexture);

            _sut.Initialize();

            Received.InOrder(
                () =>
                {
                    _mockEngine.GrfxFactory.Texture2DFactory.Create(_mockEngine.Content, "Textures\\" + target);
                    _mockEngine.AssetBank.Put<ITexture2D>(target, mockTexture);
                }
            );
        }

        [TestMethod]
        public void InitializeShouldLoadTexture_DebriefPhoto_Default_Setting()
        {
            string target = "DebriefPhoto_Default_Setting";
            ITexture2D mockTexture = Substitute.For<ITexture2D>();
            _mockEngine.GrfxFactory.Texture2DFactory.Create(_mockEngine.Content, "Textures\\" + target).Returns(mockTexture);

            _sut.Initialize();

            Received.InOrder(
                () =>
                {
                    _mockEngine.GrfxFactory.Texture2DFactory.Create(_mockEngine.Content, "Textures\\" + target);
                    _mockEngine.AssetBank.Put<ITexture2D>(target, mockTexture);
                }
            );
        }
        #endregion

        #region Fonts
        [TestMethod]
        public void InitializeShouldLoadFont_TypewriterNormalFont()
        {
            string target = "TypewriterNormalFont";
            ISpriteFont mockFont = Substitute.For<ISpriteFont>();
            _mockEngine.GrfxFactory.SpriteFontFactory.Create(_mockEngine.Content, "Fonts\\" + target).Returns(mockFont);

            _sut.Initialize();

            Received.InOrder(
                () =>
                {
                    _mockEngine.GrfxFactory.SpriteFontFactory.Create(_mockEngine.Content, "Fonts\\" + target);
                    _mockEngine.AssetBank.Put<ISpriteFont>(target, mockFont);
                }
            );
        }

        [TestMethod]
        public void InitializeShouldLoadFont_TypewriterTitleFont()
        {
            string target = "TypewriterTitleFont";
            ISpriteFont mockFont = Substitute.For<ISpriteFont>();
            _mockEngine.GrfxFactory.SpriteFontFactory.Create(_mockEngine.Content, "Fonts\\" + target).Returns(mockFont);

            _sut.Initialize();

            Received.InOrder(
                () =>
                {
                    _mockEngine.GrfxFactory.SpriteFontFactory.Create(_mockEngine.Content, "Fonts\\" + target);
                    _mockEngine.AssetBank.Put<ISpriteFont>(target, mockFont);
                }
            );
        }

        [TestMethod]
        public void InitializeShouldLoadFont_TypewriterSmallFont()
        {
            string target = "TypewriterSmallFont";
            ISpriteFont mockFont = Substitute.For<ISpriteFont>();
            _mockEngine.GrfxFactory.SpriteFontFactory.Create(_mockEngine.Content, "Fonts\\" + target).Returns(mockFont);

            _sut.Initialize();

            Received.InOrder(
                () =>
                {
                    _mockEngine.GrfxFactory.SpriteFontFactory.Create(_mockEngine.Content, "Fonts\\" + target);
                    _mockEngine.AssetBank.Put<ISpriteFont>(target, mockFont);
                }
            );
        }
        #endregion

        #region Songs
        [TestMethod]
        public void InitializeShouldLodaSong_Debriefing()
        {
            string target = "Debriefing";
            ISong mockSong = Substitute.For<ISong>();
            _mockEngine.AudioManager.MusicController.CreateSong(_mockEngine.Content, "Music\\" + target).Returns(mockSong);

            _sut.Initialize();

            Received.InOrder(
                () =>
                {
                    _mockEngine.AudioManager.MusicController.CreateSong(_mockEngine.Content, "Music\\" + target);
                    _mockEngine.AssetBank.Put<ISong>(target, mockSong);
                }
            );
        }
        #endregion
    }
}
