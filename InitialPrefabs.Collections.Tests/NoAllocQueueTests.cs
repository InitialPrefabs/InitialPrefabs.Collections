using NUnit.Framework;
using System;

namespace InitialPrefabs.Collections.Tests {
    public class NoAllocQueueTests {

        [Test]
        public void FullQueueTest() {
            Assert.Multiple(() => {
                Span<int> s = stackalloc int[10];
                NoAllocQueue<int> q = new NoAllocQueue<int>(s);

                // Testing queue operations on an emtpy queue
                Assert.That(q.TryPeek(out _) == false, "Peeking an empty queue is not allowed.");
                Assert.That(q.TryDequeue(out _) == false, "Dequeing an empty queue is not allowed.");
                Assert.That(q.Capacity == 10, "The queue should take the fixed capacity defined by the Span.");
                Assert.That(q.Count == 0, "No elements should be added to the queue.");
                
                // Testing enqueing
                for (int i = 0; i < 10; i++) {
                    Assert.That(q.TryEnqueue(i), $"Failed to enqueue {i}");
                    Assert.That(q.Count == i + 1);
                }

                Assert.That(q.TryEnqueue(10) == false, "10 should not be enqueued.");
                
                // Testing dequeing
                int count = 0;
                while (!q.IsEmpty) {
                    Assert.That(q.TryPeek(out int element), "Failed to view the head of the queue.");
                    Assert.That(element == count, $"The head of the queue was not the same as {count}");
                    Assert.That(q.TryDequeue(out element) && element == count);
                    count++;
                    Assert.That(q.Count == q.Capacity - count);
                }
                Assert.That(count == 10, "10 elements should have been processed.");
                // Adding more elements to the queue
                q.Enqueue(11);
                Assert.That(q.Count == 1, "11 should have been enqueued.");
                Assert.That(q.Contains(11), "Contains returns true if the element exists in the Queue.");
                q.Enqueue(0);
                q.Enqueue(1);
                Assert.That(q.Contains(0), "Contains returns true if the element exists in the Queue.");
                
                // Testing clearing
                q.Clear();
                Assert.That(q.Count == 0, "The queue should have been resetted");
                Assert.That(q.Contains(0) == false, "An empty queue should not contain any element.");
            });
        }
    }
}