using System;
using System.Collections.ObjectModel;
using System.Web;
using SuperScript.Configuration;
using SuperScript.Declarables;

namespace SuperScript
{
    /// <summary>
    /// This class is instantiated for each HTTP request. [This is wired-up in the web.config.]
    /// </summary>
    public class HttpInitialiser : IHttpModule
    {
        private bool _initialised;
        private readonly object _obj = new object();


        #region IHttpModule Members

        public void Dispose()
        {
            //clean-up code here.
        }

        #endregion


        public void Init(HttpApplication context)
        {
            if (_initialised) return;

            lock (_obj)
            {
                if (_initialised) return;

                InitEvents(context);
                _initialised = true;
            }
        }


        /// <summary>
        /// Custom handler for wiring-up events.
        /// </summary>
        /// <param name="context">
        /// The current HttpContext.
        /// </param>
        public void InitEvents(HttpApplication context)
        {
            context.BeginRequest += OnBeginRequest;
        }


        public virtual void OnBeginRequest(object s, EventArgs e)
        {
            // Initialise the collection of Declarations for this request
            Declarations.Collection = new Collection<DeclarationBase>();

			// Set all instances of BundledEmitter
	        Declarations.EmitterBundles = Settings.Instance.EmitterBundles;

            // Set all instances of IEmitter
            Declarations.Emitters = Settings.Instance.Emitters;
        }
    }
}