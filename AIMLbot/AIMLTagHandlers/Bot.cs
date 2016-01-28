using System.Xml;
using AIMLbot.Utils;

namespace AIMLbot.AIMLTagHandlers
{
    /// <summary>
    /// An element called ChatBot, which may be considered a restricted version of get, is used to 
    /// tell the AIML interpreter that it should substitute the contents of a "ChatBot predicate". The 
    /// value of a ChatBot predicate is set at load-time, and cannot be changed at run-time. The AIML 
    /// interpreter may decide how to set the values of ChatBot predicate at load-time. If the ChatBot 
    /// predicate has no value defined, the AIML interpreter should substitute an empty string.
    /// 
    /// The ChatBot element has a required name attribute that identifies the ChatBot predicate. 
    /// 
    /// The ChatBot element does not have any content. 
    /// </summary>
    public class Bot : AIMLTagHandler
    {
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="template">The node to be processed</param>
        public Bot(XmlNode template) : base(template)
        {
        }

        public override string ProcessChange()
        {
            if (Template.Name.ToLower() == "bot")
            {
                if (Template.Attributes == null || Template.Attributes.Count != 1) return string.Empty;
                if (Template.Attributes[0].Name.ToLower() != "name") return string.Empty;
                var key = Template.Attributes["name"].Value;
                return ChatBot.Predicates.ContainsKey(key) ? ChatBot.Predicates[key] : string.Empty;
            }
            return string.Empty;
        }
    }
}