namespace Jayrock.Json.Conversion.Converters
{
    #region Imports

    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using CustomTypeDescriptor = Conversion.CustomTypeDescriptor;

    #endregion
    
    public sealed class ComponentImporter : ImporterBase
    {
        private readonly PropertyDescriptorCollection _properties; // TODO: Review thread-safety of PropertyDescriptorCollection

        public ComponentImporter(Type type) :
            this(type, null) {}

        public ComponentImporter(Type type, ICustomTypeDescriptor typeDescriptor) :
            base(type)
        {
            if (typeDescriptor == null)
                typeDescriptor = new CustomTypeDescriptor(type);
            
            _properties = typeDescriptor.GetProperties();
        }

        protected override object ImportFromObject(ImportContext context, JsonReader reader)
        {
            Debug.Assert(context != null);
            Debug.Assert(reader != null);

            reader.Read();
            
            object o = Activator.CreateInstance(OutputType);
            
            while (reader.TokenClass != JsonTokenClass.EndObject)
            {
                string memberName = reader.ReadMember();
               
                PropertyDescriptor property = _properties.Find(memberName, true);
                
                if (property != null && !property.IsReadOnly)
                    property.SetValue(o, context.Import(property.PropertyType, reader));
                else 
                    reader.Skip();
            }
         
            return ReadReturning(reader, o);
        }
    }
}
