using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aha.Core;

namespace Aha.Base
{
    public partial class module_StrUtils : AhaModule
    {
        class obj_StringBuilder : iobj_StringBuilder
        {
            const int block = 1024;
            private List<char[]> list;
            private int count = 0;
            private char[] gather()
            {
                char[] buf = new char[count];
                int j = 0;
                for (int i = 0; i < list.Count - 1; i++) { Array.Copy(list[i], 0, buf, j, block); j += block; }
                if (j != count) Array.Copy(list[list.Count - 1], 0, buf, j, count - j);
                return buf;
            }
            private void split(char[] buf, int index, int length)
            {
                list = new List<char[]>();
                int j = index; //from position
                count = length;
                char[] b;
                for (int i = 0; i < length / block; i++) { b = new char[block]; Array.Copy(buf, j, b, 0, block); j += block; list.Add(b); }
                if (j != index + length) { b = new char[block]; Array.Copy(buf, j, b, 0, index + length - j); list.Add(b); }
            }
            public obj_StringBuilder() { list = new List<char[]>(); }
            public IahaArray<char> state() { return new AhaString(gather()); }
            public IahaObject<IahaArray<char>> copy() { obj_StringBuilder fb = new obj_StringBuilder() { count = count }; for (int i = 0; i < count; i++) { char[] b = new char[block]; list[i].CopyTo(b, 0); fb.list.Add(b); } return fb; }
            public void action_add(char ch) { if (count == list.Count * block) list.Add(new char[block]); list[count / block][count % block] = ch; }
            public void action_append(IahaArray<char> str) 
            { 
                char[] temp = str.get();
                int j = list.Count * block - count; //free positions in last block
                if (j > temp.Length) j = temp.Length; //characters to copy into last block
                if (j != 0) Array.Copy(temp, 0, list[list.Count - 1], count % block, j);
                char[] b;
                while (j + block < temp.Length)
                {
                    b = new char[block];
                    Array.Copy(temp, j, b, 0, block);
                    list.Add(b); 
                    j += block; // j = number of chars copied
                }
                if (j != temp.Length)
                {
                    b = new char[block];
                    Array.Copy(temp, j, b, 0, temp.Length - j); //copy remaining chars
                    list.Add(b);
                }
            }
            public void action_extract(icomp_Substring sub)
            {
                char[] buf = gather();
                split(buf, (int)sub.attr_index(), (int)sub.attr_length());
            }
            public void action_trimSpaces()
            {
                char[] buf = gather();
                int i = 0;
                while ((i < count) && (buf[i] == ' ')) i++; //count leading spaces
                int l = count - i;
                if (i < count) //not all spaces?
                {
                    while (buf[i + l - 1] == ' ') l--; //exclude trailing spaces
                }
                if (l != 0) split(buf, i, l); else list = new List<char[]>();
            }
            public void action_put(icomp_PutParams param) { int at = (int)param.attr_at(); char ch = param.attr_char(); list[at / block][at % block] = ch; }
            public void action_replace(icomp_ReplaceParams param)
            {
                char[] source = gather();
                char[] with = param.attr_with().get();
                IahaArray<icomp_Substring> sub = param.attr_substr();
                int j = 0;
                IahaSequence<icomp_Substring> seq = sub.sort(delegate(icomp_Substring x, icomp_Substring y) { return x.attr_index() <= y.attr_index(); });
                IahaSequence<icomp_Substring> seq2 = (IahaSequence<icomp_Substring>)seq.copy();
                for (int i = 0; i < sub.size(); i++) { j += with.Length - (int)seq2.state().attr_length(); seq2.action_skip(); }
                char[] target = new char[source.Length + j];
                int from = 0;
                int to = 0;
                for (int i = 0; i < sub.size(); i++)
                {
                    Array.Copy(source, from, target, to, (int)seq.state().attr_index() - to); //copy chars between replacements
                    Array.Copy(with, 0, target, (int)seq.state().attr_index() + to - from, with.Length); //copy replace chars
                    from = (int)seq.state().attr_index() + (int)seq.state().attr_length();
                    to += (int)seq.state().attr_index() - to + with.Length; //increment by number of chars copied
                    seq.action_skip();
                }
                Array.Copy(source, from, target, to, source.Length + j - to);
                split(target, 0, source.Length + j);
            }
            //void padL(IPadParams param);
            //void padR(IPadParams param);
            //void apply(Convert conv);
        }
    }
}
