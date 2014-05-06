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
        public partial struct opaque_Event
        {
            private IahaArray<char> field_input;
            public IahaArray<char> attr_input() { return field_input; }
            public opaque_Event(IahaArray<char> param_input) { field_input = param_input; }
        }

        public class module_Application : AhaModule, imod_Application
        {
            //private module_Jobs<opaque_Event> nick_Jobs = new module_Jobs<opaque_Event>();

            class obj_Behavior : Jobs.iobj_Behavior<opaque_Event>
            {
                private icomp_BehaviorParams field_param;
                private IahaArray<Jobs.opaque_Job<opaque_Event>> field_jobs;
                private Int64 op_Exclaim_integer(Int64 param_n) { return (new AhaSegment(1, param_n + 1)).foldl(delegate(Int64 x, Int64 y) { return checked(x * y); }); }
                public void action_handle(opaque_Event param_event)
                {
                    try
                    {
                        field_jobs = new AhaArray<Jobs.opaque_Job<opaque_Event>>
                            (
                                new Jobs.opaque_Job<opaque_Event>[] 
                                    { 
                                        field_param.fattr_output
                                            (
                                                new AhaString
                                                    (
                                                        op_Exclaim_integer
                                                            (
                                                                Convert.ToInt64
                                                                    (
                                                                        new string((param_event).attr_input().get())
                                                                    )
                                                            ).ToString()
                                                    )
                                            )
                                    }
                            );
                    }
                    catch (System.Exception)
                    {
                        field_jobs = new AhaArray<Jobs.opaque_Job<opaque_Event>>
                            (
                                new Jobs.opaque_Job<opaque_Event>[] 
                                    { 
                                        field_param.fattr_output(new AhaString("Error"))
                                    }
                            );
                    }
                }
                public IahaArray<Jobs.opaque_Job<opaque_Event>> state() { return field_jobs; }
                public IahaObject<IahaArray<Jobs.opaque_Job<opaque_Event>>> copy() { return new obj_Behavior(field_param); }
                public obj_Behavior(icomp_BehaviorParams param_param) { field_param = param_param; field_jobs = new AhaArray<Jobs.opaque_Job<opaque_Event>>(new Jobs.opaque_Job<opaque_Event>[] { }); }
            }

            public IahaArray<char> attr_Title() { return new AhaString("Factorial"); }
            public IahaArray<char> attr_Signature() { return new AhaString("Demo"); }
            public Jobs.iobj_Behavior<opaque_Event> fattr_Behavior(icomp_BehaviorParams param_param) { return new obj_Behavior(param_param); }
            public opaque_Event fattr_Receive(IahaArray<char> param_input) { return new comp_Event(param_input); }
        }
    }
}
