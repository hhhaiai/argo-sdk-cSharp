namespace Jayrock.Json.Conversion.Converters
{
    #region Importer

    using System;
    using System.Diagnostics;

    #endregion

    public sealed class ExportAwareExporter : ExporterBase
    {
        public ExportAwareExporter(Type type) : 
            base(type) {}

        protected override void ExportValue(ExportContext context, object value, JsonWriter writer)
        {
            Debug.Assert(context != null);
            Debug.Assert(value != null);
            Debug.Assert(writer != null);

            ((IJsonExportable) value).Export(context, writer);
        }
    }
}