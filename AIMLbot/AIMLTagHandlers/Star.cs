using System;
using System.Xml;
using AIMLbot.Utils;
using log4net;

namespace AIMLbot.AIMLTagHandlers
{
    /// <summary>
    ///     The star element indicates that an AIML interpreter should substitute the value "captured"
    ///     by a particular wildcard from the pattern-specified portion of the match path when returning
    ///     the template.
    ///     The star element has an optional integer index attribute that indicates which wildcard to use.
    ///     The minimum acceptable value for the index is "1" (the first wildcard), and the maximum
    ///     acceptable value is equal to the number of wildcards in the pattern.
    ///     An AIML interpreter should raise an error if the index attribute of a star specifies a wildcard
    ///     that does not exist in the category element's pattern. Not specifying the index is the same as
    ///     specifying an index of "1".
    ///     The star element does not have any content.
    /// </summary>
    public class Star : AIMLTagHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof (Star));

        /// <summary>
        ///     Ctor
        /// </summary>
        /// <param name="query">The query that originated this node</param>
        /// <param name="request">The request inputted into the system</param>
        /// <param name="template">The node to be processed</param>
        public Star(SubQuery query, Request request, XmlNode template) : base(template)
        {
            Query = query;
            Request = request;
        }

        public SubQuery Query { get; set; }

        public Request Request { get; set; }

        public override string ProcessChange()
        {
            if (!Template.Name.Equals("star", StringComparison.CurrentCultureIgnoreCase)) return string.Empty;
            if (Query.InputStar.Count > 0)
            {
                if (Template.Attributes != null && Template.Attributes.Count == 0)
                {
                    // return the first (latest) star in the List<>
                    return Query.InputStar[0];
                }
                if (Template.Attributes == null || Template.Attributes.Count != 1) return string.Empty;
                if (Template.Attributes[0].Name.ToLower() != "index") return string.Empty;
                try
                {
                    var index = Convert.ToInt32(Template.Attributes[0].Value);
                    index--;
                    if ((index >= 0) & (index < Query.InputStar.Count))
                    {
                        return Query.InputStar[index];
                    }
                    Log.Error("InputStar out of bounds reference caused by input: " +
                              Request.RawInput);
                }
                catch
                {
                    Log.Error(
                        "Index set to non-integer value whilst processing star tag in response to the input: " +
                        Request.RawInput);
                }
            }
            else
            {
                Log.Error(
                    "A star tag tried to reference an empty InputStar collection when processing the input: " +
                    Request.RawInput);
            }
            return string.Empty;
        }
    }
}