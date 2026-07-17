---
title: NoAllocListExtensions
tags: ["Tools", "API"]
---

# NoAllocListExtensions
## `namespace InitialPrefabs.Collections `

## `void Clear<T>(this ref NoAllocList<T> list)`

### Summary
 Resets the counter internally to 0. Does not clear out the memory so the elements still persist. As far as the list is aware, there is nothing stored. 

### TypeParam T
Any type implementing IEquatable{T}

### Param
| Parameter | Description |
|-----------|-------------|
| **list** | A reference to the NoAllocList{T} |

## `void Add<T>(this ref NoAllocList<T> list, T item)`

### Summary
 Adds an element to the list. 

### TypeParam T
Any type implementing IEquatable{T}

### Param
| Parameter | Description |
|-----------|-------------|
| **list** | A reference to the NoAllocList{T} |
| **item** | An element to store into the list. |

## `void RemoveAtSwapback<T>(this ref NoAllocList<T> list, int index)`

### Summary
 Swaps the element at the index with the very last element of the list. 

### TypeParam T
Any type implementing IEquatable{T}

### Param
| Parameter | Description |
|-----------|-------------|
| **list** | A reference to the NoAllocList{T} |
| **index** | The index to remove at |

## `void RemoveAt<T>(this ref NoAllocList<T> list, int index)`

### Summary
 Removes an element at the index and shifts all succeeding elements down 1. 

### TypeParam T
Any type implementing IEquatable{T}

### Param
| Parameter | Description |
|-----------|-------------|
| **list** | A reference to the NoAllocList{T} |
| **index** | The index to remove at |

## `ref T ElementAt<T>(this ref NoAllocList<T> list, int index)`

### Summary
 Gets a reference to the element at the index. 

### TypeParam T
Any type implementing IEquatable{T}

### Param
| Parameter | Description |
|-----------|-------------|
| **list** | A reference to the NoAllocList{T} |
| **index** | The index to get the reference at |

### Returns
**ref T** - A reference to the element at the index

## `int IndexOf<T>(this in NoAllocList<T> list, T item)`

### Summary
 Attempst to find the element at an index. 

### TypeParam T
Any type implementing IEquatable{T}

### Param
| Parameter | Description |
|-----------|-------------|
| **list** | A reference to the NoAllocList{T} |
| **item** | The element to find in the list |

### Returns
**int** - The index of the element if found, otherwise -1 if it doesn't exist


Go back to [API Home](_index)