//**********************************************************
//* PriorityQueue                                          *
//* Copyright (c) Julian M Bucknall 2004                   *
//* All rights reserved.                                   *
//* This code can be used in your applications, providing  *
//*    that this copyright comment box remains as-is       *
//**********************************************************
//* .NET priority queue class (heap algorithm              *
//**********************************************************

using System;
using System.Collections;

namespace JMBucknall.Containers {
    public struct HeapEntry {
        private object item;
        private int priority;

        public HeapEntry(object item, int priority) {
            this.item = item;
            this.priority = priority;
        }

        public object Item {
            get {return item;}
        }
        public int Priority {
            get {return priority;}
        }
    }

    public class PriorityQueue : ICollection 
    {
        private int count;
        private int capacity;
        private int version;
        private HeapEntry[] heap;

        public PriorityQueue() {
            capacity = 15; // 15 is equal to 4 complete levels
            heap = new HeapEntry[capacity];
        }

        public object Dequeue() {
            if (count == 0) 
                throw new InvalidOperationException();
            
            object result = heap[0].Item;
            count--;
            trickleDown(0, heap[count]);
            version++;
            return result;
        }

        public void Enqueue(object item, int priority) {
            if (count == capacity)  
                growHeap();
            count++;
            bubbleUp(count - 1, new HeapEntry(item, priority));
            version++;
        }

        private void bubbleUp(int index, HeapEntry he)
        {
            int parent = getParent(index);
            // note: (index > 0) means there is a parent
            while ((index > 0) && (heap[parent].Priority < he.Priority)) {
                heap[index] = heap[parent];
                index = parent;
                parent = getParent(index);
            }
            heap[index] = he;
        }

        private int getLeftChild(int index) {
            return (index * 2) + 1;
        }

        private int getParent(int index) {
            return (index - 1) / 2;
        }

        private void growHeap() {
            capacity = (capacity * 2) + 1;
            HeapEntry[] newHeap = new HeapEntry[capacity];
            System.Array.Copy(heap, 0, newHeap, 0, count);
            heap = newHeap;
        }

        private void trickleDown(int index, HeapEntry he) {
            int child = getLeftChild(index);
            while (child < count) {
                if (((child + 1) < count) && 
                    (heap[child].Priority < heap[child + 1].Priority)) {
                    child++;
                }
                heap[index] = heap[child];
                index = child;
                child = getLeftChild(index);
            }
            bubbleUp(index, he);
        }
        
        #region IEnumerable implementation
        public IEnumerator GetEnumerator() {
            return new PriorityQueueEnumerator(this);
        }
        #endregion

        #region ICollection implementation
        public int Count {
            get {return count;}
        }

        public void CopyTo(Array array, int index) {
            System.Array.Copy(heap, 0, array, index, count);
        }

        public object SyncRoot {
            get {return this;}
        }

        public bool IsSynchronized { 
            get {return false;}
        }
        #endregion

        #region Priority Queue enumerator
        private class PriorityQueueEnumerator : IEnumerator {
            private int index;
            private PriorityQueue pq;
            private int version;

            public PriorityQueueEnumerator(PriorityQueue pq) {
                this.pq = pq;
                Reset();
            }

            private void checkVersion() {
                if (version != pq.version)
                    throw new InvalidOperationException();
            }

            #region IEnumerator Members

            public void Reset() {
                index = -1;
                version = pq.version;
            }

            public object Current {
                get { 
                    checkVersion();
                    return pq.heap[index].Item; 
                }
            }

            public bool MoveNext() {
                checkVersion();
                if (index + 1 == pq.count) 
                    return false;
                index++;
                return true;
            }

            #endregion
        }
        #endregion

    }
}
