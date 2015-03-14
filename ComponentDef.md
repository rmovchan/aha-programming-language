# Introduction #

To define a component, the developer must implement module `ComponentDef`. This module exports a composite with attributes `Title`, `Signature` and `Behavior` and declares some opaque types. It also declares custom types `Input`,  `Settings` and `Output`, which every user of the component must match with the corresponding type parameters of module [Component](Component.md), plus custom type `Event`, which is used internally.

`Title` and `Signature` are character strings used by the runtime framework for identification.

The behavior object is returned from a function ("constructor") which receives three parameters: `settings`,`output` and `engine`. `settings` is the value passed to the component when it is created. `output` is a function that for given output (of type `Output`) creates a job that passes the output to the client. Finally, `engine` is a job engine (declared in [Jobs](Jobs.md)) used to create basic jobs.

The behavior object determines how the component reacts both on internal events and external input.

Notice that a component definition is very similar to an application, see [ConsoleApp](ConsoleApp.md). The main difference is that the types of input and output can be customized.

# Specification #

```
doc 
    Title: "ComponentDef"
    Purpose: "Definition of a component"
    Comment: "This specification cannot be changed by the user"
    Package: "User defined"
    Author: "User"
    Created: "2013-09-10"
end

type Event: opaque "custom event type"
type Settings: opaque "component's settings"
type Input: opaque "component's input (commands)"
type Output: opaque "component's output (created using an output job)"
use Jobs: API/Jobs(Event: Event)
export
    [
        Title: [character] "component's title"
        Signature: [character]  "vendor's signature"
        Behavior: 
            { 
                [ settings: Settings output: { Output -> Jobs::Job } engine: Jobs::Engine ] -> 
                    obj [Jobs::Job]
                    handleEvent(Event)
                    handleInput(Input)
                    end
            } "component's behavior"
    ]
```

# Examples #

The first example implements a Timer component that passes the current time to the client application repeatedly at equal intervals. The interval and the event to be raised are specified when creating an instance of the timer. See [Component](Component.md) for an example of using this component.

```
implement Timer/ComponentDef
use Time: Base/Time
type Settings: Time::Interval "interval between events raised"
type Input: void "no input"
type Output: Time::Timestamp "pass current time as output"
type Event: Time::Timestamp "receive current time"
export
    [
        Title: "Timer";
        Signature: "Demo";
        Behavior:
            { param: [ settings: Settings output: { Output -> Job } engine: Engine ] -> 
                (
                    obj { state: [Jobs::Job] -> state } [wait]; `wait`
                    handleEvent(event: Event): [param.output(event), wait]; `when time was received, pass it and wait`
                    handleInput: invalid [Jobs::Job]; `never occurs`
                    end
                where
                    wait = 
                        param.engine.delay(param.settings, askTime) `wait for the specified interval to expire`
                        where
                            askTime = param.engine.enquireTime({ time: Time::Timestamp -> time })! `request time from system and send event with result`
                        !
                )
            };
    ]
```

The second example is a modified version of the Timer component defined above. The difference is that the new version can receive the start/stop commands and is idle until the first start command is received, plus the interval can be changed dynamically.

```
implement Timer/ComponentDef
use Time: Base/Time
type Settings: Time::Interval "interval between events raised"
type Input:
    [
        start: "start timer" |
        stop: "stop timer" |
        interval: Time::Interval "set new interval" 
    ] "external commands"
type Output: Time::Timestamp "pass current time to client"
type Event: Time::Timestamp "current time received" 
use Jobs: API/Jobs(Event: Event)
export 
    [
        Title: "Timer";
        Signature: "Demo";
        Behavior:
            { param: [ settings: Settings output: { Output -> Jobs::Job } engine: Jobs::Engine ] -> 
                obj { state: [ jobs: [Jobs::Job] interval: Time::Interval active: [ yes: | no: ] ] -> state.jobs }
                    [ jobs: [param.engine.enable]; interval: param.settings; active: [no:]; ]; `idle`
                handleEvent(event: Event): `handle events`
                    [ jobs: [param.output(event), wait(prev.interval)]; interval: prev.interval; active: prev.active; ];
                        `when time is received, pass it to client and wait another interval`
                handleInput(cmd: Input): `handle commands`
                    [ jobs: jobs; interval: interval; active: active; ] where
                        all
                            unless `define jobs`
                                any
                                    jobs = [wait(prev.interval)] when all cmd.start? prev.active.no? end! `start timer when inactive`
                                    jobs = [param.engine.break] when all cmd.stop? prev.active.yes? end! `stop timer when active`
                                end
                            then
                                jobs = Jobs::Job[]! `otherwise, no new jobs`
                            unless `define interval`
                                interval = cmd.interval! `if new interval is being set, use it`
                            then
                                interval = prev.interval! `otherwise, keep old one`
                            any `define active`
                                active = [yes:] when cmd.start?!
                                active = [no:] when cmd.stop?!
                                active = prev.active when cmd.interval?!
                            end
                        end; `handleInput`
                end `obj` 
                where
                    wait = 
                        { interval: Time::Interval ->
                            param.engine.delay(interval, askTime) where `wait for the current interval to expire`
                                askTime = param.engine.enquireTime({ time: Time::Timestamp -> time })! `request time from system and send event with result`
                        }!
            }; `Behavior`
    ]

```