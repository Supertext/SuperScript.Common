using SuperScript.Declarables;

namespace SuperScript
{
    /// <summary>
    /// Base class for SuperScript declaration options.
    /// </summary>
    public abstract class OptionsBase<T>
    {
        protected internal string _emitterKey;
        protected internal int? _insertAt;


        /// <summary>
        /// Specifies which instance of <see cref="SuperScript.Emitters.IEmitter"/> should be used to emit this instance of <see cref="DeclarationBase"/>.
        /// </summary>
        public abstract T EmitterKey(string value);


        /// <summary>
        /// Permits this instance of <see cref="DeclarationBase"/> to be inserted into the collection at the specified index.
        /// </summary>
        public abstract T InsertAt(int? value);
    }
}