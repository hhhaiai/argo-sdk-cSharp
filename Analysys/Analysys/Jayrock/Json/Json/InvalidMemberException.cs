namespace Jayrock.Json
{
    #region Imports

    using System;

    using SerializationInfo = System.Runtime.Serialization.SerializationInfo;
    using StreamingContext = System.Runtime.Serialization.StreamingContext;

    #endregion

    [ Serializable ]
    public class InvalidMemberException : System.ApplicationException
    {
        private const string _defaultMessage = "No element exists at the specified index.";
        
        public InvalidMemberException() : this(null) {}

        public InvalidMemberException(string message) : 
            base(Mask.NullString(message, _defaultMessage)) {}

        public InvalidMemberException(string message, Exception innerException) :
            base(Mask.NullString(message, _defaultMessage), innerException) {}

        protected InvalidMemberException(SerializationInfo info, StreamingContext context) :
            base(info, context) {}
    }
}