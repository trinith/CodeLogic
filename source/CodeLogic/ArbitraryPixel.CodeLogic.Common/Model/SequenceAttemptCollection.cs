using System.Collections;
using System.Collections.Generic;

namespace ArbitraryPixel.CodeLogic.Common.Model
{
    public interface ISequenceAttemptCollection : IEnumerable<ISequenceAttemptRecord>, ICollection<ISequenceAttemptRecord>, IList<ISequenceAttemptRecord>
    {
    }

    public class SequenceAttemptCollection : ISequenceAttemptCollection
    {
        private List<ISequenceAttemptRecord> _items = new List<ISequenceAttemptRecord>();

        public ISequenceAttemptRecord this[int index]
        {
            get { return _items[index]; }
            set { _items[index] = value; }
        }

        public int Count => _items.Count;
        public bool IsReadOnly => false;

        public void Add(ISequenceAttemptRecord item)
        {
            _items.Add(item);
        }

        public void Clear()
        {
            _items.Clear();
        }

        public bool Contains(ISequenceAttemptRecord item)
        {
            return _items.Contains(item);
        }

        public void CopyTo(ISequenceAttemptRecord[] array, int arrayIndex)
        {
            _items.CopyTo(array, arrayIndex);
        }

        public IEnumerator<ISequenceAttemptRecord> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        public int IndexOf(ISequenceAttemptRecord item)
        {
            return _items.IndexOf(item);
        }

        public void Insert(int index, ISequenceAttemptRecord item)
        {
            _items.Insert(index, item);
        }

        public bool Remove(ISequenceAttemptRecord item)
        {
            return _items.Remove(item);
        }

        public void RemoveAt(int index)
        {
            _items.RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _items.GetEnumerator();
        }
    }
}
