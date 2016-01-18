using System;
using System.Xml;
using AIMLbot.Utils;
using log4net;

namespace AIMLbot.AIMLTagHandlers
{
    /// <summary>
    /// The input element tells the AIML interpreter that it should substitute the contents of a 
    /// previous user input. 
    /// 
    /// The template-side input has an optional index attribute that may contain either a single 
    /// integer or a comma-separated pair of integers. The minimum value for either of the integers 
    /// in the index is "1". The index tells the AIML interpreter which previous user input should 
    /// be returned (first dimension), and optionally which "sentence" (see [8.3.2.]) of the previous 
    /// user input. 
    /// 
    /// The AIML interpreter should raise an error if either of the specified index dimensions is 
    /// invalid at run-time. 
    /// 
    /// An unspecified index is the equivalent of "1,1". An unspecified second dimension of the index 
    /// is the equivalent of specifying a "1" for the second dimension. 
    /// 
    /// The input element does not have any content. 
    /// </summary>
    public class Input : AIMLTagHandler
    {

        private static readonly ILog Log = LogManager.GetLogger(typeof(Input));

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="chatBot">The ChatBot involved in this request</param>
        /// <param name="user">The user making the request</param>
        /// <param name="query">The query that originated this node</param>
        /// <param name="request">The request inputted into the system</param>
        /// <param name="result">The result to be passed to the user</param>
        /// <param name="templateNode">The node to be processed</param>
        public Input(ChatBot chatBot,
                     User user,
                     SubQuery query,
                     Request request,
                     Result result,
                     XmlNode templateNode)
            : base(chatBot, user, query, request, result, templateNode)
        {
        }

        protected override string ProcessChange()
        {
            if (TemplateNode.Name.ToLower() != "input") return string.Empty;
            if (TemplateNode.Attributes != null && TemplateNode.Attributes.Count == 0)
            {
                return User.getResultSentence();
            }
            if (TemplateNode.Attributes != null && TemplateNode.Attributes.Count == 1)
            {
                if (TemplateNode.Attributes[0].Name.ToLower() == "index")
                {
                    if (TemplateNode.Attributes[0].Value.Length > 0)
                    {
                        try
                        {
                            // see if there is a split
                            string[] dimensions = TemplateNode.Attributes[0].Value.Split(",".ToCharArray());
                            if (dimensions.Length == 2)
                            {
                                int result = Convert.ToInt32(dimensions[0].Trim());
                                int sentence = Convert.ToInt32(dimensions[1].Trim());
                                if ((result > 0) & (sentence > 0))
                                {
                                    return User.getResultSentence(result - 1, sentence - 1);
                                }
                                else
                                {
                                    Log.Error("ERROR! An input tag with a bady formed index (" +
                                              TemplateNode.Attributes[0].Value +
                                              ") was encountered processing the input: " + Request.RawInput);
                                }
                            }
                            else
                            {
                                int result = Convert.ToInt32(TemplateNode.Attributes[0].Value.Trim());
                                if (result > 0)
                                {
                                    return User.getResultSentence(result - 1);
                                }
                                else
                                {
                                    Log.Error("ERROR! An input tag with a bady formed index (" +
                                              TemplateNode.Attributes[0].Value +
                                              ") was encountered processing the input: " + Request.RawInput);
                                }
                            }
                        }
                        catch
                        {
                            Log.Error("ERROR! An input tag with a bady formed index (" +
                                      TemplateNode.Attributes[0].Value +
                                      ") was encountered processing the input: " + Request.RawInput);
                        }
                    }
                }
            }
            return string.Empty;
        }
    }
}