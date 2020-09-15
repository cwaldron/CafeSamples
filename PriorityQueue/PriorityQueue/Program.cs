using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Algorithms
{
    static class Program
    {
        static void Main(string[] args)
        {
            TestQueue();
            //TestHeap();
        }


        static void TestHeap()
        {
            var heap = new BinaryHeap<int>
            {
                3,
                8,
                5,
                6,
                4,
                10
            };
            int value = heap.Remove();
            Console.WriteLine(value);
        }

        private static void TestQueue()
        {
            var queue = new PriorityQueue<string>();
            var rnd = new Random();

            for (var ii = 0; ii < 1000; ++ii)
            {
                int priority = rnd.Next() % 3;
                queue.Enqueue($"This is message {ii}", priority);
                Thread.Sleep(priority);
            }

            while (queue.Any())
            {
                Console.WriteLine(queue.Dequeue());
            }
        }
    }
}
