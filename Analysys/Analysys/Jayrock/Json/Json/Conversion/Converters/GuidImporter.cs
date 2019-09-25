namespace Jayrock.Json.Conversion.Converters
{
    #region Imports

    using System;
    using System.Diagnostics;

    #endregion

    public sealed class GuidImporter : ImporterBase
    {
        public GuidImporter() : 
            base(typeof(Guid)) { }

        protected override object ImportFromString(ImportContext context, JsonReader reader)
        {
            Debug.Assert(context != null);
            Debug.Assert(reader != null);

            try
            {
                return ReadReturning(reader, new Guid(reader.Text.Trim()));
            }
            catch (FormatException e)
            {
                throw new JsonException(e.Message, e);
            }
        }
    }
}