using System;
using System.ComponentModel;
using System.Configuration;

namespace SuperScript.Configuration
{
	public class ModifierElement : ConfigurationElement, IAssemblyElement
	{
		private static readonly ConfigurationProperty PropertiesElement = new ConfigurationProperty("properties", typeof (PropertyCollection), null, ConfigurationPropertyOptions.None);


		[ConfigurationProperty("emitMode", IsRequired = false, DefaultValue = EmitMode.Always)]
		public EmitMode EmitMode
		{
			get { return (EmitMode) this["emitMode"]; }
		}


		[ConfigurationProperty("properties", IsRequired = false)]
		public PropertyCollection ModifierProperties
		{
			get { return (PropertyCollection) this[PropertiesElement]; }
		}


		[ConfigurationProperty("type", IsRequired = true)]
		[TypeConverter(typeof (TypeNameConverter))]
		public Type Type
		{
			get { return (Type) this["type"]; }
		}


		[ConfigurationProperty("useWhenBundled", IsRequired = false, DefaultValue = true)]
		public bool UseWhenBundled
		{
			get { return (bool) this["useWhenBundled"]; }
		}
	}
}