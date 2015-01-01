using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using SuperScript.Emitters;
using SuperScript.ExtensionMethods;

namespace SuperScript.Configuration
{
    /// <summary>
    /// This class represents any default configured declarations from the web.config.
    /// </summary>
    public class Settings
    {
        #region Public properties

		/// <summary>
		/// Gets or sets a collection of <see cref="EmitterBundle"/> instances.
		/// </summary>
	    public List<EmitterBundle> EmitterBundles { get; set; }


		/// <summary>
        /// <para>Gets the instance of <see cref="IEmitter"/> which has IsDefault=true.</para>
        /// <para>If no instance of <see cref="IEmitter"/> has this status explicitly indicated then the first instance of <see cref="IEmitter"/> in the configuration shall be returned.</para>
        /// </summary>
        /// <exception cref="SuperScriptException">Thrown if no instances of <see cref="IEmitter"/> have been configured.</exception>
        public IEmitter DefaultEmitter
        {
            get
            {
                // find which is the default Emitter
                return Emitters.FirstOrDefault(e => e.IsDefault) ?? Emitters.FirstOrDefault();
            }
        }


	    /// <summary>
	    /// Gets or sets a collection containing implementations of <see cref="IEmitter"/>.
	    /// </summary>
	    public List<IEmitter> Emitters { get; set; }

        #endregion


        #region Singleton stuff

        private static readonly Settings ThisInstance = new Settings();

        // Explicit static constructor to tell C# compiler not to mark type as beforefieldinit
        static Settings()
        {
        }

        #endregion


        /// <summary>
        /// This constructor contains the logic for parsing the configured values out of the web.config.
        /// </summary>
        /// <exception cref="EmitterConfigurationException">Thrown if multiple &lt;emitter&gt; elements have isDefault=true.</exception>
        /// <exception cref="EmitterConfigurationException">Thrown if multiple &lt;emitter&gt; elements have the same Key property.</exception>
        private Settings()
        {
	        EmitterBundles = new List<EmitterBundle>();
	        Emitters = new List<IEmitter>();

            var config = ConfigurationManager.GetSection("superScript") as SuperScriptConfig;
            if (config == null)
            {
                return;
            }

            // instantiate the emitters
            Emitters.AddRange(config.Emitters.ToEmitterCollection());

			// instantiate any emitter bundles
	        EmitterBundles.AddRange(config.EmitterBundles.ToEmitterBundles());
        }


        /// <summary>
        /// Check the web.config file for configured default declarations.
        /// </summary>
        public static Settings Instance
        {
            get { return ThisInstance; }
        }
    }
}