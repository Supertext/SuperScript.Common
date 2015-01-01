using System.Configuration;

namespace SuperScript.Configuration
{
	[ConfigurationCollection(typeof (EmitterBundleElement), AddItemName = "emitterBundle", CollectionType = ConfigurationElementCollectionType.BasicMap)]
	public class EmitterBundlesCollection : ConfigurationElementCollection
	{
		protected override ConfigurationElement CreateNewElement()
		{
			return new EmitterBundleElement();
		}

		protected override object GetElementKey(ConfigurationElement element)
		{
			return ((EmitterBundleElement) element).Key;
		}

		public new EmitterBundleElement this[string name]
		{
			get { return (EmitterBundleElement) BaseGet(name); }
		}
	}
}