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

    public class module_Jobs<tpar_Event> : AhaModule
    {
        public delegate void opaque_Job();

        public interface iobj_Behavior : IahaObject<IahaArray<opaque_Job>>
        {
            void action_handle(tpar_Event param_event);
        }

        public delegate tpar_Event func_EnquireTime(module_Time.opaque_Timestamp time);

        public interface icomp_Engine
        {
            opaque_Job fattr_run(opaque_Job job);
            opaque_Job fattr_raise(tpar_Event e);
            //Job delay(double interval, Event e);
            opaque_Job fattr_enquireTime(func_EnquireTime enq);
        }
    }

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
    public class module_Environment : AhaModule
    {
        public struct opaque_FilePath
        {
            string value;
        }

        public struct opaque_DirPath
        {
            string value;
        }

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
            public IahaArray<IahaArray<char>> attr_components() { return new AhaArray<IahaArray<char>>(new IahaArray<char>[] { new AhaString("Aha! for .NET") }); }
        }

        public icomp_Framework attr_Framework() { return new comp_Framework(); }

        public interface icomp_Platform
        {
            bool attr_Windows(); 
            bool attr_MacOSX(); 
            bool attr_Linux(); 
            bool attr_FreeBSD(); 
            bool attr_iOS(); 
            bool attr_Android();
            bool attr_other();
        }

        struct comp_Platform : icomp_Platform
        {
            public bool attr_Windows() { return true; }
            public bool attr_MacOSX() { return false; }
            public bool attr_Linux() { return false; }
            public bool attr_FreeBSD() { return false; }
            public bool attr_iOS() { return false; }
            public bool attr_Android() { return false; }
            public bool attr_other() { return false; }
        }

        public icomp_Platform attr_Platform() { return new comp_Platform(); }

        public interface icomp_Locale
        {
            module_Time.opaque_Interval GMToffset();
            IahaArray<char> attr_country();
            IahaArray<char> attr_language();
            IahaArray<char> attr_currency();
            char attr_decimal();
            //format: @Format!Format "formatting routines"
            //deformat: @Format!Deformat "deformatting routines"
            //charCompare: @StrUtils!CharCompare "character comparison function"
            char fattr_upper(char ch);
            char fattr_lower(char ch); 
        }

        public interface icomp_FileSystem
        {
            IahaArray<char> attr_eol();
            //splitLines: { character* -> String* } "convert sequence of chars to sequence of lines"
            //joinLines: { String* -> character* } "convert sequence of lines to sequence of chars"
            opaque_FilePath fattr_filePath(opaque_DirPath param_dir, IahaArray<char> param_name); 
            opaque_DirPath fattr_subDirPath(opaque_DirPath param_dir, IahaArray<char> param_name);
            opaque_DirPath fattr_parentDirPath(opaque_DirPath param_dir);
            IahaArray<char> fattr_fileName(opaque_FilePath param_path);
            //fileDir: { FilePath -> DirPath } "get file's directory path"
            //fileExt: { FilePath -> String } "extract file's extension (empty string if none)"
            //changeExt: { FilePath, String -> FilePath } "change file's extension"
            //splitDirPath: { DirPath -> [String] } "split directory path into string components"
            //buildDirPath: { [String] -> DirPath } "build directory path from string components"
            //workingDir: DirPath "directory where application can write data"
            //appDir: DirPath "directory from which application has started"
            //rootDir: DirPath "file system's root directory"
        }
    }
}
