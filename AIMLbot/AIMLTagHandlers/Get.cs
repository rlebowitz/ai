using System.Xml;
using AIMLbot.Utils;

namespace AIMLbot.AIMLTagHandlers
{
    /// <summary>
    ///     The get element tells the AIML interpreter that it should substitute the contents of a
    ///     predicate, if that predicate has a value defined. If the predicate has no value defined,
    ///     the AIML interpreter should substitute the empty string "".
    ///     The AIML interpreter implementation may optionally provide a mechanism that allows the
    ///     AIML author to designate default values for certain predicates (see [9.3.]).
    ///     The get element must not perform any text formatting or other "normalization" on the predicate
    ///     contents when returning them.
    ///     The get element has a required name attribute that identifies the predicate with an AIML
    ///     predicate name.
    ///     The get element does not have any content.
    /// </summary>
    public class Get : AIMLTagHandler
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
        public Get(ChatBot chatBot,
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
            if (TemplateNode.Name.ToLower() == "get")
            {
                if (TemplateNode.Attributes != null && TemplateNode.Attributes.Count == 1)
                {
                    if (TemplateNode.Attributes[0].Name.ToLower() == "name")
                    {
                        var value = TemplateNode.Attributes[0].Value;
                        if (User.Predicates.ContainsKey(value)) { 
                            return User.Predicates[value];
                        }
                    }
                }
            }
            return string.Empty;
        }
    }
}