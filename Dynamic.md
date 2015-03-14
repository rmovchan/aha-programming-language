# Introduction #

This module introduces the Dynamic data type, which effectively implements the dynamic typing. Using the dynamic typing reduces both the type safety and the performance and therefore should only be used where necessary - for example, to pass the data externally.

See also [Conversions](Conversions.md).

# Specification #

```
doc 
    Title: "Dynamic"
    Purpose: "Dynamic typing"
    Package: "Aha! Base Library"
    Author: "Roman Movchan, Melbourne, Australia"
    Created: "2013-04-06"
end

type String: [character] "alias for character array type"

use Rat: Base/Rational
use Float: Base/Float
use Time: Base/Time
use Money: Base/Money
use Bits: Base/Bits
use Formatting: Base/Formatting

type Dynamic: opaque "a dynamic value"
type NameValue: [ name: String "field name" value: Dynamic "associated value" ] "a name-value pair"
type Record: [NameValue] "a record, i.e. an array of name-value pairs"

export 
    [
        Names: { Dynamic -> [String] } "list all names in record; fail if not a record"
        Values: { Dynamic -> [Dynamic] } "list all values in record; fail if not a record"
        Null: Dynamic "pre-defined null value"
        DynamicToJSON: { Dynamic, Formatting::Format -> String } "convert dynamic to JSON using format settings"
        DynamicToXML: { Dynamic, Formatting::Format -> String } "convert dynamic to XML using format settings"
        JSONToDynamic: { String, Formatting::Deformat -> Dynamic } "convert a JSON-format string to dynamic"
        XMLToDynamic: { String, Formatting::Deformat -> Dynamic } "convert an XML-format string to dynamic"

        (~dyn integer): { integer -> Dynamic } "convert integer to dynamic"
        (~dyn String): { String -> Dynamic } "convert string to dynamic"
        (~dyn Rat::Rational): { Rat::Rational -> Dynamic } "convert rational to dynamic"
        (~dyn Float::Float): { Float::Float -> Dynamic } "convert float to dynamic"
        (~dyn Time::Timestamp): { Time::Timestamp -> Dynamic } "convert timestamp to dynamic"
        (~dyn Time::Interval): { Time::Interval -> Dynamic } "convert interval to dynamic"
        (~dyn Money::Money): { Money::Money -> Dynamic } "convert money to dynamic"
        (~dyn Bits::Bits): { Bits::Bits -> Dynamic } "convert bits to dynamic"
        (~dyn [Dynamic]): { [Dynamic] -> Dynamic } "convert array of dynamic to dynamic"
        (~dyn Record): { Record -> Dynamic } "convert record to dynamic"
        (Dynamic @ String): { Dynamic, String -> Dynamic } "get value associated with given name; fail if given name is not found in record or dynamic isn't a record"
        (String = Dynamic): { String, Dynamic -> NameValue } "obtain a name-value pair from its parts"
        (Dynamic ~with NameValue): { Dynamic, NameValue -> Dynamic } "alter value of a field; fail if given name is not found in record or dynamic isn't a record"
        (Dynamic = Dynamic): { Dynamic, Dynamic } "equality relation for dynamic"
        (~struc Dynamic): 
            { Dynamic ->
                [
                    null: |
                    int: integer |
                    string: String |
                    ratio: Rat::Rational |
                    float: Float::Float |
                    time: Time::Timestamp |
                    interval: Time::Interval |
                    money: Money::Money |
                    bits: Bits::Bits |
                    arr: [Dynamic] |
                    record: Record
                ]
            } "dynamic data as a variant composite"
    
    ]
```


# Examples #

```
use Dyn: Base/Dynamic
use Time: Base/Time

((~struc n).null where n = ~dyn 111!) with @Dyn? `fails, because n isn't null`

all
    myname = (~struc (rec @ "name")).string with @Dyn! `myname is "Roman Movchan"`
    (~struc `convert dynamic record to a composite`
        (
            (
                rec ~with ("D.O.B." = Null) `alter field 'D.O.B.' to null`
            ) @ "D.O.B." `extract field 'D.O.B.'`
        )
    ).null with @Dyn? `succeeds`
where
    rec = 
        ~dyn 
            [
                "name" = ~dyn "Roman Movchan", `first pair is name-string`
                "D.O.B." = ~dyn(~date [ year: 1960; month: 9; day: 11; ]) `second pair is name-date`
            ] with @Dyn, @Time! `create a record with dynamic fields`
end
```