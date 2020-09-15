using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace PriorityQueueTest
{
    public class PriorityQueue<T> : IEnumerable<T>
    {
        private int _count;
        private int _capacity;
        private int _version;
        private KeyValuePair<int, T>[] _heap;

        public PriorityQueue()
        {
            _capacity = 15; // 15 is equal to 4 complete levels
            _heap = new KeyValuePair<int, T>[_capacity];
        }

        public T Dequeue()
        {
            if (_count == 0)
                throw new InvalidOperationException();

            T result = _heap[0].Value;
            _count--;
            TrickleDown(0, _heap[_count]);
            _version++;
            return result;
        }

        public void Enqueue(T item, int priority = 1)
        {
            if (_count == _capacity)
                GrowHeap();
            _count++;
            BubbleUp(_count - 1, new KeyValuePair<int, T>(priority, item));
            _version++;
        }

        private void BubbleUp(int index, KeyValuePair<int, T> entry)
        {
            int parent = GetParent(index);
            // note: (index > 0) means there is a parent
            while (index > 0 && _heap[parent].Key < entry.Key)
            {
                _heap[index] = _heap[parent];
                index = parent;
                parent = GetParent(index);
            }

            _heap[index] = entry;
        }

        private int GetLeftChild(int index)
        {
            return (index * 2) + 1;
        }

        private int GetParent(int index)
        {
            return (index - 1) / 2;
        }

        private void GrowHeap()
        {
            _capacity = (_capacity * 2) + 1;
            var newHeap = new KeyValuePair<int, T>[_capacity];
            Array.Copy(_heap, 0, newHeap, 0, _count);
            _heap = newHeap;
        }

        private void TrickleDown(int index, KeyValuePair<int, T> entry)
        {
            var child = GetLeftChild(index);
            while (child < _count)
            {
                if (child + 1 < _count && _heap[child].Key < _heap[child + 1].Key)
                {
                    child++;
                }

                _heap[index] = _heap[child];
                index = child;
                child = GetLeftChild(index);
            }

            BubbleUp(index, entry);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new PriorityQueueEnumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #region Priority Queue enumerator
        private class PriorityQueueEnumerator : IEnumerator<T>
        {
            private int _index;
            private int _version;
            private readonly PriorityQueue<T> _pq;

            public PriorityQueueEnumerator(PriorityQueue<T> pq)
            {
                _pq = pq;
                Reset();
            }

            private void CheckVersion()
            {
                if (_version != _pq._version)
                    throw new InvalidOperationException();
            }

            #region IEnumerator Members

            public void Reset()
            {
                _index = -1;
                _version = _pq._version;
            }

            object IEnumerator.Current => Current;

            public T Current
            {
                get
                {
                    CheckVersion();
                    return _pq._heap[_index].Value;
                }
            }

            public bool MoveNext()
            {
                CheckVersion();
                if (_index + 1 == _pq._count)
                    return false;
                _index++;
                return true;
            }

            public void Dispose()
            {
            }

            #endregion
        }
        #endregion
    }
}
