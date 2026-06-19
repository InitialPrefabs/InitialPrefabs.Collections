using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace InitialPrefabs.Collections.Tests {

    public readonly struct IntComparer : IComparer<int> {
        public int Compare(int x, int y) => x - y;
    }
    public class NoAllocPriorityQueueTests {

        [Test]
        public void EnqueingTest() {
            Assert.Multiple(() => {
                Span<char> _items = stackalloc char[10];
                Span<int> _priorities = stackalloc int[10];

                NoAllocPriorityQueue<char, int, IntComparer> queue = new NoAllocPriorityQueue<char, int, IntComparer>(_items, _priorities, default);

                Assert.That(queue.TryDequeue(out _, out _), Is.False);
                Assert.That(queue.TryPeek(out _, out _), Is.False);
                Assert.That(queue.TryEnqueue('a', 10), "Failed to enqueue");
                Assert.That(queue.TryEnqueue('b', 0), "Failed to enqueue");
                Assert.That(queue.Count, Is.EqualTo(2), "Mismatched enqueing count");

                Assert.That(queue.TryDequeue(out char item, out int priority), "Failed to dequeue");
                Assert.That(item, Is.EqualTo('b'), "b has a priority of 0 so should be ahead in the queue");

                Assert.That(queue.TryEnqueue('c', 10), "Failed to enqueue");
                Assert.That(queue.Count, Is.EqualTo(2));

                Assert.That(queue.TryEnqueue('d', 20));
                Assert.That(queue.Count, Is.EqualTo(3));

                Assert.That(queue.TryPeek(out item, out priority), "failed to peek");
                Assert.That(item, Is.EqualTo('a'), "a should come before c, even if the priorities are both 10");
                Assert.That(priority, Is.EqualTo(10), "The priority of a was 10");

                Assert.That(queue.TryDequeue(out item, out priority), "Failed to dequeue");
                Assert.That(item, Is.EqualTo('a'), "a should come before c, even if the priorities are both 10");
                Assert.That(priority, Is.EqualTo(10), "The priority of a was 10");
            });
        }
    }
}