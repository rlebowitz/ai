using System.Configuration;
using System.Xml;
using AIMLbot.Utils;

namespace AIMLbot.AIMLTagHandlers
{
    /// <summary>
    /// An element called ChatBot, which may be considered a restricted version of get, is used to 
    /// tell the AIML interpreter that it should substitute the contents of a "ChatBot predicate". The 
    /// value of a ChatBot predicate is set at load-time, and cannot be changed at run-time. The AIML 
    /// interpreter may decide how to set the values of ChatBot predicate at load-time. If the ChatBot 
    /// predicate has no value defined, the AIML interpreter should substitute an empty string.
    /// 
    /// The ChatBot element has a required name attribute that identifies the ChatBot predicate. 
    /// 
    /// The ChatBot element does not have any content. 
    /// </summary>
    public class Bot : AIMLTagHandler
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
        public Bot(ChatBot chatBot,
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
            if (TemplateNode.Name.ToLower() == "bot")
            {
                if (TemplateNode.Attributes == null || TemplateNode.Attributes.Count != 1) return string.Empty;
                if (TemplateNode.Attributes[0].Name.ToLower() != "name") return string.Empty;
                var key = TemplateNode.Attributes["name"].Value;
                return ChatBot.Predicates.ContainsKey(key) ? ChatBot.Predicates[key] : string.Empty;
            }
            return string.Empty;
        }
    }
}