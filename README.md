# InitialPrefabs.Collections

This repository stores generic data structures that only exist in **stack** memory. The 
use of this is if you need to only use these data structures for a frame and do not want to 
allocate on the heap.

All data structures use a supplied `Span<T>` that must be stack allocated or represents a slice 
from the heap.

## Data Structures implemented so far:
* `NoAllocList<T>`
* `NoAllocQueue<T>`
* `NoAllocHashSet<T>`
* `NoAllocBitArray<T>`
* `NoAllocHashMap<T>`

## Planned
* `NoAllocStack<T>`

# Limitations
* The `Span<T>` must fit the stack memory. 
* Because all data structures implement using `ref struct`, you cannot pass `ref struct` to a new closure.
