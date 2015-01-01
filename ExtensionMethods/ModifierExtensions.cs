using System.Collections.Generic;
using System.Linq;
using SuperScript.Modifiers;
using SuperScript.Modifiers.Post;
using SuperScript.Modifiers.Pre;

namespace SuperScript.ExtensionMethods
{
    public static class ModifierExtensions
    {
        /// <summary>
        /// Processes all <see cref="CollectionPreModifier"/> objects in the specified <see cref="modifiers"/> collection.
        /// </summary>
        /// <param name="modifiers">
        /// An <see cref="ICollection{T}"/> of <see cref="CollectionPreModifier"/> objects which will be processed consecutively
        /// using the specified <see cref="PreModifierArgs"/>.
        /// </param>
        /// <param name="args">The specified instance will be modified by each <see cref="CollectionPreModifier"/>.</param>
        /// <returns>A modified version of the specified <see cref="args"/> parameter.</returns>
        public static PreModifierArgs Process(this IEnumerable<CollectionPreModifier> modifiers, PreModifierArgs args)
        {
            return modifiers == null
                ? args
                : modifiers.Aggregate(args, (current, modifier) => modifier.Process(current));
        }


        /// <summary>
        /// Processes all <see cref="CollectionPostModifier"/> objects in the specified <see cref="modifiers"/> collection.
        /// </summary>
        /// <param name="modifiers">
        /// An <see cref="ICollection{T}"/> of <see cref="CollectionPostModifier"/> objects which will be processed consecutively
        /// using the specified <see cref="PostModifierArgs"/>.
        /// </param>
        /// <param name="args">The specified instance will be modified by each <see cref="CollectionPostModifier"/>.</param>
        /// <returns>A modified version of the specified <see cref="args"/> parameter.</returns>
        public static PostModifierArgs Process(this IEnumerable<CollectionPostModifier> modifiers, PostModifierArgs args)
        {
            return modifiers == null
                ? args
                : modifiers.Aggregate(args, (current, modifier) => modifier.Process(current));
        }
    }
}