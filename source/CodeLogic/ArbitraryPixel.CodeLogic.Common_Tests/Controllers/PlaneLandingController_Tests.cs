using System;
using ArbitraryPixel.CodeLogic.Common.Controllers;
using ArbitraryPixel.Common.Drawing;
using ArbitraryPixel.Platform2D.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;
using NSubstitute;

namespace ArbitraryPixel.CodeLogic.Common_Tests.Controllers
{
    [TestClass]
    public class PlaneLandingController_Tests
    {
        private PlaneLandingController _sut;

        private IGameEntity _mockEntity;
        private SizeF _screenSize = new SizeF(800, 600);

        private Vector2 _expectedDir;

        [TestInitialize]
        public void Initialize()
        {
            _mockEntity = Substitute.For<IGameEntity>();

            float angle = 75;
            float angleRadians = MathHelper.ToRadians(angle);

            _expectedDir = new Vector2(
                (float)Math.Cos(angleRadians),
                (float)Math.Sin(angleRadians)
            );
            _expectedDir.Normalize();
        }

        private void Construct()
        {
            _sut = new PlaneLandingController(_mockEntity, _screenSize);
        }

        #region Constructor Tests
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullParameterShouldThrowException_PlaneEntity()
        {
            _sut = new PlaneLandingController(null, _screenSize);
        }
        #endregion

        #region Reset Tests
        [TestMethod]
        public void ResetShouldRestorePlaneEntityPositionToOriginalPosition()
        {
            _mockEntity.Bounds.Returns(new RectangleF(200, 100, 400, 300));
            Construct();

            _sut.Reset();

            _mockEntity.Received(1).Bounds = new RectangleF(200, 100, 400, 300);
        }
        #endregion

        #region Update Tests
        [TestMethod]
        public void UpdateWithEntityOnScreenShouldSetEntityPositionToExpectedValue()
        {
            _mockEntity.Bounds.Returns(new RectangleF(200, 100, 400, 300));
            Construct();
            float t = 0.25f;

            _sut.Update(new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(t)));

            float v = PlaneLandingController.Constants.InitialDescentVelocity;
            v += PlaneLandingController.Constants.DecentAcceleration * t;

            Vector2 newPos = new Vector2(200, 100) + _expectedDir * v * t;

            _mockEntity.Received(1).Bounds = new RectangleF(newPos, new SizeF(400, 300));
        }

        [TestMethod]
        public void UpdateAfterResetShouldSetEntityPositionToExpectedValue()
        {
            _mockEntity.Bounds.Returns(new RectangleF(200, 100, 400, 300));
            Construct();
            float t = 0.25f;

            _sut.Update(new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(t)));
            _sut.Reset();
            _mockEntity.ClearReceivedCalls();

            _sut.Update(new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(t)));

            float v = PlaneLandingController.Constants.InitialDescentVelocity;
            v += PlaneLandingController.Constants.DecentAcceleration * t;

            Vector2 newPos = new Vector2(200, 100) + _expectedDir * v * t;

            _mockEntity.Received(1).Bounds = new RectangleF(newPos, new SizeF(400, 300));
        }

        [TestMethod]
        public void UpdateWithEntityMovingOffScreenShouldTriggerLandingFinishedEvent()
        {
            _mockEntity.Bounds.Returns(new RectangleF(200, 100, 400, 300));
            Construct();
            float t = 50f;  // Should move off screen within 50 seconds :)
            var subscriber = Substitute.For<EventHandler<EventArgs>>();
            _sut.LandingFinished += subscriber;

            _sut.Update(new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(t)));

            subscriber.Received(1)(_sut, Arg.Any<EventArgs>());
        }

        [TestMethod]
        public void UpdateWithEntityNotMovingOffScreenShouldNotTriggerLandingFinishedEvent()
        {
            _mockEntity.Bounds.Returns(new RectangleF(200, 100, 400, 300));
            Construct();
            float t = 0.50f;  // Should move off screen within 50 seconds :)
            var subscriber = Substitute.For<EventHandler<EventArgs>>();
            _sut.LandingFinished += subscriber;

            _sut.Update(new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(t)));

            subscriber.Received(0)(_sut, Arg.Any<EventArgs>());
        }

        [TestMethod]
        public void UpdateWithEntityOffScreenShouldNotUpdateEntityPosition()
        {
            _mockEntity.Bounds.Returns(new RectangleF(200, 100, 400, 300));
            Construct();
            float t = 50f;  // Should move off screen within 50 seconds :)

            _sut.Update(new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(t)));
            _mockEntity.ClearReceivedCalls();

            _sut.Update(new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(t)));

            _mockEntity.Received(0).Bounds = Arg.Any<RectangleF>();
        }
        #endregion
    }
}
