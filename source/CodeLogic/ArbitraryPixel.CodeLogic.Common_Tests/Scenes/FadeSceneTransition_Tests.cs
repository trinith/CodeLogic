using System;
using ArbitraryPixel.CodeLogic.Common;
using ArbitraryPixel.CodeLogic.Common.Model;
using ArbitraryPixel.CodeLogic.Common.Scenes;
using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Platform2D.Engine;
using ArbitraryPixel.Platform2D.Scene;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NSubstitute;

namespace ArbitraryPixel.CodeLogic.Common_Tests.Scenes
{
    [TestClass]
    public class FadeSceneTransition_Tests
    {
        private FadeSceneTransition _sut;
        private IEngine _mockEngine;
        private IFadeSceneTransitionModel _mockModel;
        private ISpriteBatch _mockSpriteBatch;

        private ITexture2D _mockPixel;
        private IRenderTarget2D _mockTargetA, _mockTargetB;

        [TestInitialize]
        public void Initialize()
        {
            _mockEngine = Substitute.For<IEngine>();
            _mockModel = Substitute.For<IFadeSceneTransitionModel>();
            _mockSpriteBatch = Substitute.For<ISpriteBatch>();

            _mockEngine.ScreenManager.Screen.Returns(new Point(500, 300));

            _mockPixel = Substitute.For<ITexture2D>();
            _mockEngine.AssetBank.Get<ITexture2D>("Pixel").Returns(_mockPixel);

            _mockTargetA = Substitute.For<IRenderTarget2D>();
            _mockTargetB = Substitute.For<IRenderTarget2D>();
            _mockModel.StartTarget.Returns(_mockTargetA);
            _mockModel.EndTarget.Returns(_mockTargetB);
        }

        private void Construct()
        {
            _sut = new FadeSceneTransition(_mockEngine, _mockModel, _mockSpriteBatch);
        }

        #region Static Create Tests
        [TestMethod]
        public void CreateShouldCreateExpectedRenderTargets()
        {
            GameObjectFactory mockGOF = Substitute.For<GameObjectFactory>();
            GameObjectFactory.SetInstance(mockGOF);

            IScene mockScene = Substitute.For<IScene>();

            FadeSceneTransition.Create(_mockEngine, mockScene, mockScene, FadeSceneTransitionMode.OutIn, 0.5f);

            _mockEngine.GrfxFactory.RenderTargetFactory.Received(2).Create(Arg.Any<IGrfxDevice>(), 500, 300, RenderTargetUsage.DiscardContents);
        }

        [TestMethod]
        public void CreateShouldCreateSpriteBatch()
        {
            GameObjectFactory mockGOF = Substitute.For<GameObjectFactory>();
            GameObjectFactory.SetInstance(mockGOF);

            IScene mockScene = Substitute.For<IScene>();

            FadeSceneTransition.Create(_mockEngine, mockScene, mockScene, FadeSceneTransitionMode.OutIn, 0.5f);

            _mockEngine.GrfxFactory.SpriteBatchFactory.Received(1).Create(Arg.Any<IGrfxDevice>());
        }

        [TestMethod]
        public void CreateShouldCreateFadeSceneTransitionModel()
        {
            GameObjectFactory mockGOF = Substitute.For<GameObjectFactory>();
            GameObjectFactory.SetInstance(mockGOF);

            IRenderTarget2D mockTarget = Substitute.For<IRenderTarget2D>();
            _mockEngine.GrfxFactory.RenderTargetFactory.Create(Arg.Any<IGrfxDevice>(), Arg.Any<int>(), Arg.Any<int>(), Arg.Any<RenderTargetUsage>()).Returns(mockTarget);

            IScene mockScene = Substitute.For<IScene>();

            FadeSceneTransition.Create(_mockEngine, mockScene, mockScene, FadeSceneTransitionMode.OutIn, 0.12345f);

            mockGOF.Received(1).CreateFadeSceneTransitionModel(mockScene, mockScene, mockTarget, mockTarget, FadeSceneTransitionMode.OutIn, 0.12345f);
        }
        #endregion

        #region Constructor Tests
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullParameterShouldThrowException_SpriteBatch()
        {
            _sut = new FadeSceneTransition(_mockEngine, _mockModel, null);
        }
        #endregion

        #region Draw Tests
        [TestMethod]
        public void DrawShouldMakeExpectedCalls_TestA()
        {
            _mockModel.StartSceneOpacity.Returns(0.25f);
            _mockModel.EndSceneOpacity.Returns(0.75f);
            _mockModel.Background.Returns(Color.Pink);

            Construct();

            _sut.Draw(new GameTime());

            Received.InOrder(
                () =>
                {
                    _mockSpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);
                    _mockSpriteBatch.Draw(_mockPixel, new Rectangle(0, 0, 500, 300), Color.Pink);
                    _mockSpriteBatch.Draw(_mockTargetA, Vector2.Zero, new Color(Color.White, 64));
                    _mockSpriteBatch.Draw(_mockTargetB, Vector2.Zero, new Color(Color.White, 192));
                    _mockSpriteBatch.End();
                }
            );
        }

        [TestMethod]
        public void DrawShouldMakeExpectedCalls_TestB()
        {
            _mockModel.StartSceneOpacity.Returns(0.33f);
            _mockModel.EndSceneOpacity.Returns(0.66f);
            _mockModel.Background.Returns(Color.Purple);

            Construct();

            _sut.Draw(new GameTime());

            Received.InOrder(
                () =>
                {
                    _mockSpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);
                    _mockSpriteBatch.Draw(_mockPixel, new Rectangle(0, 0, 500, 300), Color.Purple);
                    _mockSpriteBatch.Draw(_mockTargetA, Vector2.Zero, new Color(Color.White, 84));
                    _mockSpriteBatch.Draw(_mockTargetB, Vector2.Zero, new Color(Color.White, 168));
                    _mockSpriteBatch.End();
                }
            );
        }
        #endregion
    }
}
