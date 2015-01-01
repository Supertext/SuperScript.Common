using System.Configuration;

namespace SuperScript.Configuration
{
    [ConfigurationCollection(typeof (AttributeElement), AddItemName = "attribute", CollectionType = ConfigurationElementCollectionType.BasicMap)]
    public class AttributesCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new AttributeElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((AttributeElement) element).Name;
        }

        public new AttributeElement this[string name]
        {
            get { return (AttributeElement) BaseGet(name); }
        }
    }
}