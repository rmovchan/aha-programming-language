# Introduction #

This module introduces floating-point numbers and basic operations on them.


# Spec #
```
doc 
    Title:   "Float"
    Purpose: "Floating-point numbers"
    Package: "Aha! Base Library"
    Author:  "Roman Movchan"
    Created: "2010-10-16"
end

use Rational: Base/Rational

type Float: opaque "a floating-point number"
type FormatParams:
    [
        general:
            [
                period: character "character for decimal period"
            ] "general format" |
        fixed:
            [
                period: character "character for decimal period"
                decimals: integer "number of decimals"
            ] "fixed format" |
        exponent: 
            [
                period: character "character for decimal period"
            ] "exponential format"
    ] "number formatting parameters"

export 
    [
        (Float + Float): { Float, Float -> Float } "the sum of two floats"
        (Float - Float): { Float, Float -> Float } "the difference between two floats"
        (Float * Float): { Float, Float -> Float } "the product of two floats"
        (Float / Float): { Float, Float -> Float } "the ratio of two floats"
        (Float < Float): { Float, Float } "is first float less than second?"
        (Float <= Float): { Float, Float } "is first float less than or equal to second?"
        (Float = Float): { Float, Float } "is first float equal to second?"
        (Float /= Float): { Float, Float } "is first float not equal to second?"
        (Float >= Float): { Float, Float } "is first float greater than or equal to second?"
        (Float > Float): { Float, Float } "is first float greater than second?"
        (~float integer): { integer -> Float } "convert integer to Float"
        (~float Rational::Rational): { Rational::Rational -> Float } "convert Rational to Float"
        sin: { Float -> Float } "the sine function"
        cos: { Float -> Float } "the cosine function"
        exp: { Float -> Float } "the exponent function"
        log: { Float -> Float } "the logarithm function"
        tan: { Float -> Float } "the tangent function"
        Pi: Float "the pi number"
        Infinity: Float "+infinity"
        NegInfinity: Float "-infinity"
        Trunc: { Float -> integer } "truncate Float to integer"
        Round: { Float, integer -> Rational } "round Float to given number of decimals after decimal point"
        FloatToString: { Float, FormatParams -> [character] } "convert Float to string"
        StringToFloat: { [character], FormatParams -> Float } "convert string to Float"
    ]

```

# Examples #
```
@Float.FloatToString(value, fmt) where
    all
        value = sin(Pi / (~float 6)) with @Float!
        fmt = [ general: [ period: $.; ]; ]!
    end
 ` returns "0.5" `

(~struc Round(~float (9 / 10), 0)) with @Float, @Rational ` returns [ num: 1; den: 1; ] `

```