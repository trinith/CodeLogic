using System;
using System.Collections.Generic;
using ArbitraryPixel.CodeLogic.Common;
using ArbitraryPixel.CodeLogic.Common.Controllers;
using ArbitraryPixel.CodeLogic.Common.Entities;
using ArbitraryPixel.CodeLogic.Common.Model;
using ArbitraryPixel.CodeLogic.Common.Scenes;
using ArbitraryPixel.Common.Drawing;
using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Platform2D.Animation;
using ArbitraryPixel.Platform2D.Engine;
using ArbitraryPixel.Platform2D.Layer;
using ArbitraryPixel.Platform2D.Scene;
using ArbitraryPixel.Platform2D.UI;
using ArbitraryPixel.Platform2D.UI.Factory;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NSubstitute;

namespace ArbitraryPixel.CodeLogic.Common_Tests.Scenes
{
    [TestClass]
    public class SplashScreenScene_Tests
    {
        private SplashScreenScene _sut;
        private IEngine _mockEngine;
        private IUIObjectFactory _mockUIObjectFactory;

        private GameObjectFactory _mockGOF;

        [TestInitialize]
        public void Initialize()
        {
            _mockEngine = Substitute.For<IEngine>();
            _mockUIObjectFactory = Substitute.For<IUIObjectFactory>();

            _mockGOF = Substitute.For<GameObjectFactory>();
            GameObjectFactory.SetInstance(_mockGOF);
        }

        private void Construct()
        {
            _sut = new SplashScreenScene(_mockEngine, _mockUIObjectFactory);
        }

        #region Constructor Tests
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullParameterShouldThrowExpectedException_UIObjectFactory()
        {
            _sut = new SplashScreenScene(_mockEngine, null);
        }

        [TestMethod]
        public void ConstructShouldCreateLayerFadeController()
        {
            IAnimationFactory<float> mockAnimationFactory = Substitute.For<IAnimationFactory<float>>();
            _mockGOF.CreateFloatAnimationFactory().Returns(mockAnimationFactory);

            Construct();

            _mockGOF.Received(1).CreateLayerFadeController(mockAnimationFactory, CodeLogicEngine.Constants.SplashScreenFadeTime);
        }
        #endregion

        #region Initialize Tests
        #region Background Layer Tests
        [TestMethod]
        public void InitializeShouldCreateBGLayer()
        {
            ISpriteBatch mockSpriteBatch = Substitute.For<ISpriteBatch>();
            _mockEngine.GrfxFactory.SpriteBatchFactory.Create(Arg.Any<IGrfxDevice>()).Returns(mockSpriteBatch, Substitute.For<ISpriteBatch>());

            Construct();

            _sut.Initialize();

            _mockGOF.Received(1).CreateGenericLayer(_mockEngine, mockSpriteBatch);
        }

        [TestMethod]
        public void InitializeShouldAddButtonToBGLayer()
        {
            _mockEngine.ScreenManager.World.Returns(new Point(800, 600));

            ISpriteBatch mockSpriteBatch = Substitute.For<ISpriteBatch>();
            _mockEngine.GrfxFactory.SpriteBatchFactory.Create(Arg.Any<IGrfxDevice>()).Returns(mockSpriteBatch, Substitute.For<ISpriteBatch>());

            ILayer mockBGLayer = Substitute.For<ILayer>();
            mockBGLayer.MainSpriteBatch.Returns(mockSpriteBatch);
            _mockGOF.CreateGenericLayer(_mockEngine, mockSpriteBatch).Returns(mockBGLayer);

            RectangleF expectedBounds = new RectangleF(Vector2.Zero, new SizeF(800, 600));
            IGenericButton mockButton = Substitute.For<IGenericButton>();
            _mockUIObjectFactory.CreateGenericButton(_mockEngine, expectedBounds, mockSpriteBatch).Returns(mockButton);

            Construct();

            _sut.Initialize();

            mockBGLayer.Received(1).AddEntity(mockButton);
        }

        [TestMethod]
        public void InitializeShouldAttachEventHandlerToButton()
        {
            _mockEngine.ScreenManager.World.Returns(new Point(800, 600));

            ISpriteBatch mockSpriteBatch = Substitute.For<ISpriteBatch>();
            _mockEngine.GrfxFactory.SpriteBatchFactory.Create(Arg.Any<IGrfxDevice>()).Returns(mockSpriteBatch, Substitute.For<ISpriteBatch>());

            ILayer mockBGLayer = Substitute.For<ILayer>();
            mockBGLayer.MainSpriteBatch.Returns(mockSpriteBatch);
            _mockGOF.CreateGenericLayer(_mockEngine, mockSpriteBatch).Returns(mockBGLayer);

            RectangleF expectedBounds = new RectangleF(Vector2.Zero, new SizeF(800, 600));
            IGenericButton mockButton = Substitute.For<IGenericButton>();
            _mockUIObjectFactory.CreateGenericButton(_mockEngine, expectedBounds, mockSpriteBatch).Returns(mockButton);

            Construct();

            _sut.Initialize();

            mockButton.Received(1).Tapped += Arg.Any<EventHandler<ButtonEventArgs>>();
        }

        [TestMethod]
        public void InitializeShouldAddTextureEntityToBackgroundLayer()
        {
            _mockEngine.ScreenManager.World.Returns(new Point(800, 600));

            ITexture2D mockTexture = Substitute.For<ITexture2D>();
            _mockEngine.AssetBank.Get<ITexture2D>("Pixel").Returns(mockTexture);

            ISpriteBatch mockSpriteBatch = Substitute.For<ISpriteBatch>();
            _mockEngine.GrfxFactory.SpriteBatchFactory.Create(Arg.Any<IGrfxDevice>()).Returns(mockSpriteBatch, Substitute.For<ISpriteBatch>());

            ILayer mockBGLayer = Substitute.For<ILayer>();
            mockBGLayer.MainSpriteBatch.Returns(mockSpriteBatch);
            _mockGOF.CreateGenericLayer(_mockEngine, mockSpriteBatch).Returns(mockBGLayer);

            RectangleF expectedBounds = new RectangleF(Vector2.Zero, new SizeF(800, 600));
            ITextureEntity mockEntity = Substitute.For<ITextureEntity>();
            _mockGOF.CreateTextureEntity(_mockEngine, expectedBounds, mockSpriteBatch, mockTexture, Color.Black).Returns(mockEntity);

            Construct();

            _sut.Initialize();

            mockBGLayer.Received(1).AddEntity(mockEntity);
        }

        [TestMethod]
        public void InitializeShouldAddBGLayerToEntities()
        {
            ISpriteBatch mockSpriteBatch = Substitute.For<ISpriteBatch>();
            _mockEngine.GrfxFactory.SpriteBatchFactory.Create(Arg.Any<IGrfxDevice>()).Returns(mockSpriteBatch, Substitute.For<ISpriteBatch>());

            ILayer mockBGLayer = Substitute.For<ILayer>();
            mockBGLayer.MainSpriteBatch.Returns(mockSpriteBatch);
            _mockGOF.CreateGenericLayer(_mockEngine, mockSpriteBatch).Returns(mockBGLayer);

            Construct();

            _sut.Initialize();

            Assert.AreSame(mockBGLayer, _sut.Entities[0]);
        }
        #endregion

        #region Logo Background Layer Tests
        [TestMethod]
        public void InitializeShouldCreateLogoBGLayer()
        {
            ISpriteBatch mockSpriteBatch = Substitute.For<ISpriteBatch>();
            _mockEngine.GrfxFactory.SpriteBatchFactory.Create(Arg.Any<IGrfxDevice>()).Returns(Substitute.For<ISpriteBatch>(), mockSpriteBatch, Substitute.For<ISpriteBatch>());

            Construct();

            _sut.Initialize();

            _mockGOF.Received(1).CreateGenericLayer(_mockEngine, mockSpriteBatch, SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp);
        }

        [TestMethod]
        public void InitializeShouldAddTextureEntityToLogoBGLayer()
        {
            _mockEngine.ScreenManager.World.Returns(new Point(800, 600));

            ISpriteBatch mockSpriteBatch = Substitute.For<ISpriteBatch>();
            _mockEngine.GrfxFactory.SpriteBatchFactory.Create(Arg.Any<IGrfxDevice>()).Returns(Substitute.For<ISpriteBatch>(), mockSpriteBatch, Substitute.For<ISpriteBatch>());

            ILayer mockLogoBGLayer = Substitute.For<ILayer>();
            mockLogoBGLayer.MainSpriteBatch.Returns(mockSpriteBatch);
            _mockGOF.CreateGenericLayer(_mockEngine, mockSpriteBatch, SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp).Returns(mockLogoBGLayer);

            ITexture2D mockTexture = Substitute.For<ITexture2D>();
            _mockEngine.AssetBank.Get<ITexture2D>("Pixel").Returns(mockTexture);

            RectangleF expectedBounds = new RectangleF(Vector2.Zero, new SizeF(800, 600));
            ITextureEntity mockEntity = Substitute.For<ITextureEntity>();
            _mockGOF.CreateTextureEntity(_mockEngine, expectedBounds, mockSpriteBatch, mockTexture, Color.White).Returns(mockEntity);

            Construct();

            _sut.Initialize();

            mockLogoBGLayer.Received(1).AddEntity(mockEntity);
        }

        [TestMethod]
        public void InitializeShouldAddLogoBGLayerToEntities()
        {
            ISpriteBatch mockSpriteBatch = Substitute.For<ISpriteBatch>();
            _mockEngine.GrfxFactory.SpriteBatchFactory.Create(Arg.Any<IGrfxDevice>()).Returns(Substitute.For<ISpriteBatch>(), mockSpriteBatch, Substitute.For<ISpriteBatch>());

            ILayer mockLogoBGLayer = Substitute.For<ILayer>();
            mockLogoBGLayer.MainSpriteBatch.Returns(mockSpriteBatch);
            _mockGOF.CreateGenericLayer(_mockEngine, mockSpriteBatch, SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp).Returns(mockLogoBGLayer);

            Construct();

            _sut.Initialize();

            Assert.AreSame(mockLogoBGLayer, _sut.Entities[1]);
        }
        #endregion

        #region Logo Layer Tests
        [TestMethod]
        public void InitializeShouldCreateLogoLayer()
        {
            ISpriteBatch mockSpriteBatch = Substitute.For<ISpriteBatch>();
            _mockEngine.GrfxFactory.SpriteBatchFactory.Create(Arg.Any<IGrfxDevice>()).Returns(Substitute.For<ISpriteBatch>(), Substitute.For<ISpriteBatch>(), mockSpriteBatch, Substitute.For<ISpriteBatch>());

            Construct();

            _sut.Initialize();

            _mockGOF.Received(1).CreateGenericLayer(_mockEngine, mockSpriteBatch, SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp);
        }

        [TestMethod]
        public void InitializeShouldAddTextureEntityToLogoLayer()
        {
            SizeF screenSize = new SizeF(800, 600);
            SizeF textureSize = new SizeF(200, 100);
            _mockEngine.ScreenManager.World.Returns(new Point((int)screenSize.Width, (int)screenSize.Height));

            ISpriteBatch mockSpriteBatch = Substitute.For<ISpriteBatch>();
            _mockEngine.GrfxFactory.SpriteBatchFactory.Create(Arg.Any<IGrfxDevice>()).Returns(Substitute.For<ISpriteBatch>(), Substitute.For<ISpriteBatch>(), mockSpriteBatch, Substitute.For<ISpriteBatch>());

            ILayer mockLogoLayer = Substitute.For<ILayer>();
            mockLogoLayer.MainSpriteBatch.Returns(mockSpriteBatch);
            _mockGOF.CreateGenericLayer(_mockEngine, mockSpriteBatch, SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp).Returns(mockLogoLayer);

            ITexture2D mockTexture = Substitute.For<ITexture2D>();
            mockTexture.Width.Returns((int)textureSize.Width);
            mockTexture.Height.Returns((int)textureSize.Height);
            _mockEngine.AssetBank.Get<ITexture2D>("APLogo").Returns(mockTexture);

            SizeF expandedTextureSize = new SizeF(textureSize.Width * 2, textureSize.Height * 2);
            RectangleF expectedBounds = new RectangleF(screenSize.Centre - expandedTextureSize.Centre, expandedTextureSize);

            ITextureEntity mockEntity = Substitute.For<ITextureEntity>();
            _mockGOF.CreateTextureEntity(_mockEngine, expectedBounds, mockSpriteBatch, mockTexture, Color.White).Returns(mockEntity);

            Construct();

            _sut.Initialize();

            mockLogoLayer.Received(1).AddEntity(mockEntity);
        }

        [TestMethod]
        public void InitializeShouldAddLogoLayerToEntities()
        {
            ISpriteBatch mockSpriteBatch = Substitute.For<ISpriteBatch>();
            _mockEngine.GrfxFactory.SpriteBatchFactory.Create(Arg.Any<IGrfxDevice>()).Returns(Substitute.For<ISpriteBatch>(), Substitute.For<ISpriteBatch>(), mockSpriteBatch, Substitute.For<ISpriteBatch>());

            ILayer mockLogoLayer = Substitute.For<ILayer>();
            mockLogoLayer.MainSpriteBatch.Returns(mockSpriteBatch);
            _mockGOF.CreateGenericLayer(_mockEngine, mockSpriteBatch, SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp).Returns(mockLogoLayer);

            Construct();

            _sut.Initialize();

            Assert.AreSame(mockLogoLayer, _sut.Entities[2]);
        }
        #endregion

        #region LayerFadeController Tests
        [TestMethod]
        public void InitializeShouldAddLogoLayerToLayerFadeController()
        {
            ILayerFadeController mockController = Substitute.For<ILayerFadeController>();
            _mockGOF.CreateLayerFadeController(Arg.Any<IAnimationFactory<float>>(), CodeLogicEngine.Constants.SplashScreenFadeTime).Returns(mockController);

            ISpriteBatch mockSpriteBatch = Substitute.For<ISpriteBatch>();
            _mockEngine.GrfxFactory.SpriteBatchFactory.Create(Arg.Any<IGrfxDevice>()).Returns(Substitute.For<ISpriteBatch>(), Substitute.For<ISpriteBatch>(), mockSpriteBatch, Substitute.For<ISpriteBatch>());

            ILayer mockLogoLayer = Substitute.For<ILayer>();
            mockLogoLayer.MainSpriteBatch.Returns(mockSpriteBatch);
            _mockGOF.CreateGenericLayer(_mockEngine, mockSpriteBatch, SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp).Returns(mockLogoLayer);

            Construct();

            _sut.Initialize();

            mockController.Received(1).AddLayer(mockLogoLayer);
        }

        [TestMethod]
        public void InitializeShouldCallLayerFadeControllerReset()
        {
            ILayerFadeController mockController = Substitute.For<ILayerFadeController>();
            _mockGOF.CreateLayerFadeController(Arg.Any<IAnimationFactory<float>>(), CodeLogicEngine.Constants.SplashScreenFadeTime).Returns(mockController);

            ISpriteBatch mockSpriteBatch = Substitute.For<ISpriteBatch>();
            _mockEngine.GrfxFactory.SpriteBatchFactory.Create(Arg.Any<IGrfxDevice>()).Returns(Substitute.For<ISpriteBatch>(), Substitute.For<ISpriteBatch>(), mockSpriteBatch, Substitute.For<ISpriteBatch>());

            ILayer mockLogoLayer = Substitute.For<ILayer>();
            mockLogoLayer.MainSpriteBatch.Returns(mockSpriteBatch);
            _mockGOF.CreateGenericLayer(_mockEngine, mockSpriteBatch, SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp).Returns(mockLogoLayer);

            Construct();

            _sut.Initialize();

            mockController.Received(1).Reset();
        }
        #endregion
        #endregion

        #region Update Tests
        [TestMethod]
        public void UpdateShouldCallLayerFadeControllerUpdate()
        {
            ILayerFadeController mockController = Substitute.For<ILayerFadeController>();
            _mockGOF.CreateLayerFadeController(Arg.Any<IAnimationFactory<float>>(), CodeLogicEngine.Constants.SplashScreenFadeTime).Returns(mockController);

            Construct();

            GameTime gameTime = new GameTime();
            _sut.Update(gameTime);

            mockController.Received(1).Update(gameTime);
        }

        [TestMethod]
        public void UpdateBeforeStartDelayShouldNotStartLayerFadeControllerFadeInAnimation()
        {
            ILayerFadeController mockController = Substitute.For<ILayerFadeController>();
            _mockGOF.CreateLayerFadeController(Arg.Any<IAnimationFactory<float>>(), CodeLogicEngine.Constants.SplashScreenFadeTime).Returns(mockController);

            ISpriteBatch mockSpriteBatch = Substitute.For<ISpriteBatch>();
            _mockEngine.GrfxFactory.SpriteBatchFactory.Create(Arg.Any<IGrfxDevice>()).Returns(Substitute.For<ISpriteBatch>(), Substitute.For<ISpriteBatch>(), mockSpriteBatch, Substitute.For<ISpriteBatch>());

            ILayer mockLogoLayer = Substitute.For<ILayer>();
            mockLogoLayer.MainSpriteBatch.Returns(mockSpriteBatch);
            _mockGOF.CreateGenericLayer(_mockEngine, mockSpriteBatch, SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp).Returns(mockLogoLayer);

            Construct();

            _sut.Initialize();

            _sut.Update(new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(0.9)));

            mockController.Received(0).StartAnimation(Arg.Any<FadeMode>());
        }

        [TestMethod]
        public void UpdateAfterStartDelayShouldStartLayerFadeControllerFadeInAnimation()
        {
            ILayerFadeController mockController = Substitute.For<ILayerFadeController>();
            _mockGOF.CreateLayerFadeController(Arg.Any<IAnimationFactory<float>>(), CodeLogicEngine.Constants.SplashScreenFadeTime).Returns(mockController);

            ISpriteBatch mockSpriteBatch = Substitute.For<ISpriteBatch>();
            _mockEngine.GrfxFactory.SpriteBatchFactory.Create(Arg.Any<IGrfxDevice>()).Returns(Substitute.For<ISpriteBatch>(), Substitute.For<ISpriteBatch>(), mockSpriteBatch, Substitute.For<ISpriteBatch>());

            ILayer mockLogoLayer = Substitute.For<ILayer>();
            mockLogoLayer.MainSpriteBatch.Returns(mockSpriteBatch);
            _mockGOF.CreateGenericLayer(_mockEngine, mockSpriteBatch, SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp).Returns(mockLogoLayer);

            Construct();

            _sut.Initialize();

            _sut.Update(new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(1)));

            mockController.Received(1).StartAnimation(FadeMode.FadeIn);
        }

        [TestMethod]
        public void UpdateShouldNotPerformExpectedFadeOutCalls_AddLayer()
        {
            ILayerFadeController mockController = Substitute.For<ILayerFadeController>();
            _mockGOF.CreateLayerFadeController(Arg.Any<IAnimationFactory<float>>(), CodeLogicEngine.Constants.SplashScreenFadeTime).Returns(mockController);

            Construct();

            _sut.Update(new GameTime());

            mockController.Received(0).AddLayer(Arg.Any<ILayer>());
        }

        [TestMethod]
        public void UpdateShouldNotPerformExpectedFadeOutCalls_StartAnimation()
        {
            ILayerFadeController mockController = Substitute.For<ILayerFadeController>();
            _mockGOF.CreateLayerFadeController(Arg.Any<IAnimationFactory<float>>(), CodeLogicEngine.Constants.SplashScreenFadeTime).Returns(mockController);

            Construct();

            _sut.Update(new GameTime());

            mockController.Received(0).StartAnimation(Arg.Any<FadeMode>());
        }

        [TestMethod]
        public void UpdateWhenFadeInCompleteAfterTapShouldPerformExpectedCalls()
        {
            ILayerFadeController mockController = Substitute.For<ILayerFadeController>();
            _mockGOF.CreateLayerFadeController(Arg.Any<IAnimationFactory<float>>(), CodeLogicEngine.Constants.SplashScreenFadeTime).Returns(mockController);

            ISpriteBatch mockSpriteBatch = Substitute.For<ISpriteBatch>();
            _mockEngine.GrfxFactory.SpriteBatchFactory.Create(Arg.Any<IGrfxDevice>()).Returns(Substitute.For<ISpriteBatch>(), mockSpriteBatch, Substitute.For<ISpriteBatch>());

            ILayer mockLogoBGLayer = Substitute.For<ILayer>();
            mockLogoBGLayer.MainSpriteBatch.Returns(mockSpriteBatch);
            _mockGOF.CreateGenericLayer(_mockEngine, mockSpriteBatch, SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp).Returns(mockLogoBGLayer);

            IGenericButton mockButton = Substitute.For<IGenericButton>();
            _mockUIObjectFactory.CreateGenericButton(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>()).Returns(mockButton);

            Construct();
            _sut.Initialize();

            mockController.IsAnimationComplete(FadeMode.FadeIn).Returns(true);
            mockButton.Tapped += Raise.Event<EventHandler<ButtonEventArgs>>(mockButton, new ButtonEventArgs(Vector2.Zero));

            _sut.Update(new GameTime());

            Received.InOrder(
                () =>
                {
                    mockController.AddLayer(mockLogoBGLayer);
                    mockController.StartAnimation(FadeMode.FadeOut);
                }
            );
        }

        [TestMethod]
        public void UpdateWhenFadeInCompleteAfterViewDelayShouldPerformExpectedCalls()
        {
            ILayerFadeController mockController = Substitute.For<ILayerFadeController>();
            _mockGOF.CreateLayerFadeController(Arg.Any<IAnimationFactory<float>>(), CodeLogicEngine.Constants.SplashScreenFadeTime).Returns(mockController);

            ISpriteBatch mockSpriteBatch = Substitute.For<ISpriteBatch>();
            _mockEngine.GrfxFactory.SpriteBatchFactory.Create(Arg.Any<IGrfxDevice>()).Returns(Substitute.For<ISpriteBatch>(), mockSpriteBatch, Substitute.For<ISpriteBatch>());

            ILayer mockLogoBGLayer = Substitute.For<ILayer>();
            mockLogoBGLayer.MainSpriteBatch.Returns(mockSpriteBatch);
            _mockGOF.CreateGenericLayer(_mockEngine, mockSpriteBatch, SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp).Returns(mockLogoBGLayer);

            IGenericButton mockButton = Substitute.For<IGenericButton>();
            _mockUIObjectFactory.CreateGenericButton(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>()).Returns(mockButton);

            Construct();
            _sut.Initialize();

            mockController.IsAnimationComplete(FadeMode.FadeIn).Returns(true);

            _sut.Update(new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(3)));

            Received.InOrder(
                () =>
                {
                    mockController.AddLayer(mockLogoBGLayer);
                    mockController.StartAnimation(FadeMode.FadeOut);
                }
            );
        }

        [TestMethod]
        public void UpdateWhenFadeInCompleteAndAlreadyFadingOutShouldNotPerformExpectedCalls()
        {
            ILayerFadeController mockController = Substitute.For<ILayerFadeController>();
            _mockGOF.CreateLayerFadeController(Arg.Any<IAnimationFactory<float>>(), CodeLogicEngine.Constants.SplashScreenFadeTime).Returns(mockController);

            ISpriteBatch mockSpriteBatch = Substitute.For<ISpriteBatch>();
            _mockEngine.GrfxFactory.SpriteBatchFactory.Create(Arg.Any<IGrfxDevice>()).Returns(Substitute.For<ISpriteBatch>(), mockSpriteBatch, Substitute.For<ISpriteBatch>());

            ILayer mockLogoBGLayer = Substitute.For<ILayer>();
            mockLogoBGLayer.MainSpriteBatch.Returns(mockSpriteBatch);
            _mockGOF.CreateGenericLayer(_mockEngine, mockSpriteBatch, SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp).Returns(mockLogoBGLayer);

            IGenericButton mockButton = Substitute.For<IGenericButton>();
            _mockUIObjectFactory.CreateGenericButton(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>()).Returns(mockButton);

            Construct();
            _sut.Initialize();

            mockController.IsAnimationComplete(FadeMode.FadeIn).Returns(true);

            _sut.Update(new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(3)));

            mockController.ClearReceivedCalls();

            _sut.Update(new GameTime());

            mockController.Received(0).StartAnimation(Arg.Any<FadeMode>());
        }

        [TestMethod]
        public void UpdateWhenFadeOutNotCompleteShouldNotSetNextScene()
        {
            Construct();

            _sut.Update(new GameTime());

            Assert.IsNull(_sut.NextScene);
        }

        [TestMethod]
        public void UpdateWhenFadeOutNotCompleteShouldNotSetSceneCompleteTrue()
        {
            Construct();

            _sut.Update(new GameTime());

            Assert.IsFalse(_sut.SceneComplete);
        }

        [TestMethod]
        public void UpdateWhenFadeOutCompleteShouldSetNextSceneToMainMenuScene()
        {
            IScene mockScene = Substitute.For<IScene>();
            Dictionary<string, IScene> scenes = new Dictionary<string, IScene>();
            scenes.Add("MainMenu", mockScene);
            _mockEngine.Scenes.Returns(scenes);

            ILayerFadeController mockController = Substitute.For<ILayerFadeController>();
            _mockGOF.CreateLayerFadeController(Arg.Any<IAnimationFactory<float>>(), CodeLogicEngine.Constants.SplashScreenFadeTime).Returns(mockController);

            mockController.IsAnimationComplete(FadeMode.FadeOut).Returns(true);

            Construct();

            _sut.Update(new GameTime());

            Assert.AreSame(mockScene, _sut.NextScene);
        }

        [TestMethod]
        public void UpdateWhenFadeOutCompleteShouldSetSceneCompleteTrue()
        {
            IScene mockScene = Substitute.For<IScene>();
            Dictionary<string, IScene> scenes = new Dictionary<string, IScene>();
            scenes.Add("MainMenu", mockScene);
            _mockEngine.Scenes.Returns(scenes);

            ILayerFadeController mockController = Substitute.For<ILayerFadeController>();
            _mockGOF.CreateLayerFadeController(Arg.Any<IAnimationFactory<float>>(), CodeLogicEngine.Constants.SplashScreenFadeTime).Returns(mockController);

            mockController.IsAnimationComplete(FadeMode.FadeOut).Returns(true);

            Construct();

            _sut.Update(new GameTime());

            Assert.IsTrue(_sut.SceneComplete);
        }
        #endregion
    }
}
