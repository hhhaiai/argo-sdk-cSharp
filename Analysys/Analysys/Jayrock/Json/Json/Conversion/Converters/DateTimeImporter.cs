namespace Jayrock.Json.Conversion.Converters
{
    #region Imports

    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.Xml;

    #endregion

    public sealed class DateTimeImporter : ImporterBase
    {
        public DateTimeImporter() :
            base(typeof(DateTime)) { }

        protected override object ImportFromString(ImportContext context, JsonReader reader)
        {
            Debug.Assert(context != null);
            Debug.Assert(reader != null);

            try
            {
                return ReadReturning(reader, XmlConvert.ToDateTime(reader.Text, XmlDateTimeSerializationMode.Local));
            }
            catch (FormatException e)
            {
                throw new JsonException("Error importing JSON String as System.DateTime.", e);
            }
        }

        protected override object ImportFromNumber(ImportContext context, JsonReader reader)
        {
            Debug.Assert(context != null);
            Debug.Assert(reader != null);

            string text = reader.Text;

            long time;

            try
            {
                time = Convert.ToInt64(text, CultureInfo.InvariantCulture);
            }
            catch (FormatException e)
            {
                throw NumberError(e, text);
            }
            catch (OverflowException e)
            {
                throw NumberError(e, text);
            }

            try
            {
                return ReadReturning(reader, UnixTime.ToDateTime(time));
            }
            catch (ArgumentException e)
            {
                throw NumberError(e, text);
            }
        }

        private static JsonException NumberError(Exception e, string text)
        {
            return new JsonException(string.Format("Error importing JSON Number {0} as System.DateTime.", text), e);
        }

        protected override JsonException GetImportException(string jsonValueType)
        {
            return new JsonException(string.Format("Found {0} where expecting a JSON String in ISO 8601 time format or a JSON Number expressed in Unix time.", jsonValueType));
        }
    }
}