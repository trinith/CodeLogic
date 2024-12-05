using ArbitraryPixel.CodeLogic.Common.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;

namespace ArbitraryPixel.CodeLogic.Common_Tests.Model
{
    [TestClass]
    public class CodeValueColourMap_Tests
    {
        private CodeValueColourMap _sut;

        [TestInitialize]
        public void Initialize()
        {
            _sut = new CodeValueColourMap();
        }

        [TestMethod]
        public void GetColourForValueShouldReturnExpectedColour_Red()
        {
            Assert.AreEqual<Color>(Color.Red, _sut.GetColour(CodeValue.Red));
        }

        [TestMethod]
        public void GetColourForValueShouldReturnExpectedColour_Green()
        {
            Assert.AreEqual<Color>(Color.Green, _sut.GetColour(CodeValue.Green));
        }

        [TestMethod]
        public void GetColourForValueShouldReturnExpectedColour_Blue()
        {
            Assert.AreEqual<Color>(Color.Blue, _sut.GetColour(CodeValue.Blue));
        }

        [TestMethod]
        public void GetColourForValueShouldReturnExpectedColour_Yellow()
        {
            Assert.AreEqual<Color>(Color.Yellow, _sut.GetColour(CodeValue.Yellow));
        }

        [TestMethod]
        public void GetColourForValueShouldReturnExpectedColour_Orange()
        {
            Assert.AreEqual<Color>(Color.Orange, _sut.GetColour(CodeValue.Orange));
        }
    }
}
