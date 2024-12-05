using ArbitraryPixel.CodeLogic.Common;
using ArbitraryPixel.CodeLogic.Common.Scenes;
using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Platform2D.Assets;
using ArbitraryPixel.Platform2D.Engine;
using ArbitraryPixel.Platform2D.UI.Factory;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace ArbitraryPixel.CodeLogic.Common_Tests.Scenes
{
    [TestClass]
    public class SplashScreenScene_AssetLoad_Tests
    {
        private SplashScreenScene _sut;
        private IEngine _mockEngine;

        [TestInitialize]
        public void Initialize()
        {
            _mockEngine = Substitute.For<IEngine>();

            IAssetLoader mockLoader = Substitute.For<IAssetLoader>();
            AssetLoadMethod loadMethod = null;
            _mockEngine.AssetBank.CreateLoader().Returns(mockLoader);
            mockLoader.When(x => x.RegisterLoadMethod(Arg.Any<AssetLoadMethod>())).Do(x => loadMethod = x[0] as AssetLoadMethod);
            mockLoader.When(x => x.LoadBank()).Do(x => loadMethod(_mockEngine.AssetBank));

            GameObjectFactory mockGameObjectFactory = Substitute.For<GameObjectFactory>();
            GameObjectFactory.SetInstance(mockGameObjectFactory);

            _sut = new SplashScreenScene(_mockEngine, Substitute.For<IUIObjectFactory>());
        }

        #region Textures
        [TestMethod]
        public void InitializeShouldLoadTexture_APLogo()
        {
            string target = "APLogo";
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
    }
}
