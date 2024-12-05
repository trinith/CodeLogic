using System;
using System.Linq;
using ArbitraryPixel.CodeLogic.Common.Entities;
using ArbitraryPixel.CodeLogic.Common.Model;
using ArbitraryPixel.Common.Drawing;
using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Platform2D.Animation;
using ArbitraryPixel.Platform2D.Engine;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;
using NSubstitute;

namespace ArbitraryPixel.CodeLogic.Common_Tests.Entities
{
    [TestClass]
    public class ScreenFadeOverlay_Tests
    {
        private ScreenFadeOverlay _sut;

        private IEngine _mockEngine;
        private ISpriteBatch _mockSpriteBatch;
        private ITexture2D _mockTexture;
        private IAnimationFactory<float> _mockAnimationFactory;

        [TestInitialize]
        public void Initialize()
        {
            _mockEngine = Substitute.For<IEngine>();
            _mockSpriteBatch = Substitute.For<ISpriteBatch>();
            _mockTexture = Substitute.For<ITexture2D>();
            _mockAnimationFactory = Substitute.For<IAnimationFactory<float>>();

            _mockEngine.ScreenManager.World.Returns(new Point(1000, 800));
        }

        private void Construct()
        {
            _sut = new ScreenFadeOverlay(_mockEngine, _mockSpriteBatch, _mockTexture, _mockAnimationFactory);
        }

        #region Constructor Tests
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullParameterShouldThrowException_SpriteBatch()
        {
            _sut = new ScreenFadeOverlay(_mockEngine, null, _mockTexture, _mockAnimationFactory);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullParameterShouldThrowException_OverlayTexture()
        {
            _sut = new ScreenFadeOverlay(_mockEngine, _mockSpriteBatch, null, _mockAnimationFactory);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullParameterShouldThrowException_AnimationFactory()
        {
            _sut = new ScreenFadeOverlay(_mockEngine, _mockSpriteBatch, _mockTexture, null);
        }

        [TestMethod]
        public void ConstructShouldCreateAnimationCollection()
        {
            Construct();

            _mockAnimationFactory.Received(1).CreateAnimationCollection();
        }

        [TestMethod]
        public void ConstructShouldCreateFadeInAnimation()
        {
            IAnimationCollection<float> mockCollection = Substitute.For<IAnimationCollection<float>>();
            _mockAnimationFactory.CreateAnimationCollection().Returns(mockCollection);

            IValueAnimation<float> mockAnimation = Substitute.For<IValueAnimation<float>>();
            _mockAnimationFactory.CreateValueAnimation(1f, Arg.Is<float[]>(x => x.SequenceEqual(new float[] { 1f, 1f, 0f, 0.5f }))).Returns(mockAnimation);

            Construct();

            mockCollection.Received(1).Add("FadeIn", mockAnimation);
        }

        [TestMethod]
        public void ConstructShouldCreateFadeOutAnimation()
        {
            IAnimationCollection<float> mockCollection = Substitute.For<IAnimationCollection<float>>();
            _mockAnimationFactory.CreateAnimationCollection().Returns(mockCollection);

            IValueAnimation<float> mockAnimation = Substitute.For<IValueAnimation<float>>();
            _mockAnimationFactory.CreateValueAnimation(0f, Arg.Is<float[]>(x => x.SequenceEqual(new float[] { 1f, 0.5f }))).Returns(mockAnimation);

            Construct();

            mockCollection.Received(1).Add("FadeOut", mockAnimation);
        }

        [TestMethod]
        public void ConstructShouldSetBoundsToExpectedValue()
        {
            Construct();

            Assert.AreEqual<RectangleF>(new RectangleF(0, 0, 1000, 800), _sut.Bounds);
        }
        #endregion

        #region Property Tests
        [TestMethod]
        public void PropertyShouldHaveExpectedDefaultValue_CurrentMode()
        {
            Construct();

            Assert.AreEqual<FadeMode>(FadeMode.FadeIn, _sut.CurrentMode);
        }

        [TestMethod]
        public void PropertyShouldHaveExpectedDefaultValue_OpaqueColour()
        {
            Construct();

            Assert.AreEqual<Color>(Color.Black, _sut.OpaqueColour);
        }

        [TestMethod]
        public void CurrentModeSetShouldEnableExpectedAnimation_FadeIn()
        {
            IAnimationCollection<float> mockCollection = Substitute.For<IAnimationCollection<float>>();
            _mockAnimationFactory.CreateAnimationCollection().Returns(mockCollection);

            IValueAnimation<float> mockAnimation = Substitute.For<IValueAnimation<float>>();
            mockCollection["FadeOut"].Returns(mockAnimation);

            Construct();

            _sut.CurrentMode = FadeMode.FadeOut;

            _sut.Update(new GameTime());

            mockAnimation.Received(1).Update(Arg.Any<GameTime>());
        }

        [TestMethod]
        public void CurrentModeSetShouldEnableExpectedAnimation_FadeOut()
        {
            IAnimationCollection<float> mockCollection = Substitute.For<IAnimationCollection<float>>();
            _mockAnimationFactory.CreateAnimationCollection().Returns(mockCollection);

            IValueAnimation<float> mockAnimation = Substitute.For<IValueAnimation<float>>();
            mockCollection["FadeIn"].Returns(mockAnimation);

            Construct();

            _sut.CurrentMode = FadeMode.FadeIn;

            _sut.Update(new GameTime());

            mockAnimation.Received(1).Update(Arg.Any<GameTime>());
        }

        [TestMethod]
        public void OpaqueColourSetShouldReturnExpectedValue()
        {
            Construct();

            _sut.OpaqueColour = Color.Pink;

            Assert.AreEqual<Color>(Color.Pink, _sut.OpaqueColour);
        }

        [TestMethod]
        public void OpaqueColourSetShouldReflectInDrawParameters()
        {
            IAnimationCollection<float> mockCollection = Substitute.For<IAnimationCollection<float>>();
            _mockAnimationFactory.CreateAnimationCollection().Returns(mockCollection);

            IValueAnimation<float> mockAnimation = Substitute.For<IValueAnimation<float>>();
            mockAnimation.Value.Returns(0.5f);
            mockCollection["FadeIn"].Returns(mockAnimation);

            Construct();

            _sut.OpaqueColour = Color.Pink;
            _sut.Draw(new GameTime());

            _mockSpriteBatch.Received(1).Draw(Arg.Any<ITexture2D>(), Arg.Any<RectangleF>(), Color.Pink * 0.5f);
        }

        [TestMethod]
        public void CurrentValueShouldReturnCurrentAnimationValue_TestA()
        {
            IAnimationCollection<float> mockCollection = Substitute.For<IAnimationCollection<float>>();
            _mockAnimationFactory.CreateAnimationCollection().Returns(mockCollection);

            IValueAnimation<float> mockFadeIn = Substitute.For<IValueAnimation<float>>();
            mockFadeIn.Value.Returns(0.75f);
            mockCollection["FadeIn"].Returns(mockFadeIn);

            IValueAnimation<float> mockFadeOut = Substitute.For<IValueAnimation<float>>();
            mockFadeOut.Value.Returns(0.5f);
            mockCollection["FadeOut"].Returns(mockFadeOut);

            Construct();

            Assert.AreEqual<float>(0.75f, _sut.CurrentValue);
        }

        [TestMethod]
        public void CurrentValueShouldReturnCurrentAnimationValue_TestB()
        {
            IAnimationCollection<float> mockCollection = Substitute.For<IAnimationCollection<float>>();
            _mockAnimationFactory.CreateAnimationCollection().Returns(mockCollection);

            IValueAnimation<float> mockFadeIn = Substitute.For<IValueAnimation<float>>();
            mockFadeIn.Value.Returns(0.75f);
            mockCollection["FadeIn"].Returns(mockFadeIn);

            IValueAnimation<float> mockFadeOut = Substitute.For<IValueAnimation<float>>();
            mockFadeOut.Value.Returns(0.5f);
            mockCollection["FadeOut"].Returns(mockFadeOut);

            Construct();
            _sut.CurrentMode = FadeMode.FadeOut;

            Assert.AreEqual<float>(0.5f, _sut.CurrentValue);
        }
        #endregion

        #region Update Tests
        [TestMethod]
        public void UpdateWithIncompleteAnimationShouldCallAnimationUpdate()
        {
            IAnimationCollection<float> mockCollection = Substitute.For<IAnimationCollection<float>>();
            _mockAnimationFactory.CreateAnimationCollection().Returns(mockCollection);

            IValueAnimation<float> mockAnimation = Substitute.For<IValueAnimation<float>>();
            mockAnimation.IsComplete.Returns(false);
            mockCollection["FadeIn"].Returns(mockAnimation);

            Construct();

            GameTime gameTime = new GameTime();
            _sut.Update(gameTime);

            mockAnimation.Received(1).Update(gameTime);
        }

        [TestMethod]
        public void UpdateWithCompleteAnimationShouldNotCallAnimationUpdate()
        {
            IAnimationCollection<float> mockCollection = Substitute.For<IAnimationCollection<float>>();
            _mockAnimationFactory.CreateAnimationCollection().Returns(mockCollection);

            IValueAnimation<float> mockAnimation = Substitute.For<IValueAnimation<float>>();
            mockAnimation.IsComplete.Returns(true);
            mockCollection["FadeIn"].Returns(mockAnimation);

            Construct();

            GameTime gameTime = new GameTime();
            _sut.Update(gameTime);

            mockAnimation.Received(0).Update(Arg.Any<GameTime>());
        }

        [TestMethod]
        public void UpdateWhenMovingToCompleteShouldFireAnimationFinishedEvent()
        {
            IAnimationCollection<float> mockCollection = Substitute.For<IAnimationCollection<float>>();
            _mockAnimationFactory.CreateAnimationCollection().Returns(mockCollection);

            bool isComplete = false;
            IValueAnimation<float> mockAnimation = Substitute.For<IValueAnimation<float>>();
            mockAnimation.IsComplete.Returns(x => isComplete);
            mockAnimation.When(x => x.Update(Arg.Any<GameTime>())).Do(x => isComplete = true);
            mockCollection["FadeIn"].Returns(mockAnimation);

            Construct();

            var sub = Substitute.For<EventHandler<EventArgs>>();
            _sut.AnimationFinished += sub;

            _sut.Update(new GameTime());

            sub.Received(1)(_sut, Arg.Any<EventArgs>());
        }

        [TestMethod]
        public void UpdateWhenNotMovingToCompleteShouldNotFireAnimationFinishedEvent()
        {
            IAnimationCollection<float> mockCollection = Substitute.For<IAnimationCollection<float>>();
            _mockAnimationFactory.CreateAnimationCollection().Returns(mockCollection);

            bool isComplete = false;
            IValueAnimation<float> mockAnimation = Substitute.For<IValueAnimation<float>>();
            mockAnimation.IsComplete.Returns(x => isComplete);
            mockAnimation.When(x => x.Update(Arg.Any<GameTime>())).Do(x => isComplete = false);
            mockCollection["FadeIn"].Returns(mockAnimation);

            Construct();

            var sub = Substitute.For<EventHandler<EventArgs>>();
            _sut.AnimationFinished += sub;

            _sut.Update(new GameTime());

            sub.Received(0)(Arg.Any<object>(), Arg.Any<EventArgs>());
        }
        #endregion

        #region Draw Tests
        [TestMethod]
        public void DrawShouldDrawOverlayTextureWithExpectedValues_TestA()
        {
            IAnimationCollection<float> mockCollection = Substitute.For<IAnimationCollection<float>>();
            _mockAnimationFactory.CreateAnimationCollection().Returns(mockCollection);

            IValueAnimation<float> mockAnimation = Substitute.For<IValueAnimation<float>>();
            mockAnimation.Value.Returns(0.123f);
            mockCollection["FadeIn"].Returns(mockAnimation);

            Construct();

            _sut.OpaqueColour = Color.Pink;
            _sut.Draw(new GameTime());

            _mockSpriteBatch.Received(1).Draw(Arg.Any<ITexture2D>(), Arg.Any<RectangleF>(), Color.Pink * 0.123f);
        }

        [TestMethod]
        public void DrawShouldDrawOverlayTextureWithExpectedValues_TestB()
        {
            IAnimationCollection<float> mockCollection = Substitute.For<IAnimationCollection<float>>();
            _mockAnimationFactory.CreateAnimationCollection().Returns(mockCollection);

            IValueAnimation<float> mockAnimation = Substitute.For<IValueAnimation<float>>();
            mockAnimation.Value.Returns(0.123f);
            mockCollection["FadeOut"].Returns(mockAnimation);

            Construct();
            _sut.CurrentMode = FadeMode.FadeOut;

            _sut.OpaqueColour = Color.Pink;
            _sut.Draw(new GameTime());

            _mockSpriteBatch.Received(1).Draw(Arg.Any<ITexture2D>(), Arg.Any<RectangleF>(), Color.Pink * 0.123f);
        }
        #endregion
    }
}
