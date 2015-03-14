# Introduction #

To define an appliance component, the developer must implement module `ApplianceDef`. Like [ProcessDef](ProcessDef.md), this module declares variables `Title`, `Permit` and `Behavior` and some opaque types. In addition to types `Settings`, `Output` and `Event`, this module defines custom type `Input`, which every user of the component must match with the `Input` type parameter of module [Appliance](Appliance.md), similarly to `Settings` and `Output`. Another difference from [ProcessDef](ProcessDef.md) is that an appliance must define an extra function, `Receive`, which converts external input (of type `Input`) to events (of type `Event`).

Like with a process, the behavior object receives four parameters: `settings`, `password`, `output` and `engine`. `settings` is the value passed when the component was created. `output` is a function that for given output (of type `Output`) creates a job that passes the output to the client. Finally, `engine` is a job engine (declared in [Jobs](Jobs.md)) used to create basic jobs.

Note that a component defined using `ApplianceDef` can still be used as a process (i.e. via module [Process](Process.md)).

# Specification #

```
doc 
    Title: "ApplianceDef"
    Purpose: "Definition of an appliance"
    Package: "Application Program Interface"
    Author: "Roman Movchan, Melbourne, Australia"
    Created: "2013-09-10"
end

type Event: opaque "custom event type"
type Settings: opaque "appliance's settings"
type Input: opaque "appliance's input (commands)"
type Output: opaque "appliance's output (created using an output job)"
use Jobs: API/Jobs(Event: Event)
export
    [
        Title: [character] "appliance's title"
        Behavior: { [ settings: Settings password: [character] output: { Output -> Jobs::Job } engine: Jobs::Engine ] -> Jobs::Behavior } "appliance's behavior"
        Receive: { Input -> Event } "convert input to event"
    ]
```

# Example #

This example is a modified version of the Timer component defined in [ProcessDef](ProcessDef.md). The difference is that the new version can receive the start/stop commands and is idle until the first start command is received, plus the interval can be changed dynamically.

```
implement API/ApplianceDef
use Time: Base/Time
type Settings: Time::Interval "interval between events raised"
type Input:
    [
        start: "start timer" |
        stop: "stop timer" |
        interval: Time::Interval "set new interval" 
    ] "external commands"
type Output: Time::Timestamp "pass current time to client"
type Event: 
    [
        cmd: Input "command received" |
        time: Time::Timestamp "current time received" 
    ] "internal events"
use Jobs: API/Jobs(Event: Event)
export 
    [
        Title: "Timer";
        Behavior:
            { param: [ settings: Settings password: [character] output: { Output -> Jobs::Job } engine: Jobs::Engine ] -> 
                obj { state: [ jobs: [Jobs::Job] interval: Time::Interval ] -> state.jobs }
                    [ jobs: [param.engine.enable]; interval: param.settings; ]; `idle`
                handle(event: Event): `handle internal events`
                    ( [ jobs: jobs; interval: interval; ] where
                        all
                            any `define jobs`
                                jobs = ( [wait] when event.cmd.start? )! `start with a new interval`
                                jobs = [param.output(event.time), wait]! `when time is known, pass it to client and wait till interval expires`
                                jobs = ( [param.engine.break] when event.cmd.stop? )! `stop the 'wait' job`
                                jobs = ( Job[] when event.cmd.interval? )! `no jobs if new interval is set`
                            end
                            unless `define interval`
                                interval = event.cmd.interval! `if new interval is being set, use it`
                            then
                                interval = prev.interval! `otherwise, keep old one`
                            end
                        where
                            wait = 
                                ( param.engine.delay(prev.interval, askTime) where `wait for the current interval to expire`
                                    askTime = param.engine.enquireTime({ time: Time::Timestamp -> [ time: time; ] })! `request time from system and send event with result`
                                )!
                        end
                    );
                end
            };
        Receive: { cmd: Input -> [ cmd: cmd; ] }; `pass command to component`
    ]

```