namespace Jayrock.Configuration
{
    #region Imports

    using System;
    using System.Collections;
    using System.Configuration;
    using System.Xml;

    #endregion

    public class TypeListSectionHandler : ListSectionHandler
    {
        private readonly Type _expectedType;
        
        public TypeListSectionHandler(string elementName) : 
            this(elementName, null) {}

        public TypeListSectionHandler(string elementName, Type expectedType) : 
            base(elementName)
        {
            _expectedType = expectedType;
        }

        protected Type ExpectedType
        {
            get { return _expectedType; }
        }

        protected override object GetItem(XmlElement element)
        {
            if (element == null)
                throw new ArgumentNullException("element");

            string typeName = element.GetAttribute("type");
            
            if (typeName.Length == 0)
            {
                throw new ConfigurationErrorsException(string.Format("Missing type name specification on <{0}> element.", ElementName), element);
            }

            Type type = GetType(typeName);
            ValidateType(type, element);
            return type;
        }
 
        protected virtual void ValidateType(Type type, XmlElement element)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            if (element == null)
                throw new ArgumentNullException("element");
            
            if (ExpectedType == null)
                return;
            
            if (!ExpectedType.IsAssignableFrom(type))
                throw new ConfigurationErrorsException(string.Format("The type {0} is not valid for the <{2}> configuration element. It must be compatible with the type {1}.", type.FullName, ExpectedType.FullName, element.Name), element);
        }

        protected virtual Type GetType(string typeName) 
        {
            if (typeName == null)
                throw new ArgumentNullException("typeName");
            
            return Compat.GetType(typeName);
        }
    }
}