namespace Jayrock.Json.Conversion.Converters
{
    #region Imports

    using System;
    using System.Collections;
    using System.Diagnostics;

    #endregion
    
    public sealed class EnumerableExporter : ExporterBase
    {
        public EnumerableExporter(Type inputType) : 
            base(inputType) {}

        protected override void ExportValue(ExportContext context, object value, JsonWriter writer)
        {
            Debug.Assert(context != null);
            Debug.Assert(value != null);
            Debug.Assert(writer != null);

            IEnumerable items = (IEnumerable) value;
            
            writer.WriteStartArray();

            foreach (object item in items)
                context.Export(item, writer);

            writer.WriteEndArray();
        }
    }
}