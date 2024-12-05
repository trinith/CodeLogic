using System;
using ArbitraryPixel.CodeLogic.Common.Controllers;
using ArbitraryPixel.CodeLogic.Common.Entities;
using ArbitraryPixel.Platform2D.Animation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;
using NSubstitute;

namespace ArbitraryPixel.CodeLogic.Common_Tests.Controllers
{
    [TestClass]
    public class CloudIntensityController_Tests
    {
        private CloudIntensityController _sut;

        private ICloud _mockCloud;
        private IValueAnimation<float> _mockAnimation;

        [TestInitialize]
        public void Initialize()
        {
            _mockCloud = Substitute.For<ICloud>();
            _mockAnimation = Substitute.For<IValueAnimation<float>>();
        }

        private void Construct()
        {
            _sut = new CloudIntensityController(_mockCloud, _mockAnimation);
        }

        #region Constructor Tests
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullParameterShouldThrowException_Cloud()
        {
            _sut = new CloudIntensityController(null, _mockAnimation);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullParameterShouldThrowException_IntensityAnimator()
        {
            _sut = new CloudIntensityController(_mockCloud, null);
        }

        [TestMethod]
        public void ConstructShouldSetCloudIntensityToAnimatorValue()
        {
            _mockAnimation.Value.Returns(0.123f);

            Construct();

            _mockCloud.Received(1).Intensity = 0.123f;
        }
        #endregion

        #region Property Tests
        [TestMethod]
        public void IsCompleteShouldReturnAnimationIsComplete_TestA()
        {
            _mockAnimation.IsComplete.Returns(false);

            Construct();

            Assert.IsFalse(_sut.IsComplete);
        }

        [TestMethod]
        public void IsCompleteShouldReturnAnimationIsComplete_TestB()
        {
            _mockAnimation.IsComplete.Returns(true);

            Construct();

            Assert.IsTrue(_sut.IsComplete);
        }
        #endregion

        #region Reset Tests
        [TestMethod]
        public void ResetShouldCallResetOnAnimator()
        {
            Construct();

            _sut.Reset();

            _mockAnimation.Received(1).Reset();
        }

        [TestMethod]
        public void ResetShouldSetCloudIntensityToAnimatorValue()
        {
            Construct();

            _mockAnimation.Value.Returns(0.321f);
            _sut.Reset();

            _mockCloud.Received(1).Intensity = 0.321f;
        }
        #endregion

        #region Update Tests
        [TestMethod]
        public void UpdateWithIncompleteAnimatorShouldCallAnimatorUpdate()
        {
            Construct();
            GameTime expected = new GameTime();
            _mockAnimation.IsComplete.Returns(false);

            _sut.Update(expected);

            _mockAnimation.Received(1).Update(expected);
        }

        [TestMethod]
        public void UpdateWithCompleteAnimatorShouldNotCallAnimatorUpdate()
        {
            Construct();
            GameTime expected = new GameTime();
            _mockAnimation.IsComplete.Returns(true);

            _sut.Update(expected);

            _mockAnimation.Received(0).Update(expected);
        }

        [TestMethod]
        public void UpdateShouldSetCloudIntensityToAnimatorValue()
        {
            Construct();
            GameTime expected = new GameTime();
            _mockAnimation.Value.Returns(0.567f);

            _sut.Update(expected);

            _mockCloud.Received(1).Intensity = 0.567f;
        }
        #endregion
    }
}
