# Introduction #
This program demonstrates using the [FileIO](FileIO.md) API module with a console application. Routine `compute` is part of type `Engine` defined in [Jobs](Jobs.md) and is used here to perform line sorting in the background.

The goal is to have a console application that requests a file name from the end user, reads the file (it is assumed to be a text file in any encoding), sorts the lines lexicographically and then writes them back. The original file is backed up by adding '.bak' to its name; if a file with such name already exists, another '.bak' is added, and so on. Files are assumed to be located in the application's working directory.

Once a file name is entered, a new name is requested. Therefore, multiple files may be read/written at the same time and the actual sequence of events cannot be predicted. Line sorting is also performed in the background. When an empty file name is entered, the application terminates after processing all the files whose names were entered before.

If any I/O errors are encountered during processing a file, they are reported to the user and the processing of that file stops.

# Code #
'
```
implement SortFile/ConsoleApp
type String: [character] "alias for character array"
use Types: API/FileIOtypes
use Env: API/Environment
type Content:
    [
        lines: [String] "array of lines"
        encoding: Types::Encoding "text encoding"
        chars: integer "character count"
    ] "text file content"
type Event:
    [
        path: Env::FilePath "file path"
        status:
            [
                new: "got new file name" |
                read: Content "content of file read" |
                sorted: Content "lines sorted" |
                backed: Content "file has been backed up" |
                clash: [name: String content: Content] "backup file name already exists" |
                error: String "error occurred" |
                done: "finished processing"
            ] "file processing status" 
    ] "application-level events"
use Jobs: API/Jobs(Event: Event)
use IO: API/FileIO(Event: Event)
export 
` Application definition: `
[   Title: "File Sort";
    Signature: "Demo";
    Behavior:
        { param: [ settings: String output: { String -> Jobs::Job } engine: Jobs::Engine ] ->
            obj { state: [Jobs::Job] -> state } `state is array of jobs`
                [display("Enter file names, empty string to finish"), engine.enable] `display prompt and enable input`
            handleInput(input: String): `generate jobs in response to input`
                jobs where
                    any `only one branch will succeed`
                        let jobs = `non-empty name was entered - build path and pass for processing`
                            ( [display(join "Start processing file ", input end), doit] where
                                doit = engine.raise([ path: fileSystem.filePath(fileSystem.workingDir, input); status: [new:]; ])!
                            )! when input# /= 0?
                        let jobs = `an empty string was entered - disable input (this will terminate application after processing all files)`
                            [display("Goodbye!"), engine.disable]! when input# = 0?
                    end `any, handleInput`
            handleEvent(event: Event): `generate jobs in response to events`
                jobs where
                    any `only one branch will succeed`
                        let jobs = `reading`
                            ( [display(join "Reading file ", fileSystem.fileName(event.path) end), doit] where
                                doit = `read file content into array of lines`
                                    ( @IO.ReadText([
                                        path: event.path;
                                        encoding: [auto:];
                                        engine: engine;
                                        success:
                                            { result: [content: character* size: integer encoding: Types::Encoding] ->
                                                alter event;
                                                    status: `signal that file content has been read`
                                                        [ read:
                                                            [
                                                                lines: lines(result.content, result.size);
                                                                encoding: result.encoding;
                                                                chars: result.size;
                                                            ];
                                                        ];
                                                end
                                            };
                                        error: { e: Types::ErrorInfo -> alter event; status: [error: e.message;]; end }; ])!
                                    where
                                        lines = `convert sequence of chars to array of lines`
                                            { chars: character*, size: integer -> list fileSystem.splitLines(chars) to size }!
                                    )!
                            )! when event.status.new? `if a new name was entered`
                        let jobs = `sorting`
                            ( [display(join "Sorting file ", fileSystem.fileName(event.path) end), doit] where
                                doit = `sort lines`
                                    param.engine.compute `compute event in the background`
                                        ([
                                            event:
                                                { ->
                                                    alter event;
                                                        status:
                                                            [ sorted:
                                                                alter event.status.read;
                                                                    lines:
                                                                        list `convert sorted sequence of lines to array`
                                                                            sort s1, s2 in event.status.read.lines that (s1 <= s2) with locale? 
                                                                        to event.status.read.lines#;
                                                                end;
                                                            ];
                                                    end
                                                };
                                            fail: alter event; status: [ error: "Out of memory"; ]; end;
                                        ])!
                            )! when event.status.read? `when file has been read`
                        let jobs = `backing up`
                            [display(join "Backing up ", fileSystem.fileName(event.path) end), doit]! where
                                doit = backup([path: event.path; newname: join fileSystem.fileName(event.path), ".bak" end; content: event.status.sorted;])!
                        let jobs = `backup file name already exists`
                            [display(join "Warning: ", event.status.clash.name, " already exists" end), doit]! where
                                doit = backup([path: event.path; newname: join event.status.clash.name, ".bak" end; content: event.status.clash.content;])!
                        let jobs = `writing`
                            [display(join "Writing file ", fileSystem.fileName(event.path) end), doit]! where
                                doit = `write sorted lines into file`
                                    @IO.WriteText([
                                        path: event.path;
                                        encoding: event.status.backed.encoding; `use original encoding`
                                        content: fileSystem.joinLines(enum line in event.status.backed.lines that void? ); `get stream of chars`
                                        size: event.status.backed.chars; `original size`
                                        engine: engine;
                                        success: alter event; status: [done:]; end; `signal completion`
                                        error: { e: Types::ErrorInfo -> alter event; status: [error: e.message;]; end }; ])!
                        let jobs = [display(join "Error processing ", fileSystem.fileName(event.path), ": ", event.status.error end)]! `display errors`
                        let jobs = [display(join "File ", fileSystem.fileName(event.path), " has been processed" end)]! when event.status.done?
                    end `any, handleEvent`
            end `obj`
            where
                all
                    display = param.output! `shorthands`
                    engine = param.engine!
                    fileSystem = param.engine.fileSystem!
                    locale = param.engine.locale!
                    let backup = `rename original file for backup`
                        { param: [ path: Env::FilePath newname: String content: Content ] ->
                            fileMgr.renameFile
                                ([
                                    path: param.path;
                                    to: param.newname; 
                                    success: [ path: param.path; status: [backed: param.content;]; ]; `retain sorted content`
                                    error: 
                                        { e: Types::ErrorInfo -> 
                                            event where
                                                unless
                                                    let event = `raise 'clash' event, attempting to add another '.bak' to name` 
                                                        [ 
                                                            path: param.path;
                                                            status: 
                                                                [
                                                                    clash: 
                                                                        [
                                                                            name: param.newname; 
                                                                            content: param.content;
                                                                        ];
                                                                ]; 
                                                        ]! 
                                                    when e.kind.nameClash? `.bak file already exists?`
                                                then
                                                    event = [ path: param.path; status: [error: e.message;]; ]! `other errors processed normally`
                                        }; 
                                ])
                        }! 
                    where fileMgr = @IO.FileMngmt(param.engine)! `file management jobs`
                end `all`
        }; `Behavior`
]
```

There are a few things to note about the code:
  * it is assumed that the entire file content fits into the available computer memory
  * all the data flow and task synchronization is implemented through events; our behavior object has no 'memory' of the past events
  * it will work on any O/S, any locale and with any text encoding
  * all the threads, file handles and other low-level details are hidden from the developer
  * `compute` is used to avoid lengthy computations inside the `handleEvent` action, to improve the responsiveness
  * by changing the top line and adding another three, we could turn this into a definition of an appliance that takes string input (not used here), receives string commands and produces string output where the `display` function is used
  * due to the immutability, very little data copying is needed during the processing
  * only the API modules are used