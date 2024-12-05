using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArbitraryPixel.CodeLogic.Common.UI;
using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Common.Drawing;
using ArbitraryPixel.Platform2D.Engine;
using NSubstitute;
using ArbitraryPixel.Platform2D.Text;
using Microsoft.Xna.Framework;

namespace ArbitraryPixel.CodeLogic.Common_Tests.UI
{
    [TestClass]
    public class MenuItemView_Tests
    {
        private MenuItemView _sut;
        private ISpriteFont _mockFont;
        private IMenuItem _mockMenuItem;
        private ISpriteBatch _mockSpriteBatch;
        private RectangleF _bounds;
        private IEngine _mockEngine;

        [TestInitialize]
        public void Initialize()
        {
            _mockEngine = Substitute.For<IEngine>();
            _bounds = new RectangleF(100, 50, 200, 150);
            _mockSpriteBatch = Substitute.For<ISpriteBatch>();
            _mockMenuItem = Substitute.For<IMenuItem>();
            _mockFont = Substitute.For<ISpriteFont>();

            _mockMenuItem.Text.Returns("Test");
            _mockFont.MeasureString(Arg.Any<string>()).Returns(new SizeF(30, 10));

            _sut = new MenuItemView(_mockEngine, _bounds, _mockMenuItem, _mockSpriteBatch, _mockFont, TextLineAlignment.Left);
        }

        #region Constructor Tests
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullParameterShouldThrowException_SpriteBatch()
        {
            _sut = new MenuItemView(_mockEngine, _bounds, _mockMenuItem, null, _mockFont, TextLineAlignment.Left);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullParameterShouldThrowException_SpriteFont()
        {
            _sut = new MenuItemView(_mockEngine, _bounds, _mockMenuItem, _mockSpriteBatch, null, TextLineAlignment.Left);
        }

        [TestMethod]
        public void ConstructShouldCallMeasureStringForItemText()
        {
            _mockFont.Received(1).MeasureString("Test");
        }

        [TestMethod]
        public void ConstructShouldSetLineAlignmentToParameterValue()
        {
            Assert.AreEqual<TextLineAlignment>(TextLineAlignment.Left, _sut.LineAlignment);
        }

        [TestMethod]
        public void ConstructShouldSetIsSelectedFalse()
        {
            Assert.IsFalse(_sut.IsSelected);
        }
        #endregion

        #region Property Tests
        [TestMethod]
        public void IsSelectedShouldReturnSetValue()
        {
            _sut.IsSelected = true;

            Assert.IsTrue(_sut.IsSelected);
        }

        [TestMethod]
        public void LineAlignmentShouldReturnSetValue()
        {
            _sut.LineAlignment = TextLineAlignment.Centre;

            Assert.AreEqual<TextLineAlignment>(TextLineAlignment.Centre, _sut.LineAlignment);
        }
        #endregion

        #region Draw Tests
        [TestMethod]
        public void DrawWithIsSelectedFalseShouldDrawWithExpectedValues()
        {
            _sut.Draw(new GameTime());

            Vector2 expectedPos = new Vector2(_bounds.Left + 10, (int)(_bounds.Top + _bounds.Height / 2f - 5 + 6));
            _mockSpriteBatch.Received(1).DrawString(_mockFont, "Test", expectedPos, Color.Gray);
        }

        [TestMethod]
        public void DrawWithIsSelectedTrueShouldDrawWithExpectedValues()
        {
            _sut.IsSelected = true;

            _sut.Draw(new GameTime());

            Vector2 expectedPos = new Vector2(_bounds.Left + 10, (int)(_bounds.Top + _bounds.Height / 2f - 5 + 6));
            _mockSpriteBatch.Received(1).DrawString(_mockFont, "Test", expectedPos, Color.White);
        }

        [TestMethod]
        public void DrawWithCentreAlignmentShouldDrawWithExpectedValues()
        {
            _sut.LineAlignment = TextLineAlignment.Centre;

            _sut.Draw(new GameTime());

            Vector2 expectedPos = new Vector2(_bounds.Left + _bounds.Width / 2f - 30f / 2f, (int)(_bounds.Top + _bounds.Height / 2f - 5 + 6));
            _mockSpriteBatch.Received(1).DrawString(_mockFont, "Test", expectedPos, Color.Gray);
        }

        [TestMethod]
        public void DrawWithRightAlignmentShouldDrawWithExpectedValues()
        {
            _sut.LineAlignment = TextLineAlignment.Right;

            _sut.Draw(new GameTime());

            Vector2 expectedPos = new Vector2(_bounds.Right - 30f - 10f, (int)(_bounds.Top + _bounds.Height / 2f - 5 + 6));
            _mockSpriteBatch.Received(1).DrawString(_mockFont, "Test", expectedPos, Color.Gray);
        }

        [TestMethod]
        public void DrawWithCentreAlignmentAndDifferentViewShouldDrawWithExpectedValues()
        {
            _sut.LineAlignment = TextLineAlignment.Centre;
            IMenuItem mockOther = Substitute.For<IMenuItem>();
            mockOther.Text.Returns("Other");
            _mockFont.MeasureString("Other").Returns(new SizeF(50, 30));
            _sut.ViewOf = mockOther;

            _sut.Draw(new GameTime());

            Vector2 expectedPos = new Vector2(_bounds.Left + _bounds.Width / 2f - 50f / 2f, (int)(_bounds.Top + _bounds.Height / 2f - 15 + 6));
            _mockSpriteBatch.Received(1).DrawString(_mockFont, "Other", expectedPos, Color.Gray);
        }
        #endregion
    }
}
