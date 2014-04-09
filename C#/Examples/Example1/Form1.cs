using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AhaCore;

namespace Example1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AhaSeq<Int64> seq = new AhaSeq<Int64> { curr = (Int64)startValue.Value, rule = delegate(Int64 prev) { return prev + 3; } };
            AhaArray<Int64>.Rule rule = delegate(Int64 x) { return x * 4; };
            AhaArray<Int64> arr = new AhaArray<Int64>(rule, (Int64)itemCount.Value);
            AhaSegment arr3 = new AhaSegment((Int64)startValue.Value, (Int64)startValue.Value + (Int64)itemCount.Value);
            AhaArray<Int64> arr2 = new AhaArray<Int64>(new Int64[][] { arr.get(), arr3.get() });
            IahaSequence<Int64> seq2 = arr2.enumerate(delegate(Int64 x) { return x >= 50; });
            listBox1.Items.Clear();
            for (int i = 0; true; i++)
            {
                try 
                { 
                    listBox1.Items.Add(seq2.state().ToString());
                    seq2.action_skip(); 
                }
                catch (System.Exception) { break; }
            }
            Int64 t = arr2.foldl(delegate(Int64 first, Int64 second) { return first + second; });
            total.Text = t.ToString();
        }
    }
}
