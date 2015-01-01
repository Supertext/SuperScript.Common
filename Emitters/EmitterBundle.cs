using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using SuperScript.Configuration;
using SuperScript.Declarables;
using SuperScript.ExtensionMethods;
using SuperScript.Modifiers;
using SuperScript.Modifiers.Post;
using SuperScript.Modifiers.Writers;

namespace SuperScript.Emitters
{
    /// <summary>
    /// Allows a collection of emitters to have their emitted output bundled together.
    /// </summary>
    public class EmitterBundle
    {
        #region Public Properties

		/// <summary>
		/// 
		/// </summary>
		public IEnumerable<string> EmitterKeys { get; set; }


		/// <summary>
		/// Gets or sets a collection of instances of <see cref="IEmitter"/> which should have their emitted outputs bundled together.
		/// </summary>
	    public IEnumerable<IEmitter> Emitters
	    {
		    get { return Settings.Instance.Emitters.Where(e => EmitterKeys.Contains(e.Key)); }
	    }


	    /// <summary>
		/// This property allows the user to pass any required object through the modification and writing process.
		/// </summary>
		public object CustomObject { get; set; }


	    /// <summary>
        /// The identifier for this instance of <see cref="IEmitter"/>.
        /// </summary>
        public string Key { get; set; }


        // Collection process extenders

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
        /// </summary>
        /// <returns></returns>
		public virtual IHtmlString ToHtmlString(IEnumerable<DeclarationBase> declarations)
        {
            if (HtmlWriter == null)
            {
                throw new NotSpecifiedException("No implementation of HtmlWriter has been specified. This collection cannot be processed.");
            }

	        if (Emitters == null || !Emitters.Any())
	        {
		        return new HtmlString(String.Empty);
	        }

			var decs = declarations as DeclarationBase[] ?? declarations.ToArray();


			// build a string buffer containing the bundled emitted output from all specified emitters

	        var emitted = new StringBuilder();
	        foreach (var emitter in Emitters)
	        {
		        // get the declarations for this emitter
		        var emitterDecs = decs.ForEmitter(emitter.Key);

		        var preArgs = new PreModifierArgs(emitterDecs, CustomObject);

		        if (emitter.PreModifiers != null)
		        {
			        preArgs = emitter.PreModifiers.Where(m => m.UseWhenBundled).Process(preArgs);
		        }

		        var postArgs = emitter.Converter.Process(preArgs);

		        if (emitter.PostModifiers != null)
		        {
			        postArgs = emitter.PostModifiers.Where(m => m.UseWhenBundled).Process(postArgs);
		        }

		        emitted.AppendLine(postArgs.Emitted);
	        }


	        // now process this bundled emitted output

	        var bundledPostArgs = new PostModifierArgs(emitted.ToString(), CustomObject);

			bundledPostArgs = PostModifiers.Process(bundledPostArgs);

			return HtmlWriter.Process(bundledPostArgs);
        }

        #endregion


		#region Constructors

		/// <summary>
		/// Default constructor for <see cref="EmitterBundle"/>.
		/// </summary>
		public EmitterBundle()
		{ }


		/// <summary>
		/// Constructor for <see cref="EmitterBundle"/> in which the <see cref="EmitterBundle.Key"/> property may be specified.
 		/// </summary>
		/// <param name="key">A value for the <see cref="EmitterBundle.Key"/> property</param>
	    public EmitterBundle(string key)
	    {
		    Key = key;
	    }

		#endregion
    }
}