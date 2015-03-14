# Introduction #

Define a function that converts a sequence of characters into the corresponding sequence of tokens. Each token is an array of characters from the original sequence that go in the same order and conform to one of the Aha! syntax rules:
  1. identifier (starts with a letter and consists of letters, digits and underscore `_`)
  1. character literal (an identifier or arbitrary single character after `$`)
  1. named operator (an identifier after `~`)
  1. string literal (arbitrary characters enclosed in `'` or `"`)
  1. number (a string of digits 0-9, possibly with minus sign)
  1. operator symbol (a string of other non-whitespace characters, followed by a white space or a delimiter `(`, `)`, `[`, `]`, `{`, `}`, `.`, `#`, `^`, `;`, `,` , `````, `'` or `"`)
'

Any white space characters (space, tab, carriage return, line feed) between tokens are ignored. In addition, any text starting from a grave accent character(`````) to the end of line is considered a comment and is ignored by the tokenizer.

# Specification #
The tokenizer is represented as a separate module that contains one auxiliary data type (`Limits`) and exports a function.

The function has two parameters. The first one is the stream (sequence) of characters, which can be infinite, of course. The second one is a composite of two integers, `token` (the maximum length of a token) and `space` (the maximum size of a white space or comment). These numbers are needed to avoid infinite computation of the next token from an infinite character stream. The function returns a sequence of character arrays.
```
doc 
    Title:   "Tokenizer"
    Purpose: "Tokenize a character sequence"
    Author:  "Roman Movchan"
    Created: "2012-10-25"
end

type Limits: 
    [
        token: integer "maximum token length"
        space: integer "maximum size of white space between tokens"
    ] "limits to prevent infinite computations"

export { character*, Limits -> [character]* } 
```

# Implementation #
The result sequence is returned as an object with the only action, `skip` (standard for sequences). The internal state of it consists of the token returned and the remaining characters in the sequence. The state is the result of applying functions `extract` and `strip`, first to the source stream and then to the remaining characters.

The `extract` subroutine returns a composite, consisting of the first token in a character sequence and the remainder of the sequence. If the token's length exceeds the specified maximum, `extract` will fail. The `strip` subroutine simply skips any white space characters at the beginning of a sequence and returns the sequence without those characters. If more spaces than the specified maximum are encountered, `strip` will fail. Failure of one of these routines means that the result sequence of tokens ends.

The `last` subroutine returns the last character in a character array. The `feed` subroutine converts a character sequence into a sequence of composites, where `n`-th item consists of the first `n` characters in the sequence as an array, and the remaining characters (as a sequence). The way characters are accumulated (using the **join** construct) is possibly non-optimal and is chosen only for clarity; a more efficient but slightly more advanced option would be using an instance of `DynamicArray` from [Base/Collections](Collections.md). Functions `isLetter`, `isDigit`, `isSpace` and `isDelimiter` return **void** if the specified character belongs to the respective class of characters and fail if it doesn't. They also could be optimized by using function `CharsIn` from module [Base/Miscellaneous](Miscellaneous.md).

The suggested solution has no external dependencies whatsoever.

It is not relevant how the original character sequence is produced; it can be obtained, for example, from an external file, or generated programmatically. The code deals with an abstract stream (sequence) and doesn't require any adjustments to accommodate to a specific representation of it.

'
```
implement Example/Tokenizer

export
    { source: character*, maxLen: Limits ->
        `return the sequence of tokens:`
        obj { state: [ token: [character] rest: character* ] -> state.token } `return current token, keep remaining characters`
            extract(strip(source)) `first token` 
        skip:
            extract(strip(prev.rest)) `following tokens`
        end
        where `define extract, strip`
            all
                extract = `return first token and remaining characters`
                    { chars: character* ->
                        p where `of type (token: [character], rest: character*)`
                            unless
                                any
                                    p =
                                        (
                                          (
                                            first p
                                            in feed(chars).skip `start with two characters`
                                            to maxLen.token
                                            that last(p.token) = p.token(0)? `first and last are equal?` 
                                          ) when any chars^ = $quote? chars^ = $apostrophe? end `begins with a quote?`
                                        )! `quoted or double-quoted string`
                                    
                                    unless
                                        p =
                                            (
                                              (
                                                first p
                                                in feed(chars).skip `start with two characters`
                                                to maxLen.token
                                                that not isLetter(p.rest^)? `next char isn't a letter?`
                                              ) when isLetter(chars.skip^)? `begins with $ followed by a letter?`
                                            )! `character literal: $<identifier>`
                                    then
                                        p = feed(chars).skip^! `two-character token: $<character>`
                                    when chars^ = $$? `begins with $?`
                                    `character literal`
                                    
                                    p =
                                        (
                                          (
                                            first p
                                            in feed(chars)
                                            to maxLen.token
                                            that 
                                                not any `isn't followed by any of:`
                                                    isLetter(p.rest^)? `a letter`
                                                    isDigit(p.rest^)? `a digit`
                                                    p.rest^ = $_? `or an underscore?`
                                                end
                                          ) when isLetter(chars^)? `begins with a letter?`
                                        )! `identifier`
                                    
                                    p =
                                        (
                                          (
                                            first p
                                            in feed(chars).skip
                                            to maxLen.token
                                            that not isLetter(p.rest^)? `next char isn't a letter?`
                                          ) when chars^ = $~? `begins with ~?`
                                        )! `named operator`
                                    
                                    p =
                                        (
                                          (
                                            first p
                                            in feed(chars)
                                            to maxLen.token
                                            that not isDigit(p.rest^)? `isn't followed by a digit?`
                                          ) when  `begins with a digit or minus sign then digit?`
                                                any
                                                    isDigit(chars^)?
                                                    all chars^ = $-? isDigit(chars.skip^)? end
                                                end
                                        )! `number`
                                end `any`
                            then 
                                p = 
                                    first p
                                    in feed(chars)
                                    to maxLen.token
                                    that any isDelimiter(p.rest^)? isSpace(p.rest^)? end
                                        `is followed by a space or delimiter?`
                                    ! `operator symbol`
                    }!
                strip = `strip a character sequence of leading whitespaces and comments:`
                    { chars: character* ->
                        first s `first subsequence`
                        in 
                            seq chars, 
                                ( next where
                                    unless 
                                        next = `skip line comment`
                                            (
                                                first s `skip all characters`
                                                in seq prev.skip, prev.skip  
                                                to maxLen.space
                                                that any s^ = $CR? s^ = $LF? not s^? end `to the end of line or stream`
                                                when prev^ = $backtick? `begins with a grave accent?`
                                            )! 
                                    then next = prev.skip!
                                )
                        to maxLen.space
                        that not any isSpace(s^)? s^ = $backtick? end 
                            `not having a whitespace or comment at the beginning`
                    }!
            end
            where `define last, feed, isLetter, isDigit, isSpace and isDelimiter`
                all
                    last = { str: [character] -> str(str# - 1) }! `return last character in a string`
                    feed = `return the sequence of possible tokens and remaining characters:`
                        { chars: character* ->
                            seq [ token: [chars^]; rest: chars.skip; ], `start with one character`
                                [ token: join prev.token, [prev.rest^] end; rest: prev.rest.skip; ]
                                `move first character of rest to end of token`
                        }!
                    isLetter = 
                        { ch: character -> 
                            ( void when forsome l in letters, ch = l? ) where 
                                letters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ"!
                        }!
                    isDigit = 
                        { ch: character -> 
                            ( void when forsome d in digits, ch = d? ) where 
                                digits = "0123456789"!
                        }!
                    isSpace = 
                        { ch: character -> 
                            void when forsome space in [$space, $tab, $CR, $LF], ch = space? 
                        }!
                    isDelimiter = 
                        { ch: character -> 
                            void when forsome d in "()[]{},.;#^`", ch = d? 
                        }!
                end
    }
```