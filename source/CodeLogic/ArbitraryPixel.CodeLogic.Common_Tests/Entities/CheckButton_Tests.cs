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
    public class CheckButton_Tests
    {
        private CheckButton _sut;
        private IEngine _mockEngine;
        private RectangleF _bounds = new RectangleF(400, 300, 200, 100);
        private ISpriteBatch _mockSpriteBatch;

        [TestInitialize]
        public void Initialize()
        {
            _mockEngine = Substitute.For<IEngine>();
            _mockSpriteBatch = Substitute.For<ISpriteBatch>();
        }

        private void Construct()
        {
            _sut = new CheckButton(_mockEngine, _bounds, _mockSpriteBatch);
        }

        #region Constructor Tests
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullParameterShouldThrowException_SpriteBatch()
        {
            _sut = new CheckButton(_mockEngine, _bounds, null);
        }
        #endregion

        #region Property Tests
        [TestMethod]
        public void PropertyShouldHaveExpectedDefault_Checked()
        {
            Construct();
            Assert.IsFalse(_sut.Checked);
        }

        [TestMethod]
        public void PropertyShouldReturnSetValue_Checked()
        {
            Construct();

            _sut.Checked = true;

            Assert.IsTrue(_sut.Checked);
        }
        #endregion

        #region Property Change Events
        [TestMethod]
        public void PropertySetToNewValueShouldTriggerEvent_Checked()
        {
            Construct();
            _sut.Checked = true;

            EventHandler subscriber = Substitute.For<EventHandler>();
            _sut.CheckStateChanged += subscriber;

            _sut.Checked = false;

            subscriber.Received(1)(_sut, Arg.Any<EventArgs>());
        }

        [TestMethod]
        public void PropertySetToSameValueShouldNotTriggerEvent_Checked()
        {
            Construct();
            _sut.Checked = true;

            EventHandler subscriber = Substitute.For<EventHandler>();
            _sut.CheckStateChanged += subscriber;

            _sut.Checked = true;

            subscriber.Received(0)(_sut, Arg.Any<EventArgs>());
        }
        #endregion

        #region Tapped Tests
        [TestMethod]
        public void TappingButtonShouldToggleCheckState_FalseToTrue()
        {
            _mockEngine.InputManager.IsActive.Returns(true);
            _mockEngine.InputManager.ShouldConsumeInput(Arg.Any<object>()).Returns(true);
            _mockEngine.InputManager.GetSurfaceState().Returns(new SurfaceState(_bounds.Centre, true));
            _mockEngine.ScreenManager.PointToWorld(Arg.Any<Vector2>()).Returns(_bounds.Centre);
            Construct();
            _sut.Checked = false;

            _sut.Update(new GameTime());
            _mockEngine.InputManager.GetSurfaceState().Returns(new SurfaceState(_bounds.Centre, false));
            _sut.Update(new GameTime());

            Assert.IsTrue(_sut.Checked);
        }

        [TestMethod]
        public void TappingButtonShouldToggleCheckState_TrueToFalse()
        {
            _mockEngine.InputManager.IsActive.Returns(true);
            _mockEngine.InputManager.ShouldConsumeInput(Arg.Any<object>()).Returns(true);
            _mockEngine.InputManager.GetSurfaceState().Returns(new SurfaceState(_bounds.Centre, true));
            _mockEngine.ScreenManager.PointToWorld(Arg.Any<Vector2>()).Returns(_bounds.Centre);
            Construct();
            _sut.Checked = true;

            _sut.Update(new GameTime());
            _mockEngine.InputManager.GetSurfaceState().Returns(new SurfaceState(_bounds.Centre, false));
            _sut.Update(new GameTime());

            Assert.IsFalse(_sut.Checked);
        }
        #endregion

        #region Draw Tests
        [TestMethod]
        public void DrawShouldRetrievePixelTexture()
        {
            _mockEngine.AssetBank.Get<ITexture2D>("Pixel").Returns(Substitute.For<ITexture2D>());
            Construct();

            _sut.Draw(new GameTime());

            _mockEngine.AssetBank.Received(1).Get<ITexture2D>("Pixel");
        }

        [TestMethod]
        public void DrawForStateShouldMakeExpectedDrawCalls_Unchecked_Unpressed()
        {
            ITexture2D mockPixel = Substitute.For<ITexture2D>();
            _mockEngine.AssetBank.Get<ITexture2D>("Pixel").Returns(mockPixel);
            Construct();

            _sut.Draw(new GameTime());

            _mockSpriteBatch.Received(2).Draw(Arg.Any<ITexture2D>(), Arg.Any<Rectangle>(), Arg.Any<Color>());
        }

        [TestMethod]
        public void DrawForStateShouldDrawAsExpected_Unchecked_Unpressed()
        {
            ITexture2D mockPixel = Substitute.For<ITexture2D>();
            _mockEngine.AssetBank.Get<ITexture2D>("Pixel").Returns(mockPixel);
            Construct();

            _sut.Draw(new GameTime());

            Color expectedFG = CodeLogicEngine.Constants.ClrMenuFGMid;
            Color expectedBG = CodeLogicEngine.Constants.ClrMenuFGLow;
            Rectangle expectedBounds = (Rectangle)_bounds;

            Received.InOrder(
                () =>
                {
                    _mockSpriteBatch.Draw(mockPixel, expectedBounds, expectedFG);
                    expectedBounds.Inflate(-2, -2);
                    _mockSpriteBatch.Draw(mockPixel, expectedBounds, expectedBG);
                }
            );
        }

        [TestMethod]
        public void DrawForStateShouldMakeExpectedDrawCalls_Unchecked_Pressed()
        {
            _mockEngine.InputManager.IsActive.Returns(true);
            _mockEngine.InputManager.ShouldConsumeInput(Arg.Any<object>()).Returns(true);
            _mockEngine.InputManager.GetSurfaceState().Returns(new SurfaceState(_bounds.Centre, true));
            _mockEngine.ScreenManager.PointToWorld(Arg.Any<Vector2>()).Returns(_bounds.Centre);

            ITexture2D mockPixel = Substitute.For<ITexture2D>();
            _mockEngine.AssetBank.Get<ITexture2D>("Pixel").Returns(mockPixel);
            Construct();

            _sut.Update(new GameTime());
            _sut.Draw(new GameTime());

            _mockSpriteBatch.Received(2).Draw(Arg.Any<ITexture2D>(), Arg.Any<Rectangle>(), Arg.Any<Color>());
        }

        [TestMethod]
        public void DrawForStateShouldDrawAsExpected_Unchecked_Pressed()
        {
            _mockEngine.InputManager.IsActive.Returns(true);
            _mockEngine.InputManager.ShouldConsumeInput(Arg.Any<object>()).Returns(true);
            _mockEngine.InputManager.GetSurfaceState().Returns(new SurfaceState(_bounds.Centre, true));
            _mockEngine.ScreenManager.PointToWorld(Arg.Any<Vector2>()).Returns(_bounds.Centre);

            ITexture2D mockPixel = Substitute.For<ITexture2D>();
            _mockEngine.AssetBank.Get<ITexture2D>("Pixel").Returns(mockPixel);
            Construct();

            _sut.Update(new GameTime());
            _sut.Draw(new GameTime());

            Color expectedFG = CodeLogicEngine.Constants.ClrMenuFGHigh;
            Color expectedBG = CodeLogicEngine.Constants.ClrMenuFGLow;
            Rectangle expectedBounds = (Rectangle)_bounds;

            Received.InOrder(
                () =>
                {
                    _mockSpriteBatch.Draw(mockPixel, expectedBounds, expectedFG);
                    expectedBounds.Inflate(-2, -2);
                    _mockSpriteBatch.Draw(mockPixel, expectedBounds, expectedBG);
                }
            );
        }

        [TestMethod]
        public void DrawForStateShouldMakeExpectedDrawCalls_Checked_Unpressed()
        {
            ITexture2D mockPixel = Substitute.For<ITexture2D>();
            _mockEngine.AssetBank.Get<ITexture2D>("Pixel").Returns(mockPixel);
            Construct();
            _sut.Checked = true;

            _sut.Update(new GameTime());
            _sut.Draw(new GameTime());

            _mockSpriteBatch.Received(3).Draw(Arg.Any<ITexture2D>(), Arg.Any<Rectangle>(), Arg.Any<Color>());
        }

        [TestMethod]
        public void DrawForStateShouldDrawAsExpected_Checked_Unpressed()
        {
            ITexture2D mockPixel = Substitute.For<ITexture2D>();
            _mockEngine.AssetBank.Get<ITexture2D>("Pixel").Returns(mockPixel);
            Construct();
            _sut.Checked = true;

            _sut.Update(new GameTime());
            _sut.Draw(new GameTime());

            Color expectedFG = CodeLogicEngine.Constants.ClrMenuFGMid;
            Color expectedBG = CodeLogicEngine.Constants.ClrMenuFGLow;
            Rectangle expectedBounds = (Rectangle)_bounds;

            Received.InOrder(
                () =>
                {
                    _mockSpriteBatch.Draw(mockPixel, expectedBounds, expectedFG);
                    expectedBounds.Inflate(-2, -2);
                    _mockSpriteBatch.Draw(mockPixel, expectedBounds, expectedBG);
                    expectedBounds.Inflate(-5, -5);
                    _mockSpriteBatch.Draw(mockPixel, expectedBounds, expectedFG);
                }
            );
        }

        [TestMethod]
        public void DrawForStateShouldMakeExpectedDrawCalls_Checked_Pressed()
        {
            _mockEngine.InputManager.IsActive.Returns(true);
            _mockEngine.InputManager.ShouldConsumeInput(Arg.Any<object>()).Returns(true);
            _mockEngine.InputManager.GetSurfaceState().Returns(new SurfaceState(_bounds.Centre, true));
            _mockEngine.ScreenManager.PointToWorld(Arg.Any<Vector2>()).Returns(_bounds.Centre);

            ITexture2D mockPixel = Substitute.For<ITexture2D>();
            _mockEngine.AssetBank.Get<ITexture2D>("Pixel").Returns(mockPixel);
            Construct();
            _sut.Checked = true;

            _sut.Update(new GameTime());
            _sut.Draw(new GameTime());

            _mockSpriteBatch.Received(3).Draw(Arg.Any<ITexture2D>(), Arg.Any<Rectangle>(), Arg.Any<Color>());
        }

        [TestMethod]
        public void DrawForStateShouldDrawAsExpected_Checked_Pressed()
        {
            _mockEngine.InputManager.IsActive.Returns(true);
            _mockEngine.InputManager.ShouldConsumeInput(Arg.Any<object>()).Returns(true);
            _mockEngine.InputManager.GetSurfaceState().Returns(new SurfaceState(_bounds.Centre, true));
            _mockEngine.ScreenManager.PointToWorld(Arg.Any<Vector2>()).Returns(_bounds.Centre);

            ITexture2D mockPixel = Substitute.For<ITexture2D>();
            _mockEngine.AssetBank.Get<ITexture2D>("Pixel").Returns(mockPixel);
            Construct();
            _sut.Checked = true;

            _sut.Update(new GameTime());
            _sut.Draw(new GameTime());

            Color expectedFG = CodeLogicEngine.Constants.ClrMenuFGHigh;
            Color expectedBG = CodeLogicEngine.Constants.ClrMenuFGLow;
            Rectangle expectedBounds = (Rectangle)_bounds;

            Received.InOrder(
                () =>
                {
                    _mockSpriteBatch.Draw(mockPixel, expectedBounds, expectedFG);
                    expectedBounds.Inflate(-2, -2);
                    _mockSpriteBatch.Draw(mockPixel, expectedBounds, expectedBG);
                    expectedBounds.Inflate(-5, -5);
                    _mockSpriteBatch.Draw(mockPixel, expectedBounds, expectedFG);
                }
            );
        }
        #endregion
    }
}
