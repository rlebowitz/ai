using System;
using System.Globalization;
using System.Xml;
using AIMLbot.Utils;

namespace AIMLbot.AIMLTagHandlers
{
    /// <summary>
    ///     The date element tells the AIML interpreter that it should substitute the system local
    ///     date and time. No formatting constraints on the output are specified.
    ///     The date element does not have any content.
    /// </summary>
    public class Date : AIMLTagHandler
    {
        /// <summary>
        /// Implements the date tag.
        /// </summary>
        /// <param name="template">The node to be processed</param>
        public Date(XmlNode template) : base(template)
        {
        }

        public override string ProcessChange()
        {
            return Template.Name.Equals("date", StringComparison.InvariantCultureIgnoreCase) ? DateTime.Now.ToString(CultureInfo.CurrentCulture) : string.Empty;
        }
    }
}