using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Aha.Core;
using Aha.Base;
//using Aha.API;

namespace Aha.Engine_
{
    struct comp_Version : Aha.API.Environment.icomp_Version
    {
        public bool attr_major(out long result) { result = 0; return true; }
        public bool attr_minor(out long result) { result = 1; return true; }
        public bool attr_build(out long result) { result = 1; return true; }
    }

    struct comp_Framework : Aha.API.Environment.icomp_Framework
    {
        public bool attr_name(out IahaArray<char> result) { result = new AhaString("Aha! for .NET"); return true; }
        public bool attr_version(out Aha.API.Environment.icomp_Version result) { result = new comp_Version(); return true; }
        public bool attr_components(out IahaArray<IahaArray<char>> result) { result = new AhaArray<IahaArray<char>>(new IahaArray<char>[] { }); return true; }
    }

    struct comp_Platform : Aha.API.Environment.icomp_Platform
    {
        public bool attr_Windows() { return true; }
        public bool attr_MacOSX() { return false; }
        public bool attr_Linux() { return false; }
        public bool attr_FreeBSD() { return false; }
        public bool attr_iOS() { return false; }
        public bool attr_Android() { return false; }
        public bool attr_other() { return false; }
    }

    class comp_Locale : Aha.API.Environment.icomp_Locale
    {
        private Aha.Base.Time.opaque_Interval field_GMToffset;

        public bool attr_GMToffset(out Aha.Base.Time.opaque_Interval result) { result = field_GMToffset; return true; }
        public bool attr_country(out IahaArray<char> result) { result = new AhaString(System.Globalization.CultureInfo.CurrentCulture.EnglishName); return true; }
        public bool attr_language(out IahaArray<char> result) { result = new AhaString(System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName); return true; }
        public bool attr_currency(out IahaArray<char> result) { result = new AhaString(System.Globalization.CultureInfo.CurrentCulture.NumberFormat.CurrencySymbol); return true; }
        public bool attr_decimal(out char result) { result = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator[0]; return true; }
        //format: @Format!Format "formatting routines"
        //deformat: @Format!Deformat "deformatting routines"
        //charCompare: @StrUtils!CharCompare "character comparison function"
        public bool fattr_upper(IahaArray<char> param_str, out IahaArray<char> result) { result = new AhaString((new string(param_str.get())).ToUpper(System.Globalization.CultureInfo.CurrentCulture)); return true; }
        public bool fattr_lower(IahaArray<char> param_str, out IahaArray<char> result) { result = new AhaString((new string(param_str.get())).ToLower(System.Globalization.CultureInfo.CurrentCulture)); return true; }
        public bool fattr_sameText(IahaArray<char> param_first, IahaArray<char> param_second)
        {
            return String.Compare(
                new string(param_first.get()),
                new string(param_second.get()),
                StringComparison.CurrentCultureIgnoreCase) == 0;
        }
        public bool op__str_integer(long param_int, out IahaArray<char> result)
        {
            result = new AhaString(param_int.ToString());
            return true;
        }
        //    (~str @Float!Float): { @Float!Float -> String } "convert Float to string (local format)"
        public bool op__str_Float(Aha.Base.Math.opaque_Float param_float, out IahaArray<char> result)
        {
            result = new AhaString(param_float.value.ToString());
            return true;
        }
        //    (~str @Time!Timestamp): { @Time!Timestamp -> String } "convert Timestamp to string (local format)"
        public bool op__str_Timestamp(Aha.Base.Time.opaque_Timestamp param_timestamp, out IahaArray<char> result)
        {
            DateTime dt = new DateTime(param_timestamp.ticks);
            result = new AhaString(dt.ToString());
            return true;
        }
        //    (~str @Money!Money): { @Money!Money -> String } "convert Money to string (local format)"
        //    (~int String): { String -> integer } "convert string to integer"
        public bool op__int_String(IahaArray<char> param_str, out long result)
        {
            result = Convert.ToInt64(new string(param_str.get()));
            return true;
        }
        //    (~float String): { String -> @Float!Float } "convert string (local format) to Float"
        public bool op__float_String(IahaArray<char> param_str, out Aha.Base.Math.opaque_Float result)
        {
            result = new Aha.Base.Math.opaque_Float() { value = Convert.ToDouble(new string(param_str.get())) };
            return true;
        }
        //    (~date String): { String -> @Time!Timestamp } "convert string (local format) to date"
        public bool op__date_String(IahaArray<char> param_str, out Aha.Base.Time.opaque_Timestamp result)
        {
            result = new Aha.Base.Time.opaque_Timestamp() { ticks = Convert.ToDateTime(new string(param_str.get())).Date.Ticks };
            return true;
        }
        //    (~time String): { String -> @Time!Interval } "convert string (local format) to time"
        public bool op__time_String(IahaArray<char> param_str, out Aha.Base.Time.opaque_Interval result)
        {
            result = new Aha.Base.Time.opaque_Interval() { ticks = Convert.ToDateTime(new string(param_str.get())).TimeOfDay.Ticks };
            return true;
        }
        //    (~timestamp String): { String -> @Time!Timestamp } "convert string (local format) to timestamp"
        //    (~money String): { String -> @Money!Money } "convert string (local format) to Money"
        //    (String <= String): { String, String } "compare string in local sorting order"
        public bool op_String_LessEqual_String(IahaArray<char> param_first, IahaArray<char> param_second)
        {
            return String.Compare(
                new string(param_first.get()),
                new string(param_second.get()),
                StringComparison.CurrentCulture) <= 0;
        }
        //    (String < String): { String, String } "compare string in local sorting order"
        public bool op_String_Less_String(IahaArray<char> param_first, IahaArray<char> param_second)
        {
            return String.Compare(
                new string(param_first.get()),
                new string(param_second.get()),
                StringComparison.CurrentCulture) < 0;
        }
        public bool op_String_Equal_String(IahaArray<char> param_first, IahaArray<char> param_second)
        {
            return String.Compare(
                new string(param_first.get()),
                new string(param_second.get()),
                StringComparison.CurrentCulture) == 0;
        }
        public bool op_String_NotEqual_String(IahaArray<char> param_first, IahaArray<char> param_second)
        {
            return String.Compare(
                new string(param_first.get()),
                new string(param_second.get()),
                StringComparison.CurrentCulture) != 0;
        }
        //    (String > String): { String, String } "compare string in local sorting order"
        public bool op_String_Greater_String(IahaArray<char> param_first, IahaArray<char> param_second)
        {
            return String.Compare(
                new string(param_first.get()),
                new string(param_second.get()),
                StringComparison.CurrentCulture) > 0;
        }
        //    (String >= String): { String, String } "compare string in local sorting order"
        public bool op_String_GreateEqual_String(IahaArray<char> param_first, IahaArray<char> param_second)
        {
            return String.Compare(
                new string(param_first.get()),
                new string(param_second.get()),
                StringComparison.CurrentCulture) >= 0;
        }
        //    (FilePath = FilePath): { FilePath, FilePath } "are paths the same?"
        public bool op_FilePath_Equal_FilePath(Aha.API.Environment.opaque_FilePath param_first, Aha.API.Environment.opaque_FilePath param_second)
        {
            return String.Compare(
                param_first.value,
                param_second.value,
                StringComparison.InvariantCultureIgnoreCase) == 0;
        }
        //    (DirPath = DirPath): { DirPath, DirPath } "are paths the same?"
        public bool op_DirPath_Equal_DirPath(Aha.API.Environment.opaque_DirPath param_first, Aha.API.Environment.opaque_DirPath param_second)
        {
            return String.Compare(
                param_first.value,
                param_second.value,
                StringComparison.InvariantCultureIgnoreCase) == 0;
        }
        public comp_Locale()
        {
            DateTime now = DateTime.Now;
            field_GMToffset = new Aha.Base.Time.opaque_Interval { ticks = now.Ticks - now.ToUniversalTime().Ticks };
        }
    }

    class comp_FileSystem : Aha.API.Environment.icomp_FileSystem
    {
        public bool attr_eol(out IahaArray<char> result) { result = new AhaString("\r\n"); return true; }
        //splitLines: { character* -> String* } "convert sequence of chars to sequence of lines"
        //joinLines: { String* -> character* } "convert sequence of lines to sequence of chars"
        public bool fattr_filePath(Aha.API.Environment.opaque_DirPath param_dir, IahaArray<char> param_name, out Aha.API.Environment.opaque_FilePath result)
        { result = new Aha.API.Environment.opaque_FilePath { value = param_dir.value + System.IO.Path.PathSeparator + (new string(param_name.get())) }; return true; }
        public bool fattr_subDirPath(Aha.API.Environment.opaque_DirPath param_dir, IahaArray<char> param_name, out Aha.API.Environment.opaque_DirPath result)
        { result = new Aha.API.Environment.opaque_DirPath { value = param_dir.value + System.IO.Path.PathSeparator + (new string(param_name.get())) }; return true; }
        public bool fattr_parentDirPath(Aha.API.Environment.opaque_DirPath param_dir, out Aha.API.Environment.opaque_DirPath result)
        { result = new Aha.API.Environment.opaque_DirPath { value = System.IO.Path.GetDirectoryName(param_dir.value) }; return true; }
        public bool fattr_fileName(Aha.API.Environment.opaque_FilePath param_path, out IahaArray<char> result)
        { result = new AhaString(System.IO.Path.GetFileName(param_path.value)); return true; }
        public bool fattr_fileDir(Aha.API.Environment.opaque_FilePath param_path, out Aha.API.Environment.opaque_DirPath result)
        { result = new Aha.API.Environment.opaque_DirPath { value = System.IO.Path.GetDirectoryName(param_path.value) }; return true; }
        public bool fattr_fileExt(Aha.API.Environment.opaque_FilePath param_path, out IahaArray<char> result)
        { result = new AhaString(System.IO.Path.GetExtension(param_path.value)); return true; }
        public bool fattr_changeExt(Aha.API.Environment.opaque_FilePath param_dir, IahaArray<char> param_ext, out Aha.API.Environment.opaque_FilePath result)
        { result = new Aha.API.Environment.opaque_FilePath { value = System.IO.Path.ChangeExtension(param_dir.value, new string(param_ext.get())) }; return true; }
        public bool fattr_splitDirPath(Aha.API.Environment.opaque_DirPath param_path, out IahaArray<IahaArray<char>> result)
        {
            List<int> list = new List<int>();
            int j = 0;
            while (j != -1)
            {
                j = param_path.value.IndexOf(System.IO.Path.PathSeparator, j, param_path.value.Length);
                if (j == -1) break;
                list.Add(j);
                j++;
            }
            IahaArray<char>[] temp = new IahaArray<char>[list.Count + 1];
            j = 0;
            char[] seg;
            for (int i = 0; i < temp.Length; i++)
            {
                seg = new char[list[i] - j];
                Array.Copy(param_path.value.ToCharArray(), j, seg, 0, list[i] - j);
                temp[i] = new AhaString(seg);
                j = list[i] + 1;

            }
            seg = new char[param_path.value.Length - j];
            Array.Copy(param_path.value.ToCharArray(), j, seg, 0, param_path.value.Length - j);
            temp[list.Count] = new AhaString(seg);
            result = new AhaArray<IahaArray<char>>(temp);
            return true;
        }
        public bool fattr_buildDirPath(IahaArray<IahaArray<char>> param_parts, out Aha.API.Environment.opaque_DirPath result)
        {
            string[] paths = new string[param_parts.size()];
            for (int i = 0; i < param_parts.size(); i++)
            {
                IahaArray<char> path;
                if (param_parts.at(i, out path))
                    paths[i] = new string(path.get());
                else
                {
                    result = default(Aha.API.Environment.opaque_DirPath);
                    return false;
                }
            }
            result = new Aha.API.Environment.opaque_DirPath { value = System.IO.Path.Combine(paths) };
            return true;
        }
        public bool fattr_workingDir(out Aha.API.Environment.opaque_DirPath result) { result = new Aha.API.Environment.opaque_DirPath { value = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData) }; return true; }
        public bool fattr_appDir(out Aha.API.Environment.opaque_DirPath result) { result = new Aha.API.Environment.opaque_DirPath { value = System.Reflection.Assembly.GetEntryAssembly().Location }; return true; }
        public bool fattr_rootDir(out Aha.API.Environment.opaque_DirPath result) { result = new Aha.API.Environment.opaque_DirPath { value = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop) }; return true; }
    }

    public delegate void func_Trace(string text);

    public class comp_Engine<tpar_Event> : Aha.API.Jobs.icomp_Engine<tpar_Event>
    {
        private struct Date : Aha.Base.Time.icomp_DateStruc
        {
            public int field_year;
            public int field_month;
            public int field_day;
            public bool attr_year(out long result) { result = field_year; return true; }
            public bool attr_month(out long result) { result = field_month; return true; }
            public bool attr_day(out long result) { result = field_day; return true; }
        }

        private bool field_terminated;
        private bool field_enabled;
        private bool field_suspended;
        private func_Trace field_trace;
        private Aha.Base.Time.opaque_Timestamp today;
        private DateTime midnight = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
        private Aha.API.Jobs.iobj_Behavior<tpar_Event> field_behavior;
        private Aha.Base.Time.module_Time nick_Time = new Aha.Base.Time.module_Time();
        private List<Thread> threads = new List<Thread>();
        private Thread workthread;
        private AutoResetEvent recv = new AutoResetEvent(false);
        private AutoResetEvent resumed = new AutoResetEvent(false);
        private Queue<tpar_Event> events = new Queue<tpar_Event>();
        //private Queue<string> field_trace = new Queue<string>();
        private void trace(string s) { if (field_trace != null) field_trace(s); }
        private SortedList<DateTime, Aha.API.Jobs.opaque_Job<tpar_Event>> schedule = new SortedList<DateTime, Aha.API.Jobs.opaque_Job<tpar_Event>>();
        private System.Timers.Timer scheduler = new System.Timers.Timer();
        private void scheduler_Elapsed(object sender, System.Timers.ElapsedEventArgs e) 
        {
            Aha.API.Jobs.opaque_Job<tpar_Event> job = schedule.Values[0]; 
            schedule.RemoveAt(0);
            trace("-->> " + job.title);
            job.execute(); 
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
            return new Base.Time.opaque_Timestamp { ticks = today.ticks + time.Ticks };
        }
        private void perform()
        {
            IahaArray<API.Jobs.opaque_Job<tpar_Event>> jobs;
            if (!field_behavior.state(out jobs)) throw (Failure.One);
            foreach (Aha.API.Jobs.opaque_Job<tpar_Event> job in jobs.get()) 
            {
                if (field_suspended) resumed.WaitOne();
                trace("JOB " + job.title);
                try
                {
                    job.execute();
                }
                catch(System.Exception ex)
                {
                    trace("EXCEPTION " + ex.Message + " IN " + job.title);
                }
            }
        }
        private void work() 
        {
            try
            {
                trace("<<START>>");
                field_terminated = false;
                field_enabled = false;
                try
                {
                    perform(); //perform initial jobs
                    while (field_enabled || events.Count != 0) //main event loop
                    {
                        try
                        {
                            if (events.Count == 0)
                            {
                                trace("IDLE");
                            }
                            recv.WaitOne(); //wait events
                            if (!field_behavior.action_handle(events.Dequeue())) throw(Failure.One); //handle event
                            perform(); //perform new jobs
                        }
                        catch (System.Threading.ThreadAbortException)
                        {
                            trace("ABORTED");
                            return;
                        }
                    }
                }
                catch (System.Threading.ThreadAbortException)
                {
                    trace("ABORTED");
                    return;
                }
            }
            catch (Failure) 
            {
                trace("FAILED");
            }
            catch (System.Exception ex) 
            {
                trace("ERROR " + ex.Message);
            }
            finally
            {
                recv.Reset();
                resumed.Reset();
                events.Clear();
                trace("<<FINISH>>");
                field_enabled = false;
                field_terminated = true;
            }
        }

        public void HandleExternal(tpar_Event e) { if (field_enabled) { trace("INPUT"); events.Enqueue(e); recv.Set(); } } //handle external event (such as user input)
        public void Suspend() { field_suspended = true; trace("SUSPENDED"); }
        public void Resume() { field_suspended = false; trace("RESUMED"); resumed.Set(); }
        public bool Terminated() { return field_terminated; }
        public bool Enabled() { return field_enabled; }
        //public void SetTrace(func_Trace trace) { field_trace = trace; }
        
        // Interface members:
        public bool attr_framework(out Aha.API.Environment.icomp_Framework result) { result = new comp_Framework(); return true; }
        public bool attr_platform(out Aha.API.Environment.icomp_Platform result) { result = new comp_Platform(); return true; }
        public bool attr_locale(out Aha.API.Environment.icomp_Locale result) { result = new comp_Locale(); return true; }
        public bool attr_fileSystem(out Aha.API.Environment.icomp_FileSystem result) { result = new comp_FileSystem(); return true; }
        public bool fattr_raise(tpar_Event e, out Aha.API.Jobs.opaque_Job<tpar_Event> result) 
        {
            result = new API.Jobs.opaque_Job<tpar_Event> 
            { 
                title = "raise", 
                execute = 
                    delegate() 
                    { 
                        events.Enqueue(e); 
                        recv.Set(); 
                    } 
            }; //put event in queue and signal
            return true;
        }
        //public Aha.API.Jobs.opaque_Job<tpar_Event> fattr_run(Aha.API.Jobs.opaque_Job<tpar_Event> job) 
        //{ 
        //    return delegate() { Thread thread = new Thread(new ThreadStart(job)); threads.Add(thread); thread.Start(); }; 
        //}
        public bool fattr_enquireTime(Aha.API.Jobs.func_EnquireTime<tpar_Event> enq, out Aha.API.Jobs.opaque_Job<tpar_Event> result) 
        {
            result = new API.Jobs.opaque_Job<tpar_Event> 
            { 
                title = "enquireTime", 
                execute = 
                    delegate() 
                    { 
                        events.Enqueue(enq(curr())); 
                        recv.Set(); 
                    } 
            };
            return true;
        }
        public bool fattr_delay(Aha.Base.Time.opaque_Interval interval, Aha.API.Jobs.opaque_Job<tpar_Event> job, out Aha.API.Jobs.opaque_Job<tpar_Event> result) 
        {
            result = new API.Jobs.opaque_Job<tpar_Event>
            {
                title = "delay", //TODO: show interval length
                execute =
                    delegate()
                    {
                        schedule.Add(new DateTime(DateTime.Now.Ticks + interval.ticks), job);
                        scheduleNext();
                    }
            };
            return true;
        }
        public bool fattr_schedule(Aha.Base.Time.opaque_Timestamp time, Aha.API.Jobs.opaque_Job<tpar_Event> job, out Aha.API.Jobs.opaque_Job<tpar_Event> result)
        {
            result = new API.Jobs.opaque_Job<tpar_Event>
            {
                title = "schedule", //TODO: show time
                execute = 
                    delegate()
                    {
                        schedule.Add(new DateTime(time.ticks), job);
                        scheduleNext();
                    }
            };
            return true;
        }
        public bool fattr_compute(Aha.API.Jobs.icomp_ComputeParams<tpar_Event> param, out Aha.API.Jobs.opaque_Job<tpar_Event> result)
        {
            result = new API.Jobs.opaque_Job<tpar_Event>
            {
                title = "compute",
                execute =
                    delegate()
                    {
                        Thread thread = new Thread(new ThreadStart(
                            delegate() 
                            { 
                                tpar_Event e; 
                                if (!param.fattr_event(out e)) param.attr_fail(out e);
                                events.Enqueue(e); 
                            })); 
                        threads.Add(thread); 
                        thread.Start();
                    }
            };
            return true;
        }
        public bool fattr_log(IahaArray<char> message, out Aha.API.Jobs.opaque_Job<tpar_Event> result)
        {
            result = new API.Jobs.opaque_Job<tpar_Event>
            {
                title = ">>> " + message.get(),
                execute =
                    delegate()
                    {
                    }
            };
            return true;
        }
        public bool attr_break(out Aha.API.Jobs.opaque_Job<tpar_Event> result) 
        {
            result = new API.Jobs.opaque_Job<tpar_Event>
            {
                title = "break",
                execute =
                    delegate()
                    {
                        schedule.Clear();
                        scheduler.Enabled = false;
                        foreach (Thread thread in threads) if (thread.IsAlive) thread.Abort();
                        threads.Clear();
                        if (workthread != null) workthread.Abort();
                    }
            };
            return true;
        }
        public bool attr_enable(out Aha.API.Jobs.opaque_Job<tpar_Event> result)
        {
            result = new API.Jobs.opaque_Job<tpar_Event>
            {
                title = "enable",
                execute =
                    delegate()
                    {
                        field_enabled = true;
                    }
            };
            return true;
        }
        public bool attr_disable(out Aha.API.Jobs.opaque_Job<tpar_Event> result)
        {
            result = new API.Jobs.opaque_Job<tpar_Event>
            {
                title = "disable",
                execute =
                    delegate()
                    {
                        field_enabled = false;
                    }
            };
            return true;
        }
        public void StartExternal(Aha.API.Jobs.iobj_Behavior<tpar_Event> param_behavior, func_Trace trace)
        {
            field_behavior = param_behavior;
            field_trace = trace;
            workthread = new Thread(new ThreadStart(work));
            workthread.Start();
            field_terminated = false;
            field_suspended = false;
            field_enabled = false;
        }
        public void StopExternal() 
        {
            //trace.Enqueue("ABORT");
            Aha.API.Jobs.opaque_Job<tpar_Event> job;
            attr_break(out job);
            job.execute();
            if (workthread != null)
            {
                workthread.Abort();
                workthread = null;
            }
        }
        public comp_Engine()
        {
            DateTime date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            today = new Base.Time.opaque_Timestamp { ticks = date.Ticks };
            scheduler.AutoReset = false;
            scheduler.Elapsed += scheduler_Elapsed;
        }
    }

}
