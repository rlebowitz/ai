using System.Xml;
using AIMLbot.Utils;
using log4net;

namespace AIMLbot.AIMLTagHandlers
{
    /// <summary>
    /// NOT IMPLEMENTED FOR SECURITY REASONS
    /// </summary>
    public class SystemTag : AIMLTagHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(SystemTag));

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="template">The node to be processed</param>
        public SystemTag(XmlNode template) : base(template)
        {
        }

        public override string ProcessChange()
        {
            Log.Error("The system tag is not implemented in this ChatBot");
            return string.Empty;
        }
    }
}