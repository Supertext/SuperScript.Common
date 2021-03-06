﻿using SuperScript.Emitters;
using SuperScript.ExtensionMethods;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

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
        /// <exception cref="CollectionNotInstantiatedException">Thrown if the <see cref="Emitters"/> collection property has not been instantiated. This should be done for each HTTP request.</exception>
        /// <exception cref="NoEmittersConfiguredException">Thrown if no instances of <see cref="IEmitter"/> have been configured.</exception>
        public IEmitter DefaultEmitter
        {
            get
            {
                if (Emitters == null)
                {
                    throw new CollectionNotInstantiatedException("The SuperScript.Configuration.Settings.Emitters collection property has not been instantiated. This should be done for each HTTP request.");
                }

                if (!Emitters.Any())
                {
                    throw new NoEmittersConfiguredException();
                }

                // find which is the default Emitter
                return Emitters.FirstOrDefault(e => e.IsDefault) ?? Emitters.FirstOrDefault();
            }
        }


	    /// <summary>
	    /// Gets or sets a collection containing implementations of <see cref="IEmitter"/>.
	    /// </summary>
	    public List<IEmitter> Emitters { get; set; }


        /// <summary>
        /// Gets whether <c>debug</c> is set to <c>true</c> or <c>false</c> in the config file.
        /// </summary>
        private static bool? _isDebuggingEnabled;
        public static bool IsDebuggingEnabled
        {
            get
            {
                if (_isDebuggingEnabled.HasValue)
                {
                    return _isDebuggingEnabled.Value;
                }

                var compilationSection = ConfigurationManager.GetSection(@"system.web/compilation") as System.Web.Configuration.CompilationSection;
                _isDebuggingEnabled = compilationSection == null || compilationSection.Debug;

                return _isDebuggingEnabled.Value;
            }
        }

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