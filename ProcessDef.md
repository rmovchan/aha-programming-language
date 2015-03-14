# Introduction #

This API module is needed to define a new process component used via module [Process](Process.md).

The classname of the component is assigned when it's actually added to the system and must be unique. The component's title can be used as the default classname.

Types `Event`, `Settings` and `Output` are opaque and therefore must be defined by the actual implementation of this module.

Type `Event` is used by the component internally. Note that the event types of the component and the client are totally independent.

Type `Settings` describes any data that can be passed to the component by a client application (argument `settings` when instantiating).

Type `Output` describes the data that is passed back from the component to the client via events. To actually pass the data, the component can use the function (created by the framework) received as the second argument (`output`).

Apart from the opaque types, the implementation must define the export with attributes `Title` and `Behavior`. The title is simply an arbitrary character string that describes the component. The behavior is a function that returns an object that handles events (of the user-defined type `Event`) via its `handle` action and returns an array of jobs as its state.

The behavior object receives four parameters: `settings`, `password`, `output` and `engine`. `settings` is the input value passed when the component was instantiated. `password` is the password supplied when creating. `output` is a function that for given output (of type `Output`) creates a job that passes the output to the client. Finally, `engine` is a job engine (declared in [Jobs](Jobs.md)) used to invoke basic jobs, such as `raise` to raise an event or `stop` to terminate all the active jobs.

If the state function or the `handle` action of the behavior object fails, the process terminates.
'
# Specification #

```
doc 
    Title: "ProcessDef"
    Purpose: "Definition of a process component"
    Package: "Application Program Interface"
    Author: "Roman Movchan, Melbourne, Australia"
    Created: "2013-09-06"
end

type Event: opaque "custom event type"
type Settings: opaque "component's settings (set at creation of an instance)"
type Output: opaque "component's output (created using an output job)"
use Jobs: API/Jobs(Event: Event)
export
    [
        Title: [character] "component's title"  
        Behavior: { [ settings: Settings password: [character] output: { Output -> Jobs::Job } engine: Jobs::Engine ] -> Jobs::Behavior } "component's behavior"
    ]
```

# Example #

This example implements a Timer component that passes the current time to the client application repeatedly at equal intervals. The interval, as well as the event to be raised, is specified when creating an instance of the timer. See [Process](Process.md) for an example of using this component.

```
implement API/ProcessDef
use Time: Base/Time
type Settings: Time::Interval "interval between events raised"
type Output: Time::Timestamp "pass current time to output job"
type Event: Time::Timestamp "pass current time"
export 
[
    Title: "Timer";
    Behavior:
        { param: [ settings: Settings password: [character] output: { Output -> Jobs::Job } engine: Jobs::Engine ] -> 
            (
                obj { state: [Jobs::Job] -> state } [wait]; `wait`
                handle(event: Event): [param.output(event), wait]; `when time is known, pass it and wait`
                end
            where
                wait = 
                    ( param.engine.delay(param.settings, askTime) `wait for the specified interval to expire`
                    where
                        askTime = param.engine.enquireTime({ time: Time::Timestamp -> time })! `request time from system and send event with result`
                    )!
            )
        };
]
```