using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArbitraryPixel.Platform2D.Engine;
using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.CodeLogic.Common.Model;
using ArbitraryPixel.Common.Drawing;
using ArbitraryPixel.CodeLogic.Common.Entities;
using NSubstitute;
using Microsoft.Xna.Framework;
using ArbitraryPixel.Common.Input;

namespace ArbitraryPixel.CodeLogic.Common_Tests.Entities
{
    [TestClass]
    public class CodeInputButton_InputResponse_Tests
    {
        private IEngine _mockEngine;
        private ISpriteBatch _mockSpriteBatch;
        private ICodeInputButtonModel _mockModel;

        private RectangleF _bounds = new RectangleF(100, 200, 300, 400);

        private CodeInputButton _sut;

        [TestInitialize]
        public void Initialize()
        {
            _mockEngine = Substitute.For<IEngine>();
            _mockSpriteBatch = Substitute.For<ISpriteBatch>();
            _mockModel = Substitute.For<ICodeInputButtonModel>();

            _mockEngine.InputManager.ShouldConsumeInput(Arg.Any<object>()).Returns(true);
            _mockEngine.InputManager.IsActive.Returns(true);
            _mockEngine.ScreenManager.PointToWorld(Arg.Any<Vector2>()).Returns(x => (Vector2)x[0]);
            _mockEngine.ScreenManager.World.Returns(new Point(1000, 1000));

            _mockModel.DeviceModel.InputSequence.Length.Returns(4);
            _mockModel.SelectorState.Returns(CodeInputButtonSelectorState.Open);

            _sut = new CodeInputButton(_mockEngine, _bounds, _mockSpriteBatch, _mockModel, 0);

            // Push first update to capture touched.
            TouchSurface();
        }

        #region Helper Methods
        private void TouchSurface()
        {
            _mockEngine.InputManager.GetSurfaceState().Returns(new SurfaceState(_bounds.Centre, true));
            _sut.Update(new GameTime());
        }

        private void ReleaseSurface()
        {
            _mockModel.ClearReceivedCalls();
            _mockModel.DeviceModel.ClearReceivedCalls();
            _mockModel.DeviceModel.InputSequence.ClearReceivedCalls();
            _mockEngine.InputManager.ClearReceivedCalls();
            _mockEngine.ScreenManager.ClearReceivedCalls();

            _mockEngine.InputManager.GetSurfaceState().Returns(new SurfaceState(_bounds.Centre, false));
            _sut.Update(new GameTime());
        }
        #endregion

        #region OnTouched Tests
        [TestMethod]
        public void OnTouchedShouldOpenSelector()
        {
            _mockModel.Received(1).OpenSelector();
        }

        [TestMethod]
        public void OnTouchedShouldOpenSelectorAndFireSelectorOpenedEvent()
        {
            _mockModel.SelectorState.Returns(CodeInputButtonSelectorState.Opening);
            _mockModel.ScaleValue.Returns(1);

            _sut = new CodeInputButton(_mockEngine, _bounds, _mockSpriteBatch, _mockModel, 0);
            EventHandler<EventArgs> subscriber = Substitute.For<EventHandler<EventArgs>>();
            _sut.SelectorOpened += subscriber;

            TouchSurface();

            subscriber.Received(1)(_sut, Arg.Any<EventArgs>());
        }
        #endregion

        #region OnReleasd Tests
        [TestMethod]
        public void OnReleasedGestureModeAcceptTests_HaveGesture()
        {
            _mockModel.SelectorMode.Returns(CodeInputButtonSelectorMode.Gesture);
            _mockModel.GestureAngle.Returns(MathHelper.ToRadians(300));
            _mockModel.MovedOutOfDeadzone.Returns(false);

            ReleaseSurface();

            Received.InOrder(
                () =>
                {
                    _mockModel.DeviceModel.InputSequence[0] = CodeValue.Red;
                    _mockModel.MovedOutOfDeadzone = false;
                    _mockModel.SelectorMode = CodeInputButtonSelectorMode.Gesture;
                    _mockModel.CloseSelector();
                }
            );
        }

        [TestMethod]
        public void OnReleasedGestureModeAcceptTests_NoGestureButMovedOut()
        {
            _mockModel.SelectorMode.Returns(CodeInputButtonSelectorMode.Gesture);
            _mockModel.GestureAngle.Returns((float?)null);
            _mockModel.MovedOutOfDeadzone.Returns(true);

            ReleaseSurface();

            Received.InOrder(
                () =>
                {
                    _mockModel.MovedOutOfDeadzone = false;
                    _mockModel.SelectorMode = CodeInputButtonSelectorMode.Gesture;
                    _mockModel.CloseSelector();
                }
            );
        }

        [TestMethod]
        public void OnReleaseGestureMode_NoGestureOrMoveOut_ShouldSetSelectorModeToSelect()
        {
            _mockModel.SelectorMode.Returns(CodeInputButtonSelectorMode.Gesture);
            _mockModel.GestureAngle.Returns((float?)null);
            _mockModel.MovedOutOfDeadzone.Returns(false);

            ReleaseSurface();

            _mockModel.Received(1).SelectorMode = CodeInputButtonSelectorMode.Select;
        }

        [TestMethod]
        public void OnReleaseSelectModeWithGesture_ShouldTakeExpectedActions()
        {
            _mockModel.SelectorMode.Returns(CodeInputButtonSelectorMode.Select);
            _mockModel.GestureAngle.Returns(MathHelper.ToRadians(180));

            ReleaseSurface();

            Received.InOrder(
                () =>
                {
                    _mockModel.DeviceModel.InputSequence[0] = CodeValue.Yellow;
                    _mockModel.MovedOutOfDeadzone = false;
                    _mockModel.SelectorMode = CodeInputButtonSelectorMode.Gesture;
                    _mockModel.CloseSelector();
                }
            );
        }

        [TestMethod]
        public void OnReleaseSelectWithNoGesture_ShouldTakeExpectedActions()
        {
            _mockModel.SelectorMode.Returns(CodeInputButtonSelectorMode.Select);
            _mockModel.GestureAngle.Returns((float?)null);

            ReleaseSurface();

            Received.InOrder(
                () =>
                {
                    _mockModel.MovedOutOfDeadzone = false;
                    _mockModel.SelectorMode = CodeInputButtonSelectorMode.Gesture;
                    _mockModel.CloseSelector();
                }
            );
        }

        [TestMethod]
        public void OnReleaseSelectWithNoGesture_ShouldFireExpectedSelectorClosedEvent()
        {
            _mockModel.SelectorMode.Returns(CodeInputButtonSelectorMode.Select);
            _mockModel.GestureAngle.Returns((float?)null);
            _mockModel.SelectorState.Returns(CodeInputButtonSelectorState.Closing);
            _mockModel.ScaleValue.Returns(0f);

            EventHandler<SelectorClosedEventArgs> subscriber = Substitute.For<EventHandler<SelectorClosedEventArgs>>();
            _sut.SelectorClosed += subscriber;

            ReleaseSurface();

            subscriber.Received(1)(_sut, Arg.Is<SelectorClosedEventArgs>(x => x.ValueChanged == false));
        }
        #endregion

        #region OnReleased - IndexSet Tests
        [TestMethod]
        public void OnReleaseWithGestureAngleShouldSetExpectedCodeValue_RedA()
        {
            _mockModel.SelectorMode.Returns(CodeInputButtonSelectorMode.Select);
            _mockModel.GestureAngle.Returns(MathHelper.ToRadians(288.1f));

            ReleaseSurface();

            Assert.AreEqual<CodeValue>(CodeValue.Red, _mockModel.DeviceModel.InputSequence[0]);
        }

        [TestMethod]
        public void OnReleaseWithGestureAngleShouldSetExpectedCodeValue_RedB()
        {
            _mockModel.SelectorMode.Returns(CodeInputButtonSelectorMode.Select);
            _mockModel.GestureAngle.Returns(MathHelper.ToRadians(359.9f));

            ReleaseSurface();

            Assert.AreEqual<CodeValue>(CodeValue.Red, _mockModel.DeviceModel.InputSequence[0]);
        }

        [TestMethod]
        public void OnReleaseWithGestureAngleShouldSetExpectedCodeValue_GreenA()
        {
            _mockModel.SelectorMode.Returns(CodeInputButtonSelectorMode.Select);
            _mockModel.GestureAngle.Returns(MathHelper.ToRadians(0.1f));

            ReleaseSurface();

            Assert.AreEqual<CodeValue>(CodeValue.Green, _mockModel.DeviceModel.InputSequence[0]);
        }

        [TestMethod]
        public void OnReleaseWithGestureAngleShouldSetExpectedCodeValue_GreenB()
        {
            _mockModel.SelectorMode.Returns(CodeInputButtonSelectorMode.Select);
            _mockModel.GestureAngle.Returns(MathHelper.ToRadians(71.9f));

            ReleaseSurface();

            Assert.AreEqual<CodeValue>(CodeValue.Green, _mockModel.DeviceModel.InputSequence[0]);
        }

        [TestMethod]
        public void OnReleaseWithGestureAngleShouldSetExpectedCodeValue_BlueA()
        {
            _mockModel.SelectorMode.Returns(CodeInputButtonSelectorMode.Select);
            _mockModel.GestureAngle.Returns(MathHelper.ToRadians(72.1f));

            ReleaseSurface();

            Assert.AreEqual<CodeValue>(CodeValue.Blue, _mockModel.DeviceModel.InputSequence[0]);
        }

        [TestMethod]
        public void OnReleaseWithGestureAngleShouldSetExpectedCodeValue_BlueB()
        {
            _mockModel.SelectorMode.Returns(CodeInputButtonSelectorMode.Select);
            _mockModel.GestureAngle.Returns(MathHelper.ToRadians(143.9f));

            ReleaseSurface();

            Assert.AreEqual<CodeValue>(CodeValue.Blue, _mockModel.DeviceModel.InputSequence[0]);
        }

        [TestMethod]
        public void OnReleaseWithGestureAngleShouldSetExpectedCodeValue_YellowA()
        {
            _mockModel.SelectorMode.Returns(CodeInputButtonSelectorMode.Select);
            _mockModel.GestureAngle.Returns(MathHelper.ToRadians(144.1f));

            ReleaseSurface();

            Assert.AreEqual<CodeValue>(CodeValue.Yellow, _mockModel.DeviceModel.InputSequence[0]);
        }

        [TestMethod]
        public void OnReleaseWithGestureAngleShouldSetExpectedCodeValue_YellowB()
        {
            _mockModel.SelectorMode.Returns(CodeInputButtonSelectorMode.Select);
            _mockModel.GestureAngle.Returns(MathHelper.ToRadians(215.9f));

            ReleaseSurface();

            Assert.AreEqual<CodeValue>(CodeValue.Yellow, _mockModel.DeviceModel.InputSequence[0]);
        }

        [TestMethod]
        public void OnReleaseWithGestureAngleShouldSetExpectedCodeValue_OrangeA()
        {
            _mockModel.SelectorMode.Returns(CodeInputButtonSelectorMode.Select);
            _mockModel.GestureAngle.Returns(MathHelper.ToRadians(216.1f));

            ReleaseSurface();

            Assert.AreEqual<CodeValue>(CodeValue.Orange, _mockModel.DeviceModel.InputSequence[0]);
        }

        [TestMethod]
        public void OnReleaseWithGestureAngleShouldSetExpectedCodeValue_OrangeB()
        {
            _mockModel.SelectorMode.Returns(CodeInputButtonSelectorMode.Select);
            _mockModel.GestureAngle.Returns(MathHelper.ToRadians(287.9f));

            ReleaseSurface();

            Assert.AreEqual<CodeValue>(CodeValue.Orange, _mockModel.DeviceModel.InputSequence[0]);
        }

        [TestMethod]
        public void OnReleaseWithGestureAngleShouldFireExpectedSelectorClosedEvent()
        {
            _mockModel.SelectorMode.Returns(CodeInputButtonSelectorMode.Select);
            _mockModel.GestureAngle.Returns(MathHelper.ToRadians(287.9f));
            _mockModel.SelectorState.Returns(CodeInputButtonSelectorState.Closing);
            _mockModel.ScaleValue.Returns(0f);

            EventHandler<SelectorClosedEventArgs> subscriber = Substitute.For<EventHandler<SelectorClosedEventArgs>>();
            _sut.SelectorClosed += subscriber;

            ReleaseSurface();

            subscriber.Received(1)(_sut, Arg.Is<SelectorClosedEventArgs>(x => x.ValueChanged == true));
        }
        #endregion
    }
}
