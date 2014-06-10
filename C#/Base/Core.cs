using System;
using System.Collections.Generic;

namespace Aha.Core
{
    public interface IahaObject<State>
    {
        bool state(out State result);
        IahaObject<State> copy();
    }

    public interface IahaSequence<Item> : IahaObject<Item>
    {
        bool action_skip();
        bool first(Predicate<Item> that, long max, out Item result);
    }

    public class Failure : System.Exception
    {
        public Failure() : base() { }
        public static readonly Failure One = new Failure();
    }

    public delegate bool Fold<Item>(Item first, Item second, out Item result);

    public delegate bool Order<Item>(Item first, Item second);

    public interface IahaArray<Item>
    {
        long size();
        bool at(long index, out Item result);
        bool forEach(Predicate<Item> that);
        bool forSome(Predicate<Item> that);
        bool such(Predicate<Item> that, out Item result);
        bool count(Predicate<Item> that, out long result);
        bool select(Predicate<Item> that, out Item[] result);
        bool enumerate(Predicate<Item> that, out IahaSequence<Item> result);
        bool foldl(Fold<Item> rule, out Item result);
        bool foldr(Fold<Item> rule, out Item result);
        bool sort(Order<Item> that, out IahaSequence<Item> result);
        Item[] get();
    }

    public struct AhaSeq<Item> : IahaSequence<Item>
    {
        public delegate bool Rule(Item prev, out Item result);

        public Item curr;
        public Rule rule;
        public bool state(out Item result) { result = curr; return true; }
        public IahaObject<Item> copy() { AhaSeq<Item> clone = new AhaSeq<Item> { curr = curr, rule = rule }; return clone; }
        public bool action_skip() { return rule(curr, out curr); }
        public bool first(Predicate<Item> that, long max, out Item result) 
        { 
            long j = 0; 
            Item item = curr; 
            while (j < max) 
            { 
                if (that(item)) { result = item; return true; }
                if (!rule(item, out item)) break; 
                j++; 
            } 
            result = default(Item); 
            return false; 
        }
    }

    public struct AhaObjSeq<Item> : IahaSequence<Item>
    {
        public IahaSequence<Item> obj;
        public bool state(out Item result) { return obj.state(out result); }
        public IahaObject<Item> copy() { AhaObjSeq<Item> clone = new AhaObjSeq<Item> { obj = obj }; return clone; }
        public bool action_skip() { return obj.action_skip(); }
        public bool first(Predicate<Item> that, long max, out Item result) 
        { 
            IahaSequence<Item> clone = (IahaSequence<Item>)obj.copy(); 
            long j = 0; 
            Item item; 
            if (clone.state(out item)) 
                while (j < max) 
                { 
                    if (that(item)) { result = item; return true; }
                    if (!(clone.action_skip() && clone.state(out item))) break;
                    j++; 
                } 
            result = default(Item); 
            return false; 
        }
    }

    public struct AhaArraySeq<Item> : IahaSequence<Item>
    {
        public Item[] items;
        public int index;
        public bool state(out Item result) 
        { 
            if (index < items.Length) 
            { 
                result = items[index]; 
                return true; 
            } 
            result = default(Item); 
            return false; 
        }
        public IahaObject<Item> copy() { return new AhaArraySeq<Item> { items = items, index = index }; }
        public bool action_skip() 
        {
            if (index < items.Length) 
            { index++; return true; }
            else 
            { return false; } 
        }
        public bool first(Predicate<Item> that, long max, out Item result) 
        { 
            int j = 0; 
            while (j < items.Length && j < max) 
            {
                if (that(items[j])) { result = items[j]; return true; }
                j++; 
            }
            result = default(Item); 
            return false; 
        }
    }

    public struct AhaFilteredArraySeq<Item> : IahaSequence<Item>
    {
        private Item[] items;
        private int index;
        private Predicate<Item> p;
        public bool state(out Item result) { result = items[index]; return true; }
        public IahaObject<Item> copy() { return new AhaFilteredArraySeq<Item> { items = items, index = index, p = p }; }
        public bool action_skip() 
        { 
            if (index < items.Length) 
                index++; 
            else 
                return false; 
            while (index < items.Length && !p(items[index])) 
                index++; 
            return index < items.Length; 
        }
        public bool first(Predicate<Item> that, long max, out Item result) 
        { 
            int j = 0; 
            while (j < items.Length && j < max) 
            {
                if (p(items[j]) && that(items[j])) { result = items[j]; return true; }
                j++; 
            }
            result = default(Item);
            return false; 
        }
        public AhaFilteredArraySeq(Item[] it, int idx, Predicate<Item> pred) 
        { 
            items = it; 
            index = idx; 
            p = pred; 
            while (index < items.Length && !p(items[index])) 
                index++; 
            if (index == items.Length) 
                throw Failure.One; 
        }
    }

    public struct AhaFilteredSegmentSeq : IahaSequence<long>
    {
        private long lo;
        private long hi;
        private long index;
        private Predicate<long> p;
        public bool state(out long result) { result = lo + index; return true; }
        public IahaObject<long> copy() { return new AhaFilteredSegmentSeq(lo, hi, index, p); }
        public bool action_skip() 
        { 
            if (lo + index < hi) 
                index++; 
            else 
                return false; 
            while (lo + index < hi && !p(lo + index)) 
                index++;
            return lo + index < hi;
        }
        public bool first(Predicate<long> that, long max, out long result) 
        { 
            long j = 0; 
            while (j + lo < hi && j < max) 
            {
                if (p(lo + j) && that(lo + j))
                {
                    result = lo + j;
                    return true;
                }
                j++; 
            }
            result = 0;
            return false; 
        }
        public AhaFilteredSegmentSeq(long l, long h, long i, Predicate<long> pred) 
        { 
            lo = l; 
            hi = h; 
            index = i; 
            p = pred; 
            while (lo + index < hi && !p(lo + index)) 
                index++; 
            if (lo + index == hi) 
                throw Failure.One; 
        }
    }

    public struct AhaEmptySeq<Item> : IahaSequence<Item>
    {
        public bool state(out Item result) { result = default(Item); return false; }
        public IahaObject<Item> copy() { return new AhaEmptySeq<Item>(); }
        public bool action_skip() { return false; }
        public bool first(Predicate<Item> that, long max, out Item result) { result = default(Item); return false; }
    }

    public struct AhaArray<Item> : IahaArray<Item>
    {
        public delegate bool Rule(long index, out Item result);

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
        public AhaArray(IahaSequence<Item> seq, long max)
        {
            Item item;
            items = new Item[max];
            IahaSequence<Item> s = (IahaSequence<Item>)seq.copy();
            for (int i = 0; i < max; i++)
            {
                if (s.state(out item))
                {
                    items[i] = item;
                    if (!s.action_skip())
                    {
                        Array.Resize<Item>(ref items, i + 1);
                        break;
                    }
                }
                else
                {
                    Array.Resize<Item>(ref items, i);
                    break;
                }
            }
        }
        public AhaArray(Rule rule, long max) 
        { 
            items = new Item[max]; 
            for (long i = 0; i < max; i++)  
                if (!rule(i, out items[i])) 
                    throw Failure.One; 
        }
        public long size() { return items.LongLength; }
        public bool at(long index, out Item result) 
        { 
            if (index >= 0 && index < items.Length) 
            { 
                result = items[index]; 
                return true; 
            } 
            else 
            { 
                result = default(Item); 
                return false; 
            } 
        }
        public bool sort(Order<Item> that, out IahaSequence<Item> result)
        {
            try
            {
                Item[] clone = (Item[])items.Clone();
                Comparison<Item> comp = 
                    delegate(Item x, Item y) 
                    { 
                        if (that(x, y)) 
                        { 
                            if (that(y, x)) 
                                return 0; 
                            else 
                                return -1; 
                        } 
                        else 
                            return 1; 
                    };
                Array.Sort<Item>(clone, comp);
                result = new AhaArraySeq<Item> { items = clone, index = 0 };
                return true;
            }
            catch(System.Exception)
            { 
                result = default(IahaSequence<Item>);
                return false;
            }
        }
        public bool forEach(Predicate<Item> that) { return Array.TrueForAll(items, that); }
        public bool forSome(Predicate<Item> that) { return Array.Exists(items, that); }
        public bool such(Predicate<Item> that, out Item result)
        { 
            int index = Array.FindIndex<Item>(items, that);
            if (index >= 0)
            {
                result = items[index];
                return true;
            }
            else
            {
                result = default(Item);
                return false;
            }
        }
        public bool count(Predicate<Item> that, out long result)
        { 
            int j = 0; 
            foreach (Item item in items) 
            { 
                if (that(item)) 
                    j++; 
            } 
            result = j;
            return true;
        }
        public bool select(Predicate<Item> that, out Item[] result)
        {
            try
            {
                result = Array.FindAll<Item>(items, that);
                return true;
            }
            catch (System.Exception)
            {
                result = default(Item[]);
                return false;
            }
        }
        public bool enumerate(Predicate<Item> that, out IahaSequence<Item> result)
        { 
            try 
            { 
                result = new AhaFilteredArraySeq<Item>(items, 0, that);
                return true;
            } 
            catch (System.Exception) 
            {
                result = default(IahaSequence<Item>);
                return false;
            } 
        }
        public bool foldl(Fold<Item> rule, out Item result) 
        {
            if (items.Length == 0)
            {
                result = default(Item);
                return false;
            }
            result = items[0]; 
            for (int i = 1; i < items.Length; i++) 
                if (!rule(result, items[i], out result))
                {
                    result = default(Item);
                    return false;
                }
            return true; 
        }
        public bool foldr(Fold<Item> rule, out Item result) 
        {
            if (items.Length == 0)
            {
                result = default(Item);
                return false;
            }
            result = items[items.Length - 1]; 
            for (int i = items.Length - 2; i >= 0; i--) 
                if (!rule(items[i], result, out result))
                {
                    result = default(Item);
                    return false;
                }
            return true; 
        }
        public Item[] get() { return items; }
    }

    public struct AhaString : IahaArray<char>
    {
        public delegate char Rule(long index);

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
        public AhaString(IahaSequence<char> seq, long max)
        {
            char[]temp = new char[max];
            IahaSequence<char> s = (IahaSequence<char>)seq.copy();
            for (int i = 0; i < max; i++)
            {
                if (s.state(out temp[i]))
                {
                    if (!s.action_skip())
                    {
                        Array.Resize<char>(ref temp, i + 1);
                        break;
                    }
                }
                else
                {
                    Array.Resize<char>(ref temp, i);
                    break;
                }
            }
            items = new string(temp);
        }
        public AhaString(Rule rule, long max) 
        { 
            char[] temp = new char[max]; 
            for (long i = 0; i < max; i++) 
                temp[i] = rule(i); 
            items = new string(temp); 
        }
        public long size() { return items.Length; }
        public bool at(long index, out char result) 
        { 
            if (index >= 0 && index < items.Length) 
            { 
                result = items[(int)index]; 
                return true; 
            } 
            else 
            { 
                result = default(char); 
                return false; 
            } 
        }
        public bool sort(Order<char> that, out IahaSequence<char> result)
        {
            try
            {
                char[] clone = items.ToCharArray();
                Comparison<char> comp = delegate(char x, char y) { if (that(x, y)) { if (that(y, x)) return 0; else return -1; } else return 1; };
                Array.Sort<char>(clone, comp);
                result = new AhaArraySeq<char> { items = clone, index = 0 };
                return true;
            }
            catch(System.Exception)
            {
                result = default(IahaSequence<char>);
                return false;
            }
        }
        public bool forEach(Predicate<char> that) { return Array.TrueForAll(items.ToCharArray(), that); }
        public bool forSome(Predicate<char> that) { return Array.Exists(items.ToCharArray(), that); }
        public bool such(Predicate<char> that, out char result)
        { 
            int index = Array.FindIndex<char>(items.ToCharArray(), that);
            if (index >= 0)
            {
                result = items[index];
                return true;
            }
            result = default(char);
            return false;
        }
        public bool count(Predicate<char> that, out long result)
        { 
            int j = 0; 
            foreach (char ch in items) 
            { 
                if (that(ch)) 
                    j++; 
            } 
            result = j;
            return true;
        }
        public bool select(Predicate<char> that, out char[] result)
        {
            try
            {
                char[] sel = Array.FindAll<char>(items.ToCharArray(), that);
                result = sel;
                return true;
            }
            catch(System.Exception)
            { 
                result = default(char[]);
                return false;
            }
        }
        public bool enumerate(Predicate<char> that, out IahaSequence<char> result)
        { 
            try 
            { 
                result = new AhaFilteredArraySeq<char>(items.ToCharArray(), 0, that);
                return true;
            } 
            catch (System.Exception) 
            {
                result = default(IahaSequence<char>);
                return false;
            } 
        }
        public bool foldl(Fold<char> rule, out char result) 
        { 
            if (items.Length == 0)
            {
                result = default(char);
                return false;
            }
            result = items[0]; 
            for (int i = 1; i < items.Length; i++) 
                if (!rule(result, items[i], out result))
                {
                    result = default(char);
                    return false;
                }
            return true; 
        }
        public bool foldr(Fold<char> rule, out char result)
        { 
            if (items.Length == 0)
            {
                result = default(char);
                return false;
            }
            result = items[items.Length - 1]; 
            for (int i = items.Length - 2; i >= 0; i--) 
                if (!rule(items[i], result, out result))
                {
                    result = default(char);
                    return false;
                }
            return true; 
        }
        public char[] get() { return items.ToCharArray(); }
    }

    public struct AhaSegment : IahaArray<long>
    {
        private long lo;
        private long hi;
        private long[] list() { long[] result = new long[hi - lo]; int j = 0; for (long i = lo; i < hi; i++) { result[j] = i; j++; } return result; }
        public AhaSegment(long low, long high) { if (low > high) throw Failure.One; lo = low; hi = high; }
        public long size() { return hi - lo; }
        public bool at(long index, out long result) { result = lo + index; return index >= 0 && index < hi - lo; }
        public bool sort(Order<long> that, out IahaSequence<long> result)
        {
            try
            {
                long[] temp = list();
                Comparison<long> comp = delegate(long x, long y) { if (that(x, y)) { if (that(y, x)) return 0; else return -1; } else return 1; };
                Array.Sort<long>(temp, comp);
                result = new AhaArraySeq<long> { items = temp, index = 0 };
                return true;
            }
            catch(System.Exception)
            {
                result = default(IahaSequence<long>);
                return false;
            }
        }
        public bool forEach(Predicate<long> that) { for (long i = lo; i < hi; i++) { if (!that(i)) return false; } return true; }
        public bool forSome(Predicate<long> that) { for (long i = lo; i < hi; i++) { if (that(i)) return true; } return false; }
        public bool such(Predicate<long> that, out long result) { for (long i = lo; i < hi; i++) { if (that(i)) { result = i; return true; } } result = 0; return false; }
        public bool count(Predicate<long> that, out long result) { long j = 0; for (long i = lo; i < hi; i++) { if (that(i)) j++; } result = j; return true; }
        public bool select(Predicate<long> that, out long[] result)
        {
            try
            {
                long[] sel = new long[hi - lo];
                int j = 0;
                for (long i = lo; i < hi; i++)
                {
                    if (that(i))
                    {
                        sel[j] = i;
                        j++;
                    }
                }
                Array.Resize<long>(ref sel, j);
                result = sel;
                return true;
            }
            catch(System.Exception)
            {
                result = default(long[]);
                return false;
            }
        }
        public bool enumerate(Predicate<long> that, out IahaSequence<long> result)
        { 
            try 
            { 
                result = new AhaFilteredSegmentSeq(lo, hi, 0, that);
                return true;
            } 
            catch(System.Exception) 
            {
                result = default(IahaSequence<long>);
                return false;
            } 
        }
        public bool foldl(Fold<long> rule, out long result) 
        {
            result = lo;
            if (lo == hi) 
                return false; 
            for (long i = lo + 1; i < hi; i++) 
                 if (!rule(result, i, out result))
                     return false;
            return true; 
        }
        public bool foldr(Fold<long> rule, out long result)
        {
            result = hi - 1;
            if (lo == hi)
                return false;
            for (long i = hi - 2; i >= lo; i--)
                if (!rule(i, result, out result))
                    return false;
            return true;
        }
        public long[] get() { return list(); }
    }

    public class AhaModule
    {
    }
}
