# Introduction #

This module introduces the Money data type.


# Spec #

```
doc 
    Title:   "Money"
    Package: "Aha! Base Library"
    Purpose: "Operating with currency"
    Author:  "Roman Movchan, Melbourne, Australia"
    Created: "2012-06-02"
end

use Rational: Base/Rational

type Money: opaque "a money sum"

export 
    [
        MoneyCompare: { Money, Money -> integer } "negative - less, positive - more, zero - equal"

        (Money + Money): { Money, Money -> Money } "sum of two money sums"
        (Money - Money): { Money, Money -> Money } "difference between two money sums"
        (Money < Money): { Money, Money } "is first sum less than second?"
        (Money <= Money): { Money, Money } "is first sum less than or equal to second?"
        (Money = Money): { Money, Money } "is first sum equal to second?"
        (Money /= Money): { Money, Money } "is first sum not equal to second?"
        (Money >= Money): { Money, Money } "is first sum greater than or equal to second?"
        (Money > Money): { Money, Money } "is first sum greater than second?"
        (Money * Rational::Rational): { Money, Rational::Rational -> Money } "multiply money sum by a rational number"
        (~money integer): { integer -> Money } "convert integer to Money"
        (~money Rational::Rational): { Rational::Rational -> Money } "convert Rational to Money"
    ]
```

## Examples ##
```
( ~money (17014 / 100) ) with @Money, @Rational ` gives sum of 170.14 currency units `

(~money (99999 / 100)) > (~money 1000) with @Money, @Rational ? ` fails ` 
```