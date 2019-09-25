namespace Jayrock.Json
{
    #region Imports

    using System;
    using System.Runtime.Serialization;
    using Jayrock.Json.Conversion;

    #endregion


    [ Serializable ]
    public sealed class JsonNull : IObjectReference, IJsonExportable
    {
        public const string Text = "null";
        public static readonly JsonNull Value = new JsonNull();

        private JsonNull() {}

        public override string ToString()
        {
            return JsonNull.Text;
        }

        public static bool LogicallyEquals(object o)
        {
            //
            // Equals a null reference?
            //

            if (o == null)
                return true;

            //
            // Equals self, of course?
            //

            if (o.Equals(JsonNull.Value))
                return true;

            //
            // Equals the logical null value used in database applications?
            //

            if (Convert.IsDBNull(o))
                return true;
            
            //
            // Instance is not one of the known logical null values.
            //

            return false;
        }
        
        void IJsonExportable.Export(ExportContext context, JsonWriter writer)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            if (writer == null)
                throw new ArgumentNullException("writer");
            
            writer.WriteNull();
        }
        
        object IObjectReference.GetRealObject(StreamingContext context)
        {
            return JsonNull.Value;
        }
    }
}
