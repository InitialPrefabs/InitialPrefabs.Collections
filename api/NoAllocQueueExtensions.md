---
title: NoAllocQueueExtensions
tags: ["Tools", "API"]
---

# NoAllocQueueExtensions
## `namespace InitialPrefabs.Collections `

## `bool TryPeek<T>(this ref NoAllocQueue<T> queue, out T item)`

### Summary
 Attempts to look at the head of the queue if there are any items available. The queue to look at.The element at the head of the queue.

### Returns
**bool** - True, if there exists elements in the queue.

## `T Peek<T>(this ref NoAllocQueue<T> queue)`

### Summary
 Looks at the head of the queue regardless if there are any items available. The queue to look at. /// If no elements exist then this method will throw an exception via accessing an invalid element in the Span{T}.

### Returns
**T** - The element at the head of the queue.

## `bool TryEnqueue<T>(this ref NoAllocQueue<T> queue, T item)`

### Summary
 Attempts to push an element into the Queue. 

### Param
| Parameter | Description |
|-----------|-------------|
| **queue** | The queue to push the element into |
| **item** | The item to push to the queue. |

### Returns
**bool** - True, if the element has been successfully pushed.

## `void Enqueue<T>(this ref NoAllocQueue<T> queue, T item)`

### Summary
 Pushes an element into the Queue. This does not check if the max capacity has been hit so any errors thrown by the Span{T} are propagated outwards. 

### Param
| Parameter | Description |
|-----------|-------------|
| **queue** | The queue to push into. |
| **item** | The element to push. |

## `bool TryDequeue<T>(this ref NoAllocQueue<T> queue, out T item)`

### Summary
 Attempts to remove the head from the queue. 

### Param
| Parameter | Description |
|-----------|-------------|
| **queue** | The queue to remove from. |
| **item** | The element that was recently removed. |

### Returns
**bool** - True, if the element has been removed.

## `bool Contains<T>(this ref NoAllocQueue<T> queue, in T item)`

### Summary
 Checks if an element exists in the queue. 

### Param
| Parameter | Description |
|-----------|-------------|
| **queue** | The queue to check. |
| **item** | The item to check if its in the queue. |

### Returns
**bool** - True, if the element is in the queue.

## `void Clear<T>(this ref NoAllocQueue<T> queue)`

### Summary
 Removes all elements in the queue. 

### Param
| Parameter | Description |
|-----------|-------------|
| **queue** | The queue to remove from. |


Go back to [API Home](_index)