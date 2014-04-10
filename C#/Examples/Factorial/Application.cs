using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API;
using BaseLibrary;
using AhaCore;

namespace API
{
    //doc 
    //    Title: "Application"
    //    Purpose: "A console application"
    //    Package: "Application Program Interface"
    //    Author: "Roman Movchan, Melbourne, Australia"
    //    Created: "2013-27-08"
    //end

    //type Event: opaque "must be defined by the implementation"
    //use Jobs: API/Jobs(Event: Event)
    //the Title: [character]  "application title"
    //the Signature: [character]  "vendor's signature"
    //the Permit: { [character] } "verify supplied password"
    //the Behavior: { [ input: [character] output: { [character] -> @Jobs!Job } engine: @Jobs!Engine ] -> @Jobs!Behavior } "application behavior"
    //the Receive: { [character] -> Event } "convert user input to events"
    public class module_Application : AhaModule
    {
        public interface opaque_Event
        {
            IahaArray<char> attr_input();
        }

        //private module_Jobs<opaque_Event> nick_Jobs = new module_Jobs<opaque_Event>();

        struct comp_Event : opaque_Event
        {
            private IahaArray<char> field_input;
            public IahaArray<char> attr_input() { return field_input; }
            public comp_Event(IahaArray<char> param_input) { field_input = param_input; }
        }

        public interface icomp_BehaviorParams
        {
            IahaArray<char> attr_settings();
            module_Jobs<opaque_Event>.opaque_Job fattr_output(IahaArray<char> text);
            module_Jobs<opaque_Event>.icomp_Engine attr_engine();
        }

        class obj_Behavior : module_Jobs<opaque_Event>.iobj_Behavior
        {
            private icomp_BehaviorParams field_param;
            private AhaArray<module_Jobs<opaque_Event>.opaque_Job> field_jobs;
            private Int64 op_Exclaim_integer(Int64 param_n) { return (new AhaSegment(1, param_n + 1)).foldl(delegate(Int64 x, Int64 y) { return x * y; }); }
            public void action_handle(opaque_Event param_event) 
            { 
                field_jobs = new AhaArray<module_Jobs<opaque_Event>.opaque_Job>(
                    new module_Jobs<opaque_Event>.opaque_Job[] 
                    { 
                        field_param.fattr_output(new AhaString(op_Exclaim_integer(Convert.ToInt64(new string(param_event.attr_input().get()))).ToString()))
                    }); 
            }
            public IahaArray<module_Jobs<opaque_Event>.opaque_Job> state() { return field_jobs; }
            public IahaObject<IahaArray<module_Jobs<opaque_Event>.opaque_Job>> copy() { return new obj_Behavior(field_param); }
            public obj_Behavior(icomp_BehaviorParams param_param) { field_param = param_param; field_jobs = new AhaArray<module_Jobs<opaque_Event>.opaque_Job>(new module_Jobs<opaque_Event>.opaque_Job[] { }); }
        }

        public IahaArray<char> attr_Title() { return new AhaString("Factorial"); }
        public IahaArray<char> attr_Signature() { return new AhaString("Demo"); }
        public module_Jobs<opaque_Event>.iobj_Behavior fattr_Behavior(icomp_BehaviorParams param_param) { return new obj_Behavior(param_param); }
        public opaque_Event fattr_Receive(IahaArray<char> param_input) { return new comp_Event(param_input); }
    }

}
