using System;

namespace AIMLbot.Utils
{
    public static class RandomStringGenerator
    {
        /// <summary>
        ///     Utility method used to derive random strings of the specified length
        /// </summary>
        /// <param name="length">The specific string length.</param>
        /// <returns>The random string.</returns>
        public static string GetUniqueId(int length = 8)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[length];
            var random = new Random();
            for (var i = 0; i < length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }
            return new string(stringChars);
        }
    }
}