namespace Jayrock.Json.Conversion.Converters
{
    #region Imports

    using System;
    using System.Diagnostics;
    using System.Globalization;

    #endregion

    public sealed class AnyImporter : ImporterBase
    {
        public AnyImporter() : 
            base(AnyType.Value) {}

        protected override object ImportFromBoolean(ImportContext context, JsonReader reader)
        {
            return BooleanObject.Box(reader.ReadBoolean());
        }

        protected override object ImportFromNumber(ImportContext context, JsonReader reader)
        {
            return reader.ReadNumber();
        }

        protected override object ImportFromString(ImportContext context, JsonReader reader)
        {
            return reader.ReadString();
        }

        protected override object ImportFromArray(ImportContext context, JsonReader reader)
        {
            JsonArray items = new JsonArray();
            ((IJsonImportable) items).Import(context, reader);
            return items;
        }

        protected override object ImportFromObject(ImportContext context, JsonReader reader)
        {
            JsonObject o = new JsonObject();
            ((IJsonImportable) o).Import(context, reader);
            return o;
        }
    }
}