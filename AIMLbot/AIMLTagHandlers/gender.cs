using System.Xml;
using AIMLbot.Utils;

namespace AIMLbot.AIMLTagHandlers
{
    /// <summary>
    ///     The atomic version of the Gender element is a shortcut for:
    ///     <Gender>
    ///         <star />
    ///     </Gender>
    ///     The atomic Gender does not have any content.
    ///     The non-atomic Gender element instructs the AIML interpreter to:
    ///     1. replace male-gendered words in the result of processing the contents of the Gender element
    ///     with the grammatically-corresponding female-gendered words; and
    ///     2. replace female-gendered words in the result of processing the contents of the Gender element
    ///     with the grammatically-corresponding male-gendered words.
    ///     The definition of "grammatically-corresponding" is left up to the implementation.
    ///     Historically, implementations of Gender have exclusively dealt with pronouns, likely due to the
    ///     fact that most AIML has been written in English. However, the decision about whether to
    ///     transform Gender of other words is left up to the implementation.
    /// </summary>
    public class Gender : AIMLTagHandler
    {
        /// <summary>
        ///     Ctor
        /// </summary>
        /// <param name="query">The query that originated this node</param>
        /// <param name="request">The request inputted into the system</param>
        /// <param name="template">The node to be processed</param>
        public Gender(SubQuery query, Request request, XmlNode template)
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
                if (Template.Name.ToLower() == "gender")
                {
                    var text = Template.InnerText;
                    if (text.Length > 0)
                    {
                        // non atomic version of the node
                        return text.Substitute(ChatBot.Genders);
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
                return string.Empty;
            }
        }
    }
}