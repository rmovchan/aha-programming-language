using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using Aha.API;
using Aha.API.Application;
using Aha.API.Jobs;
using Aha.Base;
using Aha.Core;
using Aha.Engine;

namespace AhaMain
{
    public partial class Console : Form
    {
        delegate opaque_Job<opaque_Event> func_Output(string text);

        struct BehaviorParams : icomp_BehaviorParams
        {
            public func_Output field_output;
            public string field_settings;
            public icomp_Engine<opaque_Event> field_engine;
            public IahaArray<char> attr_settings() { return new AhaString(field_settings); }
            public opaque_Job<opaque_Event> fattr_output(IahaArray<char> text) { return field_output(new string(text.get())); }
            public icomp_Engine<opaque_Event> attr_engine() { return field_engine; }
        }

        private Assembly assembly;
        private imod_Application app;
        private iobj_Behavior<opaque_Event> b;
        private comp_Engine<opaque_Event> eng;
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
            toolStripStatusLabel1.Text = "Running application '" + new string(app.attr_Title().get()) +"'";
            textBox1.ReadOnly = false;
            textBox1.Focus();
            running = true;
            eng.StartExternal((iobj_Behavior<opaque_Event>)b.copy());
            runToolStripMenuItem.Enabled = false;
            toolStripMenuItem1.Enabled = false;
            stopToolStripMenuItem.Enabled = true;
        }

        private void listBox1_MeasureItem(object sender, MeasureItemEventArgs e)
        {
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            while (messages.Count > 0) listBox1.Items.Add(messages.Dequeue());
            if (running) while (eng.Trace().Count > 0) listBox2.Items.Add(DateTime.Now.TimeOfDay.ToString() + ": " + eng.Trace().Dequeue());
            if (running && eng.Terminated())
            {
                toolStripStatusLabel1.Text = "Terminated";
                textBox1.ReadOnly = true;
                running = false;
                runToolStripMenuItem.Enabled = true;
                toolStripMenuItem1.Enabled = true;
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
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                eng = null;
                assembly = Assembly.LoadFrom(openFileDialog1.FileName);
                foreach (Type type in assembly.ExportedTypes)
                {
                    if (type.IsClass)
                    {
                        try
                        {
                            app = Activator.CreateInstance(type) as imod_Application;
                            if (app != null)
                            {
                                eng = new comp_Engine<opaque_Event>();
                                toolStripStatusLabel1.Text = "Enter application settings for '" + new string(app.attr_Title().get()) + "'";
                                textBox2.ReadOnly = false;
                                textBox2.Focus();
                                return;
                            }
                        }
                        catch (System.Exception) { }
                    }   
                }
                toolStripStatusLabel1.Text = "Error: assembly doesn't contain an Aha! application";
            }
       }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (eng != null) eng.StopExternal();
            Application.Exit();
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r') 
            {
                BehaviorParams bp = new BehaviorParams 
                { 
                    field_output = 
                        delegate(string text) 
                        {
                            return new Aha.API.Jobs.opaque_Job<opaque_Event>
                            {
                                title = "output",
                                execute = delegate() { output(text); }
                            };
                        }, 
                    field_engine = eng, 
                    field_settings = ((TextBox)sender).Text 
                };
                b = app.fattr_Behavior(bp);
                //this.Text = new string(app.attr_Title().get());
                toolStripStatusLabel1.Text = "Ready to run '" + new string(app.attr_Title().get()) + "'";
                runToolStripMenuItem.Enabled = true;
                textBox2.ReadOnly = true;
            }

        }

        private void clearTraceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listBox2.Items.Clear();
        }

    }
}
