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
        public struct com_Location
        {
            public long attr_row;
            public long attr_column;
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
            bool attr_kwimplement();
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
        public struct com_Token
        {
            public IahaArray<char> attr_text;
            public com_Location attr_location;
            public icom_TokenInfo attr_info;
        }
        //public interface icom_TokenStream
        //{
        //    bool attr_tokens(out IahaSequence<icom_Token> result);
        //    bool attr_size(out long result);
        //}

        public interface imod_Scanner
        {
            bool fexport(IahaSequence<char> source, long size, out IahaSequence<com_Token> tokens);
        }

        public class module_Scanner : AhaModule, imod_Scanner
        {
            class com_TokenInfo : icom_TokenInfo
            {
                protected string text;
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
                public bool attr_kwimplement() { return text == "implement"; }
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
                public com_TokenInfo(string t) { text = t; }
                public com_TokenInfo() { text = default(string); }
            }
            class com_IntLit : com_TokenInfo
            {
                long value;
                public override bool attr_intlit(out long result) { result = value; return true; }
                public com_IntLit(string t) : base(t) { value = Convert.ToInt64(t); }
            }
            class com_CharLit : com_TokenInfo
            {
                char value;
                public override bool attr_charlit(out char result) { result = value; return true; }
                public com_CharLit(string t) : base(t) 
                {
                    if (t == "$space")
                        value = ' ';
                    else
                        if (t == "$tab")
                            value = '\t';
                        else
                            if (t == "$CR")
                                value = '\r';
                            else
                                if (t == "$LF")
                                    value = '\n';
                                else
                                    if (t == "$backtick")
                                        value = '`';
                                    else
                                        if (t == "$quote")
                                            value = '"';
                                        else
                                            if (t.Length == 2)
                                                value = t[1];
                                            else
                                                throw Failure.One;
                }
            }
            class com_StrLit : com_TokenInfo
            {
                public override bool attr_strlit(out IahaArray<char> result) { result = new AhaString(text.Substring(1, text.Length - 2)); return true; }
                public com_StrLit(string t) : base(t) { }
            }
            class com_Id : com_TokenInfo
            {
                public override bool attr_id(out IahaArray<char> result) { result = new AhaString(text); return true; }
                public com_Id(string t) : base(t) { }
            }
            class com_OpSymbol : com_TokenInfo
            {
                public override bool attr_opsymbol(out IahaArray<char> result) { result = new AhaString(text); return true; }
                public com_OpSymbol(string t) : base(t) { }
            }
            class com_Error : com_TokenInfo
            {
                public override bool attr_error(out IahaArray<char> result) { result = new AhaString(text); return true; }
                public com_Error(string t) : base(t) { }
            }
            class Tokens : IahaObjSequence<com_Token>
            {
                char[] buffer;
                int stpos; //current token's start position in buffer 
                int row;
                int col;
                com_Token token;
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
                private com_Token Token(icom_TokenInfo info, string text, long row, long col)
                {
                    return new com_Token { attr_info = info, attr_text = new AhaString(text), attr_location = new com_Location { attr_row = row, attr_column = col } };
                }
                private void extract()
                {
                    string letters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
                    string digits = "0123456789";
                    string delim = "()[]{}.#^:;,`'\" \t\r\n";
                    string[] keywords = { "all", "alter", "any", "arbitrary", "array", "character", "count", "doc", "end", "enum", "export", "extend", "first", "foldl", "foldr", "foreach", "forsome",
                                          "implement", "in", "integer", "into", "invalid", "join", "letrec", "list", "no", "not", "obj", "opaque", "select", "seq", "sort", "such", "that", "to", "type", 
                                          "unless", "use", "void", "when", "where", "with" };
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
                                token = Token(new com_TokenInfo(text), text, row, col); //module export or named operator
                            else
                                token = Token(new com_OpSymbol(text), text, row, col); //operator @ or ~
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
                            token = Token(new com_TokenInfo(text), text, row, col);
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
                            token = Token(new com_TokenInfo(text), text, row, col);
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
                                    token = Token(new com_IntLit(text), text, row, col);
                                }
                                catch (System.Exception)
                                {
                                    token = Token(new com_Error("Number is too long"), "", row, col);
                                }
                            }
                            else
                                if (buffer[stpos + 1] == '>')
                                {
                                    len = 2;
                                }
                                else
                                {
                                    len = 1;
                                }
                                text = new string(buffer, stpos, len);
                                newcol = col + len;
                                token = Token(new com_TokenInfo(text), text, row, col);
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
                            len = 1;
                            while (stpos + len < buffer.Length && digits.IndexOf(buffer[stpos + len]) >= 0) len++;
                            text = new string(buffer, stpos, len);
                            newcol = col + len;
                            try
                            {
                                token = Token(new com_IntLit(text), text, row, col);
                            }
                            catch (System.Exception)
                            {
                                token = Token(new com_Error("Number is too long"), "", row, col);
                            }
                            break;
                        case '$': //character literal
                            if (stpos < buffer.Length - 1)
                            {
                                while (stpos + len < buffer.Length && letters.IndexOf(buffer[stpos + len]) >= 0) len++;
                                if (len == 1) len = 2;
                                text = new string(buffer, stpos, len);
                                newcol = col + len;
                                try
                                {
                                    token = Token(new com_CharLit(text), text, row, col);
                                }
                                catch (System.Exception)
                                {
                                    token = Token(new com_Error("Invalid character literal"), "", row, col);
                                }
                            }
                            else
                            {
                                len = 1;
                                newcol = col + len;
                                token = Token(new com_Error("Incomplete character literal"), "", row, col);
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
                                token = Token(new com_StrLit(text), text, row, col);
                            }
                            else
                            {
                                token = Token(new com_Error("Incomplete string literal"), "", row, col);
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
                                    token = Token(new com_TokenInfo(text), text, row, col); //keyword
                                else
                                    token = Token(new com_Id(text), text, row, col); //identifier
                            }
                            else
                            {
                                while (stpos + len < buffer.Length && delim.IndexOf(buffer[stpos + len]) == 0) len++;
                                text = new string(buffer, stpos, len);
                                newcol = col + len;
                                token = Token(new com_OpSymbol(text), text, row, col); //operator
                            }
                            break;
                    }
                    stpos += len;
                    row = newrow;
                    col = newcol;
                }
                public bool state(out com_Token result)
                {
                    if (stpos == buffer.Length)
                    {
                        result = default(com_Token);
                        return false;
                    }
                    result = token;
                    return true;
                }
                public IahaObject<com_Token> copy() { return new Tokens(buffer, stpos, row, col, token); }
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
                public Tokens(char[] b, int s, int r, int c, com_Token t) { buffer = b; stpos = s; row = r; col = c; token = t; }
                public Tokens(char[] b) { buffer = b; stpos = 0; row = 0; col = 0; strip(); if (stpos < buffer.Length) extract(); }
                public Tokens() { }
            }
            public bool fexport(IahaSequence<char> source, long size, out IahaSequence<com_Token> tokens)
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
                tokens = new AhaObjSeq<com_Token> { obj = new Tokens(buffer) };
                return true;
            }
        }
    }

    namespace Parser
    {
        public interface icom_ExpressionKind
        {
            bool attr_number(out long result);
            bool attr_char(out char result);
            bool attr_string(out IahaArray<char> result);
            bool attr_invalidexp();
            bool attr_varref(out IahaArray<char> result);
            bool attr_unaryop(out IahaArray<char> result);
            bool attr_binaryop(out IahaArray<char> result);
            bool attr_modexp(out IahaArray<char> result);
            bool attr_litarray();
        }
        public interface icom_StatementKind
        {
            bool attr_definition();
            bool attr_assertion();
            bool attr_conjunction();
            bool attr_disjunction();
            bool attr_negation();
            bool attr_alternative();
            bool attr_recursion();
        }
        public struct com_ModulePath
        {
            public IahaArray<char> attr_package;
            public IahaArray<char> attr_module;
        }
        public interface icom_NodeInfo
        {
            bool attr_specification();
            bool attr_implementation();
            bool attr_implementDcl(out com_ModulePath result);
            bool attr_useDcl();
            bool attr_typeDcl();
            bool attr_expression(out icom_ExpressionKind result);
            bool attr_statement(out icom_StatementKind result);
            bool attr_variable(out IahaArray<char> result);
            bool attr_pattern();
            bool attr_typeRef();
            bool attr_typeRefExt();
            bool attr_whenClause();
            bool attr_whereClause();
            bool attr_error(out IahaArray<char> result);
        }
        public struct com_Node
        {
            public Scanner.com_Location attr_location;
            public icom_NodeInfo attr_info;
        }
        public interface imod_Parser
        {
            bool fattr_ParseSpecification(IahaSequence<Scanner.com_Token> tokens, long size, out Base.Trees.opaque_Tree<com_Node> tree);
            bool fattr_ParseImplementation(IahaSequence<Scanner.com_Token> tokens, long size, out Base.Trees.opaque_Tree<com_Node> tree);
        }

        public class module_Parser : AhaModule, imod_Parser
        {
            class ExpressionKind : icom_ExpressionKind
            {
                public virtual bool attr_number(out long result) { result = 0; return false; }
                public virtual bool attr_char(out char result) { result = default(char); return false; }
                public virtual bool attr_string(out IahaArray<char> result) { result = default(IahaArray<char>); return false; }
                public virtual bool attr_invalidexp() { return false; }
                public virtual bool attr_varref(out IahaArray<char> result) { result = default(IahaArray<char>); return false; }
                public virtual bool attr_unaryop(out IahaArray<char> result) { result = default(IahaArray<char>); return false; }
                public virtual bool attr_binaryop(out IahaArray<char> result) { result = default(IahaArray<char>); return false; }
                public virtual bool attr_modexp(out IahaArray<char> result) { result = default(IahaArray<char>); return false; }
                public virtual bool attr_litarray() { return false; }
            }
            class Number : ExpressionKind
            {
                private long value;
                public override bool attr_number(out long result) { result = value; return true; }
                public Number(long v) { value = v; }
            }
            class NodeInfo : icom_NodeInfo
            {
                public virtual bool attr_specification() { return false; }
                public virtual bool attr_implementation() { return false; }
                public virtual bool attr_implementDcl(out com_ModulePath result) { result = default(com_ModulePath); return false; }
                public virtual bool attr_useDcl() { return false; }
                public virtual bool attr_typeDcl() { return false; }
                public virtual bool attr_expression(out icom_ExpressionKind result) { result = default(icom_ExpressionKind); return false; }
                public virtual bool attr_statement(out icom_StatementKind result) { result = default(icom_StatementKind); return false; }
                public virtual bool attr_variable(out IahaArray<char> result) { result = default(IahaArray<char>); return false; }
                public virtual bool attr_pattern() { return false; }
                public virtual bool attr_typeRef() { return false; }
                public virtual bool attr_typeRefExt() { return false; }
                public virtual bool attr_whenClause() { return false; }
                public virtual bool attr_whereClause() { return false; }
                public virtual bool attr_export() { return false; }
                public virtual bool attr_error(out IahaArray<char> result) { result = default(IahaArray<char>); return false; }
            }
            class Specification : NodeInfo
            {
                public override bool attr_specification() { return true; }
            }
            class Implementation : NodeInfo
            {
                public override bool attr_implementation() { return true; }
            }
            class ImplementDcl : NodeInfo
            {
                private com_ModulePath path;
                public override bool attr_implementDcl(out com_ModulePath result) { result = path; return true; }
                public ImplementDcl(com_ModulePath p) { path = p; }
            }
            class UseDcl : NodeInfo
            {
                public override bool attr_useDcl() { return true; }
            }
            class TypeDcl : NodeInfo
            {
                public override bool attr_typeDcl() { return true; }
            }
            class Expression : NodeInfo
            {
                private icom_ExpressionKind kind;
                public override bool attr_expression(out icom_ExpressionKind result) { result = kind; return true; }
                public Expression(icom_ExpressionKind k) { kind = k; }
            }
            class Pattern : NodeInfo
            {
                public override bool attr_pattern() { return true; }
            }
            class TypeRef : NodeInfo
            {
                public override bool attr_typeRef() { return true; }
            }
            class TypeRefExt : NodeInfo
            {
                public override bool attr_typeRefExt() { return true; }
            }
            class WhenClause : NodeInfo
            {
                public override bool attr_whenClause() { return true; }
            }
            class WhereClause : NodeInfo
            {
                public override bool attr_whereClause() { return true; }
            }
            class Export : NodeInfo
            {
                public override bool attr_export() { return true; }
            }
            struct com_ParseResult
            {
                public Base.Trees.opaque_Tree<com_Node> attr_tree;
                public IahaSequence<Scanner.com_Token> attr_rest;
            }

            private Base.Trees.imod_Trees<com_Node> nick_Trees = new Base.Trees.module_Trees<com_Node>();

            private bool parseTypeRef(IahaSequence<Scanner.com_Token> tokens, long size, out com_ParseResult result)
            {
                result = default(com_ParseResult);
                return false;
            }
            private bool parseTypeRefExt(IahaSequence<Scanner.com_Token> tokens, long size, out com_ParseResult result)
            {
                result = default(com_ParseResult);
                return false;
            }
            private bool parseExpression(IahaSequence<Scanner.com_Token> tokens, long size, out com_ParseResult result)
            {
                result = default(com_ParseResult);
                return false;
            }
            private bool parseFullExpression(IahaSequence<Scanner.com_Token> tokens, long size, out com_ParseResult result)
            {
                Base.Trees.opaque_Tree<com_Node> tree, tree2;
                Scanner.com_Token token;
                IahaArray<char> id1, id2;
                IahaSequence<Scanner.com_Token> tokens2 = (IahaSequence<Scanner.com_Token>)tokens.copy();
                com_Node node;
                com_ParseResult pres;
                List<Base.Trees.opaque_Tree<com_Node>> trees = new List<Base.Trees.opaque_Tree<com_Node>>();
                Scanner.com_Location loc;
                long intlit;
                result = default(com_ParseResult);
                if (!tokens2.state(out token))
                    return false;
                if (token.attr_info.attr_lrb()) //(
                {
                    if (!(tokens2.action_skip() && parseExpression(tokens2, size, out pres) && pres.attr_rest.state(out token) && token.attr_info.attr_rrb())) //)
                        return false;
                    result = pres;
                    result.attr_rest.action_skip();
                }
                else
                    if (token.attr_info.attr_intlit(out intlit))
                    {
                        tokens2.action_skip();
                        node = new com_Node { attr_info = new ImplementDcl(path), attr_location = loc };
                        result = new com_ParseResult { attr_tree = makeLeaf(node), attr_rest = tokens2 };
                    }
                return true;
            }
            private bool parseStatement(IahaSequence<Scanner.com_Token> tokens, long size, out com_ParseResult result)
            {
                result = default(com_ParseResult);
                return false;
            }
            private bool parseUseDcl(IahaSequence<Scanner.com_Token> tokens, long size, out com_ParseResult result)
            {
                result = default(com_ParseResult);
                return false;
            }
            private bool parseTypeDcl(IahaSequence<Scanner.com_Token> tokens, long size, out com_ParseResult result)
            {
                result = default(com_ParseResult);
                return false;
            }
            private Base.Trees.opaque_Tree<com_Node> makeTree(com_Node root, Base.Trees.opaque_Tree<com_Node>[] children)
            {
                Base.Trees.opaque_Tree<com_Node> result;
                Base.Trees.com_TreeParam<com_Node> param = new Base.Trees.com_TreeParam<com_Node> { attr_root = root, attr_children = new AhaArray<Base.Trees.opaque_Tree<com_Node>>(children) };
                nick_Trees.fattr_Tree(param, out result);
                return result;
            }
            private Base.Trees.opaque_Tree<com_Node> makeLeaf(com_Node root)
            {
                Base.Trees.opaque_Tree<com_Node> result;
                nick_Trees.fattr_Leaf(root, out result);
                return result;
            }
            
            public bool fattr_ParseSpecification(IahaSequence<Scanner.com_Token> tokens, long size, out Base.Trees.opaque_Tree<com_Node> tree)
            {
                tree = default(Base.Trees.opaque_Tree<com_Node>);
                return false;
            }
            public bool fattr_ParseImplementation(IahaSequence<Scanner.com_Token> tokens, long size, out Base.Trees.opaque_Tree<com_Node> result)
            {
                Base.Trees.opaque_Tree<com_Node> tree;
                Scanner.com_Token token;
                IahaArray<char> id1, id2;
                IahaSequence<Scanner.com_Token> tokens2 = (IahaSequence<Scanner.com_Token>)tokens.copy();
                com_Node node;
                com_ParseResult pres;
                List<Base.Trees.opaque_Tree<com_Node>> trees = new List<Base.Trees.opaque_Tree<com_Node>>();
                Scanner.com_Location loc;
                result = default(Base.Trees.opaque_Tree<com_Node>);
                if (!(tokens2.state(out token) && token.attr_info.attr_kwimplement())) //implement
                    return false;
                loc = token.attr_location;
                if (!(tokens2.action_skip() && tokens2.state(out token) && token.attr_info.attr_id(out id1))) //package name
                    return false;
                if (!(tokens2.action_skip() && tokens2.state(out token) && token.attr_info.attr_slash())) //slash
                    return false;
                if (!(tokens2.action_skip() && tokens2.state(out token) && token.attr_info.attr_id(out id2))) //module name
                    return false;
                com_ModulePath path = new com_ModulePath { attr_package = id1, attr_module = id2 };
                node = new com_Node { attr_info = new ImplementDcl(path), attr_location = loc };
                tree = makeLeaf(node);
                trees.Add(tree);
                if (!(tokens2.action_skip()))
                    return false;
                while (tokens2.state(out token) && !token.attr_info.attr_kwexport())
                {
                    if (token.attr_info.attr_kwtype())
                    {
                        if (!parseTypeDcl(tokens2, size, out pres))
                            return false;
                        trees.Add(pres.attr_tree);
                        tokens2 = pres.attr_rest;
                    }
                    else
                        if (token.attr_info.attr_kwuse())
                        {
                            if (!parseUseDcl(tokens2, size, out pres))
                                return false;
                            trees.Add(pres.attr_tree);
                            tokens2 = pres.attr_rest;
                        }
                        else
                            return false;
                }
                tokens2.state(out token);
                if (!tokens2.action_skip()) //export
                    return false;
                if (!parseExpression(tokens2, size, out pres))
                    return false;
                node = new com_Node { attr_info = new Export(), attr_location = token.attr_location };
                tree = makeTree(node, new Base.Trees.opaque_Tree<com_Node>[] { pres.attr_tree });
                trees.Add(tree);
                if (pres.attr_rest.state(out token))
                    return false;
                node = new com_Node { attr_info = new Implementation(), attr_location = loc };
                result = makeTree(node, trees.ToArray());
                return true;
            }
        }
    }
}
