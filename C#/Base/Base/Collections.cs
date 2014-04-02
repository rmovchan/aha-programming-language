using System;
using System.Collections.Generic;
using Core;

namespace Collections
{
    public interface IReplaceParam<Item>
    {
        Int64 index();
        Item item();
    }

    public interface IDynamicArray<Item> : IahaObject<IahaArray<Item>>
    {
        void add(Item item);
        void replace(IReplaceParam<Item> param);
        void insert(IReplaceParam<Item> param);
        void delete(Int64 index);
    }

    public interface IDynamicSequence<Item> : IahaObject<Item>
    {
        void push(Item item);
        void pop();
    }

    public class DynamicArray<Item> : IDynamicArray<Item>
    {
        private List<Item> list;
        public DynamicArray() { list = new List<Item>(); }
        public DynamicArray(Item[] items) { list = new List<Item>(items); }
        public IahaArray<Item> state() { return new AhaArray<Item>(list.ToArray()); }
        public IahaObject<IahaArray<Item>> copy() { DynamicArray<Item> clone = new DynamicArray<Item>(list.ToArray()); return clone; }
        public void add(Item item) { list.Add(item); }
        public void replace(IReplaceParam<Item> param) { list[(int)param.index()] = param.item(); }
        public void insert(IReplaceParam<Item> param) { list.Insert((int)param.index(), param.item()); }
        public void delete(Int64 index) { list.RemoveAt((int)index); }
    }


    public class Stack<Item> : IDynamicSequence<Item>
    {
        class element
        {
            public Item item;
            public element next;
        }
        private element head = null;
        public Item state() { if (head != null) return head.item; else throw Failure.One; }
        public IahaObject<Item> copy()
            { 
                element tail = null; 
                element p = head; 
                element q; 
                while (p != null) { q = new element(); q.item = p.item; q.next = tail; tail = q; p = p.next; } 
                Stack<Item> clone = new Stack<Item>();
                while (tail != null) { clone.push(tail.item); tail = tail.next; }
                return clone; 
            }
        public void push(Item item) { element h = new element(); h.item = item; h.next = head; head = h; }
        public void pop() { if (head != null) head = head.next; else throw Failure.One; }
    }


    public class Queue<Item> : IDynamicSequence<Item>
    {
        class element
        {
            public Item item;
            public element next;
            public element prev;
        }
        private element head = null;
        private element tail = null;
        public Item state() { if (tail != null) return tail.item; else throw Failure.One; }
        public IahaObject<Item> copy() { Queue<Item> clone = new Queue<Item>(); element p = head; while (p != null) { clone.push(p.item); p = p.next; } return clone; }
        public void push(Item item) { element h = new element(); h.item = item; h.next = head; h.prev = null; head = h; if (tail == null) tail = h; }
        public void pop() { if (tail != null) { tail = tail.prev; tail.prev = tail.prev.prev; } else throw Failure.One; }
    }
}
