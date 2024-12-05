using ArbitraryPixel.CodeLogic.Common.Entities;
using ArbitraryPixel.Common.Drawing;
using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Common.Input;
using ArbitraryPixel.Platform2D.Engine;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;
using NSubstitute;
using System;

namespace ArbitraryPixel.CodeLogic.Common_Tests.Entities
{
    [TestClass]
    public class SimpleButton_Tests
    {
        private SimpleButton _sut;
        private ISpriteFont _mockFont;
        private ISpriteBatch _mockSpriteBatch;
        private RectangleF _bounds;
        private IEngine _mockEngine;

        [TestInitialize]
        public void Initialize()
        {
            _mockEngine = Substitute.For<IEngine>();
            _bounds = new RectangleF(200, 100, 400, 300);
            _mockSpriteBatch = Substitute.For<ISpriteBatch>();
            _mockFont = Substitute.For<ISpriteFont>();
        }

        private void Construct()
        {
            _sut = new SimpleButton(_mockEngine, _bounds, _mockSpriteBatch, _mockFont);
        }

        #region Constructor Tests
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullParameterShouldThrowException_SpriteBatch()
        {
            _sut = new SimpleButton(_mockEngine, _bounds, null, _mockFont);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullParameterShouldThrowException_Font()
        {
            _sut = new SimpleButton(_mockEngine, _bounds, _mockSpriteBatch, null);
        }

        [TestMethod]
        public void ConstructShouldRequestPixelTexture()
        {
            Construct();
            _mockEngine.AssetBank.Received(1).Get<ITexture2D>("Pixel");
        }

        [TestMethod]
        public void ConstructShouldSetTextToEmpty()
        {
            Construct();
            Assert.AreEqual<string>("", _sut.Text);
        }
        #endregion

        #region Property Tests
        [TestMethod]
        public void TextShouldReturnSetValue()
        {
            Construct();
            _sut.Text = "Blah";

            Assert.AreEqual<string>("Blah", _sut.Text);
        }

        [TestMethod]
        public void BackColourShouldDefaultToExpectedValue()
        {
            Construct();
            Assert.AreEqual<Color>(new Color(32, 32, 32, 128), _sut.BackColour);
        }

        [TestMethod]
        public void BackColourShouldReturnSetValue()
        {
            Construct();
            _sut.BackColour = Color.Pink;
            Assert.AreEqual<Color>(Color.Pink, _sut.BackColour);
        }

        [TestMethod]
        public void NormalColourShouldDefaultToExpectedValue()
        {
            Construct();
            Assert.AreEqual<Color>(Color.Gray, _sut.NormalColour);
        }

        [TestMethod]
        public void NormalColourShouldReturnSetValue()
        {
            Construct();
            _sut.NormalColour = Color.Pink;
            Assert.AreEqual<Color>(Color.Pink, _sut.NormalColour);
        }

        [TestMethod]
        public void PressedColourShouldDefaultToExpectedValue()
        {
            Construct();
            Assert.AreEqual<Color>(Color.White, _sut.PressedColour);
        }

        [TestMethod]
        public void PressedColourShouldReturnSetValue()
        {
            Construct();
            _sut.PressedColour = Color.Pink;
            Assert.AreEqual<Color>(Color.Pink, _sut.PressedColour);
        }
        #endregion

        #region Draw Tests
        [TestMethod]
        public void DrawWhenNotPressedShouldDrawAsExpected()
        {
            ITexture2D mockPixel = Substitute.For<ITexture2D>();
            _mockEngine.AssetBank.Get<ITexture2D>("Pixel").Returns(mockPixel);
            Construct();

            _sut.Draw(new GameTime());

            Received.InOrder(
                () =>
                {
                    Rectangle bounds = (Rectangle)_bounds;
                    _mockSpriteBatch.Draw(mockPixel, bounds, new Color(32, 32, 32, 128));
                    _mockSpriteBatch.Draw(mockPixel, new Rectangle(bounds.Location, new Point(bounds.Width, 1)), Color.Gray);
                    _mockSpriteBatch.Draw(mockPixel, new Rectangle(new Point(bounds.Left, bounds.Bottom - 1), new Point(bounds.Width, 1)), Color.Gray);
                    _mockSpriteBatch.Draw(mockPixel, new Rectangle(bounds.Location, new Point(1, bounds.Height)), Color.Gray);
                    _mockSpriteBatch.Draw(mockPixel, new Rectangle(new Point(bounds.Right - 1, bounds.Top), new Point(1, bounds.Height)), Color.Gray);
                }
            );
        }

        [TestMethod]
        public void DrawWhenNotPressedShouldUseExpectedColours()
        {
            ITexture2D mockPixel = Substitute.For<ITexture2D>();
            _mockEngine.AssetBank.Get<ITexture2D>("Pixel").Returns(mockPixel);
            Construct();
            _sut.BackColour = Color.Pink;
            _sut.NormalColour = Color.Purple;

            _sut.Draw(new GameTime());

            Received.InOrder(
                () =>
                {
                    Rectangle bounds = (Rectangle)_bounds;
                    _mockSpriteBatch.Draw(mockPixel, bounds, Color.Pink);
                    _mockSpriteBatch.Draw(mockPixel, new Rectangle(bounds.Location, new Point(bounds.Width, 1)), Color.Purple);
                    _mockSpriteBatch.Draw(mockPixel, new Rectangle(new Point(bounds.Left, bounds.Bottom - 1), new Point(bounds.Width, 1)), Color.Purple);
                    _mockSpriteBatch.Draw(mockPixel, new Rectangle(bounds.Location, new Point(1, bounds.Height)), Color.Purple);
                    _mockSpriteBatch.Draw(mockPixel, new Rectangle(new Point(bounds.Right - 1, bounds.Top), new Point(1, bounds.Height)), Color.Purple);
                }
            );
        }

        [TestMethod]
        public void DrawWhenPressedShouldDRawAsExpected()
        {
            ITexture2D mockPixel = Substitute.For<ITexture2D>();
            _mockEngine.AssetBank.Get<ITexture2D>("Pixel").Returns(mockPixel);
            _mockEngine.InputManager.IsActive.Returns(true);
            _mockEngine.InputManager.ShouldConsumeInput(Arg.Any<object>()).Returns(true);
            _mockEngine.InputManager.GetSurfaceState().Returns(new SurfaceState(_bounds.Centre, true));
            _mockEngine.ScreenManager.PointToWorld(Arg.Any<Vector2>()).Returns(_bounds.Centre);
            Construct();

            _sut.Update(new GameTime());
            _sut.Draw(new GameTime());

            Received.InOrder(
                () =>
                {
                    Rectangle bounds = (Rectangle)_bounds;
                    _mockSpriteBatch.Draw(mockPixel, bounds, new Color(32, 32, 32, 128));
                    _mockSpriteBatch.Draw(mockPixel, new Rectangle(bounds.Location, new Point(bounds.Width, 1)), Color.White);
                    _mockSpriteBatch.Draw(mockPixel, new Rectangle(new Point(bounds.Left, bounds.Bottom - 1), new Point(bounds.Width, 1)), Color.White);
                    _mockSpriteBatch.Draw(mockPixel, new Rectangle(bounds.Location, new Point(1, bounds.Height)), Color.White);
                    _mockSpriteBatch.Draw(mockPixel, new Rectangle(new Point(bounds.Right - 1, bounds.Top), new Point(1, bounds.Height)), Color.White);
                }
            );
        }

        [TestMethod]
        public void DrawWhenPressedShouldUseExpectedColours()
        {
            ITexture2D mockPixel = Substitute.For<ITexture2D>();
            _mockEngine.AssetBank.Get<ITexture2D>("Pixel").Returns(mockPixel);
            _mockEngine.InputManager.IsActive.Returns(true);
            _mockEngine.InputManager.ShouldConsumeInput(Arg.Any<object>()).Returns(true);
            _mockEngine.InputManager.GetSurfaceState().Returns(new SurfaceState(_bounds.Centre, true));
            _mockEngine.ScreenManager.PointToWorld(Arg.Any<Vector2>()).Returns(_bounds.Centre);
            Construct();
            _sut.BackColour = Color.Pink;
            _sut.PressedColour = Color.Purple;

            _sut.Update(new GameTime());
            _sut.Draw(new GameTime());

            Received.InOrder(
                () =>
                {
                    Rectangle bounds = (Rectangle)_bounds;
                    _mockSpriteBatch.Draw(mockPixel, bounds, Color.Pink);
                    _mockSpriteBatch.Draw(mockPixel, new Rectangle(bounds.Location, new Point(bounds.Width, 1)), Color.Purple);
                    _mockSpriteBatch.Draw(mockPixel, new Rectangle(new Point(bounds.Left, bounds.Bottom - 1), new Point(bounds.Width, 1)), Color.Purple);
                    _mockSpriteBatch.Draw(mockPixel, new Rectangle(bounds.Location, new Point(1, bounds.Height)), Color.Purple);
                    _mockSpriteBatch.Draw(mockPixel, new Rectangle(new Point(bounds.Right - 1, bounds.Top), new Point(1, bounds.Height)), Color.Purple);
                }
            );
        }

        [TestMethod]
        public void DrawWithNoTextShouldNotCallSpriteBatchDrawString()
        {
            Construct();

            _sut.Draw(new GameTime());

            _mockSpriteBatch.Received(0).DrawString(Arg.Any<ISpriteFont>(), Arg.Any<string>(), Arg.Any<Vector2>(), Arg.Any<Color>());
        }

        [TestMethod]
        public void DrawWithTextShouldCallSpriteBatchDrawString_NotPressed()
        {
            Construct();
            _mockFont.MeasureString(Arg.Any<string>()).Returns(new SizeF(50, 10));
            _sut.Text = "Blah";

            _sut.Draw(new GameTime());

            Vector2 expectedPos = new Vector2(
                (int)(_bounds.Left + _bounds.Width / 2f - 50f / 2f),
                (int)(_bounds.Top + _bounds.Height / 2f - 10f / 2f + 2f)
            );

            _mockSpriteBatch.Received(1).DrawString(_mockFont, "Blah", expectedPos, Color.Gray);
        }

        [TestMethod]
        public void DrawWithTextShouldCallSpriteBatchDrawString_Pressed()
        {
            Construct();
            _mockFont.MeasureString(Arg.Any<string>()).Returns(new SizeF(50, 10));
            _sut.Text = "Blah";

            _mockEngine.InputManager.IsActive.Returns(true);
            _mockEngine.InputManager.ShouldConsumeInput(Arg.Any<object>()).Returns(true);
            _mockEngine.InputManager.GetSurfaceState().Returns(new SurfaceState(_bounds.Centre, true));
            _mockEngine.ScreenManager.PointToWorld(Arg.Any<Vector2>()).Returns(_bounds.Centre);
            _sut.Update(new GameTime());

            _sut.Draw(new GameTime());

            Vector2 expectedPos = new Vector2(
                (int)(_bounds.Left + _bounds.Width / 2f - 50f / 2f),
                (int)(_bounds.Top + _bounds.Height / 2f - 10f / 2f + 2f)
            );

            _mockSpriteBatch.Received(1).DrawString(_mockFont, "Blah", expectedPos, Color.White);
        }

        [TestMethod]
        public void DrawAfterBoundsChangeShouldDrawTextInExpectedPosition()
        {
            Vector2 expectedPos = new Vector2(
                (int)(_bounds.Left + _bounds.Width / 2f - 50f / 2f),
                (int)(_bounds.Top + _bounds.Height / 2f - 10f / 2f + 2f)
            );
            expectedPos += new Vector2(100, 0);

            Construct();
            _mockFont.MeasureString(Arg.Any<string>()).Returns(new SizeF(50, 10));
            _sut.Text = "Blah";
            _sut.Bounds = new RectangleF(
                _sut.Bounds.Location + new Vector2(100, 0),
                _sut.Bounds.Size
            );

            _sut.Draw(new GameTime());

            _mockSpriteBatch.Received(1).DrawString(_mockFont, "Blah", expectedPos, Color.Gray);
        }
        #endregion
    }
}
