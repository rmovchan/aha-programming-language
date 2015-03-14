# Introduction #

This module introduces various utilities.


# Specification #

```
doc 
    Title:   "Miscellaneous"
    Package: "Aha! Base Library"
    Purpose: "Miscellaneous utilities"
    Author:  "Roman Movchan, Melbourne, Australia"
    Created: "2012-06-02"
end

export 
    [
        SetOfCardinals:
            obj { integer }
                include(integer) "include cardinal number into the set"
                exclude(integer) "exclude cardinal number from the set"
            end "dynamic set of cardinal numbers"
        Random: { [ seed: integer limit: integer ] -> integer* } "generate a pseudo-random sequence of cardinals"
        PrimesTo: { integer -> integer* } "generate prime numbers up to given number"
        CharsIn: { [character] -> { character } } "set of characters in a given string"
    ]
```

# Examples #
```
use Misc: Base/Miscellaneous

set(3) where set = @Misc.SetOfCardinals.include(1).include(2).include(4)^!? ` fails (3 doesn't belong to set) `

@Misc.Random([ seed: 0; limit: 1000; ]).skip.skip.skip^ ` returns 4th random number in sequence `

@Misc.PrimesTo(100).skip.skip^ ` returns 5 `

chars($D) where chars = @Misc.CharsIn("abcdefgh")!? `fails (character D doesn't belong to set chars)`
```