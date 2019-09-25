namespace Jayrock.Json.Conversion.Converters
{
    #region Imports

    using System.Collections;

    #endregion

    public class ListImporter : ImportAwareImporter
    {
        public ListImporter() : 
            base(typeof(IList)) {}

        protected override IJsonImportable CreateObject()
        {
            return new JsonArray();
        }
    }
}