using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace AIMLbot.Utils
{
    //http://shadowcoding.blogspot.com/2008/10/configuration-section-handlers-via.html
    public class AimlConfigurationHandler : IConfigurationSectionHandler
    {
        public object Create(object parent, object configContext, XmlNode section)
        {
            var doc = XDocument.Parse(section.OuterXml);
            var root = (XElement) doc.FirstNode;
            if (root.Descendants().Any(e => e.Name != "item"))
                throw new ConfigurationErrorsException("The section only accepts item elements.");
            try
            {
                var elements = root.Descendants("item");
                var list = elements
                    .Select(p => new {name = p.Attribute("name").Value, value = p.Attribute("value").Value});
                var dictionary = new Dictionary<string, string>();
                foreach (var item in list)
                {
                    if (!dictionary.ContainsKey(item.name))
                    {
                        dictionary.Add(item.name, item.value);
                    }
                    else
                    {
                        dictionary[item.name] = item.value;
                    }
                }
                return dictionary;
            }
            catch (Exception ex)
            {
                throw new ConfigurationErrorsException("Error reading AIML item element.", ex);
            }
        }
    }
}