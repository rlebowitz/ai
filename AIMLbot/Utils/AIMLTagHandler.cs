using System.Xml;

namespace AIMLbot.Utils
{
    /// <summary>
    ///     The template for all classes that handle the AIML tags found within template nodes of a
    ///     category.
    /// </summary>
    public abstract class AIMLTagHandler : IAIMLTagHandler
    {
        /// <summary>
        ///     A flag to denote if inner tags are to be processed recursively before processing this tag
        /// </summary>
        public bool IsRecursive = true;

        ///// <summary>
        /////     A representation of the input into the ChatBot made by the user
        ///// </summary>
        //public Request Request;

        ///// <summary>
        /////     A representation of the result to be returned to the user
        ///// </summary>
        //public Result Result;

        /// <summary>
        ///     The template node to be processed by the class
        /// </summary>
        public XmlNode Template { get; set; }

        ///// <summary>
        /////     A representation of the user who made the request
        ///// </summary>
        //public User User;

        /// <summary>
        ///  Default constructor
        /// </summary>
        protected AIMLTagHandler(XmlNode template)
        {
            Template = template;
        }

        public abstract string ProcessChange();

        //{
        //    return InputString.Length > 0 ? ProcessChange() : string.Empty;
        //}

        #region Helper methods

        /// <summary>
        ///     Helper method that turns the passed string into an XML node
        /// </summary>
        /// <param name="outerXML">the string to XMLize</param>
        /// <returns>The XML node</returns>
        public static XmlNode GetNode(string outerXML)
        {
            var temp = new XmlDocument();
            temp.LoadXml(outerXML);
            return temp.FirstChild;
        }

        #endregion
    }
}