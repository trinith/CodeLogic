using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArbitraryPixel.CodeLogic.Common.Scenes;
using ArbitraryPixel.Platform2D.Engine;
using ArbitraryPixel.CodeLogic.Common;
using ArbitraryPixel.Platform2D.Scene;
using NSubstitute;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Common.Drawing;
using ArbitraryPixel.CodeLogic.Common.Model;

namespace ArbitraryPixel.CodeLogic.Common_Tests.Scenes
{
    [TestClass]
    public class PanSceneTransition_Tests
    {
        private IEngine _mockEngine;
        private IPanSceneTransitionModel _mockModel;
        private ISpriteBatch _mockSpriteBatch;

        private PanSceneTransition _sut;

        [TestInitialize]
        public void Initialize()
        {
            _mockEngine = Substitute.For<IEngine>();

            _mockEngine.ScreenManager.Screen.Returns(new Point(1000, 750));

            _mockSpriteBatch = Substitute.For<ISpriteBatch>();
            _mockModel = Substitute.For<IPanSceneTransitionModel>();

            _sut = new PanSceneTransition(_mockEngine, _mockModel, _mockSpriteBatch);
        }

        #region Static Create Tests
        [TestMethod]
        public void CreateShouldCreateExpectedRenderTargets()
        {
            GameObjectFactory mockGOF = Substitute.For<GameObjectFactory>();
            GameObjectFactory.SetInstance(mockGOF);

            IScene mockScene = Substitute.For<IScene>();

            PanSceneTransition.Create(_mockEngine, mockScene, mockScene, PanSceneTransitionMode.PanLeft, 0.5f);

            _mockEngine.GrfxFactory.RenderTargetFactory.Received(2).Create(Arg.Any<IGrfxDevice>(), 1000, 750, RenderTargetUsage.DiscardContents);
        }

        [TestMethod]
        public void CreateShouldCreateSpriteBatch()
        {
            GameObjectFactory mockGOF = Substitute.For<GameObjectFactory>();
            GameObjectFactory.SetInstance(mockGOF);

            IScene mockScene = Substitute.For<IScene>();

            PanSceneTransition.Create(_mockEngine, mockScene, mockScene, PanSceneTransitionMode.PanLeft, 0.5f);

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

            PanSceneTransition.Create(_mockEngine, mockScene, mockScene, PanSceneTransitionMode.PanLeft, 0.12345f);

            mockGOF.Received(1).CreatePanSceneTransitionModel(mockScene, mockScene, mockTarget, mockTarget, PanSceneTransitionMode.PanLeft, 0.12345f, new SizeF(1000, 750));
        }
        #endregion

        #region Constructor Tests
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullParameterShouldThrowExpectedException_SpriteBatch()
        {
            _sut = new PanSceneTransition(_mockEngine, _mockModel, null);
        }
        #endregion

        #region Draw Tests
        [TestMethod]
        public void DrawShouldPerformExpectedSequence()
        {
            IRenderTarget2D _mockStartTarget = Substitute.For<IRenderTarget2D>();
            IRenderTarget2D _mockEndTarget = Substitute.For<IRenderTarget2D>();

            _mockModel.StartAnchor.Returns(new Vector2(11, 22));
            _mockModel.EndAnchor.Returns(new Vector2(33, 44));
            _mockModel.StartTarget.Returns(_mockStartTarget);
            _mockModel.EndTarget.Returns(_mockEndTarget);
            _sut.Draw(new GameTime());

            Received.InOrder(
                () =>
                {
                    _mockSpriteBatch.Begin();
                    _mockSpriteBatch.Draw(_mockStartTarget, new Vector2(11, 22), Color.White);
                    _mockSpriteBatch.Draw(_mockEndTarget, new Vector2(33, 44), Color.White);
                    _mockSpriteBatch.End();
                }
            );
        }
        #endregion
    }
}
