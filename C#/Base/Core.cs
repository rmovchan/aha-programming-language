using System;
using System.Collections.Generic;

namespace AhaCore
{
    public interface IahaObject<State>
    {
        State state();
        IahaObject<State> copy();
    }

    public interface IahaSequence<Item> : IahaObject<Item>
    {
        void action_skip();
        Item first(Predicate<Item> that, Int64 max);
    }

    public class Failure : System.Exception
    {
        public Failure() : base() { }
        public static readonly Failure One = new Failure();
    }

    public delegate Item Fold<Item>(Item first, Item second);

    public delegate bool Order<Item>(Item first, Item second);

    public interface IahaArray<Item>
    {
        Int64 size();
        Item at(Int64 index);
        bool forEach(Predicate<Item> that);
        bool forSome(Predicate<Item> that);
        Item such(Predicate<Item> that);
        Int64 count(Predicate<Item> that);
        Item[] select(Predicate<Item> that);
        IahaSequence<Item> enumerate(Predicate<Item> that);
        Item foldl(Fold<Item> rule);
        Item foldr(Fold<Item> rule);
        IahaSequence<Item> sort(Order<Item> that);
        Item[] get();
    }

    public struct AhaSeq<Item> : IahaSequence<Item>
    {
        public delegate Item Rule(Item prev);

        public Item curr;
        public Rule rule;
        public Item state() { return curr; }
        public IahaObject<Item> copy() { AhaSeq<Item> clone = new AhaSeq<Item> { curr = curr, rule = rule }; return clone; }
        public void action_skip() { curr = rule(curr); }
        public Item first(Predicate<Item> that, Int64 max) { Int64 j = 0; Item item = curr; while (j < max) { if (that(item)) return item; item = rule(item); j++; } throw Failure.One; }
    }

    public struct AhaObjSeq<Item> : IahaSequence<Item>
    {
        public IahaSequence<Item> obj;
        public Item state() { return obj.state(); }
        public IahaObject<Item> copy() { AhaObjSeq<Item> clone = new AhaObjSeq<Item> { obj = obj }; return clone; }
        public void action_skip() { obj.action_skip(); }
        public Item first(Predicate<Item> that, Int64 max) { IahaSequence<Item> clone = (IahaSequence<Item>)obj.copy(); Int64 j = 0; Item item = clone.state(); while (j < max) { if (that(item)) return item; clone.action_skip(); item = clone.state(); j++; } throw Failure.One; }
    }

    public struct AhaArraySeq<Item> : IahaSequence<Item>
    {
        public Item[] items;
        public int index;
        public Item state() { return items[index]; }
        public IahaObject<Item> copy() { return new AhaArraySeq<Item> { items = items, index = index }; }
        public void action_skip() { if (index < items.Length) index++; else throw Failure.One; }
        public Item first(Predicate<Item> that, Int64 max) { Int64 j = 0; while (j < items.Length) { if (that(items[j])) return items[j]; j++; } throw Failure.One; }
    }

    public struct AhaEmptySeq<Item> : IahaSequence<Item>
    {
        public Item state() { throw Failure.One; }
        public IahaObject<Item> copy() { return new AhaEmptySeq<Item>(); }
        public void action_skip() { throw Failure.One; }
        public Item first(Predicate<Item> that, Int64 max) { throw Failure.One; }
    }

    public struct AhaArray<Item> : IahaArray<Item>
    {
        public delegate Item Rule(Int64 index);

        private Item[] items;
        public AhaArray(Item[] list) { items = list; }
        public AhaArray(Item[][] join)
        {
            int total = 0;
            for (int i = 0; i < join.Length; i++) total += join[i].Length;
            items = new Item[total];
            int k = 0;
            for (int i = 0; i < join.Length; i++) { Array.Copy(join[i], 0, items, k, join[i].Length); k += join[i].Length; }
        }
        public AhaArray(IahaSequence<Item> seq, Int64 max)
        {
            items = new Item[max];
            IahaSequence<Item> s = (IahaSequence<Item>)seq.copy();
            for (int i = 0; i < max; i++)
            {
                items[i] = s.state();
                try { s.action_skip(); }
                catch (System.Exception) { Array.Resize<Item>(ref items, i + 1); break; }
            }
        }
        public AhaArray(Rule rule, Int64 max) { items = new Item[max]; for (Int64 i = 0; i < max; i++) items[i] = rule(i); }
        public Int64 size() { return items.LongLength; }
        public Item at(Int64 index) { return items[index]; }
        public IahaSequence<Item> sort(Order<Item> that)
        {
            Item[] clone = (Item[])items.Clone();
            Comparison<Item> comp = delegate(Item x, Item y) { if (that(x, y)) { if (that(y, x)) return 0; else return -1; } else { if (that(y, x)) return 0; else return 1; } };
            Array.Sort<Item>(clone, comp);
            return new AhaArraySeq<Item> { items = clone, index = 0 };
        }
        public bool forEach(Predicate<Item> that) { return Array.TrueForAll(items, that); }
        public bool forSome(Predicate<Item> that) { return Array.Exists(items, that); }
        public Item such(Predicate<Item> that)
        { int index = Array.FindIndex<Item>(items, that); if (index >= 0) return items[index]; else throw Failure.One; }
        public Int64 count(Predicate<Item> that)
        { int j = 0; for (int i = 0; i < items.Length; i++) { if (that(items[i])) j++; } return j; }
        public Item[] select(Predicate<Item> that)
        { Item[] sel = Array.FindAll<Item>(items, that); return sel; }
        public IahaSequence<Item> enumerate(Predicate<Item> that)
        { Item[] sel = Array.FindAll<Item>(items, that); return new AhaArraySeq<Item> { items = sel, index = 0 }; }
        public Item foldl(Fold<Item> rule) { if (items.Length == 0) throw Failure.One; Item result = items[0]; for (int i = 1; i < items.Length; i++) result = rule(result, items[i]); return result; }
        public Item foldr(Fold<Item> rule) { if (items.Length == 0) throw Failure.One; Item result = items[items.Length - 1]; for (int i = items.Length - 2; i >= 0; i--) result = rule(items[i], result); return result; }
        public Item[] get() { return items; }
    }

    public struct AhaString : IahaArray<char>
    {
        public delegate char Rule(Int64 index);

        private string items;
        public AhaString(char[] list) { items = new string(list); }
        public AhaString(string s) { items = s; }
        public AhaString(char[][] join)
        {
            int size = 0;
            for (int i = 0; i < join.Length; i++) size += join[i].Length;
            char[] buf = new char[size];
            int j = 0;
            for (int i = 0; i < join.Length; i++) { Array.Copy(join[i], 0, buf, j, join[i].Length); j += join[i].Length; }
            items = new string(buf);
        }
        public AhaString(IahaSequence<char> seq, Int64 max)
        {
            char[]temp = new char[max];
            IahaSequence<char> s = (IahaSequence<char>)seq.copy();
            for (int i = 0; i < max; i++)
            {
                temp[i] = s.state();
                try { s.action_skip(); }
                catch (System.Exception) { Array.Resize<char>(ref temp, i + 1); break; }
            }
            items = new string(temp);
        }
        public AhaString(Rule rule, Int64 max) { char[] temp = new char[max]; for (Int64 i = 0; i < max; i++) temp[i] = rule(i); items = new string(temp); }
        public Int64 size() { return items.Length; }
        public char at(Int64 index) { return items[(int)index]; }
        public IahaSequence<char> sort(Order<char> that)
        {
            char[] clone = items.ToCharArray();
            Comparison<char> comp = delegate(char x, char y) { if (that(x, y)) { if (that(y, x)) return 0; else return -1; } else { if (that(y, x)) return 0; else return 1; } };
            Array.Sort<char>(clone, comp);
            return new AhaArraySeq<char> { items = clone, index = 0 };
        }
        public bool forEach(Predicate<char> that) { return Array.TrueForAll(items.ToCharArray(), that); }
        public bool forSome(Predicate<char> that) { return Array.Exists(items.ToCharArray(), that); }
        public char such(Predicate<char> that)
        { int index = Array.FindIndex<char>(items.ToCharArray(), that); if (index >= 0) return items[index]; else throw Failure.One; }
        public Int64 count(Predicate<char> that)
        { int j = 0; for (int i = 0; i < items.Length; i++) { if (that(items[i])) j++; } return j; }
        public char[] select(Predicate<char> that)
        { char[] sel = Array.FindAll<char>(items.ToCharArray(), that); return sel; }
        public IahaSequence<char> enumerate(Predicate<char> that)
        { char[] sel = Array.FindAll<char>(items.ToCharArray(), that); return new AhaArraySeq<char> { items = sel, index = 0 }; }
        public char foldl(Fold<char> rule) { if (items.Length == 0) throw Failure.One; char result = items[0]; for (int i = 1; i < items.Length; i++) result = rule(result, items[i]); return result; }
        public char foldr(Fold<char> rule) { if (items.Length == 0) throw Failure.One; char result = items[items.Length - 1]; for (int i = items.Length - 2; i >= 0; i--) result = rule(items[i], result); return result; }
        public char[] get() { return items.ToCharArray(); }
    }

    public struct AhaSegment : IahaArray<Int64>
    {
        private Int64 lo;
        private Int64 hi;
        private Int64[] list() { Int64[] result = new Int64[hi - lo]; int j = 0; for (Int64 i = lo; i < hi; i++) { result[j] = i; j++; } return result; }
        public AhaSegment(Int64 low, Int64 high) { if (low > high) throw Failure.One; lo = low; hi = high; }
        public Int64 size() { return hi - lo; }
        public Int64 at(Int64 index) { return lo + index; }
        public IahaSequence<Int64> sort(Order<Int64> that)
        {
            Int64[] temp = list();
            Comparison<Int64> comp = delegate(Int64 x, Int64 y) { if (that(x, y)) { if (that(y, x)) return 0; else return -1; } else { if (that(y, x)) return 0; else return 1; } };
            Array.Sort<Int64>(temp, comp);
            return new AhaArraySeq<Int64> { items = temp, index = 0 };
        }
        public bool forEach(Predicate<Int64> that) { for (Int64 i = lo; i < hi; i++) { if (!that(i)) return false; } return true; }
        public bool forSome(Predicate<Int64> that) { for (Int64 i = lo; i < hi; i++) { if (that(i)) return true; } return false; }
        public Int64 such(Predicate<Int64> that) { for (Int64 i = lo; i < hi; i++) { if (that(i)) return i; } throw Failure.One; }
        public Int64 count(Predicate<Int64> that) { int j = 0; for (Int64 i = lo; i < hi; i++) { if (that(i)) j++; } return j; }
        public Int64[] select(Predicate<Int64> that)
        { Int64[] sel = new Int64[hi - lo]; int j = 0; for (Int64 i = lo; i < hi; i++) { if (that(i)) { sel[j] = i; j++; } } Array.Resize<Int64>(ref sel, j); return sel; }
        public IahaSequence<Int64> enumerate(Predicate<Int64> that)
        { Int64[] sel = new Int64[hi - lo]; int j = 0; for (Int64 i = lo; i < hi; i++) { if (that(i)) { sel[j] = i; j++; } } Array.Resize<Int64>(ref sel, j); return new AhaArraySeq<Int64> { items = sel, index = 0 }; }
        public Int64 foldl(Fold<Int64> rule) { if (lo == hi) throw Failure.One; Int64 result = lo; for (Int64 i = 1; i < hi; i++) result = rule(result, lo + i); return result; }
        public Int64 foldr(Fold<Int64> rule) { if (lo == hi) throw Failure.One; Int64 result = hi - 1; for (Int64 i = hi - 2; i >= lo; i--) result = rule(lo + i, result); return result; }
        public Int64[] get() { return list(); }
    }

    public class AhaModule
    {
    }
}
