namespace Jayrock.Json.Conversion.Converters
{
    #region Imports

    using System;
    using System.Diagnostics;
    using System.Globalization;

    #endregion

    public abstract class NumberExporterBase : ExporterBase
    {
        protected NumberExporterBase(Type inputType) : 
            base(inputType) {}
        
        protected override void ExportValue(ExportContext context, object value, JsonWriter writer)
        {
            if (context == null)
                throw new ArgumentNullException("context");
            
            if (value == null)
                throw new ArgumentNullException("value");

            if (writer == null)
                throw new ArgumentNullException("writer");
            
            string s;

            try
            {
                s = ConvertToString(value);
            }
            catch (InvalidCastException e)
            {
                throw new JsonException(e.Message, e);
            }

            writer.WriteNumber(s);
        }
        
        protected abstract string ConvertToString(object value);
    }
    
    public class ByteExporter : NumberExporterBase
    {
        public ByteExporter() : 
            base(typeof(byte)) {}

        protected override string ConvertToString(object value)
        {
            return ((byte) value).ToString(CultureInfo.InvariantCulture);
        }
    }

    public class Int16Exporter : NumberExporterBase
    {
        public Int16Exporter() : 
            base(typeof(short)) {}

        protected override string ConvertToString(object value)
        {
            return ((short) value).ToString(CultureInfo.InvariantCulture);
        }
    }

    public class Int32Exporter : NumberExporterBase
    {
        public Int32Exporter() : 
            base(typeof(int)) {}

        protected override string ConvertToString(object value)
        {
            return ((int) value).ToString(CultureInfo.InvariantCulture);
        }
    }

    public class Int64Exporter : NumberExporterBase
    {
        public Int64Exporter() : 
            base(typeof(long)) {}

        protected override string ConvertToString(object value)
        {
            return ((long) value).ToString(CultureInfo.InvariantCulture);
        }
    }

    public class SingleExporter : NumberExporterBase
    {
        public SingleExporter() : 
            base(typeof(float)) {}

        protected override string ConvertToString(object value)
        {
            return ((float) value).ToString(CultureInfo.InvariantCulture);
        }
    }

    public class DoubleExporter : NumberExporterBase
    {
        public DoubleExporter() : 
            base(typeof(double)) {}

        protected override string ConvertToString(object value)
        {
            return ((double) value).ToString(CultureInfo.InvariantCulture);
        }
    }

    public class DecimalExporter : NumberExporterBase
    {
        public DecimalExporter() : 
            base(typeof(decimal)) {}

        protected override string ConvertToString(object value)
        {
            return ((decimal) value).ToString(CultureInfo.InvariantCulture);
        }
    }
}
