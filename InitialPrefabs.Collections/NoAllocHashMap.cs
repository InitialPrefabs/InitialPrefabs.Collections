using System;
using System.Collections.Generic;

namespace InitialPrefabs.Collections {

    /// <summary>
    /// A dictionary that stores a key value pair.
    /// </summary>
    /// <typeparam name="K">Any type implementing <see cref="IEquatable{T}"/></typeparam>
    /// <typeparam name="V">Any type</typeparam>
    public ref struct NoAllocHashMap<K, V> where K : IEquatable<K> {

        private const int Mask = 0x7FFFFFFF;
        internal Span<K> Keys;
        internal Span<V> Values;
        internal NoAllocBitArray OccupiedFlags;
        public int Count => count;
        private int count;

        public NoAllocHashMap(Span<K> keys, Span<V> backing, NoAllocBitArray occupiedFlags) {
            Keys = keys;
            Values = backing;
            OccupiedFlags = occupiedFlags;
            count = 0;
        }

        /// <summary>
        /// Attempts to add a value given a key if it does not exist.
        /// </summary>
        /// <param name="key">A unique identifier</param>
        /// <param name="item">The value to associate with the key</param>
        /// <returns>True, if successfully added</returns>
        public bool TryAdd(K key, V item) {
            if (Count == Values.Length) {
                return false;
            }

            var hash = EqualityComparer<K>.Default.GetHashCode(key) & Mask;
            var idx = hash % Values.Length;

            for (var i = 0; i < Values.Length; i++) {
                var probe = (idx + i) % Values.Length;
                if (!OccupiedFlags[probe]) {
                    Keys[probe] = key;
                    Values[probe] = item;
                    OccupiedFlags[probe] = true;
                    count++;
                    return true;
                }

                if (OccupiedFlags[probe] && EqualityComparer<K>.Default.Equals(Keys[probe], key)) {
                    return false;
                }
            }
            return false;
        }

        /// <summary>
        /// Attempts to get a value given a key.
        /// </summary>
        /// <param name="key">The unique id to look for</param>
        /// <param name="value">The value stored in the hash map</param>
        /// <returns>True, if successfully retrieved, otherwise false</returns>
        public bool TryGetValue(K key, out V value) {
            value = default;
            if (count == 0) {
                return false;
            }

            var hash = EqualityComparer<K>.Default.GetHashCode(key) & Mask;
            var index = hash % Values.Length;

            for (var i = 0; i < Keys.Length; i++) {
                var probe = (index + i) % Keys.Length;
                if (!OccupiedFlags[probe]) {
                    return false;
                }

                if (EqualityComparer<K>.Default.Equals(Keys[probe], key)) {
                    value = Values[probe];
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Removes all elements in the hashmap.
        /// </summary>
        public void Clear() {
            OccupiedFlags.Bytes.Clear();
            count = 0;
            Values.Clear();
            Keys.Clear();
        }

        public V this[K key] {
            get {
                if (TryGetValue(key, out var v)) {
                    return v;
                }
                throw new KeyNotFoundException($"Failed to find key: {key}");
            }
            set {
                if (!TryAdd(key, value)) {
                    // We need to update the existing value at the key.
                    var hash = EqualityComparer<K>.Default.GetHashCode(key) & Mask;
                    var index = hash % Values.Length;

                    for (var i = 0; i < Keys.Length; i++) {
                        var probe = (index + i) % Keys.Length;
                        if (OccupiedFlags[probe]) {
                            Values[probe] = value;
                            return;
                        }
                    }
                }
            }
        }
    }
}