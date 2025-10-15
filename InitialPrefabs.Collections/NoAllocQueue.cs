using System;
using System.Runtime.CompilerServices;

namespace InitialPrefabs.Collections {

    /// <summary>
    /// A queue that uses a <see cref="Span{T}"/> as it's internal data structure.
    /// </summary>
    public ref struct NoAllocQueue<T> where T : IEquatable<T> {
        internal readonly Span<T> Ptr;
        internal int Head;
        internal int Tail;
        public readonly int Capacity;

        public int Count { get; internal set; }
        public bool IsEmpty => Count == 0;

        public NoAllocQueue(Span<T> span) {
            Ptr = span;
            Capacity = span.Length;
            Head = 0;
            Tail = 0;
            Count = 0;
        }
    }

    public static class NoAllocQueueExtensions {
        
        /// <summary>
        /// Attempts to look at the head of the queue if there are any items available.
        /// <param name="queue">The queue to look at.</param>
        /// <param name="item">The element at the head of the queue.</param>
        /// </summary>
        /// <returns>True, if there exists elements in the queue.</returns>
        public static bool TryPeek<T>(this ref NoAllocQueue<T> queue, out T item) where T : IEquatable<T> {
            if (queue.Count > 0) {
                item = queue.Peek();
                return true;
            }

            item = default;
            return false;
        }

        /// <summary>
        /// Looks at the head of the queue regardless if there are any items available.
        /// <param name="queue">The queue to look at.</param>
        ///
        /// <remarks>If no elements exist then this method will throw an exception via accessing an invalid element in the Span{T}.</remarks>
        /// </summary>
        /// <returns>The element at the head of the queue.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Peek<T>(this ref NoAllocQueue<T> queue) where T : IEquatable<T> {
            return queue.Ptr[queue.Head];
        }
        
        /// <summary>
        /// Attempts to push an element into the Queue.
        /// </summary>
        /// <param name="queue">The queue to push the element into</param>
        /// <param name="item">The item to push to the queue.</param>
        /// <returns>True, if the element has been successfully pushed.</returns>
        public static bool TryEnqueue<T>(this ref NoAllocQueue<T> queue, T item) where T : IEquatable<T> {
            if (queue.Count == queue.Capacity) {
                return false;
            }

            queue.Ptr[queue.Tail] = item;
            queue.Tail = (queue.Tail + 1) % queue.Capacity;
            queue.Count++;
            return true;
        }
        
        /// <summary>
        /// Pushes an element into the Queue. This does not check if the max capacity has been hit so any 
        /// errors thrown by the <see cref="Span{T}"/> are propagated outwards.
        /// </summary>
        /// <param name="queue">The queue to push into.</param>
        /// <param name="item">The element to push.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Enqueue<T>(this ref NoAllocQueue<T> queue, T item) where T : IEquatable<T> {
            queue.Ptr[queue.Tail] = item;
            queue.Tail = (queue.Tail + 1) % queue.Capacity;
            queue.Count++;
        }

        /// <summary>
        /// Attempts to remove the head from the queue.
        /// </summary>
        /// <param name="queue">The queue to remove from.</param>
        /// <param name="item">The element that was recently removed.</param>
        /// <returns>True, if the element has been removed.</returns>
        public static bool TryDequeue<T>(this ref NoAllocQueue<T> queue, out T item) where T : IEquatable<T> {
            if (queue.Count > 0) {
                item = queue.Dequeue();
                return true;
            }

            item = default;
            return false;
        }

        /// <summary>
        /// Dequeues from the queue. This method does not check if there are any elements within the queue.
        /// </summary>
        /// <param name="queue">The queue to remove from.</param>
        /// <returns>The element that was removed from the queue.</returns>
        internal static T Dequeue<T>(this ref NoAllocQueue<T> queue) where T : IEquatable<T> {
            var element = queue.Ptr[queue.Head];
            queue.Head = (queue.Head + 1) % queue.Capacity;
            queue.Count--;
            return element;
        }

        /// <summary>
        /// Checks if an element exists in the queue.
        /// </summary>
        /// <param name="queue">The queue to check.</param>
        /// <param name="item">The item to check if its in the queue.</param>
        /// <returns>True, if the element is in the queue.</returns>
        public static bool Contains<T>(this ref NoAllocQueue<T> queue, in T item) where T : IEquatable<T> {
            for (int i = 0, index = queue.Head; i < queue.Count; i++) {
                if (queue.Ptr[index].Equals(item)) {
                    return true;
                }
                index = (index + 1) % queue.Capacity;
            }
            return false;
        }

        /// <summary>
        /// Removes all elements in the queue.
        /// </summary>
        /// <param name="queue">The queue to remove from.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Clear<T>(this ref NoAllocQueue<T> queue) where T : IEquatable<T> {
            queue.Head = 0;
            queue.Tail = 0;
            queue.Count = 0;
        }
    }
}