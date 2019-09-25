namespace Jayrock.Json.Conversion
{
    #region Imports

    using Jayrock.Configuration;

    #endregion

    internal sealed class ExporterListSectionHandler : TypeListSectionHandler
    {
        public ExporterListSectionHandler() : 
            base("exporter", typeof(IExporter)) {}
    }
}