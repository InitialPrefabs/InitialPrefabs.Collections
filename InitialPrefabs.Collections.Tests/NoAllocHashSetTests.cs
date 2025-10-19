using System;
using NUnit.Framework;

namespace InitialPrefabs.Collections.Tests {
    public class NoAllocBitArrayTests {
        [Test]
        public void FullNoAllocBitArrayTests() {
            Assert.Multiple(() => {
                Span<byte> _bytes = stackalloc byte[NoAllocBitArray.CalculateSize(1)];
                NoAllocBitArray bitArray = new NoAllocBitArray(_bytes);

                Assert.That(bitArray.Length == 4, "1 byte represents 4 booleans.");

                for (int i = 0; i < bitArray.Length; i++) {
                    bitArray[i] = true;
                    Assert.That(bitArray[i], "The flag should be set true.");
                }

                NoAllocBitArrayEnumerator it = bitArray.GetEnumerator();
                Assert.That(it.Index == -1, "The enumerator should start at -1");

                while (it.MoveNext()) {
                    Assert.That(it.Current, "The flag should be set to true");
                }
                it.Reset();
                Assert.That(it.Index == -1, "The reset should set the counter to -1");

                bitArray[0] = false;
                Assert.That(bitArray[0] == false, "Setting the first element to false should make it false");
            });
        }

        [Test]
        public void CalculateSizeTest() {
            Assert.Throws<DivideByZeroException>(() => { MathUtils.CeilToIntDivision(0, 0); });
        }

        [Test]
        public void MinTest() {
            Assert.Multiple(() => {
                Assert.That(MathUtils.Min(0, 0) == 0, "The min between 0 and 0 is 0");
                Assert.That(MathUtils.Min(0, 1) == 0, "The min between 0 and 1 is 0");
                Assert.That(MathUtils.Min(-1, 1) == -1, "The min between -1 and 1 is -1");
            });
        }
    }

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
                Assert.That(h.Contains(11) == false, ("11 should not exist"));
                Assert.That(h.TryAdd(11) == false, "Adding should not be allowed to a max capacity hashset.");

                Span<int> copy = stackalloc int[10];
                h.FillSpan(ref copy);
                for (int i = 0; i < copy.Length; i++) {
                    Assert.That(copy[i] == i, "Failed to copy the hashset to the span.");
                }
            });
        }
    }
}