using System.Xml;
using AIMLbot.Utils;
using log4net;

namespace AIMLbot.AIMLTagHandlers
{
    /// <summary>
    ///     The gossip element instructs the AIML interpreter to capture the result of processing the
    ///     contents of the gossip elements and to store these contents in a manner left up to the
    ///     implementation. Most common uses of gossip have been to store captured contents in a separate
    ///     file.
    ///     The gossip element does not have any attributes. It may contain any AIML template elements.
    /// </summary>
    public class Gossip : AIMLTagHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof (Gossip));

        /// <summary>
        ///     Ctor
        /// </summary>
        /// <param name="user">The user making the request</param>
        /// <param name="template">The node to be processed</param>
        public Gossip(User user, XmlNode template) : base(template)
        {
            User = user;
        }

        public User User { get; set; }

        public override string ProcessChange()
        {
            if (Template.Name.ToLower() == "gossip")
            {
                // gossip is merely logged by the ChatBot and written to log files
                if (Template.InnerText.Length > 0)
                {
                    Log.Info("GOSSIP from user: " + User.UserId + ", '" + Template.InnerText + "'");
                }
            }
            return string.Empty;
        }
    }
}