using System;
using System.Collections.Generic;
using System.Text;
using Aha.Package.API;
using Aha.Package.API.Application;
using Aha.Package.API.FileIO;
using Aha.Package.Base;
using Aha.Core;
using Aha.Package.Compiler.Scanner;

namespace Aha.Package.API
{
    namespace Application
    {
        public struct opaque_Event
        {
            public string path;
            public string error;
            public icomp_TextReadParams<opaque_Event> param;
        }

        struct Encoding : FileIOtypes.icomp_Encoding
        {
            public bool attr_ASCII() { return false; }
            public bool attr_UTF8() { return false; }
            public bool attr_UCS2LE() { return false; }
            public bool attr_UCS2BE() { return false; }
            public bool attr_auto() { return true; }
        }

        class ReadTextParam : icom_ReadTextParam<opaque_Event>
        {
            public string path;
            public Jobs.icomp_Engine<opaque_Event> engine;
            public bool attr_path(out Aha.Package.API.Environment.opaque_FilePath result) { result = new Environment.opaque_FilePath() { value = path }; return true; }
            public bool attr_engine(out Aha.Package.API.Jobs.icomp_Engine<opaque_Event> result) { result = engine; return true; }
            public bool attr_encoding(out FileIOtypes.icomp_Encoding result) { result = new Encoding(); return true; }
            public bool fattr_success(icomp_TextReadParams<opaque_Event> param, out opaque_Event result) { result = new opaque_Event() { path = path, param = param, error = "" }; return true; }
            public bool fattr_error(FileIOtypes.icomp_ErrorInfo error, out opaque_Event result) { IahaArray<char> arr; error.attr_message(out arr); string msg = new string(arr.get()); result = new opaque_Event() { path = path, param = null, error = msg }; return true; }
        }

        public class module_Application : AhaModule, imod_Application<opaque_Event>
        {
            class obj_Behavior : iobj_Behavior<opaque_Event>
            {
                private icom_BehaviorParams<opaque_Event> field_param;
                private IahaArray<Jobs.opaque_Job<opaque_Event>> field_jobs;
                private imod_Scanner Scanner = new module_Scanner();
                private imod_FileIO<opaque_Event> FileIO = new module_FileIO<opaque_Event>();
                public bool action_handleEvent(opaque_Event param_event) 
                {
                    IahaArray<char> msg;
                    Jobs.opaque_Job<opaque_Event> job;
                    Jobs.icomp_Engine<opaque_Event> engine; 
                    field_param.attr_engine(out engine);
                    if (param_event.error != "")
                    {
                        msg = new AhaString(param_event.error);
                        field_param.fattr_output(msg, out job);
                        field_jobs = new AhaArray<Jobs.opaque_Job<opaque_Event>>(new Jobs.opaque_Job<opaque_Event>[] { job });
                    }
                    else
                    {
                        Jobs.opaque_Job<opaque_Event>[] jobs = new Jobs.opaque_Job<opaque_Event>[200];
                        int j = 0;
                        IahaSequence<com_Token> tokens, clone;
                        IahaSequence<char> content;
                        long size;
                        com_Token token;
                        long row;
                        long col;
                        IahaArray<char> text;
                        com_Location loc;
                        param_event.param.attr_content(out content);
                        param_event.param.attr_size(out size);
                        Scanner.fexport(content, size, out tokens);
                        clone = (IahaSequence<com_Token>)tokens.copy();
                        while (j < jobs.Length && clone.state(out token))
                        {
                            loc = token.attr_location;
                            row = loc.attr_row;
                            col = loc.attr_column;
                            text = token.attr_text;
                            text = new AhaString(row.ToString() + ":" + col.ToString() + "\t" + new string(text.get()));
                            field_param.fattr_output(text, out jobs[j]);
                            j++;
                            if (!clone.action_skip()) break;
                        }
                        Array.Resize<Jobs.opaque_Job<opaque_Event>>(ref jobs, j);
                        field_jobs = new AhaArray<Jobs.opaque_Job<opaque_Event>>(jobs);
                    }
                    return true; 
                }
                public bool action_handleInput(IahaArray<char> param_input)
                {
                    //Jobs.opaque_Job<opaque_Event> job;
                    //Jobs.icomp_Engine<opaque_Event> engine;
                    //field_param.attr_engine(out engine);
                    //if (param_input.get().Length == 0)
                    //{
                    //    engine.attr_disable(out job);
                    //    field_jobs = new AhaArray<Jobs.opaque_Job<opaque_Event>>(new Jobs.opaque_Job<opaque_Event>[] { job });
                    //    return true;
                    //}
                    //string path = new string(param_input.get());
                    //icomp_ReadTextParam<opaque_Event> readp = new ReadTextParam() { path = path, engine = engine };
                    //FileIO.fattr_ReadText(readp, out job);
                    //field_jobs = new AhaArray<Jobs.opaque_Job<opaque_Event>>(new Jobs.opaque_Job<opaque_Event>[] { job });
                    return false;
                }
                public bool state(out IahaArray<Jobs.opaque_Job<opaque_Event>> result) { result = field_jobs; return true; }
                public IahaObject<IahaArray<Jobs.opaque_Job<opaque_Event>>> copy() { return new obj_Behavior(field_param); }
                public obj_Behavior(icom_BehaviorParams<opaque_Event> param_param)
                {
                    field_param = param_param;
                    Jobs.icomp_Engine<opaque_Event> engine;
                    field_param.attr_engine(out engine);
                    Jobs.opaque_Job<opaque_Event> job;
                    IahaArray<char> settings;
                    if (param_param.attr_settings(out settings) && settings.get().Length != 0)
                    {
                        string path = new string(settings.get());
                        icom_ReadTextParam<opaque_Event> readp = new ReadTextParam() { path = path, engine = engine };
                        FileIO.fattr_ReadText(readp, out job);
                        field_jobs = new AhaArray<Jobs.opaque_Job<opaque_Event>>(new Jobs.opaque_Job<opaque_Event>[] { job });
                    }
                    else
                    {
                        IahaArray<char> msg = new AhaString("Invalid settings");
                        param_param.fattr_output(msg, out job);
                        field_jobs = new AhaArray<Jobs.opaque_Job<opaque_Event>>(new Jobs.opaque_Job<opaque_Event>[] { job });
                    }
                }
            }

            public bool attr_Title(out IahaArray<char> result) { result = new AhaString("Scanner"); return true; }
            public bool attr_Signature(out IahaArray<char> result) { result = new AhaString("Test"); return true; }
            public bool fattr_Behavior(icom_BehaviorParams<opaque_Event> param_param, out iobj_Behavior<opaque_Event> result) { result = new obj_Behavior(param_param); return true; }
        }
    }
}
