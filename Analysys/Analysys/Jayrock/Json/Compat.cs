namespace Jayrock
{
    #region Imports

    using System;

    #endregion

    internal sealed class Compat
    {
        public static Type FindType(string typeName)
        {
            return Type.GetType(typeName, /* throwOnError = */ false, /* ignoreCase = */ false);
        }

        public static Type GetType(string typeName)
        {
            return Type.GetType(typeName, /* throwOnError = */ true, /* ignoreCase = */ false);
        }
        
        private Compat() {}
    }
}
