using ArbitraryPixel.CodeLogic.Common.Layers;
using ArbitraryPixel.CodeLogic.Common.Model;
using ArbitraryPixel.Common;
using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Platform2D.Engine;
using ArbitraryPixel.Platform2D.Scene;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;
using NSubstitute;

namespace ArbitraryPixel.CodeLogic.Common_Tests
{
    [TestClass]
    public class CodeLogicEngine_UpdateAndDraw_Tests : CodeLogicEngineTestBase
    {
        private IScene _mockScene;
        private IBuildInfoOverlayLayer _mockBuildInfoOverlayLayer;

        protected override void OnInitialized()
        {
            _mockScene = Substitute.For<IScene>();
            _mockBuildInfoOverlayLayer = Substitute.For<IBuildInfoOverlayLayer>();

            // Use the factory creator for whatever we set the initial scene to :)
            _mockGameObjectFactory.CreateGameStartupScene(Arg.Any<IEngine>(), Arg.Any<IScene>()).Returns(_mockScene);

            _mockGameObjectFactory.CreateBuildInfoOverlayLayer(Arg.Any<IEngine>(), Arg.Any<ISpriteBatch>(), Arg.Any<IBuildInfoOverlayLayerModel>(), Arg.Any<IRandom>()).Returns(_mockBuildInfoOverlayLayer);

            _sut.LoadContent();
        }

        #region Update Tests
        [TestMethod]
        public void UpdateWithRenderBuildInfoOverlayFlagTrueShouldCallOverlayLayerUpdate()
        {
            GameTime expectedGT = new GameTime();
            _sut.RenderBuildInfoOverlay = true;

            _sut.Update(expectedGT);

            _mockBuildInfoOverlayLayer.Received(1).Update(expectedGT);
        }
        #endregion

        #region Draw Tests
        [TestMethod]
        public void DrawWithOverlayEnabledShouldDrawOverlayLayer()
        {
            GameTime expectedGT = new GameTime();
            _sut.RenderBuildInfoOverlay = true;

            _sut.Draw(expectedGT);

            _mockBuildInfoOverlayLayer.Received(1).Draw(expectedGT);
        }

        [TestMethod]
        public void DrawWithOverlayDisabledShouldNotDrawOverlayLayer()
        {
            _sut.RenderBuildInfoOverlay = false;

            _sut.Draw(new GameTime());

            _mockBuildInfoOverlayLayer.Received(0).Draw(Arg.Any<GameTime>());
        }
        #endregion
    }
}
