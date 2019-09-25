namespace Jayrock.Json
{
    #region Imports

    using System;

    using SerializationInfo = System.Runtime.Serialization.SerializationInfo;
    using StreamingContext = System.Runtime.Serialization.StreamingContext;

    #endregion

    [ Serializable ]
    public class JsonException : System.ApplicationException
    {
        private const string _defaultMessage = "An error occurred dealing with JSON data.";

        public JsonException() : 
            this(null) {}

        public JsonException(string message) : 
            base(Mask.NullString(message, _defaultMessage), null) {}

        public JsonException(string message, Exception innerException) :
            base(Mask.NullString(message, _defaultMessage), innerException) {}

        protected JsonException(SerializationInfo info, StreamingContext context) :
            base(info, context) {}
    }
}