namespace Jayrock.Json.Conversion.Converters
{
    #region Imports

    using System.Collections;

    #endregion

    public class DictionaryImporter : ImportAwareImporter
    {
        public DictionaryImporter() : 
            base(typeof(IDictionary)) {}

        protected override IJsonImportable CreateObject()
        {
            return new JsonObject();
        }
    }
}