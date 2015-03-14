# Introduction #

We offer two versions of the factorial function implementation, both non-recursive. The first version uses only the integer datatype and therefore can handle only a small range of input values (since the factorial function grows very quickly). The second version makes use of the `BigInt` datatype and the function ("constructor") with the same name, defined in a library module `BigNumbers`.

Due to the commutativity and associativity of multiplication, **foldl** could be replaced with **foldr**.
# Code #
First version:
```
(! integer) = { n: integer -> foldl x, y in [1 .. n] into (x * y) }!
```
Second version:
```
use BigNum: Math/BigNumbers
. . .
(! integer) = { n: integer -> foldl x, y in (array @BigNum.BigInt(i + 1) by i to n) into ((x * y) with @BigNum) }!
```