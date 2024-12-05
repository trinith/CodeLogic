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
    public class Slider_Tests
    {
        private Slider _sut;
        private IEngine _mockEngine;
        private ISpriteBatch _mockSpriteBatch;

        private RectangleF _bounds;
        private ITexture2D _mockPixel;

        [TestInitialize]
        public void Initialize()
        {
            _mockEngine = Substitute.For<IEngine>();
            _mockSpriteBatch = Substitute.For<ISpriteBatch>();

            _mockPixel = Substitute.For<ITexture2D>();
            _mockEngine.AssetBank.Get<ITexture2D>("Pixel").Returns(_mockPixel);

            _bounds = new RectangleF(200, 100, 500, 50);
        }

        private void Construct()
        {
            _sut = new Slider(_mockEngine, _bounds, _mockSpriteBatch);
        }

        #region Constructor Tests
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullSpriteBatchShouldThrowExpectedException()
        {
            _sut = new Slider(_mockEngine, _bounds, null);
        }
        #endregion

        #region Property Tests
        [TestMethod]
        public void PropertyShouldDefaultToExpectedValue_HotColour()
        {
            Construct();

            Assert.AreEqual<Color>(Color.White, _sut.HotColour);
        }

        [TestMethod]
        public void PropertyShouldReturnSetValue_HotColour()
        {
            Construct();

            _sut.HotColour = Color.Pink;

            Assert.AreEqual<Color>(Color.Pink, _sut.HotColour);
        }
        #endregion

        #region Update Tests
        [TestMethod]
        public void UpdateWhileTouchedShouldRetrieveSurfaceState()
        {
            // Expect two calls, one by base class, one by Slider.

            _mockEngine.InputManager.IsActive.Returns(true);
            _mockEngine.InputManager.ShouldConsumeInput(Arg.Any<object>()).Returns(true);
            _mockEngine.InputManager.GetSurfaceState().Returns(new SurfaceState(_bounds.Centre, true));
            _mockEngine.ScreenManager.PointToWorld(Arg.Any<Vector2>()).Returns(_bounds.Centre);

            Construct();

            _sut.Update(new GameTime());

            _mockEngine.InputManager.Received(2).GetSurfaceState();
        }

        [TestMethod]
        public void UpdateAfterNotTouchedShouldNotRetrievedSurfaceState()
        {
            // Expect one call, one by base class, zero by Slider.

            _mockEngine.InputManager.IsActive.Returns(true);
            _mockEngine.InputManager.ShouldConsumeInput(Arg.Any<object>()).Returns(true);
            _mockEngine.InputManager.GetSurfaceState().Returns(new SurfaceState(_bounds.Centre, false));
            _mockEngine.ScreenManager.PointToWorld(Arg.Any<Vector2>()).Returns(_bounds.Centre);

            Construct();

            _sut.Update(new GameTime());

            _mockEngine.InputManager.Received(1).GetSurfaceState();
        }

        [TestMethod]
        public void UpdateWhileTouchedShouldUpdateWithExpectedValue_TestA()
        {
            _mockEngine.InputManager.IsActive.Returns(true);
            _mockEngine.InputManager.ShouldConsumeInput(Arg.Any<object>()).Returns(true);
            _mockEngine.InputManager.GetSurfaceState().Returns(new SurfaceState(_bounds.Centre, true));
            _mockEngine.ScreenManager.PointToWorld(Arg.Any<Vector2>()).Returns(_bounds.Centre);

            Construct();
            _sut.Value = 25;

            _sut.Update(new GameTime());

            Assert.That.AlmostEqual(50f, _sut.Value);
        }

        [TestMethod]
        public void UpdateWhileTouchedShouldUpdateWithExpectedValue_TestB()
        {
            Vector2 loc = _bounds.Centre + new Vector2(_bounds.Width / 4f, 0f);
            _mockEngine.InputManager.IsActive.Returns(true);
            _mockEngine.InputManager.ShouldConsumeInput(Arg.Any<object>()).Returns(true);
            _mockEngine.InputManager.GetSurfaceState().Returns(new SurfaceState(loc, true));
            _mockEngine.ScreenManager.PointToWorld(Arg.Any<Vector2>()).Returns(_bounds.Centre, loc);

            Construct();
            _sut.Value = 25;

            _sut.Update(new GameTime());

            Assert.That.AlmostEqual(75f, _sut.Value);
        }

        [TestMethod]
        public void UpdateWhileTouchedOutsideBoundsShouldUpdateWithExpectedValue_LeftOfBounds()
        {
            Vector2 loc = _bounds.Centre + new Vector2(-_bounds.Width, 0f);
            _mockEngine.InputManager.IsActive.Returns(true);
            _mockEngine.InputManager.ShouldConsumeInput(Arg.Any<object>()).Returns(true);
            _mockEngine.InputManager.GetSurfaceState().Returns(new SurfaceState(loc, true));
            _mockEngine.ScreenManager.PointToWorld(Arg.Any<Vector2>()).Returns(_bounds.Centre, loc);

            Construct();
            _sut.Value = 25;

            _sut.Update(new GameTime());

            Assert.That.AlmostEqual(0f, _sut.Value);
        }

        [TestMethod]
        public void UpdateWhileTouchedOutsideBoundsShouldUpdateWithExpectedValue_RightOfBounds()
        {
            Vector2 loc = _bounds.Centre + new Vector2(_bounds.Width, 0f);
            _mockEngine.InputManager.IsActive.Returns(true);
            _mockEngine.InputManager.ShouldConsumeInput(Arg.Any<object>()).Returns(true);
            _mockEngine.InputManager.GetSurfaceState().Returns(new SurfaceState(loc, true));
            _mockEngine.ScreenManager.PointToWorld(Arg.Any<Vector2>()).Returns(_bounds.Centre, loc);

            Construct();
            _sut.Value = 25;

            _sut.Update(new GameTime());

            Assert.That.AlmostEqual(100f, _sut.Value);
        }
        #endregion

        #region Draw Tests - Not Touched
        [TestMethod]
        public void DrawShouldDrawBorderWithExpectedColour()
        {
            Construct();
            _sut.ForegroundColour = Color.Pink;

            _sut.Draw(new GameTime());

            _mockSpriteBatch.Received(1).Draw(_mockPixel, _bounds, Color.Pink);
        }

        [TestMethod]
        public void DrawShouldDrawBackgroundWithExpectedColour()
        {
            Construct();
            _sut.BackgroundColour = Color.Pink;
            _bounds.Inflate(-2, -2);

            _sut.Draw(new GameTime());

            _mockSpriteBatch.Received(1).Draw(_mockPixel, _bounds, Color.Pink);
        }

        [TestMethod]
        public void DrawShouldDrawValueIndicatorWithExpectedColour()
        {
            Construct();
            _sut.ForegroundColour = Color.Pink;
            _sut.Maximum = 1f;
            _sut.Minimum = 0f;
            _sut.Value = 0.75f;
            _bounds.Inflate(-2, -2);
            _bounds.Inflate(-5, -5);

            _sut.Draw(new GameTime());

            _mockSpriteBatch.Received(1).Draw(_mockPixel, new RectangleF(_bounds.Left, _bounds.Top, _bounds.Width * 0.75f, _bounds.Height), Color.Pink);
        }
        #endregion

        #region Draw Tests - Touched
        [TestMethod]
        public void DrawWhileTouchedShouldDrawBorderWithExpectedColour()
        {
            Construct();
            _sut.HotColour = Color.Pink;

            _mockEngine.InputManager.ShouldConsumeInput(_sut).Returns(true);
            _mockEngine.InputManager.IsActive.Returns(true);
            _mockEngine.InputManager.GetSurfaceState().Returns(new SurfaceState(_bounds.Centre, true));
            _mockEngine.ScreenManager.PointToWorld(Arg.Any<Vector2>()).Returns(_bounds.Centre);
            _sut.Update(new GameTime());

            _sut.Draw(new GameTime());

            _mockSpriteBatch.Received(1).Draw(_mockPixel, _bounds, Color.Pink);
        }

        [TestMethod]
        public void DrawWhileTouchedShouldDrawValueIndicatorWithExpectedColour()
        {
            Construct();
            _sut.HotColour = Color.Pink;
            _sut.Maximum = 1f;
            _sut.Minimum = 0f;
            _bounds.Inflate(-2, -2);
            _bounds.Inflate(-5, -5);

            _mockEngine.InputManager.ShouldConsumeInput(_sut).Returns(true);
            _mockEngine.InputManager.IsActive.Returns(true);
            _mockEngine.InputManager.GetSurfaceState().Returns(new SurfaceState(_bounds.Centre, true));
            _mockEngine.ScreenManager.PointToWorld(Arg.Any<Vector2>()).Returns(_bounds.Centre);
            _sut.Update(new GameTime());

            _sut.Draw(new GameTime());

            _mockSpriteBatch.Received(1).Draw(_mockPixel, new RectangleF(_bounds.Left, _bounds.Top, _bounds.Width * 0.5f, _bounds.Height), Color.Pink);
        }
        #endregion
    }
}
