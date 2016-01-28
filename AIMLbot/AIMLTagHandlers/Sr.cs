using System.Xml;
using AIMLbot.Utils;

namespace AIMLbot.AIMLTagHandlers
{
    /// <summary>
    /// The sr element is a shortcut for: 
    /// 
    /// <srai><star/></srai> 
    /// 
    /// The atomic sr does not have any content. 
    /// </summary>
    public class Sr : AIMLTagHandler
    {
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="chatBot">The ChatBot involved in this request</param>
        /// <param name="user">The user making the request</param>
        /// <param name="query">The query that originated this node</param>
        /// <param name="request">The request inputted into the system</param>
        /// <param name="template">The node to be processed</param>
        public Sr(ChatBot chatBot, User user, SubQuery query, Request request, XmlNode template) : base(template)
        {
            ChatBot = chatBot;
            User = user;
            Query = query;
            Request = request;
        }

        public ChatBot ChatBot { get; set; }

        public User User { get; set; }

        public SubQuery Query { get; set; }

        public Request Request { get; set; }

        public override string ProcessChange()
        {
            if (Template.Name.ToLower() == "sr")
            {
                XmlNode starNode = GetNode("<star/>");
                Star recursiveStar = new Star(Query, Request, starNode);
                string starContent = recursiveStar.ProcessChange();

                XmlNode sraiNode = GetNode("<srai>" + starContent + "</srai>");
                Srai sraiHandler = new Srai(ChatBot, User, Request, sraiNode);
                return sraiHandler.ProcessChange();
            }
            return string.Empty;
        }
    }
}