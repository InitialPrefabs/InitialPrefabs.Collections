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
                
                // Testing iteration
                for (int i = 0; i < l.Capacity; i++) {
                    l.Add(i);
                    Assert.That(l.Count == i + 1, $"The total elements stored should be {i + 1}");
                }
                
                int counter = 0;
                foreach (int element in l) {
                    Assert.That(element == counter, "The foreach should use the GetEnumerator() implementation.");
                    counter++;
                }
                
                // Testing the Enumerator
                NoAllocEnumerator<int> it = l.GetEnumerator();
                Assert.That(it.MoveNext(), "An enumerator can iterate to the next element as it is at the head.");
                Assert.That(it.Current == 0, "The head should be 0.");
                it.Reset();
                Assert.That(it.Index == -1, "Resetting the enumerator should reset to -1");
                
                // Testing some add/removes
                l.Add(0);
                Assert.That(l.Count == l.Capacity, "No element should be added if the Capacity has been reached");
                l.RemoveAt(l.Count - 1);
                Assert.That(l.Count == l.Capacity - 1, "One element should be removed, meaning 9 should be left");
                l.RemoveAtSwapback(0);
                Assert.That(l.Count == l.Capacity - 2, "A second element should be removed, meaning 8 are left");
                
                // Testing assignments via operator & by ref
                ref int first = ref l.ElementAt(0);
                Assert.That(first == 8, "8 should be first element due to the swapback");

                first = 99;
                Assert.That(l.IndexOf(99) == 0, "The ElementAt takes a reference to the variable instead of a copy");
                Assert.That(l.IndexOf(int.MaxValue) == -1, "int.MaxValue was never added to the list.");
                l[0] = 20;
                Assert.That(l[0] == 20, "The [] operator gets & sets");
                
                // Testing removal and clears
                int index = l.Count / 2;
                l.RemoveAt(index);
                Assert.That(l.Count == l.Capacity - 3, "A third element should have been removed");

                l.Clear();
                Assert.That(l.Count == 0, "The list should be emptied.");
                Assert.That(l.IndexOf(0) == -1, "An empty list should not have any index");
                
                // Testing the other constructor with a preset length
                NoAllocList<int> l2 = new NoAllocList<int>(s, 2);
                Assert.That(l2.Count == 2, "Using the 2 parameter constructor sets the Length automatically.");
            });
        }
    }
}