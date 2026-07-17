---
title: NoAllocBitArray
tags: ["Tools", "API"]
---

# NoAllocBitArray
## `namespace InitialPrefabs.Collections `

## Summary
 A BitArray treats each bit as a boolean per byte. This means that each byte can store 8 booleans. 

## `int CalculateSize(int totalBools)`

### Summary
 Calculates the total number of bytes the NoAllocBitArray can store. For example if you want to store 8 booleans, we will only need 1 byte. 

### Param
| Parameter | Description |
|-----------|-------------|
| **totalBools** | The total number of booleans to store. |

### Returns
**int** - An integer to the nearest total number of bytes.

## `NoAllocBitArrayEnumerator GetEnumerator()`


Go back to [API Home](_index)