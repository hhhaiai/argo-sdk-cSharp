namespace Jayrock.Json.Conversion.Converters
{
    #region Imports

    using System;
    using System.Diagnostics;
    using Jayrock.Diagnostics;
    using Jayrock.Json.Conversion;

    #endregion

    public sealed class EnumImporter : ImporterBase
    {
        public EnumImporter(Type type) :
            base(type)
        {
            if (!type.IsEnum)
                throw new ArgumentException(string.Format("{0} does not inherit from System.Enum.", type));
            
            if (type.IsDefined(typeof(FlagsAttribute), true))
                throw new ArgumentException(string.Format("{0} is a bit field, which are not currently supported.", type));
        }

        protected override object ImportFromString(ImportContext context, JsonReader reader)
        {
            Debug.Assert(context != null);
            Debug.Assert(reader != null);

            string s = reader.Text.Trim();
        
            if (s.Length > 0)
            {
                char ch = s[0];
            
                if (Char.IsDigit(ch) || ch == '+' || ch == '-')
                    throw Error(s, null);
            }

            try
            {
                return ReadReturning(reader, Enum.Parse(OutputType, s, true));
            }
            catch (ArgumentException e)
            {
                //
                // Value is either an empty string ("") or only contains 
                // white space. Value is a name, but not one of the named
                // constants defined for the enumeration.
                //
            
                throw Error(s, e);
            }
        }

        private JsonException Error(string s, Exception e)
        {
            return new JsonException(string.Format("The value '{0}' cannot be imported as {1}.", DebugString.Format(s), OutputType.FullName), e);
        }
    }
}