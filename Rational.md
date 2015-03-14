# Introduction #

This module introduces the Rational data type.


# Spec #

```
doc 
    Title:   "Rational"
    Package: "Aha! Base Library"
    Purpose: "Rational numbers"
    Author:  "Roman Movchan, Melbourne, Australia"
    Created: "2012-06-02"
end

type Rational: opaque "a rational number"
type RatioStruc:
    [
        num: integer "numerator" 
        den: integer "denominator"
    ] "rational as composite"

export 
    [
        (integer / integer): { integer, integer -> Rational } "divide integers to get Rational"
        (~struc Rational): { Rational -> RatioStruc } "convert Rational to RatioStruc"
        (Rational + Rational): { Rational, Rational -> Rational } "sum of two rationals"
        (Rational - Rational): { Rational, Rational -> Rational } "difference between two rationals"
        (Rational * Rational): { Rational, Rational -> Rational } "product of two rationals"
        (Rational / Rational): { Rational, Rational -> Rational } "quotient of two rationals"
        (Rational < Rational): { Rational, Rational } "is first rational less than second?"
        (Rational <= Rational): { Rational, Rational } "is first rational less than or equal to second?"
        (Rational = Rational): { Rational, Rational } "is first rational equal to second?"
        (Rational /= Rational): { Rational, Rational } "is first rational not equal to second?"
        (Rational >= Rational): { Rational, Rational } "is first rational greater than or equal to second?"
        (Rational > Rational): { Rational, Rational } "is first rational greater than second?"
    ]
```

# Examples #
```
use Rat: Base/Rational

(1 / 3) with @Rat ` one third, a Rat::Rational (i.e. opaque) value `

(~struc ((1 / 2) * (4 / 5))) with @Rat ` returns [num: 2; den: 5;] `
```