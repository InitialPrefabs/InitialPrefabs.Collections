using System;
using System.Runtime.CompilerServices;

namespace InitialPrefabs.Collections {

    /// <summary>
    /// A stack only enumerator to iterate through the <see cref="NoAllocBitArray"/>.
    /// </summary>
    public ref struct NoAllocBitArrayEnumerator {
        internal Span<byte> Ptr;
        internal int Index;
        internal int Length;

        public NoAllocBitArrayEnumerator(NoAllocBitArray array) {
            Length = array.Length;
            Index = -1;
            Ptr = array.Bytes;
        }

        /// <summary>
        /// The current index within the <see cref="NoAllocBitArray"/>.
        /// </summary>
        public readonly bool Current {
            get {
                var remappedIndex = Index / 8;
                var accessor = 1 << (Index % 8);
                var b = Ptr[remappedIndex];
                return (b & accessor) > 0;
            }
        }

        /// <summary>
        /// Increments the iterator.
        /// </summary>
        /// <returns>True, until the iterator reaches the end of the <see cref="NoAllocBitArray"/>.</returns>
        public bool MoveNext() {
            return ++Index < Length;
        }

        /// <summary>
        /// Resets the enumerator back to the beginning.
        /// </summary>
        public void Reset() {
            Index = -1;
        }
    }

    /// <summary>
    /// A BitArray treats each bit as a boolean per byte. This means that each byte can store 8 booleans.
    /// </summary>
    public ref struct NoAllocBitArray {
        internal readonly Span<byte> Bytes;
        /// <summary>
        /// The total number of booleans that can be held in the array.
        /// </summary>
        public readonly int Length;

        public NoAllocBitArray(Span<byte> bytes) {
            Bytes = bytes;
            Length = bytes.Length * 8;
        }

        public bool this[int i] {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get {
                ref byte element = ref this.ElementAt(i);
                int accessor = i % 8;
                return ((1 << accessor) & element) > 0;
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set {
                ref byte element = ref this.ElementAt(i);
                var accessor = i % 8;
                byte mask = (byte)(1 << accessor);
                if (value) {
                    element |= mask;
                } else {
                    element &= (byte)~mask;
                }
            }
        }

        /// <summary>
        /// Calculates the total number of bytes the <see cref="NoAllocBitArray"/> can store.
        /// For example if you want to store 8 booleans, we will only need 1 byte.
        /// </summary>
        /// <param name="totalBools">The total number of booleans to store.</param>
        /// <returns>An integer to the nearest total number of bytes.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int CalculateSize(int totalBools) {
            return MathUtils.CeilToIntDivision(totalBools, 8);
        }

        public readonly NoAllocBitArrayEnumerator GetEnumerator() {
            return new NoAllocBitArrayEnumerator(this);
        }
    }

    public static class NoAllocBitArrayExtensions {
        /// <summary>
        /// Returns the byte associated with a specific index you want to access.
        /// </summary>
        /// <param name="array">The <see cref="NoAllocBitArray"/> to access.</param>
        /// <param name="i">The location of the bit to access.</param>
        /// <returns>A reference to the nearest byte.</returns>
        public static ref byte ElementAt(this ref NoAllocBitArray array, int i) {
            var index = i / 8;
            return ref array.Bytes[index];
        }
    }
}