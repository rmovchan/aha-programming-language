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
        delegate module_Jobs<API.module_Application.opaque_Event>.opaque_Job func_Output(string text);

        struct BehaviorParams : API.module_Application.icomp_BehaviorParams
        {
            public func_Output field_output;
            public module_Jobs<API.module_Application.opaque_Event>.icomp_Engine field_engine;
            public IahaArray<char> attr_settings() { return new AhaString(""); }
            public module_Jobs<API.module_Application.opaque_Event>.opaque_Job fattr_output(IahaArray<char> text) { return field_output(new string(text.get())); }
            public module_Jobs<API.module_Application.opaque_Event>.icomp_Engine attr_engine() { return field_engine; }
        }

        private API.module_Application app = new API.module_Application();
        private module_Jobs<API.module_Application.opaque_Event>.iobj_Behavior b;
        private comp_Engine<API.module_Application.opaque_Event> eng;
        private bool running;
        private void output(string text) { listBox1.Items.Add(text); }
        public Console()
        {
            InitializeComponent();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r') { b.action_handle(app.fattr_Receive(new AhaString(((TextBox)sender).Text))); ((TextBox)sender).Clear(); eng.perform(); }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            BehaviorParams bp = new BehaviorParams { field_output = delegate(string text) { return delegate() { output(text); }; }, field_engine = eng };
            b = app.fattr_Behavior(bp);
            this.Text = new string(app.attr_Title().get());

        }

        private void terminate()
        {
            toolStripStatusLabel1.Text = "Terminated";
            textBox1.ReadOnly = true;
            running = false;
        }

        private void runToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Running";
            eng = new comp_Engine<API.module_Application.opaque_Event>(b, terminate);
            eng.perform();
            textBox1.ReadOnly = false;
            running = true;
        }

        private void listBox1_MeasureItem(object sender, MeasureItemEventArgs e)
        {
        }

    }
}
