using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using API;
using BaseLibrary;
using AhaCore;

namespace API
{
    public class Application<Event> : AhaModule
    {
        class behavior : Behavior<Event>
        {
            private Engine<Event> engine;
            private IahaArray<char> settings;
            private Output output;
            public void handle(Event e) { throw Failure.One; }
            public IahaArray<Job> state() { return new AhaArray<Job>(new Job[] { output(new AhaString("Hello, World!")) }); }
            public IahaObject<IahaArray<Job>> copy() { return this; }
            public behavior(IahaArray<char> inp, Output outp, Engine<Event> eng) { settings = inp; outp = output; engine = eng; }
        }

        private Engine<Event> e;
        private Behavior<Event> b;
        public IahaArray<char> Title() { return new AhaString("HelloWorld"); }
        public IahaArray<char> Signature() { return new AhaString(""); }
        public GetBehavior<Event> Behavior() { return delegate(IBehaviorParams<Event> param) { return new behavior(param.settings(), param.output(), param.engine()); }; }
        public Event Receive(IahaArray<char> input) { return default(Event); }
    }

}
