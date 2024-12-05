using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArbitraryPixel.CodeLogic.Common.Entities;
using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Common.Drawing;
using ArbitraryPixel.Platform2D.Engine;
using NSubstitute;
using Microsoft.Xna.Framework;
using ArbitraryPixel.Common;

namespace ArbitraryPixel.CodeLogic.Common_Tests.Entities
{
    [TestClass]
    public class ProgressBar_Tests
    {
        private ProgressBar _sut;
        private ISpriteBatch _mockSpritebatch;
        private RectangleF _bounds;
        private IEngine _mockEngine;

        private ITexture2D _mockPixel;

        [TestInitialize]
        public void Initialize()
        {
            _mockEngine = Substitute.For<IEngine>();
            _mockSpritebatch = Substitute.For<ISpriteBatch>();
            _bounds = new RectangleF(200, 100, 400, 300);

            _mockPixel = Substitute.For<ITexture2D>();
            _mockEngine.AssetBank.Get<ITexture2D>("Pixel").Returns(_mockPixel);
        }

        private void Construct()
        {
            _sut = new ProgressBar(_mockEngine, _bounds, _mockSpritebatch);
        }

        #region Constructor Tests
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullParameterShouldThrowException_SpriteBatch()
        {
            _sut = new ProgressBar(_mockEngine, _bounds, null);
        }

        [TestMethod]
        public void ConstructShouldRequestPixelTexture()
        {
            Construct();

            _mockEngine.AssetBank.Received(1).Get<ITexture2D>("Pixel");
        }
        #endregion

        #region Properties
        [TestMethod]
        public void PropertyShouldHaveExpectedDefault_Maximum()
        {
            Construct();

            Assert.AreEqual<float>(100, _sut.Maximum);
        }

        [TestMethod]
        public void PropertyShouldHaveExpectedDefault_Minimum()
        {
            Construct();

            Assert.AreEqual<float>(0, _sut.Minimum);
        }

        [TestMethod]
        public void PropertyShouldHaveExpectedDefault_Value()
        {
            Construct();

            Assert.AreEqual<float>(50, _sut.Value);
        }

        [TestMethod]
        public void PropertyShouldHaveExpectedDefault_BackgroundColour()
        {
            Construct();

            Assert.AreEqual<Color>(new Color(32, 32, 32), _sut.BackgroundColour);
        }

        [TestMethod]
        public void PropertyShouldHaveExpectedDefault_ForegroundColour()
        {
            Construct();

            Assert.AreEqual<Color>(Color.LightGray, _sut.ForegroundColour);
        }
        #endregion

        #region Draw Tests
        [TestMethod]
        public void DrawShouldDrawBackground_TestA()
        {
            Construct();

            _sut.Draw(new GameTime());

            _mockSpritebatch.Received(1).Draw(_mockPixel, _bounds, new Color(32, 32, 32));
        }

        [TestMethod]
        public void DrawShouldDrawBackground_TestB()
        {
            Construct();
            _sut.BackgroundColour = Color.Pink;

            _sut.Draw(new GameTime());

            _mockSpritebatch.Received(1).Draw(_mockPixel, _bounds, Color.Pink);
        }

        [TestMethod]
        public void DrawShouldDrawForeground_ForegroundColour_TestA()
        {
            Construct();
            _sut.Draw(new GameTime());
            _mockSpritebatch.Received(1).Draw(_mockPixel, new RectangleF(200, 100, 200, 300), Color.LightGray);
        }

        [TestMethod]
        public void DrawShouldDrawForeground_ForegroundColour_TestB()
        {
            Construct();
            _sut.ForegroundColour = Color.Pink;
            _sut.Draw(new GameTime());
            _mockSpritebatch.Received(1).Draw(_mockPixel, new RectangleF(200, 100, 200, 300), Color.Pink);
        }

        [TestMethod]
        public void DrawWithDifferentValueRangeShouldDrawForeground()
        {
            Construct();
            _sut.Maximum = 20;
            _sut.Minimum = 10;
            _sut.Value = 12.5f;

            _sut.Draw(new GameTime());

            _mockSpritebatch.Received(1).Draw(_mockPixel, new RectangleF(200, 100, 100, 300), Color.LightGray);
        }

        [TestMethod]
        public void DrawWithValueBelowMinimumShouldDrawAsExpected()
        {
            Construct();
            _sut.Value = -10f;

            _sut.Draw(new GameTime());

            _mockSpritebatch.Received(1).Draw(_mockPixel, new RectangleF(200, 100, 0, 300), Color.LightGray);
        }

        [TestMethod]
        public void DrawWithValueAboveMaximumShouldDrawAsExpected()
        {
            Construct();
            _sut.Value = 150f;

            _sut.Draw(new GameTime());

            _mockSpritebatch.Received(1).Draw(_mockPixel, new RectangleF(200, 100, 400, 300), Color.LightGray);
        }
        #endregion

        #region ValueChanged Tests
        [TestMethod]
        public void ValueSetWithDifferentValueShouldGenerateValueChangedEvent()
        {
            Construct();
            _sut.Value = 25;

            var subscriber = Substitute.For<EventHandler<StateChangedEventArgs<float>>>();
            _sut.ValueChanged += subscriber;

            _sut.Value = 50;

            subscriber.Received(1)(
                _sut,
                Arg.Is<StateChangedEventArgs<float>>(x => x.PreviousState == 25f && x.CurrentState == 50f)
            );
        }

        [TestMethod]
        public void ValueSetWithSameValueShouldNotGenerateValueChangedEvent()
        {
            Construct();
            _sut.Value = 50;

            var subscriber = Substitute.For<EventHandler<StateChangedEventArgs<float>>>();
            _sut.ValueChanged += subscriber;

            _sut.Value = 50;

            subscriber.Received(0)(Arg.Any<object>(), Arg.Any<StateChangedEventArgs<float>>());
        }
        #endregion
    }
}
