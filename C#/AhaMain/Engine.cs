using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Aha.Core;
using Aha.Base;
using Aha.API;

namespace Aha.Engine
{
    public class comp_Engine<tpar_Event> : Aha.API.Jobs.icomp_Engine<tpar_Event, Aha.API.Jobs.Implementation.opaque_Job>
    {
        public delegate void Terminate();

        private struct Today : Aha.Base.module_Time.icomp_DateStruc
        {
            public Int64 attr_year() { return DateTime.Now.Year; }
            public Int64 attr_month() { return DateTime.Now.Month; }
            public Int64 attr_day() { return DateTime.Now.Day; }
        }

        private bool field_terminate;
        private Aha.Base.module_Time.opaque_Timestamp today;
        private DateTime midnight = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
        private Aha.API.Jobs.iobj_Behavior<tpar_Event, Aha.API.Jobs.Implementation.opaque_Job> field_behavior;
        private Aha.Base.module_Time nick_Time = new Aha.Base.module_Time();
        private List<Thread> threads = new List<Thread>();
        private Thread workthread;
        private AutoResetEvent recv = new AutoResetEvent(false);
        private Queue<tpar_Event> events = new Queue<tpar_Event>();
        private Aha.Base.module_Time.opaque_Timestamp curr()
        {
            TimeSpan time = DateTime.Now - midnight;
            return nick_Time.op_Timestamp_Plus_Interval(today, nick_Time.op__interval_integer(time.Ticks));
        }
        private void perform()
        {
            foreach (Aha.API.Jobs.Implementation.opaque_Job job in field_behavior.state().get()) job();
        }
        private void work() 
        { 
            try
            {
                perform(); //perform initial jobs
                while (true) //main event loop
                { 
                    recv.WaitOne(); //wait events
                    field_behavior.action_handle(events.Dequeue()); //handle events
                    perform(); //perform new jobs
                } 
            }
            catch (System.Exception) { field_terminate = true; }
        }
        public void HandleExternal(tpar_Event e) { events.Enqueue(e); recv.Set(); } //handle external event (such as user input)
        public bool Terminated() { return field_terminate; }
        public Aha.API.Jobs.Implementation.opaque_Job fattr_raise(tpar_Event e) 
        {
            return delegate() { events.Enqueue(e); recv.Set(); }; //put event in queue and signal
        }
        public Aha.API.Jobs.Implementation.opaque_Job fattr_run(Aha.API.Jobs.Implementation.opaque_Job job) 
        { 
            return delegate() { Thread thread = new Thread(new ThreadStart(job)); threads.Add(thread); thread.Start(); }; 
        }
        public Aha.API.Jobs.Implementation.opaque_Job fattr_enquireTime(Aha.API.Jobs.func_EnquireTime<tpar_Event> enq) 
        {
            return delegate() { events.Enqueue(enq(curr())); recv.Set(); }; 
        }
        public Aha.API.Jobs.Implementation.opaque_Job fattr_delay(module_Time.opaque_Interval interval, tpar_Event e) 
        {
            Aha.API.Jobs.Implementation.opaque_Job job = delegate() { Thread.Sleep(new TimeSpan(interval.ticks)); field_behavior.action_handle(e); perform(); }; 
            return delegate() { Thread thread = new Thread(new ThreadStart(job)); threads.Add(thread); thread.Start(); }; 
        }
        public Aha.API.Jobs.Implementation.opaque_Job fattr_stop() 
        { 
            return delegate() { foreach (Thread thread in threads) if (thread.IsAlive) thread.Abort(); threads.Clear(); }; 
        }
        public void StartExternal(Aha.API.Jobs.iobj_Behavior<tpar_Event, Aha.API.Jobs.Implementation.opaque_Job> param_behavior)
        {
            field_behavior = param_behavior;
            workthread = new Thread(new ThreadStart(work));
            workthread.Start();
            field_terminate = false;
        }
        public void StopExternal() 
        { 
            fattr_stop()();
            if (workthread != null)
            {
                workthread.Abort();
                workthread = null;
            }
        }
        public comp_Engine()
        {
            today = nick_Time.op__date_DateStruc(new Today());
        }
    }

}
