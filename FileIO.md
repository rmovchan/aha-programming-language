# Introduction #

This module introduces file I/O operations and is an important part of the Aha! [API](API.md).

`CreateReader` and `CreateWriter` provide asynchronous direct access to file content in the binary format. These functions return jobs that create 'controllers' which can be used to obtain jobs that perform I/O operations. The user can read file content from any position as a bit string or write at any position, including after the end, as a bit string (which contains a whole number of bytes). The `position` parameter is relative to the top of file or its bottom, in bytes (must not be negative); `next` reads/writes data sequentially. `close` releases the file for other readers/writers after all the I/O operations are complete (using a writer for writing locks the file for other readers and writers until the writer is closed).

Functions `ReadText` and `WriteText` provide simplified access to the content of text files. The entire content is passed as a sequence of characters. The user can also specify the character encoding both when reading and writing.

Functions `DirWatch` and `DeleteWatch` let the developer set and delete directory watches. A directory watch is a job that raises events when the content of the watched directory changes. The job runs until the watch is deleted.

The `error` parameter in the I/O routines is used to specify the error processing function. If an I/O error occurs when performing a job, this function is used (receiving the error information as argument) to obtain the event that is then sent to the application's/component's `handle` action.

The functions have an `Engine` parameter so that the file access routines can pass output to the client application/component. Type `Engine` is defined in [Jobs](Jobs.md).

To obtain file and directory paths, module [Environment](Environment.md) can be used. Types `FileInfo`, `ErrorInfo` and `Encoding` are defined in module [FileIOtypes](FileIOtypes.md).

Note: none of the functions directly affects the application's environment (file system). Instead, each of them returns a `Job` type value that needs to be passed to the runtime framework to be actually executed - obviously, via the state of the application's behavior object.

# Specification #
```
doc 
    Title: "FileIO"
    Purpose: "File I/O (binary and text) and file/directory management"
    Package: "Application Program Interface"
    Author: "Roman Movchan, Melbourne, Australia"
    Created: "2013-09-12"
end

type Event: arbitrary "custom event type"
use Jobs: API/Jobs(Event: Event)
use Env: API/Environment
use Bits: Base/Bits
use Types: API/FileIOtypes
type String: [character] "alias for character array type"
type ReadParams:
    [ 
        position: [ top: integer | bottom: integer | next: ] "position: relative to top (bytes), bottom (bytes) or current"
        bytes: integer "number of bytes" 
        result: { Bits::Bits -> Event } "event that receives bytes read"
    ] "read given number of bytes at given position" 
type WriteParams:
    [ 
        position: [ top: integer | bottom: integer | next: ] "position: relative to top (bytes), bottom (bytes) or current"
        data: Bits::BitString "data to write (must be whole number of bytes)" 
        written: Event "event raised upon writing"
    ] "write data at given position" 
type ReaderCommand:
    [
        read: ReadParams "read data" |
        close: Event "release file for write operations and raise event"
    ] "reader control commands"
type WriterCommand:
    [
        write: WriteParams "write data" |
        close: Event "release file for other operations and raise event"
    ] "writer control commands"
type Reader: { ReaderCommand -> Jobs::Job } "return reader jobs"
type Writer: { WriterCommand -> Jobs::Job } "return writer jobs"
type FileMngmt:
    [
        findFile: { [ path: Env::FilePath yes: Event no: Event ] -> Jobs::Job } "check if file with given path exists"
        getFileInfo: { [ path: Env::FilePath success: { Env::FileInfo -> Event } error: { Types::ErrorInfo -> Event } ] -> Jobs::Job } 
            "get detailed file information"
        makeFile: { [ path: Env::FilePath success: Event error: { Types::ErrorInfo -> Event } ] -> Jobs::Job } 
            "create an empty file"
        renameFile: { [ path: Env::FilePath to: String success: Event error: { Types::ErrorInfo -> Event } ] -> Jobs::Job } 
            "rename file"
        deleteFile: { [ path: Env::FilePath success: Event error: { Types::ErrorInfo -> Event } ] -> Jobs::Job } 
            "delete a file"
        findDir: { [ path: Env::DirPath yes: Event no: Event ] -> Job } "check if directory with given path exists"
        listDir: { [ path: Env::DirPath success: { [ files: [String] dirs: [String] ] -> Event } error: { Types::ErrorInfo -> Event } ] -> Jobs::Job } 
            "list files and subdirectories in given directory"
        makeDir: { [ path: Env::DirPath success: Event error: { Types::ErrorInfo -> Event } ] -> Jobs::Job } 
            "create a directory"
        renameDir: { [ path: Env::DirPath to: String success: Event error: { Types::ErrorInfo -> Event } ] -> Jobs::Job } 
            "rename directory"
        deleteDir: { [ path: Env::DirPath success: Event error: { Types::ErrorInfo -> Event } ] -> Jobs::Job } 
            "delete a directory"
    ] "file management jobs"
type DirChange:
    [
        newFile: String "new file created" |
        newDir: String "new subdirectory created" |
        modified: String "file modified" |
        deleted: String "file/subdirectory deleted" 
    ] "changes in a directory"
    
export 
    [
        CreateReader: { [ path: Env::FilePath engine: Jobs::Engine success: { Reader -> Event } error: { Types::ErrorInfo -> Event } ] -> Jobs::Job } "create file reader"
        CreateWriter: { [ path: Env::FilePath engine: Jobs::Engine success: { Writer -> Event } error: { Types::ErrorInfo -> Event } ] -> Jobs::Job } "create file writer"
        ReadText: 
                { 
                    [ 
                        path: Env::FilePath 
                        encoding: Types::Encoding 
                        success: { [ content: character* size: integer encoding: Types::Encoding ] -> Event } 
                        error: { Types::ErrorInfo -> Event } 
                        engine: Jobs::Engine
                    ] -> Jobs::Job 
                } "job that returns text file content as a character sequence"
        WriteText: 
                { 
                    [ 
                        path: Env::FilePath 
                        encoding: Types::Encoding 
                        content: character*
                        size: integer
                        success: Event
                        error: { Types::ErrorInfo -> Event } 
                        engine: Jobs::Engine
                    ] -> Jobs::Job 
                } "job that creates text file from a character sequence"
        FileMngmt: { Jobs::Engine -> FileMngmt } "obtain file management interface"
        DirWatch: { [ path: Env::DirPath watch: { DirChange -> Event } error: { Types::ErrorInfo -> Event } engine: Jobs::Engine ] -> Jobs::Job } 
            "raise events when watched directory changes"
        DeleteWatch: { [ path: Env::DirPath success: Event error: { Types::ErrorInfo -> Event } engine: Jobs::Engine ] -> Jobs::Job } "delete directory watch"     
    ]
```

# Examples #

```
use Bits: Base/Bits
use Env: API/Environment
type Event:
    [
        reader: Reader "reader was created" |
        writer: Writer "writer was created" |
        read: [character] "characters read" |
        written: "data were written" |
        error: [character] "error message" |
        deleted: "file was deleted"
    ] "custom event type"
use IO: API/FileIO(Event: Event)
use T: API/FileIOtypes
 .  .  .
    `Various jobs:`
    job = @IO.CreateWriter
            ([ 
                path: FileSystem.filePath(param.engine.fileSystem.workingDir, "abc.tmp");
                success: { w: IO::Writer -> [ writer: w; ] }; `signal writer was created`
                error: { e: T::ErrorInfo -> [ error: e.message; ] }; `pass error message` 
                engine: param.engine;
            ])!
 .  .  .
    job = event.writer
        ([ 
            write: 
                [ 
                    position: [ bottom: 0; ]; `at bottom`
                    data: (~bits "ABC") with @Bits; `binary representation of A, B and C`
                    written: [written:]; `signal that data were written`
                ]; 
        ])!
 `job that writes binary representation of characters A, B, C to file 'abc.tmp' causing event 'written'`
 .  .  .
    job = @IO.CreateReader
            ([ 
                path: param.engine.fileSystem.filePath(param.engine.fileSystem.workingDir, "abc.tmp");
                success: { r: IO::Reader -> [ reader: r; ] }; `signal reader was created`
                error: { e: T::ErrorInfo -> [ error: e.message; ] }; `pass error message` 
                engine: param.engine;
            ])!
 .  .  .
    job = event.reader
        ([ 
            read: 
                [ 
                    position: [ bottom: 6; ]; 
                    bytes: 6; 
                    result: { data: Bits::BitString -> [ read: array ((~char Substr(data, [index: 16 * i; length: 16;])) with @Bits) by i to 3 end; ] }; `convert data to characters and pass via event`
                ]; 
        ])! 
 `job that reads the last 6 bytes from file 'abc.tmp' causing event 'read'`
 .  .  .
    job = 
        ( mngr.deleteFile([ path: param.engine.fileSystem.filePath(param.engine.fileSystem.workingDir, "abc.tmp"); success: [deleted:]; error: { e: T::ErrorInfo -> [error: e.message;] }; ])
        where mngr = @IO.FileMngmt(param.engine)!
        )! `job that deletes file 'abc.tmp'`
```

See also [SortFile](SortFile.md).