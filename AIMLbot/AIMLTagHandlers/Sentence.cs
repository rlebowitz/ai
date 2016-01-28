using System;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using AIMLbot.Utils;

namespace AIMLbot.AIMLTagHandlers
{
    /// <summary>
    ///     The sentence element tells the AIML interpreter to render the contents of the element
    ///     such that the first letter of each sentence is in uppercase, as defined (if defined) by
    ///     the locale indicated by the specified language (if specified). Sentences are interpreted
    ///     as strings whose last character is the period or full-stop character .. If the string does
    ///     not contain a ., then the entire string is treated as a sentence.
    ///     If no character in this string has a different uppercase version, based on the Unicode
    ///     standard, then the original string is returned.
    /// </summary>
    public class Sentence : AIMLTagHandler
    {
        /// <summary>
        ///     Ctor
        /// </summary>
        /// <param name="query">The query that originated this node</param>
        /// <param name="request">The request inputted into the system</param>
        /// <param name="template">The node to be processed</param>
        public Sentence(SubQuery query, Request request, XmlNode template) : base(template)
        {
            Query = query;
            Request = request;
        }

        public SubQuery Query { get; set; }

        public Request Request { get; set; }

        public override string ProcessChange()
        {
            while (true)
            {
                if (Template.Name.ToLower() != "sentence") return string.Empty;
                if (Template.InnerText.Length > 0)
                {
                    var result = new StringBuilder();
                    var letters = Template.InnerText.Trim().ToCharArray();
                    var doChange = true;
                    foreach (var t in letters)
                    {
                        var letterAsString = Convert.ToString(t);
                        if (ChatBot.Splitters.Contains(letterAsString))
                        {
                            doChange = true;
                        }

                        var lowercaseLetter = new Regex("[a-zA-Z]");

                        if (lowercaseLetter.IsMatch(letterAsString))
                        {
                            if (doChange)
                            {
                                result.Append(letterAsString.ToUpper(CultureInfo.CurrentCulture));
                                doChange = false;
                            }
                            else
                            {
                                result.Append(letterAsString.ToLower(CultureInfo.CurrentCulture));
                            }
                        }
                        else
                        {
                            result.Append(letterAsString);
                        }
                    }
                    return result.ToString();
                }
                // atomic version of the node
                var starNode = GetNode("<star/>");
                var recursiveStar = new Star(Query, Request, starNode);
                Template.InnerText = recursiveStar.ProcessChange();
                if (!string.IsNullOrEmpty(Template.InnerText)) continue;
                return string.Empty;
            }
        }
    }
}