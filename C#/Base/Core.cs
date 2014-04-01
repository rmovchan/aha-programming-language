using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core
{
    public interface IahaObject<State>
    {
        State state();
    }

    public interface IahaSequence<Item> : IahaObject<Item>
    {
        void skip();
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
        IahaSequence<Item> sort(IComparer<Item> comp);
        void forEach(Predicate<Item> cond);
        void forSome(Predicate<Item> cond);
        Item such(Predicate<Item> cond);
        Int64 count(Predicate<Item> cond);
        Item[] select(Predicate<Item> cond);
    }

    public class AhaArraySeq<Item> : IahaSequence<Item>
    {
        private Item[] items;
        private int index;
        public Item state() { return items[index]; }
        public void skip() { if (index < items.Length - 1) index++; else throw Failure.One; }
        public AhaArraySeq(Item[] list) { items = list; index = 0; }
    }

    public struct AhaArray<Item> : IahaArray<Item>
    {
        private Item[] items;
        public void init(Item[] list) { items = list; }
        public Int64 size() { return items.LongLength; }
        public Item at(Int64 index) { return items[index]; }
        public IahaSequence<Item> sort(IComparer<Item> comp)
            { Item[] clone = (Item[])items.Clone(); Array.Sort<Item>(clone, comp); return new AhaArraySeq<Item>(clone); }
        public void forEach(Predicate<Item> cond) { if (! Array.TrueForAll(items, cond)) throw Failure.One; }
        public void forSome(Predicate<Item> cond) { if (! Array.Exists(items, cond)) throw Failure.One; }
        public Item such(Predicate<Item> cond) 
            { int index = Array.FindIndex<Item>(items, cond); if (index >= 0) return items[index]; else throw Failure.One; }
        public Int64 count(Predicate<Item> cond)
            { Item[] sel = Array.FindAll<Item>(items, cond); return sel.LongLength; }
        public Item[] select(Predicate<Item> cond)
            { Item[] sel = Array.FindAll<Item>(items, cond); return sel; }
    }

    public struct AhaSegment : IahaArray<Int64>
    {
        private Int64 lo;
        private Int64 hi;
        private Int64[] list() { Int64[] result = new Int64[hi - lo]; int j = 0; for (Int64 i = lo; i < hi; i++) { result[j] = i; j++; } return result; }
        public void init(Int64 low, Int64 high) { lo = low; hi = high; }
        public Int64 size() { return hi - lo; }
        public Int64 at(Int64 index) { return lo + index; }
        public IahaSequence<Int64> sort(IComparer<Int64> comp)
            { Int64[] clone = list(); Array.Sort<Int64>(clone, comp); return new AhaArraySeq<Int64>(clone); }
        public void forEach(Predicate<Int64> cond) { for (Int64 i = lo; i < hi; i++) { if (!cond(i)) throw Failure.One; } }
        public void forSome(Predicate<Int64> cond) { for (Int64 i = lo; i < hi; i++) { if (cond(i)) return; } throw Failure.One; } 
        public Int64 such(Predicate<Int64> cond) { for (Int64 i = lo; i < hi; i++) { if (cond(i)) return i; } throw Failure.One; }
        public Int64 count(Predicate<Int64> cond) { int j = 0; for (Int64 i = lo; i < hi; i++) { if (cond(i)) j++; } return j; }
        public Int64[] select(Predicate<Int64> cond)
            { Int64[] sel = new Int64[count(cond)]; int j = 0; for (Int64 i = lo; i < hi; i++) { if (cond(i)) { sel[j] = i; j++; } } return sel; }
    }
}
