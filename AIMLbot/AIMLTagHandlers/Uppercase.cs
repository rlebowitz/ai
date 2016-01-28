using System.Globalization;
using System.Xml;
using AIMLbot.Utils;

namespace AIMLbot.AIMLTagHandlers
{
    /// <summary>
    /// The uppercase element tells the AIML interpreter to render the contents of the element
    /// in uppercase, as defined (if defined) by the locale indicated by the specified language
    /// if specified).
    /// 
    /// If no character in this string has a different uppercase version, based on the Unicode 
    /// standard, then the original string is returned. 
    /// </summary>
    public class Uppercase : AIMLTagHandler
    {
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="template">The node to be processed</param>
        public Uppercase(XmlNode template) : base(template)
        {
        }

        public override string ProcessChange()
        {
            if (Template.Name.ToLower() == "uppercase")
            {
                return Template.InnerText.ToUpper(CultureInfo.CurrentCulture);
            }
            return string.Empty;
        }
    }
}