using System.Configuration;

namespace SuperScript.Configuration
{
    [ConfigurationCollection(typeof (EmitterElement), AddItemName = "emitter", CollectionType = ConfigurationElementCollectionType.BasicMap)]
    public class EmittersCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new EmitterElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((EmitterElement) element).Key;
        }

        public new EmitterElement this[string name]
        {
            get { return (EmitterElement) BaseGet(name); }
        }
    }
}