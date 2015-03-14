# Introduction #

This function gets a (potentially infinite) sequence of lines (strings) and merges them into a flat sequence of characters, inserting a given special character after each "line".


# Code #
flatten =
    { source: [character]*, eol: character ->
        obj
            { state: [ index: integer rest: [character]* ] -> 
             `internal state consists of the rest of sequence and char index in the first line`
                ( ch where `external state is a character`                  
                    unless
                        ch = state.rest^(state.index)! `get current char from first line, if present`
                    then
                        ch = eol! `otherwise, return end-of-line char`
                )
            } [ index: 0; rest: source; ] `start from the first char of the first line in sequence`
        skip:
            [ index: index; rest: rest; ] where `define index, rest`
                unless
                    let
                        all
                            index = prev.index + 1! `get next char`
                            rest = prev.rest! `in the same line`
                        end
                    when
                        prev.index < prev.rest^#? `unless line length exceeded`
                then `otherwise`
                    all
                        index = 0! `start from the beginning`
                        rest = prev.rest.skip! `of the next line in source sequence`
                    end
        end
    }!}}}```