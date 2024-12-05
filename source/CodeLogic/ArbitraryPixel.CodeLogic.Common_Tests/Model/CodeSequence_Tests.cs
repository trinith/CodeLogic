using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArbitraryPixel.CodeLogic.Common.Model;
using System.Linq;
using ArbitraryPixel.Common;
using NSubstitute;

namespace ArbitraryPixel.CodeLogic.Common_Tests.Model
{
    [TestClass]
    public class CodeSequence_Tests
    {
        private CodeSequence _sut;
        private CodeValue[] _testCode = new CodeValue[] { CodeValue.Red, CodeValue.Green, CodeValue.Blue };

        [TestInitialize]
        public void Initialize()
        {
            _sut = new CodeSequence(_testCode.Length);
        }

        #region Constructor Tests
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructWithInvalidLengthShouldThrowException_Zero()
        {
            _sut = new CodeSequence(0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructWithInvalidLengthShouldThrowException_NegativeOne()
        {
            _sut = new CodeSequence(0);
        }

        [TestMethod]
        public void CodeSequenceSetsAllIndexesToValueZero()
        {
            _sut = new CodeSequence(3);
            Assert.IsTrue(_sut.Code.SequenceEqual(new CodeValue[] { CodeValue.Red, CodeValue.Red, CodeValue.Red }));
        }
        #endregion

        #region SetCode Tests()
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SetCodeWithNullCodeShouldThrowException()
        {
            _sut.SetCode(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SetCodeWithEmptyCodeShouldThrowException()
        {
            _sut.SetCode(new CodeValue[] { });
        }
        #endregion

        #region Indexer Tests
        [TestMethod]
        public void IndexerShouldReturnExpectedValueForIndex_0()
        {
            _sut.SetCode(_testCode);
            Assert.AreEqual(CodeValue.Red, _sut[0]);
        }

        [TestMethod]
        public void IndexerShouldReturnExpectedValueForIndex_1()
        {
            _sut.SetCode(_testCode);
            Assert.AreEqual(CodeValue.Green, _sut[1]);
        }

        [TestMethod]
        public void IndexerShouldReturnExpectedValueForIndex_2()
        {
            _sut.SetCode(_testCode);
            Assert.AreEqual(CodeValue.Blue, _sut[2]);
        }

        [TestMethod]
        public void IndexerSetShouldReturnExpectedValue_Index0()
        {
            _sut[0] = CodeValue.Orange;
            Assert.AreEqual<CodeValue>(CodeValue.Orange, _sut[0]);
        }

        [TestMethod]
        public void IndexerSetShouldReturnExpectedValue_Index2()
        {
            _sut[2] = CodeValue.Yellow;
            Assert.AreEqual<CodeValue>(CodeValue.Yellow, _sut[2]);
        }

        [TestMethod]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void IndexerGetFromInvalidIndexShouldThrowException()
        {
            CodeValue dummy = _sut[3];
        }

        [TestMethod]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void IndexerSetWithInvalidIndexShouldThrowException()
        {
            _sut[3] = CodeValue.Green;
        }
        #endregion

        #region Property Tests
        [TestMethod]
        public void LengthShouldReturnExpectedValue()
        {
            Assert.AreEqual(3, _sut.Length);
        }
        #endregion

        #region Generate Random Code Tests
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GenerateRandomCodeWithNullRandomShouldThrowException()
        {
            _sut.GenerateRandomCode(null);
        }

        [TestMethod]
        public void GeneratedRandomCodeWithRandomSeedShouldGiveExpectedCode_Seed1()
        {
            CodeValue[] expectedSequence = new CodeValue[] { CodeValue.Green, CodeValue.Red, CodeValue.Green };
            IRandom mockRandom = Substitute.For<IRandom>();
            int currentIndex = 0;
            mockRandom.Next(Arg.Any<int>(), Arg.Any<int>()).Returns(
                x =>
                {
                    return (int)expectedSequence[currentIndex++];
                }
            );

            _sut.GenerateRandomCode(mockRandom);

            Assert.IsTrue(_sut.Code.SequenceEqual(new CodeValue[] { CodeValue.Green, CodeValue.Red, CodeValue.Green }));
        }
        #endregion

        #region Contains Tests
        [TestMethod]
        public void ContainsWithValueNotPresentShouldReturnFalse()
        {
            _sut.SetCode(_testCode);
            Assert.IsFalse(_sut.Contains(CodeValue.Yellow));
        }

        [TestMethod]
        public void ContainsWithValuePresentShouldReturnTrue_Index0()
        {
            _sut.SetCode(_testCode);
            Assert.IsTrue(_sut.Contains(CodeValue.Red));
        }

        [TestMethod]
        public void ContainsWithValuePresentShouldReturnTrue_Index1()
        {
            _sut.SetCode(_testCode);
            Assert.IsTrue(_sut.Contains(CodeValue.Green));
        }

        [TestMethod]
        public void ContainsWithValuePresentShouldReturnTrue_Index2()
        {
            _sut.SetCode(_testCode);
            Assert.IsTrue(_sut.Contains(CodeValue.Blue));
        }
        #endregion

        #region ToString Tests
        [TestMethod]
        public void ToStringShouldGiveExpectedValue()
        {
            _sut.SetCode(_testCode);
            Assert.AreEqual<string>("{Red, Green, Blue}", _sut.ToString());
        }
        #endregion
    }
}
