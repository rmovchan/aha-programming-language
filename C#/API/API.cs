using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aha.Base;
using Aha.Core;

namespace Aha.API
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
    //            raise: { Event -> Job } "raise event to be immediately handled"
    //            compute: { [ event: { -> Event } fail: Event ] -> Job } "job that computes an event in the background and raises it"
    //            enquireTime: { { @Time!Timestamp -> Event } -> Job } "job that raises event that receives current system time"
    //            delay: { @Time!Interval, Job -> Job } "do job after delay"
    //            schedule: { @Time!Timestamp, Job -> Job } "do job at specific time"
    //            stop: Job "terminate all current jobs"
    //        ] "interface to the job engine"
    //end
    {
        public class opaque_Job<tpar_Event>
        {
            public delegate void Execute();

            public string title;
            public Execute execute;
        }

        public interface iobj_Behavior<tpar_Event> : IahaObject<IahaArray<opaque_Job<tpar_Event>>>
        {
            bool action_handle(tpar_Event param_event);
        }

        public delegate tpar_Event func_EnquireTime<tpar_Event>(Aha.Base.Time.opaque_Timestamp time);

        public interface icomp_ComputeParams<tpar_Event>
        {
            bool fattr_event(out tpar_Event result);
            bool attr_fail(out tpar_Event result);
        }

        public interface icomp_Engine<tpar_Event>
        {
            bool fattr_raise(tpar_Event e, out opaque_Job<tpar_Event> result);
            bool fattr_delay(Aha.Base.Time.opaque_Interval interval, opaque_Job<tpar_Event> job, out opaque_Job<tpar_Event> result);
            bool fattr_schedule(Aha.Base.Time.opaque_Timestamp time, opaque_Job<tpar_Event> job, out opaque_Job<tpar_Event> result);
            bool fattr_enquireTime(func_EnquireTime<tpar_Event> enq, out opaque_Job<tpar_Event> result);
            bool fattr_compute(icomp_ComputeParams<tpar_Event> param, out opaque_Job<tpar_Event> result);
            bool attr_break(out opaque_Job<tpar_Event> result);
            bool attr_shutdown(out opaque_Job<tpar_Event> result);
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
        public struct opaque_FilePath
        {
            public string value;
        }

        public struct opaque_DirPath
        {
            public string value;
        }

        public interface icomp_Version
        {
            bool attr_major(out long result);
            bool attr_minor(out long result);
            bool attr_build(out long result);
        }

        public interface icomp_Framework
        {
            bool attr_name(out IahaArray<char> result);
            bool attr_version(out icomp_Version result);
            bool attr_components(out IahaArray<IahaArray<char>> result);
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
            bool attr_GMToffset(out Aha.Base.Time.opaque_Interval result);
            bool attr_country(out IahaArray<char> result);
            bool attr_language(out IahaArray<char> result);
            bool attr_currency(out IahaArray<char> result);
            bool attr_decimal(out char result);
            //format: @Format!Format "formatting routines"
            //deformat: @Format!Deformat "deformatting routines"
            //charCompare: @StrUtils!CharCompare "character comparison function"
            bool fattr_upper(IahaArray<char> ch, out IahaArray<char> result);
            bool fattr_lower(IahaArray<char> ch, out IahaArray<char> result);
            bool fattr_sameText(IahaArray<char> param_first, IahaArray<char> param_second);
        }

        public interface icomp_FileSystem
        {
            bool attr_eol(out IahaArray<char> result);
            //splitLines: { character* -> String* } "convert sequence of chars to sequence of lines"
            //joinLines: { String* -> character* } "convert sequence of lines to sequence of chars"
            bool fattr_filePath(opaque_DirPath param_dir, IahaArray<char> param_name, out opaque_FilePath result);
            bool fattr_subDirPath(opaque_DirPath param_dir, IahaArray<char> param_name, out opaque_DirPath result);
            bool fattr_parentDirPath(opaque_DirPath param_dir, out opaque_DirPath result);
            bool fattr_fileName(opaque_FilePath param_path, out IahaArray<char> result);
            bool fattr_fileDir(opaque_FilePath param_dir, out opaque_DirPath result);
            bool fattr_fileExt(opaque_FilePath param_path, out IahaArray<char> result);
            bool fattr_changeExt(opaque_FilePath param_dir, IahaArray<char> param_ext, out opaque_FilePath result);
            bool fattr_splitDirPath(opaque_DirPath param_path, out IahaArray<IahaArray<char>> result);
            bool fattr_buildDirPath(IahaArray<IahaArray<char>> param_parts, out opaque_DirPath result);
            bool fattr_workingDir(out opaque_DirPath result);
            bool fattr_appDir(out opaque_DirPath result);
            bool fattr_rootDir(out opaque_DirPath result);
        }

        public interface imod_Environment
        {
            bool attr_Framework(out icomp_Framework result);
            bool attr_Platform(out icomp_Platform result);
            bool attr_Locale(out icomp_Locale result);
            bool attr_FileSystem(out icomp_FileSystem result);
        }

        public class module_Environment : AhaModule, imod_Environment
        {
            struct comp_Version : icomp_Version
            {
                public bool attr_major(out long result) { result = 0; return true; }
                public bool attr_minor(out long result) { result = 1; return true; }
                public bool attr_build(out long result) { result = 1; return true; }
            }

            struct comp_Framework : icomp_Framework
            {
                public bool attr_name(out IahaArray<char> result) { result = new AhaString("Aha! for .NET"); return true; }
                public bool attr_version(out icomp_Version result) { result = new comp_Version(); return true; }
                public bool attr_components(out IahaArray<IahaArray<char>> result) { result = new AhaArray<IahaArray<char>>(new IahaArray<char>[] { }); return true; }
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
                private Aha.Base.Time.opaque_Interval field_GMToffset;
                public Aha.Base.Time.opaque_Interval attr_GMToffset() { return field_GMToffset; }
                public bool attr_country(out IahaArray<char> result) { result = new AhaString(System.Globalization.CultureInfo.CurrentCulture.EnglishName); return true; }
                public bool attr_language(out IahaArray<char> result) { result = new AhaString(System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName); return true; }
                public bool attr_currency(out IahaArray<char> result) { result = new AhaString(System.Globalization.CultureInfo.CurrentCulture.NumberFormat.CurrencySymbol); return true; }
                public bool attr_decimal(out char result) { result = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator[0]; return true; }
                //format: @Format!Format "formatting routines"
                //deformat: @Format!Deformat "deformatting routines"
                //charCompare: @StrUtils!CharCompare "character comparison function"
                public bool fattr_upper(IahaArray<char> param_str, out IahaArray<char> result) { result = new AhaString((new string(param_str.get())).ToUpper(System.Globalization.CultureInfo.CurrentCulture)); return true; }
                public bool fattr_lower(IahaArray<char> param_str, out IahaArray<char> result) { result = new AhaString((new string(param_str.get())).ToLower(System.Globalization.CultureInfo.CurrentCulture)); return true; }
                public bool fattr_sameText(IahaArray<char> param_first, IahaArray<char> param_second)
                {
                    return String.Compare(
                        new string(param_first.get()),
                        new string(param_second.get()),
                        StringComparison.CurrentCultureIgnoreCase) == 0;
                }
                public comp_Locale() 
                { 
                    DateTime now = DateTime.Now; 
                    field_GMToffset = new Aha.Base.Time.opaque_Interval { ticks = now.Ticks - now.ToUniversalTime().Ticks }; 
                }
            }

            public icomp_Locale attr_Locale() { return new comp_Locale(); }

            class comp_FileSystem : icomp_FileSystem
            {
                public bool attr_eol(out IahaArray<char> result) { result = new AhaString("\r\n"); return true; }
                //splitLines: { character* -> String* } "convert sequence of chars to sequence of lines"
                //joinLines: { String* -> character* } "convert sequence of lines to sequence of chars"
                public bool fattr_filePath(opaque_DirPath param_dir, IahaArray<char> param_name, out opaque_FilePath result)
                { result = new opaque_FilePath { value = param_dir.value + System.IO.Path.PathSeparator + (new string(param_name.get())) }; return true; }
                public bool fattr_subDirPath(opaque_DirPath param_dir, IahaArray<char> param_name, out opaque_DirPath result)
                { result = new opaque_DirPath { value = param_dir.value + System.IO.Path.PathSeparator + (new string(param_name.get())) }; return true; }
                public bool fattr_parentDirPath(opaque_DirPath param_dir, out opaque_DirPath result)
                { result = new opaque_DirPath { value = System.IO.Path.GetDirectoryName(param_dir.value) }; return true; }
                public bool fattr_fileName(opaque_FilePath param_path, out IahaArray<char> result)
                { result = new AhaString(System.IO.Path.GetFileName(param_path.value)); return true; }
                public bool fattr_fileDir(opaque_FilePath param_path, out opaque_DirPath result)
                { result = new opaque_DirPath { value = System.IO.Path.GetDirectoryName(param_path.value) }; return true; }
                public bool fattr_fileExt(opaque_FilePath param_path, out IahaArray<char> result)
                { result = new AhaString(System.IO.Path.GetExtension(param_path.value)); return true; }
                public bool fattr_changeExt(opaque_FilePath param_dir, IahaArray<char> param_ext, out opaque_FilePath result)
                { result = new opaque_FilePath { value = System.IO.Path.ChangeExtension(param_dir.value, new string(param_ext.get())) }; return true; }
                public bool fattr_splitDirPath(opaque_DirPath param_path, out IahaArray<IahaArray<char>> result)
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
                    result = new AhaArray<IahaArray<char>>(temp);
                    return true;
                }
                public bool fattr_buildDirPath(IahaArray<IahaArray<char>> param_parts, out opaque_DirPath result)
                {
                    string[] paths = new string[param_parts.size()];
                    for (int i = 0; i < param_parts.size(); i++)
                    {
                        IahaArray<char> path;
                        if (param_parts.at(i, out path))
                            paths[i] = new string(path.get());
                        else
                        {
                            result = default(opaque_DirPath);
                            return false;
                        }
                    }
                    result = new opaque_DirPath { value = System.IO.Path.Combine(paths) };
                    return true;
                }
                public bool fattr_workingDir(out opaque_DirPath result) { result = new opaque_DirPath { value = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData) }; return true; }
                public bool fattr_appDir(out opaque_DirPath result) { result = new opaque_DirPath { value = System.Reflection.Assembly.GetEntryAssembly().Location }; return true; }
                public bool fattr_rootDir(out opaque_DirPath result) { result = new opaque_DirPath { value = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop) }; return true; }
            }

            public bool attr_FileSystem(out icomp_FileSystem result) { result = new comp_FileSystem(); return true; }

            //    (~str integer): { integer -> String } "convert integer to string"
            public bool op__str_integer(long param_int, out IahaArray<char> result) 
            { 
                  result = new AhaString(param_int.ToString()); 
                  return true; 
            }
            //    (~str @Float!Float): { @Float!Float -> String } "convert Float to string (local format)"
            public bool op__str_Float(Aha.Base.Math.opaque_Float param_float, out IahaArray<char> result)
            { 
                result = new AhaString(param_float.value.ToString()); 
                return true; 
            }
            //    (~str @Time!Timestamp): { @Time!Timestamp -> String } "convert Timestamp to string (local format)"
            public bool op__str_Timestamp(Aha.Base.Time.opaque_Timestamp param_timestamp, out IahaArray<char> result) 
            { 
                DateTime dt = new DateTime(param_timestamp.ticks); 
                result = new AhaString(dt.ToString());
                return true;
            }
            //    (~str @Money!Money): { @Money!Money -> String } "convert Money to string (local format)"
            //    (~int String): { String -> integer } "convert string to integer"
            public bool op__int_String(IahaArray<char> param_str, out long result) 
            { 
                result = Convert.ToInt64(new string(param_str.get()));
                return true;
            }
            //    (~float String): { String -> @Float!Float } "convert string (local format) to Float"
            public bool op__float_String(IahaArray<char> param_str, out Aha.Base.Math.opaque_Float result) 
            { 
                result = new Aha.Base.Math.opaque_Float() { value = Convert.ToDouble(new string(param_str.get())) };
                return true;
            }
            //    (~date String): { String -> @Time!Timestamp } "convert string (local format) to date"
            public bool op__date_String(IahaArray<char> param_str, out Aha.Base.Time.opaque_Timestamp result) 
            { 
                result = new Aha.Base.Time.opaque_Timestamp() { ticks = Convert.ToDateTime(new string(param_str.get())).Date.Ticks };
                return true;
            }
            //    (~time String): { String -> @Time!Interval } "convert string (local format) to time"
            public bool op__time_String(IahaArray<char> param_str, out Aha.Base.Time.opaque_Interval result) 
            { 
                result = new Aha.Base.Time.opaque_Interval() { ticks = Convert.ToDateTime(new string(param_str.get())).TimeOfDay.Ticks };
                return true;
            }
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

    namespace FileIOtypes
//doc 
//    Title: "FileIO"
//    Purpose: "File I/O (binary and text)"
//    Package: "Application Program Interface"
//    Author: "Roman Movchan, Melbourne, Australia"
//    Created: "2013-09-05"
//end

//use Time: Base/Time
//type String: [character] "alias for character array type"
//export Types:
//    type FileInfo:
//        [
//            fileType: String "file type"
//            modified: @Time!Timestamp "date/time of last modification"
//            size: integer "size in bytes"
//        ] "detailed file information"
//    type ErrorKind:
//        [
//            access: "access denied" |
//            IO: "permanent I/O error" |
//            notFound: "file or directory doesn't exist" |
//            nameClash: "file name already exists" |
//            outOfMemory: "out of memory" |
//            other: "other"
//        ] "error kind"
//    type ErrorInfo: 
//        [
//            kind: ErrorKind "error kind"
//            message: String "text message"
//        ] "error information"
//    type Encoding:
//        [
//            ANSI: "ANSI" |
//            UTF8: "UTF-8" |
//            UCS2LE: "UCS-2 Little Endian" |
//            UCS2BE: "UCS-2 Big Endian" |
//            auto: "automatic"
//        ] "text encoding"
//end
    {
        public interface icomp_FileInfo
        {
            bool attr_fileType(out IahaArray<char> result);
            bool attr_modified(out Aha.Base.Time.opaque_Timestamp result);
            bool attr_size(out long result);
        }

        public interface icomp_ErrorKind
        {
            bool attr_access();
            bool attr_IO();
            bool attr_notFound();
            bool attr_nameClash();
            bool attr_outOfMemory();
            bool attr_other();
        }

        public interface icomp_ErrorInfo
        {
            bool attr_kind(out icomp_ErrorKind result);
            bool attr_message(out IahaArray<char> result);
        }

        public interface icomp_Encoding
        {
            bool attr_ASCII();
            bool attr_UTF8();
            bool attr_UCS2LE();
            bool attr_UCS2BE();
            bool attr_auto();
        }

    }

    namespace FileIO
//doc 
//    Title: "FileIO"
//    Purpose: "File I/O (binary and text) and file/directory management"
//    Package: "Application Program Interface"
//    Author: "Roman Movchan, Melbourne, Australia"
//    Created: "2013-09-12"
//end

//type Event: arbitrary "custom event type"
//use Jobs: API/Jobs(Event: Event)
//import Jobs(Types)
//use Env: API/Environment
//import Env(Types)
//use Bits: Base/Bits
//import Bits(Types)
//use Types: API/FileIOtypes
//import Types(Types)
//type String: [character] "alias for character array type"
//type ReadParams:
//    [ 
//        position: [ top: integer | bottom: integer | next: ] "position: from top (bytes), bottom (bytes) or current"
//        bytes: integer "number of bytes" 
//        result: { Bits -> Event } "event that receives bytes read"
//    ] "read given number of bytes at given position" 
//type WriteParams:
//    [ 
//        position: [ top: integer | bottom: integer | next: ] "position: from top (bytes), bottom (bytes) or current"
//        data: Bits "data to write (must be whole number of bytes)" 
//        written: Event "event raised upon writing"
//    ] "write data at given position" 
//type ReaderCommand:
//    [
//        read: ReadParams "read data" |
//        close: Event "release file for write operations and raise event"
//    ] "reader control commands"
//type WriterCommand:
//    [
//        write: WriteParams "write data" |
//        close: Event "release file for other operations and raise event"
//    ] "writer control commands"
//type Reader: { ReaderCommand -> Job } "return reader jobs"
//type Writer: { WriterCommand -> Job } "return writer jobs"
//type FileMngmt:
//    [
//        findFile: { [ path: FilePath yes: Event no: Event ] -> Job } "check if file with given path exists"
//        getFileInfo: { [ path: FilePath success: { FileInfo -> Event } error: { ErrorInfo -> Event } ] -> Job } 
//            "get detailed file information"
//        makeFile: { [ path: FilePath success: Event error: { ErrorInfo -> Event } ] -> Job } 
//            "create an empty file"
//        renameFile: { [ path: FilePath to: String success: Event error: { ErrorInfo -> Event } ] -> Job } 
//            "rename file"
//        deleteFile: { [ path: FilePath success: Event error: { ErrorInfo -> Event } ] -> Job } 
//            "delete a file"
//        findDir: { [ path: DirPath yes: Event no: Event ] -> Job } "check if directory with given path exists"
//        listDir: { [ path: DirPath success: { [ files: [String] dirs: [String] ] -> Event } error: { ErrorInfo -> Event } ] -> Job } 
//            "list files and subdirectories in given directory"
//        makeDir: { [ path: DirPath success: Event error: { ErrorInfo -> Event } ] -> Job } 
//            "create a directory"
//        renameDir: { [ path: DirPath to: String success: Event error: { ErrorInfo -> Event } ] -> Job } 
//            "rename directory"
//        deleteDir: { [ path: DirPath success: Event error: { ErrorInfo -> Event } ] -> Job } 
//            "delete a directory"
//    ] "file management jobs"
//type DirChange:
//    [
//        newFile: String "new file created" |
//        newDir: String "new subdirectory created" |
//        modified: String "file modified" |
//        deleted: String "file/subdirectory deleted" 
//    ] "changes in a directory"
//export Utils:
//    the CreateReader: { [ path: FilePath engine: Engine success: { Reader -> Event } error: { ErrorInfo -> Event } ] -> Job } "create file reader"
//    the CreateWriter: { [ path: FilePath engine: Engine success: { Writer -> Event } error: { ErrorInfo -> Event } ] -> Job } "create file writer"
//    the ReadText: 
//            { 
//                [ 
//                    path: FilePath 
//                    encoding: Encoding 
//                    success: { [ content: character* size: integer encoding: Encoding ] -> Event } 
//                    error: { ErrorInfo -> Event } 
//                    engine: Engine
//                ] -> Job 
//            } "job that returns text file content as a character sequence"
//    the WriteText: 
//            { 
//                [ 
//                    path: FilePath 
//                    encoding: Encoding 
//                    content: character*
//                    size: integer
//                    success: Event
//                    error: { ErrorInfo -> Event } 
//                    engine: Engine
//                ] -> Job 
//            } "job that creates text file from a character sequence"
//    the FileMngmt: { Engine -> FileMngmt } "obtain file management interface"
//    the DirWatch: { [ path: DirPath watch: { DirChange -> Event } error: { ErrorInfo -> Event } engine: Engine ] -> Job } 
//        "raise events when watched directory changes"
//    the DeleteWatch: { [ path: DirPath success: Event error: { ErrorInfo -> Event } engine: Engine ] -> Job } "delete directory watch"     
//end
    {
        public interface icomp_ReadParams<tpar_Event>
        {
            bool attr_position(out icomp_Position<tpar_Event> result);
            bool attr_bytes(out long result);
            bool attr_result(Aha.Base.Bits.opaque_BitString data, out tpar_Event result);
        }

        public interface icomp_WriteParams<tpar_Event>
        {
            bool attr_position(out icomp_Position<tpar_Event> result);
            bool attr_data(out Aha.Base.Bits.opaque_BitString result);
            bool attr_written(out tpar_Event result);
        }

        public interface icomp_Position<tpar_Event>
        {
            bool attr_top(out long result);
            bool attr_bottom(out long result);
            bool attr_next();
        }

        public interface icomp_ReaderCommand<tpar_Event>
        {
            bool attr_read(out icomp_ReadParams<tpar_Event> result);
            bool attr_close(out tpar_Event result);
        }

        public interface icomp_WriterCommand<tpar_Event>
        {
            bool attr_write(out icomp_WriteParams<tpar_Event> result);
            bool attr_close(out tpar_Event result);
        }

        public delegate bool func_Reader<tpar_Event>(icomp_ReaderCommand<tpar_Event> cmd, out Jobs.opaque_Job<tpar_Event> result);

        public delegate bool func_Writer<tpar_Event>(icomp_WriterCommand<tpar_Event> cmd, out Jobs.opaque_Job<tpar_Event> result);

        public interface imod_FileIO<tpar_Event>
        {
            bool attr_CreateReader(icomp_CreateReaderParam<tpar_Event> param, out Jobs.opaque_Job<tpar_Event> result);
            bool attr_CreateWriter(icomp_CreateWriterParam<tpar_Event> param, out Jobs.opaque_Job<tpar_Event> result);
            bool attr_ReadText(icomp_ReadTextParam<tpar_Event> param, out Jobs.opaque_Job<tpar_Event> result);
            bool attr_WriteText(icomp_WriteTextParam<tpar_Event> param, out Jobs.opaque_Job<tpar_Event> result);
        }

        public interface icomp_CreateReaderParam<tpar_Event>
        {
            bool attr_path(out Aha.API.Environment.opaque_FilePath result);
            bool attr_engine(out Aha.API.Jobs.icomp_Engine<tpar_Event> result);
            bool attr_success(func_Reader<tpar_Event> reader, out tpar_Event result);
            bool attr_error(FileIOtypes.icomp_ErrorInfo error, out tpar_Event result);
        }

        public interface icomp_CreateWriterParam<tpar_Event>
        {
            bool attr_path(out Aha.API.Environment.opaque_FilePath result);
            bool attr_engine(out Aha.API.Jobs.icomp_Engine<tpar_Event> result);
            bool attr_success(func_Writer<tpar_Event> writer, out tpar_Event result);
            bool attr_error(FileIOtypes.icomp_ErrorInfo error, out tpar_Event result);
        }

        public interface icomp_ReadTextParam<tpar_Event>
        {
            bool attr_path(out Aha.API.Environment.opaque_FilePath result);
            bool attr_engine(out Aha.API.Jobs.icomp_Engine<tpar_Event> result);
            bool attr_encoding(out FileIOtypes.icomp_Encoding result);
            bool fattr_success(icomp_TextReadParams<tpar_Event> result, out tpar_Event result);
            bool fattr_error(FileIOtypes.icomp_ErrorInfo error, out tpar_Event result);
        }

        public interface icomp_TextReadParams<tpar_Event>
        {
            bool attr_content(out IahaSequence<char> result);
            bool attr_size(out long result);
            bool attr_encoding(out FileIOtypes.icomp_Encoding result);
        }

        public interface icomp_WriteTextParam<tpar_Event>
        {
            bool attr_path(out Aha.API.Environment.opaque_FilePath result);
            bool attr_engine(out Aha.API.Jobs.icomp_Engine<tpar_Event> result);
            bool attr_content(out IahaSequence<char> result);
            bool attr_size(out long result);
            bool attr_encoding(out FileIOtypes.icomp_Encoding result);
            bool attr_success(out tpar_Event result);
            bool attr_error(FileIOtypes.icomp_ErrorInfo error, out tpar_Event result);
        }

        public class module_FileIO<tpar_Event> : AhaModule, imod_FileIO<tpar_Event>
        {
            class ErrorInfo : Aha.API.FileIOtypes.icomp_ErrorKind, Aha.API.FileIOtypes.icomp_ErrorInfo
            {
                private System.Exception field_ex;
                public bool attr_access() { return field_ex is System.Security.SecurityException || field_ex is System.UnauthorizedAccessException; }
                public bool attr_IO() { return field_ex is System.IO.IOException; }
                public bool attr_notFound() { return field_ex is System.IO.FileNotFoundException || field_ex is System.IO.DirectoryNotFoundException; }
                public bool attr_nameClash() { return field_ex is System.IO.IOException; } //TODO
                public bool attr_outOfMemory() { return field_ex is System.OutOfMemoryException; }
                public bool attr_other() { return !(field_ex is System.Security.SecurityException || field_ex is System.UnauthorizedAccessException || field_ex is System.IO.IOException || field_ex is System.IO.FileNotFoundException || field_ex is System.IO.DirectoryNotFoundException || field_ex is System.OutOfMemoryException); }
                public bool attr_kind(out FileIOtypes.icomp_ErrorKind result) 
                { 
                    result = this;
                    return true;
                }
                public bool attr_message(out IahaArray<char> result) { result = new AhaString(field_ex.Message); return true; }
                public ErrorInfo(System.Exception param_ex)
                {
                    field_ex = param_ex;
                }
            }
            class Reader
            {
                private icomp_CreateReaderParam<tpar_Event> field_param;
                private System.IO.FileStream stream = null;
                private async void read(icomp_ReadParams<tpar_Event> p)
                {
                    long ipos;
                    icomp_Position<tpar_Event> pos;
                    Environment.opaque_FilePath path;
                    Jobs.icomp_Engine<tpar_Event> engine;
                    tpar_Event evt;
                    Jobs.opaque_Job<tpar_Event> job;
                    if (field_param.attr_engine(out engine))
                        try
                        {
                            if (stream == null && field_param.attr_path(out path))
                            {
                                stream = new System.IO.FileStream(path.value, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read);
                            }
                            if (p.attr_position(out pos) && pos.attr_top(out ipos))
                                stream.Position = ipos;
                            else
                                if (pos.attr_bottom(out ipos))
                                    stream.Position = stream.Length - ipos;
                            long bytes;
                            if (p.attr_bytes(out bytes))
                            {
                                byte[] data = new byte[bytes];
                                int byteCount = await stream.ReadAsync(data, 0, (int)bytes);
                                if (byteCount != bytes) { Array.Resize<byte>(ref data, byteCount); }
                                Aha.Base.Bits.opaque_BitString bits = new Base.Bits.opaque_BitString { bytes = data, bits = byteCount * 8 };
                                if (p.attr_result(bits, out evt) && engine.fattr_raise(evt, out job))
                                    job.execute();
                            }
                        }
                        catch(System.Exception ex)
                        {
                            if (field_param.attr_error(new ErrorInfo(ex), out evt) && engine.fattr_raise(evt, out job))
                                job.execute();
                        }
                }
                private void close()
                {
                    if (stream != null)
                    {
                        stream.Dispose();
                        stream = null;
                    }
                }
                public bool func_Reader(icomp_ReaderCommand<tpar_Event> cmd, out Jobs.opaque_Job<tpar_Event> Job)
                {
                    Environment.opaque_FilePath path;
                    icomp_ReadParams<tpar_Event> p;
                    if (field_param.attr_path(out path) && cmd.attr_read(out p))
                    {
                        Job = new Jobs.opaque_Job<tpar_Event>
                        {
                            title = "Reader.read " + path.value,
                            execute = delegate() { read(p); }
                        };
                    }
                    else
                    {
                        Job = new Jobs.opaque_Job<tpar_Event>
                        {
                            title = "Reader.close " + path.value,
                            execute = close
                        };
                    }
                    return true;
                }
                public Reader(icomp_CreateReaderParam<tpar_Event> param)
                {
                    field_param = param;
                }
            }
            class Writer
            {
                private icomp_CreateWriterParam<tpar_Event> field_param;
                private System.IO.FileStream stream = null;
                private async void write(icomp_WriteParams<tpar_Event> p)
                {
                    long ipos;
                    icomp_Position<tpar_Event> pos;
                    Environment.opaque_FilePath path;
                    Jobs.icomp_Engine<tpar_Event> engine;
                    tpar_Event evt;
                    Jobs.opaque_Job<tpar_Event> job;
                    if (field_param.attr_engine(out engine))
                        try
                        {
                            if (stream == null && field_param.attr_path(out path))
                            {
                                stream = new System.IO.FileStream(path.value, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Write, System.IO.FileShare.None);
                            }
                            if (p.attr_position(out pos) && pos.attr_top(out ipos))
                                stream.Position = ipos;
                            else
                                if (pos.attr_bottom(out ipos))
                                    stream.Position = stream.Length - ipos;
                            Base.Bits.opaque_BitString bits;
                            if (p.attr_data(out bits))
                            {
                                byte[] data = bits.bytes;
                                await stream.WriteAsync(data, 0, data.Length);
                                if (p.attr_written(out evt) && engine.fattr_raise(evt, out job))
                                    job.execute();
                            }
                        }
                        catch (System.Exception ex)
                        {
                            if (field_param.attr_error(new ErrorInfo(ex), out evt) && engine.fattr_raise(evt, out job))
                                job.execute();
                        }
                }
                private void close()
                {
                    if (stream != null)
                    {
                        stream.Dispose();
                        stream = null;
                    }
                }
                public bool func_Writer(icomp_WriterCommand<tpar_Event> cmd, out Jobs.opaque_Job<tpar_Event> job)
                {
                    Environment.opaque_FilePath path;
                    icomp_WriteParams<tpar_Event> p;
                    if (field_param.attr_path(out path) && cmd.attr_write(out p))
                    {
                        job = new Jobs.opaque_Job<tpar_Event>
                        {
                            title = "Writer.write " + path.value,
                            execute =
                                delegate() { write(p); }
                        };
                    }
                    else
                    {
                        job = new Jobs.opaque_Job<tpar_Event>
                        {
                            title = "Writer.close " + path.value,
                            execute = close
                        };
                    }
                    return true;
                }
                public Writer(icomp_CreateWriterParam<tpar_Event> param)
                {
                    field_param = param;
                }
            }
            private struct comp_TextReadParam : icomp_TextReadParams<tpar_Event>
            {
                public IahaSequence<char> field_content;
                public long field_size;
                public FileIOtypes.icomp_Encoding field_encoding;
                public IahaSequence<char> attr_content() { return field_content; }
                public long attr_size() { return field_size; }
                public FileIOtypes.icomp_Encoding attr_encoding() { return field_encoding; }
            }
            public Jobs.opaque_Job<tpar_Event> attr_CreateReader(icomp_CreateReaderParam<tpar_Event> param)
            {
                Reader reader = new Reader(param);
                Environment.opaque_FilePath path;
                Jobs.icomp_Engine<tpar_Event> engine;
                tpar_Event evt;
                Jobs.opaque_Job<tpar_Event> job;
                param.attr_engine(out engine);
                param.attr_path(out path);
                param.attr_success(reader.func_Reader, out evt);
                engine.fattr_raise(evt, out job);
                return new Jobs.opaque_Job<tpar_Event>
                {
                    title = "CreateReader " + path.value,
                    execute =
                        delegate()
                        {
                            job.execute();
                        }
                };
            }
            public Jobs.opaque_Job<tpar_Event> attr_CreateWriter(icomp_CreateWriterParam<tpar_Event> param)
            {
                Writer writer = new Writer(param);
                Environment.opaque_FilePath path;
                Jobs.icomp_Engine<tpar_Event> engine;
                tpar_Event evt;
                Jobs.opaque_Job<tpar_Event> job;
                param.attr_engine(out engine);
                param.attr_path(out path);
                param.attr_success(writer.func_Writer, out evt);
                engine.fattr_raise(evt, out job);
                return new Jobs.opaque_Job<tpar_Event>
                {
                    title = "CreateWriter " + path.value,
                    execute =
                        delegate()
                        {
                            job.execute();
                        }
                };
            }
            struct FileText : IahaSequence<char>
            {
                public List<string> list;
                public int index;
                public int block;
                public char state() { return list[block][index]; }
                public IahaObject<char> copy() { FileText clone = new FileText { list = list, index = index, block = block }; return clone; }
                public void action_skip() { if (index == list[block].Length) { index = 0; block++; } else index++; }
                public char first(Predicate<char> that, long max) 
                { 
                    long j = 0; 
                    int i = index; 
                    int b = block; 
                    char item = list[block][index]; 
                    while (j < max) 
                    { 
                        item = list[b][i]; 
                        if (that(item)) return item;
                        if (i == list[b].Length) { i = 0; b++; } else i++;
                        j++; 
                    } 
                    throw Failure.One; 
                }
            }
            struct FileEncoding : FileIOtypes.icomp_Encoding
            {
                public Encoding field_encoding;
                bool FileIOtypes.icomp_Encoding.attr_ASCII()
                {
                    return field_encoding.Equals(Encoding.ASCII);
                }

                bool FileIOtypes.icomp_Encoding.attr_UTF8()
                {
                    return field_encoding.Equals(Encoding.UTF8);
                }

                bool FileIOtypes.icomp_Encoding.attr_UCS2LE()
                {
                    return field_encoding.Equals(Encoding.Unicode);
                }

                bool FileIOtypes.icomp_Encoding.attr_UCS2BE()
                {
                    return field_encoding.Equals(Encoding.BigEndianUnicode);
                }

                bool FileIOtypes.icomp_Encoding.attr_auto()
                {
                    return field_encoding.Equals(Encoding.Default);
                }
            }
            public Jobs.opaque_Job<tpar_Event> attr_ReadText(icomp_ReadTextParam<tpar_Event> param)
            {

                Environment.opaque_FilePath path;
                param.attr_path(out path);
                FileIOtypes.icomp_Encoding enc;
                param.attr_encoding(out enc);
                Jobs.icomp_Engine<tpar_Event> engine;
                param.attr_engine(out engine);
                tpar_Event evt;
                Jobs.opaque_Job<tpar_Event> job;
                return new Jobs.opaque_Job<tpar_Event>
                {
                    title = "ReadText " + path.value,
                    execute =
                        async delegate ()
                        {
                            System.IO.FileStream stream = null;
                            const int block = 8192;
                            System.Text.Encoding encoding;
                            try
                            {
                                stream = new System.IO.FileStream(path.value, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read);
                                FileText text = new FileText() { list = new List<string>(), block = 0, index = 0 };
                                if (enc.attr_ASCII())
                                { encoding = Encoding.ASCII; }
                                else
                                    if (enc.attr_UTF8())
                                    { encoding = Encoding.UTF8; }
                                    else
                                        if (enc.attr_UCS2LE())
                                        { encoding = Encoding.Unicode; }
                                        else
                                            if (enc.attr_UCS2BE())
                                            { encoding = Encoding.BigEndianUnicode; }
                                            else
                                            { encoding = null; } 
                                while (stream.Position < stream.Length)
                                {
                                    byte[] data = new byte[block];
                                    int byteCount = await stream.ReadAsync(data, 0, block);
                                    if (byteCount != block) { Array.Resize<byte>(ref data, byteCount); }
                                    if (encoding == null)
                                    {
                                        if (byteCount > 2 && data[0] == 0xEF && data[1] == 0xBB && data[2] == 0xBF)
                                            encoding = Encoding.UTF8;
                                        else
                                            if (byteCount > 1 && data[0] == 0xFE && data[1] == 0xFF)
                                                encoding = Encoding.BigEndianUnicode;
                                            else
                                                if (byteCount > 1 && data[0] == 0xFF && data[1] == 0xFE)
                                                    encoding = Encoding.Unicode;
                                                else
                                                    encoding = Encoding.Default;
                                        text.index = encoding.GetCharCount(encoding.GetPreamble());
                                    }
                                    text.list.Add(encoding.GetString(data));
                                }
                                comp_TextReadParam p = new comp_TextReadParam() { field_encoding = new FileEncoding { field_encoding = encoding }, field_size = stream.Length, field_content = text };
                                param.fattr_success(p, out evt);
                                engine.fattr_raise(evt, out job);
                                job.execute();
                            }
                            catch (System.Exception ex)
                            {
                                param.fattr_error(new ErrorInfo(ex), out evt);
                                engine.fattr_raise(evt, out job);
                                job.execute();
                            }
                            finally
                            {
                                if (stream != null) stream.Dispose();
                            }
                        }
                };
            }
            public Jobs.opaque_Job<tpar_Event> attr_WriteText(icomp_WriteTextParam<tpar_Event> param)
            {

                return new Jobs.opaque_Job<tpar_Event>
                {
                    title = "WriteText " + param.attr_path().value,
                    execute =
                        async delegate()
                        {
                            System.IO.FileStream stream = null;
                            const int block = 4096;
                            System.Text.Encoding encoding;
                            try
                            {
                                stream = new System.IO.FileStream(param.attr_path().value, System.IO.FileMode.CreateNew, System.IO.FileAccess.Write, System.IO.FileShare.None);
                                if (param.attr_encoding().attr_ASCII())
                                { encoding = Encoding.ASCII; }
                                else
                                    if (param.attr_encoding().attr_UTF8())
                                    { encoding = Encoding.UTF8; }
                                    else
                                        if (param.attr_encoding().attr_UCS2LE())
                                        { encoding = Encoding.Unicode; }
                                        else
                                            if (param.attr_encoding().attr_UCS2BE())
                                            { encoding = Encoding.BigEndianUnicode; }
                                            else
                                            { encoding = Encoding.Default; }
                                byte[] BOM = encoding.GetPreamble();
                                if (BOM.Length > 0)
                                {
                                    await stream.WriteAsync(BOM, 0, BOM.Length);
                                }
                                char[] data = new char[block];
                                long left = param.attr_size();
                                int size = block;
                                IahaSequence<char> seq = (IahaSequence<char>)param.attr_content().copy();
                                while (left > 0)
                                {
                                    if (left < block) size = (int)left;
                                    for (int i = 0; i < size; i++)
                                    {
                                        try
                                        {
                                            data[i] = seq.state();
                                            try { seq.action_skip(); }
                                            catch (Failure) { size = i + 1; left = size; }
                                        }
                                        catch (Failure) { size = i; left = size; }
                                    }
                                    if (size != block) Array.Resize<char>(ref data, size);
                                    byte[] buf = encoding.GetBytes(data);
                                    await stream.WriteAsync(buf, 0, buf.Length);
                                    left -= size;
                                }
                                param.attr_engine().fattr_raise(param.attr_success()).execute();
                            }
                            catch (System.Exception ex)
                            {
                                param.attr_engine().fattr_raise(param.attr_error(new ErrorInfo(ex))).execute();
                            }
                            finally
                            {
                                if (stream != null) stream.Dispose();
                            }
                        }
                };
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
    //the Behavior: { [ settings: [character] password: [character] output: { [character] -> @Jobs!Job } engine: @Jobs!Engine ] -> @Jobs!Behavior } "application behavior"
    //the Receive: { [character] -> Event } "convert user input to events"
    {
        public class opaque_Event { }

        public interface icomp_BehaviorParams
        {
            IahaArray<char> attr_settings();
            IahaArray<char> attr_password();
            Jobs.opaque_Job<opaque_Event> fattr_output(IahaArray<char> text);
            Jobs.icomp_Engine<opaque_Event> attr_engine();
        }

        public interface imod_Application
        {
            IahaArray<char> attr_Title();
            IahaArray<char> attr_Signature();
            Jobs.iobj_Behavior<opaque_Event> fattr_Behavior(icomp_BehaviorParams param_param);
            opaque_Event fattr_Receive(IahaArray<char> param_input);
        }
    }

    namespace Process
//doc 
//    Title: "Process"
//    Purpose: "Use a component that runs a job"
//    Package: "Application Program Interface"
//    Author: "Roman Movchan, Melbourne, Australia"
//    Created: "2013-09-06"
//end

//type Settings: arbitrary "component settings"
//type Output: arbitrary "component output"
//type Event: arbitrary "client's event type"
//use Jobs: API/Jobs(Event: Event)
//the CreateProcess: { [ classname: [character] password: [character] engine: @Jobs!Engine settings: Settings output: { Output -> Event } ] -> @Jobs!Job } "return job that creates process"
    {
        public interface icomp_ProcessParam<tpar_Settings, tpar_Output, tpar_Event>
        {
            IahaArray<char> attr_classname();
            IahaArray<char> attr_password();
            Aha.API.Jobs.icomp_Engine<tpar_Event> attr_engine();
            tpar_Settings attr_settings();
            tpar_Event fattr_output(tpar_Output output);
        }

        public interface imod_Process<tpar_Settings, tpar_Output, tpar_Event> 
            where tpar_Event : ProcessDef.base_Event
            where tpar_Output : ProcessDef.base_Output
            where tpar_Settings : ProcessDef.base_Settings
        {
            Aha.API.Jobs.opaque_Job<tpar_Event> fattr_Create(icomp_ProcessParam<tpar_Settings, tpar_Output, tpar_Event> param);
        }

        public class module_Process<tpar_Settings, tpar_Output, tpar_Event> : AhaModule, imod_Process<tpar_Settings, tpar_Output, tpar_Event>
            where tpar_Event : ProcessDef.base_Event
            where tpar_Output : ProcessDef.base_Output
            where tpar_Settings : ProcessDef.base_Settings
        {
            delegate tpar_Event func_Output(tpar_Output output);

            class comp_BehaviorParams : ProcessDef.icomp_BehaviorParams
            {
                private ProcessDef.base_Settings field_settings;
                private IahaArray<char> field_password;
                private func_Output field_output;
                private Aha.API.Jobs.icomp_Engine<tpar_Event> field_engine;
                private Aha.Engine.comp_Engine<ProcessDef.base_Event> field_engine2;
                public ProcessDef.base_Settings attr_settings() { return field_settings; }
                public IahaArray<char> attr_password() { return field_password; }
                public Jobs.opaque_Job<ProcessDef.base_Event> fattr_output(ProcessDef.base_Output output) 
                {
                    return new Jobs.opaque_Job<ProcessDef.base_Event>
                    {
                        title = "output",
                        execute =
                            delegate()
                            {
                                field_engine.fattr_raise(field_output((tpar_Output)output)).execute();
                            }
                    };
                }
                public Jobs.icomp_Engine<ProcessDef.base_Event> attr_engine() { return field_engine2; }
                public comp_BehaviorParams
                    (
                        ProcessDef.base_Settings param_settings, 
                        IahaArray<char> param_password, 
                        func_Output param_output, 
                        Aha.API.Jobs.icomp_Engine<tpar_Event> param_engine,
                        Aha.Engine.comp_Engine<ProcessDef.base_Event> param_engine2
                    ) 
                {
                    field_settings = param_settings;
                    field_password = param_password;
                    field_output = param_output;
                    field_engine = param_engine;
                    field_engine2 = param_engine2;
                }
            }

            public Aha.API.Jobs.opaque_Job<tpar_Event> fattr_Create(icomp_ProcessParam<tpar_Settings, tpar_Output, tpar_Event> param)
            {
                string classname = new string(param.attr_classname().get());
                return new Jobs.opaque_Job<tpar_Event>
                {
                    title = "Create " + classname,
                    execute =
                        delegate()
                        {
                            string path = (string)Microsoft.Win32.Registry.GetValue("HKEY_CURRENT_USER\\Software\\Aha! for .NET\\Components\\" + classname, "Path", "");
                            if (!path.Equals(""))
                            {
                                try
                                {
                                    System.Reflection.Assembly assembly = System.Reflection.Assembly.LoadFrom(path);
                                    Type settingsType = assembly.GetType("opaque_Settings", true, false);
                                    Type outputType = assembly.GetType("opaque_Output", true, false);
                                    if (settingsType.IsAssignableFrom(typeof(tpar_Settings)) && typeof(tpar_Output).IsAssignableFrom(outputType))
                                    {
                                        Type eventType = assembly.GetType("opaque_Event", true, false);
                                        Type engType = typeof(Aha.Engine.comp_Engine<>).MakeGenericType(new Type[] { eventType });
                                        Object eng = Activator.CreateInstance(engType);
                                        func_Output output = param.fattr_output;
                                        comp_BehaviorParams bp = new comp_BehaviorParams
                                            (
                                                param.attr_settings(),
                                                param.attr_password(),
                                                output, 
                                                param.attr_engine(),
                                                (Aha.Engine.comp_Engine<ProcessDef.base_Event>)eng
                                            );
                                        foreach (Type type in assembly.ExportedTypes)
                                        {
                                            if (type.IsClass)
                                            {
                                                try
                                                {
                                                    ProcessDef.imod_ProcessDef proc = Activator.CreateInstance(type) as ProcessDef.imod_ProcessDef;
                                                    if (proc != null)
                                                    {
                                                        engType.InvokeMember
                                                            (
                                                                "StartExternal",
                                                                System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.InvokeMethod,
                                                                null,
                                                                eng,
                                                                new Object[] { proc.fattr_Behavior(bp) }
                                                            );
                                                        return;
                                                    }
                                                }
                                                catch (System.Exception) { }
                                            }
                                        }
                                        // "Error: assembly doesn't contain an Aha! component";
                                    }
                                    //Error: type parameters mismatch
                                }
                                catch (System.Exception) { } //Error: type parameters don't match
                            }
                        } // Error: classname is not registered
                };
            }
        }

        namespace ProcessDef
//doc 
//    Title: "ProcessDef"
//    Purpose: "Definition of a process component"
//    Package: "Application Program Interface"
//    Author: "Roman Movchan, Melbourne, Australia"
//    Created: "2013-09-06"
//end

//type Settings: opaque "component's settings (set at creation of an instance)"
//type Output: opaque "component's output (created using an output job)"
//type Event: opaque "custom event type"
//use Jobs: API/Jobs<Event: Event>
//the Title: [character] "component's title"  
//the Behavior: { [ settings: Settings password: [character] output: { Output -> @Jobs!Job } engine: @Jobs!Engine ] -> @Jobs!Behavior } "component's behavior"
        {
            public class base_Settings { }

            public class base_Output { }

            public class base_Event { }

            public interface icomp_BehaviorParams
            {
                base_Settings attr_settings();
                IahaArray<char> attr_password();
                Jobs.opaque_Job<base_Event> fattr_output(base_Output text);
                Jobs.icomp_Engine<base_Event> attr_engine();
            }

            public interface imod_ProcessDef
            {
                IahaArray<char> attr_Title();
                IahaArray<char> attr_Signature();
                Jobs.iobj_Behavior<base_Event> fattr_Behavior(icomp_BehaviorParams param_param);
            }
        }

    }
}
