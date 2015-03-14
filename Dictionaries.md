# Introduction #

This generic module introduces the type Dictionary for associative collections.


# Specification #
```
doc 
    Title:   "Dictionaries"
    Package: "Aha! Base Library"
    Purpose: "Associative arrays"
    Author:  "Roman Movchan, Melbourne, Australia"
    Created: "2011-08-12"
end

type Key: arbitrary "lookup key"
type Value: arbitrary "lookup value"

type Dictionary:
    [
        at: { Key -> Value } "returns Value for given Key"
        keys: [Key] "all keys in the dictionary"
        values: [Value] "all values in the dictionary"
    ] "a dictionary"
type DictionaryBuilder: 
    obj Dictionary
        add([ key: Key value: Value ]) "add a new Key-Value pair"
        replace([ key: Key value: Value ]) "replace Value for existing Key"
        remove(Key) "remove Value for the Key"
    end "a dictionary builder"
type HashFunc: { Key -> integer } "a hash function"
type Equality: { Key, Key } "a key comparison function"
type HashTableParams: 
    [
        hash: HashFunc "hash function" 
        equal: Equality "key comparison function"
        size: integer "table size"
    ] "hash table parameters"

export 
    [
        HashTable: { HashTableParams -> DictionaryBuilder } "hash table constructor"
    ]
```

# Examples #
```
use Dict: Base/Dictionaries(Key: [character], Value: [character])
use Str: Base/StrUtils
use Misc: Base/Miscellaneous
  .  .  .
dict.at("aha") where
    dict = 
        @Dict.HashTable([ hash: @Str.StringHashFunc; equal: engine.locale.SameText; size: size; ]) ` returns a DictionaryBuilder `
            .add([ key: "aha"; value: "Aha!"; ]) ` add some entries `
            .add([ key: "c"; value: "C"; ])
            .add([ key: "java"; value: "Java"; ])
          .  .  .
            ^ ` get Dictionary from DictionaryBuilder `
        !
    where
        size = (first p in @Misc.PrimesTo(1200) that p > 1000?)! ` size is first prime number greater than 1000`
` returns "Aha!" `
```