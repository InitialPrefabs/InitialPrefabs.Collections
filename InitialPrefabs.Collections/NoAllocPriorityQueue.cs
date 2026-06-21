using System;
using System.Collections.Generic;

namespace InitialPrefabs.Collections {

    /// <summary>
    /// A priority queue is a queue that allows each item stored an assigned priority. The priority determines
    /// the order of service when an element is dequed. They can serve the highest priority or lowest priority 
    /// based on the implementation of <see cref="TComparer"/>.
    /// </summary>
    /// <typeparam name="TItem">The element to store</typeparam>
    /// <typeparam name="TPriority">The assigned priority</typeparam>
    /// <typeparam name="TComparer">The comparison function to determine the service of order.</typeparam>
    public ref struct NoAllocPriorityQueue<TItem, TPriority, TComparer> where TComparer : struct, IComparer<TPriority> {
        /// <summary>
        /// The total number of elements in the Queue.
        /// </summary>
        public int Count {
            get => count;
        }

        /// <summary>
        /// The maximum number of elements that can be stored in the queue.
        /// </summary>
        public int Capacity {
            get => items.Length;
        }

        private readonly Span<TItem> items;
        private readonly Span<TPriority> priorities;
        private readonly TComparer comparer;
        private int count;

        public NoAllocPriorityQueue(Span<TItem> items, Span<TPriority> priorities, TComparer comparer) {
            this.items = items;
            this.priorities = priorities;
            this.comparer = comparer;
            count = 0;
        }

        private void Swap(int a, int b) {
            (items[a], items[b]) = (items[b], items[a]);
            (priorities[a], priorities[b]) = (priorities[b], priorities[a]);
        }

        private void HeapifyUp(int index) {
            while (index > 0) {
                int parent = (index - 1) >> 1;

                if (comparer.Compare(priorities[index], priorities[parent]) >= 0) {
                    break;
                }

                Swap(index, parent);
                index = parent;
            }
        }

        private void HeapifyDown(int index) {
            while (true) {
                int left = (index << 1) + 1;

                if (left >= count)
                    return;

                int right = left + 1;
                int smallest = left;

                if (right < count &&
                    comparer.Compare(
                        priorities[right],
                        priorities[left]) < 0) {
                    smallest = right;
                }

                if (comparer.Compare(
                        priorities[index],
                        priorities[smallest]) <= 0) {
                    return;
                }

                Swap(index, smallest);
                index = smallest;
            }
        }

        /// <summary>
        /// Attempts to enqueue an item to the queue.
        /// </summary>
        /// <param name="item">The element to store.</param>
        /// <param name="priority">The assigned priority of the element,</param>
        /// <returns>True, if successfully stored.</returns>
        public bool TryEnqueue(TItem item, TPriority priority) {
            if (count >= Capacity) {
                return false;
            }

            int index = count++;
            items[index] = item;
            priorities[index] = priority;

            HeapifyUp(index);
            return true;
        }
        
        /// <summary>
        /// Attempts to dequeue an item from the queue.
        /// </summary>
        /// <param name="item">The element in the queue</param>
        /// <param name="priority">The associated priority of the queue</param>
        /// <returns>True, if there are elements stored in the queue.</returns>
        public bool TryDequeue(out TItem item, out TPriority priority) {
            if (count == 0) {
                item = default!;
                priority = default!;
                return false;
            }

            item = items[0];
            priority = priorities[0];

            count--;

            if (count > 0) {
                items[0] = items[count];
                priorities[0] = priorities[count];

                HeapifyDown(0);
            }

            return true;
        }

        /// <summary>
        /// Attempts to look at the head of queue based on the assigned priority.
        /// </summary>
        /// <param name="item">The element at the head of the queue.</param>
        /// <param name="priority">The associated priority of the queue.</param>
        /// <returns>True, if there is an element at the end.</returns>
        public bool TryPeek(out TItem item, out TPriority priority) {
            if (count == 0) {
                item = default;
                priority = default;
                return false;
            }

            item = items[0];
            priority = priorities[0];
            return true;
        }
    }
}