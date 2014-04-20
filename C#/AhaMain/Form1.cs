using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Aha.API;
using Aha.Base;
using Aha.Core;
using Aha.Engine;

namespace AhaMain
{
    public partial class Console : Form
    {
        delegate Aha.API.Jobs.Implementation.opaque_Job func_Output(string text);

        struct BehaviorParams : Aha.API.Application.icomp_BehaviorParams<Aha.API.Application.Implementation.opaque_Event>
        {
            public func_Output field_output;
            public Aha.API.Jobs.icomp_Engine<Aha.API.Application.Implementation.opaque_Event, Aha.API.Jobs.Implementation.opaque_Job> field_engine;
            public IahaArray<char> attr_settings() { return new AhaString(""); }
            public Aha.API.Jobs.Implementation.opaque_Job fattr_output(IahaArray<char> text) { return field_output(new string(text.get())); }
            public Aha.API.Jobs.icomp_Engine<Aha.API.Application.Implementation.opaque_Event, Aha.API.Jobs.Implementation.opaque_Job> attr_engine() { return field_engine; }
        }

        private Aha.API.Application.imod_Application<Aha.API.Application.Implementation.opaque_Event> app;
        private Aha.API.Jobs.iobj_Behavior<Aha.API.Application.Implementation.opaque_Event, Aha.API.Jobs.Implementation.opaque_Job> b;
        private Aha.Engine.comp_Engine<Aha.API.Application.Implementation.opaque_Event> eng;
        private bool running;
        private Queue<string> messages = new Queue<string>();
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

        }

        private void runToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Running";
            textBox1.ReadOnly = false;
            running = true;
            eng.StartExternal(b);
            runToolStripMenuItem.Enabled = false;
            stopToolStripMenuItem.Enabled = true;
        }

        private void listBox1_MeasureItem(object sender, MeasureItemEventArgs e)
        {
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            while (messages.Count > 0) listBox1.Items.Add(messages.Dequeue());
            if (running && eng.Terminated())
            {
                toolStripStatusLabel1.Text = "Terminated";
                textBox1.ReadOnly = true;
                running = false;
                runToolStripMenuItem.Enabled = true;
                stopToolStripMenuItem.Enabled = false;
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
            System.Reflection.Assembly assembly;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                eng = null;
                assembly = System.Reflection.Assembly.LoadFrom(openFileDialog1.FileName);
                foreach (Type type in assembly.ExportedTypes)
                {
                    if (type.IsClass)
                    {
                        try
                        {
                            app = Activator.CreateInstance(type) as Aha.API.Application.imod_Application<Aha.API.Application.Implementation.opaque_Event>;
                            eng = new Aha.Engine.comp_Engine<Aha.API.Application.Implementation.opaque_Event>();
                            BehaviorParams bp = new BehaviorParams { field_output = delegate(string text) { return delegate() { output(text); }; }, field_engine = eng };
                            b = app.fattr_Behavior(bp);
                            this.Text = new string(app.attr_Title().get());
                            toolStripStatusLabel1.Text = "Ready";
                            runToolStripMenuItem.Enabled = true;
                            return;
                        }
                        catch (System.Exception) { }
                    }
                }
                toolStripStatusLabel1.Text = "Assembly doesn't contain an Aha! application";
            }
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
        }

    }
}
