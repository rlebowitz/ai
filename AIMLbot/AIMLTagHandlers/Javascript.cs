using System.Xml;
using AIMLbot.Utils;
using log4net;

namespace AIMLbot.AIMLTagHandlers
{
    /// <summary>
    ///     NOT IMPLEMENTED FOR SECURITY REASONS
    /// </summary>
    public class Javascript : AIMLTagHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof (Javascript));

        /// <summary>
        ///     Ctor
        /// </summary>
        /// <param name="template">The node to be processed</param>
        public Javascript(XmlNode template) : base(template)
        {
        }

        public override string ProcessChange()
        {
            Log.Error("The javascript tag is not implemented in this ChatBot");
            return string.Empty;
        }
    }
}