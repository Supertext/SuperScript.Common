using System.Collections.Generic;
using SuperScript.Emitters;
using SuperScript.Modifiers.Converters;
using SuperScript.Modifiers.Post;

namespace SuperScript.Modifiers.Pre
{
    /// <summary>
    /// <para>Modifies the <see cref="IEnumerable{DeclarationBase}"/>.</para>
    /// <para>Implementations of this abstract class are processed prior to the <see cref="IEnumerable{DeclarationBase}"/> being converted using an <see cref="CollectionConverter"/>.</para>
    /// </summary>
    public abstract class CollectionPreModifier : ModifierBase, IUseWhenBundled
    {
        /// <summary>
        /// Executes this instance of <see cref="CollectionConverter"/> upon the specified <see cref="PreModifierArgs"/>.
        /// </summary>
        /// <param name="args">An instance of <see cref="PreModifierArgs"/> which contains the data for this modifier.</param>
        /// <returns>An instance of <see cref="PreModifierArgs"/> which is expected to be a modified version of the input argument.</returns>
		public abstract PreModifierArgs Process(PreModifierArgs args);


		/// <summary>
		/// Gets or sets whether this <see cref="CollectionPostModifier"/> should be implemented when its parent <see cref="IEmitter"/> is referenced by a <see cref="EmitterBundle"/>.
		/// </summary>
		public bool UseWhenBundled { get; set; }
    }
}