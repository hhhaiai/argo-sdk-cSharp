namespace Jayrock.Json.Conversion.Converters
{
    #region Imports

    using System;
    using System.Diagnostics;
    using System.Globalization;

    #endregion

    public sealed class BooleanExporter : ExporterBase
    {
        public BooleanExporter() : base(typeof(bool)) { }

        protected override void ExportValue(ExportContext context, object value, JsonWriter writer)
        {
            Debug.Assert(context != null);
            Debug.Assert(value != null);
            Debug.Assert(writer != null);
            
            writer.WriteBoolean((bool) value);
        }
    }
}
