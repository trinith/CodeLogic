using System;
using ArbitraryPixel.CodeLogic.Common.Entities;
using ArbitraryPixel.Common.Drawing;
using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Platform2D.Engine;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;
using NSubstitute;

namespace ArbitraryPixel.CodeLogic.Common_Tests.Entities
{
    [TestClass]
    public class Cloud_Tests
    {
        private Cloud _sut;

        private IEngine _mockEngine;
        private ISpriteBatch _mockSpriteBatch;
        private ITexture2D _mockTexture;

        private RectangleF _bounds = new RectangleF(200, 100, 400, 300);
        private Color _maskColour = Color.Pink;

        [TestInitialize]
        public void Initialize()
        {
            _mockEngine = Substitute.For<IEngine>();
            _mockSpriteBatch = Substitute.For<ISpriteBatch>();
            _mockTexture = Substitute.For<ITexture2D>();
        }

        private void Construct()
        {
            _sut = new Cloud(_mockEngine, _bounds, _mockSpriteBatch, _mockTexture, _maskColour);
        }

        #region Property Tests
        [TestMethod]
        public void PropertyShouldReturnExpectedDefaultValue_Depth()
        {
            Construct();

            Assert.AreEqual<int>(0, _sut.Depth);
        }

        [TestMethod]
        public void PropertyShouldReturnSetValue_Depth()
        {
            Construct();

            _sut.Depth = 12;

            Assert.AreEqual<int>(12, _sut.Depth);
        }

        [TestMethod]
        public void PropertyShouldReturnExpectedDefaultValue_Intensity()
        {
            Construct();

            Assert.AreEqual<float>(0.75f, _sut.Intensity);
        }

        [TestMethod]
        public void PropertyShouldReturnSetValue_Intensity()
        {
            Construct();

            _sut.Intensity = 0.987f;

            Assert.AreEqual<float>(0.987f, _sut.Intensity);
        }

        [TestMethod]
        public void IntensitySetWithValueBelowMinimumShouldReturnMinimum()
        {
            Construct();

            _sut.Intensity = 0.123f;

            Assert.AreEqual<float>(Cloud.Constants.MinIntensity, _sut.Intensity);
        }

        [TestMethod]
        public void IntensitySetWithValueAboveMaximumShouldReturnMaximum()
        {
            Construct();

            _sut.Intensity = 12.123f;

            Assert.AreEqual<float>(Cloud.Constants.MaxIntensity, _sut.Intensity);
        }
        #endregion
    }
}
