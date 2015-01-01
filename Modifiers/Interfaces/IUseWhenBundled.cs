using SuperScript.Emitters;

namespace SuperScript.Modifiers
{
	public interface IUseWhenBundled
	{
		/// <summary>
		/// Gets or sets whether this <see cref="ModifierBase"/> should be implemented when its parent <see cref="IEmitter"/> is referenced by a <see cref="EmitterBundle"/>.
		/// </summary>
		bool UseWhenBundled { get; set; }
	}
}