using System;
using System.Web;
using SuperScript.Modifiers.Converters;
using SuperScript.Modifiers.Post;

namespace SuperScript.Modifiers
{
    /// <summary>
    /// Contains the objects, collections, etc., which will be passed to all instances of <see cref="CollectionPostModifier"/>.
    /// </summary>
    public class PostModifierArgs
    {
        #region Properties

        private Lazy<bool> _isDebug = new Lazy<bool>(HttpContext.Current.IsDebuggingEnabled);


        /// <summary>
        /// This property allows the user to pass any required object through the conversion process.
        /// </summary>
        public object CustomObject { get; set; }


        /// Expected to be the output of an <see cref="CollectionConverter"/> or a previous instance of <see cref="PostModifierArgs"/>.
        public string Emitted { get; set; }


        /// <summary>
        /// Gets a value indicating whether the current HTTP request is in debug mode.
        /// </summary
        public bool IsDebug
        {
            get { return _isDebug.Value; }
        }

        #endregion


        #region Constructors

        /// <summary>
        /// Default constructor for <see cref="PostModifierArgs"/>.
        /// </summary>
        public PostModifierArgs()
        {
        }


        /// <summary>
        /// Constuctor for <see cref="PostModifierArgs"/> which allows the <see cref="Emitted"/> property to be set.
        /// </summary>
        /// <param name="emitted">Value to be passed to the <see cref="Emitted"/> property.</param>
        public PostModifierArgs(string emitted)
        {
            Emitted = emitted;
        }


        /// <summary>
        /// Constuctor for <see cref="PostModifierArgs"/> which allows the <see cref="CustomObject"/> property to be set.
        /// </summary>
        /// <param name="customObject">Value to be passed to the <see cref="CustomObject"/> property.</param>
        /// <remarks>
        /// <para>If the <see cref="customObject"/> is a <see cref="string"/> then this constructor should not be used.</para>
        /// <para>Passing a <see cref="string"/> to this constructor will result in the wrong property being assigned.</para>
        /// </remarks>
        public PostModifierArgs(object customObject)
        {
            CustomObject = customObject;
        }


        /// <summary>
        /// Constuctor for <see cref="PostModifierArgs"/> which allows both the <see cref="Emitted"/> and <see cref="CustomObject"/> properties to be set.
        /// </summary>
        /// <param name="emitted">Value to be passed to the <see cref="Emitted"/> property.</param>
        /// <param name="customObject">Value to be passed to the <see cref="CustomObject"/> property.</param>
        public PostModifierArgs(string emitted, object customObject)
        {
            CustomObject = customObject;
            Emitted = emitted;
        }

        #endregion
    }
}