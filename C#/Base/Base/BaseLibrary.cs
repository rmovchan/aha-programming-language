//
// Package: Aha! Base Library 
// Author: Roman Movchan
// Created: 2014-04-01

using System;
using System.Collections.Generic;
using Aha.Core;

namespace Aha.Base
{
    namespace Collections
//doc
//    Title:   "Collections"
//    Purpose: "Generic collections: dynamic arrays, stacks, queues"
//    Package: "Aha! Base Library"
//    Author:  "Roman Movchan"
//    Created: "2010-10-14"
//end

//type Item: arbitrary "collection item"

//export Types:
//    type DynamicArray:
//        obj [Item]
//            add(Item) "add new item"
//            replace([ at: integer item: Item ]) "replace item at index"
//            exchange([ first: integer second: integer ]) "swap two items"
//            move([ from: integer to: integer ]) "move item to new position"
//            insert([ at: integer item: Item ]) "insert item at index"
//            delete(integer) "delete item at index"
//        end "a dynamic array"

//    type DynamicSequence:
//        obj Item
//            push(Item) "add an item"
//            pop "remove an item"
//        end "a dynamic sequence"

//export Constructors:
//    the DynamicArray: DynamicArray "zero-length dynamic array"
//    the Stack: DynamicSequence "empty stack"
//    the Queue: DynamicSequence "empty queue"
//    the Storage: RandomStorage "empty random storage"
//end
    {
        public interface icomp_ReplaceParam<Item>
        {
            Int64 attr_index();
            Item attr_item();
        }

        public interface icomp_InsertParam<Item>
        {
            Int64 attr_index();
            Item attr_item();
        }

        public interface iobj_DynamicArray<Item> : IahaObject<IahaArray<Item>>
        {
            void action_add(Item item);
            void action_replace(icomp_ReplaceParam<Item> param);
            void action_insert(icomp_InsertParam<Item> param);
            void action_delete(Int64 index);
        }

        public interface iobj_DynamicSequence<Item> : IahaObject<Item>
        {
            void action_push(Item item);
            void action_pop();
        }

        public interface imod_Collections<Item>
        {
            iobj_DynamicArray<Item> attr_DynamicArray();
            iobj_DynamicSequence<Item> attr_Stack();
            iobj_DynamicSequence<Item> attr_Queue();
        }
    }

    namespace Rational
//doc
//    Title:   "Rational"
//    Package: "Aha! Base Library"
//    Purpose: "Rational numbers"
//    Author:  "Roman Movchan, Melbourne, Australia"
//    Created: "2012-06-02"
//end

//export Types:
//    type Rational: opaque "a rational number"
//    type RatioStruc:
//        [
//            num: integer "numerator"
//            den: integer "denominator"
//        ] "rational as composite"
//end

//export Operators:
//    (integer / integer): { integer, integer -> Rational } "divide integers to get Rational"
//    (~struc Rational): { Rational -> RatioStruc } "convert Rational to RatioStruc"
//    (Rational + Rational): { Rational, Rational -> Rational } "sum of two rationals"
//    (Rational - Rational): { Rational, Rational -> Rational } "difference between two rationals"
//    (Rational * Rational): { Rational, Rational -> Rational } "product of two rationals"
//    (Rational / Rational): { Rational, Rational -> Rational } "quotient of two rationals"
//    (Rational < Rational): { Rational, Rational } "is first rational less than second?"
//    (Rational <= Rational): { Rational, Rational } "is first rational less than or equal to second?"
//    (Rational = Rational): { Rational, Rational } "is first rational equal to second?"
//    (Rational /= Rational): { Rational, Rational } "is first rational not equal to second?"
//    (Rational >= Rational): { Rational, Rational } "is first rational greater than or equal to second?"
//    (Rational > Rational): { Rational, Rational } "is first rational greater than second?"
//end
    {
        public struct opaque_Rational
        {
            public Int64 num;
            public Int64 den;
        }

        public interface icomp_RatioStruc
        {
            Int64 attr_num();
            Int64 attr_den();
        }

        public interface imod_Rational<opaque_Rational>
        {
            opaque_Rational op_integer_Slash_integer(Int64 num, Int64 den);
            icomp_RatioStruc op__struc(opaque_Rational x);
            opaque_Rational op_Rational_Plus_Rational(opaque_Rational a, opaque_Rational b);
            bool op_Rational_Less_Rational(opaque_Rational a, opaque_Rational b);
        }

        public class module_Rational : AhaModule
        {
            class comp_RatioStruc : icomp_RatioStruc
            {
                private Int64 num;
                private Int64 den;
                public comp_RatioStruc(Int64 n, Int64 d) { num = n; den = d; }
                public Int64 attr_num() { return num; }
                public Int64 attr_den() { return den; }
            }

            public opaque_Rational op_integer_Slash_integer(Int64 num, Int64 den) { return new opaque_Rational { num = num, den = den }; }
            public icomp_RatioStruc op__struc(opaque_Rational x) { return new comp_RatioStruc(x.num, x.den); }
            public opaque_Rational op_Rational_Plus_Rational(opaque_Rational a, opaque_Rational b) { return new opaque_Rational { num = a.num * b.den + a.den * b.num, den = a.den * b.den }; }
            public bool op_Rational_Less_Rational(opaque_Rational a, opaque_Rational b) { return a.num * b.den < a.den * b.num; }
        }
    }

    namespace Math
//doc 
//    Title:   "Math"
//    Purpose: "Floating-point numbers and matrices"
//    Package: "Aha! Base Library"
//    Author:  "Roman Movchan"
//    Created: "2010-10-16"
//end

//use Rational: Base/Rational
//import Rational(Types)

//export Types:
//    type Float: opaque "a floating-point number"
//    type FormatParams:
//        [
//            general:
//                [
//                    period: character "character for decimal period"
//                ] "general format" |
//            fixed:
//                [
//                    period: character "character for decimal period"
//                    decimals: integer "number of decimals"
//                ] "fixed format" |
//            exponent: 
//                [
//                    period: character "character for decimal period"
//                ] "exponential format"
//        ] "number formatting parameters"
//end

//export Operators:
//    (Float + Float): { Float, Float -> Float } "the sum of two floats"
//    (Float - Float): { Float, Float -> Float } "the difference between two floats"
//    (Float * Float): { Float, Float -> Float } "the product of two floats"
//    (Float / Float): { Float, Float -> Float } "the ratio of two floats"
//    (Float < Float): { Float, Float } "is first float less than second?"
//    (Float <= Float): { Float, Float } "is first float less than or equal to second?"
//    (Float = Float): { Float, Float } "is first float equal to second?"
//    (Float /= Float): { Float, Float } "is first float not equal to second?"
//    (Float >= Float): { Float, Float } "is first float greater than or equal to second?"
//    (Float > Float): { Float, Float } "is first float greater than second?"
//    (~float integer): { integer -> Float } "convert integer to Float"
//    (~float Rational): { Rational -> Float } "convert Rational to Float"
//end

//export Functions:
//    the sin: { Float -> Float } "the sine function"
//    the cos: { Float -> Float } "the cosine function"
//    the exp: { Float -> Float } "the exponent function"
//    the log: { Float -> Float } "the logarithm function"
//    the tan: { Float -> Float } "the tangent function"
//    the Pi: Float "the pi number"
//    the Infinity: Float "+infinity"
//    the NegInfinity: Float "-infinity"
//    the Trunc: { Float -> integer } "truncate Float to integer"
//    the Round: { Float, integer -> Rational } "round Float to given number of decimals after decimal point"
//    the FloatToString: { Float, FormatParams -> [character] } "convert Float to string"
//    the StringToFloat: { [character], FormatParams -> Float } "convert string to Float"
//end

//export MatrixAlgebra:
//    type Scalar: Float "alias for floating-point numbers"
//    type Matrix: opaque "a matrix"
//    the Size: { Matrix -> [ rows: integer columns: integer ] } "matrix dimensions"
//    (Matrix @ [ row: integer col: integer ]): { Matrix, [ row: integer col: integer ] -> Scalar } "matrix element with given coordinates"
//    (~id integer): { integer -> Matrix } "identity matrix of given size"
//    (~rows [[integer]]): { [[integer]] -> Matrix } "build matrix from rows of integers (must be of same size)"
//    (~columns [[integer]]): { [[integer]] -> Matrix } "build matrix from columns of integers (must be of same size)"
//    (~rows [[Rational]]): { [[Rational]] -> Matrix } "build matrix from rows of rationals (must be of same size)"
//    (~columns [[Rational]]): { [[Rational]] -> Matrix } "build matrix from columns of rationals (must be of same size)"
//    (~rows [[Scalar]]): { [[Scalar]] -> Matrix } "build matrix from rows of scalars (must be of same size)"
//    (~columns [[Scalar]]): { [[Scalar]] -> Matrix } "build matrix from columns of scalars (must be of same size)"
//    (Scalar * Matrix): { Scalar, Matrix -> Matrix } "multiply matrix by scalar"
//    (Matrix + Matrix): { Matrix, Matrix -> Matrix } "sum of matrices"
//    (Matrix - Matrix): { Matrix, Matrix -> Matrix } "difference of matrices"
//    (Matrix * Matrix): { Matrix, Matrix -> Matrix } "product of matrices"
//    the Det: { Matrix -> Scalar } "determinant"
//    (~inv Matrix): { Matrix -> Matrix } "inverse matrix"
//    (~tr Matrix): { Matrix -> Matrix } "transpose matrix"
//end
    {
        public struct opaque_Float
        {
            public double value;
        }

        public class opaque_Matrix
        {
            public double[,] value;
        }

        public interface icomp_GeneralFormatParams
        {
            char attr_period();
        }

        public interface icomp_FixedFormatParams
        {
            char attr_period();
            Int64 attr_decimals();
        }

        public interface icomp_ExponentFormatParams
        {
            char attr_period();
        }

        public interface icomp_FormatParams
        {
            icomp_GeneralFormatParams attr_general();
            icomp_FixedFormatParams attr_fixed();
            icomp_ExponentFormatParams attr_exponent();
        }

        public interface imod_Math
        {
            opaque_Float op__float_integer(Int64 x);
            opaque_Float op__float_Rational(Rational.opaque_Rational x);
            opaque_Float op_Float_Plus_Float(opaque_Float a, opaque_Float b);
            opaque_Float op_Float_Minus_Float(opaque_Float a, opaque_Float b);
            opaque_Float op_Float_Times_Float(opaque_Float a, opaque_Float b);
            opaque_Float op_Float_Div_Float(opaque_Float a, opaque_Float b);
            opaque_Float op_Float_StarStar_integer(opaque_Float a, Int64 b);
            opaque_Float op_Float_StarStar_Float(opaque_Float a, opaque_Float b);
            bool op_Float_Less_Float(opaque_Float a, opaque_Float b);
            bool op_Float_LessEqual_Float(opaque_Float a, opaque_Float b);
            bool op_Float_Equal_Float(opaque_Float a, opaque_Float b);
            bool op_Float_NotEqual_Float(opaque_Float a, opaque_Float b);
            bool op_Float_GreaterEqual_Float(opaque_Float a, opaque_Float b);
            bool op_Float_Greater_Float(opaque_Float a, opaque_Float b);
            opaque_Float fattr_sin(opaque_Float a);
            opaque_Float fattr_cos(opaque_Float a);
            opaque_Float fattr_exp(opaque_Float a);
            opaque_Float fattr_log(opaque_Float a);
            opaque_Float fattr_tan(opaque_Float a);
            opaque_Float fattr_sqrt(opaque_Float a);
            opaque_Float attr_Pi();
        }

        public class module_Math : AhaModule, imod_Math
        {
            public opaque_Float op__float_integer(Int64 x) { return new opaque_Float { value = (double)x }; }
            public opaque_Float op__float_Rational(Rational.opaque_Rational x) { return new opaque_Float { value = (double)x.num / (double)x.den }; }
            public opaque_Float op_Float_Plus_Float(opaque_Float a, opaque_Float b) { return new opaque_Float { value = a.value + b.value }; }
            public opaque_Float op_Float_Minus_Float(opaque_Float a, opaque_Float b) { return new opaque_Float { value = a.value - b.value }; }
            public opaque_Float op_Float_Times_Float(opaque_Float a, opaque_Float b) { return new opaque_Float { value = a.value * b.value }; }
            public opaque_Float op_Float_Div_Float(opaque_Float a, opaque_Float b) { return new opaque_Float { value = a.value / b.value }; }
            public opaque_Float op_Float_StarStar_integer(opaque_Float a, Int64 b) { return new opaque_Float { value = System.Math.Pow(a.value, (double)b) }; }
            public opaque_Float op_Float_StarStar_Float(opaque_Float a, opaque_Float b) { return new opaque_Float { value = System.Math.Pow(a.value, b.value) }; }
            public bool op_Float_Less_Float(opaque_Float a, opaque_Float b) { return a.value < b.value; }
            public bool op_Float_LessEqual_Float(opaque_Float a, opaque_Float b) { return a.value <= b.value; }
            public bool op_Float_Equal_Float(opaque_Float a, opaque_Float b) { return a.value == b.value; }
            public bool op_Float_NotEqual_Float(opaque_Float a, opaque_Float b) { return a.value != b.value; }
            public bool op_Float_GreaterEqual_Float(opaque_Float a, opaque_Float b) { return a.value >= b.value; }
            public bool op_Float_Greater_Float(opaque_Float a, opaque_Float b) { return a.value > b.value; }
            public opaque_Float fattr_sin(opaque_Float a) { return new opaque_Float { value = System.Math.Sin(a.value) }; }
            public opaque_Float fattr_cos(opaque_Float a) { return new opaque_Float { value = System.Math.Cos(a.value) }; }
            public opaque_Float fattr_exp(opaque_Float a) { return new opaque_Float { value = System.Math.Exp(a.value) }; }
            public opaque_Float fattr_log(opaque_Float a) { return new opaque_Float { value = System.Math.Log(a.value) }; }
            public opaque_Float fattr_tan(opaque_Float a) { return new opaque_Float { value = System.Math.Tan(a.value) }; }
            public opaque_Float fattr_sqrt(opaque_Float a) { return new opaque_Float { value = System.Math.Sqrt(a.value) }; }
            public opaque_Float attr_Pi() { return new opaque_Float { value = System.Math.PI }; }
        }
    }

    namespace Time
    //doc 
    //    Title:   "Time"
    //    Package: "Aha! Base Library"
    //    Purpose: "Date and time manipulation"
    //    Author:  "Roman Movchan, Melbourne, Australia"
    //    Created: "2011-10-11"
    //end

//export Types:
    //    type Timestamp: opaque "Date and time"
    //    type Interval: opaque "Time interval"
    //    type DateStruc:
    //        [
    //            year: integer "year(s)" 
    //            month: integer "month(s)"
    //            day: integer "day(s)"
    //        ] "date as composite"
    //    type TimeStruc:
    //        [
    //            hour: integer "hour(s)"
    //            min: integer "minute(s)"
    //            sec: integer "second(s)"
    //            msec: integer "millisecond(s)"
    //        ] "time as composite"
    //    type TimestampStruc: [ date: DateStruc "date part" time: TimeStruc "time part" ] "timestamp as composite"
    //    type DayOfWeek:
    //        [
    //            monday: "Monday?" |
    //            tuesday: "Tuesday?" |
    //            wednesday: "Wednesday?" |
    //            thursday: "Thursday?" |
    //            friday: "Friday?" |
    //            saturday: "Saturday?" |
    //            sunday: "Sunday?" 
    //        ] "a day of the week"
    //end

//export Utils:
    //    the DayOfWeek: { Timestamp -> DayOfWeek } "day of week for given Timestamp"
    //    the Year: Interval "1-year interval"
    //    the Month: Interval "1-month interval"
    //    the Day: Interval "1-day interval"
    //    the Hour: Interval "1-hour interval"
    //    the Minute: Interval "1-minute interval"
    //    the Second: Interval "1-second interval"
    //    the Millisecond: Interval "1-millisecond interval"
    //    the Tick: Interval "minimum length interval"
    //    the Zero: Interval "zero length interval"
    //end

//export Operators:
    //    (Timestamp - Timestamp): { Timestamp, Timestamp -> Interval } "difference between two timestamps"
    //    (Interval + Interval): { Interval, Interval -> Interval } "sum of intervals"
    //    (Interval - Interval): { Interval, Interval -> Interval } "difference between intervals"
    //    (Timestamp + Interval): { Timestamp, Interval -> Timestamp } "timestamp plus interval"
    //    (Timestamp - Interval): { Timestamp, Interval -> Timestamp } "timestamp minus interval"
    //    (integer * Interval): { integer, Interval -> Interval } "integer times interval"
    //    (Interval / integer): { Interval, integer -> Interval } "interval divided by an integer"
    //    (~date DateStruc): { DateStruc -> Timestamp } "convert DateStruc to Timestamp (date only)"
    //    (~time TimeStruc): { TimeStruc -> Interval } "convert TimeStruc to Interval (from midnight)"
    //    (~date Timestamp): { Timestamp -> Timestamp } "date part of a Timestamp"
    //    (~time Timestamp): { Timestamp -> Interval } "time part of a Timestamp as Interval (from midnight)"
    //    (~struc Timestamp): { Timestamp -> TimestampStruc } "convert Timestamp to TimestampStruc"
    //    (~struc Interval): { Interval -> TimestampStruc } "convert Interval to TimestampStruc"
    //    (~ticks Interval): { Interval -> integer } "number of ticks in Interval"
    //end
    {
        public struct opaque_Timestamp
        {
            public Int64 ticks;
        }

        public struct opaque_Interval
        {
            public Int64 ticks;
        }

        public interface icomp_DateStruc
        {
            Int64 attr_year();
            Int64 attr_month();
            Int64 attr_day();
        }

        public interface icomp_TimeStruc
        {
            Int64 attr_hour();
            Int64 attr_min();
            Int64 attr_sec();
            Int64 attr_msec();
        }

        public interface icomp_TimestampStruc
        {
            icomp_DateStruc attr_date();
            icomp_TimeStruc attr_time();
        }

        public interface icomp_DayOfWeek
        {
            bool attr_Monday();
            bool attr_Tuesday();
            bool attr_Wednesday();
            bool attr_Thursday();
            bool attr_Friday();
            bool attr_Saturday();
            bool attr_Sunday();
        }

        public interface imod_Time
        {
            icomp_DayOfWeek fattr_DayOfWeek(opaque_Timestamp t);
            opaque_Interval attr_Day();
            opaque_Interval attr_Hour();
            opaque_Interval attr_Minute();
            opaque_Interval attr_Second();
            opaque_Interval attr_Millisecond();
            opaque_Interval attr_Tick();
            opaque_Interval attr_Zero();
            Int64 fattr_TimestampHashFunc(opaque_Timestamp t);
            Int64 fattr_IntervalHashFunc(opaque_Interval i);
            opaque_Interval op_Timestamp_Minus_Timestamp(opaque_Timestamp a, opaque_Timestamp b);
            opaque_Timestamp op_Timestamp_Plus_Interval(opaque_Timestamp a, opaque_Interval b);
            opaque_Timestamp op_Timestamp_Minus_Interval(opaque_Timestamp a, opaque_Interval b);
            opaque_Interval op_Interval_Plus_Interval(opaque_Interval a, opaque_Interval b);
            opaque_Interval op_Interval_Minus_Interval(opaque_Interval a, opaque_Interval b);
            opaque_Interval op_integer_Times_Interval(Int64 a, opaque_Interval i);
            opaque_Interval op_Interval_Div_integer(opaque_Interval i, Int64 a);
            bool op_Timestamp_LessEqual_Timestamp(opaque_Timestamp a, opaque_Timestamp b);
            bool op_Timestamp_Less_Timestamp(opaque_Timestamp a, opaque_Timestamp b);
            bool op_Timestamp_Equal_Timestamp(opaque_Timestamp a, opaque_Timestamp b);
            bool op_Timestamp_NotEqual_Timestamp(opaque_Timestamp a, opaque_Timestamp b);
            bool op_Timestamp_Greater_Timestamp(opaque_Timestamp a, opaque_Timestamp b);
            bool op_Timestamp_GreaterEqual_Timestamp(opaque_Timestamp a, opaque_Timestamp b);
            bool op_Interval_LessEqual_Interval(opaque_Interval a, opaque_Interval b);
            bool op_Interval_Less_Interval(opaque_Interval a, opaque_Interval b);
            bool op_Interval_Equal_Interval(opaque_Interval a, opaque_Interval b);
            bool op_Interval_NotEqual_Interval(opaque_Interval a, opaque_Interval b);
            bool op_Interval_Greater_Interval(opaque_Interval a, opaque_Interval b);
            bool op_Interval_GreaterEqual_Interval(opaque_Interval a, opaque_Interval b);
            opaque_Timestamp op__date_DateStruc(icomp_DateStruc date);
            opaque_Interval op__time_TimeStruc(icomp_TimeStruc time);
            Int64 op__ticks_Interval(opaque_Interval i);
            opaque_Interval op__interval_integer(Int64 param_ticks);
        }

        public class module_Time : AhaModule
        {
            struct comp_DateStruc : icomp_DateStruc
            {
                public DateTime dt;
                public Int64 attr_year() { return dt.Year; }
                public Int64 attr_month() { return dt.Month; }
                public Int64 attr_day() { return dt.Day; }
            }

            struct comp_DayOfWeek : icomp_DayOfWeek
            {
                public System.DayOfWeek value;
                public bool attr_Monday() { return value == System.DayOfWeek.Monday; }
                public bool attr_Tuesday() { return value == System.DayOfWeek.Tuesday; }
                public bool attr_Wednesday() { return value == System.DayOfWeek.Wednesday; }
                public bool attr_Thursday() { return value == System.DayOfWeek.Thursday; }
                public bool attr_Friday() { return value == System.DayOfWeek.Friday; }
                public bool attr_Saturday() { return value == System.DayOfWeek.Saturday; }
                public bool attr_Sunday() { return value == System.DayOfWeek.Sunday; }
            }

            public icomp_DayOfWeek fattr_DayOfWeek(opaque_Timestamp t) { return new comp_DayOfWeek { value = (new DateTime(t.ticks)).DayOfWeek }; }
            public opaque_Interval attr_Day() { return new opaque_Interval { ticks = (new TimeSpan(1, 0, 0, 0)).Ticks }; }
            public opaque_Interval attr_Hour() { return new opaque_Interval { ticks = 14400000000 }; }
            public opaque_Interval attr_Minute() { return new opaque_Interval { ticks = 600000000 }; }
            public opaque_Interval attr_Second() { return new opaque_Interval { ticks = 10000000 }; }
            public opaque_Interval attr_Millisecond() { return new opaque_Interval { ticks = 10000 }; }
            public opaque_Interval attr_Tick() { return new opaque_Interval { ticks = 1 }; }
            public opaque_Interval attr_Zero() { return new opaque_Interval { ticks = 0 }; }
            public Int64 fattr_TimestampHashFunc(opaque_Timestamp t) { return (new DateTime(t.ticks)).GetHashCode(); }
            public Int64 fattr_IntervalHashFunc(opaque_Interval i) { return (new TimeSpan(i.ticks)).GetHashCode(); }
            public opaque_Interval op_Timestamp_Minus_Timestamp(opaque_Timestamp a, opaque_Timestamp b) { return new opaque_Interval { ticks = a.ticks - b.ticks }; }
            public opaque_Timestamp op_Timestamp_Plus_Interval(opaque_Timestamp a, opaque_Interval b) { return new opaque_Timestamp { ticks = a.ticks + b.ticks }; }
            public opaque_Timestamp op_Timestamp_Minus_Interval(opaque_Timestamp a, opaque_Interval b) { return new opaque_Timestamp { ticks = a.ticks - b.ticks }; }
            public opaque_Interval op_Interval_Plus_Interval(opaque_Interval a, opaque_Interval b) { return new opaque_Interval { ticks = a.ticks + b.ticks }; }
            public opaque_Interval op_Interval_Minus_Interval(opaque_Interval a, opaque_Interval b) { return new opaque_Interval { ticks = a.ticks - b.ticks }; }
            public opaque_Interval op_integer_Times_Interval(Int64 a, opaque_Interval i) { return new opaque_Interval { ticks = a * i.ticks }; }
            public opaque_Interval op_Interval_Div_integer(opaque_Interval i, Int64 a) { return new opaque_Interval { ticks = i.ticks / a }; }
            public bool op_Timestamp_LessEqual_Timestamp(opaque_Timestamp a, opaque_Timestamp b) { return a.ticks <= b.ticks; }
            public bool op_Timestamp_Less_Timestamp(opaque_Timestamp a, opaque_Timestamp b) { return a.ticks < b.ticks; }
            public bool op_Timestamp_Equal_Timestamp(opaque_Timestamp a, opaque_Timestamp b) { return a.ticks == b.ticks; }
            public bool op_Timestamp_NotEqual_Timestamp(opaque_Timestamp a, opaque_Timestamp b) { return a.ticks != b.ticks; }
            public bool op_Timestamp_Greater_Timestamp(opaque_Timestamp a, opaque_Timestamp b) { return a.ticks > b.ticks; }
            public bool op_Timestamp_GreaterEqual_Timestamp(opaque_Timestamp a, opaque_Timestamp b) { return a.ticks >= b.ticks; }
            public bool op_Interval_LessEqual_Interval(opaque_Interval a, opaque_Interval b) { return a.ticks <= b.ticks; }
            public bool op_Interval_Less_Interval(opaque_Interval a, opaque_Interval b) { return a.ticks < b.ticks; }
            public bool op_Interval_Equal_Interval(opaque_Interval a, opaque_Interval b) { return a.ticks == b.ticks; }
            public bool op_Interval_NotEqual_Interval(opaque_Interval a, opaque_Interval b) { return a.ticks != b.ticks; }
            public bool op_Interval_Greater_Interval(opaque_Interval a, opaque_Interval b) { return a.ticks > b.ticks; }
            public bool op_Interval_GreaterEqual_Interval(opaque_Interval a, opaque_Interval b) { return a.ticks >= b.ticks; }
            public opaque_Timestamp op__date_DateStruc(icomp_DateStruc date) { return new opaque_Timestamp { ticks = (new DateTime((int)date.attr_year(), (int)date.attr_month(), (int)date.attr_day())).Ticks }; }
            public opaque_Interval op__time_TimeStruc(icomp_TimeStruc time) { return new opaque_Interval { ticks = (new TimeSpan(0, (int)time.attr_hour(), (int)time.attr_min(), (int)time.attr_sec(), (int)time.attr_msec())).Ticks }; }
            public Int64 op__ticks_Interval(opaque_Interval i) { return i.ticks; }
            public opaque_Interval op__interval_integer(Int64 param_ticks) { return new opaque_Interval { ticks = param_ticks }; }
        }
    }

    namespace StrUtils
    //doc
    //    Title:   "StrUtils"
    //    Package: "Aha! Base Library"
    //    Purpose: "String utilities"
    //    Author:  "Roman Movchan, Melbourne, Australia"
    //    Created: "2012-06-03"
    //end

//export Types:
    //    type String: [character] "character string"
    //    type Substring:
    //        [
    //            index: integer "first character index"
    //            length: integer "substring length"
    //        ] "identifies a substring inside a string"
    //    type RegEx: opaque "a regular expression"
    //    type Pattern: 
    //        [
    //            string: String "exact search string" |
    //            regEx: RegEx "regular expression" |
    //            equality: { character, character } "character-wise equality relation"
    //        ] "search pattern"
    //    type SearchParams:
    //        [
    //            for: Pattern "search pattern"
    //             in: String "where to search"
    //        ] "search parameters"
    //    type CharCompare: { character, character -> integer } "character comparison function: negative - <, zero - =, positive - >"
    //    type StringCompare: { String, String -> integer } "string comparison function: negative - <, zero - =, positive - >"
    //end

//export Utils:
    //    the Substr: { String, Substring -> String } "extract substring from string"
    //    the RegEx: { String -> RegEx } "construct regular expression from string"
    //    the Search: { SearchParams -> Substring* } "return all occurrences of search pattern in string as a sequence"    
    //    the StringBuilder:
    //        obj String
    //            add(character) "add a single character"
    //            put([ at: integer char: character ]) "replace character at given index"
    //            append(String) "append string to the end"
    //            replace([ substr: [Substring] with: String ]) "simultaneously replace multiple non-overlapping substrings with string"
    //            padLeft([ with: character to: integer ]) "pad with given character from left to given length"
    //            padRight([ with: character to: integer ]) "pad with given character from right to given length"
    //            extract(Substring) "extract substring"
    //            trimSpaces "trim spaces"
    //            apply({ character -> character }) "convert all characters using provided function"
    //        end "the string builder"
    //    the StringHashFunc: { String -> integer } "standard hash function for strings"
    //    the StringCompare: { CharCompare -> StringCompare } "string comparison function from character comparison function"
    //end

//export Operators:
    //    (String = String): { String, String } "compare strings"
    //end
    {
        public interface icomp_Substring
        {
            Int64 attr_index();
            Int64 attr_length();
        }

        public interface icomp_Pattern
        {
            IahaArray<char> attr_string();
            opaque_RegEx attr_regEx();
            bool fattr_equality(char a, char b);
        }

        public interface icomp_SearchParams
        {
            icomp_Pattern attr_for();
            IahaArray<char> attr_in();
        }

        public interface icomp_PutParams
        {
            Int64 attr_at();
            char attr_char();
        }

        public interface icomp_ReplaceParams
        {
            IahaArray<icomp_Substring> attr_substr();
            IahaArray<char> attr_with();
        }

        public interface icomp_PadParams
        {
            char attr_with();
            Int64 attr_to();
        }

        public interface iobj_StringBuilder : IahaObject<IahaArray<char>>
        {
            void action_add(char ch);
            void action_put(icomp_PutParams param);
            void action_append(IahaArray<char> str);
            void action_replace(icomp_ReplaceParams param);
            void action_extract(icomp_Substring sub);
            //void action_padL(IPadParams param);
            //void action_padR(IPadParams param);
            void action_trimSpaces();
        }

        public interface imod_StrUtils
        {
            IahaArray<char> fattr_Substr(IahaArray<char> s, icomp_Substring ss);
            opaque_RegEx fattr_RegEx(IahaArray<char> s);
            IahaSequence<icomp_Substring> fattr_Search(icomp_SearchParams param);
            iobj_StringBuilder attr_StringBuilder();
            Int64 fattr_StringHashFunc(IahaArray<char> s);
        }

        public struct opaque_RegEx
        {
            public string value;
        }

        public class module_StrUtils : AhaModule, imod_StrUtils
        {
            class obj_SearchSeq : IahaSequence<icomp_Substring>
            {
                struct comp_Substring : icomp_Substring
                {
                    public int field_index;
                    private int field_length;
                    public Int64 attr_index() { return field_index; }
                    public Int64 attr_length() { return field_length; }
                    public comp_Substring(int i, int l) { field_index = i; field_length = l; }
                }

                private string str;
                private string sub;
                private int index;
                public obj_SearchSeq(string s, string ss) { str = s; sub = ss; index = s.IndexOf(ss); }
                public icomp_Substring state() { return new comp_Substring(index, sub.Length); }
                public IahaObject<icomp_Substring> copy() { return new obj_SearchSeq(str, sub) { index = index }; }
                public void action_skip() { index = str.IndexOf(sub, index + 1); if (index == -1) throw Failure.One; }
                public icomp_Substring first(Predicate<icomp_Substring> that, Int64 max)
                {
                    int i = index;
                    Int64 j = 0;
                    comp_Substring substr = new comp_Substring(i, sub.Length);
                    while (i != -1 && j < max)
                    {
                        substr.field_index = i;
                        if (that(substr)) return substr;
                        i = str.IndexOf(sub, i + 1);
                        j++;
                    }
                    throw Failure.One;
                }
            }

            public IahaArray<char> fattr_Substr(IahaArray<char> s, icomp_Substring ss) { char[] items = new char[ss.attr_length()]; Array.Copy(s.get(), ss.attr_index(), items, 0, ss.attr_length()); return new AhaString(items); }
            public opaque_RegEx fattr_RegEx(IahaArray<char> s) { return new opaque_RegEx { value = new string(s.get()) }; }
            public IahaSequence<icomp_Substring> fattr_Search(icomp_SearchParams param)
            {
                IahaArray<char> sub = param.attr_for().attr_string();
                string temp1 = new string(sub.get());
                string temp2 = new string(param.attr_in().get());
                return new obj_SearchSeq(temp2, temp1);
            }
            public iobj_StringBuilder attr_StringBuilder() { return new obj_StringBuilder(); }
            public Int64 fattr_StringHashFunc(IahaArray<char> s) { return s.get().GetHashCode(); }
        }
    }

    namespace Trees
//doc 
//    Title:   "Trees"
//    Purpose: "Generic trees"
//    Author:  "Roman Movchan, Melbourne, Australia"
//    Created: "2013-08-10"
//end

//type Node: arbitrary "Tree node"

//export Types:
//    type Tree: opaque "abstract tree"
//    type Path: opaque "path to a sub-tree (sequence of child indexes), starting from the root"
//end

//export Constructors:
//    the Tree: { [ root: Node children: [Tree] ] -> Tree } "create a tree with given root and children"
//    the Leaf: { Node -> Tree } "create a leaf from given node"
//end

//export Utils:
//    the Root: { Tree -> Node } "tree's root node"
//    the Children: { Tree -> [Tree] } "all children of a tree"
//    the NodesByLevel: { Tree -> Node* } "enumerate all tree nodes level by level"
//    the NodesByBranch: { Tree -> Node* } "enumerate all tree nodes branch by branch"
//    the PathsByLevel: { Tree -> Path* } "paths to all sub-trees level by level"
//    the PathsByBranch: { Tree -> Path* } "paths to all sub-trees branch by branch"
//    the Subtree: { Tree, Path -> Tree } "tree's sub-tree at given path"
//    the Ancestors: { Tree, Path -> Tree* } "all ancestors of sub-tree at given path, starting from its parent"
//    the LevelCount: { Tree -> integer } "tree's number of levels"
//    the NodeCount: { Tree -> integer } "tree's total number of nodes"
//    the InsertSubtree: { Tree, [ path: Path subtree: Tree ] -> Tree } "insert sub-tree into given tree at given path"
//    the ReplaceSubtree: { Tree, [ path: Path subtree: Tree ] -> Tree } "replace sub-tree of given tree at given path"
//    the DeleteSubtree: { Tree, Path -> Tree } "delete sub-tree of given tree at given path"
//end    
    {
        public class opaque_Tree<tpar_Node>
        {
            public tpar_Node root;
            public opaque_Tree<tpar_Node>[] children;
            public int levels;
        }

        public struct opaque_Path<tpar_Node>
        {
            public Int64[] indexes;
        }

        public interface icomp_TreeParam<tpar_Node>
        {
            tpar_Node attr_root();
            IahaArray<opaque_Tree<tpar_Node>> attr_children();
        }

        public interface imod_Trees<tpar_Node>
        {
            opaque_Tree<tpar_Node> fattr_Tree(icomp_TreeParam<tpar_Node> param);
            opaque_Tree<tpar_Node> fattr_Leaf(tpar_Node node);
            tpar_Node fattr_Root(opaque_Tree<tpar_Node> tree);
            IahaArray<opaque_Tree<tpar_Node>> fattr_Children(opaque_Tree<tpar_Node> tree);
            IahaSequence<tpar_Node> fattr_NodesByLevel(opaque_Tree<tpar_Node> tree);
            IahaSequence<tpar_Node> fattr_NodesByBranch(opaque_Tree<tpar_Node> tree);
            Int64 fattr_LevelCount(opaque_Tree<tpar_Node> tree);
            Int64 fattr_NodeCount(opaque_Tree<tpar_Node> tree);
            opaque_Tree<tpar_Node> op_Tree_at_Path(opaque_Tree<tpar_Node> tree, opaque_Path<tpar_Node> path);
            IahaSequence<Int64> fattr_Indexes(opaque_Path<tpar_Node> path);
            IahaSequence<opaque_Tree<tpar_Node>> fattr_Ancestors(opaque_Tree<tpar_Node> tree, opaque_Path<tpar_Node> path);
        }

        public class module_Trees<tpar_Node> : AhaModule, imod_Trees<tpar_Node>
        {
            class obj_NodesByLevel : IahaSequence<tpar_Node>
            {
                private Queue<opaque_Tree<tpar_Node>> queue = new Queue<opaque_Tree<tpar_Node>>();
                public obj_NodesByLevel(opaque_Tree<tpar_Node> tree) { queue.Enqueue(tree); }
                public tpar_Node state() { return queue.Peek().root; }
                public IahaObject<tpar_Node> copy() { return new obj_NodesByLevel(queue.Peek()); }
                public void action_skip() 
                {
                    opaque_Tree<tpar_Node> top = queue.Dequeue();
                    if (top.children != null)
                    {
                        foreach (opaque_Tree<tpar_Node> child in top.children)
                            queue.Enqueue(child);
                    }
                }
                public tpar_Node first(Predicate<tpar_Node> that, Int64 max)
                {
                    Queue<opaque_Tree<tpar_Node>> q = new Queue<opaque_Tree<tpar_Node>>();
                    q.Enqueue(queue.Peek());
                    while (q.Count > 0)
                    {
                        opaque_Tree<tpar_Node> top = q.Dequeue();
                        if (that(top.root)) return top.root;
                        if (top.children != null)
                        {
                            foreach (opaque_Tree<tpar_Node> child in top.children)
                                queue.Enqueue(child);
                        }
                    }
                    throw Failure.One;
                }
            }

            class obj_NodesByBranch : IahaSequence<tpar_Node>
            {
                private Stack<opaque_Tree<tpar_Node>> stack = new Stack<opaque_Tree<tpar_Node>>();
                public obj_NodesByBranch(opaque_Tree<tpar_Node> tree) { stack.Push(tree); }
                public tpar_Node state() { return stack.Peek().root; }
                public IahaObject<tpar_Node> copy() { return new obj_NodesByBranch(stack.Peek()); }
                public void action_skip()
                {
                    opaque_Tree<tpar_Node> top = stack.Pop();
                    if (top.children != null)
                    {
                        for (int i = top.children.Length - 1; i >= 0; i--)
                            stack.Push(top.children[i]);
                    }
                }
                public tpar_Node first(Predicate<tpar_Node> that, Int64 max)
                {
                    Stack<opaque_Tree<tpar_Node>> q = new Stack<opaque_Tree<tpar_Node>>();
                    q.Push(stack.Peek());
                    while (q.Count > 0)
                    {
                        opaque_Tree<tpar_Node> top = q.Pop();
                        if (that(top.root)) return top.root;
                        if (top.children != null)
                        {
                            for (int i = top.children.Length - 1; i >= 0; i--)
                                stack.Push(top.children[i]);
                        }
                    }
                    throw Failure.One;
                }
            }

            class obj_PathsByLevel : IahaSequence<opaque_Path<tpar_Node>>
            {
                struct Subtree
                {
                    public opaque_Tree<tpar_Node> tree;
                    public Int64[] indexes;
                }

                private Queue<Subtree> queue = new Queue<Subtree>();
                public obj_PathsByLevel(opaque_Tree<tpar_Node> tree) { queue.Enqueue(new Subtree { tree = tree, indexes = new Int64[0] }); }
                public opaque_Path<tpar_Node> state() { return new opaque_Path<tpar_Node> { indexes = queue.Peek().indexes }; }
                public IahaObject<opaque_Path<tpar_Node>> copy() { return new obj_PathsByLevel(queue.Peek().tree); }
                public void action_skip()
                {
                    Subtree top = queue.Dequeue();
                    if (top.tree.children != null)
                    {
                        for (int i = 0; i < top.tree.children.Length; i++)
                        {
                            Int64[] indexes = new Int64[top.indexes.Length + 1];
                            for (int j = 0; j < top.indexes.Length; j++)
                                indexes[j] = top.indexes[j];
                            indexes[top.indexes.Length] = i;
                            queue.Enqueue(new Subtree { tree = top.tree.children[i], indexes = indexes });
                        }
                    }
                }
                public opaque_Path<tpar_Node> first(Predicate<opaque_Path<tpar_Node>> that, Int64 max)
                {
                    Queue<Subtree> q = new Queue<Subtree>();
                    q.Enqueue(queue.Peek());
                    while (q.Count > 0)
                    {
                        Subtree top = q.Dequeue();
                        opaque_Path<tpar_Node> path = new opaque_Path<tpar_Node> { indexes = top.indexes };
                        if (that(path)) return path;
                        if (top.tree.children != null)
                        {
                            for (int i = top.tree.children.Length - 1; i >= 0; i--)
                            {
                                Int64[] indexes = new Int64[top.indexes.Length + 1];
                                for (int j = 0; j < top.indexes.Length; j++)
                                    indexes[j] = top.indexes[j];
                                indexes[top.indexes.Length] = i;
                                q.Enqueue(new Subtree { tree = top.tree.children[i], indexes = indexes });
                            }
                        }
                    }
                    throw Failure.One;
                }
            }

            class obj_PathsByBranch : IahaSequence<opaque_Path<tpar_Node>>
            {
                struct Subtree
                {
                    public opaque_Tree<tpar_Node> tree;
                    public Int64[] indexes;
                }

                private Stack<Subtree> stack = new Stack<Subtree>();
                public obj_PathsByBranch(opaque_Tree<tpar_Node> tree) { stack.Push(new Subtree { tree = tree, indexes = new Int64[0] }); }
                public opaque_Path<tpar_Node> state() { return new opaque_Path<tpar_Node> { indexes = stack.Peek().indexes }; }
                public IahaObject<opaque_Path<tpar_Node>> copy() { return new obj_PathsByBranch(stack.Peek().tree); }
                public void action_skip()
                {
                    Subtree top = stack.Pop();
                    if (top.tree.children != null)
                    {
                        for (int i = top.tree.children.Length - 1; i >= 0; i--)
                        {
                            Int64[] indexes = new Int64[top.indexes.Length + 1];
                            for (int j = 0; j < top.indexes.Length; j++)
                                indexes[j] = top.indexes[j];
                            indexes[top.indexes.Length] = i;
                            stack.Push(new Subtree { tree = top.tree.children[i], indexes = indexes });
                        }
                    }
                }
                public opaque_Path<tpar_Node> first(Predicate<opaque_Path<tpar_Node>> that, Int64 max)
                {
                    Stack<Subtree> q = new Stack<Subtree>();
                    q.Push(stack.Peek());
                    while (q.Count > 0)
                    {
                        Subtree top = q.Pop();
                        opaque_Path<tpar_Node> path = new opaque_Path<tpar_Node> { indexes = top.indexes };
                        if (that(path)) return path;
                        if (top.tree.children != null)
                        {
                            for (int i = top.tree.children.Length - 1; i >= 0; i--)
                            {
                                Int64[] indexes = new Int64[top.indexes.Length + 1];
                                for (int j = 0; j < top.indexes.Length; j++)
                                    indexes[j] = top.indexes[j];
                                indexes[top.indexes.Length] = i;
                                q.Push(new Subtree { tree = top.tree.children[i], indexes = indexes });
                            }
                        }
                    }
                    throw Failure.One;
                }
            }

            public opaque_Tree<tpar_Node> fattr_Tree(icomp_TreeParam<tpar_Node> param)
            {
                int levels = 0;
                foreach (opaque_Tree<tpar_Node> child in param.attr_children().get())
                    if (child.levels > levels) levels = child.levels;
                return new opaque_Tree<tpar_Node>() { root = param.attr_root(), children = param.attr_children().get(), levels = levels + 1 }; 
            }
            public opaque_Tree<tpar_Node> fattr_Leaf(tpar_Node node) { return new opaque_Tree<tpar_Node>() { root = node, children = null, levels = 0 }; }
            public tpar_Node fattr_Root(opaque_Tree<tpar_Node> tree) { return tree.root; }
            public IahaArray<opaque_Tree<tpar_Node>> fattr_Children(opaque_Tree<tpar_Node> tree) 
            { 
                if (tree.children == null) 
                    return new AhaArray<opaque_Tree<tpar_Node>>(new opaque_Tree<tpar_Node>[] { }); 
                else
                    return new AhaArray<opaque_Tree<tpar_Node>>(tree.children); 
            }
            public IahaSequence<tpar_Node> fattr_NodesByLevel(opaque_Tree<tpar_Node> tree) { return new obj_NodesByLevel(tree); }
            public IahaSequence<tpar_Node> fattr_NodesByBranch(opaque_Tree<tpar_Node> tree) { return new obj_NodesByBranch(tree); }
            public Int64 fattr_LevelCount(opaque_Tree<tpar_Node> tree) { return tree.levels; }
            public Int64 fattr_NodeCount(opaque_Tree<tpar_Node> tree)
            {
                int count = 0;
                Stack<opaque_Tree<tpar_Node>> stack = new Stack<opaque_Tree<tpar_Node>>();
                opaque_Tree<tpar_Node> subtree;
                stack.Push(tree);
                while (stack.Count > 0)
                {
                    subtree = stack.Pop();
                    if (subtree.children != null)
                    {
                        count += subtree.children.Length;
                        foreach (opaque_Tree<tpar_Node> child in subtree.children)
                            stack.Push(child);
                    }
                }
                return count;
            }
            public opaque_Tree<tpar_Node> op_Tree_at_Path(opaque_Tree<tpar_Node> tree, opaque_Path<tpar_Node> path)
            {
                opaque_Tree<tpar_Node> result = tree;
                int i = 0;
                while (i < path.indexes.Length)
                {
                    result = result.children[path.indexes[i]];
                    i++;
                }
                return result;
            }
            public IahaSequence<Int64> fattr_Indexes(opaque_Path<tpar_Node> path) { return new AhaArraySeq<Int64> { items = path.indexes, index = 0 }; }
            public IahaSequence<opaque_Tree<tpar_Node>> fattr_Ancestors(opaque_Tree<tpar_Node> tree, opaque_Path<tpar_Node> path)
            {
                opaque_Tree<tpar_Node>[] items = new opaque_Tree<tpar_Node>[path.indexes.Length - 1];
                opaque_Tree<tpar_Node> temp = tree;
                int i = 0;
                int j = path.indexes.Length - 2;
                while (i < path.indexes.Length - 1)
                {
                    items[j] = temp;
                    temp = temp.children[path.indexes[i]];
                    i++;
                    j--;
                }
                return new AhaArraySeq<opaque_Tree<tpar_Node>> { items = items, index = 0 };
            }
        }
    }

    namespace Bits
    //doc 
    //    Title:   "Bits"
    //    Purpose: "Working with bits and bit strings"
    //    Package: "Aha! Base Library"
    //    Author:  "Roman Movchan"
    //    Created: "2012-06-07"
    //end

//export Types:
    //    type BitString: opaque "a string of bits"
    //    type BitStrCom:
    //        [
    //            at: { integer } "is bit at index non-zero?"
    //            size: integer "size of the string"
    //        ] "a structure that provides the info about a bit string"
    //    type DynamicBitString:
    //        obj BitString
    //            append(BitString) "append a bit string"
    //            resize(integer) "set size to given value, pad with 0's"
    //            set(integer) "set bit at index to 1"
    //            reset(integer) "set bit at index to 0"
    //            invert "invert all bits in bit string"
    //        end "a dynamic (changeable) bit string"
    //    type Substring:
    //        [
    //            index: integer "first bit index"
    //            length: integer"number of bits"
    //        ] "a structure representing a substring of a bit string"
    //end

//export Constructors:
    //    the DynamicBitString: DynamicBitString "zero-length dynamic bit string"
    //end

//export Utils:
    //    the Substr: { BitString, Substring -> BitString } "extract a substring from bit string"
    //    the True: BitString "bit string consisting of a single '1'"
    //    the False: BitString "bit string consisting of a single '0'"
    //    the Nil: BitString "a zero-length bit string"
    //end

//export Operators:
    //    (BitString = BitString): { BitString, BitString } "are bit strings equal?"
    //    (BitString & BitString): { BitString, BitString -> BitString } "logical AND of two bit strings"
    //    (BitString | BitString): { BitString, BitString -> BitString } "logical OR of two bit strings"
    //    (BitString || BitString): { BitString, BitString -> BitString } "logical XOR of two bit strings"
    //    (- BitString): { BitString -> BitString } "inverse of a bit string"
    //    (BitString << integer): { BitString, integer -> BitString } "shift left, padding with zeros"
    //    (BitString >> integer): { BitString, integer -> BitString } "shift right, losing last bits"
    //    (BitString + BitString): { BitString, BitString -> BitString } "concatenation of two bit strings"
    //    (BitString * integer): { BitString, integer -> BitString } "repeat a bit string a number of times"
    //    (~bits character): { character -> BitString } "convert a character to a Unicode 16-bit string"
    //    (~bits integer): { integer -> BitString } "convert an integer to a 64-bit string"
    //    (~struc BitString): { BitString -> BitStrCom } "convert a bit string to a structure"
    //    (~char BitString): { BitString -> character } "convert a 16-bit string to a character"
    //    (~int BitString): { BitString -> integer } "convert a 64-bit string to an integer"
    //end
    {
        public struct opaque_BitString
        {
            public Int64 bits;
            public Byte[] bytes;
        }

        public interface iobj_DynamicBitString : IahaObject<opaque_BitString>
        {
            void action_append(opaque_BitString param_str);
            void action_resize(Int64 index);
            void action_set(Int64 index);
            void action_reset(Int64 index);
        }

        public interface icomp_Substring
        {
            Int64 attr_index();
            Int64 attr_length();
        }

        public interface imod_Bits
        {
            opaque_BitString fattr_Substr(opaque_BitString str, icomp_Substring sub);
            opaque_BitString attr_True();
            opaque_BitString attr_False();
            opaque_BitString attr_Nil();
            opaque_BitString op_BitString_Equal_BitString(opaque_BitString first, opaque_BitString second);
            opaque_BitString op_BitString_And_BitString(opaque_BitString first, opaque_BitString second);
            opaque_BitString op_BitString_Or_BitString(opaque_BitString first, opaque_BitString second);
            opaque_BitString op_BitString_Xor_BitString(opaque_BitString first, opaque_BitString second);
        }
    }

}
