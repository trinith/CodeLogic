using ArbitraryPixel.CodeLogic.Common.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace ArbitraryPixel.CodeLogic.Common_Tests.Model
{
    [TestClass]
    public class SequenceAttemptRecord_Tests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullCodeShouldThrowException()
        {
            var sut = new SequenceAttemptRecord(null, new SequenceIndexCompareResult[] { SequenceIndexCompareResult.Equal });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullResultShouldThrowException()
        {
            var sut = new SequenceAttemptRecord(new CodeValue[] { CodeValue.Red }, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructWithEmptyCodeShouldThrowException()
        {
            var sut = new SequenceAttemptRecord(new CodeValue[] { }, new SequenceIndexCompareResult[] { SequenceIndexCompareResult.Equal });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructWithEmptyResultShouldThrowException()
        {
            var sut = new SequenceAttemptRecord(new CodeValue[] { CodeValue.Red }, new SequenceIndexCompareResult[] { });
        }

        [TestMethod]
        public void CodeShouldReturnCodeFromConstructor()
        {
            var sut = new SequenceAttemptRecord(new CodeValue[] { CodeValue.Red, CodeValue.Green, CodeValue.Orange }, new SequenceIndexCompareResult[] { SequenceIndexCompareResult.Equal, SequenceIndexCompareResult.PartialEqual, SequenceIndexCompareResult.NotEqual });
            Assert.IsTrue(sut.Code.SequenceEqual(new CodeValue[] { CodeValue.Red, CodeValue.Green, CodeValue.Orange }));
        }

        [TestMethod]
        public void ResultShouldReturnResultFromConstructor()
        {
            var sut = new SequenceAttemptRecord(new CodeValue[] { CodeValue.Red, CodeValue.Green, CodeValue.Orange }, new SequenceIndexCompareResult[] { SequenceIndexCompareResult.Equal, SequenceIndexCompareResult.PartialEqual, SequenceIndexCompareResult.NotEqual });
            Assert.IsTrue(sut.Result.SequenceEqual(new SequenceIndexCompareResult[] { SequenceIndexCompareResult.Equal, SequenceIndexCompareResult.PartialEqual, SequenceIndexCompareResult.NotEqual }));
        }
    }
}
