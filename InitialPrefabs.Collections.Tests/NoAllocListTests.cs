using NUnit.Framework;
using System;

namespace InitialPrefabs.Collections.Tests {
    public class NoAllocListTests {
        [Test]
        public void FullListTest() {
            Assert.Multiple(() => {
                Span<int> s = stackalloc int[10];
                NoAllocList<int> l = new NoAllocList<int>(s);
                Assert.That(l.Count == 0, "The list should be empty on construction");
                Assert.That(l.Capacity == s.Length, "The list should match the size of the span");

                for (int i = 0; i < l.Capacity; i++) {
                    l.Add(i);
                    Assert.That(l.Count == i + 1, $"The total elements stored should be {i + 1}");
                }
                l.RemoveAt(l.Count - 1);
                Assert.That(l.Count == l.Capacity - 1, "One element should be removed, meaning 9 should be left");
                l.RemoveAtSwapback(0);
                Assert.That(l.Count == l.Capacity - 2, "A second element should be removed, meaning 8 are left");
                Assert.That(l[0] == 8, "8 should be first element due to the swapback");
            });
        }
    }
}