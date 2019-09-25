namespace Jayrock.Json.Conversion
{
    #region Imports

    using System;
    using Jayrock.Configuration;

    #endregion

    internal sealed class ImporterListSectionHandler : TypeListSectionHandler
    {
        public ImporterListSectionHandler() : 
            base("importer", typeof(IImporter)) {}
    }
}