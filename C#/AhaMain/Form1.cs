using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using API;
using BaseLibrary;
using AhaCore;
using Engine;

namespace AhaMain
{
    public partial class Console : Form
    {
        delegate module_Jobs<API.Application.Event>.opaque_Job Output(string text);

        struct BehaviorParams : API.Application.icomp_BehaviorParams
        {
            public Output outp;
            public module_Jobs<API.Application.Event>.icomp_Engine eng;
            public IahaArray<char> attr_settings() { return new AhaString(""); }
            public module_Jobs<API.Application.Event>.opaque_Job fattr_output(IahaArray<char> text) { return outp(new string(text.get())); }
            public module_Jobs<API.Application.Event>.icomp_Engine attr_engine() { return eng; }
        }

        private API.Application app = new API.Application();
        private module_Jobs<API.Application.Event>.iobj_Behavior b;
        private Engine<API.Application.Event> eng;
        private List<string> messages = new List<string>();
        private ListViewItem[] cache;
        private int firstItem; //stores the index of the first item in the cache
        private void output(string text) { messages.Add(text); listView1.VirtualListSize = listView1.VirtualListSize + 1; }
        public Console()
        {
            InitializeComponent();
        }

        private void perform() 
        { 
            module_Jobs<API.Application.Event>.opaque_Job[] jobs = b.state().get(); 
            try 
            { 
                for (int i = 0; i < jobs.Length; i++) 
                { 
                    module_Jobs<API.Application.Event>.opaque_Job job = jobs[i]; 
                    job(); 
                } 
            } 
            catch (System.Exception) { System.Windows.Forms.Application.Exit(); } 
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r') { b.action_handle(app.fattr_Receive(new AhaString(((TextBox)sender).Text))); perform(); }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            BehaviorParams bp = new BehaviorParams { outp = delegate(string text) { return delegate() { output(text); }; }, eng = eng };
            b = app.fattr_Behavior(bp);
            eng = new Engine<API.Application.Event>(b, perform);
            this.Text = new string(app.attr_Title().get());
            perform();

        }

        private void listView1_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
        {
            if (cache != null && e.ItemIndex >= firstItem && e.ItemIndex < firstItem + cache.Length)
            {
                //A cache hit, so get the ListViewItem from the cache instead of making a new one.
                e.Item = cache[e.ItemIndex - firstItem];
            }
            else
            {
                //A cache miss, so create a new ListViewItem and pass it back.
                int x = e.ItemIndex * e.ItemIndex;
                e.Item = new ListViewItem(x.ToString());
            }
        }

        private void listView1_CacheVirtualItems(object sender, CacheVirtualItemsEventArgs e)
        {
            //We've gotten a request to refresh the cache.
            //First check if it's really neccesary.
            if (cache != null && e.StartIndex >= firstItem && e.EndIndex <= firstItem + cache.Length)
            {
                //If the newly requested cache is a subset of the old cache, 
                //no need to rebuild everything, so do nothing.
                return;
            }

            //Now we need to rebuild the cache.
            firstItem = e.StartIndex;
            int length = e.EndIndex - e.StartIndex + 1; //indexes are inclusive
            cache = new ListViewItem[length];

            //Fill the cache with the appropriate ListViewItems.
            for (int i = 0; i < length; i++)
            {
                cache[i] = new ListViewItem(messages[i + firstItem]);
            }
        }

    }
}
