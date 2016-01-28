using System.Linq;
using System.Xml;
using AIMLbot.Utils;

namespace AIMLbot.AIMLTagHandlers
{
    /// <summary>
    ///     The random element instructs the AIML interpreter to return exactly one of its contained li
    ///     elements randomly. The random element must contain one or more li elements of type
    ///     defaultListItem, and cannot contain any other elements.
    /// </summary>
    public class Random : AIMLTagHandler
    {
        /// <summary>
        ///     Ctor
        /// </summary>
        /// <param name="template">The node to be processed</param>
        public Random(XmlNode template) : base(template)
        {
            IsRecursive = false;
        }

        public override string ProcessChange()
        {
            if (Template.Name.ToLower() != "random") return string.Empty;
            if (!Template.HasChildNodes) return string.Empty;
            // only grab <li> nodes
            var listNodes =
                Template.ChildNodes.Cast<XmlNode>().Where(childNode => childNode.Name == "li").ToList();
            if (listNodes.Count > 0)
            {
                var r = new System.Random();
                var chosenNode = listNodes[r.Next(listNodes.Count)];
                return chosenNode.InnerXml;
            }
            return string.Empty;
        }
    }
}