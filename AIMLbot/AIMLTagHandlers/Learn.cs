using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using AIMLbot.Utils;
using log4net;

namespace AIMLbot.AIMLTagHandlers
{
    /// <summary>
    /// The learn element instructs the AIML interpreter to retrieve a resource specified by a URI, 
    /// and to process its AIML object contents.
    /// </summary>
    public class Learn : AIMLTagHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(Learn));

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="chatBot">The ChatBot involved in this request</param>
        /// <param name="user">The user making the request</param>
        /// <param name="query">The query that originated this node</param>
        /// <param name="request">The request inputted into the system</param>
        /// <param name="result">The result to be passed to the user</param>
        /// <param name="templateNode">The node to be processed</param>
        public Learn(ChatBot chatBot,
                     User user,
                     SubQuery query,
                     Request request,
                     Result result,
                     XmlNode templateNode)
            : base(chatBot, user, query, request, result, templateNode)
        {
        }

        protected override string ProcessChange()
        {
            if (TemplateNode.Name.ToLower() != "learn") return string.Empty;
            // currently only AIML files in the local filesystem can be referenced
            // ToDo: Network HTTP and web service based learning
            if (TemplateNode.InnerText.Length > 0)
            {
                string path = TemplateNode.InnerText;
                FileInfo fi = new FileInfo(path);
                if (fi.Exists)
                {
                    var doc = XDocument.Load(path);
                    try
                    {
                        ChatBot.LoadAIML(doc);
                    }
                    catch(Exception ex)
                    {
                        Log.Error("Unable to learn some new AIML from the following URI: " + path, ex);
                    }
                }
            }
            return string.Empty;
        }
    }
}