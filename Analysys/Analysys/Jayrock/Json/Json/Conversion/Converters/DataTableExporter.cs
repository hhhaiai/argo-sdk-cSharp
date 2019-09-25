namespace Jayrock.Json.Conversion.Converters
{
    #region Imports

    using System;
    using System.Data;
    using System.Diagnostics;

    #endregion
    
    public sealed class DataTableExporter : ExporterBase
    {
        public DataTableExporter() :
            this(typeof(DataTable)) {}

        public DataTableExporter(Type inputType) : 
            base(inputType) {}

        protected override void ExportValue(ExportContext context, object value, JsonWriter writer)
        {
            Debug.Assert(context != null);
            Debug.Assert(value != null);
            Debug.Assert(writer != null);

            ExportTable(context, (DataTable) value, writer);
        }

        internal static void ExportTable(ExportContext context, DataTable table, JsonWriter writer)
        {
            Debug.Assert(context != null);
            Debug.Assert(table != null);
            Debug.Assert(writer != null);

            DataViewExporter.ExportView(context, table.DefaultView, writer);
       }
    }
}
