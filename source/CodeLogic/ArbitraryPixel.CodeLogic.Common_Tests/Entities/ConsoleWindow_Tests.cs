using ArbitraryPixel.CodeLogic.Common.Entities;
using ArbitraryPixel.CodeLogic.Common.Theme;
using ArbitraryPixel.CodeLogic.Common.Theme.Device;
using ArbitraryPixel.Common;
using ArbitraryPixel.Common.Drawing;
using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Common.Input;
using ArbitraryPixel.Platform2D.Engine;
using ArbitraryPixel.Platform2D.Text;
using ArbitraryPixel.Platform2D.Theme;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;
using NSubstitute;
using System;
using System.Linq;

namespace ArbitraryPixel.CodeLogic.Common_Tests.Entities
{
    [TestClass]
    public class ConsoleWindow_Tests : UnitTestBase<ConsoleWindow>
    {
        private const double EXPECTED_WINDOW_ANIMATE_TIME = 0.25;

        private IEngine _mockEngine;
        private ISpriteBatch _mockSpriteBatch;
        private ITextObjectBuilder _mockTextBuilder;
        private ITextObjectRenderer _mockTextRenderer;
        private RectangleF _bounds = new RectangleF(0, 0, 2000, 1000);

        private ITexture2D _mockPixelTexture;
        private ISpriteFont _mockNormalFont;
        private ISpriteFont _mockHeadingFont;

        protected override void OnInitializing()
        {
            _mockEngine = Substitute.For<IEngine>();
            _mockSpriteBatch = Substitute.For<ISpriteBatch>();

            _mockEngine.InputManager.IsActive.Returns(true);
            _mockEngine.InputManager.ShouldConsumeInput(Arg.Any<object>()).Returns(true);

            _mockEngine.ScreenManager.PointToWorld(Arg.Any<Vector2>()).Returns(x => x[0]);
            _mockEngine.ScreenManager.World.Returns((Point)_bounds.Size);

            _mockEngine.GrfxFactory.Texture2DFactory.Create(Arg.Any<IGrfxDevice>(), 1, 1).Returns(_mockPixelTexture = Substitute.For<ITexture2D>());
            _mockEngine.AssetBank.Get<ISpriteFont>("ConsoleNormalFont").Returns(_mockNormalFont = Substitute.For<ISpriteFont>());
            _mockEngine.AssetBank.Get<ISpriteFont>("ConsoleHeadingFont").Returns(_mockHeadingFont = Substitute.For<ISpriteFont>());

            _mockTextBuilder = Substitute.For<ITextObjectBuilder>();
            _mockTextBuilder.GetRegisteredFont("Normal").Returns(_mockNormalFont);

            _mockTextRenderer = Substitute.For<ITextObjectRenderer>();
            _mockTextRenderer.IsComplete.Returns(true); // default to rendering complete so that state machine just skips over processing for most tests.
        }

        protected override ConsoleWindow OnCreateSUT()
        {
            return new ConsoleWindow(_mockEngine, _bounds, _mockSpriteBatch, _mockTextBuilder, _mockTextRenderer);
        }

        #region Constructor Tests
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullSpriteBatchShouldThrowException()
        {
            _sut = new ConsoleWindow(_mockEngine, _bounds, null, _mockTextBuilder, _mockTextRenderer);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullTextBuilderShouldThrowException()
        {
            _sut = new ConsoleWindow(_mockEngine, _bounds, _mockSpriteBatch, null, _mockTextRenderer);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullTextRendererShouldThrowException()
        {
            _sut = new ConsoleWindow(_mockEngine, _bounds, _mockSpriteBatch, _mockTextBuilder, null);
        }

        [TestMethod]
        public void ConstructShouldCreateTextureForPixel()
        {
            _mockEngine.GrfxFactory.Texture2DFactory.Received(1).Create(Arg.Any<IGrfxDevice>(), 1, 1);
        }

        [TestMethod]
        public void ConstructShouldSetDataOnPixel()
        {
            _mockPixelTexture.Received(1).SetData<Color>(Arg.Is<Color[]>(x => x.SequenceEqual(new Color[] { Color.White })));
        }

        [TestMethod]
        public void ConstructWithAnimateOpenFalseShouldSetWindowStateProcessing()
        {
            _sut = new ConsoleWindow(_mockEngine, _bounds, _mockSpriteBatch, _mockTextBuilder, _mockTextRenderer, false);
            Assert.AreEqual<WindowState>(WindowState.Processing, _sut.WindowState);
        }
        #endregion

        #region Property Tests
        [TestMethod]
        public void WindowStateShouldDefaultReady()
        {
            Assert.AreEqual<WindowState>(WindowState.Ready, _sut.WindowState);
        }

        [TestMethod]
        public void ClientRectangleShouldReturnExpectedValue()
        {
            RectangleF clientRect = _bounds;
            clientRect.Inflate(-1, -1);
            Assert.AreEqual<RectangleF>(clientRect, _sut.ClientRectangle);
        }

        [TestMethod]
        public void ClientRectangleWithPaddingShouldReturnExpectedValue()
        {
            _sut.Padding = new SizeF(200, 100);
            RectangleF expected = _bounds;
            expected.Inflate(-1, -1);
            expected.Inflate(-200, -100);
            Assert.AreEqual<RectangleF>(expected, _sut.ClientRectangle);
        }

        [TestMethod]
        public void ClientRectangleWithPaddingAndBorderSizeShouldReturnExpectedValue()
        {
            _sut.Padding = new SizeF(200, 100);
            _sut.BorderSize = new SizeF(5, 5);
            RectangleF expected = _bounds;
            expected.Inflate(-200, -100);
            expected.Inflate(-5, -5);
            Assert.AreEqual<RectangleF>(expected, _sut.ClientRectangle);
        }

        [TestMethod]
        public void AutoAdvanceOnTapShouldDefaultTrue()
        {
            Assert.IsTrue(_sut.AutoAdvanceOnTap);
        }

        [TestMethod]
        public void AutoAdvanceOnTapShouldReturnSetValue()
        {
            _sut.AutoAdvanceOnTap = false;
            Assert.IsFalse(_sut.AutoAdvanceOnTap);
        }

        [TestMethod]
        public void ShowBackgroundShouldDefaultTrue()
        {
            Assert.IsTrue(_sut.ShowBackground);
        }

        [TestMethod]
        public void ShowBackgroundShouldReturnSetValue()
        {
            _sut.ShowBackground = false;
            Assert.IsFalse(_sut.ShowBackground);
        }

        [TestMethod]
        public void PaddingShouldDefaultToEmpty()
        {
            Assert.AreEqual<SizeF>(SizeF.Empty, _sut.Padding);
        }

        [TestMethod]
        public void PaddingShouldReturnSetValue()
        {
            _sut.Padding = new SizeF(100, 200);

            Assert.AreEqual<SizeF>(new SizeF(100, 200), _sut.Padding);
        }

        [TestMethod]
        public void BorderSizeShouldDefaultToExpectedValue()
        {
            Assert.AreEqual<SizeF>(new SizeF(1, 1), _sut.BorderSize);
        }

        [TestMethod]
        public void BorderSizeShouldReturnSetValue()
        {
            _sut.BorderSize = new SizeF(200, 100);

            Assert.AreEqual<SizeF>(new SizeF(200, 100), _sut.BorderSize);
        }

        [TestMethod]
        public void BackgroundColourShouldDefaultToExpectedColor()
        {
            Assert.AreEqual<Color>(new Color(32, 32, 32), _sut.BackgroundColour);
        }

        [TestMethod]
        public void BackgroundColourShouldReturnSetValue()
        {
            _sut.BackgroundColour = Color.Pink;

            Assert.AreEqual<Color>(Color.Pink, _sut.BackgroundColour);
        }

        [TestMethod]
        public void BorderColourShouldDefaultToExpectedValue()
        {
            Assert.AreEqual<Color>(Color.Red, _sut.BorderColour);
        }

        [TestMethod]
        public void BorderColourShouldReturnSetValue()
        {
            _sut.BorderColour = Color.Pink;

            Assert.AreEqual<Color>(Color.Pink, _sut.BorderColour);
        }
        #endregion

        #region Update Tests (State Machine)
        [TestMethod]
        public void WindowStateShouldStartAsReady()
        {
            Assert.AreEqual<WindowState>(WindowState.Ready, _sut.WindowState);
        }

        [TestMethod]
        public void WindowStateShouldStartAsOpeningAfterFirstUpdate()
        {
            _sut.Update(new GameTime());
            Assert.AreEqual<WindowState>(WindowState.Opening, _sut.WindowState);
        }

        [TestMethod]
        public void WindowStateShouldAdvanceToProcessingAfterExpectedTime()
        {
            _sut.Update(new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(EXPECTED_WINDOW_ANIMATE_TIME)));

            Assert.AreEqual<WindowState>(WindowState.Processing, _sut.WindowState);
        }

        [TestMethod]
        public void WindowStateAsProcessingShouldAdvanceToWaitingAfterNextUpdate()
        {
            _sut.Update(new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(EXPECTED_WINDOW_ANIMATE_TIME)));
            _sut.Update(new GameTime());

            Assert.AreEqual<WindowState>(WindowState.Waiting, _sut.WindowState);
        }

        [TestMethod]
        public void WindowStateAsProcessingShouldAdvanceToWaitingAfterTap()
        {
            _mockTextRenderer.IsComplete.Returns(false);

            _sut.Update(new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(EXPECTED_WINDOW_ANIMATE_TIME)));
            _sut.Update(new GameTime());

            // Inject a touch and update to process it.
            _mockEngine.InputManager.GetSurfaceState().Returns(new SurfaceState(_bounds.Centre, true));
            _sut.Update(new GameTime());

            // Inject a release and update to process it, which should set state to closing.
            _mockEngine.InputManager.GetSurfaceState().Returns(new SurfaceState(_bounds.Centre, false));
            _sut.Update(new GameTime());

            Assert.AreEqual<WindowState>(WindowState.Waiting, _sut.WindowState);
        }

        [TestMethod]
        public void WindowWithAutoAdvanceFalseAndStateAsProcessingShouldNotAdvanceToWaitingAfterTap()
        {
            _mockTextRenderer.IsComplete.Returns(false);

            _sut.AutoAdvanceOnTap = false;
            _sut.Update(new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(EXPECTED_WINDOW_ANIMATE_TIME)));
            _sut.Update(new GameTime());

            // Inject a touch and update to process it.
            _mockEngine.InputManager.GetSurfaceState().Returns(new SurfaceState(_bounds.Centre, true));
            _sut.Update(new GameTime());

            // Inject a release and update to process it, which should set state to closing.
            _mockEngine.InputManager.GetSurfaceState().Returns(new SurfaceState(_bounds.Centre, false));
            _sut.Update(new GameTime());

            Assert.AreEqual<WindowState>(WindowState.Processing, _sut.WindowState);
        }

        [TestMethod]
        public void WindowStateAsProcessingShouldCallRenderOnTextRenderer()
        {
            _sut.Update(new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(EXPECTED_WINDOW_ANIMATE_TIME)));

            _sut.Update(new GameTime());

            _mockTextRenderer.Received(1).Update(Arg.Any<GameTime>());
        }

        [TestMethod]
        public void WindowStateAsWaitingShouldAdvanceToClosingAfterTap()
        {
            _sut.Update(new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(EXPECTED_WINDOW_ANIMATE_TIME)));
            _sut.Update(new GameTime());

            // Inject a touch and update to process it.
            _mockEngine.InputManager.GetSurfaceState().Returns(new SurfaceState(_bounds.Centre, true));
            _sut.Update(new GameTime());

            // Inject a release and update to process it, which should set state to closing.
            _mockEngine.InputManager.GetSurfaceState().Returns(new SurfaceState(_bounds.Centre, false));
            _sut.Update(new GameTime());

            Assert.AreEqual<WindowState>(WindowState.Closing, _sut.WindowState);
        }

        [TestMethod]
        public void WindowWithAutoAdvanceFalseAndStateAsWaitingShouldNotAdvanceToClosingAfterTap()
        {
            _sut.AutoAdvanceOnTap = false;
            _sut.Update(new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(EXPECTED_WINDOW_ANIMATE_TIME)));
            _sut.Update(new GameTime());

            // Inject a touch and update to process it.
            _mockEngine.InputManager.GetSurfaceState().Returns(new SurfaceState(_bounds.Centre, true));
            _sut.Update(new GameTime());

            // Inject a release and update to process it, which should set state to closing.
            _mockEngine.InputManager.GetSurfaceState().Returns(new SurfaceState(_bounds.Centre, false));
            _sut.Update(new GameTime());

            Assert.AreEqual<WindowState>(WindowState.Waiting, _sut.WindowState);
        }

        [TestMethod]
        public void WindowStateAsClosingShouldAdvanceToCompleteAfterExpectedTime()
        {
            _sut.Update(new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(EXPECTED_WINDOW_ANIMATE_TIME)));
            _sut.Update(new GameTime());

            // Inject a touch and update to process it.
            _mockEngine.InputManager.GetSurfaceState().Returns(new SurfaceState(_bounds.Centre, true));
            _sut.Update(new GameTime());

            // Inject a release and update to process it, which should set state to closing.
            _mockEngine.InputManager.GetSurfaceState().Returns(new SurfaceState(_bounds.Centre, false));
            _sut.Update(new GameTime());

            _sut.Update(new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(EXPECTED_WINDOW_ANIMATE_TIME)));

            Assert.AreEqual<WindowState>(WindowState.Complete, _sut.WindowState);
        }

        [TestMethod]
        public void AliveShouldBeSetToFalseWhenStateReachesComplete()
        {
            _sut.Update(new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(EXPECTED_WINDOW_ANIMATE_TIME)));
            _sut.Update(new GameTime());

            // Inject a touch and update to process it.
            _mockEngine.InputManager.GetSurfaceState().Returns(new SurfaceState(_bounds.Centre, true));
            _sut.Update(new GameTime());

            // Inject a release and update to process it, which should set state to closing.
            _mockEngine.InputManager.GetSurfaceState().Returns(new SurfaceState(_bounds.Centre, false));
            _sut.Update(new GameTime());

            _sut.Update(new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(EXPECTED_WINDOW_ANIMATE_TIME)));

            Assert.IsFalse(_sut.Alive);
        }

        [TestMethod]
        public void WindowOpeningAnimatesWithExpectedVelocity()
        {
            _sut.BorderColour = Color.Pink;
            Vector2 expectedVelocity = _bounds.Size / (float)EXPECTED_WINDOW_ANIMATE_TIME;
            double timeInterval = 0.1;

            _sut.Update(new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(timeInterval)));

            RectangleF drawnRectangle = RectangleF.Empty;
            // Trap the border draw and get the rectangle
            _mockSpriteBatch.When(x => x.Draw(Arg.Any<ITexture2D>(), Arg.Any<RectangleF>(), Color.Pink)).Do(
                x =>
                {
                    drawnRectangle = (RectangleF)x[1];
                }
            );

            _sut.Draw(new GameTime());

            if (drawnRectangle == RectangleF.Empty)
                Assert.Fail();

            Vector2 actualVelocity = drawnRectangle.Size / (float)timeInterval;

            Assert.AreEqual<Vector2>(expectedVelocity, actualVelocity);
        }

        [TestMethod]
        public void WindowClosingAnimatesWithExpectedVelocity()
        {
            _sut.Update(new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(EXPECTED_WINDOW_ANIMATE_TIME)));
            _sut.Update(new GameTime());

            // Inject a touch and update to process it.
            _mockEngine.InputManager.GetSurfaceState().Returns(new SurfaceState(_bounds.Centre, true));
            _sut.Update(new GameTime());

            // Inject a release and update to process it, which should set state to closing.
            _mockEngine.InputManager.GetSurfaceState().Returns(new SurfaceState(_bounds.Centre, false));
            _sut.Update(new GameTime());

            _sut.BorderColour = Color.Pink;
            Vector2 expectedVelocity = _bounds.Size / (float)EXPECTED_WINDOW_ANIMATE_TIME;
            double timeInterval = 0.1;

            _sut.Update(new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(timeInterval)));

            RectangleF drawnRectangle = RectangleF.Empty;
            // Trap the border draw and get the rectangle
            _mockSpriteBatch.When(x => x.Draw(Arg.Any<ITexture2D>(), Arg.Any<RectangleF>(), Color.Pink)).Do(
                x =>
                {
                    drawnRectangle = (RectangleF)x[1];
                }
            );

            _sut.Draw(new GameTime());

            if (drawnRectangle == RectangleF.Empty)
                Assert.Fail();

            Vector2 actualVelocity = (_bounds.Size - drawnRectangle.Size) / (float)timeInterval;

            Assert.AreEqual<Vector2>(expectedVelocity, actualVelocity);
        }
        #endregion

        #region PreDraw Tests
        [TestMethod]
        public void PreDrawWhileProcessingShouldCallTextRendererRender()
        {
            _sut.Update(new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(EXPECTED_WINDOW_ANIMATE_TIME)));

            _sut.PreDraw(new GameTime());

            _mockTextRenderer.Received(1).Render();
        }

        [TestMethod]
        public void PreDrawWhileWaitingShouldCallTextRendererRender()
        {
            _sut.Update(new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(EXPECTED_WINDOW_ANIMATE_TIME)));
            _sut.Update(new GameTime());

            _sut.PreDraw(new GameTime());

            _mockTextRenderer.Received(1).Render();
        }
        #endregion

        #region Draw Tests
        [TestMethod]
        public void DrawShouldDrawBorder()
        {
            _sut.BorderColour = Color.Pink;
            _sut.Update(new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(EXPECTED_WINDOW_ANIMATE_TIME))); // Push to state where fully open.

            _sut.Draw(new GameTime());

            _mockSpriteBatch.Received(1).Draw(_mockPixelTexture, _bounds, Color.Pink);
        }

        [TestMethod]
        public void DrawShouldDrawBackground()
        {
            _sut.BackgroundColour = Color.Pink;
            _sut.Update(new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(EXPECTED_WINDOW_ANIMATE_TIME))); // Push to state where fully open.
            RectangleF expectedBounds = (RectangleF)_bounds;
            expectedBounds.Inflate(-1, -1);

            _sut.Draw(new GameTime());

            _mockSpriteBatch.Received(1).Draw(_mockPixelTexture, expectedBounds, Color.Pink);
        }

        [TestMethod]
        public void DrawWithBorderSizeShouldDrawBackground()
        {
            _sut.BorderSize = new SizeF(5, 5);
            _sut.BackgroundColour = Color.Pink;
            _sut.Update(new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(EXPECTED_WINDOW_ANIMATE_TIME))); // Push to state where fully open.
            RectangleF expectedBounds = (RectangleF)_bounds;
            expectedBounds.Inflate(-5, -5);

            _sut.Draw(new GameTime());

            _mockSpriteBatch.Received(1).Draw(_mockPixelTexture, expectedBounds, Color.Pink);
        }

        [TestMethod]
        public void DrawWithShowBackgroundFalseShouldNotDrawBorder()
        {
            _sut.ShowBackground = false;

            _sut.Update(new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(EXPECTED_WINDOW_ANIMATE_TIME))); // Push to state where fully open.

            _sut.Draw(new GameTime());

            _mockSpriteBatch.Received(0).Draw(_mockPixelTexture, _bounds, Arg.Any<Color>());
        }

        [TestMethod]
        public void DrawWithShowBackgroundFalseShouldNotDrawBackground()
        {
            _sut.ShowBackground = false;

            _sut.Update(new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(EXPECTED_WINDOW_ANIMATE_TIME))); // Push to state where fully open.
            RectangleF expectedBounds = (RectangleF)_bounds;
            expectedBounds.Inflate(-1, -1);

            _sut.Draw(new GameTime());

            _mockSpriteBatch.Received(0).Draw(_mockPixelTexture, expectedBounds, Arg.Any<Color>());
        }

        [TestMethod]
        public void DrawShouldNotDrawTextWhileOpening()
        {
            _sut.Draw(new GameTime());

            _mockSpriteBatch.Received(0).DrawString(Arg.Any<ISpriteFont>(), Arg.Any<string>(), Arg.Any<Vector2>(), Arg.Any<Color>());
        }

        [TestMethod]
        public void DrawShouldNotDrawTextWhileClosing()
        {
            _sut.Update(new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(EXPECTED_WINDOW_ANIMATE_TIME)));
            _sut.Update(new GameTime());

            // Inject a touch and update to process it.
            _mockEngine.InputManager.GetSurfaceState().Returns(new SurfaceState(_bounds.Centre, true));
            _sut.Update(new GameTime());

            // Inject a release and update to process it, which should set state to closing.
            _mockEngine.InputManager.GetSurfaceState().Returns(new SurfaceState(_bounds.Centre, false));
            _sut.Update(new GameTime());

            _sut.Draw(new GameTime());

            _mockSpriteBatch.Received(0).DrawString(Arg.Any<ISpriteFont>(), Arg.Any<string>(), Arg.Any<Vector2>(), Arg.Any<Color>());
        }

        [TestMethod]
        public void DrawShouldDrawTextWhileProcessing()
        {
            ITexture2D mockTextTexture = Substitute.For<ITexture2D>();
            _mockTextRenderer.Render().Returns(mockTextTexture);
            _sut.Update(new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(EXPECTED_WINDOW_ANIMATE_TIME)));
            _sut.PreDraw(new GameTime());

            _sut.Draw(new GameTime());

            _mockSpriteBatch.Received(1).Draw(mockTextTexture, _sut.ClientRectangle.Location, Color.White);
        }

        [TestMethod]
        public void DrawShouldDrawTextWhileWaiting()
        {
            ITexture2D mockTextTexture = Substitute.For<ITexture2D>();
            _mockTextRenderer.Render().Returns(mockTextTexture);
            _sut.Update(new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(EXPECTED_WINDOW_ANIMATE_TIME)));
            _sut.PreDraw(new GameTime());
            _sut.Update(new GameTime());

            _sut.Draw(new GameTime());

            _mockSpriteBatch.Received(1).Draw(mockTextTexture, _sut.ClientRectangle.Location, Color.White);
        }

        [TestMethod]
        public void DrawWithNullTextTextureShouldNotCallDraw()
        {
            ITexture2D mockTextTexture = null;
            _mockTextRenderer.Render().Returns(mockTextTexture);
            _sut.Update(new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(EXPECTED_WINDOW_ANIMATE_TIME)));
            _sut.PreDraw(new GameTime());
            _sut.Update(new GameTime());

            _sut.Draw(new GameTime());

            _mockSpriteBatch.Received(0).Draw(Arg.Any<ITexture2D>(), Arg.Any<Vector2>(), Arg.Any<Color>());
        }
        #endregion

        #region FlushText Tests
        [TestMethod]
        public void FlushTextShouldCallTextRendererFlushText()
        {
            _sut.FlushText();
            _mockTextRenderer.Received(1).Flush();
        }
        #endregion

        #region CloseWindow Tests
        [TestMethod]
        public void CloseWindowShouldSetWindowStateToClosing()
        {
            _sut.CloseWindow();

            Assert.AreEqual<WindowState>(WindowState.Closing, _sut.WindowState);
        }
        #endregion

        #region WindowStateChanged Tests
        [TestMethod]
        public void WindowStateChangeEventShouldFireWhenWindowStateChanges_TestA()
        {
            EventHandler<StateChangedEventArgs<WindowState>> mockSubscriber = Substitute.For<EventHandler<StateChangedEventArgs<WindowState>>>();
            _sut.WindowStateChanged += mockSubscriber;
            _sut.Update(new GameTime());
            mockSubscriber.ClearReceivedCalls();

            _sut.CloseWindow();

            mockSubscriber.Received(1)(_sut, Arg.Is<StateChangedEventArgs<WindowState>>(x => x.PreviousState == WindowState.Opening && x.CurrentState == WindowState.Closing));
        }

        [TestMethod]
        public void WindowStateChangeEventShouldFireWhenWindowStateChanges_TestB()
        {
            EventHandler<StateChangedEventArgs<WindowState>> mockSubscriber = Substitute.For<EventHandler<StateChangedEventArgs<WindowState>>>();
            _sut.WindowStateChanged += mockSubscriber;

            _sut.Update(new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(EXPECTED_WINDOW_ANIMATE_TIME)));

            mockSubscriber.Received(1)(_sut, Arg.Is<StateChangedEventArgs<WindowState>>(x => x.PreviousState == WindowState.Opening && x.CurrentState == WindowState.Processing));
        }
        #endregion
    }
}
