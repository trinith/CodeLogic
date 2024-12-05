using ArbitraryPixel.CodeLogic.Common.Entities;
using ArbitraryPixel.Common.Drawing;
using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Platform2D.Engine;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;
using NSubstitute;
using System;

namespace ArbitraryPixel.CodeLogic.Common_Tests.Entities
{
    [TestClass]
    public class TextureEntity_Tests
    {
        public TextureEntity _sut;
        private Color _mask;
        private ITexture2D _mockTexture;
        private ISpriteBatch _mockSpriteBatch;
        private RectangleF _bounds;
        private IEngine _mockEngine;

        [TestInitialize]
        public void Initialize()
        {
            _mockEngine = Substitute.For<IEngine>();
            _bounds = new RectangleF(200, 100, 400, 300);
            _mockSpriteBatch = Substitute.For<ISpriteBatch>();
            _mockTexture = Substitute.For<ITexture2D>();
            _mask = Color.Pink;
        }

        private void Construct(Rectangle? sourceRectangle = null)
        {
            _sut = new TextureEntity(_mockEngine, _bounds, _mockSpriteBatch, _mockTexture, _mask, sourceRectangle);
        }

        #region Constructor Tests
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullParameterShouldThrowException_SpriteBatch()
        {
            _sut = new TextureEntity(_mockEngine, _bounds, null, _mockTexture, _mask);
        }
        #endregion

        #region Property Tests
        [TestMethod]
        public void TextureShouldReturnConstructorParameter()
        {
            Construct();
            Assert.AreSame(_mockTexture, _sut.Texture);
        }

        [TestMethod]
        public void MaskShouldReturnConstructorParameter()
        {
            Construct();
            Assert.AreEqual<Color>(Color.Pink, _sut.Mask);
        }

        [TestMethod]
        public void SourceRectangleShouldDefaultNull()
        {
            Construct();
            Assert.IsNull(_sut.SourceRectangle);
        }

        [TestMethod]
        public void SourceRectangleShouldReturnConstructorParameter()
        {
            Construct(new Rectangle(100, 200, 300, 400));

            Assert.AreEqual<Rectangle?>(new Rectangle(100, 200, 300, 400), _sut.SourceRectangle);
        }
        #endregion
    }
}
