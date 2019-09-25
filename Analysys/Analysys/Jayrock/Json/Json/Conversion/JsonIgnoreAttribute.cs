namespace Jayrock.Json.Conversion
{
    #region Imports

    using System;
    using System.Reflection;

    #endregion


    [ Serializable ]
    [ AttributeUsage(AttributeTargets.Property | AttributeTargets.Field) ]
    public sealed class JsonIgnoreAttribute : Attribute
    {
    }
}