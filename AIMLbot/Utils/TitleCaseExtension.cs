using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AIMLbot.Utils
{
    public static class TitleCaseExtension
    {
        /// <summary>
        /// Converts the specified string to title case.
        /// </summary>
        /// <remarks>
        /// This functionality is based off John Gruber's title case function, described at http://daringfireball.net/2008/05/title_case.
        /// 
        /// It capitalizes each word in the specified string, with the following exceptions:
        /// <list type="bullet">
        ///		<item><description>Some words are not capitalized, such as "for" and "the".</description></item>
        ///		<item><description>Words with capitalized letters other than the first, words containing dots and words containing numbers are left unchanged.</description></item>
        ///		<item><description>The first and last words are always capitalized.</description></item>
        ///		<item><description>Unix style paths (i.e. starting with a forward slash) are changed to lower case.</description></item>
        ///		<item><description>Words containing dashes or forward slashes are split and each part processed.</description></item>
        ///	</list>
        ///	</remarks>
        /// <param name="text">The string to convert to title case.</param>
        /// <returns>A string that consists of the text converted to title case.</returns>
        public static string ToTitleCase(string text)
        {
            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            // This is the array of words that should be output in lower case
            string[] lowerCaseWords = new string[] {
                "a", "an", "and", "as", "at", "but", "by", "en", "for", "if", "in",
                "nor", "of", "on", "or", "the", "to", "v", "v.", "vs", "vs.", "via" };

            // Split the title on white space
            var parts = text.Split(new[] { ' ', '\t', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            // Process each part of the title (recursively if necessary to split further)
            var b = new List<string>();
            for (var i = 0; i < parts.Length; i++)
            {
                var previousCharacter = (i > 0 ? parts[i - 1][parts[i - 1].Length - 1] : ' ');
                var processedPart = ProcessTitlePart(parts, i, previousCharacter, lowerCaseWords);
                if (!string.IsNullOrWhiteSpace(processedPart))
                {
                    b.Add(processedPart);
                }
            }

            // Re-join the title with spaces
            var result = string.Join(" ", b.ToArray());

            return result;
        }

        private static string ProcessTitlePart(string[] parts, int index, char previousCharacter, string[] lowerCaseWords)
        {
            string result = parts[index];

            if (index > 0 &&
                index < (parts.Length - 1) &&
                previousCharacter != ':' &&
                previousCharacter != '-' &&
                Array.IndexOf(lowerCaseWords, parts[index].ToLower()) != -1)
            {
                // It's a small word (but not the first or last or after a colon or dash) and should
                // be returned in lower-case
                result = parts[index].ToLower();
            }
            else if (parts[index].Length == 1)
            {
                // It's just one letter, capitalize it
                result = parts[index].ToUpper();
            }
            else if (parts[index].StartsWith("/"))
            {
                // It's a Unix style path and should be returned in lower-case
                result = parts[index].ToLower();
            }
            else if (Regex.IsMatch(parts[index], @"\w[A-Z]|\.\w|[0-9]"))
            {
                // It contains an upper-case letter (following another character), a dot (followed by
                // another character) or a number (anywhere) and should be returned as-is
                result = parts[index];
            }
            else if (parts[index].Contains("-"))
            {
                // It contains a hyphen and so each sub-part should be processed (e.g. Step-by-Step)
                result = ProcessTitleSubParts(parts[index], '-', previousCharacter, lowerCaseWords);
            }
            else if (parts[index].Contains("/"))
            {
                // It contains a forward slash and so each sub-part should be processed (e.g. Could/Should)
                result = ProcessTitleSubParts(parts[index], '/', previousCharacter, lowerCaseWords);
            }
            else
            {
                // The first letter should be capitalized (ignoring things like an opening parenthesis)
                for (int j = 0; j < parts[index].Length; j++)
                {
                    if (char.IsLetter(parts[index][j]))
                    {
                        result = (j > 0 ? parts[index].Substring(0, j) : "") + parts[index][j].ToString().ToUpper() + parts[index].Substring(j + 1);
                        break;
                    }
                }
            }

            return result;
        }

        private static string ProcessTitleSubParts(string part, char separator, char previousCharacter, string[] lowerCaseWords)
        {
            string[] subParts = part.Split(new[] { separator }, StringSplitOptions.RemoveEmptyEntries);
            for (var j = 0; j < subParts.Length; j++)
            {
                var subPreviousCharacter = j > 0 ? subParts[j - 1][subParts[j - 1].Length - 1] : previousCharacter;
                subParts[j] = ProcessTitlePart(subParts, j, subPreviousCharacter, lowerCaseWords);
            }
            return string.Join(separator.ToString(), subParts);
        }
    }
}