using System;
using System.Xml;
using AIMLbot.Utils;
using log4net;

namespace AIMLbot.AIMLTagHandlers
{
    /// <summary>
    /// The topicstar element tells the AIML interpreter that it should substitute the contents of 
    /// a wildcard from the current topic (if the topic contains any wildcards).
    /// 
    /// The topicstar element has an optional integer index attribute that indicates which wildcard 
    /// to use; the minimum acceptable value for the index is "1" (the first wildcard). Not 
    /// specifying the index is the same as specifying an index of "1". 
    /// 
    /// The topicstar element does not have any content. 
    /// </summary>
    public class Topicstar : AIMLTagHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof (Topicstar));

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="query">The query that originated this node</param>
        /// <param name="request">The request inputted into the system</param>
        /// <param name="template">The node to be processed</param>
        public Topicstar(SubQuery query, Request request, XmlNode template) : base(template)
        {
            Query = query;
            Request = request;
        }

        public SubQuery Query { get; set; }
        public Request Request { get; set; }

        public override string ProcessChange()
        {
            if (Template.Name.ToLower() != "topicstar") return string.Empty;
            if (Template.Attributes != null && Template.Attributes.Count == 0)
            {
                if (Query.TopicStar.Count > 0)
                {
                    return Query.TopicStar[0];
                }
                Log.Error(
                    "ERROR! An out of bounds index to topicstar was encountered when processing the input: " +
                    Request.RawInput);
            }
            else if (Template.Attributes != null && Template.Attributes.Count == 1)
            {
                if (Template.Attributes[0].Name.ToLower() != "index") return string.Empty;
                if (Template.Attributes[0].Value.Length <= 0) return string.Empty;
                try
                {
                    int result = Convert.ToInt32(Template.Attributes[0].Value.Trim());
                    if (Query.TopicStar.Count > 0)
                    {
                        if (result > 0)
                        {
                            return Query.TopicStar[result - 1];
                        }
                        Log.Error("ERROR! An input tag with a bady formed index (" +
                                  Template.Attributes[0].Value +
                                  ") was encountered processing the input: " + Request.RawInput);
                    }
                    else
                    {
                        Log.Error(
                            "ERROR! An out of bounds index to topicstar was encountered when processing the input: " +
                            Request.RawInput);
                    }
                }
                catch
                {
                    Log.Error("ERROR! A thatstar tag with a bady formed index (" +
                              Template.Attributes[0].Value +
                              ") was encountered processing the input: " + Request.RawInput);
                }
            }
            return string.Empty;
        }
    }
}