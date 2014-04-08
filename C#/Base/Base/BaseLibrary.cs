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
    public class Collections<Item> : AhaModule
    {
        public IDynamicArray<Item> DynamicArray() { return new Collections.DynamicArray<Item>(); }
        public IDynamicSequence<Item> Stack() { return new Collections.Stack<Item>(); }
        public IDynamicSequence<Item> Queue() { return new Collections.Queue<Item>(); }
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
    public struct Rational
    {
        public Int64 num;
        public Int64 den;
    }

    public interface IRatioStruc
    {
        Int64 num();
        Int64 den();
    }

    public class Rationals : AhaModule
    {
        class RatioStruc : IRatioStruc
        {
            private Int64 fnum;
            private Int64 fden;
            public RatioStruc(Int64 n, Int64 d) { fnum = n; fden = d; }
            public Int64 num() { return fnum; }
            public Int64 den() { return fden; }
        }

        public Rational Ratio(Int64 num, Int64 den) { return new Rational { num = num, den = den }; }
        public IRatioStruc Struc(Rational x) { return new RatioStruc(x.num, x.den); }
        public Rational RationalSum(Rational a, Rational b) { return new Rational { num = a.num * b.den + a.den * b.num, den = a.den * b.den }; }
        public bool RationalLess(Rational a, Rational b) { return a.num * b.den < a.den * b.num; }
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
    public struct Float
    {
        public double value;
    }

    public interface IGeneralFormatParams
    {
        char period();
    }

    public interface IFixedFormatParams
    {
        char period();
        Int64 decimals();
    }

    public interface IExponentFormatParams
    {
        char period();
    }

    public interface FormatParams
    {
        IGeneralFormatParams general();
        IFixedFormatParams fixed_();
        IExponentFormatParams exponent();
    }

    public class Matrix
    {
        public double[,] value;
    }

    public class Math : AhaModule
    {
        public Float FloatFromInt(Int64 x) { return new Float { value = x }; }
        public Float FloatFromRational(Rational x) { return new Float { value = x.num / x.den }; }
        public Float FloatSum(Float a, Float b) { return new Float { value = a.value + b.value }; }
        public bool FloatLess(Float a, Float b) { return a.value < b.value; }
        public Float sin(Float a) { return new Float { value = System.Math.Sin(a.value) }; }
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
//    the Zero: Interval "zero length interval"
//    the TimestampCompare: { Timestamp, Timestamp -> integer } "negative - before, positive - after, zero - same time"
//    the IntervalCompare: { Interval, Interval -> integer } "negative - shorter, positive - longer, zero - same length"
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
//end

    public class Time : AhaModule
    {
        public struct Timestamp
        { 
            public Int64 ticks; 
        }

        public struct Interval
        {
            public Int64 ticks;
        }

        public interface IDateStruc
        { 
            Int64 year();
            Int64 month();
            Int64 day();
        }

        struct DateStruc : IDateStruc
        {
            public DateTime dt;
            public Int64 year() { return dt.Year; }
            public Int64 month() { return dt.Month; }
            public Int64 day() { return dt.Day; }
        }

        public interface ITimeStruc
        {
            Int64 hour();
            Int64 min();
            Int64 sec();
        }

        public interface ITimestampStruc
        {
            IDateStruc date();
            ITimeStruc time();
        }

        public interface IDayOfWeek
        { 
            bool monday();
            bool tuesday();
            bool wednesday();
            bool thursday();
            bool friday();
            bool saturday();
            bool sunday();
        }

        struct DayOfWeekStr : IDayOfWeek
        {
            public System.DayOfWeek value;
            public bool monday() { return value == System.DayOfWeek.Monday;  }
            public bool tuesday() { return value == System.DayOfWeek.Tuesday; }
            public bool wednesday() { return value == System.DayOfWeek.Wednesday; }
            public bool thursday() { return value == System.DayOfWeek.Thursday; }
            public bool friday() { return value == System.DayOfWeek.Friday; }
            public bool saturday() { return value == System.DayOfWeek.Saturday; }
            public bool sunday() { return value == System.DayOfWeek.Sunday; }
        }

        public IDayOfWeek DayOfWeek(Timestamp dt) { return new DayOfWeekStr { value = (new DateTime(dt.ticks)).DayOfWeek }; }

        public Interval Hour() { return new Interval { ticks = (new TimeSpan(1, 0, 0)).Ticks }; }

        public Interval Minute() { return new Interval { ticks = (new TimeSpan(0, 1, 0)).Ticks }; }

        public Interval Second() { return new Interval { ticks = (new TimeSpan(0, 0, 1)).Ticks }; }

        public Interval Millisecond() { return new Interval { ticks = (new TimeSpan(0, 0, 0, 0, 1)).Ticks }; }

        public Interval Zero() { return new Interval { ticks = 0 }; }
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
    
    public interface ISubstring
    {
        Int64 index();
        Int64 length();
    }

    public struct RegEx
    {
        public string value;
    }

    public delegate bool Equality(char a, char b);

    public interface IPattern
    {
        IahaArray<char> string_();
        RegEx regEx();
        Equality equality();
    }

    public interface ISearchParams
    {
        IPattern for_();
        IahaArray<char> in_();
    }

    public interface IPutParams
    {
        Int64 at();
        char char_();
    }

    public interface IReplaceParams
    {
        IahaArray<ISubstring> substr();
        IahaArray<char> with();
    }

    public interface IPadParams
    {
        char with();
        Int64 to();
    }

    public delegate char Convert(char ch);

    public interface IStringBuilder : IahaObject<IahaArray<char>>
    {
        void add(char ch);
        void put(IPutParams param);
        void append(IahaArray<char> str);
        void replace(IReplaceParams param);
        void extract(ISubstring sub);
        //void padL(IPadParams param);
        //void padR(IPadParams param);
        void trimSpaces();
        //void apply(Convert conv);
    }

    public class StrUtils : AhaModule
    {
        class SearchSeq : IahaSequence<ISubstring>
        {
            struct substring : ISubstring
            {
                public int idx;
                private int len;
                public Int64 index() { return idx; }
                public Int64 length() { return len; }
                public substring(int i, int l) { idx = i; len = l; }
            }

            private string str;
            private string sub;
            private int index;
            public SearchSeq(string s, string ss) { str = s; sub = ss; index = s.IndexOf(ss); }
            public ISubstring state() { return new substring(index, sub.Length); }
            public IahaObject<ISubstring> copy() { return new SearchSeq(str, sub) { index = index }; }
            public void skip() { index = str.IndexOf(sub, index + 1); if (index == -1) throw Failure.One; }
            public ISubstring first(Predicate<ISubstring> that, Int64 max) { Int64 j = 0; substring substr = new substring(index, sub.Length); while (index != -1) { if (j == max) break; substr.idx = index; if (that(substr)) return substr; index = str.IndexOf(sub, index + 1); j++; } throw Failure.One; }
        }

        class fastBuilder : IStringBuilder
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
            public void add(char ch) { if (count == list.Count * block) list.Add(new char[block]); list[count / block][count % block] = ch; }
            public void append(IahaArray<char> str) 
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
            public void extract(ISubstring sub)
            {
                char[] buf = gather();
                split(buf, (int)sub.index(), (int)sub.length());
            }
            public void trimSpaces()
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
            public void put(IPutParams param) { int at = (int)param.at(); char ch = param.char_(); list[at / block][at % block] = ch; }
            public void replace(IReplaceParams param)
            {
                char[] source = gather();
                char[] with = param.with().get();
                IahaArray<ISubstring> sub = param.substr();
                int j = 0;
                IahaSequence<ISubstring> seq = sub.sort(delegate(ISubstring x, ISubstring y) { return x.index() <= y.index(); });
                for (int i = 0; i < sub.size(); i++) j += (int)seq.state().length() - with.Length;
                char[] target = new char[source.Length + j];
                int from = 0;
                int to = 0;
                for (int i = 0; i < sub.size(); i++)
                {
                    Array.Copy(source, from, target, to, seq.state().index());
                }
            }
            //void padL(IPadParams param);
            //void padR(IPadParams param);
            //void apply(Convert conv);
        }

        public IahaArray<char> Substr(IahaArray<char> s, ISubstring ss) { char[] items = new char[ss.length()]; Array.Copy(s.get(), ss.index(), items, 0, ss.length()); return new AhaString(items); }
        public RegEx RegEx(IahaArray<char> s) { return new RegEx { value = new string(s.get()) }; }
        public IahaSequence<ISubstring> Search(ISearchParams param)
        {
            IahaArray<char> sub = param.for_().string_();
            string temp1 = new string(sub.get());
            string temp2 = new string(param.in_().get());
            return new SearchSeq(temp2, temp1);
        }
        public IStringBuilder StringBuilder() { return new fastBuilder(); }
        public Int64 StringHashFunc(IahaArray<char> s) { return s.get().GetHashCode(); }
    }

}
