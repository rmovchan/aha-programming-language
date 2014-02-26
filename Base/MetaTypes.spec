doc 
    Title: "Meta Types"
    Purpose: "Values describing Aha! types"
    Package: "Aha! Base Library"
    Author: "Roman Movchan, Melbourne, Australia"
    Created: "2013-08-10"
end

type String: [character] "alias for character array type"

export Types:
    type DataType: opaque "meta-type"
    type ComField: [ id: [ name: String | op: String ] "field name or operator" datatype: DataType "field type" ] "composite field descriptor"
    type ComVariant: [ComField] "variant of a composite" 
    type TypeInfo:
        [ 
            nilType: "nil" | 
            integerType: "integer" | 
            characterType: "character" | 
            arrayType: DataType "array" | 
            compositeType: [ComVariant] "composite type" |
            functionType: [ param: [ single: DataType | first: DataType second: DataType ] result: DataType ] "function" |
            objectType: [ state: DataType actions: [[ name: String param: DataType ]] ] "object or sequence" |
            opaqueType: "opaque"
        ] "type information"
end

export Utils:
    the TypeInfo: { DataType -> TypeInfo } "type info for given type"
    the Nesting: { DataType -> integer } "nesting level for given type; 0 for a simple type"
    the Nil: DataType "nil meta-type"
    the Integer: DataType "integer meta-type"
    the Character: DataType "character meta-type"
    the Array: { DataType -> DataType } "array of given type meta-type"
    the Seq: { DataType -> DataType } "sequence of given type meta-type"
    the Com: { [ComVariant] -> DataType } "composite meta-type"
    the Function: { [ param: [ single: DataType | first: DataType second: DataType ] result: DataType ] -> DataType } "function meta-type"
    the Object: { [ state: DataType actions: [[ name: String param: DataType ]] ] -> DataType } "object meta-type"
    (DataType <= DataType): { DataType, DataType } "covariance: does every value of first type belong to second?"
    (DataType >= DataType): { DataType, DataType } "contravariance: reverse to <="
    (DataType = DataType): { DataType, DataType } "are types equivalent (i.e.both <= and >=)?"
end