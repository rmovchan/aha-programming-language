# Introduction #

Components defined using specification [ProcessDef](ProcessDef.md) and used via module [Process](Process.md) have one big limitation: once created, they cannot be controlled externally. To overcome this, we introduced the concept of _appliance_.

Using an appliance is a little more complex than creating a process. An appliance differs from a process in that it produces jobs in response to _input_ (commands). Commands, like events, can be of an arbitrary, custom type that is specified when defining the appliance using [ApplianceDef](ApplianceDef.md).

Function `Appliance` returns an opaque `Appliance` value encapsulating the description of the appliance.

Function `Create` returns a job that creates a new job engine and runs asynchronously the component's behavior object; additionally, it raises an event that receives the appliance's _control_ function.

The control function returns jobs from commands. Internally, if the component is written in Aha! (see [ApplianceDef](ApplianceDef.md)), such a job causes an event to be sent to the component's `handle` action (although the client needs to know nothing about the component's events).

# Specification #

```
doc 
    Title: "Appliance"
    Purpose: "Use a component that can receive commands"
    Package: "Application Program Interface"
    Author: "Roman Movchan, Melbourne, Australia"
    Created: "2013-09-10"
end

type Event: arbitrary "client's event type"
type Settings: arbitrary "component settings (must match definition)"
type Input: arbitrary "component input (must match definition)"
type Output: arbitrary "component output (must match definition)"
use Jobs: API/Jobs(Event: Event)
type Appliance: opaque "reference to the description of an appliance"
type Control: { Input -> Jobs::Job } "appliance control function"
type ApplianceParam:
    [ 
        classname: [character] 
        password: [character] 
        settings: Settings 
        control: { Control -> Event } 
        output: { Output -> Event } 
        engine: Jobs::Engine 
    ] "appliance's parameters"
export { ApplianceParam -> Jobs::Job } 
```
# Example #

In this example, we use the modified version of the Timer component (first defined in [ProcessDef](ProcessDef.md)) which is defined in [ApplianceDef](ApplianceDef.md).

See also [SortFile](SortFile.md) for a full example of an application.

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
use Timer: API/Appliance(Settings: Time::Interval Input: Command Output: Time::Timestamp Event: Event)
 .  .  .
 `Application definition`
 .  .  .
    job = 
        @Timer
            ([ 
                classname: "User.Timer"; 
                password: ""; 
                settings: Second; 
                control: { ctl: Timer::Control -> [ control: ctl; ] };
                output: { time: Time::Timestamp -> [ update: time; ] }; 
                engine: param.engine; 
            ])!
    `create timer with 1 sec interval that sends an 'update' event; we assume its classname is 'User.Timer'`
 .  .  .
    job = event.control([start:])!  `job that starts timer by sending command` 
 .  .  .
    job = event.control([interval: 3 * Second;])!  `job that sets interval to 3 sec` 
 .  .  .
    job = event.control([stop:])!  `job that stops timer` 
```