using System;
using System.Configuration;

namespace SuperScript.Configuration
{
    public class AttributeElement : ConfigurationElement
    {
        [ConfigurationProperty("name", IsRequired = true)]
        public string Name
        {
            get { return (String) this["name"]; }
        }

        [ConfigurationProperty("value", IsRequired = false)]
        public string Value
        {
            get { return (string) this["value"]; }
        }
    }
}