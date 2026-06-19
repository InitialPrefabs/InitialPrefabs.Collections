using System;
using System.Collections.Generic;

namespace InitialPrefabs.Collections {
    public ref struct NoAllocPriorityQueue<TItem, TPriority, TComparer> where TComparer : struct, IComparer<TPriority> {
        public int Count {
            get => count;
        }
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