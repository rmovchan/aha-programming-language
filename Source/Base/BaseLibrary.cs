//
// Package: Aha! Base Library 
// Author: Roman Movchan
// Created: 2014-04-01

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Aha.Core;

namespace Aha.Package.Base
{
    namespace Collections
    {
        public struct com_ReplaceParam<Item>
        {
            public long attr_index;
            public Item attr_item;
        }

        public struct com_InsertParam<Item>
        {
            public long attr_index;
            public Item attr_item;
        }

        public struct com_SwapParam<Item>
        {
            public long attr_first;
            public long attr_second;
        }

        public struct com_MoveParam<Item>
        {
            public long attr_from;
            public long attr_to;
        }

        public interface iobj_DynamicArray<Item> : IahaObject<IahaArray<Item>>
        {
            bool action_add(Item item);
            bool action_replace(com_ReplaceParam<Item> param);
            bool action_insert(com_InsertParam<Item> param);
            bool action_swap(com_SwapParam<Item> param);
            bool action_move(com_MoveParam<Item> param);
            bool action_delete(long index);
        }

        public interface iobj_DynamicSequence<Item> : IahaSequence<Item>
        {
            bool action_add(Item item);
        }

        public interface imod_Collections<Item>
        {
            bool attr_DynamicArray(out iobj_DynamicArray<Item> result);
            bool fattr_DynamicArrayOf(IahaArray<Item> items, out iobj_DynamicArray<Item> result);
            bool attr_Stack(out iobj_DynamicSequence<Item> result);
            bool attr_Queue(out iobj_DynamicSequence<Item> result);
        }

        public class module_Collections<Item> : AhaModule, imod_Collections<Item>
        {
            class obj_DynamicArray : iobj_DynamicArray<Item>
            {
                private List<Item> list;
                public obj_DynamicArray() { list = new List<Item>(); }
                public obj_DynamicArray(Item[] items) { list = new List<Item>(items); }
                public bool state(out IahaArray<Item> result)
                {
                    result = new AhaArray<Item>(list.ToArray());
                    return true;
                }
                public IahaObject<IahaArray<Item>> copy() { obj_DynamicArray clone = new obj_DynamicArray(list.ToArray()); return clone; }
                public bool action_add(Item item)
                {
                    list.Add(item);
                    return true;
                }
                public bool action_replace(com_ReplaceParam<Item> param)
                {
                    if (param.attr_index >= 0 && param.attr_index < list.Count)
                    {
                        list[(int)param.attr_index] = param.attr_item;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                public bool action_insert(com_InsertParam<Item> param)
                {
                    if (param.attr_index >= 0 && param.attr_index <= list.Count)
                    {
                        list.Insert((int)param.attr_index, param.attr_item);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                public bool action_swap(com_SwapParam<Item> param)
                {
                    if (param.attr_first >= 0 && param.attr_first < list.Count && param.attr_second >= 0 && param.attr_second < list.Count)
                    {
                        Item item = list[(int)param.attr_first];
                        list[(int)param.attr_first] = list[(int)param.attr_second];
                        list[(int)param.attr_second] = item;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                public bool action_move(com_MoveParam<Item> param)
                {
                    if (param.attr_from >= 0 && param.attr_from < list.Count && param.attr_to >= 0 && param.attr_to < list.Count)
                    {
                        if (param.attr_from != param.attr_to)
                        {
                            Item[] items = list.ToArray();
                            Item item = list[(int)param.attr_from]; //from
                            //shift items:
                            if (param.attr_from < param.attr_to)
                                list.CopyTo((int)param.attr_from + 1, items, (int)param.attr_from, (int)(param.attr_to - param.attr_from)); //down
                            else
                                list.CopyTo((int)param.attr_from, items, (int)param.attr_from + 1, (int)(param.attr_from - param.attr_to)); //up

                            list = new List<Item>(items);
                            list[(int)param.attr_to] = item; //to
                        }
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                public bool action_delete(long index)
                {
                    if (index >= 0 && index <= list.Count)
                    {
                        list.RemoveAt((int)index);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            class obj_Stack : iobj_DynamicSequence<Item>
            {
                class node
                {
                    public Item item;
                    public node next;
                }
                private node head = null;
                public bool state(out Item result)
                {
                    if (head != null)
                    {
                        result = head.item;
                        return true;
                    }
                    else
                    {
                        result = default(Item);
                        return false;
                    }
                }
                public IahaObject<Item> copy() { obj_Stack clone = new obj_Stack() { head = head }; return clone; }
                public bool action_add(Item item)
                {
                    node h = new node();
                    h.item = item;
                    h.next = head;
                    head = h;
                    return true;
                }
                public bool action_skip()
                {
                    if (head != null)
                    {
                        head = head.next;
                        return true;
                    }
                    else
                        return false;
                }
                public bool first(Predicate<Item> that, long max, out Item result)
                {
                    if (head != null)
                    {
                        node h = head;
                        for (long j = 0; j < max; j++)
                        {
                            if (that(h.item))
                            {
                                result = h.item;
                                return true;
                            }
                            h = h.next;
                            if (h == null) break;
                        }
                    }
                    result = default(Item);
                    return false;
                }
            }

            class obj_Queue : iobj_DynamicSequence<Item>
            {
                class node
                {
                    public Item item;
                    public node next;
                    public node prev;
                }
                private node head = null;
                private node tail = null;
                public bool state(out Item result)
                {
                    if (tail != null)
                    {
                        result = tail.item;
                        return true;
                    }
                    else
                    {
                        result = default(Item);
                        return false;
                    }
                }
                public IahaObject<Item> copy() { obj_Queue clone = new obj_Queue(); node p = head; while (p != null) { clone.action_add(p.item); p = p.next; } return clone; }
                public bool action_add(Item item)
                {
                    node h = new node();
                    h.item = item;
                    h.next = head;
                    h.prev = null;
                    head = h;
                    if (tail == null)
                        tail = h;
                    return true;
                }
                public bool action_skip()
                {
                    if (tail != null)
                    {
                        tail = tail.prev;
                        tail.prev = tail.prev.prev;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                public bool first(Predicate<Item> that, long max, out Item result)
                {
                    if (tail != null)
                    {
                        node t = tail;
                        for (long j = 0; j < max; j++)
                        {
                            if (that(t.item))
                            {
                                result = t.item;
                                return true;
                            }
                            t = t.prev;
                            if (t == null) break;
                        }
                    }
                    result = default(Item);
                    return false;
                }
            }

            public bool attr_DynamicArray(out iobj_DynamicArray<Item> result) { try { result = new obj_DynamicArray(); return true; } catch (System.Exception) { result = default(iobj_DynamicArray<Item>); return false; } }
            public bool fattr_DynamicArrayOf(IahaArray<Item> items, out iobj_DynamicArray<Item> result) { try { result = new obj_DynamicArray(items.get()); return true; } catch (System.Exception) { result = default(iobj_DynamicArray<Item>); return false; } }
            public bool attr_Stack(out iobj_DynamicSequence<Item> result) { try { result = new obj_Stack(); return true; } catch (System.Exception) { result = default(iobj_DynamicSequence<Item>); return false; } }
            public bool attr_Queue(out iobj_DynamicSequence<Item> result) { try { result = new obj_Queue(); return true; } catch (System.Exception) { result = default(iobj_DynamicSequence<Item>); return false; } }
        }
    }

    namespace Rational
    {
        public struct com_RatioStruc
        {
            public long attr_num;
            public long attr_den;
        }

        public interface imod_Rational
        {
            bool op_integer_Slash_integer(long num, long den, out opaque_Rational result);
            bool op__struc(opaque_Rational x, out com_RatioStruc result);
            bool op_Rational_Plus_Rational(opaque_Rational a, opaque_Rational b, out opaque_Rational result);
            bool op_Rational_Less_Rational(opaque_Rational a, opaque_Rational b);
        }

        public struct opaque_Rational
        {
            public long num;
            public long den;
        }

        public class module_Rational : AhaModule, imod_Rational
        {
            public bool op_integer_Slash_integer(long num, long den, out opaque_Rational result) { result = new opaque_Rational { num = num, den = den }; return true; }
            public bool op__struc(opaque_Rational x, out com_RatioStruc result) { result = new com_RatioStruc { attr_num = x.num, attr_den = x.den }; return true; }
            public bool op_Rational_Plus_Rational(opaque_Rational a, opaque_Rational b, out opaque_Rational result)
            {
                try
                {
                    result = new opaque_Rational { num = checked(a.num * b.den + a.den * b.num), den = checked(a.den * b.den) };
                    return true;
                }
                catch (System.Exception)
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
                catch (System.Exception)
                {
                    return false;
                }
            }
        }
    }

    namespace Float
    {
        public struct com_GeneralFormatParams
        {
            public char attr_period;
        }

        public struct com_FixedFormatParams
        {
            public char attr_period;
            public long attr_decimals;
        }

        public struct com_ExponentFormatParams
        {
            public char attr_period;
        }

        public struct com_FormatParams
        {
            public com_GeneralFormatParams attr_general;
            public com_FixedFormatParams attr_fixed;
            public com_ExponentFormatParams attr_exponent;
        }

        public interface imod_Float
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

        public struct opaque_Float
        {
            public double value;
        }

        //public class opaque_Matrix
        //{
        //    public double[,] value;
        //}

        public class module_Float : AhaModule, imod_Float
        {
            public bool op__float_integer(long x, out opaque_Float result) { result = new opaque_Float { value = (double)x }; return true; }
            public bool op__float_Rational(Rational.opaque_Rational x, out opaque_Float result) { try { result = new opaque_Float { value = (double)x.num / (double)x.den }; return true; } catch (System.Exception) { result = default(opaque_Float); return false; } }
            public bool op_Float_Plus_Float(opaque_Float a, opaque_Float b, out opaque_Float result) { try { result = new opaque_Float { value = a.value + b.value }; return true; } catch (System.Exception) { result = default(opaque_Float); return false; } }
            public bool op_Float_Minus_Float(opaque_Float a, opaque_Float b, out opaque_Float result) { try { result = new opaque_Float { value = a.value - b.value }; return true; } catch (System.Exception) { result = default(opaque_Float); return false; } }
            public bool op_Float_Times_Float(opaque_Float a, opaque_Float b, out opaque_Float result) { try { result = new opaque_Float { value = a.value * b.value }; return true; } catch (System.Exception) { result = default(opaque_Float); return false; } }
            public bool op_Float_Div_Float(opaque_Float a, opaque_Float b, out opaque_Float result) { try { result = new opaque_Float { value = a.value / b.value }; return true; } catch (System.Exception) { result = default(opaque_Float); return false; } }
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
            public bool fattr_sin(opaque_Float a, out opaque_Float result) { try { result = new opaque_Float { value = System.Math.Sin(a.value) }; return true; } catch (System.Exception) { result = default(opaque_Float); return false; } }
            public bool fattr_cos(opaque_Float a, out opaque_Float result) { try { result = new opaque_Float { value = System.Math.Cos(a.value) }; return true; } catch (System.Exception) { result = default(opaque_Float); return false; } }
            public bool fattr_exp(opaque_Float a, out opaque_Float result) { try { result = new opaque_Float { value = System.Math.Exp(a.value) }; return true; } catch (System.Exception) { result = default(opaque_Float); return false; } }
            public bool fattr_log(opaque_Float a, out opaque_Float result) { try { result = new opaque_Float { value = System.Math.Log(a.value) }; return true; } catch (System.Exception) { result = default(opaque_Float); return false; } }
            public bool fattr_tan(opaque_Float a, out opaque_Float result) { try { result = new opaque_Float { value = System.Math.Tan(a.value) }; return true; } catch (System.Exception) { result = default(opaque_Float); return false; } }
            public bool fattr_sqrt(opaque_Float a, out opaque_Float result) { try { result = new opaque_Float { value = System.Math.Sqrt(a.value) }; return true; } catch (System.Exception) { result = default(opaque_Float); return false; } }
            public bool attr_Pi(out opaque_Float result) { result = new opaque_Float { value = System.Math.PI }; return true; }
        }
    }

    namespace Time
    {
        public struct opaque_Timestamp
        {
            public long ticks;
        }

        public struct opaque_Interval
        {
            public long ticks;
        }

        public struct com_DateStruc
        {
            public long attr_year;
            public long attr_month;
            public long attr_day;
        }

        public struct com_TimeStruc
        {
            public long attr_hour;
            public long attr_min;
            public long attr_sec;
            public long attr_msec;
        }

        public struct com_TimestampStruc
        {
            public com_DateStruc attr_date;
            public com_TimeStruc attr_time;
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
            bool op__date_DateStruc(com_DateStruc date, out opaque_Timestamp result);
            bool op__time_TimeStruc(com_TimeStruc time, out opaque_Interval result);
            bool op__ticks_Interval(opaque_Interval i, out long result);
            bool op__interval_integer(long param_ticks, out opaque_Interval result);
        }

        public class module_Time : AhaModule, imod_Time
        {
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
            public bool op__date_DateStruc(com_DateStruc date, out opaque_Timestamp result)
            {
                try
                {
                    result = new opaque_Timestamp { ticks = (new DateTime((int)date.attr_year, (int)date.attr_month, (int)date.attr_day)).Ticks };
                    return true;
                }
                catch(System.Exception)
                {
                    result = default(opaque_Timestamp);
                    return false;
                }
            }
            public bool op__time_TimeStruc(com_TimeStruc time, out opaque_Interval result)
            {
                try
                {
                    result = new opaque_Interval { ticks = (new TimeSpan(0, (int)time.attr_hour, (int)time.attr_min, (int)time.attr_sec, (int)time.attr_msec)).Ticks };
                    return true;
                }
                catch (System.Exception)
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
    {
        public struct com_Substring
        {
            public long attr_index;
            public long attr_length;
        }

        public interface icom_Pattern
        {
            bool attr_string(out IahaArray<char> result);
            bool attr_regEx(out opaque_RegEx result);
        }

        public struct com_SearchParams
        {
            public icom_Pattern attr_for;
            public IahaArray<char> attr_in;
        }

        public struct com_PutParams
        {
            public long attr_at;
            public char attr_char;
        }

        public struct com_ReplaceParams
        {
            public IahaArray<com_Substring> attr_substr;
            public IahaArray<char> attr_with;
        }

        public struct com_PadParams
        {
            public char attr_with;
            public long attr_to;
        }

        public interface imod_StrUtils
        {
            bool fattr_Substr(IahaArray<char> s, com_Substring ss, out IahaArray<char> result);
            bool fattr_RegEx(IahaArray<char> s, out opaque_RegEx result);
            bool fattr_Search(com_SearchParams param, out IahaSequence<com_Substring> result);
            bool fattr_StringHashFunc(IahaArray<char> s, out long result);
        }

        public struct opaque_RegEx
        {
            public System.Text.RegularExpressions.Regex value;
        }

        public class module_StrUtils : AhaModule, imod_StrUtils
        {
            class obj_SearchSeq : IahaSequence<com_Substring>
            {
                private string str;
                private string sub;
                private int index;
                public obj_SearchSeq(string s, string ss) { str = s; sub = ss; index = s.IndexOf(ss); }
                public bool state(out com_Substring result)
                {
                    if (index >= 0)
                    {
                        result = new com_Substring { attr_index = index, attr_length = sub.Length };
                        return true;
                    }
                    else
                    {
                        result = default(com_Substring);
                        return false;
                    }
                }
                public IahaObject<com_Substring> copy() { return new obj_SearchSeq(str, sub) { index = index }; }
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
                public bool first(Predicate<com_Substring> that, long max, out com_Substring result)
                {
                    int i = index;
                    long j = 0;
                    com_Substring substr = new com_Substring { attr_index = i, attr_length = sub.Length };
                    while (i != -1 && j < max)
                    {
                        substr.attr_index = i;
                        if (that(substr)) { result = substr; return true; }
                        i = str.IndexOf(sub, i + 1);
                        j++;
                    }
                    result = default(com_Substring);
                    return false;
                }
            }

            class obj_SearchRegExSeq : IahaSequence<com_Substring>
            {
                private string str;
                private System.Text.RegularExpressions.Regex regEx;
                private System.Text.RegularExpressions.Match match;
                public obj_SearchRegExSeq(string s, System.Text.RegularExpressions.Regex r) { str = s; regEx = r; match = regEx.Match(s); }
                public bool state(out com_Substring result)
                {
                    if (match.Success)
                    {
                        result = new com_Substring { attr_index = match.Index, attr_length = match.Length };
                        return true;
                    }
                    else
                    {
                        result = default(com_Substring);
                        return false;
                    }
                }
                public IahaObject<com_Substring> copy() { return new obj_SearchRegExSeq(str, regEx) { match = match }; }
                public bool action_skip() { match = match.NextMatch(); return match.Success; }
                public bool first(Predicate<com_Substring> that, long max, out com_Substring result)
                {
                    long j = 0;
                    com_Substring substr = new com_Substring { attr_index = match.Index, attr_length = match.Length };
                    System.Text.RegularExpressions.Match m = match;
                    while (m.Success && j < max)
                    {
                        substr.attr_index = m.Index;
                        substr.attr_length = m.Length;
                        if (that(substr)) { result = substr; return true; }
                        m = m.NextMatch();
                        j++;
                    }
                    result = default(com_Substring);
                    return false;
                }
            }

            public bool fattr_Substr(IahaArray<char> s, com_Substring ss, out IahaArray<char> result)
            {
                char[] items = new char[ss.attr_length];
                Array.Copy(s.get(), ss.attr_index, items, 0, ss.attr_length);
                result = new AhaString(items);
                return true;
            }
            public bool fattr_RegEx(IahaArray<char> s, out opaque_RegEx result)
            {
                try
                {
                    result = new opaque_RegEx { value = new System.Text.RegularExpressions.Regex(new string(s.get())) };
                    return true;
                }
                catch (System.Exception)
                {
                    result = default(opaque_RegEx);
                    return false;
                }
            }
            public bool fattr_Search(com_SearchParams param, out IahaSequence<com_Substring> result)
            {
                icom_Pattern p;
                IahaArray<char> sub;
                opaque_RegEx rex;
                result = default(IahaSequence<com_Substring>);
                if (param.attr_for.attr_string(out sub))
                {
                    string temp1 = new string(sub.get());
                    string temp2 = new string(param.attr_in.get());
                    result = new obj_SearchSeq(temp2, temp1);
                    return true;
                }
                if (param.attr_for.attr_regEx(out rex))
                {
                    string temp = new string(param.attr_in.get());
                    result = new obj_SearchRegExSeq(temp, rex.value);
                    return true;
                }
                return false;
            }
            public bool fattr_StringHashFunc(IahaArray<char> s, out long result) { result = s.get().GetHashCode(); return true; }
        }
    }

    namespace Trees
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

        public struct com_TreeParam<tpar_Node>
        {
            public tpar_Node attr_root;
            public IahaArray<opaque_Tree<tpar_Node>> attr_children;
        }

        public interface imod_Trees<tpar_Node>
        {
            bool fattr_Tree(com_TreeParam<tpar_Node> param, out opaque_Tree<tpar_Node> result);
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

            public bool fattr_Tree(com_TreeParam<tpar_Node> param, out opaque_Tree<tpar_Node> result)
            {
                int levels = 0;
                foreach (opaque_Tree<tpar_Node> child in param.attr_children.get())
                    if (child.levels > levels) levels = child.levels;
                result = new opaque_Tree<tpar_Node>() { root = param.attr_root, children = param.attr_children.get(), levels = levels + 1 };
                return true;
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

        public struct com_Substring
        {
            public long attr_index;
            public long attr_length;
        }

        public interface imod_Bits
        {
            bool fattr_Substr(opaque_BitString str, com_Substring sub, out opaque_BitString result);
            bool attr_True(out opaque_BitString result);
            bool attr_False(out opaque_BitString result);
            bool attr_Nil(out opaque_BitString result);
            bool op_BitString_Equal_BitString(opaque_BitString first, opaque_BitString second, out opaque_BitString result);
            bool op_BitString_And_BitString(opaque_BitString first, opaque_BitString second, out opaque_BitString result);
            bool op_BitString_Or_BitString(opaque_BitString first, opaque_BitString second, out opaque_BitString result);
            bool op_BitString_Xor_BitString(opaque_BitString first, opaque_BitString second, out opaque_BitString result);
        }

        public class module_Bits : AhaModule//, imod_Bits
        {
        }
    }
}
