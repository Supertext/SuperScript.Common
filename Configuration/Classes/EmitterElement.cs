using System;
using System.ComponentModel;
using System.Configuration;

namespace SuperScript.Configuration
{
    public class EmitterElement : ConfigurationElement, IAssemblyElement
    {
        private static readonly ConfigurationProperty ConvertersElement = new ConfigurationProperty("converters", typeof (ConvertersCollection), null, ConfigurationPropertyOptions.None);
        private static readonly ConfigurationProperty CustomObjectElement = new ConfigurationProperty("customObject", typeof(CustomObjectElement), null, ConfigurationPropertyOptions.None);
        private static readonly ConfigurationProperty PreModifiersElement = new ConfigurationProperty("preModifiers", typeof (PreModifiersCollection), null, ConfigurationPropertyOptions.None);
        private static readonly ConfigurationProperty PostModifiersElement = new ConfigurationProperty("postModifiers", typeof(PostModifiersCollection), null, ConfigurationPropertyOptions.None);
        private static readonly ConfigurationProperty WritersElement = new ConfigurationProperty("writers", typeof (WritersCollection), null, ConfigurationPropertyOptions.None);

	    [ConfigurationProperty("converters", IsRequired = true)]
	    public ConvertersCollection Converters
	    {
		    get { return (ConvertersCollection) this[ConvertersElement]; }
	    }

	    [ConfigurationProperty("customObject", IsRequired = false)]
        public CustomObjectElement CustomObject
        {
            get { return (CustomObjectElement)this[CustomObjectElement]; }
        }

        [ConfigurationProperty("isDefault", IsRequired = false, DefaultValue = false)]
        public bool IsDefault
        {
            get { return (bool) this["isDefault"]; }
        }

        [ConfigurationProperty("key", IsRequired = true)]
        public string Key
        {
            get { return (string) this["key"]; }
        }

        [ConfigurationProperty("preModifiers", IsRequired = false)]
        public PreModifiersCollection PreModifiers
        {
            get { return (PreModifiersCollection) this[PreModifiersElement]; }
        }

        [ConfigurationProperty("postModifiers", IsRequired = false)]
        public PostModifiersCollection PostModifiers
        {
            get { return (PostModifiersCollection) this[PostModifiersElement]; }
        }

        [ConfigurationProperty("type", IsRequired = false, DefaultValue = "SuperScript.Emitters.StandardEmitter, SuperScript.Common")]
        [TypeConverter(typeof (TypeNameConverter))]
        public Type Type
        {
            get { return (Type) this["type"]; }
        }

	    [ConfigurationProperty("writers", IsRequired = false)]
	    public WritersCollection Writers
	    {
		    get { return (WritersCollection) this[WritersElement]; }
	    }
    }
}