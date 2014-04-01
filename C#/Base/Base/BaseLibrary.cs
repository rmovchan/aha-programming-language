using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core;

namespace BaseLibrary
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

    class dynamicArray<Item> : IDynamicArray<Item>
    {
        private List<Item> list;
        public dynamicArray() { list = new List<Item>(); }
        public IahaArray<Item> state() { AhaArray<Item> result = new AhaArray<Item>(list.ToArray()); return result; }
        public void add(Item item) { list.Add(item); }
        public void replace(IReplaceParam<Item> param) { list[(int)param.index()] = param.item(); }
        public void insert(IReplaceParam<Item> param) { list.Insert((int)param.index(), param.item()); }
        public void delete(Int64 index) { list.RemoveAt((int)index); }
    }

    public struct Collections<Item>
    {
        public IDynamicArray<Item> DynamicArray() { return new dynamicArray<Item>(); }
    }
}
