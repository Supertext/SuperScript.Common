using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;

namespace SuperScript.Configuration
{
	public class EmitterBundleElement : ConfigurationElement
	{
		private static readonly ConfigurationProperty CustomObjectElement = new ConfigurationProperty("customObject", typeof(CustomObjectElement), null, ConfigurationPropertyOptions.None);
		private static readonly ConfigurationProperty PostModifiersElement = new ConfigurationProperty("postModifiers", typeof (PostModifiersCollection), null, ConfigurationPropertyOptions.None);
		private static readonly ConfigurationProperty WritersElement = new ConfigurationProperty("writers", typeof (WritersCollection), null, ConfigurationPropertyOptions.None);

		[ConfigurationProperty("bundleKeys", IsRequired = true)]
		[TypeConverter(typeof (CommaDelimitedStringCollectionConverter))]
		public StringCollection BundleKeys
		{
			get { return (CommaDelimitedStringCollection) this["bundleKeys"]; }
		}

		[ConfigurationProperty("customObject", IsRequired = false)]
		public CustomObjectElement CustomObject
		{
			get { return (CustomObjectElement) this[CustomObjectElement]; }
		}

		[ConfigurationProperty("key", IsRequired = true)]
		public string Key
		{
			get { return (string) this["key"]; }
		}

		[ConfigurationProperty("postModifiers", IsRequired = false)]
		public PostModifiersCollection PostModifiers
		{
			get { return (PostModifiersCollection) this[PostModifiersElement]; }
		}

		[ConfigurationProperty("writers", IsRequired = true)]
		public WritersCollection Writers
		{
			get { return (WritersCollection) this[WritersElement]; }
		}
	}
}