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
using Aha.Engine_;

namespace AhaMain
{
    public partial class Console : Form
    {
        delegate opaque_Job<tpar_Event> func_Output<tpar_Event>(string text);

        struct BehaviorParams<tpar_Event> : icomp_BehaviorParams<tpar_Event>
        {
            public func_Output<tpar_Event> field_output;
            public string field_settings;
            public icomp_Engine<tpar_Event> field_engine;
            public bool attr_settings(out IahaArray<char> result) { result = new AhaString(field_settings); return true; }
            public bool attr_password(out IahaArray<char> result) { result = new AhaString(""); return true; } //TODO
            public opaque_Job<tpar_Event> fattr_output(IahaArray<char> text) { return field_output(new string(text.get())); }
            public bool attr_engine(out icomp_Engine<tpar_Event> result) { result = field_engine; return true; }
        }

        private Assembly assembly;
        private object app;
        private Type eventType;
        private object b;
        private Type appType;
        private Type engType;
        private object eng;
        private bool running;
        private string title;
        private Queue<string> messages = new Queue<string>();
        private void output(string text) { messages.Enqueue(text); }

        public Console()
        {
            InitializeComponent();
        }

        private void PopulateApplications()
        {
            Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("Software").OpenSubKey("Aha! Factor").OpenSubKey("Aha! for .NET").OpenSubKey("ConsoleApp");
            string[] names = key.GetSubKeyNames();
            foreach (string name in names)
            {
                ToolStripDropDownItem item = new ToolStripDropDownButton(name, null, item_Click); 
                toolStripMenuItem1.DropDownItems.Add(item);
            }
        }

        private void item_Click(Object sender, EventArgs e)
        {

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
            toolStripStatusLabel1.Text = "Running application '" + title + "'";
            textBox1.ReadOnly = false;
            textBox1.Focus();
            running = true;
            // get application's behavior
            object[] args = new object[2];
            appType.InvokeMember
                (
                    "attr_Behavior",
                    System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.InvokeMethod,
                    null,
                    app,
                    args
                );
            engType.InvokeMember
                (
                    "StartExternal",
                    System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.InvokeMethod,
                    null,
                    eng,
                    new object[] { args[1] }
                );
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
            //if (running) while (eng.Trace().Count > 0) listBox2.Items.Add(DateTime.Now.TimeOfDay.ToString() + ": " + eng.Trace().Dequeue());
            bool term = (Boolean)engType.InvokeMember
                (
                    "Terminated",
                    System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.InvokeMethod,
                    null,
                    eng,
                    new object[] { }
                );
            if (running && term)
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
            engType.InvokeMember
                (
                    "StopExternal",
                    System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.InvokeMethod,
                    null,
                    eng,
                    new object[] { }
                );
        }

        private void Console_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (eng != null)
            {
                engType.InvokeMember
                    (
                        "StopExternal",
                        System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.InvokeMethod,
                        null,
                        eng,
                        new object[] { }
                    );
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                eng = null;
                assembly = Assembly.LoadFrom(openFileDialog1.FileName);
                try
                {
                    eventType = assembly.GetType("opaque_Event");
                    Type exportType = assembly.GetType("export");
                    object export = Activator.CreateInstance(exportType);
                    app = exportType.GetField("value").GetValue(export);
                    appType = typeof(imod_Application<>).MakeGenericType(new Type[] { eventType });
                    engType = typeof(comp_Engine<>).MakeGenericType(new Type[] { eventType });
                    eng = Activator.CreateInstance(engType);
                    // get application's title
                    object[] args = new object[1];
                    appType.InvokeMember(
                        "attr_Title",
                        System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.InvokeMethod,
                        null,
                        app,
                        args);
                    title = new string(((IahaArray<char>)args[0]).get());
                    toolStripStatusLabel1.Text = "Enter application settings for '" + title + "'";
                    textBox2.ReadOnly = false;
                    textBox2.Focus();
                }
                catch (System.Exception)
                {
                    toolStripStatusLabel1.Text = "Error: assembly doesn't contain an Aha! application";
                }
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
