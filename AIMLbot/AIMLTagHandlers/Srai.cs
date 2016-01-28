using System.Xml;
using AIMLbot.Utils;

namespace AIMLbot.AIMLTagHandlers
{
    /// <summary>
    ///     The srai element instructs the AIML interpreter to pass the result of processing the contents
    ///     of the srai element to the AIML matching loop, as if the input had been produced by the user
    ///     (this includes stepping through the entire input normalization process). The srai element does
    ///     not have any attributes. It may contain any AIML template elements.
    ///     As with all AIML elements, nested forms should be parsed from inside out, so embedded srais are
    ///     perfectly acceptable.
    /// </summary>
    public class Srai : AIMLTagHandler
    {
        /// <summary>
        ///     Ctor
        /// </summary>
        /// <param name="chatBot">The ChatBot involved in this request</param>
        /// <param name="user">The user making the request</param>
        /// <param name="request">The request inputted into the system</param>
        /// <param name="templateNode">The node to be processed</param>
        public Srai(ChatBot chatBot,
            User user,
            Request request,
            XmlNode templateNode)
            : base(templateNode)
        {
            ChatBot = chatBot;
            User = user;
            Request = request;
        }

        public ChatBot ChatBot { get; set; }

        public User User { get; set; }

        public Request Request { get; set; }

        public override string ProcessChange()
        {
            if (Template.Name.ToLower() == "srai")
            {
                if (Template.InnerText.Length > 0)
                {
                    // make sure we don't keep adding time to the request
                    var subRequest = new Request(Template.InnerText, User) {StartedOn = Request.StartedOn};
                    var subQuery = ChatBot.Chat(subRequest);
                    Request.HasTimedOut = subRequest.HasTimedOut;
                    return subQuery.Output;
                }
            }
            return string.Empty;
        }
    }
}