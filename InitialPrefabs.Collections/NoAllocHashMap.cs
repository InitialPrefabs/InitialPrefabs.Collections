using System;
using System.Collections.Generic;

namespace InitialPrefabs.Collections {

    public ref struct NoAllocHashMap<K, V>
        where K : unmanaged
        where V : unmanaged {

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

        public bool TryAdd(K key, V item) {
            if (Count == Values.Length) {
                return false;
            }

            var hash = EqualityComparer<K>.Default.GetHashCode(key) & Mask;
            var idx = hash % Values.Length;

            for (var i = 0; i < Values.Length; i++) {
                var probe = (idx + i) % Values.Length;
                if (!OccupiedFlags[probe]) {
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