# Introduction #
This example demonstrates using an object to generate a sequence.

The problem is: to generate in increasing order the sequence 2, 3, 4, 5, 6, 8, 9, 10, 12 ... of all numbers divisible by no primes other than 2, 3 or 5. (Taken from E.W. Dijkstra's book 'A Discipline of Programming')

# Code #
To generate the sequence, we use an object with a composite state consisting of three integer fields: _i_, _j_, _k_. The field with the lowest value is returned (possibly, non-deterministically) as the value of the current item. The field values initially are 2, 3 and 5, respectively. When the next item is requested, the fields with the lowest value are chosen out of _i_, _j_ and _k_ and incremented: i by 2, j by 3 and k by 5.

```
obj 
    { state: [ i: integer j: integer k: integer ] -> 
        item where `return minimum of i, j, k`
            any
                 item = state.i when all state.i <= state.j? state.i <= state.k? end!
                 item = state.j when all state.j <= state.k? state.j <= state.i? end!
                 item = state.k when all state.k <= state.i? state.k <= state.j? end!
            end
    } [ i: 2; j: 3; k: 5; ]
skip:
    [ i: i; j: j; k: k; ] where `define new values of fields i, j, k`
        let
            all
                unless `define i`
                    i = (prev.i + 2) when all prev.i <= prev.j? prev.i <= prev.k? end! 
                then 
                    i = prev.i! 
                unless `define j`
                    j = (prev.j + 3) when all prev.j <= prev.k? prev.j <= prev.i? end!
                then 
                    j = prev.j! 
                unless `define k`
                    k = (prev.k + 5) when all prev.k <= prev.i? prev.k <= prev.j? end!
                then 
                    k = prev.k! 
            end
        when
             all prev.i + 2? prev.j + 3? prev.k + 5? end `fail if  one of i, j, k overflows`
end
```
Note the following about our solution:
  * any number of the sequence items can be generated, without requiring any additional memory
  * the only arithmetic operation used is addition
  * since type **integer** is finite, the result sequence will be finite too - that's why we check for overflow
# Second version #
Using the function `merge` from example [Merge](Merge.md), the program could look like:
```
merge(m2, merge(m3, m5)) `merge multiples of 2, 3 and 5`
where `define m2, m3, m5 and merge`
    all
        m2 = seq 2, (prev + 2)! `multiples of 2`
        m3 = seq 3, (prev + 3)! `multiples of 3`
        m5 = seq 5, (prev + 5)! `multiples of 5`
        merge = . . .
    end
```