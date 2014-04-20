using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aha.API;
using Aha.Base;
using Aha.Core;

namespace Aha.API
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
    public class Application : AhaModule
    {
        public struct Event { }

        public interface icomp_BehaviorParams
        {
            IahaArray<char> attr_settings();
            Aha.API.Jobs.Implementation.opaque_Job fattr_output(IahaArray<char> text);
            Aha.API.Jobs.icomp_Engine attr_engine();
        }

        public delegate module_Jobs<Event>.opaque_Job Output(IahaArray<char> output);

        class obj_Behavior : module_Jobs<Event>.iobj_Behavior
        {
            private module_Jobs<Event>.icomp_Engine engine;
            private IahaArray<char> settings;
            private Output output;
            public void action_handle(Event e) { throw Failure.One; }
            public IahaArray<module_Jobs<Event>.opaque_Job> state() { return new AhaArray<module_Jobs<Event>.opaque_Job>(new module_Jobs<Event>.opaque_Job[] { output(new AhaString("Hello, World!")) }); }
            public IahaObject<IahaArray<module_Jobs<Event>.opaque_Job>> copy() { return new obj_Behavior(settings, output, engine); }
            public obj_Behavior(IahaArray<char> inp, Output outp, module_Jobs<Event>.icomp_Engine eng) { settings = inp; output = outp; engine = eng; }
        }

        public IahaArray<char> attr_Title() { return new AhaString("HelloWorld"); }
        public IahaArray<char> attr_Signature() { return new AhaString("Demo"); }
        public module_Jobs<Event>.iobj_Behavior fattr_Behavior(icomp_BehaviorParams param) { return new obj_Behavior(param.attr_settings(), param.fattr_output, param.attr_engine()); }
        public Event fattr_Receive(IahaArray<char> input) { return new Event(); }
    }

}
