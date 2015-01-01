using SuperScript.Emitters;
using SuperScript.Modifiers.Converters;

namespace SuperScript.Modifiers.Post
{
    /// <summary>
    /// <para>Modifies the <see cref="string"/> output of an <see cref="CollectionConverter"/> implementation.</para>
    /// <para>Implementations of this abstract class will be processed at the final stage before writing to the webpage.</para>
    /// </summary>
    public abstract class CollectionPostModifier : ModifierBase, IUseWhenBundled
    {
        /// <summary>
        /// Executes this instance of <see cref="CollectionPostModifier"/> upon the specified <see cref="PostModifierArgs"/>.
        /// </summary>
        /// <param name="args">An instance of <see cref="PostModifierArgs"/> which contains the data for this modifier.</param>
        /// <returns>An instance of <see cref="PostModifierArgs"/> which is expected to be a modified version of the input argument.</returns>
        public abstract PostModifierArgs Process(PostModifierArgs args);


		/// <summary>
		/// Gets or sets whether this <see cref="CollectionPostModifier"/> should be implemented when its parent <see cref="IEmitter"/> is referenced by a <see cref="EmitterBundle"/>.
		/// </summary>
		public bool UseWhenBundled { get; set; }
    }
}