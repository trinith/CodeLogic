using ArbitraryPixel.CodeLogic.Common;
using ArbitraryPixel.CodeLogic.Common.UI;
using ArbitraryPixel.Common.Drawing;
using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Platform2D.Engine;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;
using NSubstitute;
using System;

namespace ArbitraryPixel.CodeLogic.Common_Tests.UI
{
    [TestClass]
    public class MenuTitleView_Tests
    {
        private MenuTitleView _sut;
        private IEngine _mockEngine;
        private RectangleF _bounds;
        private IMenuItem _mockMenuItem;
        private ISpriteBatch _mockSpriteBatch;
        private ISpriteFont _mockMenuFont;

        private ITexture2D _mockPixel;

        [TestInitialize]
        public void Initialize()
        {
            _mockEngine = Substitute.For<IEngine>();
            _bounds = new RectangleF(200, 100, 400, 300);
            _mockMenuItem = Substitute.For<IMenuItem>();
            _mockSpriteBatch = Substitute.For<ISpriteBatch>();
            _mockMenuFont = Substitute.For<ISpriteFont>();

            _mockEngine.AssetBank.Get<ITexture2D>("Pixel").Returns(_mockPixel = Substitute.For<ITexture2D>());

            _mockMenuItem.Parent.Returns((IMenuItem)null);
            _mockMenuItem.Text.Returns("Test");

            _sut = new MenuTitleView(_mockEngine, _bounds, _mockMenuItem, _mockSpriteBatch, _mockMenuFont);
        }

        #region Constructor Tests
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullParameterShouldThrowException_SpriteBatch()
        {
            _sut = new MenuTitleView(_mockEngine, _bounds, _mockMenuItem, null, _mockMenuFont);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullParameterShouldThrowException_MenuFont()
        {
            _sut = new MenuTitleView(_mockEngine, _bounds, _mockMenuItem, _mockSpriteBatch, null);
        }

        [TestMethod]
        public void ConstructShouldRequestPixelTextureFromBank()
        {
            _mockEngine.AssetBank.Received(1).Get<ITexture2D>("Pixel");
        }

        [TestMethod]
        public void ConstructShouldSetTitleText()
        {
            Assert.AreEqual<string>("Test", _sut.TitleText);
        }
        #endregion

        #region Draw Tests
        [TestMethod]
        public void DrawShouldDrawBackground()
        {
            _sut.Draw(new GameTime());

            _mockSpriteBatch.Received(1).Draw(_mockPixel, _bounds, CodeLogicEngine.Constants.ClrMenuBGHigh);
        }

        [TestMethod]
        public void DrawWithTitleTextShouldCallSpriteBatchDrawString()
        {
            _sut.Draw(new GameTime());

            _mockSpriteBatch.Received(1).DrawString(_mockMenuFont, "Test", _bounds.Location + new Vector2(5), Color.White);
        }

        [TestMethod]
        public void DrawWithNoTitleTextShouldNotCallSpriteBatchDrawString()
        {
            IMenuItem mockItem = Substitute.For<IMenuItem>();
            mockItem.Text.Returns("");
            mockItem.Parent.Returns((IMenuItem)null);
            _sut.ViewOf = mockItem;

            _sut.Draw(new GameTime());

            _mockSpriteBatch.Received(0).DrawString(Arg.Any<ISpriteFont>(), Arg.Any<string>(), Arg.Any<Vector2>(), Arg.Any<Color>());
        }
        #endregion

        #region TitleText Tests
        [TestMethod]
        public void TitleTextShouldUpdateWithNewView()
        {
            IMenuItem mockItem = Substitute.For<IMenuItem>();
            mockItem.Parent.Returns((IMenuItem)null);
            mockItem.Text.Returns("Blah");

            _sut.ViewOf = mockItem;

            Assert.AreEqual<string>("Blah", _sut.TitleText);
        }

        [TestMethod]
        public void TItleTextWithParentedViewShouldGiveExpectedValue()
        {
            IMenuItem mockParent = Substitute.For<IMenuItem>();
            mockParent.Text.Returns("Parent");
            mockParent.Parent.Returns((IMenuItem)null);

            IMenuItem mockChild = Substitute.For<IMenuItem>();
            mockChild.Text.Returns("Child");
            mockChild.Parent.Returns(mockParent);

            _sut.ViewOf = mockChild;

            Assert.AreEqual<string>("Parent > Child", _sut.TitleText);
        }
        #endregion
    }
}
