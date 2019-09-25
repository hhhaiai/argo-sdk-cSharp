namespace Jayrock.Json.Conversion
{
    #region Imports

    using System;
    using Jayrock.Collections;

    #endregion

    [ Serializable ]
    internal sealed class ExporterCollection : KeyedCollection
    {
        public IExporter this[Type type]
        {
            get { return (IExporter) GetByKey(type); }
        }
       
        public void Put(IExporter exporter)
        {
            if (exporter == null)
                throw new ArgumentNullException("exporter");
            
            Remove(exporter.InputType);
            Add(exporter);
        }

        public void Add(IExporter exporter)
        {
            if (exporter == null)
                throw new ArgumentNullException("exporter");
            
            base.Add(exporter);
        }
        
        protected override object KeyFromValue(object value)
        {
            return ((IExporter) value).InputType;
        }
    }
}