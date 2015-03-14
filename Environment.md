# Introduction #

This module provides data types used by an application to get information on the runtime environment (via the application's job engine).
'

# Specification #

```
doc 
    Title: "Environment"
    Purpose: "Static information on the runtime environment"
    Package: "Application Program Interface"
    Author: "Roman Movchan, Melbourne, Australia"
    Created: "2013-08-22"
end

type String: [character] "alias for character array type"

use Format: Base/Formatting
use StrUtils: Base/StrUtils
use Time: Base/Time
use Money: Base/Money
use Float: Base/Float

type Framework:
    [
        name: String "framework name"
        version:
            [
                major: integer
                minor: integer
                build: integer
            ] "framework version"
        components: [String] "classnames of all registered components"
    ] "runtime framework info"
    
type Locale:
    [
        GMToffset: Time::Interval "GMT offset"
        country: String "country name"
        language: String "language"
        currency: String "currency symbol(s)"
        decimal: character "decimal separator"
        format: Format::Format "formatting routines"
        deformat: Format::Deformat "deformatting routines"
        charCompare: StrUtils::CharCompare "character comparison function"
        upper: { character -> character } "upper case conversion"
        lower: { character -> character } "lower case conversion"
        (~str integer): { integer -> String } "convert integer to string"
        (~str Float::Float): { Float::Float -> String } "convert Float to string (local format)"
        (~str Time::Timestamp): { Time::Timestamp -> String } "convert Timestamp to string (local format)"
        (~str Money::Money): { Money::Money -> String } "convert Money to string (local format)"
        (~int String): { String -> integer } "convert string to integer"
        (~float String): { String -> Float::Float } "convert string (local format) to Float"
        (~date String): { String -> Time::Timestamp } "convert string (local format) to date"
        (~time String): { String -> Time::Interval } "convert string (local format) to time"
        (~timestamp String): { String -> Time::Timestamp } "convert string (local format) to timestamp"
        (~money String): { String -> Money::Money } "convert string (local format) to Money"
        (String <= String): { String, String } "compare string in local sorting order"
        (String < String): { String, String } "compare string in local sorting order"
        (String > String): { String, String } "compare string in local sorting order"
        (String >= String): { String, String } "compare string in local sorting order"
    ] "locale info"
    
type FilePath: opaque "file path"
type DirPath: opaque "directory (folder) path"

type FileSystem: 
    [
        eol: String "end-of-line characters"
        splitLines: { character* -> String* } "convert sequence of chars to sequence of lines"
        joinLines: { String* -> character* } "convert sequence of lines to sequence of chars"
        filePath: { DirPath, String -> FilePath } "get file path from file name and directory path"
        subDirPath: { DirPath, String -> DirPath } "get subdirectory path from its name and parent directory path"
        parentDirPath: { DirPath -> DirPath } "get parent directory path"
        fileName: { FilePath -> String } "get file name (with extension) from file path"
        fileDir: { FilePath -> DirPath } "get file's directory path"
        fileExt: { FilePath -> String } "extract file's extension (empty string if none)"
        changeExt: { FilePath, String -> FilePath } "change file's extension"
        splitDirPath: { DirPath -> [String] } "split directory path into string components"
        buildDirPath: { [String] -> DirPath } "build directory path from string components"
        workingDir: DirPath "directory where application can write data"
        appDir: DirPath "directory from which application has started"
        rootDir: DirPath "file system's root directory"
        (FilePath = FilePath): { FilePath, FilePath } "are paths the same?"
        (DirPath = DirPath): { DirPath, DirPath } "are paths the same?"
    ] "file system info and path handling routines"

type SystemInfo:
    [
        platform: 
            [
                Windows: " Windows" |
                MacOSX: "Mac OSX" |
                Linux: "Linux" |
                FreeBSD: "FreeBSD" |
                iOS: "iOS" |
                Android: "Android" |
                other: "other"
            ] "platform kind"
        username: String "user name"
        systemID: String "system identification"
        variables: [[ name: String value: String ]] "list of all environment variables and their values"
    ] "general information about the system"

export void
```

# Examples #
```
(~str (~money (19883 / 100)) with param.engine.locale, @Money, @Rational)` returns "$198.83" in Australian locale `

param.engine.locale.format.monthName.long((~date [ year: 2012; month: 6; day: 5; ] with @Time)) ` returns "June" in English-language locales`

param.engine.platform.Windows? `is application running on Windows?`

param.engine.fileSystem.eol `returns [$CR, $LF] on Windows`

forsome classname in param.engine.framework.components, classname = "User.Timer"? end? `does component 'User.Timer' exist?`

path = 
    ( var.value where
        var = such var in param.engine.systemInfo.variables that var.name = "PATH"? end!
    )! `get system variable PATH`
```