namespace Jayrock.Json.Conversion
{
    
    public interface IJsonExportable
    {
        void Export(ExportContext context, JsonWriter writer);
    }
}