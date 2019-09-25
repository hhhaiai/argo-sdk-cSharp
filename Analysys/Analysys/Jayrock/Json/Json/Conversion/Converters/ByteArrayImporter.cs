namespace Jayrock.Json.Conversion.Converters
{
    #region Imports

    using System;
    using System.Diagnostics;
    using System.IO;

    #endregion

    public sealed class ByteArrayImporter : ImporterBase
    {
        public ByteArrayImporter() : base(typeof(byte[])) {}

        protected override object ImportFromArray(ImportContext context, JsonReader reader)
        {
            Debug.Assert(context != null);
            Debug.Assert(reader != null);

            reader.Read();
            
            MemoryStream ms = new MemoryStream();
            Type byteType = typeof(byte);

            while (reader.TokenClass != JsonTokenClass.EndArray)
                ms.WriteByte((byte) context.Import(byteType, reader));

            return ReadReturning(reader, ms.ToArray());
        }

        protected override object ImportFromBoolean(ImportContext context, JsonReader reader)
        {
            return new byte[] { (byte) (reader.ReadBoolean() ? 1 : 0) };
        }

        protected override object ImportFromNumber(ImportContext context, JsonReader reader)
        {
            return new byte[] { reader.ReadNumber().ToByte() };
        }

        protected override object ImportFromString(ImportContext context, JsonReader reader)
        {
            try
            {
                return Convert.FromBase64String(reader.ReadString());
            }
            catch (FormatException e)
            {
                throw new JsonException("Error converting JSON String containing base64-encode data to " + OutputType.FullName + ".", e);
            }
        }
    }
}