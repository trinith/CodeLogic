using System;
using System.Linq;
using ArbitraryPixel.CodeLogic.Common.Entities;
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
    public class TapScreenText_Tests
    {
        private TapScreenText _sut;

        private IEngine _mockEngine;
        private ISpriteBatch _mockSpriteBatch;
        private ISpriteFont _mockFont;
        private IAnimationFactory<float> _mockAnimationFactory;

        [TestInitialize]
        public void Initialize()
        {
            _mockEngine = Substitute.For<IEngine>();
            _mockSpriteBatch = Substitute.For<ISpriteBatch>();
            _mockFont = Substitute.For<ISpriteFont>();
            _mockAnimationFactory = Substitute.For<IAnimationFactory<float>>();
        }

        private void Construct()
        {
            _sut = new TapScreenText(_mockEngine, _mockSpriteBatch, _mockFont, _mockAnimationFactory);
        }

        #region Constructor Tests
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullParameterShouldThrowException_SpriteBatch()
        {
            _sut = new TapScreenText(_mockEngine, null, _mockFont, _mockAnimationFactory);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullParameterShouldThrowException_Font()
        {
            _sut = new TapScreenText(_mockEngine, _mockSpriteBatch, null, _mockAnimationFactory);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullParameterShouldThrowException_AnimationFactory()
        {
            _sut = new TapScreenText(_mockEngine, _mockSpriteBatch, _mockFont, null);
        }

        [TestMethod]
        public void ConstructShouldCreateExpectedAnimation()
        {
            Construct();

            _mockAnimationFactory.Received(1).CreateValueAnimation(0f, Arg.Is<float[]>(x => x.SequenceEqual(new float[] { 1f, 1f, 0f, 1f })), true);
        }

        [TestMethod]
        public void ConstructShouldSetBoundsToExpectedValue()
        {
            _mockFont.MeasureString(Arg.Any<string>()).Returns(new SizeF(100, 25));
            _mockEngine.ScreenManager.World.Returns(new Point(1000, 800));

            Construct();

            Assert.AreEqual<RectangleF>(new RectangleF(450, 25, 100, 25), _sut.Bounds);
        }
        #endregion

        #region Update Tests
        [TestMethod]
        public void UpdateBeforeShowDelayExpiresShouldNotUpdateAnimator()
        {
            IValueAnimation<float> mockAnimation = Substitute.For<IValueAnimation<float>>();
            _mockAnimationFactory.CreateValueAnimation(Arg.Any<float>(), Arg.Any<float[]>(), Arg.Any<bool>()).Returns(mockAnimation);

            Construct();

            _sut.Update(new GameTime());

            mockAnimation.Received(0).Update(Arg.Any<GameTime>());
        }

        [TestMethod]
        public void UpdateAfterShowDelayExpiresShouldUpdateAnimator()
        {
            IValueAnimation<float> mockAnimation = Substitute.For<IValueAnimation<float>>();
            _mockAnimationFactory.CreateValueAnimation(Arg.Any<float>(), Arg.Any<float[]>(), Arg.Any<bool>()).Returns(mockAnimation);

            Construct();

            _sut.Update(new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(TapScreenText.DelayBeforeVisible)));

            GameTime gameTime = new GameTime();
            _sut.Update(gameTime);

            mockAnimation.Received(1).Update(gameTime);
        }
        #endregion

        #region Draw Tests
        [TestMethod]
        public void DrawBeforeShowDelayExpiresShouldNotDrawText()
        {
            IValueAnimation<float> mockAnimation = Substitute.For<IValueAnimation<float>>();
            _mockAnimationFactory.CreateValueAnimation(Arg.Any<float>(), Arg.Any<float[]>(), Arg.Any<bool>()).Returns(mockAnimation);
            mockAnimation.Value.Returns(0.5f);

            Construct();

            _sut.Draw(new GameTime());

            _mockSpriteBatch.Received(0).DrawString(Arg.Any<ISpriteFont>(), Arg.Any<string>(), Arg.Any<Vector2>(), Arg.Any<Color>());
        }

        [TestMethod]
        public void DrawAfterShowDelayExpiresShouldDrawTextWithExpectedParameters()
        {
            _mockFont.MeasureString(Arg.Any<string>()).Returns(new SizeF(100, 25));
            _mockEngine.ScreenManager.World.Returns(new Point(1000, 800));

            IValueAnimation<float> mockAnimation = Substitute.For<IValueAnimation<float>>();
            _mockAnimationFactory.CreateValueAnimation(Arg.Any<float>(), Arg.Any<float[]>(), Arg.Any<bool>()).Returns(mockAnimation);
            mockAnimation.Value.Returns(0.5f);

            Construct();
            _sut.Update(new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(TapScreenText.DelayBeforeVisible)));

            _sut.Draw(new GameTime());

            _mockSpriteBatch.Received(1).DrawString(_mockFont, "Tap the Screen", new Vector2(450, 25), Color.White * 0.5f);
        }
        #endregion
    }
}
