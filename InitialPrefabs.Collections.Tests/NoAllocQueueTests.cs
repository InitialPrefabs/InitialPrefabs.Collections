using NUnit.Framework;
using System;

namespace InitialPrefabs.Collections.Tests {
    public class NoAllocQueueTests {

        [Test]
        public void AddingElementsToTheQueue() {
            Assert.Multiple(() => {
                Span<int> s = stackalloc int[10];
                NoAllocQueue<int> q = new NoAllocQueue<int>(s);
                Assert.That(q.Capacity == 10, "The queue should take the fixed capacity defined by the Span.");
                Assert.That(q.Count == 0, "No elements should be added to the queue.");

                for (int i = 0; i < 10; i++) {
                    Assert.That(q.TryEnqueue(i), $"Failed to enqueue {i}");
                    Assert.That(q.Count == i + 1);
                }

                Assert.That(q.TryEnqueue(10) == false, "10 should not be enqueued.");

                int count = 0;
                while (!q.IsEmpty) {
                    Assert.That(q.TryPeek(out int element), "Failed to view the head of the queue.");
                    Assert.That(element == count, $"The head of the queue was not the same as {count}");
                    Assert.That(q.TryDequeue(out element) && element == count);
                    count++;
                    Assert.That(q.Count == q.Capacity - count);
                }
            });
        }
    }
}