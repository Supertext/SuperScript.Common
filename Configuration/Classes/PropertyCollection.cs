using System.Configuration;

namespace SuperScript.Configuration
{
	[ConfigurationCollection(typeof (PropertyCollection), AddItemName = "property", CollectionType = ConfigurationElementCollectionType.BasicMap)]
	public class PropertyCollection : ConfigurationElementCollection
	{
		private static readonly ConfigurationProperty PropertyCollectionElement = new ConfigurationProperty("property", typeof (PropertyElement), null, ConfigurationPropertyOptions.None);

		protected override ConfigurationElement CreateNewElement()
		{
			return new PropertyElement();
		}

		protected override object GetElementKey(ConfigurationElement element)
		{
			return ((PropertyElement) element).Name;
		}

		public new PropertyElement this[string name]
		{
			get { return (PropertyElement) this[PropertyCollectionElement]; }
		}
	}
}