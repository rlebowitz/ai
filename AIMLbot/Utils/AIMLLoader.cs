using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using AIMLbot.Normalize;
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

        public PathGenerator PathGenerator { get; }

        /// <summary>
        ///  Creates an instance of the AIML loader.
        /// </summary>
        public AIMLLoader()
        {
            PathGenerator = new PathGenerator();
        }

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

            var categoryPath = PathGenerator.Generate(category, topicName, false);
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

 

        #endregion
    }
}