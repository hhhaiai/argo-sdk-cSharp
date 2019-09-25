namespace Jayrock.Json.Conversion
{
    using System;

    /// <summary>
    /// Defines the contract for exporting an object as JSON.
    /// </summary>
    
    public interface IExporter
    {
        Type InputType { get; }
        void Export(ExportContext context, object value, JsonWriter writer);
    }
}