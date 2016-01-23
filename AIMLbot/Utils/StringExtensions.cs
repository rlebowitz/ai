using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AIMLbot.Utils
{
    public static class StringExtensions
    {
        public static string StripCharacters(this string input)
        {
            var clone = String.Copy(input);
            var stripper = ConfigurationManager.AppSettings.Get("stripperRegex", "[^0-9a-zA-Z]");
            return new Regex(stripper).Replace(clone, " ");
        }

        public static string[] SplitStrings(this string input)
        {
            var clone = string.Copy(input);
            var tokens = ChatBot.Splitters.ToArray();
            var rawResult = clone.Split(tokens, StringSplitOptions.RemoveEmptyEntries);
            return rawResult.Select(rawSentence => rawSentence.Trim()).Where(tidySentence => tidySentence.Length > 0).ToArray();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>The processed string</returns>
        public static string Substitute(this string target, Dictionary<string,string> dictionary)
        {
            string marker = $"{'\u0393'}{'\u0393'}";
            var result = string.Copy(target);
            foreach (var pattern in dictionary.Keys)
            {
                var safePattern = MakeRegexSafe(pattern);
                var match = @"\b" + safePattern.Trim() + @"\b";
                var replacement = marker + dictionary[pattern].Trim() + marker;
                result = Regex.Replace(result, match, replacement, RegexOptions.IgnoreCase);
            }
            return result.Replace(marker, string.Empty);
        }

        /// <summary>
        /// Escapes special Regex characters so the input can be used as part of a regex
        /// </summary>
        /// <param name="input">The raw input</param>
        /// <returns>the safe version</returns>
        private static string MakeRegexSafe(string input)
        {
            var sb = new StringBuilder(input);
            sb.Replace(@"\", "");
            sb.Replace(")", @"\)");
            sb.Replace("(", @"\(");
            sb.Replace(".", @"\.");
            return sb.ToString();
        }
    }
}