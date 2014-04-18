using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseLibrary;
using AhaCore;

namespace API
{
    namespace Jobs
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
    {
        public interface iobj_Behavior<tpar_Event, opaque_Job> : IahaObject<IahaArray<opaque_Job>>
        {
            void action_handle(tpar_Event param_event);
        }

        public delegate tpar_Event func_EnquireTime<tpar_Event>(module_Time.opaque_Timestamp time);

        public interface icomp_Engine<tpar_Event, opaque_Job>
        {
            opaque_Job fattr_run(opaque_Job job);
            opaque_Job fattr_raise(tpar_Event e);
            opaque_Job fattr_delay(module_Time.opaque_Interval interval, tpar_Event e);
            opaque_Job fattr_enquireTime(func_EnquireTime<tpar_Event> enq);
            opaque_Job fattr_stop();
        }

        namespace Implementation
        {
            public delegate void opaque_Job();
        }
    }

    namespace Environment
    //doc 
    //    Title: "Environment"
    //    Purpose: "Static information on the runtime environment"
    //    Package: "Application Program Interface"
    //    Author: "Roman Movchan, Melbourne, Australia"
    //    Created: "2013-08-22"
    //end

//type String: [character] "alias for character array type"

//use Format: Base/Formatting
    //use StrUtils: Base/StrUtils
    //use Time: Base/Time
    //use Money: Base/Money
    //use Float: Base/Float

//export Types:
    //    type FilePath: opaque "file path"
    //    type DirPath: opaque "directory (folder) path"
    //end

//export Info:    
    //    the Framework:
    //        [
    //            name: String "framework name"
    //            version:
    //                [
    //                    major: integer
    //                    minor: integer
    //                    build: integer
    //                ] "framework version"
    //            components: [String] "classnames of all registered components"
    //        ] "runtime framework info"
    //    the Platform: 
    //        [
    //            Windows: [ win32: win64: ] " Windows (32- or 64-bit)" 
    //            MacOSX: "Mac OSX" 
    //            Linux: "Linux" 
    //            FreeBSD: "FreeBSD" 
    //            iOS: "iOS" 
    //            Android: "Android" 
    //            other: "other"
    //        ] "platform kind"
    //    the Locale:
    //        [
    //            GMToffset: @Time!Interval "GMT offset"
    //            country: String "country name"
    //            language: String "language"
    //            currency: String "currency symbol(s)"
    //            decimal: character "decimal separator"
    //            format: @Format!Format "formatting routines"
    //            deformat: @Format!Deformat "deformatting routines"
    //            charCompare: @StrUtils!CharCompare "character comparison function"
    //            upper: { character -> character } "upper case conversion"
    //            lower: { character -> character } "lower case conversion"
    //        ] "locale info"
    //    the FileSystem: 
    //        [
    //            eol: String "end-of-line characters"
    //            splitLines: { character* -> String* } "convert sequence of chars to sequence of lines"
    //            joinLines: { String* -> character* } "convert sequence of lines to sequence of chars"
    //            filePath: { DirPath, String -> FilePath } "get file path from file name and directory path"
    //            subDirPath: { DirPath, String -> DirPath } "get subdirectory path from its name and parent directory path"
    //            parentDirPath: { DirPath -> DirPath } "get parent directory path"
    //            fileName: { FilePath -> String } "get file name (with extension) from file path"
    //            fileDir: { FilePath -> DirPath } "get file's directory path"
    //            fileExt: { FilePath -> String } "extract file's extension (empty string if none)"
    //            changeExt: { FilePath, String -> FilePath } "change file's extension"
    //            splitDirPath: { DirPath -> [String] } "split directory path into string components"
    //            buildDirPath: { [String] -> DirPath } "build directory path from string components"
    //            workingDir: DirPath "directory where application can write data"
    //            appDir: DirPath "directory from which application has started"
    //            rootDir: DirPath "file system's root directory"
    //        ] "file system info and path handling routines"
    //    the Username: String "user name"
    //    the SystemID: String "system identification"
    //    the Variables: [[ name: String value: String ]] "list of all environment variables and their values"
    //end
    //export Operators:
    //    (~str integer): { integer -> String } "convert integer to string"
    //    (~str @Float!Float): { @Float!Float -> String } "convert Float to string (local format)"
    //    (~str @Time!Timestamp): { @Time!Timestamp -> String } "convert Timestamp to string (local format)"
    //    (~str @Money!Money): { @Money!Money -> String } "convert Money to string (local format)"
    //    (~int String): { String -> integer } "convert string to integer"
    //    (~float String): { String -> @Float!Float } "convert string (local format) to Float"
    //    (~date String): { String -> @Time!Timestamp } "convert string (local format) to date"
    //    (~time String): { String -> @Time!Interval } "convert string (local format) to time"
    //    (~timestamp String): { String -> @Time!Timestamp } "convert string (local format) to timestamp"
    //    (~money String): { String -> @Money!Money } "convert string (local format) to Money"
    //    (String <= String): { String, String } "compare string in local sorting order"
    //    (String < String): { String, String } "compare string in local sorting order"
    //    (String > String): { String, String } "compare string in local sorting order"
    //    (String >= String): { String, String } "compare string in local sorting order"
    //    (FilePath = FilePath): { FilePath, FilePath } "are paths the same?"
    //    (DirPath = DirPath): { DirPath, DirPath } "are paths the same?"
    //end    
    {
        public interface icomp_Version
        {
            Int64 attr_major();
            Int64 attr_minor();
            Int64 attr_build();
        }

        public interface icomp_Framework
        {
            IahaArray<char> attr_name();
            icomp_Version attr_version();
            IahaArray<IahaArray<char>> attr_components();
        }

        public interface icomp_Platform
        {
            bool attr1_Windows();
            bool attr2_MacOSX();
            bool attr3_Linux();
            bool attr4_FreeBSD();
            bool attr5_iOS();
            bool attr6_Android();
            bool attr7_other();
        }

        public interface icomp_Locale
        {
            module_Time.opaque_Interval attr_GMToffset();
            IahaArray<char> attr_country();
            IahaArray<char> attr_language();
            IahaArray<char> attr_currency();
            char attr_decimal();
            //format: @Format!Format "formatting routines"
            //deformat: @Format!Deformat "deformatting routines"
            //charCompare: @StrUtils!CharCompare "character comparison function"
            IahaArray<char> fattr_upper(IahaArray<char> ch);
            IahaArray<char> fattr_lower(IahaArray<char> ch);
            bool fattr_sameText(IahaArray<char> param_first, IahaArray<char> param_second);
        }

        public interface icomp_FileSystem<opaque_FilePath, opaque_DirPath>
        {
            IahaArray<char> attr_eol();
            //splitLines: { character* -> String* } "convert sequence of chars to sequence of lines"
            //joinLines: { String* -> character* } "convert sequence of lines to sequence of chars"
            opaque_FilePath fattr_filePath(opaque_DirPath param_dir, IahaArray<char> param_name);
            opaque_DirPath fattr_subDirPath(opaque_DirPath param_dir, IahaArray<char> param_name);
            opaque_DirPath fattr_parentDirPath(opaque_DirPath param_dir);
            IahaArray<char> fattr_fileName(opaque_FilePath param_path);
            opaque_DirPath fattr_fileDir(opaque_FilePath param_dir);
            IahaArray<char> fattr_fileExt(opaque_FilePath param_path);
            opaque_FilePath fattr_changeExt(opaque_FilePath param_dir, IahaArray<char> param_ext);
            IahaArray<IahaArray<char>> fattr_splitDirPath(opaque_DirPath param_path);
            opaque_DirPath fattr_buildDirPath(IahaArray<IahaArray<char>> param_parts);
            opaque_DirPath fattr_workingDir();
            opaque_DirPath fattr_appDir();
            opaque_DirPath fattr_rootDir();
        }

        public interface imod_Environment<opaque_FilePath, opaque_DirPath>
        {
            icomp_Framework attr_Framework();
            icomp_Platform attr_Platform();
            icomp_FileSystem<opaque_FilePath, opaque_DirPath> attr_FileSystem();
        }

        namespace Implementation
        {
            public struct opaque_FilePath
            {
                public string value;
            }

            public struct opaque_DirPath
            {
                public string value;
            }

            public class module_Environment : AhaModule, imod_Environment<opaque_FilePath, opaque_DirPath>
            {
                struct comp_Version : icomp_Version
                {
                    public Int64 attr_major() { return 0; }
                    public Int64 attr_minor() { return 1; }
                    public Int64 attr_build() { return 1; }
                }

                struct comp_Framework : icomp_Framework
                {
                    public IahaArray<char> attr_name() { return new AhaString("Aha! for .NET"); }
                    public icomp_Version attr_version() { return new comp_Version(); }
                    public IahaArray<IahaArray<char>> attr_components() { return new AhaArray<IahaArray<char>>(new IahaArray<char>[] { }); }
                }

                public icomp_Framework attr_Framework() { return new comp_Framework(); }

                struct comp_Platform : icomp_Platform
                {
                    public bool attr1_Windows() { return true; }
                    public bool attr2_MacOSX() { return false; }
                    public bool attr3_Linux() { return false; }
                    public bool attr4_FreeBSD() { return false; }
                    public bool attr5_iOS() { return false; }
                    public bool attr6_Android() { return false; }
                    public bool attr7_other() { return false; }
                }

                public icomp_Platform attr_Platform() { return new comp_Platform(); }

                class comp_Locale : icomp_Locale
                {
                    private module_Time.opaque_Interval field_GMToffset = new module_Time.opaque_Interval { ticks = DateTime.Now.Ticks - DateTime.Now.ToUniversalTime().Ticks };
                    public module_Time.opaque_Interval attr_GMToffset() { return field_GMToffset; }
                    public IahaArray<char> attr_country() { return new AhaString(System.Globalization.CultureInfo.CurrentCulture.EnglishName); }
                    public IahaArray<char> attr_language() { return new AhaString(System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName); }
                    public IahaArray<char> attr_currency() { return new AhaString(System.Globalization.CultureInfo.CurrentCulture.NumberFormat.CurrencySymbol); }
                    public char attr_decimal() { return System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator[0]; }
                    //format: @Format!Format "formatting routines"
                    //deformat: @Format!Deformat "deformatting routines"
                    //charCompare: @StrUtils!CharCompare "character comparison function"
                    public IahaArray<char> fattr_upper(IahaArray<char> param_str) { return new AhaString((new string(param_str.get())).ToUpper(System.Globalization.CultureInfo.CurrentCulture)); }
                    public IahaArray<char> fattr_lower(IahaArray<char> param_str) { return new AhaString((new string(param_str.get())).ToLower(System.Globalization.CultureInfo.CurrentCulture)); }
                    public bool fattr_sameText(IahaArray<char> param_first, IahaArray<char> param_second)
                    {
                        return String.Compare(
                            new string(param_first.get()),
                            new string(param_second.get()),
                            StringComparison.CurrentCultureIgnoreCase) == 0;
                    }
                }

                public icomp_Locale attr_Locale() { return new comp_Locale(); }

                class comp_FileSystem : icomp_FileSystem<opaque_FilePath, opaque_DirPath>
                {
                    public IahaArray<char> attr_eol() { return new AhaString("\r\n"); }
                    //splitLines: { character* -> String* } "convert sequence of chars to sequence of lines"
                    //joinLines: { String* -> character* } "convert sequence of lines to sequence of chars"
                    public opaque_FilePath fattr_filePath(opaque_DirPath param_dir, IahaArray<char> param_name)
                    { return new opaque_FilePath { value = param_dir.value + System.IO.Path.PathSeparator + (new string(param_name.get())) }; }
                    public opaque_DirPath fattr_subDirPath(opaque_DirPath param_dir, IahaArray<char> param_name)
                    { return new opaque_DirPath { value = param_dir.value + System.IO.Path.PathSeparator + (new string(param_name.get())) }; }
                    public opaque_DirPath fattr_parentDirPath(opaque_DirPath param_dir)
                    { return new opaque_DirPath { value = System.IO.Path.GetDirectoryName(param_dir.value) }; }
                    public IahaArray<char> fattr_fileName(opaque_FilePath param_path)
                    { return new AhaString(System.IO.Path.GetFileName(param_path.value)); }
                    public opaque_DirPath fattr_fileDir(opaque_FilePath param_path)
                    { return new opaque_DirPath { value = System.IO.Path.GetDirectoryName(param_path.value) }; }
                    public IahaArray<char> fattr_fileExt(opaque_FilePath param_path)
                    { return new AhaString(System.IO.Path.GetExtension(param_path.value)); }
                    public opaque_FilePath fattr_changeExt(opaque_FilePath param_dir, IahaArray<char> param_ext)
                    { return new opaque_FilePath { value = System.IO.Path.ChangeExtension(param_dir.value, new string(param_ext.get())) }; }
                    public IahaArray<IahaArray<char>> fattr_splitDirPath(opaque_DirPath param_path)
                    {
                        List<int> list = new List<int>();
                        int j = 0;
                        while (j != -1)
                        {
                            j = param_path.value.IndexOf(System.IO.Path.PathSeparator, j, param_path.value.Length);
                            if (j == -1) break;
                            list.Add(j);
                            j++;
                        }
                        IahaArray<char>[] temp = new IahaArray<char>[list.Count + 1];
                        j = 0;
                        char[] seg;
                        for (int i = 0; i < temp.Length; i++)
                        {
                            seg = new char[list[i] - j];
                            Array.Copy(param_path.value.ToCharArray(), j, seg, 0, list[i] - j);
                            temp[i] = new AhaString(seg);
                            j = list[i] + 1;

                        }
                        seg = new char[param_path.value.Length - j];
                        Array.Copy(param_path.value.ToCharArray(), j, seg, 0, param_path.value.Length - j);
                        temp[list.Count] = new AhaString(seg);
                        return new AhaArray<IahaArray<char>>(temp);
                    }
                    public opaque_DirPath fattr_buildDirPath(IahaArray<IahaArray<char>> param_parts)
                    {
                        string[] paths = new string[param_parts.size()];
                        for (int i = 0; i < param_parts.size(); i++) paths[i] = new string(param_parts.at(i).get());
                        return new opaque_DirPath { value = System.IO.Path.Combine(paths) };
                    }
                    public opaque_DirPath fattr_workingDir() { return new opaque_DirPath { value = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData) }; }
                    public opaque_DirPath fattr_appDir() { return new opaque_DirPath { value = System.Reflection.Assembly.GetEntryAssembly().Location }; }
                    public opaque_DirPath fattr_rootDir() { return new opaque_DirPath { value = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop) }; }
                }

                public icomp_FileSystem<opaque_FilePath, opaque_DirPath> attr_FileSystem() { return new comp_FileSystem(); }

                //    (~str integer): { integer -> String } "convert integer to string"
                public IahaArray<char> op__str_integer(Int64 param_int) { return new AhaString(param_int.ToString()); }
                //    (~str @Float!Float): { @Float!Float -> String } "convert Float to string (local format)"
                public IahaArray<char> op__str_Float(module_Math.opaque_Float param_float) { return new AhaString(param_float.ToString()); }
                //    (~str @Time!Timestamp): { @Time!Timestamp -> String } "convert Timestamp to string (local format)"
                public IahaArray<char> op__str_Timestamp(module_Time.opaque_Timestamp param_timestamp) { DateTime dt = new DateTime(param_timestamp.ticks); return new AhaString(dt.ToString()); }
                //    (~str @Money!Money): { @Money!Money -> String } "convert Money to string (local format)"
                //    (~int String): { String -> integer } "convert string to integer"
                public Int64 op__int_String(IahaArray<char> param_str) { return Convert.ToInt64(new string(param_str.get())); }
                //    (~float String): { String -> @Float!Float } "convert string (local format) to Float"
                public module_Math.opaque_Float op__float_String(IahaArray<char> param_str) { return new module_Math.opaque_Float() { value = Convert.ToDouble(new string(param_str.get())) }; }
                //    (~date String): { String -> @Time!Timestamp } "convert string (local format) to date"
                public module_Time.opaque_Timestamp op__date_String(IahaArray<char> param_str) { return new module_Time.opaque_Timestamp() { ticks = Convert.ToDateTime(new string(param_str.get())).Date.Ticks }; }
                //    (~time String): { String -> @Time!Interval } "convert string (local format) to time"
                public module_Time.opaque_Interval op__time_String(IahaArray<char> param_str) { return new module_Time.opaque_Interval() { ticks = Convert.ToDateTime(new string(param_str.get())).TimeOfDay.Ticks }; }
                //    (~timestamp String): { String -> @Time!Timestamp } "convert string (local format) to timestamp"
                //    (~money String): { String -> @Money!Money } "convert string (local format) to Money"
                //    (String <= String): { String, String } "compare string in local sorting order"
                public bool op_String_LessEqual_String(IahaArray<char> param_first, IahaArray<char> param_second)
                {
                    return String.Compare(
                        new string(param_first.get()),
                        new string(param_second.get()),
                        StringComparison.CurrentCulture) <= 0;
                }
                //    (String < String): { String, String } "compare string in local sorting order"
                public bool op_String_Less_String(IahaArray<char> param_first, IahaArray<char> param_second)
                {
                    return String.Compare(
                        new string(param_first.get()),
                        new string(param_second.get()),
                        StringComparison.CurrentCulture) < 0;
                }
                public bool op_String_Equal_String(IahaArray<char> param_first, IahaArray<char> param_second)
                {
                    return String.Compare(
                        new string(param_first.get()),
                        new string(param_second.get()),
                        StringComparison.CurrentCulture) == 0;
                }
                public bool op_String_NotEqual_String(IahaArray<char> param_first, IahaArray<char> param_second)
                {
                    return String.Compare(
                        new string(param_first.get()),
                        new string(param_second.get()),
                        StringComparison.CurrentCulture) != 0;
                }
                //    (String > String): { String, String } "compare string in local sorting order"
                public bool op_String_Greater_String(IahaArray<char> param_first, IahaArray<char> param_second)
                {
                    return String.Compare(
                        new string(param_first.get()),
                        new string(param_second.get()),
                        StringComparison.CurrentCulture) > 0;
                }
                //    (String >= String): { String, String } "compare string in local sorting order"
                public bool op_String_GreateEqual_String(IahaArray<char> param_first, IahaArray<char> param_second)
                {
                    return String.Compare(
                        new string(param_first.get()),
                        new string(param_second.get()),
                        StringComparison.CurrentCulture) >= 0;
                }
                //    (FilePath = FilePath): { FilePath, FilePath } "are paths the same?"
                public bool op_FilePath_Equal_FilePath(opaque_FilePath param_first, opaque_FilePath param_second)
                {
                    return String.Compare(
                        param_first.value,
                        param_second.value,
                        StringComparison.InvariantCultureIgnoreCase) == 0;
                }
                //    (DirPath = DirPath): { DirPath, DirPath } "are paths the same?"
                public bool op_DirPath_Equal_DirPath(opaque_DirPath param_first, opaque_DirPath param_second)
                {
                    return String.Compare(
                        param_first.value,
                        param_second.value,
                        StringComparison.InvariantCultureIgnoreCase) == 0;
                }
            }
        }
    }

    namespace Application
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
    {
        public interface icomp_BehaviorParams<opaque_Event>
        {
            IahaArray<char> attr_settings();
            Jobs.Implementation.opaque_Job fattr_output(IahaArray<char> text);
            Jobs.icomp_Engine<opaque_Event, Jobs.Implementation.opaque_Job> attr_engine();
        }

        public interface imod_Application<opaque_Event>
        {
            IahaArray<char> attr_Title();
            IahaArray<char> attr_Signature();
            Jobs.iobj_Behavior<opaque_Event, Jobs.Implementation.opaque_Job> fattr_Behavior(icomp_BehaviorParams<opaque_Event> param_param);
            opaque_Event fattr_Receive(IahaArray<char> param_input);
        }
    }
}
