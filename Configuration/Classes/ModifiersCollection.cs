using System.Configuration;

namespace SuperScript.Configuration
{
    [ConfigurationCollection(typeof (ModifierElement), AddItemName = "modifier", CollectionType = ConfigurationElementCollectionType.BasicMap)]
    public class ModifiersCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new ModifierElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ModifierElement) element).Type;
        }

        public new ModifierElement this[string name]
        {
            get { return (ModifierElement) BaseGet(name); }
        }
    }
}