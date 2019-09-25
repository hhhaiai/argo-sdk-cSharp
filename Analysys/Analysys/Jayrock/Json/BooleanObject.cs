namespace Jayrock
{
    #region Imports

    using System;

    #endregion

    internal sealed class BooleanObject
    {
        //
        // The following two statics are only used as an optimization so that we
        // don't create a boxed Boolean each time an Object is expecting somewhere.
        // This should help put a little less pressure on the GC where possible.
        //

        public readonly static object True = true;
        public readonly static object False = false;
        
        public static object Box(bool value)
        {
            return value ? True : False;
        }
        
        private BooleanObject()
        {
            throw new NotSupportedException();
        }
    }
}
