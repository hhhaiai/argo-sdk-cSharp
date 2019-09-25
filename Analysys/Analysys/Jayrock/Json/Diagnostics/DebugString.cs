namespace Jayrock.Diagnostics
{
    #region Imports

    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.Text;

    #endregion


    public sealed class DebugString
    {
        public static readonly string Ellipsis = "\x2026";
        public static readonly char ControlReplacement = '?';
        
        public static string Format(string s)
        {
            return Format(s, 50);
        }

        public static string Format(string s, int width)
        {
            Debug.Assert(width > Ellipsis.Length);
            
            if (s == null)
                return string.Empty;
            
            StringBuilder sb = new StringBuilder(width);

            for (int i = 0; i < Math.Min(width, s.Length); i++)
            {
                sb.Append(!Char.IsControl(s, i) ? s[i] : ControlReplacement);
            }
            
            if (s.Length > width)
            {
                sb.Remove(width - Ellipsis.Length, Ellipsis.Length);
                sb.Append(Ellipsis);
            }
            
            return sb.ToString();
        }

        private DebugString()
        {
            throw new NotSupportedException();
        }
    }
}
