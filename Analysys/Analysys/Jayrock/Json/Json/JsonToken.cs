namespace Jayrock.Json
{
    #region Imports

    using System;
    using Jayrock.Diagnostics;

    #endregion

    [ Serializable ]
    public struct JsonToken
    {
        private readonly JsonTokenClass _class;
        private readonly string _text;

        private JsonToken(JsonTokenClass clazz) :
            this(clazz, null) {}

        private JsonToken(JsonTokenClass clazz, string text)
        {
            _class = clazz;
            _text = text;
        }
        
        /// <summary>
        /// Gets the class/type/category of the token.
        /// </summary>

        public JsonTokenClass Class
        {
            get { return _class; }
        }

        /// <summary>
        /// Gets the current token text, if applicable, otherwise null.
        /// </summary>

        public string Text
        {
            get { return _text; }
        }

        public override string ToString()
        {
            return Text == null ? Class.Name : Class.Name + ":" + DebugString.Format(Text);
        }
        
        //
        // Static methods for building tokens of various classes...
        //

        public static JsonToken Null()
        {
            return new JsonToken(JsonTokenClass.Null, JsonNull.Text);
        }

        public static JsonToken Array()
        {
            return new JsonToken(JsonTokenClass.Array);
        }

        public static JsonToken EndArray()
        {
            return new JsonToken(JsonTokenClass.EndArray);
        }

        public static JsonToken Object()
        {
            return new JsonToken(JsonTokenClass.Object);
        }

        public static JsonToken EndObject()
        {
            return new JsonToken(JsonTokenClass.EndObject);
        }

        public static JsonToken BOF()
        {
            return new JsonToken(JsonTokenClass.BOF);
        }

        public static JsonToken EOF()
        {
            return new JsonToken(JsonTokenClass.EOF);
        }

        public static JsonToken String(string text)
        {
            return new JsonToken(JsonTokenClass.String, Mask.NullString(text));
        }

        public static JsonToken Boolean(bool value)
        {
            return new JsonToken(JsonTokenClass.Boolean, value ? "true" : "false");
        }

        public static JsonToken True()
        {
            return Boolean(true);
        }

        public static JsonToken False()
        {
            return Boolean(false);
        }
        
        public static JsonToken Number(string text)
        {
            if (text == null)                   
                throw new ArgumentNullException("text)");
                    
            if (text.Length == 0)
                throw new ArgumentException("Number text cannot zero in length.", "text)");
                    
            return new JsonToken(JsonTokenClass.Number, text);
        }

        public static JsonToken Member(string name)
        {
            return new JsonToken(JsonTokenClass.Member, name);
        }
    }
}