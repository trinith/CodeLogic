using ArbitraryPixel.CodeLogic.Common.Config;
using ArbitraryPixel.CodeLogic.Common.Controllers;
using ArbitraryPixel.CodeLogic.Common.Model;
using ArbitraryPixel.CodeLogic.Common.UI;
using ArbitraryPixel.Common;
using ArbitraryPixel.Common.ContentManagement;
using ArbitraryPixel.Common.Drawing;
using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Common.Screen;
using ArbitraryPixel.Platform2D.Engine;
using ArbitraryPixel.Platform2D.Scene;
using ArbitraryPixel.Platform2D.Text;
using ArbitraryPixel.Platform2D.UI.Factory;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;
using NSubstitute;
using System;
using System.Linq;

namespace ArbitraryPixel.CodeLogic.Common_Tests
{
    [TestClass]
    public class CodeLogicEngine_InitializeAndLoadContent_Tests : CodeLogicEngineTestBase
    {
        #region Initialize Tests
        [TestMethod]
        public void InitializeShouldSetSongManagerIsRepeatingToTrue()
        {
            _sut.Initialize();

            _mockAudioManager.MusicController.Received(1).IsRepeating = true;
        }

        [TestMethod]
        public void InitializeShouldSetMusicControllerEnabledToConfigValue_TestA()
        {
            _mockSettings.MusicEnabled = false;

            _sut.Initialize();

            _mockAudioManager.MusicController.Received(1).Enabled = false;
        }

        [TestMethod]
        public void InitializeShouldSetMusicControllerEnabledToConfigValue_TestB()
        {
            _mockSettings.MusicEnabled = true;

            _sut.Initialize();

            _mockAudioManager.MusicController.Received(1).Enabled = true;
        }

        [TestMethod]
        public void InitializeShouldSetMusicControllerVolumeAttentuationToExpectedValue()
        {
            _sut.Initialize();

            _mockAudioManager.MusicController.Received(1).VolumeAttenuation = 0f;
        }

        [TestMethod]
        public void InitializeShouldSetMusicControllerVolumeToConfigValue_TestA()
        {
            _mockSettings.MusicVolume.Returns(0.123f);

            _sut.Initialize();

            _mockAudioManager.MusicController.Received(1).Volume = 0.123f;
        }

        [TestMethod]
        public void InitializeShouldSetMusicControllerVolumeToConfigValue_TestB()
        {
            _mockSettings.MusicVolume.Returns(0.75f);

            _sut.Initialize();

            _mockAudioManager.MusicController.Received(1).Volume = 0.75f;
        }

        [TestMethod]
        public void InitializeShouldSetSoundControllerEnabledToConfigValue_TestA()
        {
            _mockSettings.SoundEnabled.Returns(true);

            _sut.Initialize();

            _mockAudioManager.SoundController.Received(1).Enabled = true;
        }

        [TestMethod]
        public void InitializeShouldSetSoundControllerEnabledToConfigValue_TestB()
        {
            _mockSettings.SoundEnabled.Returns(false);

            _sut.Initialize();

            _mockAudioManager.SoundController.Received(1).Enabled = false;
        }

        [TestMethod]
        public void InitializeShouldSetSoundControllerVolumeToConfigValue_TestA()
        {
            _mockSettings.SoundVolume.Returns(0.123f);

            _sut.Initialize();

            _mockAudioManager.SoundController.Received(1).Volume = 0.123f;
        }

        [TestMethod]
        public void InitializeShouldSetSoundControllerVolumeToConfigValue_TestB()
        {
            _mockSettings.SoundVolume.Returns(0.75f);

            _sut.Initialize();

            _mockAudioManager.SoundController.Received(1).Volume = 0.75f;
        }
        #endregion

        #region LoadContent Tests
        #region Misc Tests
        [TestMethod]
        public void LoadContentShouldCreateSpriteBatch()
        {
            _sut.LoadContent();

            _mockSpriteBatchFactory.Received(2).Create(_mockGraphics.GraphicsDevice);
        }

        [TestMethod]
        public void LoadContentShouldCallCreateRandomOnFactory()
        {
            _sut.LoadContent();

            _mockGameObjectFactory.Received(2).CreateRandom();
        }

        [TestMethod]
        public void LoadContentShouldCreateDeviceModel()
        {
            IRandom mockRandom = Substitute.For<IRandom>();
            _mockGameObjectFactory.CreateRandom().Returns(mockRandom);

            _sut.LoadContent();

            _mockGameObjectFactory.Received(1).CreateDeviceModel(mockRandom, _mockStopwatchManager);
        }

        [TestMethod]
        public void LoadContentShouldCreateAndStorePixel()
        {
            ITexture2D mockPixel = Substitute.For<ITexture2D>();
            _mockGrfxFactory.Texture2DFactory.Create(Arg.Any<IGrfxDevice>(), 1, 1).Returns(mockPixel);

            _sut.LoadContent();

            Received.InOrder(
                () =>
                {
                    _mockGrfxFactory.Texture2DFactory.Create(_mockGraphics.GraphicsDevice, 1, 1);
                    mockPixel.SetData<Color>(Arg.Is<Color[]>(x => x.SequenceEqual(new Color[] { Color.White })));
                    _mockAssetBank.Put<ITexture2D>("Pixel", mockPixel);
                }
            );
        }

        [TestMethod]
        public void LoadContentShouldSetExpectedScreenManagerOptions()
        {
            ITexture2D mockPixel = Substitute.For<ITexture2D>();
            _mockGrfxFactory.Texture2DFactory.Create(Arg.Any<IGrfxDevice>(), 1, 1).Returns(mockPixel);

            _sut.LoadContent();

            _mockScreenManager.Received(1).Options = Arg.Is<ScreenManagerOptions>(
                x =>
                    _mockSpriteBatch.Equals(x.SpriteBatch)
                    && mockPixel.Equals(x.ScreenBackground.Texture)
                    && Color.Black.Equals(x.ScreenBackground.Mask)
                    && mockPixel.Equals(x.WorldBackground.Texture)
                    && Color.CornflowerBlue.Equals(x.WorldBackground.Mask)
            );
        }
        #endregion

        #region LogPanelModel Tests
        [TestMethod]
        public void LoadContentShouldCreateLogPanelModel()
        {
            _mockScreenManager.World.Returns(new Point(123, 456));
            _mockLogPanelModel.WorldBounds.Returns(new SizeF(246, 135));

            _sut.LoadContent();

            Received.InOrder(
                () =>
                {
                    _mockGameObjectFactory.CreateLogPanelModel(_mockSettings, new SizeF(123, 456));
                    _mockLogPanelModel.ClosedSize = new SizeF(246, 0);
                    _mockLogPanelModel.PartialSize = new SizeF(246, 44);
                    _mockLogPanelModel.FullSize = new SizeF(246, 118);
                    _mockLogPanelModel.SetOffsetForMode();
                }
            );
        }
        #endregion

        #region Overlay Layer
        [TestMethod]
        public void LoadContentShouldCreateTextObjectBuilder()
        {
            _sut.LoadContent();

            Received.InOrder(
                () =>
                {
                    _mockGameObjectFactory.CreateTextFormatValueHandlerManager();
                    _mockGameObjectFactory.CreateTextFormatProcessor(Arg.Any<ITextFormatValueHandlerManager>());
                    _mockGameObjectFactory.CreateTextObjectFactory();
                    _mockGameObjectFactory.CreateTextObjectBuilder(Arg.Any<ITextFormatProcessor>(), Arg.Any<ITextObjectFactory>());
                }
            );
        }

        [TestMethod]
        public void LoadContentShouldCreateFontForOverlayFont()
        {
            _sut.LoadContent();

            _mockGrfxFactory.SpriteFontFactory.Received(1).Create(_mockContentManager, @"Fonts\OverlayFont");
        }

        [TestMethod]
        public void LoadContentShouldSetTextBuilderDefaultFont()
        {
            ISpriteFont mockFont = Substitute.For<ISpriteFont>();
            _mockGrfxFactory.SpriteFontFactory.Create(Arg.Any<IContentManager>(), @"Fonts\OverlayFont").Returns(mockFont);

            ITextObjectBuilder mockBuilder = Substitute.For<ITextObjectBuilder>();
            _mockGameObjectFactory.CreateTextObjectBuilder(Arg.Any<ITextFormatProcessor>(), Arg.Any<ITextObjectFactory>()).Returns(mockBuilder);

            _sut.LoadContent();

            mockBuilder.Received(1).DefaultFont = mockFont;
        }

        [TestMethod]
        public void LoadContentShouldCreateBuildInfoLayer()
        {
            _sut.LoadContent();

            Received.InOrder(
                () =>
                {
                    _mockGrfxFactory.SpriteBatchFactory.Create(_mockGraphics.GraphicsDevice); // For initial call.

                    // This doesn't check the exact parameters, but it's good enough.
                    _mockGrfxFactory.SpriteBatchFactory.Create(_mockGraphics.GraphicsDevice);
                    _mockGameObjectFactory.CreateBuildInfoOverlayModel(_mockBuildInfoStore, Arg.Any<ITextObjectBuilder>());
                    _mockGameObjectFactory.CreateBuildInfoOverlayLayer(_sut, Arg.Any<ISpriteBatch>(), Arg.Any<IBuildInfoOverlayLayerModel>(), Arg.Any<IRandom>());
                }
            );
        }
        #endregion

        #region Main Menu Scene
        [TestMethod]
        public void LoadContentShouldCreateMenuFactory()
        {
            _sut.LoadContent();

            _mockGameObjectFactory.Received(1).CreateMenuFactory();
        }

        [TestMethod]
        public void LoadContentShouldCreateMenuContentMap()
        {
            _sut.LoadContent();

            _mockGameObjectFactory.Received(1).CreateMenuContentMap();
        }

        [TestMethod]
        public void LoadContentShouldCreateMainMenuContentLayerFactory()
        {
            _sut.LoadContent();

            _mockGameObjectFactory.Received(1).CreateMainMenuContentLayerFactory();
        }

        [TestMethod]
        public void LoadContentShouldCreateMainMenuModel()
        {
            IMenuContentMap mockContentMap = Substitute.For<IMenuContentMap>();
            _mockGameObjectFactory.CreateMenuContentMap().Returns(mockContentMap);

            IMainMenuContentLayerFactory mockContentFactory = Substitute.For<IMainMenuContentLayerFactory>();
            _mockGameObjectFactory.CreateMainMenuContentLayerFactory().Returns(mockContentFactory);

            _sut.LoadContent();

            _mockGameObjectFactory.Received(1).CreateMainMenuModel(_sut, _mockMenuFactory, mockContentMap, mockContentFactory);
        }

        [TestMethod]
        public void LoadContentShouldCreateMainMenuScene()
        {
            IMainMenuModel mockMenuModel = Substitute.For<IMainMenuModel>();
            IMenuItem mockMenuRoot = Substitute.For<IMenuItem>();
            _mockGameObjectFactory.CreateMainMenuModel(Arg.Any<IEngine>(), Arg.Any<IMenuFactory>(), Arg.Any<IMenuContentMap>(), Arg.Any<IMainMenuContentLayerFactory>()).Returns(mockMenuModel);

            _sut.LoadContent();

            _mockGameObjectFactory.Received(1).CreateMainMenuScene(_sut, mockMenuModel, _mockMenuFactory, _mockGameStatsData);
        }

        [TestMethod]
        public void LoadContentShouldAddMenuMenuSceneToSceneList()
        {
            IScene mockScene = Substitute.For<IScene>();
            _mockGameObjectFactory.CreateMainMenuScene(Arg.Any<IEngine>(), Arg.Any<IMainMenuModel>(), Arg.Any<IMenuFactory>(), Arg.Any<IGameStatsData>()).Returns(mockScene);

            _sut.LoadContent();

            Assert.AreSame(mockScene, _sut.Scenes["MainMenu"]);
        }

        [TestMethod]
        public void LoadContentShouldInitializeMainMenuScene()
        {
            IScene mockScene = Substitute.For<IScene>();
            _mockGameObjectFactory.CreateMainMenuScene(Arg.Any<IEngine>(), Arg.Any<IMainMenuModel>(), Arg.Any<IMenuFactory>(), Arg.Any<IGameStatsData>()).Returns(mockScene);

            _sut.LoadContent();

            mockScene.Received(1).Initialize();
        }
        #endregion

        #region Device Asset Load Scene
        [TestMethod]
        public void LoadContentShouldCreateDeviceAssetLoadScene()
        {
            _sut.LoadContent();

            _mockGameObjectFactory.Received(1).CreateDeviceAssetLoadScene(_sut);
        }

        [TestMethod]
        public void LoadContentShouldAddDeviceAssetLoadSceneToSceneList()
        {
            IScene mockScene = Substitute.For<IScene>();
            _mockGameObjectFactory.CreateDeviceAssetLoadScene(Arg.Any<IEngine>()).Returns(mockScene);

            _sut.LoadContent();

            Assert.AreSame(mockScene, _sut.Scenes["DeviceAssetLoad"]);
        }

        [TestMethod]
        public void LoadContentShouldInitializeDeviceAssetLoadScene()
        {
            IScene mockScene = Substitute.For<IScene>();
            _mockGameObjectFactory.CreateDeviceAssetLoadScene(Arg.Any<IEngine>()).Returns(mockScene);

            _sut.LoadContent();

            mockScene.Received(1).Initialize();
        }
        #endregion

        #region Device Main Scene
        [TestMethod]
        public void LoadContentShouldCreateDeviceMainScene()
        {
            _sut.LoadContent();

            _mockGameObjectFactory.Received(1).CreateDeviceMainScene(_sut, _mockDeviceModel, _mockLogPanelModel);
        }

        [TestMethod]
        public void LoadContentShouldAddDeviceSceneToSceneList()
        {
            IScene mockScene = Substitute.For<IScene>();
            _mockGameObjectFactory.CreateDeviceMainScene(Arg.Any<IEngine>(), Arg.Any<IDeviceModel>(), Arg.Any<ILogPanelModel>()).Returns(mockScene);

            _sut.LoadContent();

            Assert.AreEqual<IScene>(mockScene, _sut.Scenes["DeviceMain"]);
        }

        [TestMethod]
        public void LoadContentShouldInitializeDeviceScene()
        {
            IScene mockScene = Substitute.For<IScene>();
            _mockGameObjectFactory.CreateDeviceMainScene(Arg.Any<IEngine>(), Arg.Any<IDeviceModel>(), Arg.Any<ILogPanelModel>()).Returns(mockScene);

            _sut.LoadContent();

            mockScene.Received(1).Initialize();
        }
        #endregion

        #region Device Boot Scene
        [TestMethod]
        public void LoadContentShouldCreateDeviceBootScene()
        {
            _sut.LoadContent();

            _mockGameObjectFactory.Received(1).CreateDeviceBootScene(_sut, _mockDeviceModel, _mockLogPanelModel);
        }

        [TestMethod]
        public void LoadContentShouldAddDeviceBootSceneToSceneList()
        {
            IScene mockScene = Substitute.For<IScene>();
            _mockGameObjectFactory.CreateDeviceBootScene(Arg.Any<IEngine>(), Arg.Any<IDeviceModel>(), Arg.Any<ILogPanelModel>()).Returns(mockScene);

            _sut.LoadContent();

            Assert.AreEqual<IScene>(mockScene, _sut.Scenes["DeviceBoot"]);
        }

        [TestMethod]
        public void LoadContentShouldInitializeDeviceBootScene()
        {
            IScene mockScene = Substitute.For<IScene>();
            _mockGameObjectFactory.CreateDeviceBootScene(Arg.Any<IEngine>(), Arg.Any<IDeviceModel>(), Arg.Any<ILogPanelModel>()).Returns(mockScene);

            _sut.LoadContent();

            mockScene.Received(1).Initialize();
        }
        #endregion

        #region Device Menu Scene
        [TestMethod]
        public void LoadContentShouldCreateDeviceMenuScene()
        {
            _sut.LoadContent();

            _mockGameObjectFactory.Received(1).CreateDeviceMenuScene(_sut, _mockDeviceModel);
        }

        [TestMethod]
        public void LoadContentShouldAddDeviceMenuSceneToSceneList()
        {
            IScene mockScene = Substitute.For<IScene>();
            _mockGameObjectFactory.CreateDeviceMenuScene(Arg.Any<IEngine>(), Arg.Any<IDeviceModel>()).Returns(mockScene);

            _sut.LoadContent();

            Assert.AreSame(mockScene, _sut.Scenes["DeviceMenu"]);
        }

        [TestMethod]
        public void LoadContentShouldInitializeDeviceMenuScene()
        {
            IScene mockScene = Substitute.For<IScene>();
            _mockGameObjectFactory.CreateDeviceMenuScene(Arg.Any<IEngine>(), Arg.Any<IDeviceModel>()).Returns(mockScene);

            _sut.LoadContent();

            mockScene.Received(1).Initialize();
        }
        #endregion

        #region Mission Debriefing Scene
        [TestMethod]
        public void LoadContentShouldCreateMissionDebriefingScene()
        {
            IGameStatsModel mockGameStatsModel = Substitute.For<IGameStatsModel>();
            _mockContainer.GetComponent<IGameStatsData>().Model.Returns(mockGameStatsModel);

            IGameStatsController mockGameStatsController = Substitute.For<IGameStatsController>();
            _mockGameObjectFactory.CreateGameStatsController(mockGameStatsModel).Returns(mockGameStatsController);

            _sut.LoadContent();

            _mockGameObjectFactory.Received(1).CreateMissionDebriefingScene(_sut, _mockDeviceModel, mockGameStatsController);
        }

        [TestMethod]
        public void LoadContentShouldAttachEventHandlerToGameStatsControllerUpdatedEvent()
        {
            IGameStatsModel mockGameStatsModel = Substitute.For<IGameStatsModel>();
            _mockContainer.GetComponent<IGameStatsData>().Model.Returns(mockGameStatsModel);

            IGameStatsController mockGameStatsController = Substitute.For<IGameStatsController>();
            _mockGameObjectFactory.CreateGameStatsController(mockGameStatsModel).Returns(mockGameStatsController);

            _sut.LoadContent();

            mockGameStatsController.Received(1).Updated += Arg.Any<EventHandler<EventArgs>>();
        }

        [TestMethod]
        public void LoadContentShouldAddMissionDebriefingSceneToSceneList()
        {
            IScene mockScene = Substitute.For<IScene>();
            _mockGameObjectFactory.CreateMissionDebriefingScene(Arg.Any<IEngine>(), Arg.Any<IDeviceModel>(), Arg.Any<IGameStatsController>()).Returns(mockScene);

            _sut.LoadContent();

            Assert.AreSame(mockScene, _sut.Scenes["MissionDebriefing"]);
        }

        [TestMethod]
        public void LoadContentShouldInitializeMissionDebriefingScene()
        {
            IScene mockScene = Substitute.For<IScene>();
            _mockGameObjectFactory.CreateMissionDebriefingScene(Arg.Any<IEngine>(), Arg.Any<IDeviceModel>(), Arg.Any<IGameStatsController>()).Returns(mockScene);

            _sut.LoadContent();

            mockScene.Received(1).Initialize();
        }
        #endregion

        #region PreGameAd Scene
        [TestMethod]
        public void LoadContentShouldCreatePreGameAdScene()
        {
            _sut.LoadContent();

            _mockGameObjectFactory.Received(1).CreatePreGameAdScene(_sut);
        }

        [TestMethod]
        public void LoadContentShouldAddPreGameAdSceneToSceneList()
        {
            IScene mockScene = Substitute.For<IScene>();
            _mockGameObjectFactory.CreatePreGameAdScene(Arg.Any<IEngine>()).Returns(mockScene);

            _sut.LoadContent();

            Assert.AreSame(mockScene, _sut.Scenes["PreGameAd"]);
        }

        [TestMethod]
        public void LoadContentShouldInitializePreGameAdScene()
        {
            IScene mockScene = Substitute.For<IScene>();
            _mockGameObjectFactory.CreatePreGameAdScene(Arg.Any<IEngine>()).Returns(mockScene);

            _sut.LoadContent();

            mockScene.Received(1).Initialize();
        }
        #endregion

        #region NoAdMessage Scene
        [TestMethod]
        public void LoadContentShouldCreateNoAdMessageScene()
        {
            _sut.LoadContent();

            _mockGameObjectFactory.Received(1).CreateNoAdMessageScene(_sut);
        }

        [TestMethod]
        public void LoadContentShouldAddNoAdMessageSceneToSceneList()
        {
            IScene mockScene = Substitute.For<IScene>();
            _mockGameObjectFactory.CreateNoAdMessageScene(Arg.Any<IEngine>()).Returns(mockScene);

            _sut.LoadContent();

            Assert.AreSame(mockScene, _sut.Scenes["NoAdMessage"]);
        }

        [TestMethod]
        public void LoadContentShouldInitializeNoAdMessageScene()
        {
            IScene mockScene = Substitute.For<IScene>();
            _mockGameObjectFactory.CreateNoAdMessageScene(Arg.Any<IEngine>()).Returns(mockScene);

            _sut.LoadContent();

            mockScene.Received(1).Initialize();
        }
        #endregion

        #region Game Start Scene
        [TestMethod]
        public void LoadContentShouldCreateGameStartupScene()
        {          
            _sut.LoadContent();

            _mockGameObjectFactory.Received(1).CreateGameStartupScene(_sut, _sut.Scenes["SplashScreen"]);
        }

        [TestMethod]
        public void LoadContentShouldAddGameStartupSceneToSceneList()
        {
            IScene mockScene = Substitute.For<IScene>();
            _mockGameObjectFactory.CreateGameStartupScene(Arg.Any<IEngine>(), Arg.Any<IScene>()).Returns(mockScene);

            _sut.LoadContent();

            Assert.AreSame(mockScene, _sut.Scenes["GameStart"]);
        }

        [TestMethod]
        public void LoadContentShouldInitializeGameStartupScene()
        {
            IScene mockScene = Substitute.For<IScene>();
            _mockGameObjectFactory.CreateGameStartupScene(Arg.Any<IEngine>(), Arg.Any<IScene>()).Returns(mockScene);

            _sut.LoadContent();

            mockScene.Received(1).Initialize();
        }
        #endregion

        #region Splash Screen Scene
        [TestMethod]
        public void LoadContentShouldCreateSplashScreenScene()
        {
            IUIObjectFactory mockUIObjectFactory = Substitute.For<IUIObjectFactory>();
            _mockGameObjectFactory.CreateUIObjectFactory().Returns(mockUIObjectFactory);

            _sut.LoadContent();

            _mockGameObjectFactory.Received(1).CreateSplashScreenScene(_sut, mockUIObjectFactory);
        }

        [TestMethod]
        public void LoadContentShouldAddSplashScreenSceneToSceneList()
        {
            IUIObjectFactory mockUIObjectFactory = Substitute.For<IUIObjectFactory>();
            _mockGameObjectFactory.CreateUIObjectFactory().Returns(mockUIObjectFactory);

            IScene mockScene = Substitute.For<IScene>();
            _mockGameObjectFactory.CreateSplashScreenScene(_sut, mockUIObjectFactory).Returns(mockScene);

            _sut.LoadContent();

            Assert.AreSame(mockScene, _sut.Scenes["SplashScreen"]);
        }

        [TestMethod]
        public void LoadContentShouldInitializeSplashScreenScene()
        {
            IUIObjectFactory mockUIObjectFactory = Substitute.For<IUIObjectFactory>();
            _mockGameObjectFactory.CreateUIObjectFactory().Returns(mockUIObjectFactory);

            IScene mockScene = Substitute.For<IScene>();
            _mockGameObjectFactory.CreateSplashScreenScene(_sut, mockUIObjectFactory).Returns(mockScene);

            _sut.LoadContent();

            mockScene.Received(1).Initialize();
        }
        #endregion
        #endregion

        #region IGameStatsController Updated Event Tests
        [TestMethod]
        public void GameStatsControllerUpdatedShouldCallGameStatsDataSaveData()
        {
            IGameStatsController mockGameStatsController = Substitute.For<IGameStatsController>();
            _mockGameObjectFactory.CreateGameStatsController(Arg.Any<IGameStatsModel>()).Returns(mockGameStatsController);
            _sut.LoadContent();

            mockGameStatsController.Updated += Raise.Event<EventHandler<EventArgs>>(mockGameStatsController, new EventArgs());

            _mockGameStatsData.Received(1).SaveData();
        }
        #endregion
    }
}
