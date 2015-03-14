# Introduction #

To create a console application, one needs to implement module `API/Application`.

This module specifies the data type `Event` as **opaque** (i.e. it must be defined by the implementation) and declares variables `Title`, `Signature`, `Permit`, `Behavior` and `Receive` that also must be defined by the code.

`Title` is a string that describes the application; it is used when the application is added to the system.

`Signature` is a string that identifies the vendor of the application.

`Behavior` is a function that returns an object that determines how events are handled. When either the behavior object's state function or `handle` action fails, the application terminates. The `settings` parameter of the behavior is the string passed from the environment, for example, via the command line; the `output` parameter is a function (created by framework) that sends text output to the system console; the `engine` is a job engine (of type `Engine` declared in [Jobs](Jobs.md)) also created by the framework automatically. The `password` is the authentication string requested from the user when starting the application, which can be used to protect it from unauthorized access.

`Receive` is a function that converts user input into an event.

A simple example of a console application is [HelloWorld](HelloWorld.md). A more advanced example is [SortFile](SortFile.md).


# Specification #
```
doc 
    Title: "Application"
    Purpose: "A console application"
    Package: "Application Program Interface"
    Author: "Roman Movchan, Melbourne, Australia"
    Created: "2013-27-08"
end

type Event: opaque "must be defined by the implementation"
use Jobs: API/Jobs(Event: Event)
the Title: [character]  "application title"
the Signature: [character]  "vendor's signature"
the Permit: { [character] } "verify supplied password"
the Behavior: { [ input: [character] output: { [character] -> @Jobs!Job } engine: @Jobs!Engine ] -> @Jobs!Behavior } "application behavior"
the Receive: { [character] -> Event } "convert user input to events"
```