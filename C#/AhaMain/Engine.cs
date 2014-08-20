using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Aha.Core;
using Aha.Base;
//using Aha.API;

namespace Aha.Engine
{
    public class comp_Engine<tpar_Event> : Aha.API.Jobs.icomp_Engine<tpar_Event>
    {
        //delegate void trace(string s);
        private struct Date : Aha.Base.Time.icomp_DateStruc
        {
            public int field_year;
            public int field_month;
            public int field_day;
            public Int64 attr_year() { return field_year; }
            public Int64 attr_month() { return field_month; }
            public Int64 attr_day() { return field_day; }
        }

        private bool field_terminated;
        private Aha.Base.Time.opaque_Timestamp today;
        private DateTime midnight = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
        private Aha.API.Jobs.iobj_Behavior<tpar_Event> field_behavior;
        private Aha.Base.Time.module_Time nick_Time = new Aha.Base.Time.module_Time();
        private List<Thread> threads = new List<Thread>();
        private Thread workthread;
        private AutoResetEvent recv = new AutoResetEvent(false);
        private Queue<tpar_Event> events = new Queue<tpar_Event>();
        private Queue<string> trace = new Queue<string>();
        private SortedList<DateTime, tpar_Event> schedule = new SortedList<DateTime, tpar_Event>();
        private System.Timers.Timer scheduler = new System.Timers.Timer();
        private void scheduler_Elapsed(object sender, System.Timers.ElapsedEventArgs e) 
        {
            trace.Enqueue("TIMER");
            tpar_Event evt = schedule.Values[0]; 
            schedule.RemoveAt(0);
            events.Enqueue(evt); 
            recv.Set();
            if (schedule.Count > 0) scheduleNext(); 
        }
        private void scheduleNext() 
        { 
            DateTime next = schedule.Keys[0];
            scheduler.Interval = (next.Ticks - DateTime.Now.Ticks) * 0.0001;
            scheduler.Enabled = true;
        }
        private Aha.Base.Time.opaque_Timestamp curr()
        {
            TimeSpan time = DateTime.Now - midnight;
            return nick_Time.op_Timestamp_Plus_Interval(today, nick_Time.op__interval_integer(time.Ticks));
        }
        private void perform()
        {
            foreach (Aha.API.Jobs.opaque_Job<tpar_Event> job in field_behavior.state().get()) 
            { 
                trace.Enqueue("JOB " + job.title);
                try
                {
                    job.execute();
                }
                catch(System.Threading.ThreadAbortException)
                {
                    throw;
                }
                catch(System.Exception ex)
                {
                    trace.Enqueue("EXCEPTION " + ex.Message + " IN " + job.title);
                }
            }
        }
        private void work() 
        {
            try
            {
                trace.Enqueue("<<START>>");
                perform(); //perform initial jobs
                while (true) //main event loop
                {
                    if (events.Count == 0)
                    {
                        trace.Enqueue("IDLE");
                    }
                    recv.WaitOne(); //wait events
                    field_behavior.action_handle(events.Dequeue()); //handle event
                    perform(); //perform new jobs
                }
            }
            catch (Failure) 
            {
                trace.Enqueue("FAILED");
            }
            catch (System.Threading.ThreadAbortException)
            {
                trace.Enqueue("ABORTED");
            }
            catch (System.Exception ex) 
            {
                trace.Enqueue("ERROR " + ex.Message);
            }
            finally
            { 
                trace.Enqueue("<<FINISH>>"); 
                field_terminated = true; 
            }
        }
        public void HandleExternal(tpar_Event e) { trace.Enqueue("INPUT"); events.Enqueue(e); recv.Set(); } //handle external event (such as user input)
        public bool Terminated() { return field_terminated; }
        public Queue<string> Trace() { return trace; }
        public Aha.API.Jobs.opaque_Job<tpar_Event> fattr_raise(tpar_Event e) 
        {
            return new API.Jobs.opaque_Job<tpar_Event> 
            { 
                title = "raise", 
                execute = 
                    delegate() 
                    { 
                        events.Enqueue(e); 
                        recv.Set(); 
                    } 
            }; //put event in queue and signal
        }
        //public Aha.API.Jobs.opaque_Job<tpar_Event> fattr_run(Aha.API.Jobs.opaque_Job<tpar_Event> job) 
        //{ 
        //    return delegate() { Thread thread = new Thread(new ThreadStart(job)); threads.Add(thread); thread.Start(); }; 
        //}
        public Aha.API.Jobs.opaque_Job<tpar_Event> fattr_enquireTime(Aha.API.Jobs.func_EnquireTime<tpar_Event> enq) 
        {
            return new API.Jobs.opaque_Job<tpar_Event> 
            { 
                title = "enquireTime", 
                execute = 
                    delegate() 
                    { 
                        events.Enqueue(enq(curr())); 
                        recv.Set(); 
                    } 
            }; 
        }
        public Aha.API.Jobs.opaque_Job<tpar_Event> fattr_delay(Aha.Base.Time.opaque_Interval interval, tpar_Event e) 
        {
            return new API.Jobs.opaque_Job<tpar_Event>
            {
                title = "delay", //TODO: show interval length
                execute =
                    delegate()
                    {
                        schedule.Add(new DateTime(DateTime.Now.Ticks + interval.ticks), e);
                        scheduleNext();
                    }
            };
        }
        public Aha.API.Jobs.opaque_Job<tpar_Event> fattr_schedule(Aha.Base.Time.opaque_Timestamp time, tpar_Event e)
        {
            return new API.Jobs.opaque_Job<tpar_Event>
            {
                title = "schedule", //TODO: show time
                execute = 
                    delegate()
                    {
                        schedule.Add(new DateTime(time.ticks), e);
                        scheduleNext();
                    }
            };
        }
        public Aha.API.Jobs.opaque_Job<tpar_Event> fattr_compute(Aha.API.Jobs.icomp_ComputeParams<tpar_Event> param)
        {
            return new API.Jobs.opaque_Job<tpar_Event>
            {
                title = "compute",
                execute =
                    delegate()
                    {
                        Thread thread = new Thread(new ThreadStart(
                            delegate() 
                            { 
                                tpar_Event e; 
                                try 
                                { 
                                    e = param.fattr_event();
                                } 
                                catch (System.Exception) 
                                { 
                                    e = param.attr_fail();
                                }
                                events.Enqueue(e); 
                            })); 
                        threads.Add(thread); 
                        thread.Start();
                    }
            };
        }
        public Aha.API.Jobs.opaque_Job<tpar_Event> attr_stop() 
        {
            return new API.Jobs.opaque_Job<tpar_Event>
            {
                title = "stop",
                execute =
                    delegate()
                    {
                        schedule.Clear();
                        scheduler.Enabled = false;
                        foreach (Thread thread in threads) if (thread.IsAlive) thread.Abort();
                        threads.Clear();
                    }
            }; 
        }
        public void StartExternal(Aha.API.Jobs.iobj_Behavior<tpar_Event> param_behavior)
        {
            field_behavior = param_behavior;
            workthread = new Thread(new ThreadStart(work));
            workthread.Start();
            field_terminated = false;
        }
        public void StopExternal() 
        {
            //trace.Enqueue("ABORT");
            attr_stop().execute();
            if (workthread != null)
            {
                workthread.Abort();
                workthread = null;
            }
        }
        public comp_Engine()
        {
            today = nick_Time.op__date_DateStruc(new Date { field_year = DateTime.Now.Year, field_month = DateTime.Now.Month, field_day = DateTime.Now.Day });
            scheduler.AutoReset = false;
            scheduler.Elapsed += scheduler_Elapsed;
        }
    }

}
