# Introduction #

To create a console application, one needs to implement module `API/ConsoleApp`.

This module specifies the data type `Event` as **opaque** (i.e. it must be defined by the implementation) and declares attributes `Title`, `Signature` and `Behavior` that also must be defined by the code.

`Title` is a string that describes the application; it is used when the application is added to the system.

`Signature` is a string that identifies the vendor of the application.

`Behavior` is a function that returns an object that determines how events and input are handled. When either the behavior object's state function or a `handle` action fails, the application terminates. The `settings` parameter of the behavior is the string passed from the environment, for example, via the command line; the `output` parameter is a function (created by the framework) that sends text output to the system console; the `engine` is a job engine (of type `Engine` declared in [Jobs](Jobs.md)) also created by the framework automatically.

A simple example of a console application is [HelloWorld](HelloWorld.md). A more advanced example is [SortFile](SortFile.md).



# Specification #
'
```
doc 
    Title: "ConsoleApp"
    Purpose: "A console application"
    Comment: "This specification cannot be changed by the user"
    Package: "User defined"
    Author: "User"
    Created: "2013-27-08"
end

type Event: opaque "must be defined by the implementation"
use Jobs: API/Jobs(Event: Event)
export
    [
        Title: [character]  "application's title"
        Signature: [character]  "vendor's signature"
        Behavior: 
        { 
            [ settings: [character] output: { [character] -> Jobs::Job } engine: Jobs::Engine ] -> 
                obj [Jobs::Job]
                handleInput([character])
                handleEvent(Event)
                end
        } "application's behavior"
    ]
```