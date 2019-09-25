namespace Jayrock.Json.Conversion.Converters
{
    #region Imports

    using System;
    using System.Data;
    using System.Diagnostics;

    #endregion

    public sealed class DataSetExporter : ExporterBase
    {
        public DataSetExporter() :
            this(typeof(DataSet)) {}

        public DataSetExporter(Type inputType) : 
            base(inputType) {}

        protected override void ExportValue(ExportContext context, object value, JsonWriter writer)
        {
            Debug.Assert(context != null);
            Debug.Assert(value != null);
            Debug.Assert(writer != null);
            
            ExportDataSet(context, (DataSet) value, writer);
        }

        private static void ExportDataSet(ExportContext context, DataSet dataSet, JsonWriter writer)
        {
            Debug.Assert(context != null);
            Debug.Assert(dataSet != null);
            Debug.Assert(writer != null);

            writer.WriteStartObject();
    
            foreach (DataTable table in dataSet.Tables)
            {
                writer.WriteMember(table.TableName);
                DataTableExporter.ExportTable(context, table, writer);
            }
    
            writer.WriteEndObject();
        }
    }
}