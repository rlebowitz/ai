using System;
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


        /// <summary>
        ///     Ctor
        /// </summary>

        #region Methods

        /// <summary>
        ///     Loads the AIML from files found in the ChatBot's AIMLpath into the ChatBot's brain
        /// </summary>
        public void LoadAIML()
        {
            var s = ConfigurationManager.AppSettings.Get("aimlPath", "AIML");
            var path = $@"{Environment.CurrentDirectory}\{s}";
            if (Directory.Exists(path))
            {
                LoadAIML(path);
            }
        }

        /// <summary>
        /// Loads AIML files from a directory or full path string.
        /// </summary>
        /// <param name="path">The directory containing AIML files, or the full path to a single file.</param>
        public void LoadAIML(string path)
        {
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
                var fileInfo = new FileInfo(path);
                LoadAIML(fileInfo);
            }
        }

        public void LoadAIML(FileInfo fileInfo)
        {
            if (fileInfo.Exists)
            {
                var doc = XDocument.Load(fileInfo.FullName);
                LoadAIML(doc);
            }
            else
            {
                var message = $"The file {fileInfo.FullName} cannot be found";
                throw new FileNotFoundException(message);
            }
        }

        /// <summary>
        /// Load the graphmaster from an AIML file or string stream.
        /// </summary>
        /// <param name="stream">The specified AIML stream.</param>
        public void LoadAIML(Stream stream)
        {
            var doc = XDocument.Load(stream);
            LoadAIML(doc);
        }

        /// <summary>
        ///  Load the graphmaster from an XDocument.
        /// </summary>
        /// <param name="doc">The XDocument containing the AIML.</param>
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
        /// Processes all the categories for the specific topic and add 
        /// them to the graphmaster.
        /// </summary>
        /// <param name="element">The specified topic category</param>
        private void ProcessTopic(XElement element)
        {
            // find the name of the topic or set to default "*"
            var topicName = "*";
            if (element.HasAttributes && element.Attribute("name") != null)
            {
                topicName = element.Attribute("name").Value;
            }

            // process all the category nodes
            foreach (var category in element.Descendants("category"))
            {
                ProcessCategory(category, topicName);
            }
        }

        /// <summary>
        ///  Processes and adds a category to the graphmaster structure for the specified topic.
        /// </summary>
        /// <param name="category">The specified AIML category.</param>
        /// <param name="topicName">The specified AIML topic.</param>
        private void ProcessCategory(XElement category, string topicName = "*")
        {
            var pattern = category.Descendants("pattern").FirstOrDefault();
            if (pattern == null)
            {
                throw new XmlException("Missing pattern tag in the current category");
            }

            var template = category.Descendants("template").FirstOrDefault();
            if (template == null)
            {
                var message = $"Missing template tag in the node with pattern: {string.Concat(pattern.Nodes())}";
                throw new XmlException(message);
            }

            var categoryPath = GeneratePath(category, topicName, false);
            // Add the processed AIML to the GraphMaster structure
            // Track the number of categories that have been processed
            if (categoryPath.Length > 0)
            {
                try
                {
                    ChatBot.Graphmaster.AddCategory(categoryPath, template.ToString());
                    ChatBot.Size++;
                }
                catch
                {
                    var message = $"Unable to load category {categoryPath} and template {template}";
                    Log.Error(message);
                }
            }
            else
            {
                var message = $"The category {categoryPath} had an empty pattern.";
                Log.Error(message);
            }
        }

        /// <summary>
        ///     Generates a path from a category XML category and topic name
        /// </summary>
        /// <param name="element">the category XML category</param>
        /// <param name="topicName">the topic</param>
        /// <param name="isUserInput">
        ///     marks the path to be created as originating from user input - so
        ///     normalize out the * and _ wildcards used by AIML
        /// </param>
        /// <returns>The appropriately processed path</returns>
        public string GeneratePath(XElement element, string topicName, bool isUserInput)
        {
            // get the nodes that we need
            var pattern = element.Descendants("pattern").FirstOrDefault();
            var that = element.Descendants("that").FirstOrDefault();
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

            if (ChatBot.TrustAIML & !isUserInput)
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
                if (normalizedThat.Length > ChatBot.MaxThatSize)
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
            var substitutedInput = input.Substitute(ChatBot.Substitutions);
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