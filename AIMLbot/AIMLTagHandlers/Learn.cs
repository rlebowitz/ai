using System;
using System.IO;
using System.Xml;
using AIMLbot.Utils;
using log4net;

namespace AIMLbot.AIMLTagHandlers
{
    /// <summary>
    ///     The learn element instructs the AIML interpreter to retrieve a resource specified by a URI,
    ///     and to process its AIML object contents.
    /// </summary>
    public class Learn : AIMLTagHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof (Learn));

        /// <summary>
        ///     Ctor
        /// </summary>
        /// <param name="template">The node to be processed</param>
        public Learn(XmlNode template) : base(template)
        {
        }

        public override string ProcessChange()
        {
            if (Template.Name.ToLower() != "learn") return string.Empty;
            // currently only AIML files in the local filesystem can be referenced
            // ToDo: Network HTTP and web service based learning
            if (Template.InnerText.Length > 0)
            {
                var path = Template.InnerText;
                var fi = new FileInfo(path);
                try
                {
                    var loader = new AIMLLoader();
                    loader.LoadAIML(fi);
                }
                catch (Exception ex)
                {
                    Log.Error("Unable to learn some new AIML from the following URI: " + path, ex);
                }
            }
            return string.Empty;
        }
    }
}