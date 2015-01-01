using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using SuperScript.Declarables;
using SuperScript.Emitters;

namespace SuperScript.ExtensionMethods
{
    public static class CollectionExtensions
    {
        /// <summary>
        /// Returns a collection of <see cref="DeclarationBase"/> which reference a specific instance of <see cref="IEmitter"/>.
        /// </summary>
        /// <param name="declarations">
        /// A collection of <see cref="DeclarationBase"/> instances which is to be filtered by Emitter.
        /// </param>
        /// <param name="emitterKeys">
        /// A comma-separated list of <see cref="IEmitter.Key"/> for the desired instances of <see cref="IEmitter"/>.
        /// </param>
        /// <returns>
        /// A collection of <see cref="DeclarationBase"/> instances, each using the desired Emitter.
        /// </returns>
        public static IEnumerable<DeclarationBase> ForEmitter(this IEnumerable<DeclarationBase> declarations, params string[] emitterKeys)
        {
            var decs = declarations as DeclarationBase[] ?? declarations.ToArray();
            if (declarations == null || !decs.Any())
            {
                yield break;
            }

	        foreach (var dec in emitterKeys.SelectMany(emitterKey => decs.Where(dec => dec.EmitterKey == emitterKey)))
	        {
		        yield return dec;
	        }
        }


	    /// <summary>
	    /// Groups collections of <see cref="DeclarationBase"/> by their specified <see cref="IEmitter"/>.
	    /// </summary>
	    /// <remarks>
	    /// Only instances of <see cref="DeclarationBase"/> which have a specified (i.e., non-default) Emitter specified will be considered.
	    /// </remarks>
	    /// <param name="declarations">
	    /// A collection of <see cref="DeclarationBase"/> instances, each of which may have a specified Emitter.
	    /// </param>
	    /// <returns>
	    /// A collection of groups of <see cref="DeclarationBase"/> instances, each with a shared Emitter.
	    /// </returns>
	    public static IEnumerable<IGrouping<string, DeclarationBase>> GroupByEmitter(this IEnumerable<DeclarationBase> declarations)
	    {
		    var decs = declarations as DeclarationBase[] ?? declarations.ToArray();
		    if (declarations == null || !decs.Any())
		    {
			    return new EmptyGroup<string, DeclarationBase>[0];
		    }

		    return decs.GroupBy(d => d.EmitterKey);
	    }


	    /// <summary>
        /// <para>Removes an instance of <see cref="DeclarationBase"/> from the specified collection.</para>
        /// <para>Any instances of <see cref="DeclarationBase"/> with a matching name will be removed.</para>
        /// </summary>
        /// <param name="collection">
        /// The instance of <see cref="Collection&lt;DeclarationBase&gt;"/> which is storing the declarations.
        /// </param>
        /// <param name="declaration">
        /// <para>An instance of <see cref="DeclarationBase"/>.</para>
        /// <para>This <see cref="DeclarationBase"/> instance will replace any existing declaration instances with a matching Name property,</para>
        /// <para>provided that the Name property it not null or whitespace.</para>
        /// </param>
        public static void RemoveDuplicates(this ICollection<DeclarationBase> collection, DeclarationBase declaration)
        {
            // if this declaration has already been added to the collection then remove it

            if (string.IsNullOrWhiteSpace(declaration.Name))
            {
                return;
            }

            foreach (var existingDecl in collection.Where(d => d.Name == declaration.Name).ToArray())
            {
                collection.Remove(existingDecl);
            }
        }


        /// <summary>
        /// <para>Removes an instance of <see cref="DeclarationBase"/> from the specified collection.</para>
        /// <para>Any instances of <see cref="DeclarationBase"/> with a matching name will be removed.</para>
        /// </summary>
        /// <param name="collection">
        /// The instance of <see cref="Collection&lt;DeclarationBase&gt;"/> which is storing the declarations.
        /// </param>
        /// <param name="name">
        /// <para>Any currently-declared instances of <see cref="DeclarationBase"/> which have a matching Name property</para>
        /// <para>will be removed, provided the Name property is neither null nor empty.</para>
        /// </param>
        public static void RemoveDuplicates(this ICollection<DeclarationBase> collection, string name)
        {
            // if this variable has already been declared then remove it from the collection

            if (string.IsNullOrEmpty(name)) return;

            foreach (var existingDecl in collection.Where(d => d.Name == name).ToArray())
            {
                collection.Remove(existingDecl);
            }
        }


        public class EmptyGroup<TKey, TValue> : IGrouping<TKey, TValue>
        {
            public TKey Key { get; set; }

            public IEnumerator<TValue> GetEnumerator()
            {
                return Enumerable.Empty<TValue>().GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
    }
}