using System.Xml;
using AIMLbot.Utils;
using log4net;

namespace AIMLbot.AIMLTagHandlers
{
    /// <summary>
    /// NOT IMPLEMENTED FOR SECURITY REASONS
    /// </summary>
    public class Javascript : AIMLTagHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(Javascript));

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="chatBot">The ChatBot involved in this request</param>
        /// <param name="user">The user making the request</param>
        /// <param name="query">The query that originated this node</param>
        /// <param name="request">The request inputted into the system</param>
        /// <param name="result">The result to be passed to the user</param>
        /// <param name="templateNode">The node to be processed</param>
        public Javascript(ChatBot chatBot,
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
            Log.Error("The javascript tag is not implemented in this ChatBot");
            return string.Empty;
        }
    }
}