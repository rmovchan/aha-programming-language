using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aha.Package.Base;
using Aha.Core;

namespace Aha.Package.API
{
    namespace ErrorInfo
    {
        public interface icomp_ErrorInfo<tpar_ErrorKind>
        {
            bool attr_kind(out tpar_ErrorKind result);
            bool attr_message(out IahaArray<char> result);
        }
    }

    namespace Environment
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
            bool attr_Windows();
            bool attr_MacOSX();
            bool attr_Linux();
            bool attr_FreeBSD();
            bool attr_iOS();
            bool attr_Android();
            bool attr_other();
        }

        public interface icomp_Locale
        {
            bool attr_GMToffset(out Aha.Package.Base.Time.opaque_Interval result);
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
            bool op__str_integer(long param_int, out IahaArray<char> result);
            bool op__str_Float(Aha.Package.Base.Float.opaque_Float param_float, out IahaArray<char> result);
            bool op__str_Timestamp(Aha.Package.Base.Time.opaque_Timestamp param_timestamp, out IahaArray<char> result);
            bool op__int_String(IahaArray<char> param_str, out long result);
            bool op__float_String(IahaArray<char> param_str, out Aha.Package.Base.Float.opaque_Float result);
            bool op__date_String(IahaArray<char> param_str, out Aha.Package.Base.Time.opaque_Timestamp result);
            bool op__time_String(IahaArray<char> param_str, out Aha.Package.Base.Time.opaque_Interval result);
            bool op_String_LessEqual_String(IahaArray<char> param_first, IahaArray<char> param_second);
            bool op_String_Less_String(IahaArray<char> param_first, IahaArray<char> param_second);
            bool op_String_Equal_String(IahaArray<char> param_first, IahaArray<char> param_second);
            bool op_String_NotEqual_String(IahaArray<char> param_first, IahaArray<char> param_second);
            bool op_String_Greater_String(IahaArray<char> param_first, IahaArray<char> param_second);
            bool op_String_GreateEqual_String(IahaArray<char> param_first, IahaArray<char> param_second);
            bool op_FilePath_Equal_FilePath(Aha.Package.API.Environment.opaque_FilePath param_first, Aha.Package.API.Environment.opaque_FilePath param_second);
            bool op_DirPath_Equal_DirPath(Aha.Package.API.Environment.opaque_DirPath param_first, Aha.Package.API.Environment.opaque_DirPath param_second);
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
    }

    namespace Jobs
    {
        public struct opaque_Job<tpar_Event>
        {
            public delegate void Execute();

            public string title;
            public Execute execute;
        }

        public delegate tpar_Event func_EnquireTime<tpar_Event>(Aha.Package.Base.Time.opaque_Timestamp time);

        public interface icomp_ComputeParams<tpar_Event>
        {
            bool fattr_event(out tpar_Event result);
            bool attr_fail(out tpar_Event result);
        }

        public interface icomp_Engine<tpar_Event>
        {
            bool attr_framework(out Environment.icomp_Framework result);
            bool attr_platform(out Environment.icomp_Platform result);
            bool attr_locale(out Environment.icomp_Locale result);
            bool attr_fileSystem(out Environment.icomp_FileSystem result);
            bool fattr_raise(tpar_Event e, out opaque_Job<tpar_Event> result);
            bool fattr_delay(Aha.Package.Base.Time.opaque_Interval interval, opaque_Job<tpar_Event> job, out opaque_Job<tpar_Event> result);
            bool fattr_schedule(Aha.Package.Base.Time.opaque_Timestamp time, opaque_Job<tpar_Event> job, out opaque_Job<tpar_Event> result);
            bool fattr_enquireTime(func_EnquireTime<tpar_Event> enq, out opaque_Job<tpar_Event> result);
            bool fattr_compute(icomp_ComputeParams<tpar_Event> param, out opaque_Job<tpar_Event> result);
            bool fattr_log(IahaArray<char> message, out Aha.Package.API.Jobs.opaque_Job<tpar_Event> result);
            bool attr_break(out opaque_Job<tpar_Event> result);
            bool attr_enable(out opaque_Job<tpar_Event> result);
            bool attr_disable(out opaque_Job<tpar_Event> result);
        }
    }

    namespace FileIOtypes
    {
        public interface icomp_FileInfo
        {
            bool attr_fileType(out IahaArray<char> result);
            bool attr_modified(out Aha.Package.Base.Time.opaque_Timestamp result);
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
    {
        public interface icomp_ReadParams<tpar_Event>
        {
            bool attr_position(out icomp_Position<tpar_Event> result);
            bool attr_bytes(out long result);
            bool fattr_result(Aha.Package.Base.Bits.opaque_BitString data, out tpar_Event result);
        }

        public interface icomp_WriteParams<tpar_Event>
        {
            bool attr_position(out icomp_Position<tpar_Event> result);
            bool attr_data(out Aha.Package.Base.Bits.opaque_BitString result);
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
            bool fattr_CreateReader(icomp_CreateReaderParam<tpar_Event> param, out Jobs.opaque_Job<tpar_Event> result);
            bool fattr_CreateWriter(icomp_CreateWriterParam<tpar_Event> param, out Jobs.opaque_Job<tpar_Event> result);
            bool fattr_ReadText(icomp_ReadTextParam<tpar_Event> param, out Jobs.opaque_Job<tpar_Event> result);
            bool fattr_WriteText(icomp_WriteTextParam<tpar_Event> param, out Jobs.opaque_Job<tpar_Event> result);
        }

        public interface icomp_CreateReaderParam<tpar_Event>
        {
            bool attr_path(out Aha.Package.API.Environment.opaque_FilePath result);
            bool attr_engine(out Aha.Package.API.Jobs.icomp_Engine<tpar_Event> result);
            bool attr_success(func_Reader<tpar_Event> reader, out tpar_Event result);
            bool attr_error(FileIOtypes.icomp_ErrorInfo error, out tpar_Event result);
        }

        public interface icomp_CreateWriterParam<tpar_Event>
        {
            bool attr_path(out Aha.Package.API.Environment.opaque_FilePath result);
            bool attr_engine(out Aha.Package.API.Jobs.icomp_Engine<tpar_Event> result);
            bool attr_success(func_Writer<tpar_Event> writer, out tpar_Event result);
            bool attr_error(FileIOtypes.icomp_ErrorInfo error, out tpar_Event result);
        }

        public interface icomp_ReadTextParam<tpar_Event>
        {
            bool attr_path(out Aha.Package.API.Environment.opaque_FilePath result);
            bool attr_engine(out Aha.Package.API.Jobs.icomp_Engine<tpar_Event> result);
            bool attr_encoding(out FileIOtypes.icomp_Encoding result);
            bool fattr_success(icomp_TextReadParams<tpar_Event> param, out tpar_Event result);
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
            bool attr_path(out Aha.Package.API.Environment.opaque_FilePath result);
            bool attr_engine(out Aha.Package.API.Jobs.icomp_Engine<tpar_Event> result);
            bool attr_content(out IahaSequence<char> result);
            bool attr_size(out long result);
            bool attr_encoding(out FileIOtypes.icomp_Encoding result);
            bool attr_success(out tpar_Event result);
            bool fattr_error(FileIOtypes.icomp_ErrorInfo error, out tpar_Event result);
        }

        public class module_FileIO<tpar_Event> : AhaModule, imod_FileIO<tpar_Event>
        {
            class ErrorInfo : Aha.Package.API.FileIOtypes.icomp_ErrorKind, Aha.Package.API.FileIOtypes.icomp_ErrorInfo
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
                                Aha.Package.Base.Bits.opaque_BitString bits = new Base.Bits.opaque_BitString { bytes = data, bits = byteCount * 8 };
                                if (p.fattr_result(bits, out evt) && engine.fattr_raise(evt, out job))
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
                public bool attr_content(out IahaSequence<char> result) { result = field_content; return true; }
                public bool attr_size(out long result) { result = field_size; return true; }
                public bool attr_encoding(out FileIOtypes.icomp_Encoding result) { result = field_encoding; return true; }
            }
            public bool fattr_CreateReader(icomp_CreateReaderParam<tpar_Event> param, out Jobs.opaque_Job<tpar_Event> result)
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
                result = new Jobs.opaque_Job<tpar_Event>
                {
                    title = "CreateReader " + path.value,
                    execute =
                        delegate()
                        {
                            job.execute();
                        }
                };
                return true;
            }
            public bool fattr_CreateWriter(icomp_CreateWriterParam<tpar_Event> param, out Jobs.opaque_Job<tpar_Event> result)
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
                result = new Jobs.opaque_Job<tpar_Event>
                {
                    title = "CreateWriter " + path.value,
                    execute =
                        delegate()
                        {
                            job.execute();
                        }
                };
                return true;
            }
            struct FileText : IahaSequence<char>
            {
                public List<string> list;
                public int index;
                public int block;
                public bool state(out char result) { if (block == list.Count) { result = default(char); return false; } result = list[block][index]; return true; }
                public IahaObject<char> copy() { FileText clone = new FileText { list = list, index = index, block = block }; return clone; }
                public bool action_skip() { if (block == list.Count) return false; if (index == list[block].Length) { index = 0; block++; } else index++; return true; }
                public bool first(Predicate<char> that, long max, out char result)
                {
                    long j = 0;
                    int i = index;
                    int b = block;
                    char item = list[block][index];
                    while (j < max)
                    {
                        item = list[b][i];
                        if (that(item)) { result = item; return true; }
                        if (i == list[b].Length) { i = 0; b++; } else i++;
                        j++;
                    }
                    result = default(char);
                    return false;
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
            public bool fattr_ReadText(icomp_ReadTextParam<tpar_Event> param, out Jobs.opaque_Job<tpar_Event> result)
            {

                Environment.opaque_FilePath path;
                param.attr_path(out path);
                FileIOtypes.icomp_Encoding enc;
                param.attr_encoding(out enc);
                Jobs.icomp_Engine<tpar_Event> engine;
                param.attr_engine(out engine);
                tpar_Event evt;
                Jobs.opaque_Job<tpar_Event> job;
                result = new Jobs.opaque_Job<tpar_Event>
                {
                    title = "ReadText " + path.value,
                    execute =
                        async delegate()
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
                return true;
            }
            public bool fattr_WriteText(icomp_WriteTextParam<tpar_Event> param, out Jobs.opaque_Job<tpar_Event> result)
            {

                Environment.opaque_FilePath path;
                param.attr_path(out path);
                FileIOtypes.icomp_Encoding enc;
                param.attr_encoding(out enc);
                Jobs.icomp_Engine<tpar_Event> engine;
                param.attr_engine(out engine);
                tpar_Event evt;
                Jobs.opaque_Job<tpar_Event> job;
                result = new Jobs.opaque_Job<tpar_Event>
                {
                    title = "WriteText " + path.value,
                    execute =
                        async delegate()
                        {
                            System.IO.FileStream stream = null;
                            const int block = 4096;
                            System.Text.Encoding encoding;
                            try
                            {
                                stream = new System.IO.FileStream(path.value, System.IO.FileMode.CreateNew, System.IO.FileAccess.Write, System.IO.FileShare.None);
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
                                            { encoding = Encoding.Default; }
                                byte[] BOM = encoding.GetPreamble();
                                if (BOM.Length > 0)
                                {
                                    await stream.WriteAsync(BOM, 0, BOM.Length);
                                }
                                char[] data = new char[block];
                                long left;
                                param.attr_size(out left);
                                int size = block;
                                IahaSequence<char> seq; param.attr_content(out seq); seq = (IahaSequence<char>)seq.copy();
                                while (left > 0)
                                {
                                    if (left < block) size = (int)left;
                                    for (int i = 0; i < size; i++)
                                    {
                                        if (seq.state(out data[i]))
                                        {
                                            if (!seq.action_skip()) { size = i + 1; left = size; }
                                        }
                                        else 
                                        { size = i; left = size; }
                                    }
                                    if (size != block) Array.Resize<char>(ref data, size);
                                    byte[] buf = encoding.GetBytes(data);
                                    await stream.WriteAsync(buf, 0, buf.Length);
                                    left -= size;
                                }
                                param.attr_success(out evt);
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
                return true;
            }
        }
    }

    namespace Application
    {
        public interface icomp_BehaviorParams<tpar_Event>
        {
            bool attr_settings(out IahaArray<char> result);
            bool attr_password(out IahaArray<char> result);
            bool fattr_output(IahaArray<char> text, out Jobs.opaque_Job<tpar_Event> result);
            bool attr_engine(out Jobs.icomp_Engine<tpar_Event> result);
        }

        public interface iobj_Behavior<tpar_Event> : IahaObject<IahaArray<Jobs.opaque_Job<tpar_Event>>>
        {
            bool action_handleEvent(tpar_Event param_event);
            bool action_handleInput(IahaArray<char> param_input);
        }

        public interface imod_Application<tpar_Event>
        {
            bool attr_Title(out IahaArray<char> result);
            bool attr_Signature(out IahaArray<char> result);
            bool fattr_Behavior(icomp_BehaviorParams<tpar_Event> param_param, out iobj_Behavior<tpar_Event> result);
        }
    }

    namespace Component
    {
        public interface icomp_ComponentParam<tpar_Settings, tpar_Output, tpar_Event>
        {
            bool attr_classname(out IahaArray<char> result);
            bool attr_password(out IahaArray<char> result);
            bool attr_engine(out Aha.Package.API.Jobs.icomp_Engine<tpar_Event> result);
            bool attr_settings(out tpar_Settings result);
            bool fattr_output(tpar_Output output, out tpar_Event result);
        }

        public interface imod_Component<tpar_Settings, tpar_Output, tpar_Event>
        {
            bool fattr_Create(icomp_ComponentParam<tpar_Settings, tpar_Output, tpar_Event> param, out Aha.Package.API.Jobs.opaque_Job<tpar_Event> result);
        }

        public class module_Component<tpar_Settings, tpar_Output, tpar_Event> : AhaModule, imod_Component<tpar_Settings, tpar_Output, tpar_Event>
        {
            delegate bool func_Output<tpar_Output, tpar_Event>(tpar_Output output, out tpar_Event result);

            class comp_BehaviorParams<tpar_Settings, tpar_Output, tpar_Event, tpar_Event2> : ProcessDef.icomp_BehaviorParams<tpar_Settings, tpar_Output, tpar_Event>
            {
                private tpar_Settings field_settings;
                private IahaArray<char> field_password;
                private func_Output<tpar_Output, tpar_Event2> field_output;
                private Aha.Package.API.Jobs.icomp_Engine<tpar_Event> field_engine; //component's engine
                private Aha.Package.API.Jobs.icomp_Engine<tpar_Event2> field_engine2; //client's engine

                public bool attr_settings(out tpar_Settings result) { result = field_settings; return true; }
                public bool attr_password(out IahaArray<char> result) { result = field_password; return true; }
                public bool fattr_output(tpar_Output output, out Jobs.opaque_Job<tpar_Event> result)
                {
                    result = new Jobs.opaque_Job<tpar_Event>
                    {
                        title = "output",
                        execute =
                            delegate()
                            {
                                tpar_Event2 evt; 
                                field_output(output, out evt);
                                Jobs.opaque_Job<tpar_Event2> job;
                                field_engine2.fattr_raise(evt, out job);
                                job.execute();
                            }
                    };
                    return true;
                }
                public bool attr_engine(out Jobs.icomp_Engine<tpar_Event> result) { result = field_engine; return true; }
                public comp_BehaviorParams
                    (
                        tpar_Settings param_settings,
                        IahaArray<char> param_password,
                        func_Output<tpar_Output, tpar_Event2> param_output,
                        Aha.Package.API.Jobs.icomp_Engine<tpar_Event> param_engine,
                        Aha.Package.API.Jobs.icomp_Engine<tpar_Event2> param_engine2
                    )
                {
                    field_settings = param_settings;
                    field_password = param_password;
                    field_output = param_output;
                    field_engine = param_engine;
                    field_engine2 = param_engine2;
                }
            }

            public bool fattr_Create(icomp_ComponentParam<tpar_Settings, tpar_Output, tpar_Event> param, out Aha.Package.API.Jobs.opaque_Job<tpar_Event> result)
            {
                IahaArray<char> cn; 
                param.attr_classname(out cn);
                string classname = new string(cn.get());
                result = new Jobs.opaque_Job<tpar_Event>
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
                                    Type eventType = assembly.GetType("opaque_Event", true, false);
                                    if (settingsType.IsAssignableFrom(typeof(tpar_Settings)) && typeof(tpar_Output).IsAssignableFrom(outputType))
                                    {
                                        Type engType = typeof(Aha.Engine.comp_Engine<>).MakeGenericType(new Type[] { eventType });
                                        object eng = Activator.CreateInstance(engType); //component's engine
                                        Type bpType = typeof(comp_BehaviorParams<,,,>).MakeGenericType(new Type[] { settingsType, outputType, eventType, typeof(tpar_Event) });
                                        func_Output<tpar_Output, tpar_Event> output = param.fattr_output;
                                        tpar_Settings settings;
                                        param.attr_settings(out settings);
                                        IahaArray<char> password;
                                        param.attr_password(out password);
                                        Jobs.icomp_Engine<tpar_Event> engine; //client's engine
                                        param.attr_engine(out engine);
                                        object bp = Activator.CreateInstance(bpType, new object[] { settings, password, output, eng, engine });
                                        foreach (Type type in assembly.ExportedTypes)
                                        {
                                            if (type.IsClass && type.Name == "export")
                                            {
                                                try
                                                {
                                                    object comp = Activator.CreateInstance(type);
                                                    if (comp != null)
                                                    {
                                                        object behavior = type.InvokeMember
                                                            (
                                                                "fattr_Behavior",
                                                                System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.InvokeMethod,
                                                                null,
                                                                comp,
                                                                new Object[] { bp }
                                                            );
                                                        engType.InvokeMember
                                                            (
                                                                "StartExternal",
                                                                System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.InvokeMethod,
                                                                null,
                                                                eng,
                                                                new Object[] { behavior }
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
                return true;
            }
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
    //the Behavior: { [ settings: Settings password: [character] output: { Output -> Jobs@Job } engine: Jobs@Engine ] -> Jobs@Behavior } "component's behavior"
    {
        public interface icomp_BehaviorParams<tpar_Settings, tpar_Output, tpar_Event>
        {
            bool attr_settings(out tpar_Settings result);
            bool attr_password(out IahaArray<char> result);
            bool fattr_output(tpar_Output text, out Jobs.opaque_Job<tpar_Event> result);
            bool attr_engine(out Jobs.icomp_Engine<tpar_Event> result);
        }

        public interface imod_ProcessDef<tpar_Settings, tpar_Output, tpar_Event>
        {
            bool attr_Title(out IahaArray<char> result);
            bool fattr_Behavior(icomp_BehaviorParams<tpar_Settings, tpar_Output, tpar_Event> bp, out Application.iobj_Behavior<tpar_Event> result);
        }
    }

} //namespace Aha.Package.API
