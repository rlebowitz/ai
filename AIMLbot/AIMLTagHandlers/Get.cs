using System;
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
        /// Implements the get tag
        /// </summary>
        /// <param name="user">The user making the request</param>
        /// <param name="template">The node to be processed</param>
        public Get(User user, XmlNode template) : base(template)
        {
            User = user;
        }

        public User User { get; set; }

        public override string ProcessChange()
        {
            if (!Template.Name.Equals("get", StringComparison.CurrentCultureIgnoreCase)) return string.Empty;
            if (Template.Attributes == null || Template.Attributes.Count != 1) return string.Empty;
            if (Template.Attributes[0].Name.ToLower() != "name") return string.Empty;
            var value = Template.Attributes[0].Value;
            return User.Predicates.ContainsKey(value) ? User.Predicates[value] : string.Empty;
        }
    }
}