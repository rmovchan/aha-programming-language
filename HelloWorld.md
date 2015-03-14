# Introduction #

This example shows the implementation of the "Hello, World" example using the Aha! [API](API.md). Application displays "Hello, World!" and waits for any input from the user, then terminates.

Using the API for such a simple application looks like an overkill. Example [SortFile](SortFile.md) is more interesting because it packs a lot more functionality in a relatively brief program.


# Code #
```
implement HelloWorld/ConsoleApp
type Event: void "dummy event type"
type String: [character] "alias for character array type"
export 
[
    Title: "Hello, World!";
    Signature: "Demo";
    Behavior:
        { param: [ settings: String output: { String -> Jobs::Job } engine: Jobs::Engine ] -> 
            obj { state: [Jobs::Job] -> state } `state is list of jobs`
                [param.output("Hello, World!"), param.engine.enable]; `display text immediately and enable input`
            handleEvent: invalid [Jobs::Job]; `no events occur; void parameter omitted`
            handleInput(input: String): [param.engine.disable]; `quit after input`
            end
        };
]
```