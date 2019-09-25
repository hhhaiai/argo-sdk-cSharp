namespace Jayrock.Json
{
    #region Imports

    using System;

    #endregion

    public abstract class JsonReaderBase : JsonReader
    {
        private JsonToken _token;
        private int _depth;

        public JsonReaderBase()
        {
            _token = JsonToken.BOF();
        }

        /// <summary>
        /// Gets the current token.
        /// </summary>

        public sealed override JsonToken Token
        {
            get { return _token; }
        }

        /// <summary>
        /// Return the current level of nesting as the reader encounters
        /// nested objects and arrays.
        /// </summary>

        public sealed override int Depth
        {
            get { return _depth; }
        }

        /// <summary>
        /// Reads the next token and returns true if one was found.
        /// </summary>

        public sealed override bool Read()
        {
            if (!EOF)
            {
                if (TokenClass == JsonTokenClass.EndObject || TokenClass == JsonTokenClass.EndArray)
                    _depth--;

                _token = ReadTokenImpl();

                if (TokenClass == JsonTokenClass.Object || TokenClass == JsonTokenClass.Array)
                    _depth++;
            }
            
            return !EOF;
        }

        /// <summary>
        /// Reads the next token and returns it.
        /// </summary>
        
        protected abstract JsonToken ReadTokenImpl();
    }
}