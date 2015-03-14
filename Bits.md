# Introduction #

This module introduces the data types and utilities for working with bits and bit strings.


# Spec #
```
doc 
    Title:   "Bits"
    Purpose: "Working with bits and bit strings"
    Package: "Aha! Base Library"
    Author:  "Roman Movchan"
    Created: "2012-06-07"
end

type BitString: opaque "a string of bits"
type BitStrCom:
    [
        at: { integer } "is bit at index non-zero?"
        size: integer "size of the string"
    ] "a structure that provides the info about a bit string"
type DynamicBitString:
    obj BitString
        append(BitString) "append a bit string"
        resize(integer) "set size to given value, pad with 0's"
        set(integer) "set bit at index to 1"
        reset(integer) "set bit at index to 0"
        invert "invert all bits in bit string"
    end "a dynamic (changeable) bit string"
type Substring:
    [
        index: integer "first bit index"
        length: integer"number of bits"
    ] "a structure representing a substring of a bit string"

export 
    [
        DynamicBitString: DynamicBitString "zero-length dynamic bit string"

        Substr: { BitString, Substring -> BitString } "extract a substring from bit string"
        True: BitString "bit string consisting of a single '1'"
        False: BitString "bit string consisting of a single '0'"
        Nil: BitString "a zero-length bit string"

        (BitString = BitString): { BitString, BitString } "are bit strings equal?"
        (BitString & BitString): { BitString, BitString -> BitString } "logical AND of two bit strings"
        (BitString | BitString): { BitString, BitString -> BitString } "logical OR of two bit strings"
        (BitString || BitString): { BitString, BitString -> BitString } "logical XOR of two bit strings"
        (- BitString): { BitString -> BitString } "inverse of a bit string"
        (BitString << integer): { BitString, integer -> BitString } "shift left, padding with zeros"
        (BitString >> integer): { BitString, integer -> BitString } "shift right, losing last bits"
        (BitString + BitString): { BitString, BitString -> BitString } "concatenation of two bit strings"
        (BitString * integer): { BitString, integer -> BitString } "repeat a bit string a number of times"
        (~bits character): { character -> BitString } "convert a character to a Unicode 16-bit string"
        (~bits [character]): { [character] -> BitString } "convert a string to Unicode"
        (~bits integer): { integer -> BitString } "convert an integer to a 64-bit string"
        (~struc BitString): { BitString -> BitStrCom } "convert a bit string to a structure"
        (~char BitString): { BitString -> character } "convert a 16-bit string to a character"
        (~int BitString): { BitString -> integer } "convert a 64-bit string to an integer"
    ]
```

# Examples #
```
(~int ((~bits 3) | (~bits 6))) with @Bits ` returns 7 `

( bits.at(bits.size - 1) `the last bit is 1?`
where
    bits = (~struc (~bits 7)) with @Bits! ` '111' `
)? `succeeds`

Ones = (~int (True * 64)) with @Bits! `integer value, consisting of 64 binary ones`

Ones = (~int(-(~bits 0))) with @Bits! `same as above`
```