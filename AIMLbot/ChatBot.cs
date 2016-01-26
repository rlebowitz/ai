using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using AIMLbot.AIMLTagHandlers;
using AIMLbot.Utils;
using log4net;
using Gender = AIMLbot.Utils.Gender;
using Random = AIMLbot.AIMLTagHandlers.Random;
using Version = AIMLbot.AIMLTagHandlers.Version;

namespace AIMLbot
{
    /// <summary>
    ///     Encapsulates a ChatBot. If no settings.xml file is found or referenced the ChatBot will try to
    ///     default to safe settings.
    /// </summary>
    public class ChatBot
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof (ChatBot));

        public ChatBot()
        {
            Graphmaster = new Node();
        }

        #region Attributes

        /// <summary>
        ///     The "brain" of the ChatBot
        /// </summary>
        public Node Graphmaster;

        /// <summary>
        ///     Flag to show if the ChatBot is willing to accept user input
        /// </summary>
        public bool IsAcceptingUserInput { get; set; } = true;

        /// <summary>
        ///     The maximum number of characters a "that" element of a path is allowed to be. Anything above
        ///     this length will cause "that" to be "*". This is to avoid having the graphmaster process
        ///     huge "that" elements in the path that might have been caused by the ChatBot reporting third party
        ///     data.
        /// </summary>
        public int MaxThatSize = 256;

       public static Dictionary<string,string> Genders = ConfigurationManager.GetSection("Gender") as Dictionary<string, string>;
        /// <summary>
        ///     A dictionary of all the first person to second person (and back) substitutions
        /// </summary>
        public static Dictionary<string, string> Person2 = ConfigurationManager.GetSection("Person2") as Dictionary<string, string>;

        /// <summary>
        ///     A dictionary of first / third person substitutions
        /// </summary>
        public static Dictionary<string, string> Person = ConfigurationManager.GetSection("Person") as Dictionary<string, string>;

        public static Dictionary<string,string> Predicates = ConfigurationManager.GetSection("Predicates") as Dictionary<string, string>;

        /// <summary>
        ///     The number of categories this ChatBot has in its graphmaster "brain"
        /// </summary>
        public int Size;

        /// <summary>
        ///     A List containing the tokens used to split the input into sentences during the
        ///     normalization process
        /// </summary>
        public static List<string> Splitters = ConfigurationManager.GetSection("Splitters") as List<string>;

        /// <summary>
        ///     When the ChatBot was initialised
        /// </summary>
        public DateTime StartedOn = DateTime.Now;

        /// <summary>
        ///     Generic substitutions that take place during the normalization process
        /// </summary>
        public static Dictionary<string, string> Substitutions = ConfigurationManager.GetSection("Gender") as Dictionary<string, string>;

        /// <summary>
        ///     If set to false the input from AIML files will undergo the same normalization process that
        ///     user input goes through. If true the ChatBot will assume the AIML is correct. Defaults to true.
        /// </summary>
        public bool TrustAIML = true;

        /// <summary>
        ///     The message to show if a user tries to use the ChatBot whilst it is set to not process user input
        /// </summary>
        private readonly string _notAcceptMessage = ConfigurationManager.AppSettings.Get("acceptInput",
            "This ChatBot is currently set to not accept user input.");

        /// <summary>
        ///     The maximum amount of time a request should take (in milliseconds)
        /// </summary>
        public double TimeOut = ConfigurationManager.AppSettings.Get("timeoutMax", 30000.0);

        /// <summary>
        ///     The message to display in the event of a timeout
        /// </summary>
        public string TimeOutMessage = ConfigurationManager.AppSettings.Get("timeoutMessage",
            "ERROR: The request has timed out.");

        /// <summary>
        ///     The locale of the ChatBot as a CultureInfo object
        /// </summary>
        public CultureInfo Locale = ConfigurationManager.AppSettings.Get("culture", new CultureInfo("en-US"));

        /// <summary>
        ///     Will match all the illegal characters that might be inputted by the user
        /// </summary>
        public Regex Strippers = ConfigurationManager.AppSettings.Get("stripperregex",
            new Regex("[^0-9a-zA-Z]", RegexOptions.IgnorePatternWhitespace));

        /// <summary>
        ///     Flag to denote if the ChatBot is writing messages to its logs
        /// </summary>
        public bool IsLogging = ConfigurationManager.AppSettings.Get("islogging", false);

        /// <summary>
        ///     The supposed sex of the ChatBot
        /// </summary>
        public Gender Sex
        {
            get
            {
                var sex = ConfigurationManager.AppSettings.Get("Gender", -1);
                Gender result;
                switch (sex)
                {
                    case 0:
                        result = Gender.Female;
                        break;
                    case 1:
                        result = Gender.Male;
                        break;
                    default:
                        result = Gender.Unknown;
                        break;
                }
                return result;
            }
        }

        /// <summary>
        ///     The directory to look in for the AIML files
        /// </summary>
        public string PathToAIML
            => Path.Combine(Environment.CurrentDirectory, ConfigurationManager.AppSettings.Get("aimldirectory", "Aiml"))
            ;

        #endregion

        #region Settings methods

        /// <summary>
        ///     Loads AIML from .aiml files into the graphmaster "brain" of the ChatBot
        /// </summary>
        public void LoadAIML()
        {
            var loader = new AIMLLoader(this);
            loader.LoadAIML();
        }

        public void LoadAIML(string path)
        {
            var loader = new AIMLLoader(this);
            loader.LoadAIML(path);
        }

        /// <summary>
        ///     Allows the ChatBot to load a new XML version of some AIML
        /// </summary>
        /// <param name="newAIML">The XML document containing the AIML</param>
        public void LoadAIML(XDocument newAIML)
        {
            var loader = new AIMLLoader(this);
            loader.LoadAIML(newAIML);
        }


        #endregion

        #region Conversation methods

        /// <summary>
        ///     Given some raw input and a unique ID creates a response for a new user
        /// </summary>
        /// <param name="rawInput">the raw input</param>
        /// <param name="userGuid">an ID for the new user (referenced in the result object)</param>
        /// <returns>the result to be output to the user</returns>
        public Result Chat(string rawInput, string userGuid)
        {
            var request = new Request(rawInput, new User(userGuid, this), this);
            return Chat(request);
        }

        /// <summary>
        ///     Given a request containing user input, produces a result from the ChatBot
        /// </summary>
        /// <param name="request">the request from the user</param>
        /// <returns>the result to be output to the user</returns>
        public Result Chat(Request request)
        {
            var result = new Result(request.User, this, request);

            if (IsAcceptingUserInput)
            {
                // Normalize the input
                var loader = new AIMLLoader(this);
                var rawSentences = request.RawInput.SplitStrings();
                foreach (var sentence in rawSentences)
                {
                    result.InputSentences.Add(sentence);
                    var path =
                        loader.GeneratePath(sentence, request.User.GetLastBotOutput(), request.User.Topic, true);
                    result.NormalizedPaths.Add(path);
                }

                // grab the templates for the various sentences from the graphmaster
                foreach (var path in result.NormalizedPaths)
                {
                    var query = new SubQuery(path);
                    var searcher = new NodeSearcher(query, request);
                    query.Template = searcher.Evaluate(Graphmaster, path, MatchState.UserInput, new StringBuilder());
//                        Graphmaster.Evaluate(path, query, request, MatchState.UserInput, new StringBuilder());
                    result.SubQueries.Add(query);
                }

                // process the templates into appropriate output
                foreach (var query in result.SubQueries)
                {
                    if (query.Template.Length > 0)
                    {
                        try
                        {
                            var templateNode = AIMLTagHandler.GetNode(query.Template);
                            var outputSentence = ProcessNode(templateNode, query, request, result, request.User);
                            if (outputSentence.Length > 0)
                            {
                                result.OutputSentences.Add(outputSentence);
                            }
                        }
                        catch (Exception ex)
                        {
                            Log.Error("WARNING! A problem was encountered when trying to process the input: " +
                                      request.RawInput + " with the template: \"" + query.Template + "\"", ex);
                        }
                    }
                }
            }
            else
            {
                result.OutputSentences.Add(_notAcceptMessage);
            }

            // populate the Result object
            result.Duration = DateTime.Now - request.StartedOn;
            request.User.AddResult(result);

            return result;
        }

        /// <summary>
        ///     Recursively evaluates the template nodes returned from the ChatBot
        /// </summary>
        /// <param name="node">the node to evaluate</param>
        /// <param name="query">the query that produced this node</param>
        /// <param name="request">the request from the user</param>
        /// <param name="result">the result to be sent to the user</param>
        /// <param name="user">the user who originated the request</param>
        /// <returns>the output string</returns>
        private string ProcessNode(XmlNode node, SubQuery query, Request request, Result result, User user)
        {
            // check for timeout (to avoid infinite loops)
            if (request.StartedOn.AddMilliseconds(request.ChatBot.TimeOut) < DateTime.Now)
            {
                Log.Error("WARNING! Request timeout. User: " + request.User.UserId + " raw input: \"" +
                          request.RawInput + "\" processing template: \"" + query.Template + "\"");
                request.HasTimedOut = true;
                return string.Empty;
            }

            // process the node
            var tagName = node.Name.ToLower();
            if (tagName == "template")
            {
                var templateResult = new StringBuilder();
                if (node.HasChildNodes)
                {
                    // recursively check
                    foreach (XmlNode childNode in node.ChildNodes)
                    {
                        templateResult.Append(ProcessNode(childNode, query, request, result, user));
                    }
                }
                return templateResult.ToString();
            }
            AIMLTagHandler tagHandler = null;
            switch (tagName)
                {
                    case "bot":
                        tagHandler = new Bot(this, user, query, request, result, node);
                        break;
                    case "condition":
                        tagHandler = new Condition(this, user, query, request, result, node);
                        break;
                    case "date":
                        tagHandler = new Date(this, user, query, request, result, node);
                        break;
                    case "formal":
                        tagHandler = new Formal(this, user, query, request, result, node);
                        break;
                    case "gender":
                        tagHandler = new AIMLTagHandlers.Gender(this, user, query, request, result, node);
                        break;
                    case "get":
                        tagHandler = new Get(this, user, query, request, result, node);
                        break;
                    case "gossip":
                        tagHandler = new Gossip(this, user, query, request, result, node);
                        break;
                    case "id":
                        tagHandler = new Id(this, user, query, request, result, node);
                        break;
                    case "input":
                        tagHandler = new Input(this, user, query, request, result, node);
                        break;
                    case "javascript":
                        tagHandler = new Javascript(this, user, query, request, result, node);
                        break;
                    case "learn":
                        tagHandler = new Learn(this, user, query, request, result, node);
                        break;
                    case "lowercase":
                        tagHandler = new Lowercase(this, user, query, request, result, node);
                        break;
                    case "person":
                        tagHandler = new Person(this, user, query, request, result, node);
                        break;
                    case "person2":
                        tagHandler = new Person2(this, user, query, request, result, node);
                        break;
                    case "random":
                        tagHandler = new Random(this, user, query, request, result, node);
                        break;
                    case "sentence":
                        tagHandler = new Sentence(this, user, query, request, result, node);
                        break;
                    case "set":
                        tagHandler = new Set(this, user, query, request, result, node);
                        break;
                    case "size":
                        tagHandler = new Size(this, user, query, request, result, node);
                        break;
                    case "sr":
                        tagHandler = new Sr(this, user, query, request, result, node);
                        break;
                    case "srai":
                        tagHandler = new Srai(this, user, query, request, result, node);
                        break;
                    case "star":
                        tagHandler = new Star(this, user, query, request, result, node);
                        break;
                    case "system":
                        tagHandler = new SystemTag(this, user, query, request, result, node);
                        break;
                    case "that":
                        tagHandler = new That(this, user, query, request, result, node);
                        break;
                    case "thatstar":
                        tagHandler = new ThatStar(this, user, query, request, result, node);
                        break;
                    case "think":
                        tagHandler = new Think(this, user, query, request, result, node);
                        break;
                    case "topicstar":
                        tagHandler = new Topicstar(this, user, query, request, result, node);
                        break;
                    case "uppercase":
                        tagHandler = new Uppercase(this, user, query, request, result, node);
                        break;
                    case "version":
                        tagHandler = new Version(this, user, query, request, result, node);
                        break;
                    default:
                        Log.ErrorFormat("Unknown AIML tag: {0}", tagName);
                        break;
                }
            if (Equals(null, tagHandler))
            {
                return node.InnerText;
            }
            if (tagHandler.IsRecursive)
            {
                if (node.HasChildNodes)
                {
                    // recursively check
                    foreach (XmlNode childNode in node.ChildNodes)
                    {
                        if (childNode.NodeType != XmlNodeType.Text)
                        {
                            childNode.InnerXml = ProcessNode(childNode, query, request, result, user);
                        }
                    }
                }
                return tagHandler.Transform();
            }
            var resultNodeInnerXML = tagHandler.Transform();
            var resultNode = AIMLTagHandler.GetNode("<node>" + resultNodeInnerXML + "</node>");
            if (resultNode.HasChildNodes)
            {
                var recursiveResult = new StringBuilder();
                // recursively check
                foreach (XmlNode childNode in resultNode.ChildNodes)
                {
                    recursiveResult.Append(ProcessNode(childNode, query, request, result, user));
                }
                return recursiveResult.ToString();
            }
            return resultNode.InnerXml;
        }

        #endregion

        #region Serialization

        /// <summary>
        ///     Saves the graphmaster node (and children) to a binary file to avoid processing the AIML each time the
        ///     ChatBot starts
        /// </summary>
        public void SaveToBinaryFile()
        {
            var path = ConfigurationManager.AppSettings.Get("graphMasterFile", "GraphMaster.dat");
            var fullPath = $@"{Environment.CurrentDirectory}\{path}";
            SaveToBinaryFile(new FileInfo(fullPath));
        }

        /// <summary>
        ///     Saves the graphmaster node (and children) to a binary file to avoid processing the AIML each time the
        ///     ChatBot starts
        /// </summary>
        public void SaveToBinaryFile(FileInfo fileInfo)
        {
            if (fileInfo.Exists)
            {
                fileInfo.Delete();
            }
            using (var stream = fileInfo.Create())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, Graphmaster);
            }
        }

        /// <summary>
        ///     Loads a dump of the graphmaster into memory so avoiding processing the AIML files again
        /// </summary>
        public void LoadFromBinaryFile()
        {
            var path = ConfigurationManager.AppSettings.Get("graphMasterFile", "GraphMaster.dat");
            var fullPath = $@"{Environment.CurrentDirectory}\{path}";
            LoadFromBinaryFile(new FileInfo(fullPath));
        }

        /// <summary>
        ///     Loads a dump of the graphmaster into memory so avoiding processing the AIML files again
        /// </summary>
        /// <param name="fileInfo">The specific file to load.</param>
        public void LoadFromBinaryFile(FileInfo fileInfo)
        {
            if (fileInfo != null && fileInfo.Exists)
            {
                using (var stream = fileInfo.OpenRead())
                {
                    var formatter = new BinaryFormatter();
                    Graphmaster = (Node) formatter.Deserialize(stream);
                }
            }
            else
            {
                throw new SerializationException("Unable to deserialize the AIML Graph.");
            }
        }

        #endregion
    }
}