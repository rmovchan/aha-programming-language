# Introduction #

This module introduces the data types and utilities needed to operate with dates, time and date/time intervals.


# Spec #

```
doc 
    Title:   "Time"
    Package: "Aha! Base Library"
    Purpose: "Date and time manipulation"
    Author:  "Roman Movchan, Melbourne, Australia"
    Created: "2011-10-11"
end

type Timestamp: opaque "Date and time"
type Interval: opaque "Time interval"
type DateStruc:
    [
        year: integer "year(s)" 
        month: integer "month(s)"
        day: integer "day(s)"
    ] "date as composite"
type TimeStruc:
    [
        hour: integer "hour(s)"
        min: integer "minute(s)"
        sec: integer "second(s)"
    ] "time as composite"
type TimestampStruc: [ date: DateStruc "date part" time: TimeStruc "time part" ] "timestamp as composite"
type DayOfWeek:
    [
        monday: "Monday?" |
        tuesday: "Tuesday?" |
        wednesday: "Wednesday?" |
        thursday: "Thursday?" |
        friday: "Friday?" |
        saturday: "Saturday?" |
        sunday: "Sunday?" 
    ] "a day of the week"

export 
    [
        DayOfWeek: { Timestamp -> DayOfWeek } "day of week for given Timestamp"
        Year: Interval "1-year interval"
        Month: Interval "1-month interval"
        Day: Interval "1-day interval"
        Hour: Interval "1-hour interval"
        Minute: Interval "1-minute interval"
        Second: Interval "1-second interval"
        Millisecond: Interval "1-millisecond interval"
        Tick: Interval "minimum length interval (~1/10000 msec)"
        Zero: Interval "zero length interval"
        TimestampCompare: { Timestamp, Timestamp -> integer } "negative - before, positive - after, zero - same time"
        IntervalCompare: { Interval, Interval -> integer } "negative - shorter, positive - longer, zero - same length"

        (Timestamp - Timestamp): { Timestamp, Timestamp -> Interval } "difference between two timestamps"
        (Interval + Interval): { Interval, Interval -> Interval } "sum of intervals"
        (Interval - Interval): { Interval, Interval -> Interval } "difference between intervals"
        (Timestamp + Interval): { Timestamp, Interval -> Timestamp } "timestamp plus interval"
        (Timestamp - Interval): { Timestamp, Interval -> Timestamp } "timestamp minus interval"
        (integer * Interval): { integer, Interval -> Interval } "integer times interval"
        (Interval / integer): { Interval, integer -> Interval } "interval divided by an integer"
        (~date DateStruc): { DateStruc -> Timestamp } "convert DateStruc to Timestamp (date only)"
        (~time TimeStruc): { TimeStruc -> Interval } "convert TimeStruc to Interval (from midnight)"
        (~date Timestamp): { Timestamp -> Timestamp } "date part of a Timestamp"
        (~time Timestamp): { Timestamp -> Interval } "time part of a Timestamp as Interval (from midnight)"
        (~struc Timestamp): { Timestamp -> TimestampStruc } "convert Timestamp to TimestampStruc"
        (~struc Interval): { Interval -> TimestampStruc } "convert Interval to TimestampStruc"
    ]
```

## Examples ##
```
use Time: Base/Time

DayOfWeek(~date [ year: 2012; month: 6; day: 5; ]).tuesday with @Time ? ` succeeds, because 5th of June, 2012 was Tuesday `

(~date [ year: 2012; month: 6; day: 5; ]) + (~time [ hour: 10; min: 35; sec: 0; ]) with @Time
    ` returns Timestamp for 5th of June, 2012, 10:35am `

weekAgo = (today - (7 * Day)) with @Time! ` note: the actual system date can be obtained through the API `

lastMonday = first d in seq today - Day, prev - Day to 7 that DayOfWeek(d).monday? end with @Time ! 
    ` returns Timestamp for last Monday before today `

(~struc (d2 - d1)).date.day with @Time ` number of days between d1 and d2 `

sort d1, d2 in dates that (TimestampCompare(d1, d2) >= 0) with @Time? end 
    ` returns dates as sorted sequence starting from most recent `
```