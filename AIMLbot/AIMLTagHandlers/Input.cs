using System;
using System.Xml;
using AIMLbot.Utils;
using log4net;

namespace AIMLbot.AIMLTagHandlers
{
    /// <summary>
    ///     The input element tells the AIML interpreter that it should substitute the contents of a
    ///     previous user input.
    ///     The template-side input has an optional index attribute that may contain either a single
    ///     integer or a comma-separated pair of integers. The minimum value for either of the integers
    ///     in the index is "1". The index tells the AIML interpreter which previous user input should
    ///     be returned (first dimension), and optionally which "sentence" (see [8.3.2.]) of the previous
    ///     user input.
    ///     The AIML interpreter should raise an error if either of the specified index dimensions is
    ///     invalid at run-time.
    ///     An unspecified index is the equivalent of "1,1". An unspecified second dimension of the index
    ///     is the equivalent of specifying a "1" for the second dimension.
    ///     The input element does not have any content.
    /// </summary>
    public class Input : AIMLTagHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof (Input));

        /// <summary>
        ///     Ctor
        /// </summary>
        /// <param name="user">The user making the request</param>
        /// <param name="request">The request inputted into the system</param>
        /// <param name="template">The node to be processed</param>
        public Input(User user, Request request, XmlNode template) : base(template)
        {
            User = user;
            Request = request;
        }

        public User User { get; set; }
        public Request Request { get; set; }

        public override string ProcessChange()
        {
            if (Template.Name.ToLower() != "input") return string.Empty;
            if (Template.Attributes != null && Template.Attributes.Count == 0)
            {
                return User.GetResultSentence();
            }
            if (Template.Attributes != null && Template.Attributes.Count == 1)
            {
                if (Template.Attributes[0].Name.ToLower() == "index")
                {
                    if (Template.Attributes[0].Value.Length > 0)
                    {
                        try
                        {
                            // see if there is a split
                            var dimensions = Template.Attributes[0].Value.Split(",".ToCharArray());
                            if (dimensions.Length == 2)
                            {
                                var result = Convert.ToInt32(dimensions[0].Trim());
                                var sentence = Convert.ToInt32(dimensions[1].Trim());
                                if ((result > 0) & (sentence > 0))
                                {
                                    return User.GetResultSentence(result - 1, sentence - 1);
                                }
                                Log.Error("ERROR! An input tag with a bady formed index (" +
                                          Template.Attributes[0].Value +
                                          ") was encountered processing the input: " + Request.RawInput);
                            }
                            else
                            {
                                var result = Convert.ToInt32(Template.Attributes[0].Value.Trim());
                                if (result > 0)
                                {
                                    return User.GetResultSentence(result - 1);
                                }
                                Log.Error("ERROR! An input tag with a bady formed index (" +
                                          Template.Attributes[0].Value +
                                          ") was encountered processing the input: " + Request.RawInput);
                            }
                        }
                        catch
                        {
                            Log.Error("ERROR! An input tag with a bady formed index (" +
                                      Template.Attributes[0].Value +
                                      ") was encountered processing the input: " + Request.RawInput);
                        }
                    }
                }
            }
            return string.Empty;
        }
    }
}