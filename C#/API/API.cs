using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseLibrary;
using AhaCore;

namespace API
{
//doc 
//    Title: "Jobs"
//    Purpose: "Basic jobs"
//    Package: "Application Program Interface"
//    Author: "Roman Movchan, Melbourne, Australia"
//    Created: "2013-09-05"
//end

//type Event: arbitrary "custom event type"
//use Time: Base/Time
//export Types:
//    type Job: opaque "a job for runtime environment"
//    type Behavior: obj [Job] handle(Event) end "event loop"
//    type Engine:
//        [
//            run: { Job -> Job } "run job asynchronously"
//            raise: { Event -> Job } "raise event to be immediately handled"
//            compute: { [ event: { -> Event } fail: Event ] -> Job } "job that computes an event in the background and raises it"
//            generate: { Event* -> Job } "generate (asynchronously) a sequence of events"
//            enquireTime: { { @Time!Timestamp -> Event } -> Job } "job that raises event that receives current system time"
//            delay: { @Time!Interval, Job -> Job } "do job after delay"
//            stop: Job "terminate all current jobs"
//        ] "interface to the job engine"
//end

    public class module_Jobs<Event> : AhaModule
    {
        public delegate void opaque_Job();

        public interface iobj_Behavior : IahaObject<IahaArray<opaque_Job>>
        {
            void action_handle(Event e);
        }

        public delegate Event func_EnquireTime(module_Time.opaque_Timestamp time);

        public interface icomp_Engine
        {
            opaque_Job fattr_run(opaque_Job job);
            opaque_Job fattr_raise(Event e);
            //Job delay(double interval, Event e);
            opaque_Job fattr_enquireTime(func_EnquireTime enq);
        }
    }
}
