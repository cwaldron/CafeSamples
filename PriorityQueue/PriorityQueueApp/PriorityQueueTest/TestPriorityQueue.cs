using System;
using System.Collections;
using System.Linq;
using JMBucknall.Containers;
using Xunit;

namespace PriorityQueueTest 
{
    public class TestPriorityQueue
    {
        [Fact] 
        public void TestEnqueueing() {
            var pq = new PriorityQueue<string>();
            pq.Enqueue("item one", 12);
            Assert.Single(pq);
            pq.Enqueue("item two", 5);
            Assert.Equal(2, pq.Count());
        }

        [Fact]
        public void TestDequeueing() {
            var pq = new PriorityQueue();
            pq.Enqueue("item one", 12);
            pq.Enqueue("item two", 5);
            var s = (string) pq.Dequeue();
            Assert.Single(pq);
            Assert.Equal("item one", s);
            s = (string) pq.Dequeue();
            Assert.Empty(pq);
            Assert.Equal("item two", s);
        }

        [Fact] 
        public void TestDequeueWithEmptyQueue() {
            var pq = new PriorityQueue();
            Assert.Throws<InvalidOperationException>(() => pq.Dequeue());
        }

        [Fact]
        public void TestGrowingQueue() 
        {
            var pq = new PriorityQueue();
            string s;
            string expectedStr;
            for (int i = 0; i < 15; i++) {
                pq.Enqueue("item: " + i.ToString(), i * 2);
            }

            Assert.Equal(15, pq.Count);
            pq.Enqueue("trigger", 15);
            Assert.Equal(16, pq.Count);
            for (int i = 14; i > 7; i--) {
                s = (string) pq.Dequeue();
                expectedStr = "item: " + i.ToString();
                Assert.Equal(expectedStr, s);
            }
            s = (string) pq.Dequeue();
            Assert.Equal("trigger", s);
            for (int i = 7; i >= 0; i--) {
                s = (string) pq.Dequeue();
                expectedStr = "item: " + i.ToString();
                Assert.Equal(expectedStr, s);
            }
        }

        [Fact] 
        public void TestEnumerator() {
            var pq = new PriorityQueue();
            string s;

            // use a hashtable to check contents of PQ
            Hashtable ht = new Hashtable(); 
            for (int i = 0; i < 5; i++) 
            {
                s = "item: " + i.ToString();
                ht.Add(s, null);
                pq.Enqueue(s, i * 2);
            }

            foreach(string str in pq) 
            {
                Assert.True(ht.Contains(str));
                ht.Remove(str); // so we don't see it again
            }

            Assert.Empty(ht);
        }

        [Fact] 
        public void TestEnumeratorWithEnqueue() {
            var pq = new PriorityQueue();
            pq.Enqueue("one", 42);
            IEnumerator ie = pq.GetEnumerator();
            ie.MoveNext();
            pq.Enqueue("two", 13);
            Assert.Throws<InvalidOperationException>(() => ie.MoveNext()); // should fail
        }

        [Fact] 
        public void TestEnumeratorWithDequeue() {
            var pq = new PriorityQueue();
            pq.Enqueue("one", 42);
            IEnumerator ie = pq.GetEnumerator();
            ie.MoveNext();
            pq.Dequeue();
            Assert.Throws<InvalidOperationException>(() => ie.MoveNext());
        }

        [Fact]
        public void TestCopyTo() {
            var pq = new PriorityQueue();
            string s;

            // use a hashtable to check contents of returned array
            Hashtable ht = new Hashtable(); 
            for (int i = 0; i < 5; i++) {
                s = "item: " + i.ToString();
                ht.Add(s, null);
                pq.Enqueue(s, i * 2);
            }

            HeapEntry[] heArray = new HeapEntry[6];

            pq.CopyTo(heArray, 1);

            foreach(HeapEntry he in heArray) {
                if (he.Item != null) {
                    Assert.True(ht.Contains(he.Item));
                    ht.Remove(he.Item); // so we don't see it again
                }
            }
            Assert.Empty(ht);
        }

    }
}