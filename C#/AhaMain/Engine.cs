using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using AhaCore;
using BaseLibrary;
using API;

namespace Engine
{
    public class comp_Engine<tpar_Event> : API.Jobs.icomp_Engine<tpar_Event, API.Jobs.Implementation.opaque_Job>
    {
        public delegate void Terminate();

        private struct Today : BaseLibrary.module_Time.icomp_DateStruc
        {
            public Int64 attr_year() { return DateTime.Now.Year; }
            public Int64 attr_month() { return DateTime.Now.Month; }
            public Int64 attr_day() { return DateTime.Now.Day; }
        }

        private bool field_terminate;
        private BaseLibrary.module_Time.opaque_Timestamp today;
        private DateTime midnight = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
        private API.Jobs.iobj_Behavior<tpar_Event, API.Jobs.Implementation.opaque_Job> field_behavior;
        private BaseLibrary.module_Time nick_Time = new BaseLibrary.module_Time();
        private List<Thread> threads = new List<Thread>();
        private Thread workthread;
        private AutoResetEvent recv = new AutoResetEvent(false);
        private Queue<tpar_Event> events = new Queue<tpar_Event>();
        private BaseLibrary.module_Time.opaque_Timestamp curr()
        {
            TimeSpan time = DateTime.Now - midnight;
            return nick_Time.op_Timestamp_Plus_Interval(today, nick_Time.op__interval_integer(time.Ticks));
        }
        private void perform()
        {
            foreach (API.Jobs.Implementation.opaque_Job job in field_behavior.state().get()) job();
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
        public void HandleExternal(tpar_Event e) { field_behavior.action_handle(e); recv.Set(); } //handle external event (such as user input)
        public bool Terminated() { return field_terminate; }
        public API.Jobs.Implementation.opaque_Job fattr_raise(tpar_Event e) 
        {
            return delegate() { events.Enqueue(e); recv.Set(); }; //put event in queue and signal
        }
        public API.Jobs.Implementation.opaque_Job fattr_run(API.Jobs.Implementation.opaque_Job job) 
        { 
            return delegate() { Thread thread = new Thread(new ThreadStart(job)); threads.Add(thread); thread.Start(); }; 
        }
        public API.Jobs.Implementation.opaque_Job fattr_enquireTime(API.Jobs.func_EnquireTime<tpar_Event> enq) 
        { 
            return delegate() { field_behavior.action_handle(enq(curr())); recv.Set(); }; 
        }
        public API.Jobs.Implementation.opaque_Job fattr_delay(module_Time.opaque_Interval interval, tpar_Event e) 
        {
            API.Jobs.Implementation.opaque_Job job = delegate() { Thread.Sleep(new TimeSpan(interval.ticks)); field_behavior.action_handle(e); perform(); }; 
            return delegate() { Thread thread = new Thread(new ThreadStart(job)); threads.Add(thread); thread.Start(); }; 
        }
        public API.Jobs.Implementation.opaque_Job fattr_stop() 
        { 
            return delegate() { foreach (Thread thread in threads) if (thread.IsAlive) thread.Abort(); threads.Clear(); }; 
        }
        public void StopExternal() { fattr_stop()(); workthread.Abort(); }
        public comp_Engine(API.Jobs.iobj_Behavior<tpar_Event, API.Jobs.Implementation.opaque_Job> param_behavior)
        {
            field_behavior = param_behavior;
            today = nick_Time.op__date_DateStruc(new Today());
            workthread = new Thread(new ThreadStart(work));
            workthread.Start();
        }
    }

}
