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
                }
            });
        }
    }
}