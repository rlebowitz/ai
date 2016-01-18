using System.Xml;
using AIMLbot.Utils;
using log4net;

namespace AIMLbot.AIMLTagHandlers
{
    /// <summary>
    /// The gossip element instructs the AIML interpreter to capture the result of processing the 
    /// contents of the gossip elements and to store these contents in a manner left up to the 
    /// implementation. Most common uses of gossip have been to store captured contents in a separate 
    /// file. 
    /// 
    /// The gossip element does not have any attributes. It may contain any AIML template elements.
    /// </summary>
    public class Gossip : AIMLTagHandler
    {

        private static readonly ILog Log = LogManager.GetLogger(typeof(Gossip));

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="chatBot">The ChatBot involved in this request</param>
        /// <param name="user">The user making the request</param>
        /// <param name="query">The query that originated this node</param>
        /// <param name="request">The request inputted into the system</param>
        /// <param name="result">The result to be passed to the user</param>
        /// <param name="templateNode">The node to be processed</param>
        public Gossip(ChatBot chatBot,
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
            if (TemplateNode.Name.ToLower() == "gossip")
            {
                // gossip is merely logged by the ChatBot and written to log files
                if (TemplateNode.InnerText.Length > 0)
                {
                    Log.Info("GOSSIP from user: " + User.UserId + ", '" + TemplateNode.InnerText + "'");
                }
            }
            return string.Empty;
        }
    }
}