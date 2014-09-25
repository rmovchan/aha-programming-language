using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aha.API;
using Aha.API.Application;
using Aha.Base;
using Aha.Core;

namespace Aha.API
{
    namespace Application
    {
        public struct opaque_Event
        {
            public string input;
        }

        class obj_Behavior : Jobs.iobj_Behavior<opaque_Event>
        {
            private icomp_BehaviorParams<opaque_Event> field_param;
            private IahaArray<Jobs.opaque_Job<opaque_Event>> field_jobs;
            private bool op_Exclaim_integer(long param_n, out long result)
            {
                IahaArray<long> seg = new AhaSegment(1, param_n + 1);
                return (seg.foldl(delegate(long x, long y, out long res) { try { res = checked(x * y); return true; } catch (System.Exception) { res = 0; return false; } }, out result));
            }
            public bool action_handle(opaque_Event param_event)
            {
                long fact;
                IahaArray<char> msg;
                Jobs.opaque_Job<opaque_Event> job;
                try
                {
                    long n = Convert.ToInt64(param_event.input);
                    if (op_Exclaim_integer(n, out fact))
                    {
                        msg = new AhaString(n.ToString() + "! = " + fact.ToString());
                    }
                    else
                    {
                        msg = new AhaString("Error: out of range");
                    }
                }
                catch(System.Exception)
                {
                    msg = new AhaString("Error: invalid string");
                }
                field_param.fattr_output(msg, out job);
                Jobs.icomp_Engine<opaque_Event> engine;
                field_param.attr_engine(out engine);
                //Jobs.opaque_Job<opaque_Event> job2;
                //engine.fattr_raise(new opaque_Event() { input = n + 1 }, out job2);
                field_jobs = new AhaArray<Jobs.opaque_Job<opaque_Event>>(new Jobs.opaque_Job<opaque_Event>[] { job });
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

        class comp_Application : API.Application.imod_Application<opaque_Event>
        {
            public bool attr_Title(out IahaArray<char> result) { result = new AhaString("Factorial"); return true; }
            public bool attr_Signature(out IahaArray<char> result) { result = new AhaString("Demo"); return true; }
            public bool fattr_Behavior(icomp_BehaviorParams<opaque_Event> param_param, out Jobs.iobj_Behavior<opaque_Event> result) { result = new obj_Behavior(param_param); return true; }
            public bool fattr_Receive(IahaArray<char> param_input, out opaque_Event result) { result = new opaque_Event { input = new string(param_input.get()) }; return true; }
        }

        public class export { public imod_Application<opaque_Event> value = new comp_Application(); }
    }
}
