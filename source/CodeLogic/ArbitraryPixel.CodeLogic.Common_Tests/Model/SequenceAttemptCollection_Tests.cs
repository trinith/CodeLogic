using ArbitraryPixel.CodeLogic.Common.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections;
using System.Collections.Generic;

namespace ArbitraryPixel.CodeLogic.Common_Tests.Model
{
    [TestClass]
    public class SequenceAttemptCollection_Tests
    {
        private SequenceAttemptCollection _sut;

        [TestInitialize]
        public void Initialize()
        {
            _sut = new SequenceAttemptCollection();
        }

        #region Indexer Tests
        [TestMethod]
        public void IndexerGetShouldReturnExpectedValue_Index0()
        {
            var mockA = Substitute.For<ISequenceAttemptRecord>();
            var mockB = Substitute.For<ISequenceAttemptRecord>();
            _sut.Add(mockA);
            _sut.Add(mockB);

            Assert.AreSame(mockA, _sut[0]);
        }

        [TestMethod]
        public void IndexerGetShouldReturnExpectedValue_Index1()
        {
            var mockA = Substitute.For<ISequenceAttemptRecord>();
            var mockB = Substitute.For<ISequenceAttemptRecord>();
            _sut.Add(mockA);
            _sut.Add(mockB);

            Assert.AreSame(mockB, _sut[1]);
        }

        [TestMethod]
        public void IndexerShouldSetExpectedValue()
        {
            var mockA = Substitute.For<ISequenceAttemptRecord>();
            var mockB = Substitute.For<ISequenceAttemptRecord>();
            _sut.Add(mockA);
            _sut[0] = mockB;

            Assert.AreSame(mockB, _sut[0]);
        }
        #endregion

        #region Count Tests
        [TestMethod]
        public void CountShouldDefaultZero()
        {
            Assert.AreEqual<int>(0, _sut.Count);
        }

        [TestMethod]
        public void CountShouldReturnExpectedValue_TestA()
        {
            for (int i = 0; i < 5; i++)
                _sut.Add(Substitute.For<ISequenceAttemptRecord>());

            Assert.AreEqual<int>(5, _sut.Count);
        }

        [TestMethod]
        public void CountShouldReturnExpectedValue_TestB()
        {
            for (int i = 0; i < 12; i++)
                _sut.Add(Substitute.For<ISequenceAttemptRecord>());

            Assert.AreEqual<int>(12, _sut.Count);
        }
        #endregion

        #region IsReadOnly Tests
        [TestMethod]
        public void IsReadOnlyReturnsFalse()
        {
            Assert.IsFalse(_sut.IsReadOnly);
        }
        #endregion

        #region Add / Remove Tests
        [TestMethod]
        public void AddShouldAddItemToCollection()
        {
            ISequenceAttemptRecord mockA;
            _sut.Add(mockA = Substitute.For<ISequenceAttemptRecord>());

            Assert.AreSame(mockA, _sut[0]);
        }

        [TestMethod]
        public void RemoveWithContainedItemShouldRemoveFromCollection()
        {
            ISequenceAttemptRecord mockA;
            _sut.Add(mockA = Substitute.For<ISequenceAttemptRecord>());

            _sut.Remove(mockA);

            Assert.AreEqual<int>(0, _sut.Count);
        }

        [TestMethod]
        public void RemoveWithContainedItemShouldReturnTrue()
        {
            ISequenceAttemptRecord mockA;
            _sut.Add(mockA = Substitute.For<ISequenceAttemptRecord>());

            Assert.IsTrue(_sut.Remove(mockA));
        }

        [TestMethod]
        public void RemoveWithNotContainedItemShouldReturnFalse()
        {
            ISequenceAttemptRecord mockA, mockB = Substitute.For<ISequenceAttemptRecord>();
            _sut.Add(mockA = Substitute.For<ISequenceAttemptRecord>());

            Assert.IsFalse(_sut.Remove(mockB));
        }

        [TestMethod]
        public void RemoveWithNotContainedItemShouldNotChangeCollectionCount()
        {
            ISequenceAttemptRecord mockA, mockB = Substitute.For<ISequenceAttemptRecord>();
            _sut.Add(mockA = Substitute.For<ISequenceAttemptRecord>());

            _sut.Remove(mockB);

            Assert.AreEqual<int>(1, _sut.Count);
        }
        #endregion

        #region Insert / RemoveAt Tests
        [TestMethod]
        public void InsertShouldPutItemAtExpectedIndex()
        {
            ISequenceAttemptRecord mockA, mockB;
            _sut.Add(mockA = Substitute.For<ISequenceAttemptRecord>());

            _sut.Insert(0, mockB = Substitute.For<ISequenceAttemptRecord>());

            Assert.AreSame(mockB, _sut[0]);
        }

        [TestMethod]
        public void InsertShouldPutExistingItemAtExpectedIndex()
        {
            ISequenceAttemptRecord mockA, mockB;
            _sut.Add(mockA = Substitute.For<ISequenceAttemptRecord>());

            _sut.Insert(0, mockB = Substitute.For<ISequenceAttemptRecord>());

            Assert.AreSame(mockA, _sut[1]);
        }

        [TestMethod]
        public void InsertShouldIncrementCount()
        {
            ISequenceAttemptRecord mockA, mockB;
            _sut.Add(mockA = Substitute.For<ISequenceAttemptRecord>());

            _sut.Insert(0, mockB = Substitute.For<ISequenceAttemptRecord>());

            Assert.AreEqual<int>(2, _sut.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void InsertAtInvalidIndexShouldThrowExpectedException()
        {
            ISequenceAttemptRecord mockA, mockB;
            _sut.Add(mockA = Substitute.For<ISequenceAttemptRecord>());

            _sut.Insert(4, mockB = Substitute.For<ISequenceAttemptRecord>());
        }

        [TestMethod]
        public void RemoveAtShouldShiftItemsToStartOfCollection()
        {
            ISequenceAttemptRecord mockA, mockB;
            _sut.Add(mockA = Substitute.For<ISequenceAttemptRecord>());
            _sut.Add(mockB = Substitute.For<ISequenceAttemptRecord>());

            _sut.RemoveAt(0);

            Assert.AreSame(mockB, _sut[0]);
        }

        [TestMethod]
        public void RemoveAtShouldUpdateCount()
        {
            ISequenceAttemptRecord mockA, mockB;
            _sut.Add(mockA = Substitute.For<ISequenceAttemptRecord>());
            _sut.Add(mockB = Substitute.For<ISequenceAttemptRecord>());

            _sut.RemoveAt(0);

            Assert.AreEqual<int>(1, _sut.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void RemoveAtWithInvalidIndexShouldThrowException()
        {
            ISequenceAttemptRecord mockA, mockB;
            _sut.Add(mockA = Substitute.For<ISequenceAttemptRecord>());
            _sut.Add(mockB = Substitute.For<ISequenceAttemptRecord>());

            _sut.RemoveAt(4);
        }
        #endregion

        #region Clear Tests
        [TestMethod]
        public void ClearShouldRemoveAllItemsInList()
        {
            for (int i = 0; i < 5; i++)
                _sut.Add(Substitute.For<ISequenceAttemptRecord>());

            _sut.Clear();

            Assert.AreEqual<int>(0, _sut.Count);
        }
        #endregion

        #region CopyTo Tests
        [TestMethod]
        public void CopyToShouldCopyListIntoArray_Index0()
        {
            ISequenceAttemptRecord mockA, mockB;
            ISequenceAttemptRecord[] array = new ISequenceAttemptRecord[2];
            _sut.Add(mockA = Substitute.For<ISequenceAttemptRecord>());
            _sut.Add(mockB = Substitute.For<ISequenceAttemptRecord>());

            _sut.CopyTo(array, 0);

            Assert.AreSame(mockA, array[0]);
        }

        [TestMethod]
        public void CopyToShouldCopyListIntoArray_Index1()
        {
            ISequenceAttemptRecord mockA, mockB;
            ISequenceAttemptRecord[] array = new ISequenceAttemptRecord[2];
            _sut.Add(mockA = Substitute.For<ISequenceAttemptRecord>());
            _sut.Add(mockB = Substitute.For<ISequenceAttemptRecord>());

            _sut.CopyTo(array, 0);

            Assert.AreSame(mockB, array[1]);
        }

        [TestMethod]
        public void CopyToWithNonZeroStartingIndexShouldCopyListIntoArrayAtThatIndex_Index2()
        {
            ISequenceAttemptRecord mockA, mockB;
            ISequenceAttemptRecord[] array = new ISequenceAttemptRecord[4];
            _sut.Add(mockA = Substitute.For<ISequenceAttemptRecord>());
            _sut.Add(mockB = Substitute.For<ISequenceAttemptRecord>());

            _sut.CopyTo(array, 2);

            Assert.AreSame(mockA, array[2]);
        }

        [TestMethod]
        public void CopyToWithNonZeroStartingIndexShouldCopyListIntoArrayAtThatIndex_Index3()
        {
            ISequenceAttemptRecord mockA, mockB;
            ISequenceAttemptRecord[] array = new ISequenceAttemptRecord[4];
            _sut.Add(mockA = Substitute.For<ISequenceAttemptRecord>());
            _sut.Add(mockB = Substitute.For<ISequenceAttemptRecord>());

            _sut.CopyTo(array, 2);

            Assert.AreSame(mockB, array[3]);
        }

        [TestMethod]
        public void CopyToWithNonZeroStartingIndexShouldCopyListIntoArrayAtThatIndex_Index1()
        {
            ISequenceAttemptRecord mockA, mockB;
            ISequenceAttemptRecord[] array = new ISequenceAttemptRecord[4];
            _sut.Add(mockA = Substitute.For<ISequenceAttemptRecord>());
            _sut.Add(mockB = Substitute.For<ISequenceAttemptRecord>());

            _sut.CopyTo(array, 2);

            Assert.IsNull(array[1]);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CopyToWithNullArrayShouldThrowException()
        {
            _sut.CopyTo(null, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CopyToWithTooMuchDataShouldThrowException()
        {
            ISequenceAttemptRecord[] array = new ISequenceAttemptRecord[2];
            for (int i = 0; i < 3; i++)
                _sut.Add(Substitute.For<ISequenceAttemptRecord>());

            _sut.CopyTo(array, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CopyToWithStartIndexThatGoesPastEndOfArrayShouldThrowException()
        {
            ISequenceAttemptRecord[] array = new ISequenceAttemptRecord[2];
            for (int i = 0; i < 2; i++)
                _sut.Add(Substitute.For<ISequenceAttemptRecord>());

            _sut.CopyTo(array, 1);
        }
        #endregion

        #region Contains / IndexOf Tests
        [TestMethod]
        public void ContainsWithItemInCollectionShouldReturnTrue()
        {
            ISequenceAttemptRecord mockA;
            _sut.Add(mockA = Substitute.For<ISequenceAttemptRecord>());

            Assert.IsTrue(_sut.Contains(mockA));
        }

        [TestMethod]
        public void ContainsWithItemNotInCollectionShouldReturnFalse()
        {
            ISequenceAttemptRecord mockA = Substitute.For<ISequenceAttemptRecord>();

            Assert.IsFalse(_sut.Contains(mockA));
        }

        [TestMethod]
        public void IndexOfShouldReturnExpectedIndex_TestA()
        {
            ISequenceAttemptRecord mockA, mockB;
            _sut.Add(mockA = Substitute.For<ISequenceAttemptRecord>());
            _sut.Add(mockB = Substitute.For<ISequenceAttemptRecord>());

            Assert.AreEqual<int>(0, _sut.IndexOf(mockA));
        }

        [TestMethod]
        public void IndexOfShouldReturnExpectedIndex_TestB()
        {
            ISequenceAttemptRecord mockA, mockB;
            _sut.Add(mockA = Substitute.For<ISequenceAttemptRecord>());
            _sut.Add(mockB = Substitute.For<ISequenceAttemptRecord>());

            Assert.AreEqual<int>(1, _sut.IndexOf(mockB));
        }

        [TestMethod]
        public void IndexOfWithItemNotInCollectionShouldReturnExpectedValue()
        {
            ISequenceAttemptRecord mockA, mockB;
            _sut.Add(mockA = Substitute.For<ISequenceAttemptRecord>());
            mockB = Substitute.For<ISequenceAttemptRecord>();

            Assert.AreEqual<int>(-1, _sut.IndexOf(mockB));
        }
        #endregion

        #region Enumeration Tests
        [TestMethod]
        public void EnumeratorEnumerates_TestA()
        {
            ISequenceAttemptRecord mockA, mockB;
            _sut.Add(mockA = Substitute.For<ISequenceAttemptRecord>());
            _sut.Add(mockB = Substitute.For<ISequenceAttemptRecord>());
            List<ISequenceAttemptRecord> storage = new List<ISequenceAttemptRecord>();
            foreach (ISequenceAttemptRecord record in _sut)
                storage.Add(record);

            Assert.AreSame(mockA, storage[0]);
        }

        [TestMethod]
        public void EnumeratorEnumerates_TestB()
        {
            ISequenceAttemptRecord mockA, mockB;
            _sut.Add(mockA = Substitute.For<ISequenceAttemptRecord>());
            _sut.Add(mockB = Substitute.For<ISequenceAttemptRecord>());
            List<ISequenceAttemptRecord> storage = new List<ISequenceAttemptRecord>();
            foreach (ISequenceAttemptRecord record in _sut)
                storage.Add(record);

            Assert.AreSame(mockB, storage[1]);
        }

        [TestMethod]
        public void GetEnumeratorForGenericShouldReturnExpectedEnumerator()
        {
            ISequenceAttemptRecord mockA, mockB;
            _sut.Add(mockA = Substitute.For<ISequenceAttemptRecord>());
            _sut.Add(mockB = Substitute.For<ISequenceAttemptRecord>());

            IEnumerator<ISequenceAttemptRecord> e = _sut.GetEnumerator();
            e.MoveNext();

            Assert.AreEqual(mockA, e.Current);
        }

        [TestMethod]
        public void GetEnumeratorForNonGenericShouldReturnExpectedEnumerator()
        {
            ISequenceAttemptRecord mockA, mockB;
            _sut.Add(mockA = Substitute.For<ISequenceAttemptRecord>());
            _sut.Add(mockB = Substitute.For<ISequenceAttemptRecord>());

            IEnumerator e = ((IEnumerable)_sut).GetEnumerator();
            e.MoveNext();

            Assert.AreEqual(mockA, e.Current);
        }
        #endregion
    }
}
