//
// Package: Aha! Base Library 
// Author: Roman Movchan
// Created: 2014-04-01

using System;
using System.Collections.Generic;
using AhaCore;
using Collections;

namespace BaseLibrary
{
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
    public class module_Collections<Item> : AhaModule
    {
        public iobj_DynamicArray<Item> attr_DynamicArray() { return new Collections.obj_DynamicArray<Item>(); }
        public iobj_DynamicSequence<Item> attr_Stack() { return new Collections.obj_Stack<Item>(); }
        public iobj_DynamicSequence<Item> attr_Queue() { return new Collections.obj_Queue<Item>(); }
    }

//doc
//    Title:   "Rationals"
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
    public class module_Rational : AhaModule
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
    public class module_Math : AhaModule
    {
        public struct opaque_Float
        {
            public double value;
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

        public class opaque_Matrix
        {
            public double[,] value;
        }

        public opaque_Float op__float_integer(Int64 x) { return new opaque_Float { value = x }; }
        public opaque_Float op__float_Rational(module_Rational.opaque_Rational x) { return new opaque_Float { value = x.num / x.den }; }
        public opaque_Float op_Float_Plus_Float(opaque_Float a, opaque_Float b) { return new opaque_Float { value = a.value + b.value }; }
        public bool op_Float_Less_Float(opaque_Float a, opaque_Float b) { return a.value < b.value; }
        public opaque_Float fattr_sin(opaque_Float a) { return new opaque_Float { value = System.Math.Sin(a.value) }; }
    }

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

    public class module_Time : AhaModule
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

        struct comp_DateStruc : icomp_DateStruc
        {
            public DateTime dt;
            public Int64 attr_year() { return dt.Year; }
            public Int64 attr_month() { return dt.Month; }
            public Int64 attr_day() { return dt.Day; }
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

        struct comp_DayOfWeek : icomp_DayOfWeek
        {
            public System.DayOfWeek value;
            public bool attr_Monday() { return value == System.DayOfWeek.Monday;  }
            public bool attr_Tuesday() { return value == System.DayOfWeek.Tuesday; }
            public bool attr_Wednesday() { return value == System.DayOfWeek.Wednesday; }
            public bool attr_Thursday() { return value == System.DayOfWeek.Thursday; }
            public bool attr_Friday() { return value == System.DayOfWeek.Friday; }
            public bool attr_Saturday() { return value == System.DayOfWeek.Saturday; }
            public bool attr_Sunday() { return value == System.DayOfWeek.Sunday; }
        }

        public icomp_DayOfWeek fattr_DayOfWeek(opaque_Timestamp dt) { return new comp_DayOfWeek { value = (new DateTime(dt.ticks)).DayOfWeek }; }

        public opaque_Interval attr_Day() { return new opaque_Interval { ticks = (new TimeSpan(1, 0, 0, 0)).Ticks }; }

        public opaque_Interval attr_Hour() { return new opaque_Interval { ticks = 14400000000 }; }

        public opaque_Interval attr_Minute() { return new opaque_Interval { ticks = 600000000 }; }

        public opaque_Interval attr_Second() { return new opaque_Interval { ticks = 10000000 }; }

        public opaque_Interval attr_Millisecond() { return new opaque_Interval { ticks = 10000 }; }

        public opaque_Interval attr_Tick() { return new opaque_Interval { ticks = 1 }; }

        public opaque_Interval attr_Zero() { return new opaque_Interval { ticks = 0 }; }

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
        public opaque_Interval op__interval_integer(Int64 t) { return new opaque_Interval { ticks = t }; }
    }
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
    
    public class module_StrUtils : AhaModule
    {
        public interface icomp_Substring
        {
            Int64 attr_index();
            Int64 attr_length();
        }

        public struct opaque_RegEx
        {
            public string value;
        }

        public delegate bool func_Equality(char a, char b);

        public interface icomp_Pattern
        {
            IahaArray<char> attr_string();
            opaque_RegEx attr_regEx();
            func_Equality attr_equality();
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

        public delegate char func_Convert(char ch);

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
            //void action_apply(Convert conv);
        }

        class obj_SearchSeq : IahaSequence<icomp_Substring>
        {
            struct substring : icomp_Substring
            {
                public int idx;
                private int len;
                public Int64 attr_index() { return idx; }
                public Int64 attr_length() { return len; }
                public substring(int i, int l) { idx = i; len = l; }
            }

            private string str;
            private string sub;
            private int index;
            public obj_SearchSeq(string s, string ss) { str = s; sub = ss; index = s.IndexOf(ss); }
            public icomp_Substring state() { return new substring(index, sub.Length); }
            public IahaObject<icomp_Substring> copy() { return new obj_SearchSeq(str, sub) { index = index }; }
            public void action_skip() { index = str.IndexOf(sub, index + 1); if (index == -1) throw Failure.One; }
            public icomp_Substring first(Predicate<icomp_Substring> that, Int64 max) { Int64 j = 0; substring substr = new substring(index, sub.Length); while (index != -1) { if (j == max) break; substr.idx = index; if (that(substr)) return substr; index = str.IndexOf(sub, index + 1); j++; } throw Failure.One; }
        }

        class fastBuilder : iobj_StringBuilder
        {
            const int block = 1024;
            private List<char[]> list;
            private int count = 0;
            private char[] gather()
            {
                char[] buf = new char[count];
                int j = 0;
                for (int i = 0; i < list.Count - 1; i++) { Array.Copy(list[i], 0, buf, j, block); j += block; }
                if (j != count) Array.Copy(list[list.Count - 1], 0, buf, j, count - j);
                return buf;
            }
            private void split(char[] buf, int index, int length)
            {
                list = new List<char[]>();
                int j = index; //from position
                count = length;
                char[] b;
                for (int i = 0; i < length / block; i++) { b = new char[block]; Array.Copy(buf, j, b, 0, block); j += block; list.Add(b); }
                if (j != index + length) { b = new char[block]; Array.Copy(buf, j, b, 0, index + length - j); list.Add(b); }
            }
            public fastBuilder() { list = new List<char[]>(); }
            public IahaArray<char> state() { return new AhaString(gather()); }
            public IahaObject<IahaArray<char>> copy() { fastBuilder fb = new fastBuilder() { count = count }; for (int i = 0; i < count; i++) { char[] b = new char[block]; list[i].CopyTo(b, 0); fb.list.Add(b); } return fb; }
            public void action_add(char ch) { if (count == list.Count * block) list.Add(new char[block]); list[count / block][count % block] = ch; }
            public void action_append(IahaArray<char> str) 
            { 
                char[] temp = str.get();
                int j = list.Count * block - count; //positions in last block
                if (j > temp.Length) j = temp.Length; //characters to copy into last block
                if (j != 0) Array.Copy(temp, 0, list[list.Count - 1], count % block, j);
                char[] b;
                while (j + block < temp.Length)
                {
                    b = new char[block];
                    Array.Copy(temp, j, b, 0, block);
                    list.Add(b); 
                    j += block; // j = number of chars copied
                }
                if (j != temp.Length)
                {
                    b = new char[block];
                    Array.Copy(temp, j, b, 0, temp.Length - j); //copy remaining chars
                    list.Add(b);
                }
            }
            public void action_extract(icomp_Substring sub)
            {
                char[] buf = gather();
                split(buf, (int)sub.attr_index(), (int)sub.attr_length());
            }
            public void action_trimSpaces()
            {
                char[] buf = gather();
                int i = 0;
                while ((i < count) && (buf[i] == ' ')) i++; //count leading spaces
                int l = count - i;
                if (i < count) //not all spaces?
                {
                    while (buf[i + l - 1] == ' ') l--; //exclude trailing spaces
                }
                if (l != 0) split(buf, i, l); else list = new List<char[]>();
            }
            public void action_put(icomp_PutParams param) { int at = (int)param.attr_at(); char ch = param.attr_char(); list[at / block][at % block] = ch; }
            public void action_replace(icomp_ReplaceParams param)
            {
                char[] source = gather();
                char[] with = param.attr_with().get();
                IahaArray<icomp_Substring> sub = param.attr_substr();
                int j = 0;
                IahaSequence<icomp_Substring> seq = sub.sort(delegate(icomp_Substring x, icomp_Substring y) { return x.attr_index() <= y.attr_index(); });
                for (int i = 0; i < sub.size(); i++) j += (int)seq.state().attr_length() - with.Length;
                char[] target = new char[source.Length + j];
                int from = 0;
                int to = 0;
                for (int i = 0; i < sub.size(); i++)
                {
                    Array.Copy(source, from, target, to, seq.state().attr_index());
                }
            }
            //void padL(IPadParams param);
            //void padR(IPadParams param);
            //void apply(Convert conv);
        }

        public IahaArray<char> Substr(IahaArray<char> s, icomp_Substring ss) { char[] items = new char[ss.attr_length()]; Array.Copy(s.get(), ss.attr_index(), items, 0, ss.attr_length()); return new AhaString(items); }
        public opaque_RegEx RegEx_(IahaArray<char> s) { return new opaque_RegEx { value = new string(s.get()) }; }
        public IahaSequence<icomp_Substring> Search(icomp_SearchParams param)
        {
            IahaArray<char> sub = param.attr_for().attr_string();
            string temp1 = new string(sub.get());
            string temp2 = new string(param.attr_in().get());
            return new obj_SearchSeq(temp2, temp1);
        }
        public iobj_StringBuilder StringBuilder() { return new fastBuilder(); }
        public Int64 StringHashFunc(IahaArray<char> s) { return s.get().GetHashCode(); }
    }

}
