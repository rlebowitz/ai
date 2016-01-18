using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text.RegularExpressions;

namespace AIMLbot.Utils
{
    public static class StringExtensions
    {
        public static string StripCharacters(this string input)
        {
            var regex = ConfigurationManager.AppSettings.Get("stripperRegex", new Regex("[^0-9a-zA-Z]"));
            return regex.Replace(input, " ");
        }

        public static string[] SplitStrings(this string input)
        {
            var tokens = ConfigurationManager.GetSection("Splitters") as string[];
            var rawResult = input.Split(tokens, StringSplitOptions.RemoveEmptyEntries);
            return rawResult.Select(rawSentence => rawSentence.Trim()).Where(tidySentence => tidySentence.Length > 0).ToArray();
        }

        /// <returns>The processed string</returns>
        public static string Substitute(this string target, Dictionary<string,string> dictionary)
        {
            const string marker = "\u10381u10381"; //getMarker(5);
            var result = target;
            foreach (var pattern in dictionary.Keys)
            {
                var p2 = MakeRegexSafe(pattern);
                //string match = "\\b"+@p2.Trim().Replace(" ","\\s*")+"\\b";
                var match = "\\b" + p2.TrimEnd().TrimStart() + "\\b";
                var replacement = marker + dictionary[pattern].Trim() + marker;
                result = Regex.Replace(result, match, replacement, RegexOptions.IgnoreCase);
            }
            return result.Replace(marker, "");
        }

        /// <summary>
        ///     Given an input, escapes certain characters so they can be used as part of a regex
        /// </summary>
        /// <param name="input">The raw input</param>
        /// <returns>the safe version</returns>
        private static string MakeRegexSafe(string input)
        {
            var result = input.Replace("\\", "").Replace(")", "\\)").Replace("(", "\\(").Replace(".", "\\.");
            return result;
        }
    }
}