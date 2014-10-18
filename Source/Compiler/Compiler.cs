using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aha.Core;
using Aha.Package.Base;

namespace Aha.Package.Compiler
{
    namespace Scanner
    {
        public interface icom_Location
        {
            bool attr_row(out long result);
            bool attr_column(out long result);
        }

        public interface icom_TokenInfo
        {
            bool attr_intlit(out long result);
            bool attr_charlit(out char result);
            bool attr_strlit(out IahaArray<char> result);
            bool attr_id(out IahaArray<char> result);
            bool attr_moduleexp(out IahaArray<char> result);
            bool attr_opname(out IahaArray<char> result);
            bool attr_opsymbol(out IahaArray<char> result);
            bool attr_colon;
            bool attr_dblcolon;
            bool attr_semicolon;
            bool attr_period;
            bool attr_exclaim;
            bool attr_question;
            bool attr_caret;
            bool attr_hash;
            bool attr_slash;
            bool attr_asterisk;
        }
        public interface icom_Token
        {
            bool attr_Text(out IahaArray<char> result);
            bool attr_Location(out icom_Location result);
            bool attr_Info(out icom_TokenInfo result);
        }

        public interface imod_Scanner
        {
            bool fexport(IahaSequence<char> source, long size, out IahaSequence<icom_Token> tokens);
        }

        public class module_Scanner : AhaModule, imod_Scanner
        {
            class com_Token : icom_Location, icom_TokenInfo, icom_Token
            {
                protected string text;
                protected int row;
                protected int col;
                public bool attr_row(out long result) { result = row; return true; }
                public bool attr_column(out long result) { result = col; return true; }
                public virtual bool attr_intlit(out long result) { result = 0; return false; }
                public virtual bool attr_charlit(out char result) { result = default(char); return false; }
                public virtual bool attr_strlit(out IahaArray<char> result) { result = default(IahaArray<char>); return false; }
                public virtual bool attr_id(out IahaArray<char> result) { result = default(IahaArray<char>); return false; }
                public bool attr_moduleexp(out IahaArray<char> result) { if (text.Length > 1 && text[0] == '@') { result = new AhaString(text.Substring(1, text.Length - 1)); return true; } else { result = default(IahaArray<char>); return false; } }
                public bool attr_opname(out IahaArray<char> result) { if (text.Length > 1 && text[0] == '~') { result = new AhaString(text.Substring(1, text.Length - 1)); return true; } else { result = default(IahaArray<char>); return false; } }
                public virtual bool attr_opsymbol(out IahaArray<char> result) { result = default(IahaArray<char>); return false; }
                public bool attr_colon() { return text == ":"; }
                public bool attr_dblcolon() { return text == "::"; }
                public bool attr_semicolon() { return text == ";"; }
                public bool attr_period() { return text == "."; }
                public bool attr_exclaim() { return text == "!"; }
                public bool attr_question() { return text == "?"; }
                public bool attr_caret() { return text == "^"; }
                public bool attr_hash() { return text == "#"; }
                public bool attr_slash() { return text == "/"; }
                public bool attr_asterisk() { return text == "*"; }
                public bool attr_Text(out IahaArray<char> result) { result = new AhaString(text); return true; }
                public bool attr_Location(out icom_Location result) { result = this; return true; }
                public bool attr_Info(out icom_TokenInfo result) { result = this; return true; }
                public com_Token(string t, int r, int c) { text = t; row = r; col = c; }
                public com_Token() { text = default(string); row = 0; col = 0; }
            }
            class com_IntLit : com_Token
            {
                long value;
                public override bool attr_intlit(out long result) { result = value; return true; }
                public com_IntLit(string t, int r, int c) : base(t, r, c) { value = Convert.ToInt64(t); }
            }
            class com_CharLit : com_Token
            {
                char value;
                public override bool attr_charlit(out char result) { result = value; return true; }
                public com_CharLit(string t, int r, int c) : base(t, r, c) { value = t[1]; }
            }
            class com_StrLit : com_Token
            {
                public override bool attr_strlit(out IahaArray<char> result) { result = new AhaString(text.Substring(1, text.Length - 2)); return true; }
                public com_StrLit(string t, int r, int c) : base(t, r, c) { }
            }
            class com_Id : com_Token
            {
                public override bool attr_id(out IahaArray<char> result) { result = new AhaString(text); return true; }
                public com_Id(string t, int r, int c) : base(t, r, c) { }
            }
            class com_OpSymbol : com_Token
            {
                public override bool attr_opsymbol(out IahaArray<char> result) { result = new AhaString(text); return true; }
                public com_OpSymbol(string t, int r, int c) : base(t, r, c) { }
            }
            class Tokens : IahaObjSequence<icom_Token>
            {
                char[] buffer;
                int stpos; //current token's start position in buffer 
                int row;
                int col;
                private void strip()
                {
                }
                private void extract()
                {
                }
                public bool state(out icom_Token result)
                {
                    if (stpos == buffer.Length)
                    {
                        result = default(icom_Token);
                        return false;
                    }
                    int len = 0; //current token's length
                    string letters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
                    string digits = "0123456789";
                    string text;
                    switch (buffer[stpos])
                    {
                        case '@':
                            len++;
                            while (stpos + len < buffer.Length && (letters.IndexOf(buffer[stpos + len]) >= 0 || digits.IndexOf(buffer[stpos + len]) >= 0)) len++;
                            text = new string(buffer, stpos, len);
                            result = new com_Token(text, row, col);
                            break;
                        case '#':
                            text = new string(buffer, stpos, 1);
                            result = new com_Token(text, row, col);
                            break;
                        case '^':
                            text = new string(buffer, stpos, 1);
                            result = new com_Token(text, row, col);
                            break;
                        default:
                            break;
                    }
                    return true;
                }
                public IahaObject<icom_Token> copy() { return new Tokens(buffer, stpos, row, col); }
                public bool action_skip() { strip(); return stpos < buffer.Length; }
                public Tokens(char[] b, int s, int r, int c) { buffer = b; stpos = s; row = r; col = c; strip(); }
                public Tokens() { }
            }
            public bool fexport(IahaSequence<char> source, long size, out IahaSequence<icom_Token> tokens)
            {
                char[] buffer = new char[size]; //allocate buffer for max size
                int count = 0; //actual number of chars
                IahaSequence<char> clone = (IahaSequence<char>)source.copy();
                for (int j = 0; j < size; j++) //read entire stream into buffer
                {
                    if (clone.state(out buffer[j]))
                    {
                        count = j + 1;
                        if (!clone.action_skip()) break;
                    }
                }
                if (count != size) Array.Resize<char>(ref buffer, count); //release unused memory in buffer
                tokens = new AhaObjSeq<icom_Token> { obj = new Tokens(buffer, 0, 0, 0) };
                return true;
            }
        }
    }
}
