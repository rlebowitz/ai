using System;
using System.Xml;
using AIMLbot.Utils;

namespace AIMLbot.AIMLTagHandlers
{
    /// <summary>
    /// The size element tells the AIML interpreter that it should substitute the number of 
    /// categories currently loaded.
    /// 
    /// The size element does not have any content. 
    /// </summary>
    public class Size : AIMLTagHandler
    {
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="template">The node to be processed</param>
        public Size(XmlNode template) : base(template)
        {
        }

        public override string ProcessChange()
        {
            if (Template.Name.ToLower() == "size")
            {
                return Convert.ToString(ChatBot.Size);
            }
            return string.Empty;
        }
    }
}