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
        /// <param name="chatBot">The ChatBot involved in this request</param>
        /// <param name="user">The user making the request</param>
        /// <param name="query">The query that originated this node</param>
        /// <param name="request">The request inputted into the system</param>
        /// <param name="result">The result to be passed to the user</param>
        /// <param name="templateNode">The node to be processed</param>
        public Size(ChatBot chatBot,
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
            if (TemplateNode.Name.ToLower() == "size")
            {
                return Convert.ToString(ChatBot.Size);
            }
            return string.Empty;
        }
    }
}