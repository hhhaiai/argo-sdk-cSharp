namespace Jayrock.Json.Conversion
{
    #region Imports

    using System;
    using System.Collections;
    using Jayrock.Json.Conversion.Converters;

    #endregion

    public interface IImporter
    {
        Type OutputType { get; }
        object Import(ImportContext context, JsonReader reader);
    }
}