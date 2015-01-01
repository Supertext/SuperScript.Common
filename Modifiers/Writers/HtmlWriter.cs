using System.Web;
using SuperScript.Declarables;

namespace SuperScript.Modifiers.Writers
{
	public abstract class HtmlWriter : ModifierBase
	{
		/// <summary>
		/// Executes this instance of <see cref="HtmlWriter"/> upon the collection of <see cref="DeclarationBase"/> objects.
		/// </summary>
		/// <param name="args">An instance of <see cref="PostModifierArgs"/> which contains the data for this <see cref="HtmlWriter"/>.</param>
		/// <returns>
		/// An implementation-specific instance of <see cref="IHtmlString"/>.
		/// </returns>
		public abstract IHtmlString Process(PostModifierArgs args);
	}
}