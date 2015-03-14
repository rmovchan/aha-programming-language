# Introduction #

Defines function _Caesar_ that encodes a string using the Caesar cipher, where each lower-case Latin letter in a string is replaced by the letter three places up in the alphabet.

# Code #
```

Caesar =
    { str: [character] ->
        encodestr(3) where
            encodestr = `define the string encoding routine`
                { shift: integer ->
                    array encodechar(str(i)) by i to str# where
                        `result is a string of the same size as str`
                        encodechar = `routine that encodes single character`
                                { ch: character -> 
                                    newch where
                                        unless
                                            let newch = abc((base + shift) ~mod abc#)! where
                                                        `index in abc is base + shift by modulo of abc length`
                                                    base = (such i in [0 .. abc#) that ch = abc(i)?)!
                                                        `base is position of ch in abc`
                                        then
                                            newch = ch! `other characters remain unchanged`
                                } 
                            where 
                                abc = "abcdefghijklmnopqrstuvwxyz"!!
                }!
    }!
```
Note that the code doesn't use any unsafe type conversions (e.g. character-to-integer or integer-to-character). It can be easily changed to apply to any other alphabet by simply modifying the value assigned to variable _abc_.