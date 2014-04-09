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
    class Engine<Event> : module_Jobs<Event>.icomp_Engine
    {
        public delegate void Perform();
        struct Today : BaseLibrary.module_Time.icomp_DateStruc
        {
            public Int64 attr_year() { return DateTime.Now.Year; }
            public Int64 attr_month() { return DateTime.Now.Month; }
            public Int64 attr_day() { return DateTime.Now.Day; }
        }

        private BaseLibrary.module_Time.opaque_Timestamp today;
        DateTime date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
        private module_Jobs<Event>.iobj_Behavior behavior;
        private Perform perform;
        BaseLibrary.module_Time.opaque_Timestamp curr()
        {
            TimeSpan time = DateTime.Now - date;
            return nick_Time.op_Timestamp_Plus_Interval(today, nick_Time.op__interval_integer(time.Ticks));
        }
        private BaseLibrary.module_Time nick_Time = new BaseLibrary.module_Time();
        public module_Jobs<Event>.opaque_Job fattr_raise(Event e) { return delegate() { behavior.action_handle(e); perform(); }; }
        public module_Jobs<Event>.opaque_Job fattr_run(module_Jobs<Event>.opaque_Job job) { return delegate() { Thread thread = new Thread(new ThreadStart(job)); thread.Start(); }; }
        public module_Jobs<Event>.opaque_Job fattr_enquireTime(module_Jobs<Event>.func_EnquireTime enq) { return delegate() { behavior.action_handle(enq(curr())); perform(); }; }
        //public Job delay(double interval, Event e) { return delegate() { b.handle(e); }; } //TODO
        public Engine(module_Jobs<Event>.iobj_Behavior b, Perform p)
        {
            behavior = b; perform = p; today = nick_Time.op__date_DateStruc(new Today());
        }
    }

}
