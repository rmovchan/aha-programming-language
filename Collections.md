# Introduction #

This module introduces the basic collection types and variables from which 'dynamic' collections can be created.

`Stack` and `Queue` are both pre-defined instances of type `DynamicSequence` that can be used for building LIFO and FIFO structures, respectively.
# Specification #
```
doc 
    Title:   "Collections"
    Purpose: "Generic collections: dynamic arrays, stacks, queues"
    Package: "Aha! Base Library"
    Author:  "Roman Movchan"
    Created: "2010-10-14"
end

type Item: arbitrary "collection item"

type DynamicArray: 
    obj [Item]
        add(Item) "add new item"
        replace([ at: integer item: Item ]) "replace item at index"
        insert([ at: integer item: Item ]) "insert item at index"
        swap([ first: integer second: integer ]) "swap two items"
        move([ from: integer to: integer ]) "move item to new position"
        delete(integer) "delete item at index"
    end "a dynamic array"

type DynamicSequence:
    obj Item
        add(Item) "add an item"
        skip "remove first item"
    end "a dynamic sequence; compatible with Item*"

export 
    [
        DynamicArray: DynamicArray "zero-length dynamic array"
        DynamicArrayOf: { [Item] -> DynamicArray } "dynamic array containing given items"
        Stack: DynamicSequence "empty stack"
        Queue: DynamicSequence "empty queue"
    ]
```

# Examples #
```
use Coll: Base/Collections(Item: integer)
  .  .  .
@Coll.Stack.add(1).add(2).add(3).add(4)^.skip^ ` returns 3 `
  .  .  .
@Coll.Queue.add(1).add(2).add(3).add(4)^.skip^ ` returns 2 `
  .  .  .
a(1) where
    a = @Coll.DynamicArray.add(1).add(2).add(3).add(4)^! ` a = [1 2 3 4] `
 ` returns 2 `
```
See also [Nodes](Nodes.md).