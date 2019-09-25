namespace Jayrock.Json.Conversion.Converters
{
    #region Imports

    using System;
    using System.Diagnostics;
    using System.Globalization;

    #endregion

    public abstract class NumberImporterBase : ImporterBase
    {
        protected NumberImporterBase(Type type) :
            base(type) {}

        protected override object ImportFromString(ImportContext context, JsonReader reader)
        {
            return ImportFromNumber(context, reader);
        }

        protected override object ImportFromNumber(ImportContext context, JsonReader reader)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            if (reader == null)
                throw new ArgumentNullException("reader");

            string text = reader.Text;
            
            try
            {
                return ReadReturning(reader, ConvertFromString(text));
            }
            catch (FormatException e)
            {
                throw NumberError(e, text);
            }
            catch (OverflowException e)
            {
                throw NumberError(e, text);
            }
        }

        protected override object ImportFromBoolean(ImportContext context, JsonReader reader)
        {
            return Convert.ChangeType(BooleanObject.Box(reader.ReadBoolean()), OutputType);
        }

        protected abstract object ConvertFromString(string s);

        private Exception NumberError(Exception e, string text)
        {
            return new JsonException(string.Format("Error importing JSON Number {0} as {1}.", text, OutputType.FullName), e);
        }
    }
    
    public sealed class ByteImporter : NumberImporterBase
    {
        public ByteImporter() : 
            base(typeof(byte)) {}

        protected override object ConvertFromString(string s)
        {
            return Convert.ToByte(s, CultureInfo.InvariantCulture);
        }
    }

    public sealed class Int16Importer : NumberImporterBase
    {
        public Int16Importer() : 
            base(typeof(short)) {}

        protected override object ConvertFromString(string s)
        {
            return Convert.ToInt16(s, CultureInfo.InvariantCulture);
        }
    }

    public sealed class Int32Importer : NumberImporterBase
    {
        public Int32Importer() : 
            base(typeof(int)) {}

        protected override object ConvertFromString(string s)
        {
            return Convert.ToInt32(s, CultureInfo.InvariantCulture);
        }
    }

    public sealed class Int64Importer : NumberImporterBase
    {
        public Int64Importer() : 
            base(typeof(long)) {}

        protected override object ConvertFromString(string s)
        {
            return Convert.ToInt64(s, CultureInfo.InvariantCulture);
        }
    }

    public sealed class SingleImporter : NumberImporterBase
    {
        public SingleImporter() : 
            base(typeof(float)) {}

        protected override object ConvertFromString(string s)
        {
            return Convert.ToSingle(s, CultureInfo.InvariantCulture);
        }
    }

    public sealed class DoubleImporter : NumberImporterBase
    {
        public DoubleImporter() : 
            base(typeof(double)) {}

        protected override object ConvertFromString(string s)
        {
            return Convert.ToDouble(s, CultureInfo.InvariantCulture);
        }
    }

    public sealed class DecimalImporter : NumberImporterBase
    {
        public DecimalImporter() : 
            base(typeof(decimal)) {}

        protected override object ConvertFromString(string s)
        {
            return Convert.ToDecimal(s, CultureInfo.InvariantCulture);
        }
    }
}