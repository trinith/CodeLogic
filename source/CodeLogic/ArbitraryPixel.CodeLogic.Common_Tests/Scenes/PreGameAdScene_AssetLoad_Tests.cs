using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArbitraryPixel.CodeLogic.Common.Scenes;
using ArbitraryPixel.CodeLogic.Common;
using ArbitraryPixel.Platform2D.Engine;
using ArbitraryPixel.Platform2D.Assets;
using ArbitraryPixel.CodeLogic.Common.Model;
using NSubstitute;
using ArbitraryPixel.Common.Graphics;

namespace ArbitraryPixel.CodeLogic.Common_Tests.Scenes
{
    [TestClass]
    public class PreGameAdScene_AssetLoad_Tests
    {
        private PreGameAdScene _sut;
        private IEngine _mockEngine;
        private IAssetLoader _mockAssetLoader;
        private AssetLoadMethod _loadMethod;
        private GameObjectFactory _mockGameObjectFactory;

        [TestInitialize]
        public void Initialize()
        {
            _mockEngine = Substitute.For<IEngine>();

            _mockEngine.AssetBank.CreateLoader().Returns(_mockAssetLoader = Substitute.For<IAssetLoader>());
            _mockAssetLoader.When(x => x.RegisterLoadMethod(Arg.Any<AssetLoadMethod>())).Do(x => _loadMethod = x[0] as AssetLoadMethod);
            _mockAssetLoader.When(x => x.LoadBank()).Do(x => _loadMethod(_mockEngine.AssetBank));

            _mockGameObjectFactory = Substitute.For<GameObjectFactory>();
            GameObjectFactory.SetInstance(_mockGameObjectFactory);

            _sut = new PreGameAdScene(_mockEngine);
        }

        #region Fonts
        [TestMethod]
        public void InitializeShouldLoadFont_AdLoadTitleFont()
        {
            string target = "AdLoadTitleFont";
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
    }
}
