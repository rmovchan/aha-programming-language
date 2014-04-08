using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using API;
using BaseLibrary;
using AhaCore;

namespace API
{
    public class Application : AhaModule, IApplication<IahaVoid>
    {
        class behavior : Jobs.Behavior<IahaVoid>
        {
            private Jobs.Engine<IahaVoid> engine;
            private IahaArray<char> settings;
            private Output output;
            public void handle(IahaVoid e) { throw Failure.One; }
            public IahaArray<Jobs.Job> state() { return new AhaArray<Jobs.Job>(new Jobs.Job[] { output(new AhaString("Hello, World!")) }); }
            public IahaObject<IahaArray<Jobs.Job>> copy() { return this; }
            public behavior(IahaArray<char> inp, Output outp, Jobs.Engine<IahaVoid> eng) { settings = inp; output = outp; engine = eng; }
        }

        public IahaArray<char> Title() { return new AhaString("HelloWorld"); }
        public IahaArray<char> Signature() { return new AhaString("Demo"); }
        public Jobs.Behavior<IahaVoid> Behavior(IBehaviorParams<IahaVoid> param) { return new behavior(param.settings(), param.output, param.engine()); }
        public IahaVoid Receive(IahaArray<char> input) { return AhaVoid.One; }
    }

}
