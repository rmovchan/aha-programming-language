using System;
using System.Collections.Generic;

namespace Core
{
    public interface IahaObject<State>
    {
        State state();
        IahaObject<State> copy();
    }

    public interface IahaSequence<Item> : IahaObject<Item>
    {
        void skip();
        Item first(Predicate<Item> that, Int64 max);
    }

    public class Failure : System.Exception 
    {
        public Failure() : base() { }
        public static Failure One = new Failure();
    }

    public interface IahaArray<Item>
    {
        Int64 size();
        Item at(Int64 index);
        IahaSequence<Item> sort(IComparer<Item> that);
        bool forEach(Predicate<Item> that);
        bool forSome(Predicate<Item> that);
        Item such(Predicate<Item> that);
        Int64 count(Predicate<Item> that);
        Item[] select(Predicate<Item> that);
        IahaSequence<Item> enumerate(Predicate<Item> that);
    }

    public class AhaArraySeq<Item> : IahaSequence<Item>
    {
        private Item[] items;
        private int index;
        public AhaArraySeq(Item[] list) { items = list; index = 0; }
        public Item state() { return items[index]; }
        public IahaObject<Item> copy() { return new AhaArraySeq<Item>(items); }
        public void skip() { if (index < items.Length - 1) index++; else throw Failure.One; }
        public Item first(Predicate<Item> that, Int64 max) { int j = 0; for (int i = index; i < items.Length; i++) { if (j == max) break; if (that(items[i])) return items[i]; } throw Failure.One; }
    }

    public struct AhaSeq<Item> : IahaSequence<Item>
    {
        public delegate Item NextItem(Item prev);
        public Item curr;
        public NextItem rule;
        public Item state() { return curr; }
        public IahaObject<Item> copy() { AhaSeq<Item> clone = new AhaSeq<Item> { curr = curr, rule = rule }; return clone; }
        public void skip() { curr = rule(curr); }
        public Item first(Predicate<Item> that, Int64 max) { Item item = curr; for (Int64 i = 0; i < max; i++) { if (that(item)) return item; item = rule(item); } throw Failure.One; }
    }

    public class AhaArray<Item> : IahaArray<Item>
    {
        public delegate Item Rule(Int64 index);
        private Item[] items;
        public AhaArray(Item[] list) { items = list; }
        public AhaArray(IahaSequence<Item> seq, Int64 max)
            { 
                items = new Item[max];
                int i; 
                IahaSequence<Item> s = (IahaSequence<Item>)seq.copy();
                for (i = 0; i < max; i++) 
                { 
                    items[i] = s.state(); 
                    try { s.skip(); } 
                    catch (System.Exception) { Array.Resize<Item>(ref items, i + 1); break; } 
                } 
            }
        public AhaArray(Rule rule, Int64 max) { items = new Item[max]; for (Int64 i = 0; i < max; i++) items[i] = rule(i); }
        public Int64 size() { return items.LongLength; }
        public Item at(Int64 index) { return items[index]; }
        public IahaSequence<Item> sort(IComparer<Item> that)
            { Item[] clone = (Item[])items.Clone(); Array.Sort<Item>(clone, that); return new AhaArraySeq<Item>(clone); }
        public bool forEach(Predicate<Item> that) { return Array.TrueForAll(items, that); }
        public bool forSome(Predicate<Item> that) { return Array.Exists(items, that); }
        public Item such(Predicate<Item> that) 
            { int index = Array.FindIndex<Item>(items, that); if (index >= 0) return items[index]; else throw Failure.One; }
        public Int64 count(Predicate<Item> that)
            { Item[] sel = Array.FindAll<Item>(items, that); return sel.LongLength; }
        public Item[] select(Predicate<Item> that)
            { Item[] sel = Array.FindAll<Item>(items, that); return sel; }
        public IahaSequence<Item> enumerate(Predicate<Item> that)
            { Item[] sel = Array.FindAll<Item>(items, that); return new AhaArraySeq<Item>(sel); }
    }

    public class AhaSegment : IahaArray<Int64>
    {
        private Int64 lo;
        private Int64 hi;
        private Int64[] list() { Int64[] result = new Int64[hi - lo]; int j = 0; for (Int64 i = lo; i < hi; i++) { result[j] = i; j++; } return result; }
        public AhaSegment(Int64 low, Int64 high) { if (low > high) throw Failure.One; lo = low; hi = high; }
        public Int64 size() { return hi - lo; }
        public Int64 at(Int64 index) { return lo + index; }
        public IahaSequence<Int64> sort(IComparer<Int64> that)
            { Int64[] clone = list(); Array.Sort<Int64>(clone, that); return new AhaArraySeq<Int64>(clone); }
        public bool forEach(Predicate<Int64> that) { for (Int64 i = lo; i < hi; i++) { if (!that(i)) return false; } return true; }
        public bool forSome(Predicate<Int64> that) { for (Int64 i = lo; i < hi; i++) { if (that(i)) return true; } return false; } 
        public Int64 such(Predicate<Int64> that) { for (Int64 i = lo; i < hi; i++) { if (that(i)) return i; } throw Failure.One; }
        public Int64 count(Predicate<Int64> that) { int j = 0; for (Int64 i = lo; i < hi; i++) { if (that(i)) j++; } return j; }
        public Int64[] select(Predicate<Int64> that)
            { Int64[] sel = new Int64[count(that)]; int j = 0; for (Int64 i = lo; i < hi; i++) { if (that(i)) { sel[j] = i; j++; } } return sel; }
        public IahaSequence<Int64> enumerate(Predicate<Int64> that)
            { Int64[] sel = new Int64[count(that)]; int j = 0; for (Int64 i = lo; i < hi; i++) { if (that(i)) { sel[j] = i; j++; } } return new AhaArraySeq<Int64>(sel); }
    }
}
