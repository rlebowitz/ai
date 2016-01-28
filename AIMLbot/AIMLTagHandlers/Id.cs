using System;
using System.Xml;
using AIMLbot.Utils;

namespace AIMLbot.AIMLTagHandlers
{
    /// <summary>
    ///     The id element tells the AIML interpreter that it should substitute the user ID.
    ///     The determination of the user ID is not specified, since it will vary by application.
    ///     A suggested default return value is "localhost".
    ///     The id element does not have any content.
    /// </summary>
    public class Id : AIMLTagHandler
    {
        /// <summary>
        ///     Ctor
        /// </summary>
        /// <param name="user">The user making the request</param>
        /// <param name="template">The node to be processed</param>
        public Id(User user, XmlNode template) : base(template)
        {
            User = user;
        }

        public User User { get; set; }

        public override string ProcessChange()
        {
            if (!Template.Name.Equals("id", StringComparison.OrdinalIgnoreCase)) return string.Empty;
            return User.UserId;
        }
    }
}