using System;
using System.Globalization;
using System.Xml;
using AIMLbot.Utils;

namespace AIMLbot.AIMLTagHandlers
{
    /// <summary>
    /// The formal element tells the AIML interpreter to render the contents of the element 
    /// such that the first letter of each word is in uppercase, as defined (if defined) by 
    /// the locale indicated by the specified language (if specified). This is similar to methods 
    /// that are sometimes called "Title Case". 
    /// 
    /// If no character in this string has a different uppercase version, based on the Unicode 
    /// standard, then the original string is returned.
    /// </summary>
    public class Formal : AIMLTagHandler
    {
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="template">The node to be processed</param>
        public Formal(XmlNode template) : base(template)
        {
        }

        public override string ProcessChange()
        {
            if (!Template.Name.Equals("formal", StringComparison.CurrentCultureIgnoreCase)) return string.Empty;
            if (Template.InnerText.Length <= 0) return string.Empty;
            var ti = CultureInfo.CurrentCulture.TextInfo;
            return ti.ToTitleCase(Template.InnerText.ToLower());
        }
    }
}