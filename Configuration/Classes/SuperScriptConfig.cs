using System.Configuration;

namespace SuperScript.Configuration
{
	/// <summary>
	/// This class represents the default configuration from the web.config file.
	/// </summary>
	public class SuperScriptConfig : ConfigurationSection
	{
		private static readonly ConfigurationProperty EmitterBundlesElement = new ConfigurationProperty("emitterBundles", typeof (EmitterBundlesCollection), null, ConfigurationPropertyOptions.None);
		private static readonly ConfigurationProperty EmittersElement = new ConfigurationProperty("emitters", typeof (EmittersCollection), null, ConfigurationPropertyOptions.None);


		[ConfigurationProperty("emitterBundles", IsRequired = false)]
		public EmitterBundlesCollection EmitterBundles
		{
			get { return (EmitterBundlesCollection) this[EmitterBundlesElement]; }
		}


		[ConfigurationProperty("emitters", IsRequired = true)]
		public EmittersCollection Emitters
		{
			get { return (EmittersCollection) this[EmittersElement]; }
		}
	}
}