# Introduction #

This module introduces various string handling utilities.


# Spec #

```
doc
    Title:   "StrUtils"
    Package: "Aha! Base Library"
    Purpose: "String utilities"
    Author:  "Roman Movchan, Melbourne, Australia"
    Created: "2012-06-03"
end

type String: [character] "character string"
type Substring:
    [
        index: integer "first character index"
        length: integer "substring length"
    ] "identifies a substring inside a string"
type RegEx: opaque "a regular expression"
type Pattern: 
    [
        string: String "exact search string" |
        regEx: RegEx "regular expression" |
    ] "search pattern"
type SearchParams:
    [
        for: Pattern "search pattern"
         in: String "where to search"
    ] "search parameters"
type ReplaceParams:
    [
        in: String "where to replace"
        at: Substring "what to replace"
        with: String "new text"
    ] "replace parameters"

export 
    [
        Substr: { String, Substring -> String } "extract substring from string"
        RegEx: { String -> RegEx } "construct regular expression from string"
        Search: { SearchParams -> Substring* } "return all occurrences of search pattern in string as a sequence"    
        Replace: { ReplaceParams -> String } "replace a substring in a string"
        StringHashFunc: { String -> integer } "standard hash function for strings"
    ]
```

# Examples #
```
@StrUtils.Search([ for: [ string: "www."; ]; in: "http://www.google.com/"; ])^ ` returns [ index: 7; length: 4; ] `

@StrUtils.Substr("http://www.google.com/", [ index: 7; length: 3; ]) ` returns "www" ` 

@StrUtils.Replace([ in: "http://www.google.com/"; at: [ index: 7; length: 3; ]; with: "code"; ]) ` returns "http://code.google.com/" `
```