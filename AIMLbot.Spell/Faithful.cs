using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AIMLbot.Spell
{
    public class Faithful
    {
        private const string Alphabet = "abcdefghijklmnopqrstuvwxyz";

        private readonly Func<string, int?> _nwords;

        public Faithful()
        {
            if (!File.Exists("big.txt"))
            {
                throw new FileNotFoundException("Cannot find big.txt.");
            }
            _nwords = Train(Words(File.ReadAllText("big.txt")));
        }

        private static IEnumerable<string> Words(string text)
        {
            return Regex.Matches(text.ToLower(), "[a-z]+")
                .Cast<Match>()
                .Select(m => m.Value);
        }

        private static Func<string, int?> Train(IEnumerable<string> features)
        {
            var dict = features.GroupBy(f => f)
                .ToDictionary(g => g.Key, g => g.Count());
            return f => dict.ContainsKey(f) ? dict[f] : (int?) null;
        }

        private static IEnumerable<string> Edits1(string word)
        {
            var splits = from i in Enumerable.Range(0, word.Length)
                select new {a = word.Substring(0, i), b = word.Substring(i)};
            var deletes = from s in splits
                where s.b != ""
                // we know it can't be null
                select s.a + s.b.Substring(1);
            var transposes = from s in splits
                where s.b.Length > 1
                select s.a + s.b[1] + s.b[0] + s.b.Substring(2);
            var replaces = from s in splits
                from c in Alphabet
                where s.b != ""
                select s.a + c + s.b.Substring(1);
            var inserts = from s in splits
                from c in Alphabet
                select s.a + c + s.b;

            return deletes
                .Union(transposes) // union translates into a set
                .Union(replaces)
                .Union(inserts);
        }

        private IEnumerable<string> KnownEdits2(string word)
        {
            return (from e1 in Edits1(word)
                from e2 in Edits1(e1)
                where _nwords(e2) != null
                select e2)
                .Distinct();
        }

        private IEnumerable<string> Known(IEnumerable<string> words)
        {
            return words.Where(w => _nwords(w) != null);
        }

        private string Correct(string word)
        {
            var candidates =
                new[]
                {
                    Known(new[] {word}),
                    Known(Edits1(word)),
                    KnownEdits2(word),
                    new[] {word}
                }
                    .First(s => s.Any());

            return candidates.OrderByDescending(c => _nwords(c) ?? 1).First();
        }
    }
}