# Introduction #

This generic module ensures the correct conversion of compatible data to dynamic and back. The base type must not contain functions, objects/sequences or non-standard opaque types, otherwise conversions will fail. There is also an auxiliary parameter-less function that checks if the base type is compatible with the Dynamic type.

See also [Dynamic](Dynamic.md).


# Specification #

```
doc 
    Title: "Conversions"
    Purpose: "Convert statically typed data to dynamic and back"
    Package: "Aha! Base Library"
    Author: "Roman Movchan, Melbourne, Australia"
    Created: "2013-08-15"
end

use Dynamic: Base/Dynamic
import Dynamic(Types)

type DataType: arbitrary "base data type"

export Everything:
    the ToDynamic: { DataType -> Dynamic } "convert statically typed data to a dynamic value; fail if data type contains objects/sequences, functions or non-standard opaque types"
    the FromDynamic: { Dynamic -> DataType } "convert dynamic value to statically typed data; fail if dynamic value doesn't match the data type"
    the Convertible: { } "fail if data type contains objects/sequences, functions or non-standard opaque data"
end
```

# Examples #
```
type MyData: [ fieldname: [character] "field name" data: [ int: integer | str: [character] ] "field data" ]
use Conv: Base/Conversions(DataType: MyData)
import Conv(Everything)
use Dynamic: Base/Dynamic
import Dynamic(Utils)
 . . .
(( data.fieldname where data = FromDynamic(var)! )
where
    var = ToDynamic([fieldname: "distance"; data: [ int: 100; ]; ])!
) `returns "distance"`
```