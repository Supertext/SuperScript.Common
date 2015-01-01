using System.Collections.Generic;
using System.Web;
using SuperScript.Declarables;
using SuperScript.Modifiers.Converters;
using SuperScript.Modifiers.Post;
using SuperScript.Modifiers.Pre;
using SuperScript.Modifiers.Writers;

namespace SuperScript.Emitters
{
    /// <summary>
    /// Implementations of this interface can be used for emitting an instance of <see cref="DeclarationBase"/> onto the webpage.
    /// </summary>
    public interface IEmitter
    {
        /// <summary>
        /// This property allows the user to pass any required object through the modification and conversion process.
        /// </summary>
        object CustomObject { get; set; }


        /// <summary>
        /// Indicates whether this is the default instance of <see cref="IEmitter"/>.
        /// </summary>
        /// <remarks>
        /// The default instance of <see cref="IEmitter"/> is used for situations where no specific instance of <see cref="IEmitter"/> has been specified.
        /// </remarks>
        bool IsDefault { get; set; }


        /// <summary>
        /// The identifier for this instance of <see cref="IEmitter"/>.
        /// </summary>
		string Key { get; set; }


        /// <summary>
        /// Enumerates through the specified collection of <see cref="DeclarationBase"/> instances and formats each of them.
        /// </summary>
        /// <param name="declarations">
        /// A collection of <see cref="DeclarationBase"/> instances.
        /// </param>
        IHtmlString ToHtmlString(IEnumerable<DeclarationBase> declarations);


        // Collection process extenders

        /// <summary>
        /// The implementation of <see cref="CollectionConverter"/> which has been assigned to this <see cref="IEmitter"/>.
        /// </summary>
        CollectionConverter Converter { get; set; }


        /// <summary>
        /// The implementations of <see cref="CollectionPreModifier"/> which have been assigned to this <see cref="IEmitter"/>.
        /// </summary>
        ICollection<CollectionPreModifier> PreModifiers { get; set; }


        /// <summary>
        /// The implementations of <see cref="CollectionPostModifier"/> which have been assigned to this <see cref="IEmitter"/>.
        /// </summary>
        ICollection<CollectionPostModifier> PostModifiers { get; set; }


        /// <summary>
        /// The implementation of <see cref="HtmlWriter"/> which has been assigned to this <see cref="IEmitter"/> for writing the formatted output to the webpage.
        /// </summary>
        HtmlWriter HtmlWriter { get; set; }
    }
}