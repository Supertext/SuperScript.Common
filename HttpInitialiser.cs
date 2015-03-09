using SuperScript.Configuration;
using SuperScript.Declarables;
using System.Collections.ObjectModel;
using System.Threading;
using System.Web;


namespace SuperScript
{
    /// <summary>
    /// This class is instantiated for each HTTP request, and is used to instantiate the core collections required by SuperScript.
    /// </summary>
    public class HttpInitialiser : IHttpModule
    {
        private readonly object _initSync = new object();
        private static int _initialized;


        #region IHttpModule Members

        public void Dispose()
        {
            //clean-up code here.
        }

        #endregion


        public void Init(HttpApplication context)
        {
            // Prevent the initialization pipeline to be executed several times.
            if (Interlocked.CompareExchange(ref _initialized, 0, 0) == 0)
            {
                lock (_initSync)
                {
                    if (Interlocked.CompareExchange(ref _initialized, 0, 0) == 0)
                    {
                        OneTimeInitialization(context);
                        Interlocked.Exchange(ref _initialized, 1);
                    }
                }
            }
        }


        protected virtual void OneTimeInitialization(HttpApplication context)
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