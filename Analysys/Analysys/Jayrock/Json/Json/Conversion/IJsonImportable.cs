namespace Jayrock.Json.Conversion
{

    public interface IJsonImportable
    {
        void Import(ImportContext context, JsonReader reader);
    }
}