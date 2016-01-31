using System.Linq;
using System.Text;
using System.Xml.Linq;
using AIMLbot.Utils;

namespace AIMLbot.Normalize
{
    public class PathGenerator
    {

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
        public string Generate(XElement element, string topicName, bool isUserInput)
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
            return Generate(patternText, thatText, topicName, isUserInput);
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
        public string Generate(string pattern, string that, string topicName, bool isUserInput)
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
    }
}