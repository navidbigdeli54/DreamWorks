/**Copyright 2016 - 2020, Dream Machine Game Studio. All Right Reserved.*/

using System;
using System.Text;
using System.Globalization;
using System.Collections.Generic;

namespace DreamMachineGameStudio.Dreamworks.Serialization.Json
{
    public abstract class FJsonNode
    {
        #region Fields
        protected const int INTEND_SPACE_COUNT = 4;
        #endregion

        #region Public Methods
        public override string ToString() => ToString();

        public abstract string ToString(int indentLevel = 0);

        public static FJsonNode Parse(string json)
        {
            FJsonNode context = null;
            StringBuilder token = new StringBuilder();
            Stack<FJsonNode> stack = new Stack<FJsonNode>();

            string tokenName = "";
            bool quoteMode = false;
            bool tokenIsQuoted = false;

            for (int i = 0; i < json.Length; ++i)
            {
                switch (json[i])
                {
                    case '{':
                        if (quoteMode)
                        {
                            token.Append(json[i]);
                            break;
                        }
                        stack.Push(new FJsonObject());
                        if (context != null)
                        {
                            if (context is FJsonArray jsonArray)
                            {
                                jsonArray.Add(stack.Peek());
                            }
                            else if (context is FJsonObject jsonObject)
                            {
                                jsonObject.Add(tokenName, stack.Peek());
                            }
                        }
                        tokenName = "";
                        token.Length = 0;
                        context = stack.Peek();
                        break;

                    case '[':
                        if (quoteMode)
                        {
                            token.Append(json[i]);
                            break;
                        }

                        stack.Push(new FJsonArray());
                        if (context != null)
                        {
                            if (context is FJsonArray jsonArray)
                            {
                                jsonArray.Add(stack.Peek());
                            }
                            else if (context is FJsonObject jsonObject)
                            {
                                jsonObject.Add(tokenName, stack.Peek());
                            }
                        }
                        tokenName = "";
                        token.Length = 0;
                        context = stack.Peek();
                        break;

                    case '}':
                    case ']':
                        if (quoteMode)
                        {

                            token.Append(json[i]);
                            break;
                        }
                        if (stack.Count == 0)
                            throw new Exception("JSON Parse: Too many closing brackets");

                        stack.Pop();
                        if (token.Length > 0 || tokenIsQuoted)
                        {
                            if (context is FJsonArray jsonArray)
                            {
                                jsonArray.Add(ParseElement(token.ToString(), tokenIsQuoted));
                            }
                            else if (context is FJsonObject jsonObject)
                            {
                                jsonObject.Add(tokenName, ParseElement(token.ToString(), tokenIsQuoted));
                            }
                        }
                        tokenIsQuoted = false;
                        tokenName = "";
                        token.Length = 0;
                        if (stack.Count > 0)
                            context = stack.Peek();
                        break;

                    case ':':
                        if (quoteMode)
                        {
                            token.Append(json[i]);
                            break;
                        }
                        tokenName = token.ToString();
                        token.Length = 0;
                        tokenIsQuoted = false;
                        break;

                    case '"':
                        quoteMode ^= true;
                        tokenIsQuoted |= quoteMode;
                        break;

                    case ',':
                        if (quoteMode)
                        {
                            token.Append(json[i]);
                            break;
                        }
                        if (token.Length > 0 || tokenIsQuoted)
                        {
                            if (context is FJsonArray jsonArray)
                            {
                                jsonArray.Add(ParseElement(token.ToString(), tokenIsQuoted));
                            }
                            else if (context is FJsonObject jsonObject)
                            {
                                jsonObject.Add(tokenName, ParseElement(token.ToString(), tokenIsQuoted));
                            }
                        }
                        tokenName = "";
                        token.Length = 0;
                        tokenIsQuoted = false;
                        break;

                    case '\r':
                    case '\n':
                        break;

                    case ' ':
                    case '\t':
                        if (quoteMode)
                            token.Append(json[i]);
                        break;

                    case '\\':
                        ++i;
                        if (quoteMode)
                        {
                            char C = json[i];
                            switch (C)
                            {
                                case 't':
                                    token.Append('\t');
                                    break;
                                case 'r':
                                    token.Append('\r');
                                    break;
                                case 'n':
                                    token.Append('\n');
                                    break;
                                case 'b':
                                    token.Append('\b');
                                    break;
                                case 'f':
                                    token.Append('\f');
                                    break;
                                case 'u':
                                    {
                                        string s = json.Substring(i + 1, 4);
                                        token.Append((char)int.Parse(
                                            s,
                                            System.Globalization.NumberStyles.AllowHexSpecifier));
                                        i += 4;
                                        break;
                                    }
                                default:
                                    token.Append(C);
                                    break;
                            }
                        }
                        break;
                    case '/':
                        if (!quoteMode && i + 1 < json.Length && json[i + 1] == '/')
                        {
                            while (++i < json.Length && json[i] != '\n' && json[i] != '\r') ;
                            break;
                        }
                        token.Append(json[i]);
                        break;
                    case '\uFEFF': // remove / ignore BOM (Byte Order Mark)
                        break;

                    default:
                        token.Append(json[i]);
                        break;
                }
            }

            if (quoteMode)
            {
                throw new Exception("JSON Parse: Quotation marks seems to be messed up.");
            }

            if (context == null)
                return ParseElement(token.ToString(), tokenIsQuoted);

            return context;
        }

        private static FJsonNode ParseElement(string token, bool quoted)
        {
            if (quoted)
                return token;

            string tmp = token.ToLower();

            if (tmp == "false" || tmp == "true")
                return tmp == "true";

            if (tmp == "null")
                return FJsonNull.NULL;

            if (double.TryParse(token, NumberStyles.Float, CultureInfo.InvariantCulture, out double value))
                return value;

            return token;
        }
        #endregion

        #region Operator Overloads
        public static implicit operator FJsonNode(string s) => new FJsonString(s);

        public static implicit operator FJsonNode(double d) => new FJsonNumber(d);

        public static implicit operator FJsonNode(bool b) => new FJsonBool(b);
        #endregion

        #region Protected Methods
        protected StringBuilder Escape(string text)
        {
            StringBuilder stringBuilder = new StringBuilder();

            foreach (char c in text)
            {
                switch (c)
                {
                    case '\\':
                        stringBuilder.Append("\\\\");
                        break;
                    case '\"':
                        stringBuilder.Append("\\\"");
                        break;
                    case '\n':
                        stringBuilder.Append("\\n");
                        break;
                    case '\r':
                        stringBuilder.Append("\\r");
                        break;
                    case '\t':
                        stringBuilder.Append("\\t");
                        break;
                    case '\b':
                        stringBuilder.Append("\\b");
                        break;
                    case '\f':
                        stringBuilder.Append("\\f");
                        break;
                    default:
                        if (c < ' ')
                        {
                            ushort val = c;
                            stringBuilder.Append("\\u").Append(val.ToString("X4"));
                        }
                        else
                            stringBuilder.Append(c);
                        break;
                }
            }
            return stringBuilder;
        }
        #endregion
    }
}