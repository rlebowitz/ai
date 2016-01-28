using System.Xml;
using AIMLbot.Utils;

namespace AIMLbot.AIMLTagHandlers
{
    /// <summary>
    ///     The set element instructs the AIML interpreter to set the value of a predicate to the result
    ///     of processing the contents of the set element. The set element has a required attribute name,
    ///     which must be a valid AIML predicate name. If the predicate has not yet been defined, the AIML
    ///     interpreter should define it in memory.
    ///     The AIML interpreter should, generically, return the result of processing the contents of the
    ///     set element. The set element must not perform any text formatting or other "normalization" on
    ///     the predicate contents when returning them.
    ///     The AIML interpreter implementation may optionally provide a mechanism that allows the AIML
    ///     author to designate certain predicates as "return-name-when-set", which means that a set
    ///     operation using such a predicate will return the name of the predicate, rather than its
    ///     captured value. (See [9.2].)
    ///     A set element may contain any AIML template elements.
    /// </summary>
    public class Set : AIMLTagHandler
    {
        /// <summary>
        ///     Ctor
        /// </summary>
        /// <param name="user">The user making the request</param>
        /// <param name="template">The node to be processed</param>
        public Set(User user, XmlNode template) : base(template)
        {
            User = user;
        }

        public User User { get; set; }

        public override string ProcessChange()
        {
            if (Template.Name.ToLower() == "set")
            {
                if (Template.Attributes != null && Template.Attributes.Count == 1)
                {
                    if (Template.Attributes[0].Name.ToLower() == "name")
                    {
                        if (Template.InnerText.Length > 0)
                        {
                            User.Predicates.Add(Template.Attributes[0].Value, Template.InnerText);
                            return User.Predicates[Template.Attributes[0].Value];
                        }
                        // remove the predicate
                        User.Predicates.Remove(Template.Attributes[0].Value);
                        return string.Empty;
                    }
                }
            }
            return string.Empty;
        }
    }
}