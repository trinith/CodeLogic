using System;
using System.Collections.Generic;
using System.Linq;
using ArbitraryPixel.CodeLogic.Common.Controllers;
using ArbitraryPixel.CodeLogic.Common.Model;
using ArbitraryPixel.Platform2D.Animation;
using ArbitraryPixel.Platform2D.Layer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;
using NSubstitute;

namespace ArbitraryPixel.CodeLogic.Common_Tests.Controllers
{
    [TestClass]
    public class LayerFadeController_Tests
    {
        private LayerFadeController _sut;

        private IAnimationFactory<float> _mockAnimationFactory;

        [TestInitialize]
        public void Initialize()
        {
            _mockAnimationFactory = Substitute.For<IAnimationFactory<float>>();
        }

        private void Construct()
        {
            _sut = new LayerFadeController(_mockAnimationFactory);
        }

        #region Constructor Tests
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullParameterShouldThrowException_AnimationFactory()
        {
            _sut = new LayerFadeController(null);
        }

        [TestMethod]
        public void ConstructShouldCreateAnimationCollection()
        {
            Construct();

            _mockAnimationFactory.Received(1).CreateAnimationCollection();
        }

        [TestMethod]
        public void ConstructShouldCreateExpectedFadeOutAnimation()
        {
            IAnimationCollection<float> mockAnimationCollection = Substitute.For<IAnimationCollection<float>>();
            _mockAnimationFactory.CreateAnimationCollection().Returns(mockAnimationCollection);

            IValueAnimation<float> mockValueAnimation = Substitute.For<IValueAnimation<float>>();
            _mockAnimationFactory.CreateValueAnimation(1f, Arg.Is<float[]>(x => x.SequenceEqual(new float[] { 0f, 0.5f }))).Returns(mockValueAnimation);

            Construct();

            mockAnimationCollection.Received(1).Add(FadeMode.FadeOut.ToString(), mockValueAnimation);
        }

        [TestMethod]
        public void ConstructShouldCreateExpectedFadeInAnimation()
        {
            IAnimationCollection<float> mockAnimationCollection = Substitute.For<IAnimationCollection<float>>();
            _mockAnimationFactory.CreateAnimationCollection().Returns(mockAnimationCollection);

            IValueAnimation<float> mockValueAnimation = Substitute.For<IValueAnimation<float>>();
            _mockAnimationFactory.CreateValueAnimation(0f, Arg.Is<float[]>(x => x.SequenceEqual(new float[] { 1f, 0.5f }))).Returns(mockValueAnimation);

            Construct();

            mockAnimationCollection.Received(1).Add(FadeMode.FadeIn.ToString(), mockValueAnimation);
        }

        [TestMethod]
        public void ConstructWithTransitionTimeShouldCreateExpectedValueAnimation()
        {
            float expected = 1.23f;
            _sut = new LayerFadeController(_mockAnimationFactory, expected);

            Received.InOrder(
                () =>
                {
                    _mockAnimationFactory.CreateValueAnimation(1f, Arg.Is<float[]>(x => x[0] == 0f && x[1] == expected));
                    _mockAnimationFactory.CreateValueAnimation(0f, Arg.Is<float[]>(x => x[0] == 1f && x[1] == expected));
                }
            );
        }
        #endregion

        #region IsAnimating Tests
        [TestMethod]
        public void IsAnimatingAfterConstructionShouldReturnFalse()
        {
            Construct();

            Assert.IsFalse(_sut.IsAnimating);
        }

        [TestMethod]
        public void IsAnimatingAfterStartingAnimationShouldReturnTrue()
        {
            IValueAnimation<float> mockAnimation = Substitute.For<IValueAnimation<float>>();
            mockAnimation.IsComplete.Returns(true);

            IAnimationCollection<float> mockCollection = Substitute.For<IAnimationCollection<float>>();
            mockCollection[FadeMode.FadeOut.ToString()].Returns(mockAnimation);

            _mockAnimationFactory.CreateAnimationCollection().Returns(mockCollection);

            Construct();

            _sut.StartAnimation(FadeMode.FadeOut);

            Assert.IsTrue(_sut.IsAnimating);
        }

        [TestMethod]
        public void IsAnimatingAfterAnimationCompletesShouldReturnFalse()
        {
            IValueAnimation<float> mockAnimation = Substitute.For<IValueAnimation<float>>();
            mockAnimation.IsComplete.Returns(true);

            IAnimationCollection<float> mockCollection = Substitute.For<IAnimationCollection<float>>();
            mockCollection[FadeMode.FadeOut.ToString()].Returns(mockAnimation);

            _mockAnimationFactory.CreateAnimationCollection().Returns(mockCollection);

            Construct();

            _sut.StartAnimation(FadeMode.FadeOut);
            _sut.Update(new GameTime());

            Assert.IsFalse(_sut.IsAnimating);
        }

        [TestMethod]
        public void IsAnimatingAfterStartingAnimationThenResetShouldReturnFalse()
        {
            IValueAnimation<float> mockAnimation = Substitute.For<IValueAnimation<float>>();
            mockAnimation.IsComplete.Returns(true);

            IAnimationCollection<float> mockCollection = Substitute.For<IAnimationCollection<float>>();
            mockCollection[FadeMode.FadeOut.ToString()].Returns(mockAnimation);

            _mockAnimationFactory.CreateAnimationCollection().Returns(mockCollection);

            Construct();

            _sut.StartAnimation(FadeMode.FadeOut);
            _sut.Reset();

            Assert.IsFalse(_sut.IsAnimating);
        }
        #endregion

        #region IsAnimationComplete Tests
        [TestMethod]
        public void IsAnimationCompleteWithCompleteAnimationShouldReturnTrue()
        {
            IValueAnimation<float> mockAnimation = Substitute.For<IValueAnimation<float>>();
            mockAnimation.IsComplete.Returns(true);

            IAnimationCollection<float> mockCollection = Substitute.For<IAnimationCollection<float>>();
            mockCollection[FadeMode.FadeOut.ToString()].Returns(mockAnimation);

            _mockAnimationFactory.CreateAnimationCollection().Returns(mockCollection);

            Construct();

            Assert.IsTrue(_sut.IsAnimationComplete(FadeMode.FadeOut));
        }

        [TestMethod]
        public void IsAnimationCompleteWithIncompleteAnimationShouldReturnFalse()
        {
            IValueAnimation<float> mockAnimation = Substitute.For<IValueAnimation<float>>();
            mockAnimation.IsComplete.Returns(false);

            IAnimationCollection<float> mockCollection = Substitute.For<IAnimationCollection<float>>();
            mockCollection[FadeMode.FadeOut.ToString()].Returns(mockAnimation);

            _mockAnimationFactory.CreateAnimationCollection().Returns(mockCollection);

            Construct();

            Assert.IsFalse(_sut.IsAnimationComplete(FadeMode.FadeOut));
        }
        #endregion

        #region AddLayer Tests
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddLayerWithNullParameterShouldThrowException()
        {
            Construct();

            _sut.AddLayer(null);
        }
        #endregion

        #region Reset Tests
        [TestMethod]
        public void ResetShouldCallResetOnAllAnimations()
        {
            IValueAnimation<float> mockAnimationA = Substitute.For<IValueAnimation<float>>();
            IValueAnimation<float> mockAnimationB = Substitute.For<IValueAnimation<float>>();
            IAnimationCollection<float> mockAnimationCollection = Substitute.For<IAnimationCollection<float>>();
            mockAnimationCollection.GetEnumerator().Returns(new List<IValueAnimation<float>>(new IValueAnimation<float>[] { mockAnimationA, mockAnimationB }).GetEnumerator());

            _mockAnimationFactory.CreateAnimationCollection().Returns(mockAnimationCollection);

            Construct();

            _sut.Reset();

            Received.InOrder(
                () =>
                {
                    mockAnimationA.Reset();
                    mockAnimationB.Reset();
                }
            );
        }

        [TestMethod]
        public void ResetShouldSetLayerPropertiesAccordingToFadeInAnimation_TestA()
        {
            IAnimationCollection<float> mockAnimationCollection = Substitute.For<IAnimationCollection<float>>();
            _mockAnimationFactory.CreateAnimationCollection().Returns(mockAnimationCollection);

            IValueAnimation<float> mockAnimation = Substitute.For<IValueAnimation<float>>();
            mockAnimationCollection[FadeMode.FadeIn.ToString()].Returns(mockAnimation);

            mockAnimation.Value.Returns(0.123f);
            mockAnimation.IsComplete.Returns(false);

            Construct();

            ILayer mockLayer = Substitute.For<ILayer>();
            _sut.AddLayer(mockLayer);

            _sut.Reset();

            Received.InOrder(
                () =>
                {
                    mockLayer.MainSpriteBatch.Opacity = 0.123f;
                    mockLayer.Enabled = false;
                }
            );
        }

        [TestMethod]
        public void ResetShouldSetLayerPropertiesAccordingToFadeInAnimation_TestB()
        {
            IAnimationCollection<float> mockAnimationCollection = Substitute.For<IAnimationCollection<float>>();
            _mockAnimationFactory.CreateAnimationCollection().Returns(mockAnimationCollection);

            IValueAnimation<float> mockAnimation = Substitute.For<IValueAnimation<float>>();
            mockAnimationCollection[FadeMode.FadeIn.ToString()].Returns(mockAnimation);

            mockAnimation.Value.Returns(0.75f);
            mockAnimation.IsComplete.Returns(true);

            Construct();

            ILayer mockLayer = Substitute.For<ILayer>();
            _sut.AddLayer(mockLayer);

            _sut.Reset();

            Received.InOrder(
                () =>
                {
                    mockLayer.MainSpriteBatch.Opacity = 0.75f;
                    mockLayer.Enabled = true;
                }
            );
        }
        #endregion

        #region Update Tests
        [TestMethod]
        public void UpdateWhenAnimationNotStartedShouldNotUpdateLayerOpacity()
        {
            Construct();

            ILayer mockLayer = Substitute.For<ILayer>();
            _sut.AddLayer(mockLayer);

            _sut.Update(new GameTime());

            mockLayer.MainSpriteBatch.Received(0).Opacity = Arg.Any<float>();
        }

        [TestMethod]
        public void UpdateWhenAnimationnotStartedShouldNotUpdateLayerEnabled()
        {
            Construct();

            ILayer mockLayer = Substitute.For<ILayer>();
            _sut.AddLayer(mockLayer);

            _sut.Update(new GameTime());

            mockLayer.Received(0).Enabled = Arg.Any<bool>();
        }

        [TestMethod]
        public void UpdateWhenAnimationStartedShouldUpdateStartedAnimation()
        {
            IValueAnimation<float> mockAnimation = Substitute.For<IValueAnimation<float>>();
            IAnimationCollection<float> mockCollection = Substitute.For<IAnimationCollection<float>>();
            mockCollection[FadeMode.FadeOut.ToString()].Returns(mockAnimation);
            _mockAnimationFactory.CreateAnimationCollection().Returns(mockCollection);

            Construct();

            ILayer mockLayer = Substitute.For<ILayer>();
            _sut.AddLayer(mockLayer);

            _sut.StartAnimation(FadeMode.FadeOut);

            GameTime gameTime = new GameTime();
            _sut.Update(gameTime);

            mockAnimation.Received(1).Update(gameTime);
        }

        [TestMethod]
        public void UpdateWhenAnimationStartedShouldSetLayerProperties()
        {
            IValueAnimation<float> mockAnimation = Substitute.For<IValueAnimation<float>>();
            mockAnimation.Value.Returns(0.123f);
            mockAnimation.IsComplete.Returns(false);

            IAnimationCollection<float> mockCollection = Substitute.For<IAnimationCollection<float>>();
            mockCollection[FadeMode.FadeOut.ToString()].Returns(mockAnimation);
            _mockAnimationFactory.CreateAnimationCollection().Returns(mockCollection);

            Construct();

            ILayer mockLayer = Substitute.For<ILayer>();
            _sut.AddLayer(mockLayer);

            _sut.StartAnimation(FadeMode.FadeOut);
            mockLayer.ClearReceivedCalls();
            mockLayer.MainSpriteBatch.ClearReceivedCalls();

            GameTime gameTime = new GameTime();
            _sut.Update(gameTime);

            Received.InOrder(
                () =>
                {
                    mockLayer.MainSpriteBatch.Opacity = 0.123f;
                    mockLayer.Enabled = false;
                }
            );
        }

        [TestMethod]
        public void UpdateWhenAnimationStartedAndAnimationCompleteShouldFinishAnimation()
        {
            bool animationComplete = false;
            IValueAnimation<float> mockAnimation = Substitute.For<IValueAnimation<float>>();
            mockAnimation.Value.Returns(0.123f);
            mockAnimation.IsComplete.Returns(x => animationComplete);
            mockAnimation.When(x => x.Update(Arg.Any<GameTime>())).Do(x => animationComplete = true);

            IAnimationCollection<float> mockCollection = Substitute.For<IAnimationCollection<float>>();
            mockCollection[FadeMode.FadeOut.ToString()].Returns(mockAnimation);
            _mockAnimationFactory.CreateAnimationCollection().Returns(mockCollection);

            Construct();

            ILayer mockLayer = Substitute.For<ILayer>();
            _sut.AddLayer(mockLayer);

            _sut.StartAnimation(FadeMode.FadeOut);
            mockLayer.ClearReceivedCalls();
            mockLayer.MainSpriteBatch.ClearReceivedCalls();

            GameTime gameTime = new GameTime();
            _sut.Update(gameTime);

            Assert.IsFalse(_sut.IsAnimating);
        }
        #endregion

        #region StartAnimation Tests
        [TestMethod]
        public void StartAnimationShouldCallResetOnStartedAnimation_FadeOut()
        {
            FadeMode targetMode = FadeMode.FadeOut;

            IValueAnimation<float> mockAnimation = Substitute.For<IValueAnimation<float>>();
            IAnimationCollection<float> mockCollection = Substitute.For<IAnimationCollection<float>>();
            _mockAnimationFactory.CreateAnimationCollection().Returns(mockCollection);

            mockCollection[targetMode.ToString()].Returns(mockAnimation);

            Construct();

            _sut.StartAnimation(targetMode);

            mockAnimation.Received(1).Reset();
        }

        [TestMethod]
        public void StartAnimationShouldCallResetOnStartedAnimation_FadeIn()
        {
            FadeMode targetMode = FadeMode.FadeIn;

            IValueAnimation<float> mockAnimation = Substitute.For<IValueAnimation<float>>();
            IAnimationCollection<float> mockCollection = Substitute.For<IAnimationCollection<float>>();
            _mockAnimationFactory.CreateAnimationCollection().Returns(mockCollection);

            mockCollection[targetMode.ToString()].Returns(mockAnimation);

            Construct();

            _sut.StartAnimation(targetMode);

            mockAnimation.Received(1).Reset();
        }

        [TestMethod]
        public void StartAnimationShouldSetLayerPropertiesBasedOnCurrentAnimation()
        {
            FadeMode targetMode = FadeMode.FadeIn;

            IValueAnimation<float> mockAnimation = Substitute.For<IValueAnimation<float>>();
            mockAnimation.Value.Returns(0.555f);
            mockAnimation.IsComplete.Returns(true);

            IAnimationCollection<float> mockCollection = Substitute.For<IAnimationCollection<float>>();
            _mockAnimationFactory.CreateAnimationCollection().Returns(mockCollection);

            mockCollection[targetMode.ToString()].Returns(mockAnimation);

            Construct();

            ILayer mockLayer = Substitute.For<ILayer>();
            _sut.AddLayer(mockLayer);

            _sut.StartAnimation(targetMode);

            Received.InOrder(
                () =>
                {
                    mockLayer.MainSpriteBatch.Opacity = 0.555f;
                    mockLayer.Enabled = true;
                }
            );
        }
        #endregion
    }
}
