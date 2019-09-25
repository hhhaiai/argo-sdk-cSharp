namespace Jayrock.Json.Conversion.Converters
{
    #region Imports

    using System;
    using System.Diagnostics;
    using System.Globalization;

    #endregion

    public sealed class BooleanImporter : ImporterBase
    {
        public BooleanImporter() : 
            base(typeof(bool)) { }

        protected override object ImportFromBoolean(ImportContext context, JsonReader reader)
        {
            Debug.Assert(context != null);
            Debug.Assert(reader != null);
            
            return BooleanObject.Box(reader.ReadBoolean());
        }

        protected override object ImportFromNumber(ImportContext context, JsonReader reader)
        {
            Debug.Assert(context != null);
            Debug.Assert(reader != null);

            try
            {
                return BooleanObject.Box(reader.ReadNumber().ToInt64() != 0);                
            }
            catch (FormatException e)
            {
                throw new JsonException(string.Format("The JSON Number {0} must be an integer to be convertible to System.Boolean.", reader.Text), e);
            }
        }
    }
}