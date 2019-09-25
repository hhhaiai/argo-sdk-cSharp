namespace Jayrock.Json.Conversion.Converters
{
    #region Imports

    using System;
    using System.Collections;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Diagnostics;
    
    #endregion

    public sealed class NameValueCollectionExporter : ExporterBase
    {
        public NameValueCollectionExporter() : 
            this(typeof(NameValueCollection)) {}

        public NameValueCollectionExporter(Type inputType) : 
            base(inputType) {}

        protected override void ExportValue(ExportContext context, object value, JsonWriter writer)
        {
            Debug.Assert(context != null);
            Debug.Assert(value != null);
            Debug.Assert(writer != null);

            ExportCollection(context, (NameValueCollection) value, writer);
        }

        private static void ExportCollection(ExportContext context, NameValueCollection collection, JsonWriter writer)
        {
            Debug.Assert(context != null);
            Debug.Assert(collection != null);
            Debug.Assert(writer != null);

            writer.WriteStartObject();

            for (int i = 0; i < collection.Count; i++)
            {
                writer.WriteMember(collection.GetKey(i));

                string[] values = collection.GetValues(i);

                if (values == null)
                    writer.WriteNull();
                else if (values.Length > 1)
                    context.Export(values, writer);
                else
                    context.Export(values[0], writer);
            }

            writer.WriteEndObject();
        }
    }
}
