using System.Configuration;

namespace SuperScript.Configuration
{
    public class CustomObjectElement : ModifierElement
    {
        //private static readonly ConfigurationProperty PropertyCollectionsElement = new ConfigurationProperty("propertyCollections", typeof (PropertyCollectionsCollection), null, ConfigurationPropertyOptions.None);
        private static readonly ConfigurationProperty PropertiesElement = new ConfigurationProperty("properties", typeof (PropertyCollection), null, ConfigurationPropertyOptions.None);


        [ConfigurationProperty("properties", IsRequired = false)]
        public PropertyCollection CustomProperties
        {
            get { return (PropertyCollection) this[PropertiesElement]; }
        }


        //[ConfigurationProperty("propertyCollections", IsRequired = false)]
        //public PropertyCollectionsCollection PropertyCollections
        //{
        //    get { return (PropertyCollectionsCollection) this[PropertyCollectionsElement]; }
        //}
    }
}