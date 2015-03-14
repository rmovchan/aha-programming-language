# Introduction #

To create a mute application, one needs to implement module `API/MuteApp`.

This module specifies the data type `Event` as **opaque** (i.e. it must be defined by the implementation) and declares attributes `Title`, `Signature` and `Behavior` that also must be defined by the code.

`Title` is a string that describes the application; it is used when the application is added to the system.

`Signature` is a string that identifies the vendor of the application.

`Behavior` is a function that returns an object that determines how events are handled. When either the behavior object's state function or `handle` action fails, the application terminates. The `settings` parameter of the behavior is the string passed from the environment, for example, via the command line; the `engine` is a job engine (of type `Engine` declared in [Jobs](Jobs.md)) also created by the framework automatically. The `password` is the authentication string requested from the user when starting the application, which can be used to protect it from unauthorized access.

# Specification #
```
doc 
    Title: "MuteApp"
    Purpose: "A mute application"
    Package: "Application Program Interface"
    Author: "Roman Movchan, Melbourne, Australia"
    Created: "2014-05-04"
end

type Event: opaque "must be defined by the implementation"
use Jobs: API/Jobs(Event: Event)
export
    [
        Title: [character]  "application's title"
        Signature: [character]  "vendor's signature"
        Behavior: { [ settings: [character] password: [character] engine: Jobs::Engine ] -> Jobs::Behavior } "application's behavior"
    ]
```