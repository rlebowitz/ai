using System.Collections.Generic;

namespace AIMLbot.Utils
{
    public static class DictionaryExtensions
    {
        public static Dictionary<string, string> Clone(this Dictionary<string, string> original)
        {
            var dictionary = new Dictionary<string, string>(original.Count, original.Comparer);
            foreach (var entry in original)
            {
                dictionary.Add((string) entry.Key.Clone(), (string) entry.Value.Clone());
            }
            return dictionary;
        }

        public static void AddOrReplace(this Dictionary<string, string> original, string key, string value)
        {
            if (!original.ContainsKey(key))
            {
                original.Add(key, value);
                return;
            }
            original[key] = value;
        }
    }
}