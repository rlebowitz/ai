using System.Xml;
using AIMLbot.Utils;

namespace AIMLbot.AIMLTagHandlers
{
    /// <summary>
    ///     The atomic version of the person2 element is a shortcut for:
    ///     <person2>
    ///         <star />
    ///     </person2>
    ///     The atomic person2 does not have any content.
    ///     The non-atomic person2 element instructs the AIML interpreter to:
    ///     1. replace words with first-person aspect in the result of processing the contents of the
    ///     person2 element with words with the grammatically-corresponding second-person aspect; and,
    ///     2. replace words with second-person aspect in the result of processing the contents of the
    ///     person2 element with words with the grammatically-corresponding first-person aspect.
    ///     The definition of "grammatically-corresponding" is left up to the implementation.
    ///     Historically, implementations of person2 have dealt with pronouns, likely due to the fact
    ///     that most AIML has been written in English. However, the decision about whether to transform
    ///     the person aspect of other words is left up to the implementation.
    /// </summary>
    public class Person2 : AIMLTagHandler
    {
        /// <summary>
        ///     Ctor
        /// </summary>
        /// <param name="query">The query that originated this node</param>
        /// <param name="request">The request inputted into the system</param>
        /// <param name="template">The node to be processed</param>
        public Person2(SubQuery query, Request request, XmlNode template)
            : base(template)
        {
            Query = query;
            Request = request;
        }

        public SubQuery Query { get; set; }

        public Request Request { get; set; }

        public override string ProcessChange()
        {
            while (true)
            {
                if (Template.Name.ToLower() != "person2") return string.Empty;
                if (Template.InnerText.Length > 0)
                {
                    // non atomic version of the node
                    return Template.InnerText.Substitute(ChatBot.Person2);
                }
                // atomic version of the node
                var starNode = GetNode("<star/>");
                var recursiveStar = new Star(Query, Request, starNode);
                Template.InnerText = recursiveStar.ProcessChange();
                if (!string.IsNullOrEmpty(Template.InnerText))
                {
                    continue;
                }
                return string.Empty;
            }
        }
    }
}