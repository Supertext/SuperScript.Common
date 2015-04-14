using SuperScript.Declarables;
using SuperScript.Modifiers.Pre;
using System;
using System.Collections.Generic;

namespace SuperScript.Modifiers
{
    /// <summary>
    /// Contains the objects, collections, etc., which will be passed to all instances of <see cref="CollectionPreModifier"/>.
    /// </summary>
    public class PreModifierArgs
    {
        #region Properties

        private readonly Lazy<bool> _isDebug = new Lazy<bool>(Configuration.Settings.IsDebuggingEnabled);


        /// <summary>
        /// This property allows the user to pass any required object through the conversion process.
        /// </summary>
        public object CustomObject { get; set; }


        /// A collection of <see cref="DeclarationBase"/> objects to be processed.
        public IEnumerable<DeclarationBase> Declarations { get; set; }


        /// <summary>
        /// Gets a value indicating whether the current HTTP request is in debug mode.
        /// </summary>
        public bool IsDebug
        {
            get { return _isDebug.Value; }
        }

        #endregion


        #region Constructors

        /// <summary>
        /// Default constructor for <see cref="PreModifierArgs"/>.
        /// </summary>
        public PreModifierArgs()
        {
        }


        /// <summary>
        /// Constuctor for <see cref="PreModifierArgs"/> which allows the <see cref="Declarations"/> collection to be set.
        /// </summary>
        /// <param name="declarations">Value to be passed to the <see cref="Declarations"/> property.</param>
        public PreModifierArgs(IEnumerable<DeclarationBase> declarations)
        {
            Declarations = declarations;
        }


        /// <summary>
        /// Constuctor for <see cref="PreModifierArgs"/> which allows the <see cref="CustomObject"/> property to be set.
        /// </summary>
        /// <param name="customObject">Value to be passed to the <see cref="CustomObject"/> property.</param>
        /// <remarks>
        /// <para>If the <see cref="customObject"/> is an <see cref="IEnumerable{DeclarationBase}"/> then this constructor should not be used.</para>
        /// <para>Passing an <see cref="IEnumerable{DeclarationBase}"/> to this constructor will result in the wrong property being assigned.</para>
        /// </remarks>
        public PreModifierArgs(object customObject)
        {
            CustomObject = customObject;
        }


        /// <summary>
        /// Constuctor for <see cref="PreModifierArgs"/> which allows both the <see cref="Declarations"/> and <see cref="CustomObject"/> properties to be set.
        /// </summary>
        /// <param name="declarations">Value to be passed to the <see cref="Declarations"/> property.</param>
        /// <param name="customObject">Value to be passed to the <see cref="CustomObject"/> property.</param>
        public PreModifierArgs(IEnumerable<DeclarationBase> declarations, object customObject)
        {
            CustomObject = customObject;
            Declarations = declarations;
        }

        #endregion
    }
}