---
title: NoAllocPriorityQueue
tags: ["Tools", "API"]
---

# NoAllocPriorityQueue
## `namespace InitialPrefabs.Collections `

## Summary
 A priority queue is a queue that allows each item stored an assigned priority. The priority determines the order of service when an element is dequed. They can serve the highest priority or lowest priority based on the implementation of TComparer. 

## TypeParam
The element to store

## TypeParam
The assigned priority

## TypeParam
The comparison function to determine the service of order.

## `bool TryEnqueue(TItem item, TPriority priority)`

### Summary
 Attempts to enqueue an item to the queue. 

### Param
| Parameter | Description |
|-----------|-------------|
| **item** | The element to store. |
| **priority** | The assigned priority of the element, |

### Returns
**bool** - True, if successfully stored.

## `bool TryDequeue(out TItem item, out TPriority priority)`

### Summary
 Attempts to dequeue an item from the queue. 

### Param
| Parameter | Description |
|-----------|-------------|
| **item** | The element in the queue |
| **priority** | The associated priority of the queue |

### Returns
**bool** - True, if there are elements stored in the queue.

## `bool TryPeek(out TItem item, out TPriority priority)`

### Summary
 Attempts to look at the head of queue based on the assigned priority. 

### Param
| Parameter | Description |
|-----------|-------------|
| **item** | The element at the head of the queue. |
| **priority** | The associated priority of the queue. |

### Returns
**bool** - True, if there is an element at the end.


Go back to [API Home](_index)