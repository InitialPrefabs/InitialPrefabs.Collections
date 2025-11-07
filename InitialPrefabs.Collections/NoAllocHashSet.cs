using System;
using System.Collections.Generic;

namespace InitialPrefabs.Collections {

    public ref struct NoAllocHashSet<T> where T : IEquatable<T> {
        /// <summary>
        /// Avoid negative integers
        /// </summary>
        private const int Mask = 0x7FFFFFFF;
        internal Span<T> Values;
        internal NoAllocBitArray OccupiedFlags; // Treat this using NoAllocBitArray
        public int Count => count;
        private int count;

        public NoAllocHashSet(Span<T> backing, NoAllocBitArray occupiedFlags) {
            Values = backing;
            OccupiedFlags = occupiedFlags;
            count = 0;
        }

        public readonly int FillSpan(ref Span<T> span) {
            int counter = 0;
            for (int i = 0; i < OccupiedFlags.Length; i++) {
                if (OccupiedFlags[i]) {
                    span[counter++] = Values[i];
                }
            }
            return counter;
        }

        public bool TryAdd(T item) {
            if (Count == Values.Length) {
                return false;
            }

            int hash = EqualityComparer<T>.Default.GetHashCode(item) & Mask;
            int idx = hash % Values.Length;

            for (int i = 0; i < Values.Length; i++) {
                int probe = (idx + i) % Values.Length;
                if (!OccupiedFlags[probe]) {
                    Values[probe] = item;
                    OccupiedFlags[probe] = true;
                    count++;
                    return true;
                }

                if (OccupiedFlags[probe] && EqualityComparer<T>.Default.Equals(Values[probe], item)) {
                    return false;
                }
            }
            return false;
        }

        public void Clear() {
            count = 0;
            OccupiedFlags.Bytes.Clear();
            Values.Clear();
        }

        public readonly bool Contains(T item) {
            if (count == 0) {
                return false;
            }

            int hash = EqualityComparer<T>.Default.GetHashCode(item) & Mask;
            int index = hash % Values.Length;

            for (int i = 0; i < Values.Length; i++) {
                int probe = (index + i) % Values.Length;
                if (!OccupiedFlags[probe]) {
                    return false;
                }

                if (EqualityComparer<T>.Default.Equals(Values[probe], item)) {
                    return true;
                }
            }
            return false;
        }
    }
}