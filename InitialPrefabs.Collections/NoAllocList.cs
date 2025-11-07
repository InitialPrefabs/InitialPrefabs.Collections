using System;
using System.Runtime.CompilerServices;

namespace InitialPrefabs.Collections {

    public ref struct NoAllocEnumerator<T> {
        internal Span<T> Ptr;
        internal int Index;
        internal int Length;
        public readonly T Current => Ptr[Index];

        public bool MoveNext() {
            return ++Index < Length;
        }

        public void Reset() {
            Index = -1;
        }
    }
    
    /// <summary>
    /// A stackonly list backed by a <see cref="Span{T}"/>. The capacity is fixed, however
    /// the total number of elements are tracked.
    /// </summary>
    public ref struct NoAllocList<T> where T : IEquatable<T> {
        public readonly Span<T> Span;
        public readonly int Capacity;
        public int Count { get; internal set; }

        public NoAllocList(Span<T> span) {
            Span = span;
            Capacity = span.Length;
            Count = 0;
        }

        public NoAllocList(Span<T> span, int count) {
            Span = span;
            Capacity = span.Length;
            Count = count;
        }

        public readonly NoAllocEnumerator<T> GetEnumerator() {
            return new NoAllocEnumerator<T> {
                Ptr = Span,
                Index = -1,
                Length = Count
            };
        }

        public readonly T this[int i] {
            get => Span[i];
            set => Span[i] = value;
        }
    }

    public static class NoAllocListExtensions {

        /// <summary>
        /// Resets the counter internally to 0. Does not clear out the memory so the elements still persist.
        /// As far as the list is aware, there is nothing stored.
        /// </summary>
        /// <typeparam name="T">Any type implementing <see cref="IEquatable{T}"/></typeparam>
        /// <param name="list">A reference to the <see cref="NoAllocList{T}"/></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Clear<T>(this ref NoAllocList<T> list) where T : IEquatable<T> {
            list.Count = 0;
        }

        /// <summary>
        /// Adds an element to the list.
        /// </summary>
        /// <typeparam name="T">Any type implementing <see cref="IEquatable{T}"/></typeparam>
        /// <param name="list">A reference to the <see cref="NoAllocList{T}"/> </param>
        /// <param name="item">An element to store into the list.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Add<T>(this ref NoAllocList<T> list, T item) where T : IEquatable<T> {
            if (list.Count >= list.Capacity) {
                return;
            }
            list.Span[list.Count++] = item;
        }

        /// <summary>
        /// Swaps the element at the index with the very last element of the list. 
        /// </summary>
        /// <typeparam name="T">Any type implementing <see cref="IEquatable{T}"/></typeparam>
        /// <param name="list">A reference to the <see cref="NoAllocList{T}"/> </param>
        /// <param name="index">The index to remove at</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void RemoveAtSwapback<T>(this ref NoAllocList<T> list, int index) where T : IEquatable<T> {
            list.Count--;
            var last = list.Span[list.Count];
            list.Span[index] = last;
        }

        /// <summary>
        /// Removes an element at the index and shifts all succeeding elements down 1.
        /// </summary>
        /// <typeparam name="T">Any type implementing <see cref="IEquatable{T}"/></typeparam>
        /// <param name="list">A reference to the <see cref="NoAllocList{T}"/> </param>
        /// <param name="index">The index to remove at</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void RemoveAt<T>(this ref NoAllocList<T> list, int index) where T : IEquatable<T> {
            list.Count--;
            for (var i = index; i < list.Count; i++) {
                list.Span[i] = list.Span[i + 1];
            }
        }

        /// <summary>
        /// Gets a reference to the element at the index.
        /// </summary>
        /// <typeparam name="T">Any type implementing <see cref="IEquatable{T}"/></typeparam>
        /// <param name="list">A reference to the <see cref="NoAllocList{T}"/> </param>
        /// <param name="index">The index to get the reference at</param>
        /// <returns>A reference to the element at the index</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref T ElementAt<T>(this ref NoAllocList<T> list, int index) where T : IEquatable<T> {
            return ref list.Span[index];
        }

        /// <summary>
        /// Attempst to find the element at an index.
        /// </summary>
        /// <typeparam name="T">Any type implementing <see cref="IEquatable{T}"/></typeparam>
        /// <param name="list">A reference to the <see cref="NoAllocList{T}"/> </param>
        /// <param name="item">The element to find in the list</param>
        /// <returns>The index of the element if found, otherwise -1 if it doesn't exist</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOf<T>(this in NoAllocList<T> list, T item) where T : IEquatable<T> {
            for (var i = 0; i < list.Count; i++) {
                var element = list[i];
                if (element.Equals(item)) {
                    return i;
                }
            }

            return -1;
        }
    }
}