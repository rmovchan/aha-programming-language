//
// Package: Aha! Base Library 
// Author: Roman Movchan
// Created: 2014-04-01

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
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
            bool attr_index(out long result);
            bool attr_item(out Item result);
        }

        public interface icomp_InsertParam<Item>
        {
            bool attr_index(out long result);
            bool attr_item(out Item result);
        }

        public interface iobj_DynamicArray<Item> : IahaObject<IahaArray<Item>>
        {
            bool action_add(Item item);
            bool action_replace(icomp_ReplaceParam<Item> param);
            bool action_insert(icomp_InsertParam<Item> param);
            bool action_delete(long index);
        }

        public interface iobj_DynamicSequence<Item> : IahaObject<Item>
        {
            bool action_push(Item item);
            bool action_pop();
        }

        public interface imod_Collections<Item>
        {
            bool attr_DynamicArray(out iobj_DynamicArray<Item> result);
            bool attr_Stack(out iobj_DynamicSequence<Item> result);
            bool attr_Queue(out iobj_DynamicSequence<Item> result);
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
            public long num;
            public long den;
        }

        public interface icomp_RatioStruc
        {
            bool attr_num(out long result);
            bool attr_den(out long result);
        }

        public interface imod_Rational<opaque_Rational>
        {
            bool op_integer_Slash_integer(long num, long den, out opaque_Rational result);
            bool op__struc(opaque_Rational x, out icomp_RatioStruc result);
            bool op_Rational_Plus_Rational(opaque_Rational a, opaque_Rational b, out opaque_Rational result);
            bool op_Rational_Less_Rational(opaque_Rational a, opaque_Rational b);
        }

        public class module_Rational : AhaModule
        {
            class comp_RatioStruc : icomp_RatioStruc
            {
                private long num;
                private long den;
                public comp_RatioStruc(long n, long d) { num = n; den = d; }
                public bool attr_num(out long result) { result = num; return true; }
                public bool attr_den(out long result) { result = den; return true; }
            }

            public bool op_integer_Slash_integer(long num, long den, out opaque_Rational result) { result = new opaque_Rational { num = num, den = den }; return true; }
            public bool op__struc(opaque_Rational x, out icomp_RatioStruc result) { result = new comp_RatioStruc(x.num, x.den); return true; }
            public bool op_Rational_Plus_Rational(opaque_Rational a, opaque_Rational b, out opaque_Rational result) 
            {
                try
                {
                    result = new opaque_Rational { num = checked(a.num * b.den + a.den * b.num), den = checked(a.den * b.den) };
                    return true;
                }
                catch(System.Exception)
                { 
                    result = default(opaque_Rational);
                    return false;
                }
            }
            public bool op_Rational_Less_Rational(opaque_Rational a, opaque_Rational b) 
            {
                try
                {
                    return checked(a.num * b.den < a.den * b.num);
                }
                catch(System.Exception)
                {
                    return false;
                }
            }
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
            bool attr_period(out char result);
        }

        public interface icomp_FixedFormatParams
        {
            bool attr_period(out char result);
            bool attr_decimals(out long result);
        }

        public interface icomp_ExponentFormatParams
        {
            bool attr_period(out char result);
        }

        public interface icomp_FormatParams
        {
            bool attr_general(out icomp_GeneralFormatParams result);
            bool attr_fixed(out icomp_FixedFormatParams result);
            bool attr_exponent(out icomp_ExponentFormatParams result);
        }

        public interface imod_Math
        {
            bool op__float_integer(long x, out opaque_Float result);
            bool op__float_Rational(Rational.opaque_Rational x, out opaque_Float result);
            bool op_Float_Plus_Float(opaque_Float a, opaque_Float b, out opaque_Float result);
            bool op_Float_Minus_Float(opaque_Float a, opaque_Float b, out opaque_Float result);
            bool op_Float_Times_Float(opaque_Float a, opaque_Float b, out opaque_Float result);
            bool op_Float_Div_Float(opaque_Float a, opaque_Float b, out opaque_Float result);
            bool op_Float_StarStar_integer(opaque_Float a, long b, out opaque_Float result);
            bool op_Float_StarStar_Float(opaque_Float a, opaque_Float b, out opaque_Float result);
            bool op_Float_Less_Float(opaque_Float a, opaque_Float b);
            bool op_Float_LessEqual_Float(opaque_Float a, opaque_Float b);
            bool op_Float_Equal_Float(opaque_Float a, opaque_Float b);
            bool op_Float_NotEqual_Float(opaque_Float a, opaque_Float b);
            bool op_Float_GreaterEqual_Float(opaque_Float a, opaque_Float b);
            bool op_Float_Greater_Float(opaque_Float a, opaque_Float b);
            bool fattr_sin(opaque_Float a, out opaque_Float result);
            bool fattr_cos(opaque_Float a, out opaque_Float result);
            bool fattr_exp(opaque_Float a, out opaque_Float result);
            bool fattr_log(opaque_Float a, out opaque_Float result);
            bool fattr_tan(opaque_Float a, out opaque_Float result);
            bool fattr_sqrt(opaque_Float a, out opaque_Float result);
            bool attr_Pi(out opaque_Float result);
        }

        public class module_Math : AhaModule, imod_Math
        {
            public bool op__float_integer(long x, out opaque_Float result) { result = new opaque_Float { value = (double)x }; return true; }
            public bool op__float_Rational(Rational.opaque_Rational x, out opaque_Float result) { result = new opaque_Float { value = (double)x.num / (double)x.den }; return true; }
            public bool op_Float_Plus_Float(opaque_Float a, opaque_Float b, out opaque_Float result) { result = new opaque_Float { value = a.value + b.value }; return true; }
            public bool op_Float_Minus_Float(opaque_Float a, opaque_Float b, out opaque_Float result) { result = new opaque_Float { value = a.value - b.value }; return true; }
            public bool op_Float_Times_Float(opaque_Float a, opaque_Float b, out opaque_Float result) { result = new opaque_Float { value = a.value * b.value }; return true; }
            public bool op_Float_Div_Float(opaque_Float a, opaque_Float b, out opaque_Float result) { result = new opaque_Float { value = a.value / b.value }; return true; }
            public bool op_Float_StarStar_integer(opaque_Float a, long b, out opaque_Float result) 
            { 
                try 
                { 
                    result = new opaque_Float { value = System.Math.Pow(a.value, (double)b) }; 
                    return true; 
                } 
                catch (System.Exception) 
                {
                    result = default(opaque_Float);
                    return false;
                } 
            }
            public bool op_Float_StarStar_Float(opaque_Float a, opaque_Float b, out opaque_Float result) 
            { 
                try 
                { 
                    result = new opaque_Float { value = System.Math.Pow(a.value, b.value) }; 
                    return true; 
                } 
                catch (System.Exception) 
                {
                    result = default(opaque_Float);
                    return false;
                } 
            }
            public bool op_Float_Less_Float(opaque_Float a, opaque_Float b) { return a.value < b.value; }
            public bool op_Float_LessEqual_Float(opaque_Float a, opaque_Float b) { return a.value <= b.value; }
            public bool op_Float_Equal_Float(opaque_Float a, opaque_Float b) { return a.value == b.value; }
            public bool op_Float_NotEqual_Float(opaque_Float a, opaque_Float b) { return a.value != b.value; }
            public bool op_Float_GreaterEqual_Float(opaque_Float a, opaque_Float b) { return a.value >= b.value; }
            public bool op_Float_Greater_Float(opaque_Float a, opaque_Float b) { return a.value > b.value; }
            public bool fattr_sin(opaque_Float a, out opaque_Float result) { result = new opaque_Float { value = System.Math.Sin(a.value) }; return true; }
            public bool fattr_cos(opaque_Float a, out opaque_Float result) { result = new opaque_Float { value = System.Math.Cos(a.value) }; return true; }
            public bool fattr_exp(opaque_Float a, out opaque_Float result) { result = new opaque_Float { value = System.Math.Exp(a.value) }; return true; }
            public bool fattr_log(opaque_Float a, out opaque_Float result) { result = new opaque_Float { value = System.Math.Log(a.value) }; return true; }
            public bool fattr_tan(opaque_Float a, out opaque_Float result) { result = new opaque_Float { value = System.Math.Tan(a.value) }; return true; }
            public bool fattr_sqrt(opaque_Float a, out opaque_Float result) { result = new opaque_Float { value = System.Math.Sqrt(a.value) }; return true; }
            public bool attr_Pi(out opaque_Float result) { result = new opaque_Float { value = System.Math.PI }; return true; }
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
            public long ticks;
        }

        public struct opaque_Interval
        {
            public long ticks;
        }

        public interface icomp_DateStruc
        {
            bool attr_year(out long result);
            bool attr_month(out long result);
            bool attr_day(out long result);
        }

        public interface icomp_TimeStruc
        {
            bool attr_hour(out long result);
            bool attr_min(out long result);
            bool attr_sec(out long result);
            bool attr_msec(out long result);
        }

        public interface icomp_TimestampStruc
        {
            bool attr_date(out icomp_DateStruc result);
            bool attr_time(out icomp_TimeStruc result);
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
            bool fattr_DayOfWeek(opaque_Timestamp t, out icomp_DayOfWeek result);
            bool attr_Day(out opaque_Interval result);
            bool attr_Hour(out opaque_Interval result);
            bool attr_Minute(out opaque_Interval result);
            bool attr_Second(out opaque_Interval result);
            bool attr_Millisecond(out opaque_Interval result);
            bool attr_Tick(out opaque_Interval result);
            bool attr_Zero(out opaque_Interval result);
            bool fattr_TimestampHashFunc(opaque_Timestamp t, out long result);
            bool fattr_IntervalHashFunc(opaque_Interval i, out long result);
            bool op_Timestamp_Minus_Timestamp(opaque_Timestamp a, opaque_Timestamp b, out opaque_Interval result);
            bool op_Timestamp_Plus_Interval(opaque_Timestamp a, opaque_Interval b, out opaque_Timestamp result);
            bool op_Timestamp_Minus_Interval(opaque_Timestamp a, opaque_Interval b, out opaque_Timestamp result);
            bool op_Interval_Plus_Interval(opaque_Interval a, opaque_Interval b, out opaque_Interval result);
            bool op_Interval_Minus_Interval(opaque_Interval a, opaque_Interval b, out opaque_Interval result);
            bool op_integer_Times_Interval(long a, opaque_Interval i, out opaque_Interval result);
            bool op_Interval_Div_integer(opaque_Interval i, long a, out opaque_Interval result);
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
            bool op__date_DateStruc(icomp_DateStruc date, out opaque_Timestamp result);
            bool op__time_TimeStruc(icomp_TimeStruc time, out opaque_Interval result);
            bool op__ticks_Interval(opaque_Interval i, out long result);
            bool op__interval_integer(long param_ticks, out opaque_Interval result);
        }

        public class module_Time : AhaModule
        {
            struct comp_DateStruc : icomp_DateStruc
            {
                public DateTime dt;
                public bool attr_year(out long result) { result = dt.Year; return true; }
                public bool attr_month(out long result) { result = dt.Month; return true; }
                public bool attr_day(out long result) { result = dt.Day; return true; }
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

            public bool fattr_DayOfWeek(opaque_Timestamp t, out icomp_DayOfWeek result) { result = new comp_DayOfWeek { value = (new DateTime(t.ticks)).DayOfWeek }; return true; }
            public bool attr_Day(out opaque_Interval result) { result = new opaque_Interval { ticks = (new TimeSpan(1, 0, 0, 0)).Ticks }; return true; }
            public bool attr_Hour(out opaque_Interval result) { result = new opaque_Interval { ticks = 14400000000 }; return true; }
            public bool attr_Minute(out opaque_Interval result) { result = new opaque_Interval { ticks = 600000000 }; return true; }
            public bool attr_Second(out opaque_Interval result) { result = new opaque_Interval { ticks = 10000000 }; return true; }
            public bool attr_Millisecond(out opaque_Interval result) { result = new opaque_Interval { ticks = 10000 }; return true; }
            public bool attr_Tick(out opaque_Interval result) { result = new opaque_Interval { ticks = 1 }; return true; }
            public bool attr_Zero(out opaque_Interval result) { result = new opaque_Interval { ticks = 0 }; return true; }
            public bool fattr_TimestampHashFunc(opaque_Timestamp t, out long result) { result = (new DateTime(t.ticks)).GetHashCode(); return true; }
            public bool fattr_IntervalHashFunc(opaque_Interval i, out long result) { result = (new TimeSpan(i.ticks)).GetHashCode(); return true; }
            public bool op_Timestamp_Minus_Timestamp(opaque_Timestamp a, opaque_Timestamp b, out opaque_Interval result) { result = new opaque_Interval { ticks = a.ticks - b.ticks }; return true; }
            public bool op_Timestamp_Plus_Interval(opaque_Timestamp a, opaque_Interval b, out opaque_Timestamp result) { result = new opaque_Timestamp { ticks = a.ticks + b.ticks }; return true; }
            public bool op_Timestamp_Minus_Interval(opaque_Timestamp a, opaque_Interval b, out opaque_Timestamp result) { result = new opaque_Timestamp { ticks = a.ticks - b.ticks }; return true; }
            public bool op_Interval_Plus_Interval(opaque_Interval a, opaque_Interval b, out opaque_Interval result) { result = new opaque_Interval { ticks = a.ticks + b.ticks }; return true; }
            public bool op_Interval_Minus_Interval(opaque_Interval a, opaque_Interval b, out opaque_Interval result) { result = new opaque_Interval { ticks = a.ticks - b.ticks }; return true; }
            public bool op_integer_Times_Interval(long a, opaque_Interval i, out opaque_Interval result) { result = new opaque_Interval { ticks = a * i.ticks }; return true; }
            public bool op_Interval_Div_integer(opaque_Interval i, long a, out opaque_Interval result) { result = new opaque_Interval { ticks = i.ticks / a }; return true; }
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
            public bool op__date_DateStruc(icomp_DateStruc date, out opaque_Timestamp result) 
            {
                long y;
                long m;
                long d;
                if (date.attr_year(out y) && date.attr_month(out m) && date.attr_day(out d))
                {
                    result = new opaque_Timestamp { ticks = (new DateTime((int)y, (int)m, (int)d)).Ticks };
                    return true;
                }
                else
                {
                    result = default(opaque_Timestamp);
                    return false; 
                }
            }
            public bool op__time_TimeStruc(icomp_TimeStruc time, out opaque_Interval result) 
            {
                long h;
                long m;
                long s;
                long msec;
                if (time.attr_hour(out h) && time.attr_min(out m) && time.attr_sec(out s) && time.attr_msec(out msec))
                {
                    result = new opaque_Interval { ticks = (new TimeSpan(0, (int)h, (int)m, (int)s, (int)msec)).Ticks };
                    return true;
                }
                else
                {
                    result = default(opaque_Interval);
                    return false;
                }
            }
            public bool op__ticks_Interval(opaque_Interval i, out long result) { result = i.ticks; return true; }
            public bool op__interval_integer(long param_ticks, out opaque_Interval result) { result = new opaque_Interval { ticks = param_ticks }; return true; }
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
            bool attr_index(out long result);
            bool attr_length(out long result);
        }

        public interface icomp_Pattern
        {
            bool attr_string(out IahaArray<char> result);
            bool attr_regEx(out opaque_RegEx result);
        }

        public interface icomp_SearchParams
        {
            bool attr_for(out icomp_Pattern result);
            bool attr_in(out IahaArray<char> result);
        }

        public interface icomp_PutParams
        {
            bool attr_at(out long result);
            bool attr_char(out char result);
        }

        public interface icomp_ReplaceParams
        {
            bool attr_substr(out IahaArray<icomp_Substring> result);
            bool attr_with(out IahaArray<char> result);
        }

        public interface icomp_PadParams
        {
            bool attr_with(out char result);
            bool attr_to(out long result);
        }

        public interface imod_StrUtils
        {
            bool fattr_Substr(IahaArray<char> s, icomp_Substring ss, out IahaArray<char> result);
            bool fattr_RegEx(IahaArray<char> s, out opaque_RegEx result);
            bool fattr_Search(icomp_SearchParams param, out IahaSequence<icomp_Substring> result);
            bool fattr_StringHashFunc(IahaArray<char> s, out long result);
        }

        public struct opaque_RegEx
        {
            public System.Text.RegularExpressions.Regex value;
        }

        public class module_StrUtils : AhaModule, imod_StrUtils
        {
            struct comp_Substring : icomp_Substring
            {
                public int field_index;
                public int field_length;
                public bool attr_index(out long result) { result = field_index; return true; }
                public bool attr_length(out long result) { result = field_length; return true; }
                public comp_Substring(int i, int l) { field_index = i; field_length = l; }
            }

            class obj_SearchSeq : IahaSequence<icomp_Substring>
            {
                private string str;
                private string sub;
                private int index;
                public obj_SearchSeq(string s, string ss) { str = s; sub = ss; index = s.IndexOf(ss); }
                public bool state(out icomp_Substring result)
                {
                    if (index >= 0)
                    {
                        result = new comp_Substring(index, sub.Length);
                        return true;
                    }
                    else
                    {
                        result = default(icomp_Substring);
                        return false;
                    }
                }
                public IahaObject<icomp_Substring> copy() { return new obj_SearchSeq(str, sub) { index = index }; }
                public bool action_skip() 
                { 
                    if (index >= 0)
                    {
                        index = str.IndexOf(sub, index + 1); 
                        return index >= 0;
                    }
                    else
                    {
                        return false;
                    }
                }
                public bool first(Predicate<icomp_Substring> that, long max, out icomp_Substring result)
                {
                    int i = index;
                    long j = 0;
                    comp_Substring substr = new comp_Substring(i, sub.Length);
                    while (i != -1 && j < max)
                    {
                        substr.field_index = i;
                        if (that(substr)) { result = substr; return true; }
                        i = str.IndexOf(sub, i + 1);
                        j++;
                    }
                    result = default(icomp_Substring);
                    return false;
                }
            }

            class obj_SearchRegExSeq : IahaSequence<icomp_Substring>
            {
                private string str;
                private System.Text.RegularExpressions.Regex regEx;
                private System.Text.RegularExpressions.Match match;
                public obj_SearchRegExSeq(string s, System.Text.RegularExpressions.Regex r) { str = s; regEx = r; match = regEx.Match(s); }
                public bool state(out icomp_Substring result) 
                {
                    if (match.Success)
                    { 
                        result = new comp_Substring(match.Index, match.Length); 
                        return true; 
                    }
                    else
                    {
                        result = default(icomp_Substring);
                        return false;
                    }
                }
                public IahaObject<icomp_Substring> copy() { return new obj_SearchRegExSeq(str, regEx) { match = match }; }
                public bool action_skip() { match = match.NextMatch(); return match.Success; }
                public bool first(Predicate<icomp_Substring> that, long max, out icomp_Substring result)
                {
                    long j = 0;
                    comp_Substring substr = new comp_Substring(match.Index, match.Length);
                    System.Text.RegularExpressions.Match m = match;
                    while (m.Success && j < max)
                    {
                        substr.field_index = m.Index;
                        substr.field_length = m.Length;
                        if (that(substr)) { result = substr; return true; }
                        m = m.NextMatch();
                        j++;
                    }
                    result = default(icomp_Substring);
                    return false;
                }
            }

            public bool fattr_Substr(IahaArray<char> s, icomp_Substring ss, out IahaArray<char> result) 
            {
                long i; long l;
                if (ss.attr_index(out i) && ss.attr_length(out l))
                {
                    char[] items = new char[l];
                    Array.Copy(s.get(), i, items, 0, l);
                    result = new AhaString(items);
                    return true;
                }
                else
                {
                    result = default(IahaArray<char>);
                    return false;
                }
            }
            public bool fattr_RegEx(IahaArray<char> s, out opaque_RegEx result) 
            {
                try
                {
                    result = new opaque_RegEx { value = new System.Text.RegularExpressions.Regex(new string(s.get())) };
                    return true;
                }
                catch(System.Exception)
                {
                    result = default(opaque_RegEx);
                    return false;
                }
            }
            public bool fattr_Search(icomp_SearchParams param, out IahaSequence<icomp_Substring> result)
            {
                IahaArray<char> source;
                icomp_Pattern p;
                IahaArray<char> sub;
                opaque_RegEx r;
                result = default(IahaSequence<icomp_Substring>);
                if (param.attr_for(out p) && param.attr_in(out source))
                {
                    if (p.attr_string(out sub))
                    {
                        string temp1 = new string(sub.get());
                        string temp2 = new string(source.get());
                        result = new obj_SearchSeq(temp2, temp1);
                        return true;
                    }
                    if (p.attr_regEx(out r))
                    {
                        string temp = new string(source.get());
                        result = new obj_SearchRegExSeq(temp, r.value);
                        return true;
                    }
                }
                return false;
            }
            public bool fattr_StringHashFunc(IahaArray<char> s, out long result) { result = s.get().GetHashCode(); return true; }
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
            public long[] indexes;
        }

        public interface icomp_TreeParam<tpar_Node>
        {
            bool attr_root(out tpar_Node result);
            bool attr_children(out IahaArray<opaque_Tree<tpar_Node>> result);
        }

        public interface imod_Trees<tpar_Node>
        {
            bool fattr_Tree(icomp_TreeParam<tpar_Node> param, out opaque_Tree<tpar_Node> result);
            bool fattr_Leaf(tpar_Node node, out opaque_Tree<tpar_Node> result);
            bool fattr_Root(opaque_Tree<tpar_Node> tree, out tpar_Node result);
            bool fattr_Children(opaque_Tree<tpar_Node> tree, out IahaArray<opaque_Tree<tpar_Node>> result);
            bool fattr_NodesByLevel(opaque_Tree<tpar_Node> tree, out IahaSequence<tpar_Node> result);
            bool fattr_NodesByBranch(opaque_Tree<tpar_Node> tree, out IahaSequence<tpar_Node> result);
            bool fattr_LevelCount(opaque_Tree<tpar_Node> tree, out long result);
            bool fattr_NodeCount(opaque_Tree<tpar_Node> tree, out long result);
            bool op_Tree_at_Path(opaque_Tree<tpar_Node> tree, opaque_Path<tpar_Node> path, out opaque_Tree<tpar_Node> result);
            bool fattr_Indexes(opaque_Path<tpar_Node> path, out IahaSequence<long> result);
            bool fattr_Ancestors(opaque_Tree<tpar_Node> tree, opaque_Path<tpar_Node> path, out IahaSequence<opaque_Tree<tpar_Node>> result);
        }

        public class module_Trees<tpar_Node> : AhaModule, imod_Trees<tpar_Node>
        {
            class obj_NodesByLevel : IahaSequence<tpar_Node>
            {
                private Queue<opaque_Tree<tpar_Node>> queue = new Queue<opaque_Tree<tpar_Node>>();
                public obj_NodesByLevel(opaque_Tree<tpar_Node> tree) { queue.Enqueue(tree); }
                public bool state(out tpar_Node result)
                {
                    if (queue.Count > 0)
                    {
                        result = queue.Peek().root;
                        return true;
                    }
                    else
                    {
                        result = default(tpar_Node);
                        return false;
                    }
                }
                public IahaObject<tpar_Node> copy() { return new obj_NodesByLevel(queue.Peek()); }
                public bool action_skip() 
                {
                    if (queue.Count > 0)
                    {
                        opaque_Tree<tpar_Node> top = queue.Dequeue();
                        if (top.children != null)
                        {
                            foreach (opaque_Tree<tpar_Node> child in top.children)
                                queue.Enqueue(child);
                        }
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                public bool first(Predicate<tpar_Node> that, long max, out tpar_Node result)
                {
                    if (queue.Count > 0 && max > 0)
                    {
                        Queue<opaque_Tree<tpar_Node>> q = new Queue<opaque_Tree<tpar_Node>>();
                        q.Enqueue(queue.Peek());
                        long j = 0;
                        while (q.Count > 0 && j < max)
                        {
                            opaque_Tree<tpar_Node> top = q.Dequeue();
                            if (that(top.root)) { result = top.root; return true; }
                            if (top.children != null)
                            {
                                foreach (opaque_Tree<tpar_Node> child in top.children)
                                    queue.Enqueue(child);
                            }
                            j++;
                        }
                    }
                    result = default(tpar_Node);
                    return false;
                }
            }

            class obj_NodesByBranch : IahaSequence<tpar_Node>
            {
                private Stack<opaque_Tree<tpar_Node>> stack = new Stack<opaque_Tree<tpar_Node>>();
                public obj_NodesByBranch(opaque_Tree<tpar_Node> tree) { stack.Push(tree); }
                public bool state(out tpar_Node result) 
                { 
                    if (stack.Count > 0) 
                    { 
                        result = stack.Peek().root; 
                        return true; 
                    } 
                    else 
                    {
                        result = default(tpar_Node);
                        return false;
                    } 
                }
                public IahaObject<tpar_Node> copy() { return new obj_NodesByBranch(stack.Peek()); }
                public bool action_skip()
                {
                    if (stack.Count > 0)
                    {
                        opaque_Tree<tpar_Node> top = stack.Pop();
                        if (top.children != null)
                        {
                            for (int i = top.children.Length - 1; i >= 0; i--)
                                stack.Push(top.children[i]);
                        }
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                public bool first(Predicate<tpar_Node> that, long max, out tpar_Node result)
                {
                    if (stack.Count > 0 && max > 0)
                    {
                        Stack<opaque_Tree<tpar_Node>> q = new Stack<opaque_Tree<tpar_Node>>();
                        q.Push(stack.Peek());
                        long j = 0;
                        while (q.Count > 0 && j < max)
                        {
                            opaque_Tree<tpar_Node> top = q.Pop();
                            if (that(top.root)) { result = top.root; return true; }
                            if (top.children != null)
                            {
                                for (int i = top.children.Length - 1; i >= 0; i--)
                                    stack.Push(top.children[i]);
                            }
                            j++;
                        }
                    }
                    result = default(tpar_Node);
                    return false;
                }
            }

            class obj_PathsByLevel : IahaSequence<opaque_Path<tpar_Node>>
            {
                struct Subtree
                {
                    public opaque_Tree<tpar_Node> tree;
                    public long[] indexes;
                }

                private Queue<Subtree> queue = new Queue<Subtree>();
                public obj_PathsByLevel(opaque_Tree<tpar_Node> tree) { queue.Enqueue(new Subtree { tree = tree, indexes = new long[0] }); }
                public bool state(out opaque_Path<tpar_Node> result) { result = new opaque_Path<tpar_Node> { indexes = queue.Peek().indexes }; return true; }
                public IahaObject<opaque_Path<tpar_Node>> copy() { return new obj_PathsByLevel(queue.Peek().tree); }
                public bool action_skip()
                {
                    if (queue.Count > 0)
                    {
                        Subtree top = queue.Dequeue();
                        if (top.tree.children != null)
                        {
                            for (int i = 0; i < top.tree.children.Length; i++)
                            {
                                long[] indexes = new long[top.indexes.Length + 1];
                                for (int j = 0; j < top.indexes.Length; j++)
                                    indexes[j] = top.indexes[j];
                                indexes[top.indexes.Length] = i;
                                queue.Enqueue(new Subtree { tree = top.tree.children[i], indexes = indexes });
                            }
                        }
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                public bool first(Predicate<opaque_Path<tpar_Node>> that, long max, out opaque_Path<tpar_Node> result)
                {
                    if (queue.Count > 0 && max > 0)
                    {
                        Queue<Subtree> q = new Queue<Subtree>();
                        q.Enqueue(queue.Peek());
                        long k = 0;
                        while (q.Count > 0 && k < max)
                        {
                            Subtree top = q.Dequeue();
                            opaque_Path<tpar_Node> path = new opaque_Path<tpar_Node> { indexes = top.indexes };
                            if (that(path)) { result = path; return true; }
                            if (top.tree.children != null)
                            {
                                for (int i = top.tree.children.Length - 1; i >= 0; i--)
                                {
                                    long[] indexes = new long[top.indexes.Length + 1];
                                    for (int j = 0; j < top.indexes.Length; j++)
                                        indexes[j] = top.indexes[j];
                                    indexes[top.indexes.Length] = i;
                                    q.Enqueue(new Subtree { tree = top.tree.children[i], indexes = indexes });
                                }
                            }
                            k++;
                        }
                    }
                    result = default(opaque_Path<tpar_Node>);
                    return false;
                }
            }

            class obj_PathsByBranch : IahaSequence<opaque_Path<tpar_Node>>
            {
                struct Subtree
                {
                    public opaque_Tree<tpar_Node> tree;
                    public long[] indexes;
                }

                private Stack<Subtree> stack = new Stack<Subtree>();
                public obj_PathsByBranch(opaque_Tree<tpar_Node> tree) { stack.Push(new Subtree { tree = tree, indexes = new long[0] }); }
                public bool state(out opaque_Path<tpar_Node> result) { result = new opaque_Path<tpar_Node> { indexes = stack.Peek().indexes }; return true; }
                public IahaObject<opaque_Path<tpar_Node>> copy() { return new obj_PathsByBranch(stack.Peek().tree); }
                public bool action_skip()
                {
                    if (stack.Count > 0)
                    {
                        Subtree top = stack.Pop();
                        if (top.tree.children != null)
                        {
                            for (int i = top.tree.children.Length - 1; i >= 0; i--)
                            {
                                long[] indexes = new long[top.indexes.Length + 1];
                                for (int j = 0; j < top.indexes.Length; j++)
                                    indexes[j] = top.indexes[j];
                                indexes[top.indexes.Length] = i;
                                stack.Push(new Subtree { tree = top.tree.children[i], indexes = indexes });
                            }
                        }
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                public bool first(Predicate<opaque_Path<tpar_Node>> that, long max, out opaque_Path<tpar_Node> result)
                {
                    if (stack.Count > 0 && max > 0)
                    {
                        Stack<Subtree> q = new Stack<Subtree>();
                        q.Push(stack.Peek());
                        long k = 0;
                        while (q.Count > 0 && k < max)
                        {
                            Subtree top = q.Pop();
                            opaque_Path<tpar_Node> path = new opaque_Path<tpar_Node> { indexes = top.indexes };
                            if (that(path)) { result = path; return true; }
                            if (top.tree.children != null)
                            {
                                for (int i = top.tree.children.Length - 1; i >= 0; i--)
                                {
                                    long[] indexes = new long[top.indexes.Length + 1];
                                    for (int j = 0; j < top.indexes.Length; j++)
                                        indexes[j] = top.indexes[j];
                                    indexes[top.indexes.Length] = i;
                                    q.Push(new Subtree { tree = top.tree.children[i], indexes = indexes });
                                }
                            }
                            k++;
                        }
                    }
                    result = default(opaque_Path<tpar_Node>);
                    return false;
                }
            }

            public bool fattr_Tree(icomp_TreeParam<tpar_Node> param, out opaque_Tree<tpar_Node> result)
            {
                int levels = 0;
                IahaArray<opaque_Tree<tpar_Node>> children;
                tpar_Node root;
                if (param.attr_children(out children) && param.attr_root(out root))
                {
                    foreach (opaque_Tree<tpar_Node> child in children.get())
                        if (child.levels > levels) levels = child.levels;
                    result = new opaque_Tree<tpar_Node>() { root = root, children = children.get(), levels = levels + 1 };
                    return true;
                }
                else
                {
                    result = default(opaque_Tree<tpar_Node>);
                    return false;
                }
            }
            public bool fattr_Leaf(tpar_Node node, out opaque_Tree<tpar_Node> result) { result = new opaque_Tree<tpar_Node>() { root = node, children = null, levels = 0 }; return true; }
            public bool fattr_Root(opaque_Tree<tpar_Node> tree, out tpar_Node result) { result = tree.root; return true; }
            public bool fattr_Children(opaque_Tree<tpar_Node> tree, out IahaArray<opaque_Tree<tpar_Node>> result) 
            { 
                if (tree.children == null) 
                    result = new AhaArray<opaque_Tree<tpar_Node>>(new opaque_Tree<tpar_Node>[] { }); 
                else
                    result = new AhaArray<opaque_Tree<tpar_Node>>(tree.children);
                return true;
            }
            public bool fattr_NodesByLevel(opaque_Tree<tpar_Node> tree, out IahaSequence<tpar_Node> result) { result = new obj_NodesByLevel(tree); return true; }
            public bool fattr_NodesByBranch(opaque_Tree<tpar_Node> tree, out IahaSequence<tpar_Node> result) { result = new obj_NodesByBranch(tree); return true; }
            public bool fattr_LevelCount(opaque_Tree<tpar_Node> tree, out long result) { result = tree.levels; return true; }
            public bool fattr_NodeCount(opaque_Tree<tpar_Node> tree, out long result)
            {
                result = 0;
                Stack<opaque_Tree<tpar_Node>> stack = new Stack<opaque_Tree<tpar_Node>>();
                opaque_Tree<tpar_Node> subtree;
                stack.Push(tree);
                while (stack.Count > 0)
                {
                    subtree = stack.Pop();
                    if (subtree.children != null)
                    {
                        result += subtree.children.Length;
                        foreach (opaque_Tree<tpar_Node> child in subtree.children)
                            stack.Push(child);
                    }
                }
                return true;
            }
            public bool op_Tree_at_Path(opaque_Tree<tpar_Node> tree, opaque_Path<tpar_Node> path, out opaque_Tree<tpar_Node> result)
            {
                result = tree;
                int i = 0;
                while (i < path.indexes.Length)
                {
                    if (path.indexes[i] >= result.children.Length)
                        return false;
                    result = result.children[path.indexes[i]];
                    i++;
                }
                return true;
            }
            public bool fattr_Indexes(opaque_Path<tpar_Node> path, out IahaSequence<long> result) 
            { 
                result = new AhaArraySeq<long> { items = path.indexes, index = 0 };
                return true;
            }
            public bool fattr_Ancestors(opaque_Tree<tpar_Node> tree, opaque_Path<tpar_Node> path, out IahaSequence<opaque_Tree<tpar_Node>> result)
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
                result = new AhaArraySeq<opaque_Tree<tpar_Node>> { items = items, index = 0 };
                return true;
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
            public long bits;
            public Byte[] bytes;
        }

        public interface iobj_DynamicBitString : IahaObject<opaque_BitString>
        {
            bool action_append(opaque_BitString param_str);
            bool action_resize(long index);
            bool action_set(long index);
            bool action_reset(long index);
        }

        public interface icomp_Substring
        {
            bool attr_index(out long result);
            bool attr_length(out long result);
        }

        public interface imod_Bits
        {
            bool fattr_Substr(opaque_BitString str, icomp_Substring sub, out opaque_BitString result);
            bool attr_True(out opaque_BitString result);
            bool attr_False(out opaque_BitString result);
            bool attr_Nil(out opaque_BitString result);
            bool op_BitString_Equal_BitString(opaque_BitString first, opaque_BitString second, out opaque_BitString result);
            bool op_BitString_And_BitString(opaque_BitString first, opaque_BitString second, out opaque_BitString result);
            bool op_BitString_Or_BitString(opaque_BitString first, opaque_BitString second, out opaque_BitString result);
            bool op_BitString_Xor_BitString(opaque_BitString first, opaque_BitString second, out opaque_BitString result);
        }
    }

}
