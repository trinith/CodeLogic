using ArbitraryPixel.CodeLogic.Common.Entities;
using ArbitraryPixel.CodeLogic.Common.Model;
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
    public class CodeInputButton_UpdateForInput_Tests
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

        [TestMethod]
        public void UpdateForInputShouldSetModelSelectedHighlightAngle()
        {
            _mockModel.DeviceModel.InputSequence[0].Returns(CodeValue.Red);

            _sut.Update(new GameTime());

            _mockModel.Received(1).SelectedHighlightAngle = MathHelper.ToRadians(0 * (360f / 5f) - (360f / 10f));
        }

        [TestMethod]
        public void UpdateForInputWithTouchedShouldConvertPointToWorld()
        {
            _mockEngine.InputManager.GetSurfaceState().Returns(new SurfaceState(new Vector2(11, 22), true));

            _sut.Update(new GameTime());

            // NOTE: For input manager processing, we get some from the base class. Tests accommodate for this.
            _mockEngine.ScreenManager.Received(2).PointToWorld(new Vector2(11, 22));
        }

        [TestMethod]
        public void UpdateForInputWithNotTouchedShouldNotConvertPointToWorld()
        {
            _mockEngine.InputManager.GetSurfaceState().Returns(new SurfaceState(new Vector2(11, 22), false));

            _sut.Update(new GameTime());

            // NOTE: For input manager processing, we get some from the base class. Tests accommodate for this.
            _mockEngine.ScreenManager.Received(1).PointToWorld(new Vector2(11, 22));
        }

        [TestMethod]
        public void UpdateForInputWithTouchedAndTouchPointOutsideDeadZoneShouldUpdateModelMovedOutOfDeadzone()
        {
            _mockEngine.InputManager.GetSurfaceState().Returns(new SurfaceState(new Vector2(11, 22), true));
            _mockEngine.ScreenManager.PointToWorld(new Vector2(11, 22)).Returns(_bounds.Centre + new Vector2(50, 0));

            _sut.Update(new GameTime());

            _mockModel.Received(1).MovedOutOfDeadzone = true;
        }

        [TestMethod]
        public void UpdateForInputWithTouchedAndTouchPointInsideDeadZoneShouldNotUpdateModelMovedOutOfDeadzone()
        {
            _mockEngine.InputManager.GetSurfaceState().Returns(new SurfaceState(new Vector2(11, 22), true));
            _mockEngine.ScreenManager.PointToWorld(new Vector2(11, 22)).Returns(_bounds.Centre + new Vector2(49, 0));

            _sut.Update(new GameTime());

            _mockModel.Received(0).MovedOutOfDeadzone = true;
        }
    }
}
