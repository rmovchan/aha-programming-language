# Introduction #

Defines function _GCD_ that finds the Greatest Common Divisor using Euclidean algorithm. Both arguments must be positive, otherwise the function will fail.
# Code #
Demonstrates using a sequence generator (each item of the sequence is a pair of integers, i.e. a composite) and the **first** expression that finds the first pair in the sequence with equal fields.
The idea of the code can be expressed briefly as: find the first pair of integers, starting with (X, Y), with equal values of the fields, where each following pair consists of the smaller number and the difference between the larger and smaller numbers from the previous pair. This is just a declarative version of the well-known algorithm.

The upper estimate of X + Y iterations (never reached in practice) is based on the observation that either of the numbers decreases at least by 1 at each iteration but never goes below 0.

There are three different local variables with the same name _p_; this is allowed because their scopes don't intersect.

```
GCD =
    { X: integer, Y: integer -> 
        p.x where ` pick either of two equal remainders (x or y) `
            let p =
                first p `pair of remainders [ x: integer y: integer ] `
                in
                    seq [ x: X; y: Y; ], `initialize remainders`
                        p where
                            any `reduce whichever remainder is greater`
                                let p = [ x: prev.x - prev.y; y: prev.y; ]! when prev.x > prev.y?
                                let p = [ y: prev.y - prev.x; x: prev.x; ]! when prev.x < prev.y?
                            end
                to X + Y ` not more than X + Y iterations `
                that p.x = p.y? ` are remainders equal? `
                ! 
            when
                all X > 0? Y > 0? end `parameters must be positive, otherwise fail immediately `
    }!
```
An elegant alternative is the recursive solution:
```
GCD = 
    { X: integer, Y: integer ->
        gcd(X, Y) where
            let
                defrec
                    gcd: integer =
                        { x: integer, y: integer ->
                            z where
                                any
                                    let z = gcd(x - y, y)! when x > y?
                                    let z = gcd(x, y - x)! when x < y?
                                    let z = x! when x = y?
                                end
                        }
                to (X + Y)
            when
                all X > 0? Y > 0? end
    }!
```