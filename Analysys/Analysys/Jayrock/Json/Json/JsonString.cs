namespace Jayrock.Json
{
    #region Imports

    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Text;

    #endregion

    public sealed class JsonString
    {
    
        public static string Enquote(string s)
        {
            if (s == null || s.Length == 0)
                return "\"\"";

            return Enquote(s, null).ToString();
        }
        
        public static StringBuilder Enquote(string s, StringBuilder sb)
        {
            int length = Mask.NullString(s).Length;
            
            if (sb == null)
                sb = new StringBuilder(length + 4);
            
            sb.Append('"');
            
            char last;
            char ch = '\0';
            
            for (int index = 0; index < length; index++)
            {
                last = ch;
                ch = s[index];

                switch (ch)
                {
                    case '\\':
                    case '"':
                    {
                        sb.Append('\\');
                        sb.Append(ch);
                        break;
                    }
                        
                    case '/':
                    {
                        if (last == '<')
                            sb.Append('\\');
                        sb.Append(ch);
                        break;
                    }
                    
                    case '\b': sb.Append("\\b"); break;
                    case '\t': sb.Append("\\t"); break;
                    case '\n': sb.Append("\\n"); break;
                    case '\f': sb.Append("\\f"); break;
                    case '\r': sb.Append("\\r"); break;
                    
                    default:
                    {
                        if (ch < ' ')
                        {
                            sb.Append("\\u");
                            sb.Append(((int) ch).ToString("x4", CultureInfo.InvariantCulture));
                        }
                        else
                        {
                            sb.Append(ch);
                        }
                    
                        break;
                    }
                }
            }

            return sb.Append('"');
        }

        /// <summary>
        /// Return the characters up to the next close quote character.
        /// Backslash processing is done. The formal JSON format does not
        /// allow strings in single quotes, but an implementation is allowed to
        /// accept them.
        /// </summary>
        /// <param name="quote">The quoting character, either " or '</param>
        /// <returns>A String.</returns>
        
        // TODO: Consider rendering Dequote public
        
        internal static string Dequote(BufferedCharReader input, char quote)
        {
            return Dequote(input, quote, null).ToString();
        }

        internal static StringBuilder Dequote(BufferedCharReader input, char quote, StringBuilder output)
        {
            Debug.Assert(input != null);

            if (output == null)
                output = new StringBuilder();
            
            char[] hexDigits = null;
            
            while (true)
            {
                char ch = input.Next();

                if (ch == BufferedCharReader.EOF) 
                    throw new FormatException("Unterminated string.");

                if (ch == '\\')
                {
                    ch = input.Next();

                    switch (ch)
                    {
                        case 'b': output.Append('\b'); break; // Backspace
                        case 't': output.Append('\t'); break; // Horizontal tab
                        case 'n': output.Append('\n'); break; // Newline
                        case 'f': output.Append('\f'); break; // Form feed
                        case 'r': output.Append('\r'); break; // Carriage return 
                            
                        case 'u':
                        {
                            if (hexDigits == null)
                                hexDigits = new char[4];
                            
                            output.Append(ParseHex(input, hexDigits)); 
                            break;
                        }
                            
                        default:
                            output.Append(ch);
                            break;
                    }
                }
                else
                {
                    if (ch == quote)
                        return output;

                    output.Append(ch);
                }
            }
        }
        
        /// <summary>
        /// Eats the next four characters, assuming hex digits, and converts
        /// into the represented character value.
        /// </summary>
        /// <returns>The parsed character.</returns>

        private static char ParseHex(BufferedCharReader input, char[] hexDigits) 
        {
            Debug.Assert(input != null);
            Debug.Assert(hexDigits != null);
            Debug.Assert(hexDigits.Length == 4);
            
            hexDigits[0] = input.Next();
            hexDigits[1] = input.Next();
            hexDigits[2] = input.Next();
            hexDigits[3] = input.Next();
            
            return (char) ushort.Parse(new string(hexDigits), NumberStyles.HexNumber);
        }

        private JsonString()
        {
            throw new NotSupportedException();
        }
    }
}
