namespace Jayrock.Json.Conversion
{
    #region Imports

    using System;
    using Jayrock.Collections;

    #endregion
    
    [ Serializable ]
    internal sealed class ImporterCollection : KeyedCollection
    {
        public IImporter this[Type type]
        {
            get { return (IImporter) GetByKey(type); }
        }
       
        public void Put(IImporter importer)
        {
            if (importer == null)
                throw new ArgumentNullException("importer");
            
            Remove(importer.OutputType);
            Add(importer);
        }

        public void Add(IImporter importer)
        {
            if (importer == null)
                throw new ArgumentNullException("importer");
            
            base.Add(importer);
        }
        
        protected override object KeyFromValue(object value)
        {
            return ((IImporter) value).OutputType;
        }
    }
}