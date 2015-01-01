using System.Collections.Generic;
using SuperScript.Declarables;
using SuperScript.Modifiers.Post;
using SuperScript.Modifiers.Pre;

namespace SuperScript.Modifiers.Converters
{
    /// <summary>
    /// <para>Converts a <see cref="IEnumerable{DeclarationBase}"/> into a <see cref="string"/>.</para>
    /// <para>An implementation of this class will be processed after any implementations of the abstract class <see cref="CollectionPreModifier"/>
    /// and before any implementations of the abstract class <see cref="CollectionPostModifier"/>.</para>
    /// </summary>
    public abstract class CollectionConverter : ModifierBase
    {
        /// <summary>
        /// Executes this instance of <see cref="CollectionConverter"/> upon the collection of <see cref="DeclarationBase"/> objects.
        /// </summary>
        /// <param name="args">An instance of <see cref="PreModifierArgs"/> which contains the data for this <see cref="CollectionConverter"/>.</param>
        /// <returns>
        /// An instance of <see cref="PostModifierArgs"/> where the 'Emitted' property is the converted version of the input 'Declarations' property.
        /// </returns>
		public abstract PostModifierArgs Process(PreModifierArgs args);
    }
}