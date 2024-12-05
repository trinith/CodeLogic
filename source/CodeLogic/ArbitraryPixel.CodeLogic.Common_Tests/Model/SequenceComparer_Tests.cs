using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArbitraryPixel.CodeLogic.Common.Model;
using System.Linq;

namespace ArbitraryPixel.CodeLogic.Common_Tests.Model
{
    [TestClass]
    public class SequenceComparer_Tests
    {
        private SequenceComparer _sut;
        private CodeSequence _masterSequence;
        private CodeSequence _testSequence;

        [TestInitialize]
        public void Initialize()
        {
            _masterSequence = new CodeSequence(4);
            _masterSequence.SetCode(new CodeValue[] { CodeValue.Red, CodeValue.Green, CodeValue.Blue, CodeValue.Yellow });

            _testSequence = new CodeSequence(4);

            _sut = new SequenceComparer();
        }

        #region Null/Mismatch Parameter Tests
        [TestMethod]
        public void CompareWithNullTestSequenceShouldReturnNotEqualResult()
        {
            var expected = new SequenceIndexCompareResult[] { SequenceIndexCompareResult.NotEqual, SequenceIndexCompareResult.NotEqual, SequenceIndexCompareResult.NotEqual, SequenceIndexCompareResult.NotEqual };
            Assert.IsTrue(_sut.Compare(null, _masterSequence).Result.SequenceEqual(expected));
        }

        [TestMethod]
        public void CompareWithNullMasterSequenceShouldReturnEmptyResult()
        {
            Assert.AreEqual<int>(0, _sut.Compare(_testSequence, null).Result.Length);
        }

        [TestMethod]
        public void CompareWithBothNullShouldReturnEmptyResult()
        {
            Assert.AreEqual<int>(0, _sut.Compare(_testSequence, null).Result.Length);
        }

        [TestMethod]
        public void CompareWithDifferentLengthsShouldReturnNotEqualResult()
        {
            var expected = new SequenceIndexCompareResult[] { SequenceIndexCompareResult.NotEqual, SequenceIndexCompareResult.NotEqual, SequenceIndexCompareResult.NotEqual, SequenceIndexCompareResult.NotEqual };
            _testSequence = new CodeSequence(3);
            Assert.IsTrue(_sut.Compare(_testSequence, _masterSequence).Result.SequenceEqual(expected));
        }
        #endregion

        #region Result Tests
        [TestMethod]
        public void CompareWithOneEqualShouldReturnExpectedResult_FirstIndex()
        {
            _testSequence.SetCode(new CodeValue[] { CodeValue.Red, CodeValue.Orange, CodeValue.Orange, CodeValue.Orange });
            var expected = new SequenceIndexCompareResult[] { SequenceIndexCompareResult.Equal, SequenceIndexCompareResult.NotEqual, SequenceIndexCompareResult.NotEqual, SequenceIndexCompareResult.NotEqual };

            ISequenceCompareResult result = _sut.Compare(_testSequence, _masterSequence);

            Assert.IsTrue(result.Result.SequenceEqual(expected));
        }

        [TestMethod]
        public void CompareWithOneEqualShouldReturnExpectedResult_AnyOtherIndex()
        {
            _testSequence.SetCode(new CodeValue[] { CodeValue.Orange, CodeValue.Orange, CodeValue.Blue, CodeValue.Orange });
            var expected = new SequenceIndexCompareResult[] { SequenceIndexCompareResult.Equal, SequenceIndexCompareResult.NotEqual, SequenceIndexCompareResult.NotEqual, SequenceIndexCompareResult.NotEqual };

            ISequenceCompareResult result = _sut.Compare(_testSequence, _masterSequence);

            Assert.IsTrue(result.Result.SequenceEqual(expected));
        }

        [TestMethod]
        public void CompareWithMultipleEqualShouldReturnExpectedResult()
        {
            _testSequence.SetCode(new CodeValue[] { CodeValue.Orange, CodeValue.Green, CodeValue.Blue, CodeValue.Orange });
            var expected = new SequenceIndexCompareResult[] { SequenceIndexCompareResult.Equal, SequenceIndexCompareResult.Equal, SequenceIndexCompareResult.NotEqual, SequenceIndexCompareResult.NotEqual };

            ISequenceCompareResult result = _sut.Compare(_testSequence, _masterSequence);

            Assert.IsTrue(result.Result.SequenceEqual(expected));
        }

        [TestMethod]
        public void CompareWithPartialShouldReturnExpectedResult_FirstIndex()
        {
            _testSequence.SetCode(new CodeValue[] { CodeValue.Orange, CodeValue.Orange, CodeValue.Red, CodeValue.Orange });
            var expected = new SequenceIndexCompareResult[] { SequenceIndexCompareResult.PartialEqual, SequenceIndexCompareResult.NotEqual, SequenceIndexCompareResult.NotEqual, SequenceIndexCompareResult.NotEqual };

            ISequenceCompareResult result = _sut.Compare(_testSequence, _masterSequence);

            Assert.IsTrue(result.Result.SequenceEqual(expected));
        }

        [TestMethod]
        public void CompareWithPartialShouldReturnExpectedResult_AnyOtherIndex()
        {
            _testSequence.SetCode(new CodeValue[] { CodeValue.Orange, CodeValue.Orange, CodeValue.Orange, CodeValue.Red });
            var expected = new SequenceIndexCompareResult[] { SequenceIndexCompareResult.PartialEqual, SequenceIndexCompareResult.NotEqual, SequenceIndexCompareResult.NotEqual, SequenceIndexCompareResult.NotEqual };

            ISequenceCompareResult result = _sut.Compare(_testSequence, _masterSequence);

            Assert.IsTrue(result.Result.SequenceEqual(expected));
        }

        [TestMethod]
        public void CompareWithPartialInMultiplePlacesShouldReturnExpectedResult()
        {
            _testSequence.SetCode(new CodeValue[] { CodeValue.Orange, CodeValue.Red, CodeValue.Orange, CodeValue.Red });
            var expected = new SequenceIndexCompareResult[] { SequenceIndexCompareResult.PartialEqual, SequenceIndexCompareResult.NotEqual, SequenceIndexCompareResult.NotEqual, SequenceIndexCompareResult.NotEqual };

            ISequenceCompareResult result = _sut.Compare(_testSequence, _masterSequence);

            Assert.IsTrue(result.Result.SequenceEqual(expected));
        }

        [TestMethod]
        public void CompareWithEqualAndPartialShouldReturnExpectedResult()
        {
            _testSequence.SetCode(new CodeValue[] { CodeValue.Orange, CodeValue.Green, CodeValue.Orange, CodeValue.Red });
            var expected = new SequenceIndexCompareResult[] { SequenceIndexCompareResult.Equal, SequenceIndexCompareResult.PartialEqual, SequenceIndexCompareResult.NotEqual, SequenceIndexCompareResult.NotEqual };

            ISequenceCompareResult result = _sut.Compare(_testSequence, _masterSequence);

            Assert.IsTrue(result.Result.SequenceEqual(expected));
        }

        [TestMethod]
        public void CompareWithAllEqualShouldReturnExpectedResult()
        {
            _testSequence.SetCode(new CodeValue[] { CodeValue.Red, CodeValue.Green, CodeValue.Blue, CodeValue.Yellow });
            var expected = new SequenceIndexCompareResult[] { SequenceIndexCompareResult.Equal, SequenceIndexCompareResult.Equal, SequenceIndexCompareResult.Equal, SequenceIndexCompareResult.Equal };

            ISequenceCompareResult result = _sut.Compare(_testSequence, _masterSequence);

            Assert.IsTrue(result.Result.SequenceEqual(expected));
        }
        #endregion
    }
}
