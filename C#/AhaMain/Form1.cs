using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using API;
using BaseLibrary;
using AhaCore;

namespace AhaMain
{
    public partial class Form1 : Form
    {
        delegate void Perform();

        class TheEngine<Event> : Jobs.Engine<Event>
        {
            public Jobs.Behavior<Event> behavior;
            public Perform perform;
            public Jobs.Job raise(Event e) { return delegate() { behavior.handle(e); perform(); }; }
            public Jobs.Job run(Jobs.Job job) { return delegate() { Thread thread = new Thread(new ThreadStart(job)); thread.Start(); }; }
            public Jobs.Job enquireTime(Jobs.EnquireTime<Event> enq) { return delegate() { behavior.handle(enq(DateTime.Now.Ticks)); }; }
            //public Job delay(double interval, Event e) { return delegate() { b.handle(e); }; } //TODO
        }

        class BehaviorParams : IBehaviorParams<IahaVoid>
        {
            public Output outp;
            public Jobs.Engine<IahaVoid> eng;
            public IahaArray<char> settings() { return new AhaString(""); }
            public Jobs.Job output(IahaArray<char> text) { return outp(text); }
            public Jobs.Engine<IahaVoid> engine() { return eng; }
        }

        private API.Application app = new API.Application();
        private Jobs.Behavior<IahaVoid> b;
        private TheEngine<IahaVoid> eng = new TheEngine<IahaVoid>();
        private void output(string text) { textBox2.Lines = new string[]{ text }; }
        public Form1()
        {
            InitializeComponent();
        }

        private void perform() { Jobs.Job[] jobs = b.state().get(); try { for (int i = 0; i < jobs.Length; i++) { Jobs.Job job = jobs[i]; job(); } } catch (System.Exception) { System.Windows.Forms.Application.Exit(); } }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r') { b.handle(app.Receive(new AhaString(((TextBox)sender).Text))); perform(); }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            BehaviorParams bp = new BehaviorParams { outp = delegate(IahaArray<char> text) { return delegate() { output(new string(text.get())); }; }, eng = eng };
            b = app.Behavior(bp);
            eng.behavior = b;
            eng.perform = perform;
            perform();

        }
    }
}
