using System;
using System.Collections.Generic;
using Aha.Core;

namespace Aha.Base.Collections
{
    public class module_Collections<Item> : AhaModule, imod_Collections<Item>
    {
        class obj_DynamicArray : iobj_DynamicArray<Item>
        {
            private List<Item> list;
            public obj_DynamicArray() { list = new List<Item>(); }
            public obj_DynamicArray(Item[] items) { list = new List<Item>(items); }
            public IahaArray<Item> state() { return new AhaArray<Item>(list.ToArray()); }
            public IahaObject<IahaArray<Item>> copy() { obj_DynamicArray clone = new obj_DynamicArray(list.ToArray()); return clone; }
            public void action_add(Item item) { list.Add(item); }
            public void action_replace(icomp_ReplaceParam<Item> param) { list[(int)param.attr_index()] = param.attr_item(); }
            public void action_insert(icomp_InsertParam<Item> param) { list.Insert((int)param.attr_index(), param.attr_item()); }
            public void action_delete(Int64 index) { list.RemoveAt((int)index); }
        }

        class obj_Stack : iobj_DynamicSequence<Item>
        {
            class node
            {
                public Item item;
                public node next;
            }
            private node head = null;
            public Item state() { if (head != null) return head.item; else throw Failure.One; }
            public IahaObject<Item> copy() { obj_Stack clone = new obj_Stack() { head = head }; return clone; }
            public void action_push(Item item) { node h = new node(); h.item = item; h.next = head; head = h; }
            public void action_pop() { if (head != null) head = head.next; else throw Failure.One; }
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
            public Item state() { if (tail != null) return tail.item; else throw Failure.One; }
            public IahaObject<Item> copy() { obj_Queue clone = new obj_Queue(); node p = head; while (p != null) { clone.action_push(p.item); p = p.next; } return clone; }
            public void action_push(Item item) { node h = new node(); h.item = item; h.next = head; h.prev = null; head = h; if (tail == null) tail = h; }
            public void action_pop() { if (tail != null) { tail = tail.prev; tail.prev = tail.prev.prev; } else throw Failure.One; }
        }

        public iobj_DynamicArray<Item> attr_DynamicArray() { return new obj_DynamicArray(); }
        public iobj_DynamicSequence<Item> attr_Stack() { return new obj_Stack(); }
        public iobj_DynamicSequence<Item> attr_Queue() { return new obj_Queue(); }
    }
}
