---
title: NoAllocHashSet
tags: ["Tools", "API"]
---

# NoAllocHashSet
## `namespace InitialPrefabs.Collections `

## Summary
 A hashset that uses a Span{T} as its internal data structure. 

## TypeParam
Any type implementing IEquatable{T}

## `int FillSpan(ref Span<T> span)`

## `bool TryAdd(T item)`

### Summary
 Attempts to add an element to the HashSet if it does not exist. 

### Param
| Parameter | Description |
|-----------|-------------|
| **item** | The value to add |

### Returns
**bool** - True, if added

## `void Clear()`

### Summary
 Clears all elements within the NoAllocHashSet{T]}

## `bool Contains(T item)`

### Summary
 Checks if an element exists in the hash set. 

### Param
| Parameter | Description |
|-----------|-------------|
| **item** | The element to find |

### Returns
**bool** - True, if it exists, otherwise false


Go back to [API Home](_index)