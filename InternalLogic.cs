using System.Text;
using HtmlAgilityPack;

namespace SuperScript
{
    public static class InternalLogic
    {
        /// <summary>
        /// Ensures that the specified <see cref="contents"/> are stripped of any wrapping tags.
        /// </summary>
        /// <param name="contents"></param>
        /// <returns>A <see cref="StringBuilder"/> representing the contents, stripped of any wrapping tags.</returns>
        public static StringBuilder EnsureStripppedContents(string contents)
        {
            // load this into an HtmlAgility document.
            // - if a <script> tag is present then HtmlAgility will recognise thSuperScript.JavaScript.Containers.WebForms.is and we can request the InnerText
            // - even if there is no <script> tag we can still request the InnerText
            var doc = new HtmlDocument();
            doc.LoadHtml(contents);
            var jsBuilder = new StringBuilder(doc.DocumentNode.InnerText.Trim());

            doc = null;


            // ensure that any HTML comments are now converted to JavaScript comments
            jsBuilder = jsBuilder.Replace("<!--", "/* <!--").Replace("-->", "--> */");

            return jsBuilder;
        }
    }
}