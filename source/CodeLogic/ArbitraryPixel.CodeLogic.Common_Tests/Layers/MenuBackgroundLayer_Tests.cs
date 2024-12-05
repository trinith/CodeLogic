using ArbitraryPixel.CodeLogic.Common;
using ArbitraryPixel.CodeLogic.Common.Controllers;
using ArbitraryPixel.CodeLogic.Common.Entities;
using ArbitraryPixel.CodeLogic.Common.EntityGenerators;
using ArbitraryPixel.CodeLogic.Common.Layers;
using ArbitraryPixel.CodeLogic.Common.Model;
using ArbitraryPixel.Common;
using ArbitraryPixel.Common.Audio;
using ArbitraryPixel.Common.Drawing;
using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Platform2D.Animation;
using ArbitraryPixel.Platform2D.Engine;
using ArbitraryPixel.Platform2D.Entity;
using ArbitraryPixel.Platform2D.Layer;
using ArbitraryPixel.Platform2D.UI;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;
using NSubstitute;
using System;
using System.Linq;

namespace ArbitraryPixel.CodeLogic.Common_Tests.Layers
{
    [TestClass]
    public class MenuBackgroundLayer_Tests
    {
        private MenuBackgroundLayer _sut;
        private IEngine _mockEngine;
        private ISpriteBatch _mockSpriteBatch;
        private ISoundPlaybackController _mockSoundController;

        private GameObjectFactory _mockGOF;
        private ITexture2D _mockPlaneTexutre;

        private Point _screenSize = new Point(1000, 700);

        [TestInitialize]
        public void Initialize()
        {
            _mockEngine = Substitute.For<IEngine>();
            _mockSpriteBatch = Substitute.For<ISpriteBatch>();
            _mockSoundController = Substitute.For<ISoundPlaybackController>();

            _mockEngine.ScreenManager.World.Returns(_screenSize);

            _mockGOF = Substitute.For<GameObjectFactory>();
            GameObjectFactory.SetInstance(_mockGOF);

            _mockPlaneTexutre = Substitute.For<ITexture2D>();
            _mockEngine.AssetBank.Get<ITexture2D>("Plane").Returns(_mockPlaneTexutre);
        }

        private void Construct()
        {
            _sut = new MenuBackgroundLayer(_mockEngine, _mockSpriteBatch, _mockSoundController);
        }

        #region Constructor Tests
        #region Construct Null Parameter Tests
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullParameterShouldThrowExpectedException_SoundController()
        {
            _sut = new MenuBackgroundLayer(_mockEngine, _mockSpriteBatch, null);
        }
        #endregion

        #region Construct BackDrop
        [TestMethod]
        public void ConstructShouldCreateExpectedTextureEntityForBackdrop()
        {
            ITexture2D mockTexture = Substitute.For<ITexture2D>();
            _mockEngine.AssetBank.Get<ITexture2D>("MainMenuBackground").Returns(mockTexture);

            ISpriteBatch mockBackDropBatch = Substitute.For<ISpriteBatch>();
            _mockEngine.GrfxFactory.SpriteBatchFactory.Create(_mockEngine.Graphics.GraphicsDevice).Returns(mockBackDropBatch);

            ILayer mockBackDropLayer = Substitute.For<ILayer>();
            mockBackDropLayer.MainSpriteBatch.Returns(mockBackDropBatch);
            _mockGOF.CreateGenericLayer(_mockEngine, mockBackDropBatch).Returns(mockBackDropLayer);

            Construct();

            _mockGOF.Received(1).CreateTextureEntity(_mockEngine, new RectangleF(Vector2.Zero, (SizeF)_screenSize), mockBackDropBatch, mockTexture, Color.White);
        }

        [TestMethod]
        public void ConstructShouldCallBackDropLayerAddEntityWithExpectedObject()
        {
            ISpriteBatch mockBackDropBatch = Substitute.For<ISpriteBatch>();
            _mockEngine.GrfxFactory.SpriteBatchFactory.Create(_mockEngine.Graphics.GraphicsDevice).Returns(mockBackDropBatch);

            ILayer mockBackDropLayer = Substitute.For<ILayer>();
            mockBackDropLayer.MainSpriteBatch.Returns(mockBackDropBatch);
            _mockGOF.CreateGenericLayer(_mockEngine, mockBackDropBatch).Returns(mockBackDropLayer);

            ITexture2D mockPixel = Substitute.For<ITexture2D>();
            _mockEngine.AssetBank.Get<ITexture2D>("MainMenuBackground").Returns(mockPixel);
            ITextureEntity mockEntity = Substitute.For<ITextureEntity>();
            _mockGOF.CreateTextureEntity(_mockEngine, new RectangleF(Vector2.Zero, (SizeF)_screenSize), mockBackDropBatch, mockPixel, Color.White).Returns(mockEntity);

            Construct();

            mockBackDropLayer.Received(1).AddEntity(mockEntity);
        }

        [TestMethod]
        public void ConstructShouldAddBackdropLayerToEntities()
        {
            ISpriteBatch mockBackDropBatch = Substitute.For<ISpriteBatch>();
            _mockEngine.GrfxFactory.SpriteBatchFactory.Create(_mockEngine.Graphics.GraphicsDevice).Returns(mockBackDropBatch);

            ILayer mockBackDropLayer = Substitute.For<ILayer>();
            _mockGOF.CreateGenericLayer(_mockEngine, mockBackDropBatch).Returns(mockBackDropLayer);

            Construct();

            Assert.IsTrue(_sut.Entities.Contains(mockBackDropLayer));
        }
        #endregion

        #region Construct Cloud Cover Layer
        [TestMethod]
        public void ConstructShouldCreateCloudCoverLayer()
        {
            _mockEngine.ScreenManager.World.Returns(new Point(1000, 800));

            ITexture2D mockCloud0 = Substitute.For<ITexture2D>();
            ITexture2D mockCloud1 = Substitute.For<ITexture2D>();
            _mockEngine.AssetBank.Get<ITexture2D>("cloud0").Returns(mockCloud0);
            _mockEngine.AssetBank.Get<ITexture2D>("cloud1").Returns(mockCloud1);
            ITexture2D[] expectedTextures = new ITexture2D[] { mockCloud1, mockCloud1, mockCloud0, mockCloud1, mockCloud1 };

            ISpriteBatch mockCloudBatch = Substitute.For<ISpriteBatch>();
            _mockEngine.GrfxFactory.SpriteBatchFactory.Create(_mockEngine.Graphics.GraphicsDevice).Returns(Substitute.For<ISpriteBatch>(), mockCloudBatch);

            IRandom mockRandom = Substitute.For<IRandom>();
            _mockGOF.CreateRandom(1337).Returns(mockRandom);

            IEntityGenerator<ICloud> mockCloudGen = Substitute.For<IEntityGenerator<ICloud>>();
            _mockGOF.CreateCloudGenerator(new SizeF(1000, 800), Arg.Is<ITexture2D[]>(x => x.SequenceEqual(expectedTextures)), mockCloudBatch, mockRandom).Returns(mockCloudGen);

            ICloudControllerFactory mockControllerFactory = Substitute.For<ICloudControllerFactory>();
            _mockGOF.CreateCloudControllerFactory().Returns(mockControllerFactory);

            IObjectSearcher mockEntitySearcher = Substitute.For<IObjectSearcher>();
            _mockGOF.CreateObjectSearcher().Returns(mockEntitySearcher);

            Construct();

            _mockGOF.Received(1).CreateCloudCoverLayer(_mockEngine, mockCloudBatch, mockCloudGen, mockControllerFactory, mockRandom, mockEntitySearcher);
        }

        [TestMethod]
        public void ConstructShouldAddCloudCoverLayerToEntities()
        {
            _mockEngine.ScreenManager.World.Returns(new Point(1000, 800));

            ITexture2D mockCloud0 = Substitute.For<ITexture2D>();
            ITexture2D mockCloud1 = Substitute.For<ITexture2D>();
            _mockEngine.AssetBank.Get<ITexture2D>("cloud0").Returns(mockCloud0);
            _mockEngine.AssetBank.Get<ITexture2D>("cloud1").Returns(mockCloud1);
            ITexture2D[] expectedTextures = new ITexture2D[] { mockCloud1, mockCloud1, mockCloud0, mockCloud1, mockCloud1 };

            ISpriteBatch mockCloudBatch = Substitute.For<ISpriteBatch>();
            _mockEngine.GrfxFactory.SpriteBatchFactory.Create(_mockEngine.Graphics.GraphicsDevice).Returns(Substitute.For<ISpriteBatch>(), mockCloudBatch);

            IRandom mockRandom = Substitute.For<IRandom>();
            _mockGOF.CreateRandom(1337).Returns(mockRandom);

            IEntityGenerator<ICloud> mockCloudGen = Substitute.For<IEntityGenerator<ICloud>>();
            _mockGOF.CreateCloudGenerator(new SizeF(1000, 800), Arg.Is<ITexture2D[]>(x => x.SequenceEqual(expectedTextures)), mockCloudBatch, mockRandom).Returns(mockCloudGen);

            ICloudControllerFactory mockControllerFactory = Substitute.For<ICloudControllerFactory>();
            _mockGOF.CreateCloudControllerFactory().Returns(mockControllerFactory);

            IObjectSearcher mockEntitySearcher = Substitute.For<IObjectSearcher>();
            _mockGOF.CreateObjectSearcher().Returns(mockEntitySearcher);

            ICloudCoverLayer mockCloudLayer = Substitute.For<ICloudCoverLayer>();
            _mockGOF.CreateCloudCoverLayer(_mockEngine, mockCloudBatch, mockCloudGen, mockControllerFactory, mockRandom, mockEntitySearcher).Returns(mockCloudLayer);

            Construct();

            Assert.IsTrue(_sut.Entities.Contains(mockCloudLayer));
        }

        [TestMethod]
        public void ConstructShouldAttachToCloudCoverLayerLightningFlashedEvent()
        {
            ICloudCoverLayer mockCloudLayer = Substitute.For<ICloudCoverLayer>();
            _mockGOF.CreateCloudCoverLayer(_mockEngine, Arg.Any<ISpriteBatch>(), Arg.Any<IEntityGenerator<ICloud>>(), Arg.Any<ICloudControllerFactory>(), Arg.Any<IRandom>(), Arg.Any<IObjectSearcher>()).Returns(mockCloudLayer);

            Construct();

            mockCloudLayer.Received(1).LightningFlashed += Arg.Any<EventHandler<LightningFlashEventArgs>>();
        }
        #endregion

        #region Construct Plane
        [TestMethod]
        public void ConstructShouldCreateExpectedTextureEntityForPlane()
        {
            ITexture2D mockTexture = Substitute.For<ITexture2D>();
            mockTexture.Width.Returns(400);
            mockTexture.Height.Returns(300);
            _mockEngine.AssetBank.Get<ITexture2D>("Plane").Returns(mockTexture);

            Construct();

            RectangleF expectedBounds = new RectangleF(300, 200, 400, 300);
            _mockGOF.Received(1).CreateTextureEntity(_mockEngine, expectedBounds, _mockSpriteBatch, mockTexture, Color.White);
        }

        [TestMethod]
        public void ConstructShouldAddPlaneEntityToLayerEntities()
        {
            ITexture2D mockTexture = Substitute.For<ITexture2D>();
            mockTexture.Width.Returns(400);
            mockTexture.Height.Returns(300);
            _mockEngine.AssetBank.Get<ITexture2D>("Plane").Returns(mockTexture);

            ITextureEntity mockEntity = Substitute.For<ITextureEntity>();
            RectangleF expectedBounds = new RectangleF(300, 200, 400, 300);
            _mockGOF.CreateTextureEntity(_mockEngine, expectedBounds, _mockSpriteBatch, mockTexture, Color.White).Returns(mockEntity);

            Construct();

            Assert.IsTrue(_sut.Entities.Contains(mockEntity));
        }
        #endregion

        #region Construct Controller
        [TestMethod]
        public void ConstructShouldCreateExpectedPlanePositionController()
        {
            ITexture2D mockTexture = Substitute.For<ITexture2D>();
            mockTexture.Width.Returns(400);
            mockTexture.Height.Returns(300);
            _mockEngine.AssetBank.Get<ITexture2D>("Plane").Returns(mockTexture);

            ITextureEntity mockEntity = Substitute.For<ITextureEntity>();
            RectangleF expectedBounds = new RectangleF(300, 200, 400, 300);
            _mockGOF.CreateTextureEntity(_mockEngine, expectedBounds, _mockSpriteBatch, mockTexture, Color.White).Returns(mockEntity);

            Construct();

            _mockGOF.Received(1).CreatePlanePositionController(mockEntity);
        }
        #endregion

        #region Construct TapScreenText
        [TestMethod]
        public void ConstructShouldCreateTapScreenText()
        {
            ISpriteFont mockFont = Substitute.For<ISpriteFont>();
            _mockEngine.AssetBank.Get<ISpriteFont>("CreditsTitleFont").Returns(mockFont);

            IAnimationFactory<float> mockAnimationFactory = Substitute.For<IAnimationFactory<float>>();
            _mockGOF.CreateFloatAnimationFactory().Returns(mockAnimationFactory);

            Construct();

            _mockGOF.Received(1).CreateTapScreenText(_mockEngine, _mockSpriteBatch, mockFont, mockAnimationFactory);
        }

        [TestMethod]
        public void ConstructShouldAddTapScreenTextToEntities()
        {
            ISpriteFont mockFont = Substitute.For<ISpriteFont>();
            _mockEngine.AssetBank.Get<ISpriteFont>("CreditsTitleFont").Returns(mockFont);

            IAnimationFactory<float> mockAnimationFactory = Substitute.For<IAnimationFactory<float>>();
            _mockGOF.CreateFloatAnimationFactory().Returns(mockAnimationFactory);

            IGameEntity mockEntity = Substitute.For<IGameEntity>();
            _mockGOF.CreateTapScreenText(_mockEngine, _mockSpriteBatch, mockFont, mockAnimationFactory).Returns(mockEntity);

            Construct();

            Assert.IsTrue(_sut.Entities.Contains(mockEntity));
        }
        #endregion

        #region Construct Version Label
        [TestMethod]
        public void ConstructShouldCreateVersionLabel()
        {
            IBuildInfoStore mockBuildInfoStore = Substitute.For<IBuildInfoStore>();
            mockBuildInfoStore.Version.Returns("1.2.3.4");
            mockBuildInfoStore.BuildName.Returns("BuildName");
            mockBuildInfoStore.Platform.Returns("Platform");
            _mockEngine.GetComponent<IBuildInfoStore>().Returns(mockBuildInfoStore);

            ISpriteFont mockFont = Substitute.For<ISpriteFont>();
            mockFont.MeasureString(Arg.Any<string>()).Returns(new SizeF(100, 50));
            _mockEngine.AssetBank.Get<ISpriteFont>("VersionFont").Returns(mockFont);

            _mockEngine.ScreenManager.World.Returns(new Point(1000, 800));

            Vector2 expectedLocation = new Vector2(
                CodeLogicEngine.Constants.TextWindowPadding.Width,
                800 - CodeLogicEngine.Constants.TextWindowPadding.Height - 50
            );

            Construct();

            _mockGOF.Received(1).CreateGenericTextLabel(_mockEngine, expectedLocation, _mockSpriteBatch, mockFont, "BuildName - 1.2.3.4 (Platform)", Color.White);
        }

        [TestMethod]
        public void ConstructShouldAddVersionLabelToEntities()
        {
            IBuildInfoStore mockBuildInfoStore = Substitute.For<IBuildInfoStore>();
            mockBuildInfoStore.Version.Returns("1.2.3.4");
            mockBuildInfoStore.BuildName.Returns("BuildName");
            mockBuildInfoStore.Platform.Returns("Platform");
            _mockEngine.GetComponent<IBuildInfoStore>().Returns(mockBuildInfoStore);

            ISpriteFont mockFont = Substitute.For<ISpriteFont>();
            mockFont.MeasureString(Arg.Any<string>()).Returns(new SizeF(100, 50));
            _mockEngine.AssetBank.Get<ISpriteFont>("VersionFont").Returns(mockFont);

            _mockEngine.ScreenManager.World.Returns(new Point(1000, 800));

            Vector2 expectedLocation = new Vector2(
                CodeLogicEngine.Constants.TextWindowPadding.Width,
                800 - CodeLogicEngine.Constants.TextWindowPadding.Height - 50
            );

            ITextLabel mockEntity = Substitute.For<ITextLabel>();
            _mockGOF.CreateGenericTextLabel(_mockEngine, expectedLocation, _mockSpriteBatch, mockFont, "BuildName - 1.2.3.4 (Platform)", Color.White).Returns(mockEntity);

            Construct();

            Assert.IsTrue(_sut.Entities.Contains(mockEntity));
        }
        #endregion

        #region Construct ScreenFadeOverlay
        [TestMethod]
        public void ConstructShouldCreateScreenFadeOverlay()
        {
            IAnimationFactory<float> mockAnimFactory = Substitute.For<IAnimationFactory<float>>();
            _mockGOF.CreateFloatAnimationFactory().Returns(mockAnimFactory);

            ITexture2D mockTexture = Substitute.For<ITexture2D>();
            _mockEngine.AssetBank.Get<ITexture2D>("Pixel").Returns(mockTexture);

            Construct();

            _mockGOF.Received(1).CreateScreenFadeOverlay(_mockEngine, _mockSpriteBatch, mockTexture, mockAnimFactory);
        }

        [TestMethod]
        public void ConstructShouldSetScreenFadeOverlayOpaqueColourToExpectedValue()
        {
            IAnimationFactory<float> mockAnimFactory = Substitute.For<IAnimationFactory<float>>();
            _mockGOF.CreateFloatAnimationFactory().Returns(mockAnimFactory);

            ITexture2D mockTexture = Substitute.For<ITexture2D>();
            _mockEngine.AssetBank.Get<ITexture2D>("Pixel").Returns(mockTexture);

            IScreenFadeOverlay mockOverlay = Substitute.For<IScreenFadeOverlay>();
            _mockGOF.CreateScreenFadeOverlay(_mockEngine, _mockSpriteBatch, mockTexture, mockAnimFactory).Returns(mockOverlay);

            Construct();

            mockOverlay.Received(1).OpaqueColour = Color.Black;
        }

        [TestMethod]
        public void ConstructShouldAttachToFadeOverlayAnimationFinishedEvent()
        {
            IAnimationFactory<float> mockAnimFactory = Substitute.For<IAnimationFactory<float>>();
            _mockGOF.CreateFloatAnimationFactory().Returns(mockAnimFactory);

            ITexture2D mockTexture = Substitute.For<ITexture2D>();
            _mockEngine.AssetBank.Get<ITexture2D>("Pixel").Returns(mockTexture);

            IScreenFadeOverlay mockOverlay = Substitute.For<IScreenFadeOverlay>();
            _mockGOF.CreateScreenFadeOverlay(_mockEngine, _mockSpriteBatch, mockTexture, mockAnimFactory).Returns(mockOverlay);

            Construct();

            mockOverlay.Received(1).AnimationFinished += Arg.Any<EventHandler<EventArgs>>();
        }

        [TestMethod]
        public void ConstructShouldAddScreenFadeOverlayToEntities()
        {
            IAnimationFactory<float> mockAnimFactory = Substitute.For<IAnimationFactory<float>>();
            _mockGOF.CreateFloatAnimationFactory().Returns(mockAnimFactory);

            ITexture2D mockTexture = Substitute.For<ITexture2D>();
            _mockEngine.AssetBank.Get<ITexture2D>("Pixel").Returns(mockTexture);

            IScreenFadeOverlay mockOverlay = Substitute.For<IScreenFadeOverlay>();
            _mockGOF.CreateScreenFadeOverlay(_mockEngine, _mockSpriteBatch, mockTexture, mockAnimFactory).Returns(mockOverlay);

            Construct();

            Assert.IsTrue(_sut.Entities.Contains(mockOverlay));
        }
        #endregion

        #region Construct Ambient Plane Sound
        [TestMethod]
        public void ConstructShouldCreateAmbientPlaneSound()
        {
            Construct();

            _mockEngine.AssetBank.Get<ISoundResource>("AirplaneNormal").Received(1).CreateInstance();
        }

        [TestMethod]
        public void ConstructShouldSetAmbientPlaneSoundVolume()
        {
            ISound mockSound = Substitute.For<ISound>();
            _mockEngine.AssetBank.Get<ISoundResource>("AirplaneNormal").CreateInstance().Returns(mockSound);

            Construct();

            mockSound.Received(1).Volume = 0f;
        }

        [TestMethod]
        public void ConstructShouldSetAmbientPlaneSoundIsLooped()
        {
            ISound mockSound = Substitute.For<ISound>();
            _mockEngine.AssetBank.Get<ISoundResource>("AirplaneNormal").CreateInstance().Returns(mockSound);

            Construct();

            mockSound.Received(1).IsLooped = true;
        }

        [TestMethod]
        public void ConstructShouldPlayAmbientPlaneSound()
        {
            ISound mockSound = Substitute.For<ISound>();
            _mockEngine.AssetBank.Get<ISoundResource>("AirplaneNormal").CreateInstance().Returns(mockSound);

            Construct();

            mockSound.Received(1).Play();
        }
        #endregion

        #region Entity Order Check
        [TestMethod]
        public void ConstructShouldAddEntitiesInExpectedOrder()
        {
            ILayer mockBackDrop = Substitute.For<ILayer>();
            _mockGOF.CreateGenericLayer(_mockEngine, Arg.Any<ISpriteBatch>()).Returns(mockBackDrop);

            ICloudCoverLayer mockCloudLayer = Substitute.For<ICloudCoverLayer>();
            _mockGOF.CreateCloudCoverLayer(_mockEngine, Arg.Any<ISpriteBatch>(), Arg.Any<IEntityGenerator<ICloud>>(), Arg.Any<ICloudControllerFactory>(), Arg.Any<IRandom>(), Arg.Any<IObjectSearcher>()).Returns(mockCloudLayer);

            ITextureEntity mockPlane = Substitute.For<ITextureEntity>();
            _mockGOF.CreateTextureEntity(_mockEngine, Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>(), Arg.Any<ITexture2D>(), Arg.Any<Color>()).Returns(mockPlane);

            IGameEntity mockTapText = Substitute.For<IGameEntity>();
            _mockGOF.CreateTapScreenText(_mockEngine, Arg.Any<ISpriteBatch>(), Arg.Any<ISpriteFont>(), Arg.Any<IAnimationFactory<float>>()).Returns(mockTapText);

            ITextLabel mockTextLabel = Substitute.For<ITextLabel>();
            _mockGOF.CreateGenericTextLabel(_mockEngine, Arg.Any<Vector2>(), Arg.Any<ISpriteBatch>(), Arg.Any<ISpriteFont>(), Arg.Any<string>(), Arg.Any<Color>()).Returns(mockTextLabel);

            IScreenFadeOverlay mockOverlay = Substitute.For<IScreenFadeOverlay>();
            _mockGOF.CreateScreenFadeOverlay(_mockEngine, _mockSpriteBatch, Arg.Any<ITexture2D>(), Arg.Any<IAnimationFactory<float>>()).Returns(mockOverlay);

            Construct();

            Assert.IsTrue(_sut.Entities.SequenceEqual(new IEntity[] { mockBackDrop, mockCloudLayer, mockPlane, mockTapText, mockTextLabel, mockOverlay }));
        }
        #endregion
        #endregion

        #region Reset Tests
        [TestMethod]
        public void ResetShouldClearAndRebuildEntities()
        {
            // Check to see that entities contains the expected objects after a reset.

            ILayer mockBackDrop = Substitute.For<ILayer>();
            _mockGOF.CreateGenericLayer(_mockEngine, Arg.Any<ISpriteBatch>()).Returns(Substitute.For<ILayer>(), mockBackDrop);

            ICloudCoverLayer mockCloudLayer = Substitute.For<ICloudCoverLayer>();
            _mockGOF.CreateCloudCoverLayer(_mockEngine, Arg.Any<ISpriteBatch>(), Arg.Any<IEntityGenerator<ICloud>>(), Arg.Any<ICloudControllerFactory>(), Arg.Any<IRandom>(), Arg.Any<IObjectSearcher>()).Returns(Substitute.For<ICloudCoverLayer>(), mockCloudLayer);

            ITextureEntity mockPlane = Substitute.For<ITextureEntity>();
            _mockGOF.CreateTextureEntity(_mockEngine, Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>(), Arg.Any<ITexture2D>(), Arg.Any<Color>()).Returns(Substitute.For<ITextureEntity>(), mockPlane);

            IGameEntity mockTapText = Substitute.For<IGameEntity>();
            _mockGOF.CreateTapScreenText(_mockEngine, Arg.Any<ISpriteBatch>(), Arg.Any<ISpriteFont>(), Arg.Any<IAnimationFactory<float>>()).Returns(Substitute.For<IGameEntity>(), mockTapText);

            ITextLabel mockTextLabel = Substitute.For<ITextLabel>();
            _mockGOF.CreateGenericTextLabel(_mockEngine, Arg.Any<Vector2>(), Arg.Any<ISpriteBatch>(), Arg.Any<ISpriteFont>(), Arg.Any<string>(), Arg.Any<Color>()).Returns(mockTextLabel);

            IScreenFadeOverlay mockOverlay = Substitute.For<IScreenFadeOverlay>();
            _mockGOF.CreateScreenFadeOverlay(_mockEngine, _mockSpriteBatch, Arg.Any<ITexture2D>(), Arg.Any<IAnimationFactory<float>>()).Returns(Substitute.For<IScreenFadeOverlay>(), mockOverlay);

            Construct();

            _sut.Reset();

            Assert.IsTrue(_sut.Entities.SequenceEqual(new IEntity[] { mockBackDrop, mockCloudLayer, mockPlane, mockTapText, mockTextLabel, mockOverlay }));
        }

        [TestMethod]
        public void ResetShouldCallResetOnSoundController()
        {
            Construct();

            _sut.Reset();

            _mockSoundController.Received(1).Reset();
        }

        [TestMethod]
        public void ResetShouldSubscribeToAudioManagerSoundControllerEnabledChangedEvent()
        {
            Construct();

            _sut.Reset();

            _mockEngine.AudioManager.SoundController.Received(1).EnabledChanged += Arg.Any<EventHandler<StateChangedEventArgs<bool>>>();
        }
        #endregion

        #region StopSounds Tests
        [TestMethod]
        public void StopSoundsShouldCallResetOnSoundController()
        {
            Construct();

            _sut.StopSounds();

            _mockSoundController.Received(1).Reset();
        }

        [TestMethod]
        public void StopSoundShouldUnsubscribeFromAudioManagerSoundControllerEnabledChangedEvent()
        {
            Construct();

            _sut.StopSounds();

            _mockEngine.AudioManager.SoundController.Received(1).EnabledChanged -= Arg.Any<EventHandler<StateChangedEventArgs<bool>>>();
        }
        #endregion

        #region Update Tests
        [TestMethod]
        public void UpdateShouldCallUpdateInPlanePositionController()
        {
            IController mockController = Substitute.For<IController>();
            _mockGOF.CreatePlanePositionController(Arg.Any<IGameEntity>()).Returns(mockController);
            GameTime expected = new GameTime();
            Construct();

            _sut.Update(expected);

            mockController.Received(1).Update(expected);
        }

        [TestMethod]
        public void UpdateAfterStartEndSequenceShouldUpdatePlaneLandingController()
        {
            _mockEngine.ScreenManager.World.Returns(new Point(800, 600));

            ITexture2D mockPlaneTexture = Substitute.For<ITexture2D>();
            _mockEngine.AssetBank.Get<ITexture2D>("Plane").Returns(mockPlaneTexture);

            ITextureEntity mockPlaneEntity = Substitute.For<ITextureEntity>();
            _mockGOF.CreateTextureEntity(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>(), mockPlaneTexture, Arg.Any<Color>()).Returns(mockPlaneEntity);

            IPlaneLandingController mockLandingController = Substitute.For<IPlaneLandingController>();
            _mockGOF.CreatePlaneLandingController(Arg.Any<IGameEntity>(), Arg.Any<SizeF>()).Returns(mockLandingController);

            Construct();
            _sut.StartEndSequence();
            GameTime gameTime = new GameTime();

            _sut.Update(gameTime);

            mockLandingController.Received(1).Update(gameTime);
        }

        [TestMethod]
        public void UpdateShouldCallUpdateOnSoundController()
        {
            Construct();

            GameTime gameTime = new GameTime();
            _sut.Update(gameTime);

            _mockSoundController.Received(1).Update(gameTime);
        }

        [TestMethod]
        public void UpdateShouldSetAmbientPlaneSoundToFadeOverlayCurrentValue()
        {
            ISound mockSound = Substitute.For<ISound>();
            _mockEngine.AssetBank.Get<ISoundResource>("AirplaneNormal").CreateInstance().Returns(mockSound);

            IScreenFadeOverlay mockFadeOverlay = Substitute.For<IScreenFadeOverlay>();
            mockFadeOverlay.CurrentValue.Returns(0.75f);
            _mockGOF.CreateScreenFadeOverlay(Arg.Any<IEngine>(), Arg.Any<ISpriteBatch>(), Arg.Any<ITexture2D>(), Arg.Any<IAnimationFactory<float>>()).Returns(mockFadeOverlay);

            Construct();

            _sut.Update(new GameTime());

            mockSound.Received(1).Volume = MathHelper.Lerp(0f, MenuBackgroundLayer.MaxPlaneAmbientVolume, 1f - 0.75f);
        }
        #endregion

        #region CloudCoverLayer LightningFlashed Event Tests
        [TestMethod]
        public void CloudCoverLayerLightningFlashedEventWithCloudShouldPlayExpectedSound_Depth1()
        {
            ISound mockSound = Substitute.For<ISound>();
            _mockEngine.AssetBank.Get<ISoundResource>("Thunder1").CreateInstance().Returns(mockSound);

            ICloud mockCloud = Substitute.For<ICloud>();
            mockCloud.Depth.Returns(1);

            ICloudCoverLayer mockCloudLayer = Substitute.For<ICloudCoverLayer>();
            _mockGOF.CreateCloudCoverLayer(_mockEngine, Arg.Any<ISpriteBatch>(), Arg.Any<IEntityGenerator<ICloud>>(), Arg.Any<ICloudControllerFactory>(), Arg.Any<IRandom>(), Arg.Any<IObjectSearcher>()).Returns(mockCloudLayer);

            Construct();

            mockCloudLayer.LightningFlashed += Raise.Event<EventHandler<LightningFlashEventArgs>>(mockCloudLayer, new LightningFlashEventArgs(mockCloud));

            Received.InOrder(
                () =>
                {
                    mockSound.Volume = 0.1f;
                    _mockSoundController.AddSound(mockSound, 3);
                }
            );
        }

        [TestMethod]
        public void CloudCoverLayerLightningFlashedEventWithCloudShouldPlayExpectedSound_Depth2()
        {
            ISound mockSound = Substitute.For<ISound>();
            _mockEngine.AssetBank.Get<ISoundResource>("Thunder1").CreateInstance().Returns(mockSound);

            ICloud mockCloud = Substitute.For<ICloud>();
            mockCloud.Depth.Returns(2);

            ICloudCoverLayer mockCloudLayer = Substitute.For<ICloudCoverLayer>();
            _mockGOF.CreateCloudCoverLayer(_mockEngine, Arg.Any<ISpriteBatch>(), Arg.Any<IEntityGenerator<ICloud>>(), Arg.Any<ICloudControllerFactory>(), Arg.Any<IRandom>(), Arg.Any<IObjectSearcher>()).Returns(mockCloudLayer);

            Construct();

            mockCloudLayer.LightningFlashed += Raise.Event<EventHandler<LightningFlashEventArgs>>(mockCloudLayer, new LightningFlashEventArgs(mockCloud));

            Received.InOrder(
                () =>
                {
                    mockSound.Volume = 0.5f;
                    _mockSoundController.AddSound(mockSound, 2);
                }
            );
        }

        [TestMethod]
        public void CloudCoverLayerLightningFlashedEventWithCloudShouldPlayExpectedSound_Depth3()
        {
            ISound mockSound = Substitute.For<ISound>();
            _mockEngine.AssetBank.Get<ISoundResource>("Thunder1").CreateInstance().Returns(mockSound);

            ICloud mockCloud = Substitute.For<ICloud>();
            mockCloud.Depth.Returns(3);

            ICloudCoverLayer mockCloudLayer = Substitute.For<ICloudCoverLayer>();
            _mockGOF.CreateCloudCoverLayer(_mockEngine, Arg.Any<ISpriteBatch>(), Arg.Any<IEntityGenerator<ICloud>>(), Arg.Any<ICloudControllerFactory>(), Arg.Any<IRandom>(), Arg.Any<IObjectSearcher>()).Returns(mockCloudLayer);

            Construct();

            mockCloudLayer.LightningFlashed += Raise.Event<EventHandler<LightningFlashEventArgs>>(mockCloudLayer, new LightningFlashEventArgs(mockCloud));

            Received.InOrder(
                () =>
                {
                    mockSound.Volume = 0.8f;
                    _mockSoundController.AddSound(mockSound, 1);
                }
            );
        }
        #endregion

        #region ScreenFadeOverlay Event Tests
        [TestMethod]
        public void FadeOverlayAnimationFinishedWithCurrentModeFadeOutShouldFireFadeOutCompleteEvent()
        {
            IScreenFadeOverlay mockOverlay = Substitute.For<IScreenFadeOverlay>();
            mockOverlay.CurrentMode.Returns(FadeMode.FadeOut);
            _mockGOF.CreateScreenFadeOverlay(Arg.Any<IEngine>(), Arg.Any<ISpriteBatch>(), Arg.Any<ITexture2D>(), Arg.Any<IAnimationFactory<float>>()).Returns(mockOverlay);

            Construct();

            var sub = Substitute.For<EventHandler<EventArgs>>();
            _sut.SequenceEnded += sub;

            mockOverlay.AnimationFinished += Raise.Event<EventHandler<EventArgs>>();

            sub.Received(1)(_sut, Arg.Any<EventArgs>());
        }

        [TestMethod]
        public void FadeOverlayAnimationFinishedWithoutCurentModeFadeOutShouldNotFireFadeOutCompleteEvent()
        {
            IScreenFadeOverlay mockOverlay = Substitute.For<IScreenFadeOverlay>();
            mockOverlay.CurrentMode.Returns(FadeMode.FadeIn);
            _mockGOF.CreateScreenFadeOverlay(Arg.Any<IEngine>(), Arg.Any<ISpriteBatch>(), Arg.Any<ITexture2D>(), Arg.Any<IAnimationFactory<float>>()).Returns(mockOverlay);

            Construct();

            var sub = Substitute.For<EventHandler<EventArgs>>();
            _sut.SequenceEnded += sub;

            mockOverlay.AnimationFinished += Raise.Event<EventHandler<EventArgs>>();

            sub.Received(0)(Arg.Any<object>(), Arg.Any<EventArgs>());
        }
        #endregion

        #region PlaneLandingController Event Tests
        [TestMethod]
        public void PlaneLandingControllerLandingFinishedShouldSetFadeOverlayCurrentModeToFadeOut()
        {
            _mockEngine.ScreenManager.World.Returns(new Point(800, 600));

            ITexture2D mockPlaneTexture = Substitute.For<ITexture2D>();
            _mockEngine.AssetBank.Get<ITexture2D>("Plane").Returns(mockPlaneTexture);

            ITextureEntity mockPlaneEntity = Substitute.For<ITextureEntity>();
            _mockGOF.CreateTextureEntity(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>(), mockPlaneTexture, Arg.Any<Color>()).Returns(mockPlaneEntity);

            IPlaneLandingController mockLandingController = Substitute.For<IPlaneLandingController>();
            _mockGOF.CreatePlaneLandingController(Arg.Any<IGameEntity>(), Arg.Any<SizeF>()).Returns(mockLandingController);

            IScreenFadeOverlay mockFadeOverlay = Substitute.For<IScreenFadeOverlay>();
            _mockGOF.CreateScreenFadeOverlay(Arg.Any<IEngine>(), Arg.Any<ISpriteBatch>(), Arg.Any<ITexture2D>(), Arg.Any<IAnimationFactory<float>>()).Returns(mockFadeOverlay);

            Construct();

            _sut.StartEndSequence();

            mockLandingController.LandingFinished += Raise.Event<EventHandler<EventArgs>>(mockLandingController, new EventArgs());

            mockFadeOverlay.Received(1).CurrentMode = FadeMode.FadeOut;
        }
        #endregion

        #region StartEndSequence Tests
        [TestMethod]
        public void StartEndSequenceShouldCreatePlaneLandingController()
        {
            _mockEngine.ScreenManager.World.Returns(new Point(800, 600));

            ITexture2D mockPlaneTexture = Substitute.For<ITexture2D>();
            _mockEngine.AssetBank.Get<ITexture2D>("Plane").Returns(mockPlaneTexture);

            ITextureEntity mockPlaneEntity = Substitute.For<ITextureEntity>();
            _mockGOF.CreateTextureEntity(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>(), mockPlaneTexture, Arg.Any<Color>()).Returns(mockPlaneEntity);

            IPlaneLandingController mockLandingController = Substitute.For<IPlaneLandingController>();
            _mockGOF.CreatePlaneLandingController(Arg.Any<IGameEntity>(), Arg.Any<SizeF>()).Returns(mockLandingController);

            Construct();

            _sut.StartEndSequence();

            Received.InOrder(
                () =>
                {
                    _mockGOF.CreatePlaneLandingController(mockPlaneEntity, new SizeF(800, 600));
                    mockLandingController.LandingFinished += Arg.Any<EventHandler<EventArgs>>();
                }
            );
        }

        [TestMethod]
        public void StartEndSequenceShouldHandleAmbientSoundChangeAsExpected()
        {
            ISound mockNormalSound = Substitute.For<ISound>();
            _mockEngine.AssetBank.Get<ISoundResource>("AirplaneNormal").CreateInstance().Returns(mockNormalSound);

            ISoundResource mockOtherResource = Substitute.For<ISoundResource>();
            _mockEngine.AssetBank.Get<ISoundResource>("AirplaneReducePowerFade").Returns(mockOtherResource);

            ISound mockOtherSound = Substitute.For<ISound>();
            mockOtherResource.CreateInstance().Returns(mockOtherSound);

            Construct();

            mockNormalSound.Volume.Returns(0.75f);
            _sut.StartEndSequence();

            Received.InOrder(
                () =>
                {
                    mockNormalSound.Stop();
                    mockOtherResource.CreateInstance();
                    mockOtherSound.Volume = 0.75f;
                    mockOtherSound.Play();
                }
            );
        }
        #endregion

        #region HideText Tests
        [TestMethod]
        public void HideTextShouldSetTapScreenTextVisibleToFalse()
        {
            IGameEntity mockText = Substitute.For<IGameEntity>();
            _mockGOF.CreateTapScreenText(Arg.Any<IEngine>(), Arg.Any<ISpriteBatch>(), Arg.Any<ISpriteFont>(), Arg.Any<IAnimationFactory<float>>()).Returns(mockText);
            Construct();

            _sut.HideText();

            mockText.Received(1).Visible = false;
        }

        [TestMethod]
        public void HideTextShouldSetTapScreenTextAliveToFalse()
        {
            IGameEntity mockText = Substitute.For<IGameEntity>();
            _mockGOF.CreateTapScreenText(Arg.Any<IEngine>(), Arg.Any<ISpriteBatch>(), Arg.Any<ISpriteFont>(), Arg.Any<IAnimationFactory<float>>()).Returns(mockText);
            Construct();

            _sut.HideText();

            mockText.Received(1).Alive = false;
        }
        #endregion

        #region AudioManager.SoundController.EnabledChanged Event Tests
        [TestMethod]
        public void SoundControllerEnabledChangedFromFalseToTrueShouldPlayAmbientPlaneSound()
        {
            ISound mockSound = Substitute.For<ISound>();
            _mockEngine.AssetBank.Get<ISoundResource>("AirplaneNormal").CreateInstance().Returns(mockSound);

            Construct();
            _sut.Reset();
            mockSound.ClearReceivedCalls();

            _mockEngine.AudioManager.SoundController.EnabledChanged += Raise.Event<EventHandler<StateChangedEventArgs<bool>>>(_mockEngine.AudioManager.SoundController, new StateChangedEventArgs<bool>(false, true));

            mockSound.Received(1).Play();
        }

        [TestMethod]
        public void SoundControllerEnabledChangedFromTrueToFalseShouldNotPlayPlaneAmbientSound()
        {
            ISound mockSound = Substitute.For<ISound>();
            _mockEngine.AssetBank.Get<ISoundResource>("AirplaneNormal").CreateInstance().Returns(mockSound);

            Construct();
            _sut.Reset();
            mockSound.ClearReceivedCalls();

            _mockEngine.AudioManager.SoundController.EnabledChanged += Raise.Event<EventHandler<StateChangedEventArgs<bool>>>(_mockEngine.AudioManager.SoundController, new StateChangedEventArgs<bool>(true, false));

            mockSound.Received(0).Play();
        }
        #endregion
    }
}
