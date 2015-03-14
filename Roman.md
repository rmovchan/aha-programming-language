# Introduction #

A function that converts an arbitrary natural number less than 4000 to the corresponding Roman numeral. For example, `Roman(28)` returns string `"XXVIII"`.

# Code #

Object `numerals` represents the sequence of base numerals that sum up into given `value`, in descending order; function `next` finds the index of the next such numeral in array `Romans`, each element in which corresponds to an integer in `Arabics`. The `list` expression turns the sequence into an array, and `foldl` then joins its elements into one string.

```
Roman = `convert an integer to Roman notation`
    { value: integer ->
            `join all base numerals in sequence 'numerals', e.g. ["X", "X", "V", "I", "I", "I"] -> "XXVIII"`
        (
            foldl result, numeral
            in
              (
                list numerals to (3 * Romans#) `each base numeral is repeated at most 3 times`
                where
                    let numerals = `relevant base numerals in descending order, e.g "X", "X", "V", "I", "I", "I"`
                        obj { state: [ rem: integer index: integer ] -> Romans(state.index) }
                            [ rem: value; index: next(value, Romans# - 1); ] `start from largest index`
                        skip:
                            [ rem: rem; index: next(rem, prev.index);  ] `when rem = 0, 'next' fails and sequence ends`
                                where rem = prev.rem - Arabics(prev.index)!
                        end !
                    where
                        next = `find index of max base numeral not exceeding x at or before given index`
                            { x: integer, index: integer ->
                                (first i in seq index, (prev - 1) to index that Arabics(i) <= x?) when x > 0?
                            }! 
              )
            into join result, numeral end
        ) where `define Romans, Arabics`
            all
                Romans  = ["I", "IV", "V", "IX", "X", "XL", "L", "XC", "C", "CD", "D", "CM", "M"]! `base Roman numerals`
                Arabics = [  1,    4,   5,    9,  10,   40,  50,   90, 100,  400, 500,  900,1000]! `corresponding Arabics`
                value > 0? `value must be positive, otherwise fail`
                value < 4000? `if value is too large, fail`
            end
    }!
```