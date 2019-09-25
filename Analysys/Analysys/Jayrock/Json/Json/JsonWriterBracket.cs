namespace Jayrock.Json
{
    #region Imports

    using System;
    using System.Collections;
    using System.Diagnostics;
    using System.Runtime.Serialization;

    #endregion

    [ Serializable ]
    public enum JsonWriterBracket 
    {
        Pending,
        Array,
        Object,
        Member,
        Closed
    };
}