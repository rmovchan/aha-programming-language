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
            public bool action_replace(icomp_ReplaceParam<Item> param) 
            { 
                long index;
                Item item;
                if (param.attr_index(out index) && index >= 0 && index < list.Count && param.attr_item(out item))
                {
                    list[(int)index] = item;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            public bool action_insert(icomp_InsertParam<Item> param)
            { 
                long index;
                Item item;
                if (param.attr_index(out index) && index >= 0 && index <= list.Count && param.attr_item(out item))
                {
                    list.Insert((int)index, item);
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
            public bool action_push(Item item) 
            { 
                node h = new node(); 
                h.item = item; 
                h.next = head; 
                head = h;
                return true;
            }
            public bool action_pop() 
            {
                if (head != null)
                {
                    head = head.next;
                    return true;
                }
                else
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
            public IahaObject<Item> copy() { obj_Queue clone = new obj_Queue(); node p = head; while (p != null) { clone.action_push(p.item); p = p.next; } return clone; }
            public bool action_push(Item item) 
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
            public bool action_pop() 
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
        }

        public bool attr_DynamicArray(out iobj_DynamicArray<Item> result) { result = new obj_DynamicArray(); return true; }
        public bool attr_Stack(out iobj_DynamicSequence<Item> result) { result = new obj_Stack(); return true; }
        public bool attr_Queue(out iobj_DynamicSequence<Item> result) { result = new obj_Queue(); return true; }
    }
}
