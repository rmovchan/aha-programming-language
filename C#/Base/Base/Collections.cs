using System;
using System.Collections.Generic;
using Aha.Core;

namespace Collections
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

    public class obj_DynamicArray<Item> : iobj_DynamicArray<Item>
    {
        private List<Item> list;
        public obj_DynamicArray() { list = new List<Item>(); }
        public obj_DynamicArray(Item[] items) { list = new List<Item>(items); }
        public IahaArray<Item> state() { return new AhaArray<Item>(list.ToArray()); }
        public IahaObject<IahaArray<Item>> copy() { obj_DynamicArray<Item> clone = new obj_DynamicArray<Item>(list.ToArray()); return clone; }
        public void action_add(Item item) { list.Add(item); }
        public void action_replace(icomp_ReplaceParam<Item> param) { list[(int)param.attr_index()] = param.attr_item(); }
        public void action_insert(icomp_InsertParam<Item> param) { list.Insert((int)param.attr_index(), param.attr_item()); }
        public void action_delete(Int64 index) { list.RemoveAt((int)index); }
    }


    public class obj_Stack<Item> : iobj_DynamicSequence<Item>
    {
        class node
        {
            public Item item;
            public node next;
        }
        private node head = null;
        public Item state() { if (head != null) return head.item; else throw Failure.One; }
        public IahaObject<Item> copy() { obj_Stack<Item> clone = new obj_Stack<Item>() { head = head }; return clone; }
        public void action_push(Item item) { node h = new node(); h.item = item; h.next = head; head = h; }
        public void action_pop() { if (head != null) head = head.next; else throw Failure.One; }
    }


    public class obj_Queue<Item> : iobj_DynamicSequence<Item>
    {
        class node
        {
            public Item item;
            public node next;
            public node prev;
        }
        private node head = null;
        private node tail = null;
        public Item state() { if (tail != null) return tail.item; else throw Failure.One; }
        public IahaObject<Item> copy() { obj_Queue<Item> clone = new obj_Queue<Item>(); node p = head; while (p != null) { clone.action_push(p.item); p = p.next; } return clone; }
        public void action_push(Item item) { node h = new node(); h.item = item; h.next = head; h.prev = null; head = h; if (tail == null) tail = h; }
        public void action_pop() { if (tail != null) { tail = tail.prev; tail.prev = tail.prev.prev; } else throw Failure.One; }
    }
}
