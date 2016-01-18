using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using log4net;

namespace AIMLbot.Utils
{
    /// <summary>
    ///     A utility class for loading AIML files from disk into the graphmaster structure that
    ///     forms an AIML ChatBot's "brain"
    /// </summary>
    public class AIMLLoader
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof (AIMLLoader));

        #region Attributes

        /// <summary>
        ///     The ChatBot whose brain is being processed
        /// </summary>
        private readonly ChatBot _chatBot;

        #endregion

        /// <summary>
        ///     Ctor
        /// </summary>
        /// <param name="chatBot">The ChatBot whose brain is being processed</param>
        public AIMLLoader(ChatBot chatBot)
        {
            _chatBot = chatBot;
        }

        #region Methods

        /// <summary>
        ///     Loads the AIML from files found in the ChatBot's AIMLpath into the ChatBot's brain
        /// </summary>
        public void LoadAIML()
        {
            var s = ConfigurationManager.AppSettings.Get("aimlPath", "AIML");
            if (File.Exists(s))
            {
                LoadAIML(s);
            }
        }

        /// <summary>
        ///     Loads the AIML from files found in the path
        /// </summary>
        /// <param name="path"></param>
        public void LoadAIML(string path)
        {
            path = $@"{Environment.CurrentDirectory}\{path}";
            var attr = File.GetAttributes(path);
            if (attr.HasFlag(FileAttributes.Directory) && Directory.Exists(path))
            {
                var fileEntries = Directory.GetFiles(path, "*.aiml");
                if (fileEntries.Length > 0)
                {
                    foreach (var filename in fileEntries)
                    {
                        LoadAIML(filename);
                    }
                }
                else
                {
                    var message = $"No .aiml files found in the specified path: {path}";
                    throw new FileNotFoundException(message);
                }
            }
            else
            {
                if (File.Exists(path))
                {
                    var doc = XDocument.Load(path);
                    LoadAIML(doc);
                }
                else
                {
                    var message = $"The file {path} cannot be found";
                    throw new FileNotFoundException(message);
                }
            }
        }

        /// <summary>
        ///     Load the graphmaster from the AIML stream.
        /// </summary>
        /// <param name="stream">The stream to process</param>
        public void LoadAIML(Stream stream)
        {
            var doc = XDocument.Load(stream, LoadOptions.None);
            LoadAIML(doc);
        }


        /// <summary>
        ///     Given an XML document containing valid AIML, attempts to load it into the graphmaster
        /// </summary>
        /// <param name="doc">The XML document containing the AIML</param>
        public void LoadAIML(XDocument doc)
        {
            // Get a list of the nodes that are children of the <aiml> tag
            // these nodes should only be either <topic> or <category>
            // the <topic> nodes will contain more <category> nodes
            if (doc == null) return;
            var root = doc.Root;
            if (root != null)
            {
                var topics = root.Descendants("topic").Select(topic => topic);
                foreach (var topic in topics)
                {
                    ProcessTopic(topic);
                }
            }
            var categories = doc.Descendants("category").Select(category => category);
            foreach (var category in categories)
            {
                ProcessCategory(category);
            }
        }

        /// <summary>
        ///     Given a "topic" node, processes all the categories for the topic and adds them to the
        ///     graphmaster "brain"
        /// </summary>
        /// <param name="node">The "topic" node</param>
        private void ProcessTopic(XElement node)
        {
            // find the name of the topic or set to default "*"
            var topicName = "*";
            if (node.HasAttributes && node.Attribute("name") != null)
            {
                topicName = node.Attribute("name").Value;
            }

            // process all the category nodes
            foreach (var thisNode in node.Descendants("category"))
            {
                ProcessCategory(thisNode, topicName);
            }
        }

        /// <summary>
        ///     Adds a category to the graphmaster structure using the given topic
        /// </summary>
        /// <param name="node">the XML node containing the category</param>
        /// <param name="topicName">the topic to be used</param>
        private void ProcessCategory(XElement node, string topicName = "*")
        {
            // reference and check the required nodes
            var pattern = node.Descendants("pattern").FirstOrDefault();
            var template = node.Descendants("template").FirstOrDefault();

            if (pattern == null)
            {
                throw new XmlException("Missing pattern tag in the current node");
            }
            if (template == null)
            {
                var message = $"Missing template tag in the node with pattern: {string.Concat(pattern.Nodes())}";
                throw new XmlException(message);
            }

            var categoryPath = GeneratePath(node, topicName, false);
            // o.k., add the processed AIML to the GraphMaster structure
            if (categoryPath.Length > 0)
            {
                try
                {
                    _chatBot.Graphmaster.AddCategory(categoryPath, template.ToString());
                    // keep count of the number of categories that have been processed
                    _chatBot.Size++;
                }
                catch
                {
                    var message = $"Unable to load category {categoryPath} and template {template}";
                    Log.Error(message);
                }
            }
            else
            {
                var message = $"Unable to load category with an empty pattern with path {categoryPath}";
                Log.Error(message);
            }
        }

        /// <summary>
        ///     Generates a path from a category XML node and topic name
        /// </summary>
        /// <param name="node">the category XML node</param>
        /// <param name="topicName">the topic</param>
        /// <param name="isUserInput">
        ///     marks the path to be created as originating from user input - so
        ///     normalize out the * and _ wildcards used by AIML
        /// </param>
        /// <returns>The appropriately processed path</returns>
        public string GeneratePath(XElement node, string topicName, bool isUserInput)
        {
            // get the nodes that we need
            var pattern = node.Descendants("pattern").FirstOrDefault();
            var that = node.Descendants("that").FirstOrDefault();
            var thatText = "*";
            var patternText = pattern == null ? string.Empty : string.Concat(pattern.Nodes());
            if (that != null)
            {
                thatText = string.Concat(that.Nodes());
            }
            return GeneratePath(patternText, thatText, topicName, isUserInput);
        }

        /// <summary>
        ///     Generates a path from the passed arguments
        /// </summary>
        /// <param name="pattern">the pattern</param>
        /// <param name="that">the that</param>
        /// <param name="topicName">the topic</param>
        /// <param name="isUserInput">
        ///     marks the path to be created as originating from user input - so
        ///     normalize out the * and _ wildcards used by AIML
        /// </param>
        /// <returns>The appropriately processed path</returns>
        public string GeneratePath(string pattern, string that, string topicName, bool isUserInput)
        {
            // to hold the normalized path to be entered into the graphmaster
            var normalizedPath = new StringBuilder();
            string normalizedPattern;
            string normalizedThat;
            string normalizedTopic;

            if (_chatBot.TrustAIML & !isUserInput)
            {
                normalizedPattern = pattern.Trim();
                normalizedThat = that.Trim();
                normalizedTopic = topicName.Trim();
            }
            else
            {
                normalizedPattern = Normalize(pattern, isUserInput).Trim();
                normalizedThat = Normalize(that, isUserInput).Trim();
                normalizedTopic = Normalize(topicName, isUserInput).Trim();
            }

            // check sizes
            if (normalizedPattern.Length > 0)
            {
                if (normalizedThat.Length == 0)
                {
                    normalizedThat = "*";
                }
                if (normalizedTopic.Length == 0)
                {
                    normalizedTopic = "*";
                }

                // This check is in place to avoid huge "that" elements having to be processed by the 
                // graphmaster. 
                if (normalizedThat.Length > _chatBot.MaxThatSize)
                {
                    normalizedThat = "*";
                }

                // o.k. build the path
                normalizedPath.Append(normalizedPattern);
                normalizedPath.Append(" <that> ");
                normalizedPath.Append(normalizedThat);
                normalizedPath.Append(" <topic> ");
                normalizedPath.Append(normalizedTopic);

                return normalizedPath.ToString();
            }
            return string.Empty;
        }

        /// <summary>
        ///     Given an input, provide a normalized output
        /// </summary>
        /// <param name="input">The string to be normalized</param>
        /// <param name="isUserInput">
        ///     True if the string being normalized is part of the user input path -
        ///     flags that we need to normalize out * and _ chars
        /// </param>
        /// <returns>The normalized string</returns>
        public string Normalize(string input, bool isUserInput)
        {
            var builder = new StringBuilder();

            // objects for normalization of the input
//            StripIllegalCharacters stripper = new StripIllegalCharacters(_chatBot);
            var substitutions = ConfigurationManager.GetSection("Substitutions") as Dictionary<string, string>;
            var substitutedInput = input.Substitute(substitutions);
            // split the pattern into it's component words
            var substitutedWords = substitutedInput.Split(" \r\n\t".ToCharArray());

            // Normalize all words unless they're the AIML wildcards "*" and "_" during AIML loading
            foreach (var word in substitutedWords)
            {
                string normalizedWord;
                if (isUserInput)
                {
                    normalizedWord = word.StripCharacters();
                }
                else
                {
                    if ((word == "*") || (word == "_"))
                    {
                        normalizedWord = word;
                    }
                    else
                    {
                        normalizedWord = word.StripCharacters();
                    }
                }
                builder.Append(normalizedWord.Trim() + " ");
            }

            return builder.ToString().Replace("  ", " "); // make sure the whitespace is neat
        }

        #endregion
    }
}