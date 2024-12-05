using System;
using ArbitraryPixel.CodeLogic.Common.Model;
using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Platform2D.Scene;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;
using NSubstitute;

namespace ArbitraryPixel.CodeLogic.Common_Tests.Model
{
    [TestClass]
    public class FadeSceneTransitionModel_Tests
    {
        private FadeSceneTransitionModel _sut;
        private IScene _mockStartScene;
        private IScene _mockEndScene;
        private IRenderTarget2D _mockStartTarget;
        private IRenderTarget2D _mockEndTarget;

        [TestInitialize]
        public void Initialize()
        {
            _mockStartScene = Substitute.For<IScene>();
            _mockEndScene = Substitute.For<IScene>();
            _mockStartTarget = Substitute.For<IRenderTarget2D>();
            _mockEndTarget = Substitute.For<IRenderTarget2D>();
        }

        private void Construct(FadeSceneTransitionMode mode)
        {
            _sut = new FadeSceneTransitionModel(_mockStartScene, _mockEndScene, _mockStartTarget, _mockEndTarget, mode, 0.5f);
        }

        #region Constructor Tests
        #region Mode = Out
        [TestMethod]
        public void ConstructWithModeShouldSetExpectedCurrentOpacity_Out()
        {
            Construct(FadeSceneTransitionMode.Out);

            Assert.AreEqual<float>(-1, _sut.CurrentOpacity);
        }

        [TestMethod]
        public void ConstructWithModeShouldSetExpectedOpacityTarget_Out()
        {
            Construct(FadeSceneTransitionMode.Out);

            Assert.AreEqual<float>(0, _sut.OpacityTarget);
        }

        [TestMethod]
        public void ConstructWithModeShouldSetExpectedOpacityVelocity_Out()
        {
            Construct(FadeSceneTransitionMode.Out);

            Assert.AreEqual<float>(2, _sut.OpacityVelocity);
        }
        #endregion

        #region Mode = In
        [TestMethod]
        public void ConstructWithModeShouldSetExpectedCurrentOpacity_In()
        {
            Construct(FadeSceneTransitionMode.In);

            Assert.AreEqual<float>(0, _sut.CurrentOpacity);
        }

        [TestMethod]
        public void ConstructWithModeShouldSetExpectedOpacityTarget_In()
        {
            Construct(FadeSceneTransitionMode.In);

            Assert.AreEqual<float>(1, _sut.OpacityTarget);
        }

        [TestMethod]
        public void ConstructWithModeShouldSetExpectedOpacityVelocity_In()
        {
            Construct(FadeSceneTransitionMode.In);

            Assert.AreEqual<float>(2, _sut.OpacityVelocity);
        }
        #endregion

        #region Mode = OutIn
        [TestMethod]
        public void ConstructWithModeShouldSetExpectedCurrentOpacity_OutIn()
        {
            Construct(FadeSceneTransitionMode.OutIn);

            Assert.AreEqual<float>(-1, _sut.CurrentOpacity);
        }

        [TestMethod]
        public void ConstructWithModeShouldSetExpectedOpacityTarget_OutIn()
        {
            Construct(FadeSceneTransitionMode.OutIn);

            Assert.AreEqual<float>(1, _sut.OpacityTarget);
        }

        [TestMethod]
        public void ConstructWithModeShouldSetExpectedOpacityVelocity_OutIn()
        {
            Construct(FadeSceneTransitionMode.OutIn);

            Assert.AreEqual<float>(4, _sut.OpacityVelocity);
        }
        #endregion
        #endregion

        #region Property Tests
        [TestMethod]
        public void PropertyShouldHaveExpectedDefaultValue_Background()
        {
            Construct(FadeSceneTransitionMode.In);
            Assert.AreEqual<Color>(Color.Black, _sut.Background);
        }

        [TestMethod]
        public void PropertyShouldReturnSetValue_Background()
        {
            Construct(FadeSceneTransitionMode.In);

            _sut.Background = Color.Pink;

            Assert.AreEqual<Color>(Color.Pink, _sut.Background);
        }
        #endregion

        #region Update Tests
        [TestMethod]
        public void UpdateWithGameTimeBeforeTransitionTimeShouldUpdateCurrentOpacityToExpected()
        {
            Construct(FadeSceneTransitionMode.OutIn);

            _sut.Update(new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(0.4f)));

            Assert.AreEqual<float>(0.6f, _sut.CurrentOpacity);
        }

        [TestMethod]
        public void UpdateWithGameTimeAtTransitionTimeShouldUpdateVelocityToZero()
        {
            Construct(FadeSceneTransitionMode.OutIn);

            _sut.Update(new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(0.5f)));

            Assert.AreEqual<float>(0f, _sut.OpacityVelocity);
        }

        [TestMethod]
        public void UpdateWithGameTimeAtTransitionTimeShouldSetCurrentOpacityToTarget()
        {
            Construct(FadeSceneTransitionMode.OutIn);

            _sut.Update(new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(0.5f)));

            Assert.AreEqual<float>(1f, _sut.CurrentOpacity);
        }

        [TestMethod]
        public void UpdateWithGameTimeAtTransitionTimeShouldSetTransitionComplete()
        {
            Construct(FadeSceneTransitionMode.OutIn);

            _sut.Update(new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(0.5f)));

            Assert.AreEqual<bool>(true, _sut.TransitionComplete);
        }

        [TestMethod]
        public void UpdateAfterTransitionTimeShouldNotUpdateCurrentOpacity()
        {
            Construct(FadeSceneTransitionMode.OutIn);

            _sut.Update(new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(0.5f)));
            _sut.Update(new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(0.5f)));

            Assert.AreEqual<float>(1f, _sut.CurrentOpacity);
        }
        #endregion

        #region Scene Opacity Property Tests
        [TestMethod]
        public void StartSceneOpacityWhenCurrentOpacityLessThanZeroShouldReturnExpectedValue_TestA()
        {
            Construct(FadeSceneTransitionMode.OutIn);

            _sut.Update(new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(0.1f)));

            Assert.That.AlmostEqual(0.6f, _sut.StartSceneOpacity);
        }

        [TestMethod]
        public void StartSceneOpacityWhenCurrentOpacityLessThanZeroShouldReturnExpectedValue_TestB()
        {
            Construct(FadeSceneTransitionMode.OutIn);

            _sut.Update(new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(0.2f)));

            Assert.That.AlmostEqual(0.2f, _sut.StartSceneOpacity);
        }

        [TestMethod]
        public void EndSceneOpacityWhenCurrentOpacityLessThanZeroShouldReturnExpectedValue()
        {
            Construct(FadeSceneTransitionMode.OutIn);

            _sut.Update(new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(0.1f)));

            Assert.AreEqual<float>(0f, _sut.EndSceneOpacity);

        }

        [TestMethod]
        public void EndSceneOpacityWhenCurrentOpacityGreaterThanZeroShouldReturnExpectedValue_TestA()
        {
            Construct(FadeSceneTransitionMode.OutIn);

            _sut.Update(new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(0.3f)));

            Assert.That.AlmostEqual(0.2f, _sut.EndSceneOpacity);
        }

        [TestMethod]
        public void EndSceneOpacityWhenCurrentOpacityGreaterThanZeroShouldReturnExpectedValue_TestB()
        {
            Construct(FadeSceneTransitionMode.OutIn);

            _sut.Update(new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(0.4f)));

            Assert.That.AlmostEqual(0.6f, _sut.EndSceneOpacity);
        }

        [TestMethod]
        public void StartSceneOpacityWhenCurrentOpacityGreaterThanZeroShouldReturnExpectedValue()
        {
            Construct(FadeSceneTransitionMode.OutIn);

            _sut.Update(new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(0.3f)));

            Assert.AreEqual<float>(0f, _sut.StartSceneOpacity);
        }
        #endregion
    }
}
