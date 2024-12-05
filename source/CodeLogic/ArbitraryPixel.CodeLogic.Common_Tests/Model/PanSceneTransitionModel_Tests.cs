using ArbitraryPixel.CodeLogic.Common.Model;
using ArbitraryPixel.Common.Drawing;
using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Platform2D.Scene;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;
using NSubstitute;
using System;

namespace ArbitraryPixel.CodeLogic.Common_Tests.Model
{
    [TestClass]
    public class PanSceneTransitionModel_Tests
    {
        private IScene _mockStartScene;
        private IScene _mockEndScene;
        private IRenderTarget2D _mockStartTarget;
        private IRenderTarget2D _mockEndTarget;

        private PanSceneTransitionModel _sut;

        [TestInitialize]
        public void Initialize()
        {
            _mockStartScene = Substitute.For<IScene>();
            _mockEndScene = Substitute.For<IScene>();
            _mockStartTarget = Substitute.For<IRenderTarget2D>();
            _mockEndTarget = Substitute.For<IRenderTarget2D>();
        }

        private void ConstructSUT(PanSceneTransitionMode transitionMode)
        {
            _sut = new PanSceneTransitionModel(_mockStartScene, _mockEndScene, _mockStartTarget, _mockEndTarget, transitionMode, 0.5, new SizeF(1000, 750));
        }

        #region Constructor Tests
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullParameterShouldThrowException_StartScene()
        {
            _sut = new PanSceneTransitionModel(null, _mockEndScene, _mockStartTarget, _mockEndTarget, PanSceneTransitionMode.PanLeft, 0.5, new SizeF(1000, 750));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullParameterShouldThrowException_EndScene()
        {
            _sut = new PanSceneTransitionModel(_mockStartScene, null, _mockStartTarget, _mockEndTarget, PanSceneTransitionMode.PanLeft, 0.5, new SizeF(1000, 750));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullParameterShouldThrowException_StartTarget()
        {
            _sut = new PanSceneTransitionModel(_mockStartScene, _mockEndScene, null, _mockEndTarget, PanSceneTransitionMode.PanLeft, 0.5, new SizeF(1000, 750));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullParameterShouldThrowException_EndTarget()
        {
            _sut = new PanSceneTransitionModel(_mockStartScene, _mockEndScene, _mockStartTarget, null, PanSceneTransitionMode.PanLeft, 0.5, new SizeF(1000, 750));
        }

        [TestMethod]
        public void ConstructShouldSetExpectedTransitionVelocity_PanLeft()
        {
            ConstructSUT(PanSceneTransitionMode.PanLeft);

            Assert.AreEqual<Vector2>(new Vector2(-2000, 0), _sut.TransitionVelocity);
        }

        [TestMethod]
        public void ConstructShouldSetExpectedTransitionVelocity_PanRight()
        {
            ConstructSUT(PanSceneTransitionMode.PanRight);

            Assert.AreEqual<Vector2>(new Vector2(2000, 0), _sut.TransitionVelocity);
        }

        [TestMethod]
        public void ConstructShouldSetExpectedStartAnchor()
        {
            ConstructSUT(PanSceneTransitionMode.PanLeft);

            Assert.AreEqual<Vector2>(Vector2.Zero, _sut.StartAnchor);
        }

        [TestMethod]
        public void ConstructShouldSetExpectedEndAnchor_PanLeft()
        {
            ConstructSUT(PanSceneTransitionMode.PanLeft);

            Assert.AreEqual<Vector2>(new Vector2(1000, 0), _sut.EndAnchor);
        }

        [TestMethod]
        public void ConstructShouldSetExpectedEndAnchor_PanRight()
        {
            ConstructSUT(PanSceneTransitionMode.PanRight);

            Assert.AreEqual<Vector2>(new Vector2(-1000, 0), _sut.EndAnchor);
        }

        [TestMethod]
        public void ConstructShouldSetTransitionCompleteFalse()
        {
            ConstructSUT(PanSceneTransitionMode.PanLeft);

            Assert.IsFalse(_sut.TransitionComplete);
        }
        #endregion

        #region Update Tests
        [TestMethod]
        public void UpdateShouldUpdateStartAnchor()
        {
            ConstructSUT(PanSceneTransitionMode.PanLeft);
            _sut.Update(new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(0.1)));

            Assert.AreEqual<Vector2>(new Vector2(-200, 0), _sut.StartAnchor);
        }

        [TestMethod]
        public void UpdateShouldUpdateEndAnchor()
        {
            ConstructSUT(PanSceneTransitionMode.PanLeft);
            _sut.Update(new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(0.1)));

            Assert.AreEqual<Vector2>(new Vector2(800, 0), _sut.EndAnchor);
        }

        [TestMethod]
        public void UpdateWhenEndAnchorPastZeroShouldZeroTransitionVelocity()
        {
            ConstructSUT(PanSceneTransitionMode.PanLeft);
            _sut.Update(new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(0.5)));

            Assert.AreEqual<Vector2>(Vector2.Zero, _sut.TransitionVelocity);
        }

        [TestMethod]
        public void UpdateWhenEndAnchorPastZeroShouldZeroEndAnchor()
        {
            ConstructSUT(PanSceneTransitionMode.PanLeft);
            _sut.Update(new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(1)));

            Assert.AreEqual<Vector2>(Vector2.Zero, _sut.EndAnchor);
        }

        [TestMethod]
        public void UpdateWhenEndAnchorPastZeroShouldSetTransitionCompleteFalse()
        {
            ConstructSUT(PanSceneTransitionMode.PanLeft);
            _sut.Update(new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(0.5)));

            Assert.IsTrue(_sut.TransitionComplete);
        }
        #endregion
    }
}
