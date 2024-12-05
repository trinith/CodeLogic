using System;
using ArbitraryPixel.CodeLogic.Common.Credits;
using ArbitraryPixel.Common.Drawing;
using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Platform2D.Engine;
using ArbitraryPixel.Platform2D.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;
using NSubstitute;

namespace ArbitraryPixel.CodeLogic.Common_Tests.Credits
{
    [TestClass]
    public class CreditLineItem_Tests
    {
        private CreditLineItem _sut;
        private IEngine _mockEngine;
        private ISpriteBatch _mockSpriteBatch;
        private ITextObject _mockTextObject;

        private RectangleF _bounds = new RectangleF(200, 100, 400, 300);

        [TestInitialize]
        public void Initialize()
        {
            _mockEngine = Substitute.For<IEngine>();
            _mockSpriteBatch = Substitute.For<ISpriteBatch>();
            _mockTextObject = Substitute.For<ITextObject>();
        }

        private void Construct()
        {
            _sut = new CreditLineItem(_mockEngine, _bounds, _mockSpriteBatch, _mockTextObject);
        }

        #region Constructor Tests
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullParameterShouldThrowException_SpriteBatch()
        {
            _sut = new CreditLineItem(_mockEngine, _bounds, null, _mockTextObject);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullParameterShouldThrowException_TextObject()
        {
            _sut = new CreditLineItem(_mockEngine, _bounds, _mockSpriteBatch, null);
        }
        #endregion

        #region Update Tests
        [TestMethod]
        public void UpdateShouldMoveTextObjectToExpectedPosition()
        {
            _mockTextObject.Location.Returns(new Vector2(100, _bounds.Bottom));

            Construct();

            GameTime gameTime = new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(0.25));
            _sut.Update(gameTime);

            _mockTextObject.Received(1).Location = new Vector2(100, _bounds.Bottom + (-25 * 0.25f));
        }

        [TestMethod]
        public void UpdateWithObjectBelowBoundsTopShouldStayAlive()
        {
            _mockTextObject.Location.Returns(new Vector2(100, _bounds.Bottom));

            Construct();

            GameTime gameTime = new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(0.25));
            _sut.Update(gameTime);

            Assert.IsTrue(_sut.Alive);
        }

        [TestMethod]
        public void UpdateWithObjectAboveBoundsTopShouldSetAliveToFalse()
        {
            _mockTextObject.TextDefinition.Font.LineSpacing.Returns(20);
            _mockTextObject.Location.Returns(new Vector2(100, _bounds.Top - 21));

            Construct();

            _sut.Update(new GameTime());

            Assert.IsFalse(_sut.Alive);
        }
        #endregion

        #region Draw Tests
        [TestMethod]
        public void DrawWithTextObjectPositionAboveBottomShouldCallSpriteBatchDrawString()
        {
            ISpriteFont mockFont = Substitute.For<ISpriteFont>();
            _mockTextObject.TextDefinition.Font.Returns(mockFont);
            _mockTextObject.TextDefinition.Colour.Returns(Color.Pink);
            _mockTextObject.Location.Returns(new Vector2(100, _bounds.Bottom));
            _mockTextObject.CurrentText.Returns("Blah");

            Construct();

            _sut.Draw(new GameTime());

            _mockSpriteBatch.Received(1).DrawString(mockFont, "Blah", new Vector2(100, _bounds.Bottom), Color.Pink);
        }

        [TestMethod]
        public void DrawWithTextBelowBoundsShouldNotCallSpriteBatchDrawString()
        {
            ISpriteFont mockFont = Substitute.For<ISpriteFont>();
            _mockTextObject.TextDefinition.Font.Returns(mockFont);
            _mockTextObject.TextDefinition.Colour.Returns(Color.Pink);
            _mockTextObject.Location.Returns(new Vector2(100, _bounds.Bottom + 1));
            _mockTextObject.CurrentText.Returns("Blah");

            Construct();

            _sut.Draw(new GameTime());

            _mockSpriteBatch.Received(0).DrawString(Arg.Any<ISpriteFont>(), Arg.Any<string>(), Arg.Any<Vector2>(), Arg.Any<Color>());
        }
        #endregion
    }
}
