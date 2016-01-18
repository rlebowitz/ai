using System.Xml;

namespace AIMLbot.Utils
{
    /// <summary>
    ///     The template for all classes that handle the AIML tags found within template nodes of a
    ///     category.
    /// </summary>
    public abstract class AIMLTagHandler : TextTransformer
    {
        /// <summary>
        ///     A flag to denote if inner tags are to be processed recursively before processing this tag
        /// </summary>
        public bool IsRecursive = true;

        /// <summary>
        ///     A representation of the input into the ChatBot made by the user
        /// </summary>
        public Request Request;

        /// <summary>
        ///     A representation of the result to be returned to the user
        /// </summary>
        public Result Result;

        /// <summary>
        ///     The template node to be processed by the class
        /// </summary>
        public XmlNode TemplateNode;

        /// <summary>
        ///     A representation of the user who made the request
        /// </summary>
        public User User;

        /// <summary>
        ///     Ctor
        /// </summary>
        /// <param name="chatBot">The ChatBot involved in this request</param>
        /// <param name="user">The user making the request</param>
        /// <param name="query">The query that originated this node</param>
        /// <param name="request">The request itself</param>
        /// <param name="result">The result to be passed back to the user</param>
        /// <param name="templateNode">The node to be processed</param>
        protected AIMLTagHandler(ChatBot chatBot,
            User user,
            SubQuery query,
            Request request,
            Result result,
            XmlNode templateNode) : base(chatBot, templateNode.OuterXml)
        {
            User = user;
            Query = query;
            Request = request;
            Result = result;
            TemplateNode = templateNode;
            TemplateNode?.Attributes?.RemoveNamedItem("xmlns");
        }

        /// <summary>
        ///     Default ctor to use when late binding
        /// </summary>
        protected AIMLTagHandler()
        {
        }

        /// <summary>
        ///     The query that produced this node containing the wildcard matches
        /// </summary>
        public SubQuery Query { get; set; }

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