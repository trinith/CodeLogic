using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArbitraryPixel.CodeLogic.Common.Model;
using System;

namespace ArbitraryPixel.CodeLogic.Common_Tests.Model
{
    [TestClass]
    public class SequenceCompareResult_Tests
    {
        [TestMethod]
        public void ResultShouldReturnSequenceFromConstructor()
        {
            var sut = new SequenceCompareResult(new SequenceIndexCompareResult[] { SequenceIndexCompareResult.Equal, SequenceIndexCompareResult.NotEqual, SequenceIndexCompareResult.PartialEqual });
            Assert.IsTrue(sut.Result.SequenceEqual(new SequenceIndexCompareResult[] { SequenceIndexCompareResult.Equal, SequenceIndexCompareResult.NotEqual, SequenceIndexCompareResult.PartialEqual }));
        }

        [TestMethod]
        public void IsEqualWithResultThatIsAllEqualShouldReturnTrue()
        {
            var sut = new SequenceCompareResult(new SequenceIndexCompareResult[] { SequenceIndexCompareResult.Equal, SequenceIndexCompareResult.Equal, SequenceIndexCompareResult.Equal });
            Assert.IsTrue(sut.IsEqual);
        }

        [TestMethod]
        public void IsEqualWithResulThatHasNotEqualIndexShouldReturnFalse()
        {
            var sut = new SequenceCompareResult(new SequenceIndexCompareResult[] { SequenceIndexCompareResult.Equal, SequenceIndexCompareResult.NotEqual, SequenceIndexCompareResult.Equal });
            Assert.IsFalse(sut.IsEqual);
        }

        [TestMethod]
        public void IsEqualWithResulThatHasPartialEqualIndexShouldReturnFalse()
        {
            var sut = new SequenceCompareResult(new SequenceIndexCompareResult[] { SequenceIndexCompareResult.Equal, SequenceIndexCompareResult.PartialEqual, SequenceIndexCompareResult.Equal });
            Assert.IsFalse(sut.IsEqual);
        }

        [TestMethod]
        public void IsEqualWithEmptyResultSHouldReturnFalse()
        {
            var sut = new SequenceCompareResult(new SequenceIndexCompareResult[] { });
            Assert.IsFalse(sut.IsEqual);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullResultShouldThrowException()
        {
            var sut = new SequenceCompareResult(null);
        }

        [TestMethod]
        public void ToStringShouldReturnExpectedResult()
        {
            var sut = new SequenceCompareResult(new SequenceIndexCompareResult[] { SequenceIndexCompareResult.Equal, SequenceIndexCompareResult.NotEqual, SequenceIndexCompareResult.PartialEqual });
            Assert.AreEqual<string>("{Equal, NotEqual, PartialEqual}", sut.ToString());
        }
    }
}
