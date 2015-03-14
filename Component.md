# Introduction #

To utilize a component, one needs to use module `Component`. This module exports a function that returns a job that creates an instance of a component with given parameters.

When the instance is created and enables input, the client (application/component) receives the component's control function, created automatically, via an event using the supplied `control` parameter. The control function returns jobs from input commands. Internally (see [ComponentDef](ComponentDef.md)), such a job causes the input to be sent to the component's `handleInput` action.
'
# Specification #

```
doc 
    Title: "Component"
    Purpose: "Use a component"
    Package: "Application Program Interface"
    Author: "Roman Movchan, Melbourne, Australia"
    Created: "2013-09-10"
end

type Event: arbitrary "client's event type"
type Settings: arbitrary "component settings (must match definition)"
type Input: arbitrary "component input (must match definition)"
type Output: arbitrary "component output (must match definition)"
use Jobs: API/Jobs(Event: Event)
type Control: { Input -> Jobs::Job } "a component's control function"
type ComponentParam:
    [ 
        classname: [character] "component's classname"
        password: [character] "component's password"
        settings: Settings "component's initialization settings"
        control: { Control -> Event } "event that receives component's control function"
        output: { Output -> Event } "convert component's output to events"
        engine: Jobs::Engine "client's job engine"
    ] "appliance's parameters"
export { ComponentParam -> Jobs::Job } 
```
# Examples #

Both examples demonstrate using the Timer component defined in [ComponentDef](ComponentDef.md), first without and then with input.

```
use Time: Base/Time
type Event:
    [
        update: Time::Timestamp "update current time" |
     .  .  .
use Timer: API/Component(Settings: Time::Interval, Input: void, Output: Time::Timestamp, Event: Event)
     .  .  .
runTimer = 
    @Timer
        ([ 
            classname: "User.Timer"; 
            password: ""; 
            engine: param.engine; 
            settings: @Time.Second; 
            control: { cmd: Timer::Control -> invalid Event }; `never occurs`
            output: { time: Time::Timestamp -> [ update: time; ] }; 
        ])! 
`job that creates a timer with 1 sec interval that sends an 'update' event when the component produces output (i.e. every second); we assume its classname is 'User.Timer'`
```

```
implement API/Application
use Time: Base/Time
type Command:
    [
        start: "start timer" |
        interval: Time::Interval "set new interval" |
        stop: "stop timer"
    ] "timer control commands (as defined by the component)"
type Event:
    [
        control: { Command -> Jobs::Job } "receive timer control" |
        update: Time::Timestamp "update current time" |
     .  .  .
    ] "application events"
use Timer: API/Component(Settings: Time::Interval, Input: Command, Output: Time::Timestamp, Event: Event)
 .  .  .
 `Application definition`
 .  .  .
    job = 
        @Timer
            ([ 
                classname: "User.Timer"; 
                password: ""; 
                settings: @Time.Second; 
                control: { ctl: Timer::Control -> [ control: ctl; ] };
                output: { time: Time::Timestamp -> [ update: time; ] }; 
                engine: param.engine; 
            ])!
    `create timer with 1 sec interval that sends an 'update' event; we assume its classname is 'User.Timer'`
 .  .  .
    job = event.control([start:])!  `job that starts timer by sending command` 
 .  .  .
    job = event.control([interval: (3 * Second) with @Time;])!  `job that sets interval to 3 sec` 
 .  .  .
    job = event.control([stop:])!  `job that stops timer` 
```

See also [SortFile](SortFile.md) for a full example of an application.