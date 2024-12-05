using ArbitraryPixel.CodeLogic.Common.Entities;
using ArbitraryPixel.CodeLogic.Common.Model;
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
    public class CodeInputButton_UpdateModelGestureAngle_Tests
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

            _mockModel.DeviceModel.InputSequence.Length.Returns(4);

            _sut = new CodeInputButton(_mockEngine, _bounds, _mockSpriteBatch, _mockModel, 0);

            _mockModel.SelectorState.Returns(CodeInputButtonSelectorState.Open);
        }

        /* NOTE: These test are a little involved... I'm not really a fan of this but in this case I decided it was necessary
         * since duplicating the algorithm is a little smoother than plugging numbers that are hard to understand.
         * 
         * The approach here is to build a desired angle that will be 0.1 degrees on either side of each 72 degree slice, then
         * test that GestureAngle gets set to the expected angle, which should be the midpoint of each 72 degree slice that
         * desired angle fits into.
         * 
         * Index corresponds to each slice, where 0 is Red, 1 is Green, and so on....
         * The angles themselves start at 0 degrees, which is at the top (12 o'clock position).
         * So red ranges from -72 (288) to 0, green ranges from 0 to 72, etc...
         */
        #region Helper Methods
        private float GetDesiredAngle(int index, float offset)
        {
            float inc = 360f / 5f;
            float desiredAngle = -inc + (index * inc) + offset;

            // Constrain to the 0 to 360 range.
            if (desiredAngle < 0)
                desiredAngle += 360;
            else if (desiredAngle >= 360)
                desiredAngle -= 360;

            return desiredAngle;
        }

        private float GetTarget(int index, float offset, float desiredAngle)
        {
            float inc = 360f / 5f;

            int mod = (int)(desiredAngle / inc);
            float target = mod * inc + inc / 2f;

            // Constrain to the 360 degree range.
            if (target < 0)
                target += 360;
            else if (target >= 360)
                target -= 360;

            // Spit out in radians.
            target = MathHelper.ToRadians(target);

            return target;
        }

        private void FinalizeArrangeThenAct(float desiredAngle)
        {
            // Create a direction at the desired angle, referenced from straight up.
            Vector2 dir = new Vector2(0, -1);
            dir = Vector2.Transform(dir, Matrix.CreateRotationZ(MathHelper.ToRadians(desiredAngle)));

            // Want the input manager to return that the surface is touched. Don't care about the vector.
            _mockEngine.InputManager.GetSurfaceState().Returns(new SurfaceState(Vector2.Zero, true));

            // When PointToWorld is called to transform the input to the world coordinate system, great a vector starting at the centre and pointing
            // in the direction we calculated that is longer than our deadzone (50).
            _mockEngine.ScreenManager.PointToWorld(Arg.Any<Vector2>()).Returns(_bounds.Centre + dir * 100f);

            _sut.Update(new GameTime());
        }
        #endregion

        #region Inside Deadzone Tests
        [TestMethod]
        public void UpdateModelGestureAngleWithTouchPointInsideDeadZoneShouldSetModelGestureAngleNull()
        {
            _mockEngine.InputManager.GetSurfaceState().Returns(new SurfaceState(new Vector2(11, 22), true));
            _mockEngine.ScreenManager.PointToWorld(new Vector2(11, 22)).Returns(_bounds.Centre + new Vector2(49, 0));

            _sut.Update(new GameTime());

            _mockModel.Received(1).GestureAngle = null;
        }
        #endregion

        #region Red Zone (288 to 0, index 0 to 1)
        [TestMethod]
        public void UpdateModelGestureAngleWithTouchShouldSetExpectedGestureAngleForZone_Red_Left()
        {
            int index = 0;
            float offset = 0.1f;

            float desiredAngle = GetDesiredAngle(index, offset);
            float target = GetTarget(index, offset, desiredAngle);

            FinalizeArrangeThenAct(desiredAngle);

            Assert.IsTrue(Math.Abs(target - _mockModel.GestureAngle.Value) < 0.00001);
        }

        [TestMethod]
        public void UpdateModelGestureAngleWithTouchShouldSetExpectedGestureAngleForZone_Red_Right()
        {
            int index = 1;
            float offset = -0.1f;

            float desiredAngle = GetDesiredAngle(index, offset);
            float target = GetTarget(index, offset, desiredAngle);

            FinalizeArrangeThenAct(desiredAngle);

            Assert.IsTrue(Math.Abs(target - _mockModel.GestureAngle.Value) < 0.00001);
        }
        #endregion

        #region Green Zone (0 to 72, index 1 to 2)
        [TestMethod]
        public void UpdateModelGestureAngleWithTouchShouldSetExpectedGestureAngleForZone_Green_Left()
        {
            int index = 1;
            float offset = 0.1f;

            float desiredAngle = GetDesiredAngle(index, offset);
            float target = GetTarget(index, offset, desiredAngle);

            FinalizeArrangeThenAct(desiredAngle);

            Assert.IsTrue(Math.Abs(target - _mockModel.GestureAngle.Value) < 0.00001);
        }

        [TestMethod]
        public void UpdateModelGestureAngleWithTouchShouldSetExpectedGestureAngleForZone_Green_Right()
        {
            int index = 2;
            float offset = -0.1f;

            float desiredAngle = GetDesiredAngle(index, offset);
            float target = GetTarget(index, offset, desiredAngle);

            FinalizeArrangeThenAct(desiredAngle);

            Assert.IsTrue(Math.Abs(target - _mockModel.GestureAngle.Value) < 0.00001);
        }
        #endregion

        #region Blue Zone (72 to 144, index 2 to 3)
        [TestMethod]
        public void UpdateModelGestureAngleWithTouchShouldSetExpectedGestureAngleForZone_Blue_Left()
        {
            int index = 2;
            float offset = 0.1f;

            float desiredAngle = GetDesiredAngle(index, offset);
            float target = GetTarget(index, offset, desiredAngle);

            FinalizeArrangeThenAct(desiredAngle);

            Assert.IsTrue(Math.Abs(target - _mockModel.GestureAngle.Value) < 0.00001);
        }

        [TestMethod]
        public void UpdateModelGestureAngleWithTouchShouldSetExpectedGestureAngleForZone_Blue_Right()
        {
            int index = 3;
            float offset = -0.1f;

            float desiredAngle = GetDesiredAngle(index, offset);
            float target = GetTarget(index, offset, desiredAngle);

            FinalizeArrangeThenAct(desiredAngle);

            Assert.IsTrue(Math.Abs(target - _mockModel.GestureAngle.Value) < 0.00001);
        }
        #endregion

        #region Yellow Zone (144 to 216, index 3 to 4)
        [TestMethod]
        public void UpdateModelGestureAngleWithTouchShouldSetExpectedGestureAngleForZone_Yellow_Left()
        {
            int index = 3;
            float offset = 0.1f;

            float desiredAngle = GetDesiredAngle(index, offset);
            float target = GetTarget(index, offset, desiredAngle);

            FinalizeArrangeThenAct(desiredAngle);

            Assert.IsTrue(Math.Abs(target - _mockModel.GestureAngle.Value) < 0.00001);
        }

        [TestMethod]
        public void UpdateModelGestureAngleWithTouchShouldSetExpectedGestureAngleForZone_Yellow_Right()
        {
            int index = 4;
            float offset = -0.1f;

            float desiredAngle = GetDesiredAngle(index, offset);
            float target = GetTarget(index, offset, desiredAngle);

            FinalizeArrangeThenAct(desiredAngle);

            Assert.IsTrue(Math.Abs(target - _mockModel.GestureAngle.Value) < 0.00001);
        }
        #endregion

        #region Orange Zone (216 to 288, index 4 to 0)
        [TestMethod]
        public void UpdateModelGestureAngleWithTouchShouldSetExpectedGestureAngleForZone_Orange_Left()
        {
            int index = 4;
            float offset = 0.1f;

            float desiredAngle = GetDesiredAngle(index, offset);
            float target = GetTarget(index, offset, desiredAngle);

            FinalizeArrangeThenAct(desiredAngle);

            Assert.IsTrue(Math.Abs(target - _mockModel.GestureAngle.Value) < 0.00001);
        }

        [TestMethod]
        public void UpdateModelGestureAngleWithTouchShouldSetExpectedGestureAngleForZone_Orange_Right()
        {
            int index = 0;
            float offset = -0.1f;

            float desiredAngle = GetDesiredAngle(index, offset);
            float target = GetTarget(index, offset, desiredAngle);

            FinalizeArrangeThenAct(desiredAngle);

            Assert.IsTrue(Math.Abs(target - _mockModel.GestureAngle.Value) < 0.00001);
        }
        #endregion
    }
}
