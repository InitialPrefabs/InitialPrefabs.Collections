using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client.Interfaces;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace InitialPrefabs.Collections.Tests {
    
    public class NoAllocHashMapTests {

        [Test]
        public void HashmapThrowsExceptionOnInvalidKey() {
            Assert.Throws<KeyNotFoundException>(() => {
                Span<int> _keys = stackalloc int[10];
                Span<char> _values = stackalloc char[10];
                Span<byte> _occupyFlags = stackalloc byte[NoAllocBitArray.CalculateSize(10)];

                NoAllocHashMap<int, char> hashMap = new NoAllocHashMap<int, char>(_keys, _values, new NoAllocBitArray(_occupyFlags));
                Assert.That(hashMap.TryAdd(10, 'Z'));
                char actual = hashMap[10];
                Assert.That(actual == 'Z', "Should have stored Z with the key 10");
                hashMap[10] = 'A';
                actual = hashMap[10];
                Assert.That(actual == 'A');
                actual = hashMap[11];
            });
        }

        [Test]
        public void FullHashMapTest() {
            Assert.Multiple(() => {
                Span<int> _keys = stackalloc int[10];
                Span<char> _values = stackalloc char[10];
                Span<byte> _occupyFlags = stackalloc byte[NoAllocBitArray.CalculateSize(10)];
                NoAllocHashMap<int, char> hashMap = new NoAllocHashMap<int, char>(_keys, _values, new NoAllocBitArray(_occupyFlags));

                Assert.That(!hashMap.TryGetValue(0, out _), "An empty hash set should not contain any values stored");

                for (int i = 0; i < 10; i++) {
                    Assert.That(hashMap.TryAdd(i, (char)(i + 65)), "Failed to add a valid key and value");
                    Assert.That(!hashMap.TryAdd(i, (char)(i + 65)), "The key has already been added, so adding twice should fail");
                }
                Assert.That(hashMap.Count == 10, "Failed to add any key value pairs to the hashmap");
                Assert.That(!hashMap.TryAdd(11, (char)(11 + 65)), "A new entry should not be added because we had hit the capacity.");

                for (int i = 0; i < 10; i++) {
                    char expected = (char)(i + 65);
                    Assert.That(hashMap.TryGetValue(i, out char actual));
                    Assert.That(!hashMap.TryGetValue(i + 10, out _), "An invalid key should not successfully return anything");
                    Assert.That(actual == expected, $"Expected: {expected}, but received {actual}");

                    hashMap[i] = (char)(i + 75);
                    char c = hashMap[i];
                    Assert.That(c == (char)(i + 75));
                }

                hashMap.Clear();
                Assert.That(hashMap.Count == 0, "The hashmap should be empty!");

                for (int i = 0; i < 5; i++) {
                    Assert.That(hashMap.TryAdd(i, (char)(i + 65)), $"Failed to add key {i}");
                }
                Assert.That(!hashMap.TryGetValue(999, out _), "Should return false and hit an emtpy slot first");
            });
        }
    }
}