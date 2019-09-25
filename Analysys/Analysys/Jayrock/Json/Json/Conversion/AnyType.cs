namespace Jayrock.Json.Conversion
{
    #region Imports

    using System;

    #endregion

    public sealed class AnyType
    {
        public static readonly Type Value = typeof(object);
        
        private AnyType()
        {
            throw new NotImplementedException();
        }
    }
}