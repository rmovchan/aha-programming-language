# Introduction #

These base modules provide dynamic access to the type information.


## Meta Types ##

```
doc 
    Title: "Meta Types"
    Purpose: "Values describing Aha! types"
    Package: "Aha! Base Library"
    Author: "Roman Movchan, Melbourne, Australia"
    Created: "2013-08-10"
end

type String: [character] "alias for character array type"

type DataType: opaque "meta-type"
type Attribute: [ id: [ name: String | op: String ] "attribute name or operator" datatype: DataType "attribute type" ] "composite's attribute descriptor"
type Variant: [Attribute] "variant of a composite"
type TypeInfo:
    [ 
        integerType: "integer" |
        characterType: "character" |
        arrayType: DataType "array" |
        compositeType: [Variant] "composite type" |
        functionType: [ param: [ single: DataType | first: DataType second: DataType ] result: DataType ] "function" |
        objectType: [ state: DataType actions: [[ name: String param: DataType ]] ] "object or sequence" |
        opaqueType: "opaque"
    ] "type information"

export 
    [
        TypeInfo: { DataType -> TypeInfo } "type info for given type"
        Nesting: { DataType -> integer } "nesting level for given type; 0 for a simple type"
        Void: DataType "void meta-type"
        Integer: DataType "integer meta-type"
        Character: DataType "character meta-type"
        Array: { DataType -> DataType } "array of given type meta-type"
        Seq: { DataType -> DataType } "sequence of given type meta-type"
        Com: { [Variant] -> DataType } "composite meta-type"
        Function: { [ param: [ single: DataType | first: DataType second: DataType ] result: DataType ] -> DataType } "function meta-type"
        Object: { [ state: DataType actions: [[ name: String param: DataType ]] ] -> DataType } "object meta-type"
        (DataType <= DataType): { DataType, DataType } "covariance: does every value of first type belong to second?"
        (DataType >= DataType): { DataType, DataType } "contravariance: reverse to <="
        (DataType = DataType): { DataType, DataType } "are types equivalent (i.e.both <= and >=)?"
    ]

```

## Meta ##

This is a generic module exporting the meta-type of its only type parameter.

```
doc 
    Title: "Meta"
    Purpose: "Get meta-type for a given data type"
    Package: "Aha! Base Library"
    Author: "Roman Movchan, Melbourne, Australia"
    Created: "2013-08-10"
end

use MetaTypes: Base/MetaTypes

type DataType: arbitrary "data type"

export MetaTypes::DataType 
```

# Examples #

```
type Association: [ key: [character] value: integer ] `user-defined composite type`
use Meta: Base/Meta(DataType: Association)
use MetaTypes: Base/MetaTypes
 . . .
( `build array of field names of Association`
    array meta(i).id.name by i to meta# end
where 
    meta = @MetaTypes.TypeInfo(@Meta).compositeType(0) ! `array of meta-data for all fields; first (and only) variant is considered`
) `returns ["key", "value"]`
 . . .
(( meta(0).datatype = Array(Character) with @MetaTypes) where meta = @MetaTypes.TypeInfo(@Meta).compositeType(0) ! )? `succeeds`

```
See also [TypeAsString](TypeAsString.md).