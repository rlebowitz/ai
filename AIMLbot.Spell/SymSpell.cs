// SymSpell: 1 million times faster through Symmetric Delete spelling correction algorithm
//
// The Symmetric Delete spelling correction algorithm reduces the complexity of edit candidate generation and dictionary lookup 
// for a given Damerau-Levenshtein distance. It is six orders of magnitude faster and language independent.
// Opposite to other algorithms only deletes are required, no transposes + replaces + inserts.
// Transposes + replaces + inserts of the input term are transformed into deletes of the dictionary term.
// Replaces and inserts are expensive and language dependent: e.g. Chinese has 70,000 Unicode Han characters!
//
// Copyright (C) 2015 Wolf Garbe
// Version: 3.0
// Author: Wolf Garbe <wolf.garbe@faroo.com>
// Maintainer: Wolf Garbe <wolf.garbe@faroo.com>
// URL: http://blog.faroo.com/2012/06/07/improved-edit-distance-based-spelling-correction/
// Description: http://blog.faroo.com/2012/06/07/improved-edit-distance-based-spelling-correction/
//
// License:
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License, 
// version 3.0 (LGPL-3.0) as published by the Free Software Foundation.
// http://www.opensource.org/licenses/LGPL-3.0
//
// Usage: single word + Enter:  Display spelling suggestions
//        Enter without input:  Terminate the program

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AIMLbot.Spell
{
    public class DictionaryItem
    {
        public readonly List<int> Suggestions = new List<int>();
        public int Count;
    }

    public class SuggestItem  {
        public SuggestItem(string term, int count, int distance)
        {
            Term = term ?? "";
            Distance = distance;
            Count = count;
        }

        public string Term { get; }

        public int Distance { get; set; }
        public int Count { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;
            var item = (SuggestItem) obj;
            return Term.Equals(item.Term);
        }

        public override int GetHashCode()
        {
            return Term.GetHashCode();
        }
    }

    public class SymSpell
    {
        private const int EditDistanceMax = 2;
        private int verbose = 0;
        //0: top suggestion
        //1: all suggestions of smallest edit distance 
        //2: all suggestions <= editDistanceMax (slower, no early termination)
        public SymSpell()
        {
            Dictionary = new Dictionary<string, object>(); 
        }


        //Dictionary that contains both the original words and the deletes derived from them. A term might be both word and delete from another word at the same time.
        //For space reduction a item might be either of type dictionaryItem or Int. 
        //A dictionaryItem is used for word, word/delete, and delete with multiple suggestions. Int is used for deletes with a single suggestion (the majority of entries).

        //List of unique words. By using the suggestions (Int) as index for this list they are translated into the original string.
        public List<string> Wordlist { get; } = new List<string>();

        //create a non-unique wordlist from sample text
        //language independent (e.g. works with Chinese characters)
        private static IEnumerable<string> ParseWords(string text)
        {
            // \w Alphanumeric characters (including non-latin characters, umlaut characters and digits) plus "_" 
            // \d Digits
            // Provides identical results to Norvigs regex "[a-z]+" for latin characters, while additionally providing compatibility with non-latin characters
            return Regex.Matches(text.ToLower(), @"[\w-[\d_]]+")
                .Cast<Match>()
                .Select(m => m.Value);
        }

        //maximum dictionary term length
        public int Maxlength;

        public Dictionary<string, object> Dictionary { get; set; }

        //for every word there all deletes with an edit distance of 1..editDistanceMax created and added to the dictionary
        //every delete entry has a suggestions list, which points to the original term(s) it was created from
        //The dictionary may be dynamically updated (word frequency and new words) at any time by calling createDictionaryEntry
        private bool CreateDictionaryEntry(string key)
        {
            bool result = false;
            DictionaryItem value = null;
            object valueo;
            if (Dictionary.TryGetValue(key, out valueo))
            {
                //int or dictionaryItem? delete existed before word!
                if (valueo is int)
                {
                    var tmp = (int)valueo;
                    value = new DictionaryItem();
                    value.Suggestions.Add(tmp);
                    Dictionary[key] = value;
                }

                //already exists:
                //1. word appears several times
                //2. word1==deletes(word2) 
                else
                {
                    value = (valueo as DictionaryItem);
                }
                //prevent overflow
                if (value != null && value.Count < int.MaxValue)
                {
                    value.Count++;
                }
            }
            else if (Wordlist.Count < int.MaxValue)
            {
                value = new DictionaryItem();
                value.Count++;
                Dictionary.Add(key, value);

                if (key.Length > Maxlength) Maxlength = key.Length;
            }


            //edits/suggestions are created only once, no matter how often word occurs
            //edits/suggestions are created only as soon as the word occurs in the corpus, 
            //even if the same term existed before in the dictionary as an edit from another word
            //a treshold might be specifid, when a term occurs so frequently in the corpus that it is considered a valid word for spelling correction
            if (value != null && value.Count == 1)
            {
                //word2index
                Wordlist.Add(key);
                if (key.Equals("I", StringComparison.InvariantCultureIgnoreCase))
                {
                    Console.WriteLine(key);
                }
                var keyint = Wordlist.Count - 1;

                result = true;

                //create deletes
                foreach (var delete in Edits(key, 0, new HashSet<string>()))
                {
                    object value2;
                    if (Dictionary.TryGetValue(delete, out value2))
                    {
                        //already exists:
                        //1. word1==deletes(word2) 
                        //2. deletes(word1)==deletes(word2) 
                        //int or dictionaryItem? single delete existed before!
                        if (value2 is int)
                        {
                            //transformes int to dictionaryItem
                            var tmp = (int)value2;
                            DictionaryItem di = new DictionaryItem();
                            di.Suggestions.Add(tmp);
                            Dictionary[delete] = di;
                            if (!di.Suggestions.Contains(keyint))
                            {
                                AddLowestDistance(di, key, keyint, delete);
                            }
                        }
                        else
                        {
                            var dictionaryItem = value2 as DictionaryItem;
                            if (dictionaryItem != null && !dictionaryItem.Suggestions.Contains(keyint))
                            {
                                AddLowestDistance((DictionaryItem) value2, key, keyint, delete);
                            }
                        }
                    }
                    else
                    {
                        Dictionary.Add(delete, keyint);
                    }
                }
            }
            return result;
        }

        //create a frequency dictionary from a corpus
        public void CreateDictionary(string corpus)
        {
            if (!File.Exists(corpus))
            {
                throw new FileNotFoundException("File not found: " + corpus);
            }

            long wordCount = 0;

            using (var sr = new StreamReader(corpus))
            {
                string line;
                //process a single line at a time only for memory efficiency
                while ((line = sr.ReadLine()) != null)
                {
                    wordCount += ParseWords(line).LongCount(CreateDictionaryEntry);
                }
            }

            Wordlist.TrimExcess();
            var count = wordCount.ToString("N0");
            var entries = Dictionary.Count.ToString("N0");
            var mb = (Process.GetCurrentProcess().PrivateMemorySize64/1000000).ToString("N0");
            var s = $@"Dictionary: {count} words, {entries} entries, edit distance={EditDistanceMax} {mb} MB";
            Console.WriteLine(s);
        }

        //save some time and space
        private void AddLowestDistance(DictionaryItem item, string suggestion, int suggestionint, string delete)
        {
            //remove all existing suggestions of higher distance, if verbose<2
            //index2word
            if ((verbose < 2) && (item.Suggestions.Count > 0) && (Wordlist[item.Suggestions[0]].Length - delete.Length > suggestion.Length - delete.Length)) item.Suggestions.Clear();
            //do not add suggestion of higher distance than existing, if verbose<2
            if ((verbose == 2) || (item.Suggestions.Count == 0) || (Wordlist[item.Suggestions[0]].Length - delete.Length >= suggestion.Length - delete.Length)) item.Suggestions.Add(suggestionint);
        }

        //inexpensive and language independent: only deletes, no transposes + replaces + inserts
        //replaces and inserts are expensive and language dependent (Chinese has 70,000 Unicode Han characters)
        private static HashSet<string> Edits(string word, int editDistance, HashSet<string> deletes)
        {
            editDistance++;
            if (word.Length > 1)
            {
                for (var i = 0; i < word.Length; i++)
                {
                    var delete = word.Remove(i, 1);
                    if (deletes.Add(delete))
                    {
                        //recursion, if maximum edit distance not yet reached
                        if (editDistance < EditDistanceMax) Edits(delete, editDistance, deletes);
                    }
                }
            }
            return deletes;
        }

        private List<SuggestItem> Lookup(string input, int editDistanceMax)
        {
            //save some time
            if (input.Length - editDistanceMax > Maxlength) return new List<SuggestItem>();

            var candidates = new List<string>();
            var hashset1 = new HashSet<string>();

            var suggestions = new List<SuggestItem>();
            var hashset2 = new HashSet<string>();

            //add original term
            candidates.Add(input);

            while (candidates.Count > 0)
            {
                var candidate = candidates[0];
                candidates.RemoveAt(0);

                //save some time
                //early termination
                //suggestion distance=candidate.distance... candidate.distance+editDistanceMax                
                //if canddate distance is already higher than suggestion distance, than there are no better suggestions to be expected
                if ((verbose < 2) && (suggestions.Count > 0) && (input.Length - candidate.Length > suggestions[0].Distance)) goto sort;


                //read candidate entry from dictionary
                object valueo;
                if (Dictionary.TryGetValue(candidate, out valueo))
                {
                    var value = new DictionaryItem();
                    if (valueo is int) value.Suggestions.Add((int)valueo); else value = (DictionaryItem)valueo;

                    //if count>0 then candidate entry is correct dictionary term, not only delete item
                    if ((value.Count > 0) && hashset2.Add(candidate))
                    {
                        //add correct dictionary term term to suggestion list
                        var si = new SuggestItem(candidate, value.Count, input.Length - candidate.Length);
                        suggestions.Add(si);
                        //early termination
                        if ((verbose < 2) && (input.Length - candidate.Length == 0)) goto sort;
                    }

                    //iterate through suggestions (to other correct dictionary items) of delete item and add them to suggestion list
                    foreach (int suggestionint in value.Suggestions)
                    {
                        //save some time 
                        //skipping double items early: different deletes of the input term can lead to the same suggestion
                        //index2word
                        string suggestion = Wordlist[suggestionint];
                        if (hashset2.Add(suggestion))
                        {
                            //True Damerau-Levenshtein Edit Distance: adjust distance, if both distances>0
                            //We allow simultaneous edits (deletes) of editDistanceMax on on both the dictionary and the input term. 
                            //For replaces and adjacent transposes the resulting edit distance stays <= editDistanceMax.
                            //For inserts and deletes the resulting edit distance might exceed editDistanceMax.
                            //To prevent suggestions of a higher edit distance, we need to calculate the resulting edit distance, if there are simultaneous edits on both sides.
                            //Example: (bank==bnak and bank==bink, but bank!=kanb and bank!=xban and bank!=baxn for editDistanceMaxe=1)
                            //Two deletes on each side of a pair makes them all equal, but the first two pairs have edit distance=1, the others edit distance=2.
                            int distance = 0;
                            if (suggestion != input)
                            {
                                if (suggestion.Length == candidate.Length) distance = input.Length - candidate.Length;
                                else if (input.Length == candidate.Length) distance = suggestion.Length - candidate.Length;
                                else
                                {
                                    //common prefixes and suffixes are ignored, because this speeds up the Damerau-levenshtein-Distance calculation without changing it.
                                    int ii = 0;
                                    int jj = 0;
                                    while ((ii < suggestion.Length) && (ii < input.Length) && (suggestion[ii] == input[ii])) ii++;
                                    while ((jj < suggestion.Length - ii) && (jj < input.Length - ii) && (suggestion[suggestion.Length - jj - 1] == input[input.Length - jj - 1])) jj++;
                                    if ((ii > 0) || (jj > 0)) { distance = DamerauLevenshteinDistance(suggestion.Substring(ii, suggestion.Length - ii - jj), input.Substring(ii, input.Length - ii - jj)); } else distance = DamerauLevenshteinDistance(suggestion, input);

                                }
                            }

                            //save some time.
                            //remove all existing suggestions of higher distance, if verbose<2
                            if ((verbose < 2) && (suggestions.Count > 0) && (suggestions[0].Distance > distance)) suggestions.Clear();
                            //do not process higher distances than those already found, if verbose<2
                            if ((verbose < 2) && (suggestions.Count > 0) && (distance > suggestions[0].Distance)) continue;

                            if (distance > editDistanceMax) continue;
                            object value2;
                            if (!Dictionary.TryGetValue(suggestion, out value2)) continue;
                            if (value2 == null) continue;
                            var item = (DictionaryItem)value2;
                            var si = new SuggestItem(suggestion, item.Count, distance);
                            suggestions.Add(si);
                        }
                    }//end foreach
                }//end if         

                //add edits 
                //derive edits (deletes) from candidate (input) and add them to candidates list
                //this is a recursive process until the maximum edit distance has been reached
                if (input.Length - candidate.Length < editDistanceMax)
                {
                    //save some time
                    //do not create edits with edit distance smaller than suggestions already found
                    if ((verbose < 2) && (suggestions.Count > 0) && (input.Length - candidate.Length >= suggestions[0].Distance)) continue;

                    candidates.AddRange(candidate.Select((t, i) => candidate.Remove(i, 1)).Where(delete => hashset1.Add(delete)));
                }
            }//end while

            //sort by ascending edit distance, then by descending word frequency
            sort: if (verbose < 2) suggestions.Sort((x, y) => -x.Count.CompareTo(y.Count)); else suggestions.Sort((x, y) => 2 * x.Distance.CompareTo(y.Distance) - x.Count.CompareTo(y.Count));
            if ((verbose == 0) && (suggestions.Count > 1)) return suggestions.GetRange(0, 1);
            return suggestions;
        }

        public SuggestItem Correct(string input)
        {
            //check in dictionary for existence and frequency; sort by ascending edit distance, then by descending word frequency
            var suggestions = Lookup(input, EditDistanceMax);

            //display term and frequency
            foreach (var suggestion in suggestions)
            {
                var s = $@"{suggestion.Term} {suggestion.Distance} {suggestion.Count}";
                Console.WriteLine(s);
            }
            if (verbose != 0) Console.WriteLine($@"{suggestions.Count} suggestions");
            if (suggestions.Count > 0)
            {
                return suggestions[0];
            }
            return null;
        }

        // Damerau–Levenshtein distance algorithm and code 
        // from http://en.wikipedia.org/wiki/Damerau%E2%80%93Levenshtein_distance (as retrieved in June 2012)
        public int DamerauLevenshteinDistance(string source, string target)
        {
            var m = source.Length;
            var n = target.Length;
            var h = new int[m + 2, n + 2];

            var inf = m + n;
            h[0, 0] = inf;
            for (var i = 0; i <= m; i++) { h[i + 1, 1] = i; h[i + 1, 0] = inf; }
            for (var j = 0; j <= n; j++) { h[1, j + 1] = j; h[0, j + 1] = inf; }

            var sd = new SortedDictionary<char, int>();
            foreach (var letter in (source + target).Where(letter => !sd.ContainsKey(letter)))
            {
                sd.Add(letter, 0);
            }

            for (var i = 1; i <= m; i++)
            {
                var db = 0;
                for (var j = 1; j <= n; j++)
                {
                    var i1 = sd[target[j - 1]];
                    var j1 = db;

                    if (source[i - 1] == target[j - 1])
                    {
                        h[i + 1, j + 1] = h[i, j];
                        db = j;
                    }
                    else
                    {
                        h[i + 1, j + 1] = Math.Min(h[i, j], Math.Min(h[i + 1, j], h[i, j + 1])) + 1;
                    }

                    h[i + 1, j + 1] = Math.Min(h[i + 1, j + 1], h[i1, j1] + (i - i1 - 1) + 1 + (j - j1 - 1));
                }

                sd[source[i - 1]] = i;
            }
            return h[m + 1, n + 1];
        }

    }
}