# Introduction #

This API module is needed to use a (previously defined) component.

Type parameters `Settings` and `Output` must match those declared when the component was defined. However, since the component has already been compiled, the type matching is done at runtime - when using the `Create` function. If the component with the specified classname doesn't have the requested input and output types or doesn't exist, the process will not start. Module [Environment](Environment.md) contains the list of all the supported classnames. It is also needed to supply a password and provide the client's job engine so that events can be passed to the client when output is produced. Note that a new copy of the job engine is created by the system for each instance of a component. Additionally, one needs to specify the settings and a function that converts the output into an event; whenever the component produces output, this function is applied and the result event passed to the client's `handle` action.

Type `Event` is the type of event for the client.

The result of `Create` is a job that creates the process.

After a component has been created this way, it starts running immediately after submitting the job, and there's no way to affect its behavior. To use components that can be controlled externally, module [Appliance](Appliance.md) can be utilized.
'
# Specification #

```
doc 
    Title: "Process"
    Purpose: "Use a component that runs a process"
    Package: "Application Program Interface"
    Author: "Roman Movchan, Melbourne, Australia"
    Created: "2013-09-06"
end

type Settings: arbitrary "component settings"
type Output: arbitrary "component output"
type Event: arbitrary "client's event type"
use Jobs: API/Jobs(Event: Event)
type ProcessParam:
    [ 
        classname: [character] 
        password: [character] 
        engine: Jobs::Engine 
        settings: Settings 
        output: { Output -> Event } 
    ] 
export { ProcessParam -> Jobs::Job } 
```

# Example #

This example uses the Timer component defined in [ProcessDef](ProcessDef.md).

```
use Time: Base/Time
type Event:
    [
        update: Time::Timestamp "update current time" |
     .  .  .
use Timer: API/Process(Settings: Time::Interval, Output: Time::Timestamp, Event: Event)
     .  .  .
runTimer = 
    @Timer([ classname: "User.Timer"; password: ""; engine: param.engine; settings: @Time.Second; output: { time: Time::Timestamp -> [ update: time; ] } ])! 
`job that runs a timer with 1 sec interval and sends an 'update' event when the process produces output; we assume its classname is 'User.Timer'`
```