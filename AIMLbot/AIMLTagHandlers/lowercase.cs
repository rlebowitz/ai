using System.Globalization;
using System.Xml;
using AIMLbot.Utils;

namespace AIMLbot.AIMLTagHandlers
{
    /// <summary>
    /// The lowercase element tells the AIML interpreter to render the contents of the element 
    /// in lowercase, as defined (if defined) by the locale indicated by the specified language
    /// (if specified). 
    /// 
    /// If no character in this string has a different lowercase version, based on the Unicode 
    /// standard, then the original string is returned. 
    /// </summary>
    public class Lowercase : AIMLTagHandler
    {
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="template">The node to be processed</param>
        public Lowercase(XmlNode template) : base(template)
        {
        }

        public override string ProcessChange()
        {
            return Template.Name.ToLower() == "lowercase" ? Template.InnerText.ToLower(CultureInfo.CurrentCulture) : string.Empty;
        }
    }
}