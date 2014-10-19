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
            bool attr_arrow();
            bool attr_colon();
            bool attr_dblcolon();
            bool attr_semicolon();
            bool attr_period();
            bool attr_dblperiod();
            bool attr_exclaim();
            bool attr_question();
            bool attr_caret();
            bool attr_hash();
            bool attr_slash();
            bool attr_asterisk();
            bool attr_lrb();
            bool attr_rrb();
            bool attr_lsb();
            bool attr_rsb();
            bool attr_lcb();
            bool attr_rcb();
            bool attr_kwall();
            bool attr_kwalter();
            bool attr_kwany();
            bool attr_kwarbitrary();
            bool attr_kwarray();
            bool attr_kwacharacter();
            bool attr_kwcount();
            bool attr_kwdoc();
            bool attr_kwend();
            bool attr_kwenum();
            bool attr_kwexport();
            bool attr_kwextend();
            bool attr_kwfirst();
            bool attr_kwfoldl();
            bool attr_kwfoldr();
            bool attr_kwforeach();
            bool attr_kwforsome();
            bool attr_kwin();
            bool attr_kwinteger();
            bool attr_kwinto();
            bool attr_kwinvalid();
            bool attr_kwjoin();
            bool attr_kwletrec();
            bool attr_kwlist();
            bool attr_kwno();
            bool attr_kwnot();
            bool attr_kwobj();
            bool attr_kwopaque();
            bool attr_kwselect();
            bool attr_kwseq();
            bool attr_kwsort();
            bool attr_kwsuch();
            bool attr_kwthat();
            bool attr_kwto();
            bool attr_kwtype();
            bool attr_kwunless();
            bool attr_kwuse();
            bool attr_kwvoid();
            bool attr_kwwhen();
            bool attr_kwwhere();
            bool attr_kwwith();
        }
        public interface icom_Token
        {
            bool attr_Text(out IahaArray<char> result);
            bool attr_Location(out icom_Location result);
            bool attr_Info(out icom_TokenInfo result);
        }
        //public interface icom_TokenStream
        //{
        //    bool attr_tokens(out IahaSequence<icom_Token> result);
        //    bool attr_size(out long result);
        //}

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
                public bool attr_arrow() { return text == "->"; }
                public bool attr_colon() { return text == ":"; }
                public bool attr_dblcolon() { return text == "::"; }
                public bool attr_semicolon() { return text == ";"; }
                public bool attr_period() { return text == "."; }
                public bool attr_dblperiod() { return text == ".."; }
                public bool attr_exclaim() { return text == "!"; }
                public bool attr_question() { return text == "?"; }
                public bool attr_caret() { return text == "^"; }
                public bool attr_hash() { return text == "#"; }
                public bool attr_slash() { return text == "/"; }
                public bool attr_asterisk() { return text == "*"; }
                public bool attr_lrb() { return text == "("; }
                public bool attr_rrb() { return text == ")"; }
                public bool attr_lsb() { return text == "["; }
                public bool attr_rsb() { return text == "]"; }
                public bool attr_lcb() { return text == "{"; }
                public bool attr_rcb() { return text == "}"; }
                public bool attr_kwall() { return text == "all"; }
                public bool attr_kwalter() { return text == "alter"; }
                public bool attr_kwany() { return text == "any"; }
                public bool attr_kwarbitrary() { return text == "arbitrary"; }
                public bool attr_kwarray() { return text == "array"; }
                public bool attr_kwacharacter() { return text == "character"; }
                public bool attr_kwcount() { return text == "count"; }
                public bool attr_kwdoc() { return text == "doc"; }
                public bool attr_kwend() { return text == "end"; }
                public bool attr_kwenum() { return text == "enum"; }
                public bool attr_kwexport() { return text == "export"; }
                public bool attr_kwextend() { return text == "extend"; }
                public bool attr_kwfirst() { return text == "first"; }
                public bool attr_kwfoldl() { return text == "foldl"; }
                public bool attr_kwfoldr() { return text == "foldr"; }
                public bool attr_kwforeach() { return text == "foreach"; }
                public bool attr_kwforsome() { return text == "forsome"; }
                public bool attr_kwin() { return text == "in"; }
                public bool attr_kwinteger() { return text == "integer"; }
                public bool attr_kwinto() { return text == "into"; }
                public bool attr_kwinvalid() { return text == "invalid"; }
                public bool attr_kwjoin() { return text == "join"; }
                public bool attr_kwletrec() { return text == "letrec"; }
                public bool attr_kwlist() { return text == "list"; }
                public bool attr_kwno() { return text == "no"; }
                public bool attr_kwnot() { return text == "not"; }
                public bool attr_kwobj() { return text == "obj"; }
                public bool attr_kwopaque() { return text == "opaque"; }
                public bool attr_kwselect() { return text == "select"; }
                public bool attr_kwseq() { return text == "seq"; }
                public bool attr_kwsort() { return text == "sort"; }
                public bool attr_kwsuch() { return text == "such"; }
                public bool attr_kwthat() { return text == "that"; }
                public bool attr_kwto() { return text == "to"; }
                public bool attr_kwtype() { return text == "type"; }
                public bool attr_kwunless() { return text == "unless"; }
                public bool attr_kwuse() { return text == "use"; }
                public bool attr_kwvoid() { return text == "void"; }
                public bool attr_kwwhen() { return text == "when"; }
                public bool attr_kwwhere() { return text == "where"; }
                public bool attr_kwwith() { return text == "with"; }
                public virtual bool attr_error(out IahaArray<char> result) { result = default(IahaArray<char>); return false; }
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
            class com_Error : com_Token
            {
                public override bool attr_error(out IahaArray<char> result) { result = new AhaString(text); return true; }
                public com_Error(string t, int r, int c) : base(t, r, c) { }
            }
            class Tokens : IahaObjSequence<icom_Token>
            {
                char[] buffer;
                int stpos; //current token's start position in buffer 
                int row;
                int col;
                icom_Token token;
                private void strip()
                {
                    while (stpos < buffer.Length)
                    {
                        switch (buffer[stpos])
                        {
                            case '\t':
                            case '\r':
                            case ' ':
                                stpos++;
                                col++;
                                break;
                            case '\n':
                                stpos++;
                                row++;
                                col = 0;
                                break;
                            case '`':
                                while (stpos < buffer.Length && buffer[stpos] != '\n')
                                {
                                    stpos++;
                                    col++;
                                }
                                break;
                            default:
                                return;
                        }
                    }
                }
                private void extract()
                {
                    string letters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
                    string digits = "0123456789";
                    string delim = "()[]{}.#^:;,`'\" \t\r\n";
                    string[] keywords = { "all", "alter", "any", "arbitrary", "array", "character", "count", "doc", "end", "enum", "export", "extend", "first", "foldl", "foldr", "foreach", "forsome",
                                          "in", "integer", "into", "invalid", "join", "letrec", "list", "no", "not", "obj", "opaque", "select", "seq", "sort", "such", "that", "to", "type", "unless",
                                          "use", "void", "when", "where", "with" };
                    string text;
                    int len;
                    int newrow;
                    int newcol;
                    len = 0; //current token's length
                    newrow = row;
                    switch (buffer[stpos])
                    {
                        case '@':
                        case '~':
                            len++;
                            while (stpos + len < buffer.Length && (letters.IndexOf(buffer[stpos + len]) >= 0 || digits.IndexOf(buffer[stpos + len]) >= 0)) len++;
                            text = new string(buffer, stpos, len);
                            newcol = col + len;
                            if (len > 1)
                                token = new com_Token(text, row, col); //module export or named operator
                            else
                                token = new com_OpSymbol(text, row, col); //operator @ or ~
                            break;
                        case '#':
                        case '^':
                        case '/':
                        case '*':
                        case '!':
                        case '?':
                        case ';':
                            len = 1;
                            text = new string(buffer, stpos, len);
                            newcol = col + len;
                            token = new com_Token(text, row, col);
                            break;
                        case '.':
                        case ':':
                            if (stpos < buffer.Length - 1 && buffer[stpos + 1] == buffer[stpos])
                            {
                                len = 2; //double colon or period
                            }
                            else
                            {
                                len = 1; //single
                            }
                            text = new string(buffer, stpos, len);
                            newcol = col + len;
                            token = new com_Token(text, row, col);
                            break;
                        case '-': //number, arrow or dash
                            if (digits.IndexOf(buffer[stpos + 1]) >= 0)
                            {
                                len = 1;
                                while (stpos + len < buffer.Length && digits.IndexOf(buffer[stpos + len]) >= 0) len++;
                                text = new string(buffer, stpos, len);
                                newcol = col + len;
                                try
                                {
                                    token = new com_IntLit(text, row, col);
                                }
                                catch (System.Exception)
                                {
                                    token = new com_Error("Number is too long", row, col);
                                }
                            }
                            else
                                if (buffer[stpos + 1] == '>')
                                {
                                    len = 2;
                                    text = new string(buffer, stpos, len);
                                    newcol = col + len;
                                    token = new com_Token(text, row, col);
                                }
                                else
                                {
                                    len = 1;
                                    newcol = col + len;
                                }
                            break;
                        case '0':
                        case '1':
                        case '2':
                        case '3':
                        case '4':
                        case '5':
                        case '6':
                        case '7':
                        case '8':
                        case '9': //number
                            len++;
                            while (stpos + len < buffer.Length && digits.IndexOf(buffer[stpos + len]) >= 0) len++;
                            text = new string(buffer, stpos, len);
                            newcol = col + len;
                            try
                            {
                                token = new com_IntLit(text, row, col);
                            }
                            catch (System.Exception)
                            {
                                token = new com_Error("Number is too long", row, col);
                            }
                            break;
                        case '$': //character literal
                            if (stpos < buffer.Length - 1)
                            {
                                len = 2;
                                text = new string(buffer, stpos, len);
                                newcol = col + len;
                                token = new com_CharLit(text, row, col);
                            }
                            else
                            {
                                len = 1;
                                newcol = col + len;
                                token = new com_Error("Incomplete character literal", row, col);
                            }
                            break;
                        case '"': //string literal
                            len++;
                            newcol = col + 1;
                            while (stpos + len < buffer.Length && buffer[stpos + len] != '"')
                            {
                                if (buffer[stpos + len] == '\n')
                                {
                                    newcol = 0;
                                    newrow++;
                                }
                                else newcol++;
                                len++;
                            }
                            if (stpos + len < buffer.Length)
                            {
                                len++;
                                text = new string(buffer, stpos, len);
                                token = new com_StrLit(text, row, col);
                            }
                            else
                            {
                                token = new com_Error("Incomplete string literal", row, col);
                            }
                            break;
                        default:
                            len++;
                            if (letters.IndexOf(buffer[stpos]) >= 0) //begins with letter?
                            {
                                while (stpos + len < buffer.Length && (letters.IndexOf(buffer[stpos + len]) >= 0 || digits.IndexOf(buffer[stpos + len]) >= 0)) len++;
                                text = new string(buffer, stpos, len);
                                newcol = col + len;
                                if (keywords.Contains(text))
                                    token = new com_Token(text, row, col); //keyword
                                else
                                    token = new com_Id(text, row, col); //identifier
                            }
                            else
                            {
                                while (stpos + len < buffer.Length && delim.IndexOf(buffer[stpos + len]) == 0) len++;
                                text = new string(buffer, stpos, len);
                                newcol = col + len;
                                token = new com_OpSymbol(text, row, col); //operator
                            }
                            break;
                    }
                    stpos += len;
                    row = newrow;
                    col = newcol;
                }
                public bool state(out icom_Token result)
                {
                    if (stpos == buffer.Length)
                    {
                        result = default(icom_Token);
                        return false;
                    }
                    result = token;
                    return true;
                }
                public IahaObject<icom_Token> copy() { return new Tokens(buffer, stpos, row, col, token); }
                public bool action_skip() 
                { 
                    strip(); 
                    if (stpos < buffer.Length) 
                    { 
                        extract();
                        return true; 
                    } 
                    else 
                        return false; 
                }
                public Tokens(char[] b, int s, int r, int c, icom_Token t) { buffer = b; stpos = s; row = r; col = c; token = t; }
                public Tokens(char[] b) { buffer = b; stpos = 0; row = 0; col = 0; strip(); if (stpos < buffer.Length) extract(); }
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
                tokens = new AhaObjSeq<icom_Token> { obj = new Tokens(buffer) };
                return true;
            }
        }
    }

    namespace Parser
    {
        public interface icom_ExpressionKind
        {
            bool attr_number();
            bool attr_char();
            bool attr_string();
            bool attr_invalidexp();
            bool attr_var();
            bool attr_pattern();
            bool attr_modexp();
            bool attr_litarray();
        }
        public interface icom_Node
        {
            bool attr_expression(out icom_ExpressionKind result);
            bool attr_statement(out icom_ExpressionKind result);
            bool attr_variable(out icom_ExpressionKind result);
            bool attr_inClause();
        }

        public class module_Parser
        {
            public bool fexport(IahaSequence<Scanner.icom_Token> tokens, long size, out Aha.Package.Base.Trees.opaque_Tree<icom_Node> tree)
            {
                tree = default(Aha.Package.Base.Trees.opaque_Tree<icom_Node>);
                return false;
            }
        }
    }
}
