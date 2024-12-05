using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArbitraryPixel.CodeLogic.Common.Scenes;
using ArbitraryPixel.Platform2D.Assets;
using ArbitraryPixel.CodeLogic.Common;
using ArbitraryPixel.Platform2D.Engine;
using NSubstitute;
using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Platform2D.Layer;
using ArbitraryPixel.CodeLogic.Common.Layers;
using ArbitraryPixel.Common.Drawing;
using ArbitraryPixel.CodeLogic.Common.Entities;
using ArbitraryPixel.Platform2D.Text;

namespace ArbitraryPixel.CodeLogic.Common_Tests.Scenes
{
    [TestClass]
    public class NoAdMessageScene_AssetLoad_Tests
    {
        private NoAdMessageScene _sut;
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

            _mockGameObjectFactory.CreateGenericLayer(Arg.Any<IEngine>(), Arg.Any<ISpriteBatch>()).Returns(Substitute.For<ILayer>());
            _mockGameObjectFactory.CreateProgressLayer(Arg.Any<IEngine>(), Arg.Any<ISpriteBatch>(), Arg.Any<ISpriteFont>(), Arg.Any<string>()).Returns(Substitute.For<IProgressLayer>());
            _mockGameObjectFactory.CreateSimpleButton(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>(), Arg.Any<ISpriteFont>()).Returns(Substitute.For<ISimpleButton>());
            _mockGameObjectFactory.CreateTextObjectBuilder(Arg.Any<ITextFormatProcessor>(), Arg.Any<ITextObjectFactory>()).Returns(Substitute.For<ITextObjectBuilder>());

            _sut = new NoAdMessageScene(_mockEngine);
        }

        #region Fonts
        [TestMethod]
        public void InitializeShouldLoadFont_AdLoadNormalFont()
        {
            string target = "AdLoadNormalFont";
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
        public void InitializeShouldLoadFont_AdLoadTitleFont_Exists()
        {
            string target = "MainMenuContentFont";
            ISpriteFont mockFont = Substitute.For<ISpriteFont>();
            _mockEngine.GrfxFactory.SpriteFontFactory.Create(_mockEngine.Content, "Fonts\\" + target).Returns(mockFont);
            _mockEngine.AssetBank.Exists<ISpriteFont>(target).Returns(true);

            _sut.Initialize();

            Received.InOrder(
                () =>
                {
                    _mockEngine.AssetBank.Exists<ISpriteFont>(target);
                }
            );

            _mockEngine.GrfxFactory.SpriteFontFactory.Received(0).Create(_mockEngine.Content, "Fonts\\" + target);
            _mockEngine.AssetBank.Received(0).Put<ISpriteFont>(target, mockFont);
        }

        [TestMethod]
        public void InitializeShouldLoadFont_AdLoadTitleFont_NoExists()
        {
            string target = "MainMenuContentFont";
            ISpriteFont mockFont = Substitute.For<ISpriteFont>();
            _mockEngine.GrfxFactory.SpriteFontFactory.Create(_mockEngine.Content, "Fonts\\" + target).Returns(mockFont);
            _mockEngine.AssetBank.Exists<ISpriteFont>(target).Returns(false);

            _sut.Initialize();

            Received.InOrder(
                () =>
                {
                    _mockEngine.AssetBank.Exists<ISpriteFont>(target);
                    _mockEngine.GrfxFactory.SpriteFontFactory.Create(_mockEngine.Content, "Fonts\\" + target);
                    _mockEngine.AssetBank.Put<ISpriteFont>(target, mockFont);
                }
            );
        }
        #endregion
    }
}
