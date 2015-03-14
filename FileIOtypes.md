# Introduction #

This module introduces several data types used by module [FileAccess](FileAccess.md).


# Specification #

```
doc 
    Title: "FileIO"
    Purpose: "File I/O (binary and text)"
    Package: "Application Program Interface"
    Author: "Roman Movchan, Melbourne, Australia"
    Created: "2013-09-05"
end

use Time: Base/Time
type String: [character] "alias for character array type"
export Types:
    type FileInfo:
        [
            fileType: String "file type"
            modified: @Time!Timestamp "date/time of last modification"
            size: integer "size in bytes"
        ] "detailed file information"
    type ErrorKind:
        [
            access: "access denied" |
            permanent: "permanent I/O error" |
            notFound: "file doesn't exist" |
            nameClash: "file name already exists" |
            invalidPath: "file path is invalid" |
            outOfMemory: "out of memory" |
            other: "other"
        ] "error kind"
    type ErrorInfo: 
        [
            kind: ErrorKind "error kind"
            message: String "text message"
        ] "error information"
    type Encoding:
        [
            ANSI: "ANSI" |
            UTF8: "UTF-8" |
            UCS2LE: "UCS-2 Little Endian" |
            UCS2BE: "UCS-2 Big Endian" |
            auto: "automatic"
        ] "text encoding"
end
```