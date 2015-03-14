# Introduction #

This module introduces generic trees (finite).

Note that in Aha! it's not possible to define a data type recursively.

See also [Nodes](Nodes.md).

'
# Spec #
```
doc 
    Title:   "Trees"
    Purpose: "Generic trees"
    Author:  "Roman Movchan, Melbourne, Australia"
    Created: "2013-08-10"
end

type Node: arbitrary "Tree node"

type Tree: opaque "abstract tree"
type Path: integer* "path to a sub-tree (sequence of child indexes), starting from the root"

export 
    [
        Tree: { [ root: Node children: [Tree] ] -> Tree } "create a tree with given root and children"
        Leaf: { Node -> Tree } "create a leaf from given node"

        Root: { Tree -> Node } "tree's root node"
        Children: { Tree -> [Tree] } "all children of a tree"
        NodesByLevel: { Tree -> Node* } "enumerate all tree nodes level by level"
        NodesByBranch: { Tree -> Node* } "enumerate all tree nodes branch by branch"
        PathsByLevel: { Tree -> Path* } "paths to all sub-trees level by level"
        PathsByBranch: { Tree -> Path* } "paths to all sub-trees branch by branch"
        Subtree: { Tree, Path -> Tree } "tree's sub-tree at given path"
        Ancestors: { Tree, Path -> Tree* } "all ancestors of sub-tree at given path, starting from its parent"
        LevelCount: { Tree -> integer } "tree's number of levels"
        NodeCount: { Tree -> integer } "tree's total number of nodes"
        InsertSubtree: { Tree, [ path: Path subtree: Tree ] -> Tree } "insert sub-tree into given tree at given path"
        ReplaceSubtree: { Tree, [ path: Path subtree: Tree ] -> Tree } "replace sub-tree of given tree at given path"
        DeleteSubtree: { Tree, Path -> Tree } "delete sub-tree of given tree at given path"
    ]
```
# Examples #
```
use Trees: Base/Trees(Node: integer)
 .  .  .

tree =  `create root with value 0 and two children`
    ( @Trees.Tree([ root: 0; children: [child1, child2]; ]) where
        all
            child1 = @Trees.Leaf(1)! `create a leaf with value 1`
            child2 = @Trees.Leaf(2)! `create a leaf with value 2`
        end
    )!

` Given the definition above,`
list @Trees.NodesByLevel(tree) to @Trees.NodeCount(tree) end `returns [0, 1, 2]`
@Trees.Root(children(1)) where children = @Trees.Children(tree)! `returns 2`
path^ where path = PathsByLevel(tree).skip^! `returns 0`
@Trees.LevelCount(tree) `returns 2`
```