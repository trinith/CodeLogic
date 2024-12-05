using System;
using ArbitraryPixel.CodeLogic.Common;
using ArbitraryPixel.CodeLogic.Common.Entities;
using ArbitraryPixel.Common.Drawing;
using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Common.Input;
using ArbitraryPixel.Platform2D.Engine;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;
using NSubstitute;

namespace ArbitraryPixel.CodeLogic.Common_Tests.Entities
{
    [TestClass]
    public class ChapterButton_Tests
    {
        private ChapterButton _sut;

        private IEngine _mockEngine;
        private ISpriteBatch _mockSpriteBatch;
        private ISpriteFont _mockTitleFont;
        private ISpriteFont _mockDescriptionFont;

        private RectangleF _bounds = new RectangleF(200, 100, 400, 300);

        private ITexture2D _mockPixel;

        [TestInitialize]
        public void Initialize()
        {
            _mockEngine = Substitute.For<IEngine>();
            _mockSpriteBatch = Substitute.For<ISpriteBatch>();
            _mockTitleFont = Substitute.For<ISpriteFont>();
            _mockDescriptionFont = Substitute.For<ISpriteFont>();

            _mockPixel = Substitute.For<ITexture2D>();
            _mockEngine.AssetBank.Get<ITexture2D>("Pixel").Returns(_mockPixel);
        }

        private void Construct()
        {
            _sut = new ChapterButton(_mockEngine, _bounds, _mockSpriteBatch, _mockTitleFont, _mockDescriptionFont);

            _sut.PressedColour = Color.Olive;
            _sut.NormalColour = Color.Pink;
            _sut.BackColour = Color.Lavender;
        }

        #region Construct Tests
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullParameterShouldThrowExpectedException_DescriptionFont()
        {
            _sut = new ChapterButton(_mockEngine, _bounds, _mockSpriteBatch, _mockTitleFont, null);
        }
        #endregion

        #region Property Tests
        [TestMethod]
        public void PropertyShouldReturnSetValue_Description()
        {
            Construct();

            _sut.Description = "asdf";

            Assert.AreEqual<string>("asdf", _sut.Description);
        }
        #endregion

        #region Draw Tests
        [TestMethod]
        public void DrawShouldDrawExpectedBackground_NotPressed()
        {
            Rectangle expectedBounds = (Rectangle)_bounds;

            Construct();

            _sut.Draw(new GameTime());

            _mockSpriteBatch.Received(1).Draw(_mockPixel, expectedBounds, Color.Lavender);
        }

        [TestMethod]
        public void DrawShouldDrawExpectedBackground_Pressed()
        {
            _mockEngine.InputManager.IsActive.Returns(true);
            _mockEngine.InputManager.ShouldConsumeInput(Arg.Any<object>()).Returns(true);
            _mockEngine.InputManager.GetSurfaceState().Returns(new SurfaceState(_bounds.Centre, true));
            _mockEngine.ScreenManager.PointToWorld(Arg.Any<Vector2>()).Returns(_bounds.Centre);

            Rectangle expectedBounds = (Rectangle)_bounds;

            Construct();
            _sut.Update(new GameTime());

            _sut.Draw(new GameTime());

            _mockSpriteBatch.Received(1).Draw(_mockPixel, expectedBounds, Color.Pink);
        }

        [TestMethod]
        public void DrawShouldDrawExpectedTextForTitle_NotPressed()
        {
            _mockTitleFont.LineSpacing.Returns(20);
            _mockDescriptionFont.LineSpacing.Returns(10);

            Rectangle expectedBounds = (Rectangle)_bounds;
            expectedBounds.Inflate(-CodeLogicEngine.Constants.TextWindowPadding.Width - 1, -CodeLogicEngine.Constants.TextWindowPadding.Height - 1);

            Vector2 expectedLocation = new Vector2(
                expectedBounds.Left,
                expectedBounds.Top + (expectedBounds.Height / 2f) - (20f + 10f) / 2f
            );

            Construct();
            _sut.Text = "TitleText";

            _sut.Draw(new GameTime());

            _mockSpriteBatch.Received(1).DrawString(_mockTitleFont, "TitleText", expectedLocation, Color.Pink);
        }

        [TestMethod]
        public void DrawShouldDrawExpectedTextForTitle_Pressed()
        {
            _mockEngine.InputManager.IsActive.Returns(true);
            _mockEngine.InputManager.ShouldConsumeInput(Arg.Any<object>()).Returns(true);
            _mockEngine.InputManager.GetSurfaceState().Returns(new SurfaceState(_bounds.Centre, true));
            _mockEngine.ScreenManager.PointToWorld(Arg.Any<Vector2>()).Returns(_bounds.Centre);

            _mockTitleFont.LineSpacing.Returns(20);
            _mockDescriptionFont.LineSpacing.Returns(10);

            Rectangle expectedBounds = (Rectangle)_bounds;
            expectedBounds.Inflate(-CodeLogicEngine.Constants.TextWindowPadding.Width - 1, -CodeLogicEngine.Constants.TextWindowPadding.Height - 1);

            Vector2 expectedLocation = new Vector2(
                expectedBounds.Left,
                expectedBounds.Top + (expectedBounds.Height / 2f) - (20f + 10f) / 2f
            );

            Construct();
            _sut.Text = "TitleText";
            _sut.Update(new GameTime());

            _sut.Draw(new GameTime());

            _mockSpriteBatch.Received(1).DrawString(_mockTitleFont, "TitleText", expectedLocation, Color.Olive);
        }

        [TestMethod]
        public void DrawShouldDrawExpectedTextForDescription_NotPressed()
        {
            _mockTitleFont.LineSpacing.Returns(20);
            _mockDescriptionFont.LineSpacing.Returns(10);

            Rectangle expectedBounds = (Rectangle)_bounds;
            expectedBounds.Inflate(-CodeLogicEngine.Constants.TextWindowPadding.Width - 1, -CodeLogicEngine.Constants.TextWindowPadding.Height - 1);

            Vector2 expectedLocation = new Vector2(
                expectedBounds.Left,
                expectedBounds.Top + (expectedBounds.Height / 2f) - (20f + 10f) / 2f + 20f
            );

            Construct();
            _sut.Description = "DescriptionText";

            _sut.Draw(new GameTime());

            _mockSpriteBatch.Received(1).DrawString(_mockDescriptionFont, "DescriptionText", expectedLocation, Color.Pink);
        }

        [TestMethod]
        public void DrawShouldDrawExpectedTextForDescription_Pressed()
        {
            _mockEngine.InputManager.IsActive.Returns(true);
            _mockEngine.InputManager.ShouldConsumeInput(Arg.Any<object>()).Returns(true);
            _mockEngine.InputManager.GetSurfaceState().Returns(new SurfaceState(_bounds.Centre, true));
            _mockEngine.ScreenManager.PointToWorld(Arg.Any<Vector2>()).Returns(_bounds.Centre);

            _mockTitleFont.LineSpacing.Returns(20);
            _mockDescriptionFont.LineSpacing.Returns(10);

            Rectangle expectedBounds = (Rectangle)_bounds;
            expectedBounds.Inflate(-CodeLogicEngine.Constants.TextWindowPadding.Width - 1, -CodeLogicEngine.Constants.TextWindowPadding.Height - 1);

            Vector2 expectedLocation = new Vector2(
                expectedBounds.Left,
                expectedBounds.Top + (expectedBounds.Height / 2f) - (20f + 10f) / 2f + 20f
            );

            Construct();
            _sut.Description = "DescriptionText";
            _sut.Update(new GameTime());

            _sut.Draw(new GameTime());

            _mockSpriteBatch.Received(1).DrawString(_mockDescriptionFont, "DescriptionText", expectedLocation, Color.Olive);
        }
        #endregion

    }
}
