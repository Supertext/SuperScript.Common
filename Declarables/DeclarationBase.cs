using SuperScript.Emitters;

namespace SuperScript.Declarables
{
    /// <summary>
    /// This class holds each of the declarations: JavaScript variable with or without value, function call, 
    /// or a JavaScript comment.
    /// </summary>
    public abstract class DeclarationBase
    {
        /// <summary>
        /// <para>Indicates which instance of IEmitter the content should be added to.</para>
        /// <para>If not specified then the contents will be added to the default implementation of <see cref="IEmitter"/>.</para>
        /// </summary>
        public string EmitterKey { get; set; }


        /// <summary>
        /// Gets or sets the name assigned to this declarable.
        /// </summary>
        public string Name { get; set; }


        /// <summary>
        /// Returns this instance as a formatted string.
        /// </summary>
        /// <returns></returns>
        public new abstract string ToString();
    }
}