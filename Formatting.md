# Introduction #

This module provides formatting utilities for various data types in Base Library. To get Format/Deformat for different locales, API needs to be used.


# Spec #
```
doc 
    Title:   "Formatting"
    Purpose: "Date, time, currency and number formatting"
    Package: "Aha! Base Library"
    Author: "Roman Movchan"
    Created: "2012-04-12"
end

use Math: Base/Math
use Time: Base/Time
use Money: Base/Money

type Format:
    [
        int: { integer -> [character] } "format integers" |
        float:
            [
                general: { Math::Float -> [character] } "general format" |
                exponent: { Math::Float -> [character] } "exponential format" |
                fixed: { Math::Float, integer -> [character] } "fixed format"
            ] "format Float" |
        date:
            [
                short: { Time::Timestamp -> [character] } "short date" |
                long: { Time::Timestamp -> [character] } "long date" 
            ] "format dates" |
        dayOfWeek:
            [
                short: { Time::Timestamp -> [character] } "short D.O.W." |
                long: { Time::Timestamp -> [character] } "long D.O.W."
            ] "format D.O.W." |
        monthName:
            [
                short: { Time::Timestamp -> [character] } "short month name" |
                long: { Time::Timestamp -> [character] } "long month name"
            ] "format month names" |
        time:
            [
                short: { Time::Interval -> [character] } "short time" |
                long: { Time::Interval -> [character] } "long time"
            ] "format time" |
        money: 
            [
                short: { Money::Money -> [character] } "short currency" |
                long: { Money::Money -> [character] } "long currency"
            ] "format currency" 
    ] "a set of formatting routines"
type Deformat:
    [
        int: { [character] -> integer } "deformat integer" |
        float: { [character] -> Math::Float } "deformat Float" |
        date: { [character] -> Time::Timestamp } "deformat date" |
        time: { [character] -> Time::Interval } "deformat time" |
        money: { [character] -> Money::Money } "deformat currency" 
    ] "a set of deformatting routines"
    
export void
```