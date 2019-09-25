namespace Jayrock.Json.Conversion.Converters
{
    #region Imports

    using System;
    using System.Data;
    using System.Diagnostics;
    using System.Globalization;

    #endregion

    public class DateTimeExporter : ExporterBase
    {
        public DateTimeExporter() : 
            base(typeof(DateTime)) {}

        protected override void ExportValue(ExportContext context, object value, JsonWriter writer)
        {
            Debug.Assert(value != null);
            Debug.Assert(writer != null);
            
            ExportTime((DateTime) value, writer);
        }
 
        private void ExportTime(DateTime localTime, JsonWriter writer)
        {
            Debug.Assert(writer != null);

            writer.WriteString(localTime.ToString("yyyy-MM-ddTHH:mm:ss.fffffffzzzzzz", CultureInfo.InvariantCulture));
        }
    }
}