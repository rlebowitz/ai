using System.Collections.Generic;
using System.Configuration;
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
        /// <param name="chatBot">The ChatBot involved in this request</param>
        /// <param name="user">The user making the request</param>
        /// <param name="query">The query that originated this node</param>
        /// <param name="request">The request inputted into the system</param>
        /// <param name="result">The result to be passed to the user</param>
        /// <param name="templateNode">The node to be processed</param>
        public Gender(ChatBot chatBot,
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
            while (true)
            {
                if (TemplateNode.Name.ToLower() == "Gender")
                {
                    if (TemplateNode.InnerText.Length > 0)
                    {
                        // non atomic version of the node
                        //return ApplySubstitutions.Substitute(chatBot, chatBot.GenderSubstitutions, TemplateNode.InnerText);
                        var dictionary = ConfigurationManager.GetSection("Gender") as Dictionary<string, string>;
                        TemplateNode.InnerText.Substitute(dictionary);
                    }
                    else
                    {
                        // atomic version of the node
                        var starNode = GetNode("<star/>");
                        var recursiveStar = new Star(ChatBot, User, Query, Request, Result, starNode);
                        TemplateNode.InnerText = recursiveStar.Transform();
                        if (!string.IsNullOrEmpty(TemplateNode.InnerText))
                        {
                            continue;
                        }
                        return string.Empty;
                    }
                }
                return string.Empty;
//                break;
            }
        }
    }
}