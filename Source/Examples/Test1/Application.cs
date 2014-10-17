using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aha.Package.API;
using Aha.Package.API.Application;
using Aha.Package.Base;
using Aha.Core;

namespace Aha.Package.API
{
    namespace Application
    {
        public struct opaque_Event
        {
            public string input;
        }

        public class module_Application : AhaModule, imod_Application<opaque_Event>
        {
            class obj_Behavior : iobj_Behavior<opaque_Event>
            {
                private icomp_BehaviorParams<opaque_Event> field_param;
                private IahaArray<Jobs.opaque_Job<opaque_Event>> field_jobs;
                private bool op_Exclaim_integer(long param_n, out long result)
                {
                    IahaArray<long> seg = new AhaSegment(1, param_n + 1);
                    return (seg.foldl(delegate(long x, long y, out long res) { try { res = checked(x * y); return true; } catch (System.Exception) { res = 0; return false; } }, out result));
                }
                public bool action_handleEvent(opaque_Event param_event) { return false; }
                public bool action_handleInput(IahaArray<char> param_input)
                {
                    long fact;
                    IahaArray<char> msg;
                    Jobs.opaque_Job<opaque_Event> job;
                    if (param_input.get().Length == 0)
                    {
                        Jobs.icomp_Engine<opaque_Event> engine;
                        field_param.attr_engine(out engine);
                        engine.attr_disable(out job);
                        field_jobs = new AhaArray<Jobs.opaque_Job<opaque_Event>>(new Jobs.opaque_Job<opaque_Event>[] { job });
                        return true;
                    }
                    try
                    {
                        long n = Convert.ToInt64(new string(param_input.get()));
                        if (op_Exclaim_integer(n, out fact))
                        {
                            msg = new AhaString(n.ToString() + "! = " + fact.ToString());
                            field_param.fattr_output(msg, out job);
                            field_jobs = new AhaArray<Jobs.opaque_Job<opaque_Event>>(new Jobs.opaque_Job<opaque_Event>[] { job });
                        }
                        else
                        {
                            msg = new AhaString("Error: out of range");
                            field_param.fattr_output(msg, out job);
                            Jobs.icomp_Engine<opaque_Event> engine;
                            field_param.attr_engine(out engine);
                            Jobs.opaque_Job<opaque_Event> job2;
                            engine.fattr_log(msg, out job2);
                            field_jobs = new AhaArray<Jobs.opaque_Job<opaque_Event>>(new Jobs.opaque_Job<opaque_Event>[] { job, job2 });
                        }
                    }
                    catch (System.Exception)
                    {
                        msg = new AhaString("Error: invalid string");
                        field_param.fattr_output(msg, out job);
                        Jobs.icomp_Engine<opaque_Event> engine;
                        field_param.attr_engine(out engine);
                        Jobs.opaque_Job<opaque_Event> job2;
                        engine.fattr_log(msg, out job2);
                        field_jobs = new AhaArray<Jobs.opaque_Job<opaque_Event>>(new Jobs.opaque_Job<opaque_Event>[] { job, job2 });
                    }
                    return true;
                }
                public bool state(out IahaArray<Jobs.opaque_Job<opaque_Event>> result) { result = field_jobs; return true; }
                public IahaObject<IahaArray<Jobs.opaque_Job<opaque_Event>>> copy() { return new obj_Behavior(field_param); }
                public obj_Behavior(icomp_BehaviorParams<opaque_Event> param_param)
                {
                    field_param = param_param;
                    Jobs.icomp_Engine<opaque_Event> engine;
                    field_param.attr_engine(out engine);
                    Jobs.opaque_Job<opaque_Event> job;
                    engine.attr_enable(out job);
                    field_jobs = new AhaArray<Jobs.opaque_Job<opaque_Event>>(new Jobs.opaque_Job<opaque_Event>[] { job });
                }
            }

            public bool attr_Title(out IahaArray<char> result) { result = new AhaString("Factorial"); return true; }
            public bool attr_Signature(out IahaArray<char> result) { result = new AhaString("Demo"); return true; }
            public bool fattr_Behavior(icomp_BehaviorParams<opaque_Event> param_param, out iobj_Behavior<opaque_Event> result) { result = new obj_Behavior(param_param); return true; }
        }
    }
}
