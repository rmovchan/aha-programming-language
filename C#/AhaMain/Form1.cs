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
        delegate API.Jobs.Implementation.opaque_Job func_Output(string text);

        struct BehaviorParams : API.Application.icomp_BehaviorParams<API.Application.Implementation.opaque_Event>
        {
            public func_Output field_output;
            public API.Jobs.icomp_Engine<API.Application.Implementation.opaque_Event, API.Jobs.Implementation.opaque_Job> field_engine;
            public IahaArray<char> attr_settings() { return new AhaString(""); }
            public API.Jobs.Implementation.opaque_Job fattr_output(IahaArray<char> text) { return field_output(new string(text.get())); }
            public API.Jobs.icomp_Engine<API.Application.Implementation.opaque_Event, API.Jobs.Implementation.opaque_Job> attr_engine() { return field_engine; }
        }

        private API.Application.imod_Application<API.Application.Implementation.opaque_Event> app;
        private module_Jobs<API.module_Application.opaque_Event>.iobj_Behavior b;
        private comp_Engine<API.module_Application.opaque_Event> eng;
        private bool running;
        private Queue<string> messages = new Queue<string>();
        private System.Reflection.Assembly assembly;
        private void output(string text) { messages.Enqueue(text); }
        public Console()
        {
            InitializeComponent();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r') { eng.HandleExternal(app.fattr_Receive(new AhaString(((TextBox)sender).Text))); ((TextBox)sender).Clear(); }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            BehaviorParams bp = new BehaviorParams { field_output = delegate(string text) { return delegate() { output(text); }; }, field_engine = eng };
            b = app.fattr_Behavior(bp);
            this.Text = new string(app.attr_Title().get());

        }

        private void runToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Running";
            eng = new comp_Engine<API.module_Application.opaque_Event>(b);
            textBox1.ReadOnly = false;
            running = true;
        }

        private void listBox1_MeasureItem(object sender, MeasureItemEventArgs e)
        {
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            while (messages.Count > 0) listBox1.Items.Add(messages.Dequeue());
            if (eng != null && eng.Terminated())
            {
                toolStripStatusLabel1.Text = "Terminated";
                textBox1.ReadOnly = true;
                running = false;
            }
        }

        private void stopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            eng.StopExternal();
        }

        private void Console_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (eng != null) eng.StopExternal();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                assembly = System.Reflection.Assembly.LoadFrom(openFileDialog1.FileName);
            }
        }

    }
}
