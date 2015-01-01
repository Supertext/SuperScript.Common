using System;
using System.ComponentModel;
using System.Configuration;

namespace SuperScript.Configuration
{
	public class PropertyElement : ConfigurationElement, IAssemblyElement
	{
		[ConfigurationProperty("exceptionIfMissing", IsRequired = false, DefaultValue = true)]
		public bool ExceptionIfMissing
		{
			get { return (bool) this["exceptionIfMissing"]; }
		}

		[ConfigurationProperty("name", IsRequired = true)]
		public string Name
		{
			get { return (String) this["name"]; }
		}

		[ConfigurationProperty("type", IsRequired = false)]
		[TypeConverter(typeof (TypeNameConverter))]
		public Type Type
		{
			get { return (Type) this["type"]; }
		}

		[ConfigurationProperty("value", IsRequired = false)]
		public string Value
		{
			get { return (string) this["value"]; }
		}

#if DEBUG
		/// <summary>
		/// <para>Returns a string containing data about the object's current state.</para>
		/// <para>This property is intended for debugging purposes.</para>
		/// </summary>
		public override string ToString()
		{
			return Type != null
				       ? Name + ": " + Value + " (" + Type + ")"
				       : Name + ": " + Value;
		}
#endif
	}
}