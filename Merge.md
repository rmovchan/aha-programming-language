# Introduction #

Define a function that merges two ascending sequences of integers (possibly, infinite) into an ascending sequence. When one of the sequences ends, the rest of the result sequence will consist of the remaining items from the other sequence.

# Code #
```
merge =
    { s1: integer*, s2: integer* ->
        obj
            { state: [ p: integer* q: integer* ] -> 
                       `composite internal state consisting of integer sequences p, q`
                min where `return lowest of first items`
                    any
                        min = state.p^ when any state.p^ <= state.q^? not state.q^? end!
                        min = state.q^ when any state.p^ >= state.q^? not state.p^? end!
                    end
            } [ p: s1; q: s2; ] `initialize p, q with original sequences`
        skip:
            [ p: p; q: q; ] where `define p, q`
                all
                    any `define p`
                        let 
                            p = prev.p.skip! 
                        when
                            any 
                                prev.p^ <= prev.q^? 
                                not prev.q^? `end of sequence q?`
                            end
                        let p = prev.p! when prev.p^ > prev.q^?
                    end
                    any `define q`
                        let 
                            q = prev.q.skip! 
                        when
                            any 
                                prev.p^ >= prev.q^? 
                                not prev.p^? `end of sequence p?`
                            end
                        let q = prev.q! when prev.p^ < prev.q^?
                    end
                end
        end
    }!
```