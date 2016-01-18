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
        /// <param name="chatBot">The ChatBot involved in this request</param>
        /// <param name="user">The user making the request</param>
        /// <param name="query">The query that originated this node</param>
        /// <param name="request">The request inputted into the system</param>
        /// <param name="result">The result to be passed to the user</param>
        /// <param name="templateNode">The node to be processed</param>
        public Version(ChatBot chatBot,
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
            return TemplateNode.Name.ToLower() == "version" ? ConfigurationManager.AppSettings.Get("version", "unknown") : string.Empty;
        }
    }
}