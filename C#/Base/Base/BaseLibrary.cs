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

    public interface IDynamicArray<Item> : IahaObject<Item>
    {
        void add(Item item);
        void replace(IReplaceParam<Item> param);
        void insert(IReplaceParam<Item> param);
        void delete(Int64 index);
    }

    public struct Collections<Item>
    {
        public IDynamicArray<Item> DynamicArray();
    }
}
