using System.Xml;

namespace AIMLbot.UnitTest
{
    public class StaticHelpers
    {
        /// <summary>
        /// Turns the passed string into an XML node
        /// </summary>
        /// <param name="outerXML">the string to XMLize</param>
        /// <returns>The XML node</returns>
        public static XmlNode GetNode(string outerXML)
        {
            XmlDocument temp = new XmlDocument();
            temp.LoadXml(outerXML);
            return temp.FirstChild;
        }
    }
}