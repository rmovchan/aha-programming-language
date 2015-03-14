# Introduction #

A non-recursive function that enumerates tree nodes level-by-level. See [Trees](Trees.md) for the specification.

# Code #
An object is used to represent the result sequence. The object's internal state is a queue of trees, its external state - the head tree's root node. Initially, the queue contains only the root tree; when the next item is requested, the head tree is removed from the queue and its children are added instead.

To obtain the nodes branch-by-branch, simply replace Queue with Stack; both are of type `DynamicSequence` defined in [Base/Collections](Collections.md).
```
use Trees: Base/Trees
use TreeColl: Base/Collections(Item: Trees::Tree)

NodesByLevel =
    { tree: Trees::Tree -> 
        obj { queue: TreeColl::DynamicSequence -> @Trees.Root(queue^) } `return root node of the queue's head tree`
            ` initially queue contains only the root tree `
            @TreeColl.Queue.add(tree)
        skip:
            item.queue where
                item =
                    (
                      (
                        first item
                        in `add head tree's children to queue`
                            seq [ queue: tail; child: 0; ],
                                [
                                    queue: prev.queue.add(children(prev.child)); `add child to queue`
                                    child: prev.child + 1; `look at next child`
                                ]
                        to children#
                        that item.child = children#? `no children left to process`
                      ) where children = @Trees.Children(head)!
                    ) where all head = prev^! tail = queue.skip! end! `pop the head tree from queue`
        end
    }!
```