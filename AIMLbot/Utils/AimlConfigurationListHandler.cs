using System;
using System.Configuration;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace AIMLbot.Utils
{
    //http://shadowcoding.blogspot.com/2008/10/configuration-section-handlers-via.html
    public class AimlConfigurationListHandler : IConfigurationSectionHandler
    {
        public object Create(object parent, object configContext, XmlNode section)
        {
            var doc = XDocument.Parse(section.OuterXml);
            var root = (XElement) doc.FirstNode;
            if (root.Descendants().Any(e => e.Name != "item"))
                throw new ConfigurationErrorsException("The section only accepts item elements.");
            try
            {
                var elements = root.Descendants("item")
                    .Select(p => p.Attribute("value").Value)
                    .ToList();
                return elements;
            }
            catch (Exception ex)
            {
                throw new ConfigurationErrorsException("Error reading AIML item element.", ex);
            }
        }
    }
}