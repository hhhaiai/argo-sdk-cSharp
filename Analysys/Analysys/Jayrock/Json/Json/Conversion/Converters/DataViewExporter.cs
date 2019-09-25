namespace Jayrock.Json.Conversion.Converters
{
    #region Imports

    using System;
    using System.Data;
    using System.Diagnostics;

    #endregion

    public sealed class DataViewExporter : ExporterBase
    {
        public DataViewExporter() :
            this(typeof(DataView)) {}

        public DataViewExporter(Type inputType) : 
            base(inputType) {}

        protected override void ExportValue(ExportContext context, object value, JsonWriter writer)
        {
            Debug.Assert(context != null);
            Debug.Assert(value != null);
            Debug.Assert(writer != null);
            
            ExportView(context, (DataView) value, writer);
        }

        internal static void ExportView(ExportContext context, DataView view, JsonWriter writer)
        {
            Debug.Assert(context != null);
            Debug.Assert(view != null);
            Debug.Assert(writer != null);

            writer.WriteStartObject();

            writer.WriteMember("columns");

            writer.WriteStartArray();
            foreach (DataColumn column in view.Table.Columns)
                context.Export(column.ColumnName, writer);
            writer.WriteEndArray();

            writer.WriteMember("rows");

            writer.WriteStartArray();
            foreach (DataRowView row in view)
                context.Export(row.Row.ItemArray, writer);
            writer.WriteEndArray();

            writer.WriteEndObject();
        }
    }
}