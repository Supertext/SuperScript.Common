using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using SuperScript.Declarables;
using SuperScript.Emitters;
using SuperScript.ExtensionMethods;

namespace SuperScript
{
    /// <summary>
    /// <para>This static class offers the entire site a centralised location for adding instances of <see cref="DeclarationBase"/>.</para>
    /// <para>This collection should then be emitted in a convenient location.</para>
    /// <para>The developer can select whether to emit specific declarations in one location and the rest of the 
    /// collection in a different location, or simply emit them all together in a single location.</para>
    /// </summary>
    public static class Declarations
    {
        #region Properties

        /// <summary>
        /// Contains the collection of all JavaScript variables across this HTTP request.
        /// </summary>
		public static IList<DeclarationBase> Collection { get; set; }


	    /// <summary>
	    /// Contains the instances of <see cref="EmitterBundle"/> which will be used to emit <see cref="DeclarationBase"/> instances.
	    /// </summary>
	    public static IEnumerable<EmitterBundle> EmitterBundles = new List<EmitterBundle>();


        /// <summary>
        /// Contains the instances of <see cref="IEmitter"/> which will be used to emit the <see cref="DeclarationBase"/> instances.
        /// </summary>
        public static IEnumerable<IEmitter> Emitters = new List<IEmitter>();
		
        #endregion


        #region Methods


        #region Adding declarations


        /// <summary>
        /// By deriving a custom Declaration, any type of declarations can be added to the collection.
        /// </summary>
        /// <param name="declaration">A custom implementation of the <see cref="DeclarationBase"/> abstract class.</param>
        /// <param name="insertAt">Permits the instance of <see cref="DeclarationBase"/> to be inserted into the collection at a specified index.</param>
        /// <exception cref="CollectionNotInstantiatedException">The Collection (<see cref="ICollection{DeclarationBase}"/>) property has not been instantiated This should be done for each HTTP request.</exception>
        public static T AddDeclaration<T>(DeclarationBase declaration, int? insertAt = null) where T : DeclarationBase
        {
            if (Collection == null)
            {
                throw new CollectionNotInstantiatedException();
            }
            // if this variable has already been declared then remove it from the collection
            Collection.RemoveDuplicates(declaration);

            if (insertAt.HasValue)
            {
                Collection.Insert(insertAt.Value, declaration);
            }
            else
            {
                Collection.Add(declaration);
            }

            return (T) declaration;
        }


	    /// <summary>
	    /// By deriving a custom Declaration, any type of declarations can be added to the collection.
	    /// </summary>
	    /// <param name="declarations">A collection of custom implementations of the <see cref="DeclarationBase"/> abstract class.</param>
	    /// <exception cref="CollectionNotInstantiatedException">The Collection (<see cref="ICollection{DeclarationBase}"/>) property has not been instantiated This should be done for each HTTP request.</exception>
	    public static IEnumerable<T> AddDeclarations<T>(IEnumerable<T> declarations) where T : DeclarationBase
	    {
		    if (Collection == null)
		    {
			    throw new CollectionNotInstantiatedException();
		    }

		    foreach (var declaration in declarations)
		    {
			    // if this variable has already been declared then remove it from the collection
			    Collection.RemoveDuplicates(declaration);

			    Collection.Add(declaration);

			    yield return declaration;
		    }
	    }


	    #endregion


        #region Removing declarations


        /// <summary>
        /// Removes all declarations, web.config defaults being optional, from the current HTTP request.
        /// </summary>
        /// <param name="removeDefaults">
        /// Determines whether the configured default declarations (if any exist) are to remain (the default action) or whether <i>all</i> declarations should be removed.
        /// </param>
        /// <exception cref="CollectionNotInstantiatedException">The Collection (<see cref="ICollection{DeclarationBase}"/>) property has not been instantiated This should be done for each HTTP request.</exception>
        public static void Reset(bool removeDefaults = false)
        {
            if (Collection == null)
            {
                throw new CollectionNotInstantiatedException();
            }

            Collection.Clear();
        }


        /// <summary>
        /// Removes the specified declared variable or function call.
        /// </summary>
        /// <param name="nameOrCall">
        /// <para>The name of the previously-declared variable or function call.</para>
        /// <para>If specifying a function call, there is no need to include parentheses and parameters.</para>
        /// </param>
        /// <exception cref="CollectionNotInstantiatedException">The Collection (<see cref="ICollection{DeclarationBase}"/>) property has not been instantiated This should be done for each HTTP request.</exception>
        public static void Remove(string nameOrCall)
        {
            if (Collection == null)
            {
                throw new CollectionNotInstantiatedException();
            }

            Remove(Collection.FirstOrDefault(d => d.Name == nameOrCall));
        }


        /// <summary>
        /// Removes the specified declaration.
        /// </summary>
        /// <param name="declaration">The instance of the previously-declared variable or function call.</param>
        /// <exception cref="CollectionNotInstantiatedException">The Collection (<see cref="ICollection{DeclarationBase}"/>) property has not been instantiated This should be done for each HTTP request.</exception>
        public static void Remove(DeclarationBase declaration)
        {
            if (Collection == null)
            {
                throw new CollectionNotInstantiatedException();
            }

            if (declaration != null)
            {
                Collection.Remove(declaration);
            }
        }


        #endregion


        #region Emitting declarations


	    /// <summary>
	    /// Emits the <see cref="DeclarationBase"/> instances.
	    /// </summary>
	    /// <param name="declarationNames">
	    /// <para>If one or more (comma-separated) declarations are specified then only these declarations will be written to the</para>
	    /// <para>page at the current location (i.e. at the location where this method was called from).</para>
	    /// <para>Furthermore, these specified declarations will not be written out at a later time if and when this method is</para>
	    /// <para>called without any declarations being specified.</para>
	    /// </param>
	    /// <returns>
	    /// The formatted output for each instance of <see cref="DeclarationBase"/>.
	    /// </returns>
	    /// <exception cref="CollectionNotInstantiatedException">The <see cref="Collection"/> property has not been instantiated This should be done for each HTTP request.</exception>
	    /// <exception cref="NoEmittersConfiguredException">No implementation of <see cref="IEmitter"/> has been configured.</exception>
	    public static IHtmlString Emit(params string[] declarationNames)
	    {
		    if (Collection == null)
		    {
			    throw new CollectionNotInstantiatedException();
		    }

		    if (Emitters == null || !Emitters.Any())
		    {
			    throw new NoEmittersConfiguredException();
		    }

		    // if specific variables have been requested for rendering then...
		    if (declarationNames == null || declarationNames.Length == 0)
			{
				throw new NotSpecifiedException("One or more declaration names must be specified to be emitted.");
		    }

			// retrieve the specified declarations from the static collection of declarations
		    var specDecs = Collection.Where(dec => declarationNames.Contains(dec.Name)).ToArray();

		    // and then remove them from the Collection so that they won't be rendered again (should the Emitter be emitted)
		    foreach (var specDec in specDecs)
		    {
			    Collection.Remove(specDec);
		    }

		    // format them and return the result
		    return Emit(specDecs);
	    }


	    /// <summary>
	    /// Emits all instances of <see cref="DeclarationBase"/> whose <see cref="IEmitter.Key"/> has been specified.
	    /// </summary>
	    /// <param name="emitterKeys">
	    /// A comma-separated list (or array) of <see cref="IEmitter.Key"/> for the desired instances of <see cref="IEmitter"/>.</param>
	    /// <returns>
	    /// The formatted output for each instance of <see cref="DeclarationBase"/> which references the specified instance of <see cref="IEmitter.Key"/>.
	    /// </returns>
	    /// <exception cref="CollectionNotInstantiatedException">The <see cref="Collection"/> property has not been instantiated This should be done for each HTTP request.</exception>
	    /// <exception cref="NoEmittersConfiguredException">No implementation of <see cref="IEmitter"/> has been configured.</exception>
	    public static IHtmlString EmitFor(params string[] emitterKeys)
	    {
		    if (Collection == null)
		    {
			    throw new CollectionNotInstantiatedException();
		    }

		    if (Emitters == null || !Emitters.Any())
		    {
			    throw new NoEmittersConfiguredException();
		    }

		    if (emitterKeys == null || emitterKeys.Length == 0)
		    {
			    throw new NotSpecifiedException("A key must be specified for the desired Emitter.");
		    }

		    if (emitterKeys.Length == 1)
		    {
			    return Emit(emitterKeys[0]);
		    }

		    var output = new StringBuilder();

		    foreach (var key in emitterKeys)
		    {
			    output.AppendLine(Emit(key).ToHtmlString());
		    }

		    return new HtmlString(output.ToString());
	    }


	    /// <summary>
	    /// Returns the emitted output of all registered instances of <see cref="IEmitter"/>.
	    /// </summary>
	    /// <returns>
	    /// The formatted output for each instance of <see cref="DeclarationBase"/>.
	    /// </returns>
	    /// <remarks>
	    /// <para>This method will first of all consider those instances of <see cref="IEmitter"/> which have not been referenced on a <see cref="EmitterBundle"/>.</para>
	    /// <para>Once these stand-alone instances of <see cref="IEmitter"/> have been omitted then the registered instances of <see cref="EmitterBundle"/> will be considered.</para>
	    /// </remarks>
	    /// <exception cref="CollectionNotInstantiatedException">The <see cref="Collection"/> property has not been instantiated This should be done for each HTTP request.</exception>
	    /// <exception cref="NoEmittersConfiguredException">No implementation of <see cref="IEmitter"/> has been configured.</exception>
	    public static IHtmlString EmitForAll()
	    {
		    if (Collection == null)
		    {
			    throw new CollectionNotInstantiatedException();
		    }

		    if (Emitters == null || !Emitters.Any())
		    {
			    throw new NoEmittersConfiguredException();
		    }

		    var output = new StringBuilder();

		    // retrieve the keys for those emitters which don't belong to a BundledEmitter
		    var allEmitterKeys = Emitters.Select(e => e.Key).ToList();
		    var allBundledEmitterKeys = new List<string>();
		    foreach (var bundledEmitter in EmitterBundles)
		    {
			    allBundledEmitterKeys.AddRange(bundledEmitter.EmitterKeys);
		    }

		    foreach (var emitterKey in allEmitterKeys.Except(allBundledEmitterKeys))
		    {
			    output.AppendLine(EmitFor(emitterKey).ToHtmlString());
		    }

		    // now emit each bundled emitter
		    foreach (var bundledEmitter in EmitterBundles)
		    {
			    // get a collection of declarations for all emitters in this bundle
			    var bundledDecs = Collection.ForEmitter(bundledEmitter.EmitterKeys.ToArray());

			    output.AppendLine(bundledEmitter.ToHtmlString(bundledDecs).ToHtmlString());
		    }

		    return new HtmlString(output.ToString());
	    }


	    /// <summary>
        /// Emits the specified <see cref="DeclarationBase"/> instances.
        /// </summary>
        private static IHtmlString Emit(IEnumerable<DeclarationBase> declarations)
        {
            var output = new StringBuilder();

            // to prevent multiple enumerations
            var decs = declarations as DeclarationBase[] ?? declarations.ToArray();

            // group the declarations which have a specific Emitter and emit from each Emitter
            foreach (var emitterGroup in decs.GroupByEmitter())
            {
                // get the Emitter
                var emitter = Emitters.FirstOrDefault(e => e.Key == emitterGroup.Key);
                if (emitter == null)
                {
                    continue;
                }

                // move the CollectedScript declarations to the end of the collection so that they're emitted after variables, arrays, enums, etc.
                output.Append(emitter.ToHtmlString(emitterGroup));
            }

            // format them and return the result
            return new HtmlString(output.ToString());
        }


	    private static IHtmlString Emit(string emitterKey)
		{
			var emitter = Emitters.FirstOrDefault(e => e.Key == emitterKey);
			if (emitter != null)
			{
				return emitter.ToHtmlString(Collection.ForEmitter(emitterKey));
			}

			// if not found in the Emitters collection then try the BundledEmitters collection
			var bundledEmitter = EmitterBundles.FirstOrDefault(ce => ce.Key == emitterKey);
		    if (bundledEmitter != null)
		    {
			    // get a collection of declarations for all emitters in this bundle
			    var bundledDecs = Collection.ForEmitter(bundledEmitter.EmitterKeys.ToArray());

			    return bundledEmitter.ToHtmlString(bundledDecs);
		    }

		    return null;
		}


        #endregion


        #endregion
    }
}