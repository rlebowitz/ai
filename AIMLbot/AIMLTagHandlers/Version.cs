using System.Configuration;
using System.Xml;
using AIMLbot.Utils;

namespace AIMLbot.AIMLTagHandlers
{
    /// <summary>
    /// The version element tells the AIML interpreter that it should substitute the version number
    /// of the AIML interpreter.
    /// 
    /// The version element does not have any content. 
    /// </summary>
    public class Version : AIMLTagHandler
    {
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="template">The node to be processed</param>
        public Version(XmlNode template) : base(template)
        {
        }

        public override string ProcessChange()
        {
            return Template.Name.ToLower() == "version"
                ? ConfigurationManager.AppSettings.Get("version", "unknown")
                : string.Empty;
        }
    }
}