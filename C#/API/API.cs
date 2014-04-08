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

    public class Jobs : AhaModule
    {
        public delegate void Job();

        public interface Behavior<Event> : IahaObject<IahaArray<Job>>
        {
            void handle(Event e);
        }

        public delegate Event EnquireTime<Event>(Time.Timestamp time);

        public interface Engine<Event>
        {
            Job run(Job job);
            Job raise(Event e);
            //Job delay(double interval, Event e);
            Job enquireTime(EnquireTime<Event> enq);
        }
    }

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
    public delegate Jobs.Job Output(IahaArray<char> output);

    public interface IBehaviorParams<Event>
    {
        IahaArray<char> settings();
        Jobs.Job output(IahaArray<char> text);
        Jobs.Engine<Event> engine();
    }

    //public delegate Behavior<Event> GetBehavior<Event>(IBehaviorParams<Event> param);

    public interface IApplication<Event>
    {
        IahaArray<char> Title();
        IahaArray<char> Signature();
        Jobs.Behavior<Event> Behavior(IBehaviorParams<Event> param);
        Event Receive(IahaArray<char> input);
    }
}
