using System;
using NUnit.Framework;

namespace InitialPrefabs.Collections.Tests {

    public class NoAllocHashSetTests {
        [Test]
        public void FullHashSetTest() {
            Assert.Multiple(() => {
                Span<byte> _bytes = stackalloc byte[3];
                NoAllocBitArray bitArray = new NoAllocBitArray(_bytes);
                Span<int> _ints = stackalloc int[10];
                NoAllocHashSet<int> h = new NoAllocHashSet<int>(_ints, bitArray);

                Assert.That(h.Contains(0) == false, "An empty hashset contains nothing.");

                for (int i = 0; i < _ints.Length; i++) {
                    Assert.That(h.TryAdd(i), "Failed to add to the hashset.");
                    Assert.That(h.Contains(i + 1) == false, "The next element was not added to the collection.");
                    Assert.That(h.TryAdd(i) == false, "Adding the same element twice should not work.");
                    Assert.That(h.Contains(i), $"The hashset should include the value: {i}");
                    Assert.That(h.Count == i + 1, "Failed to add the element to the hashset.");
                }
                Assert.That(h.Contains(11) == false, "11 should not exist");
                Assert.That(h.TryAdd(11) == false, "Adding should not be allowed to a max capacity hashset.");

                Span<int> copy = stackalloc int[10];
                h.FillSpan(ref copy);
                for (int i = 0; i < copy.Length; i++) {
                    Assert.That(copy[i] == i, "Failed to copy the hashset to the span.");
                }

                Assert.That(!h.TryAdd(int.MaxValue), "The hashset capacity has been reached!");
                h.Clear();
                Assert.That(h.Count == 0, "The hashset should be emtpy");
            });
        }
    }
}