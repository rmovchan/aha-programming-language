# Introduction #

This module is used by an application/component to access the system information and perform some basic jobs. An application/component receives a value of type `Engine`, created by the framework automatically, via its `engine` parameter. There is therefore an individual job engine for every instance of a component or application.

Attributes `framework`, `locale`, `fileSystem` and `systemInfo` are of the corresponding types declared in module `Environment` and can be used to get various information about the system.

`raise` is a function that for a given event returns a job that raises this event to the engine's owner (application or component).

`compute` is a function that returns a job performing computation of an event in the background (using provided function).

`enquireTime` returns a job that raises an event that can be used to obtain the current system time.

`delay` for a given job returns another job that performs the first one after a delay, specified as a time interval. `schedule` performs a given job at a specific time.

`log` simply puts a given text string into the system log, which can be used for debugging.

`break` terminates all the currently running and cancels all the scheduled jobs.

`enable` enables the input for the engine's owner, `disable` disables it. When the input is disabled (as it is initially when the application/component starts) and all the events have been handled, the application/component has no work to do and therefore is automatically terminated.

# Specification #

```
doc 
    Title: "Jobs"
    Purpose: "Basic jobs and access to the system"
    Package: "Application Program Interface"
    Author: "Roman Movchan, Melbourne, Australia"
    Created: "2013-09-05"
end

type Event: arbitrary "custom event type"
type Job: opaque "a job for runtime environment"
use Time: Base/Time
use Env: API/Environment
type Engine:
    [
        framework: Env::Framework "framework info"
        locale: Env::Locale "locale info"
        fileSystem: Env::FileSystem "file system"
        systemInfo: Env::SystemInfo "system info"
        raise: { Event -> Job } "raise event to be immediately handled"
        compute: { [ event: { -> Event } fail: Event ] -> Job } "job that computes an event in the background and then raises it"
        enquireTime: { { Time::Timestamp -> Event } -> Job } "job that raises event that receives current system time"
        delay: { Time::Interval, Job -> Job } "do job after delay"
        schedule: { Time::Timestamp, Job -> Job } "do job at a specific time"
        log: { [character] -> Job } "write a message to the system log"
        break: Job "immediately terminate all current jobs"
        enable: Job "enable external input"
        disable: Job "disable external input"
    ] "interface to the job engine"

export void
```

# Examples #

```
    param.engine.delay((Second / 2) with @Time, param.engine.raise([start:])) `raise event 'start' after half a second delay`

    param.engine.enquireTime({ time: Timestamp -> [ update: time; ] })! `request current time`
```
See [ProcessDef](ProcessDef.md) and [SortFile](SortFile.md) for more examples.