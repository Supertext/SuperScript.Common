using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SuperScript.Declarables;
using SuperScript.ExtensionMethods;
using SuperScript.Modifiers;
using SuperScript.Modifiers.Converters;
using SuperScript.Modifiers.Post;
using SuperScript.Modifiers.Pre;
using SuperScript.Modifiers.Writers;

namespace SuperScript.Emitters
{
    /// <summary>
    /// The default implementation of <see cref="IEmitter"/>.
    /// </summary>
    public class StandardEmitter : IEmitter
    {
        #region Public Properties

        /// <summary>
        /// This property allows the user to pass any required object through the modification and conversion process.
        /// </summary>
        public object CustomObject { get; set; }


        /// <summary>
        /// Indicates whether this is the default instance of <see cref="IEmitter"/>.
        /// </summary>
        /// <remarks>
        /// The default instance of <see cref="IEmitter"/> is used for situations where no specific instance of <see cref="IEmitter"/> has been specified.
        /// </remarks>
        public bool IsDefault { get; set; }


        /// <summary>
        /// The identifier for this instance of <see cref="IEmitter"/>.
        /// </summary>
        public string Key { get; set; }


        // Collection process extenders

        /// <summary>
        /// The implementation of <see cref="CollectionConverter"/> which has been assigned to this <see cref="IEmitter"/>.
        /// </summary>
        public CollectionConverter Converter { get; set; }


        /// <summary>
        /// The implementations of <see cref="CollectionPreModifier"/> which have been assigned to this <see cref="IEmitter"/>.
        /// </summary>
        public ICollection<CollectionPreModifier> PreModifiers { get; set; }


        /// <summary>
        /// The implementations of <see cref="CollectionPostModifier"/> which have been assigned to this <see cref="IEmitter"/>.
        /// </summary>
        public ICollection<CollectionPostModifier> PostModifiers { get; set; }


        /// <summary>
        /// The implementation of <see cref="HtmlWriter"/> which has been assigned to this <see cref="IEmitter"/> for writing the formatted output to the webpage.
        /// </summary>
        public HtmlWriter HtmlWriter { get; set; }

        #endregion


        #region Methods

	    /// <summary>
	    /// Converts the specified collection of <see cref="DeclarationBase"/> instances into a string.
	    /// </summary>
	    /// <param name="declarations">A collection of JavaScript declarations.</param>
	    /// <returns>
	    /// An <see cref="HtmlString"/> containing the contents of the specified collection of <see cref="DeclarationBase"/>.
	    /// </returns>
	    /// <exception cref="NotSpecifiedException">Thrown if either the <see cref="Converter"/> or the <see cref="HtmlWriter"/> properties are null.</exception>
	    public virtual IHtmlString ToHtmlString(IEnumerable<DeclarationBase> declarations)
	    {
		    if (Converter == null)
		    {
			    throw new NotSpecifiedException("No implementation of ICollectionConverter has been specified. This collection cannot be processed.");
		    }

		    if (HtmlWriter == null)
		    {
			    throw new NotSpecifiedException("No implementation of HtmlWriter has been specified. This collection cannot be processed.");
		    }

		    // to prevent multiple enumerations
		    var arrDecs = declarations as DeclarationBase[] ?? declarations.ToArray();


		    // process all extenders

		    var preArgs = new PreModifierArgs(arrDecs, CustomObject);

		    preArgs = PreModifiers.Process(preArgs);

		    var postArgs = Converter.Process(preArgs);

		    postArgs = PostModifiers.Process(postArgs);

			// If there is an instance of HtmlWriter then use it, otherwise return the emitted output from the PostModifiersCollection.
			// - an HtmlWriter is not required because an emitter can be part of a BundledEmitter which will have its own HtmlWriter.
			return HtmlWriter != null
			           ? HtmlWriter.Process(postArgs)
			           : new HtmlString(postArgs.Emitted);
	    }


	    /// <summary>
	    /// Returns a string that represents the current object, and should be used for debugging purposes only.
	    /// </summary>
	    /// <returns>
	    /// A string containing information about this instance of <see cref="IEmitter"/>.
	    /// </returns>
	    public override string ToString()
	    {
		    // retrieve all declarations which reference this emitter
		    var decCount = Declarations.Collection.Count(dec => dec.EmitterKey == Key);

		    return String.Concat("Key: ", Key, "\nDeclaration count: ", decCount);
	    }

	    #endregion
    }
}