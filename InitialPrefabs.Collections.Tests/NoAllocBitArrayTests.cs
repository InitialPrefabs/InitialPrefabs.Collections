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
}