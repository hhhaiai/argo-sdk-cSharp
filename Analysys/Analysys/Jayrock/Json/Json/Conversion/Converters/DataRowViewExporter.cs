namespace Jayrock.Json.Conversion.Converters
{
    #region Imports

    using System;
    using System.Collections;
    using System.Data;
    using System.Diagnostics;

    #endregion

    public sealed class DataRowViewExporter : ExporterBase
    {
        public DataRowViewExporter() :
            this(typeof(DataRowView)) {}

        public DataRowViewExporter(Type inputType) : 
            base(inputType) {}

        protected override void ExportValue(ExportContext context, object value, JsonWriter writer)
        {
            Debug.Assert(context != null);
            Debug.Assert(value != null);
            Debug.Assert(writer != null);

            ExportRowView(context, (DataRowView) value, writer);
        }

        private static void ExportRowView(ExportContext context, DataRowView rowView, JsonWriter writer)
        {
            Debug.Assert(context != null);
            Debug.Assert(rowView != null);
            Debug.Assert(writer != null);

            writer.WriteStartObject();
    
            foreach (DataColumn column in rowView.DataView.Table.Columns)
            {
                writer.WriteMember(column.ColumnName);
                context.Export(rowView[column.Ordinal], writer);
            }
    
            writer.WriteEndObject();
        }
    }
}