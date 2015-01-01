using System.Configuration;

namespace SuperScript.Configuration
{
    [ConfigurationCollection(typeof(PropertyCollectionsCollection), AddItemName = "propertyCollection", CollectionType = ConfigurationElementCollectionType.BasicMap)]
    public class PropertyCollectionsCollection : ConfigurationElementCollection
    {
        private static readonly ConfigurationProperty PropertyCollectionElement = new ConfigurationProperty("propertyCollection", typeof(PropertyCollection), null, ConfigurationPropertyOptions.None);

        protected override ConfigurationElement CreateNewElement()
        {
            return new PropertyCollection();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((PropertyCollection) element).CollectionType;
        }

        public new PropertyCollection this[string name]
        {
            get { return (PropertyCollection) this[PropertyCollectionElement]; }
        }
    }
}