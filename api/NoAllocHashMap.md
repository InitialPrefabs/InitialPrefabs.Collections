---
title: NoAllocHashMap
tags: ["Tools", "API"]
---

# NoAllocHashMap
## `namespace InitialPrefabs.Collections `

## Summary
 A dictionary that stores a key value pair. 

## TypeParam
Any type implementing IEquatable{T}

## TypeParam
Any type

## `bool TryAdd(K key, V item)`

### Summary
 Attempts to add a value given a key if it does not exist. 

### Param
| Parameter | Description |
|-----------|-------------|
| **key** | A unique identifier |
| **item** | The value to associate with the key |

### Returns
**bool** - True, if successfully added

## `bool TryGetValue(K key, out V value)`

### Summary
 Attempts to get a value given a key. 

### Param
| Parameter | Description |
|-----------|-------------|
| **key** | The unique id to look for |
| **value** | The value stored in the hash map |

### Returns
**bool** - True, if successfully retrieved, otherwise false

## `void Clear()`

### Summary
 Removes all elements in the hashmap. 


Go back to [API Home](_index)