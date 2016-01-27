using System;
using System.Collections.Generic;
using AIMLbot.Utils;

namespace AIMLbot
{
    /// <summary>
    ///     Encapsulates information and history of a user who has interacted with the ChatBot
    /// </summary>
    public class User
    {
        /// <summary>
        ///     the predicates associated with this particular user
        /// </summary>
        public Dictionary<string, string> Predicates;

        /// <summary>
        ///     A collection of all the result objects returned to the user in this session
        /// </summary>
        private readonly List<Result> _results = new List<Result>();

        /// <summary>
        ///     The GUID that identifies this user to the ChatBot
        /// </summary>
        public string UserId { get; }

        /// <summary>
        ///     the value of the "topic" predicate
        /// </summary>
        public string Topic => Predicates["topic"];

        /// <summary>
        ///     The most recent result to be returned by the ChatBot
        /// </summary>
        public Result LastResult => _results.Count > 0 ? _results[0] : null;

        /// <summary>
        /// Default constructor using a randomly generated Id.
        /// </summary>
        public User() : this(RandomStringGenerator.GetUniqueId())
        {
        }

        /// <summary>
        ///  Creates an instance of the User class.
        /// </summary>
        /// <param name="userId">The GUID of the user</param>
        public User(string userId)
        {
            if (userId.Length > 0)
            {
                UserId = userId;
                Predicates = ChatBot.Predicates.Clone();
                Predicates.Add("topic", "*");
            }
            else
            {
                throw new Exception("The UserId cannot be empty");
            }
        }

        /// <summary>
        ///     Returns the string to use for the next that part of a subsequent path
        /// </summary>
        /// <returns>the string to use for that</returns>
        public string GetLastBotOutput()
        {
            if (_results.Count > 0)
            {
                return _results[0].RawOutput;
            }
            return "*";
        }

        /// <summary>
        ///     Returns the first sentence of the last output from the ChatBot
        /// </summary>
        /// <returns>the first sentence of the last output from the ChatBot</returns>
        public string GetThat()
        {
            return GetThat(0, 0);
        }

        /// <summary>
        ///     Returns the first sentence of the output "n" steps ago from the ChatBot
        /// </summary>
        /// <param name="n">the number of steps back to go</param>
        /// <returns>the first sentence of the output "n" steps ago from the ChatBot</returns>
        public string GetThat(int n)
        {
            return GetThat(n, 0);
        }

        /// <summary>
        ///     Returns the sentence numbered by "sentence" of the output "n" steps ago from the ChatBot
        /// </summary>
        /// <param name="n">the number of steps back to go</param>
        /// <param name="sentence">the sentence number to get</param>
        /// <returns>the sentence numbered by "sentence" of the output "n" steps ago from the ChatBot</returns>
        public string GetThat(int n, int sentence)
        {
            if ((n >= 0) & (n < _results.Count))
            {
                var historicResult = _results[n];
                if ((sentence >= 0) & (sentence < historicResult.OutputSentences.Count))
                {
                    return historicResult.OutputSentences[sentence];
                }
            }
            return string.Empty;
        }

        /// <summary>
        ///     Returns the first sentence of the last output from the ChatBot
        /// </summary>
        /// <returns>the first sentence of the last output from the ChatBot</returns>
        public string GetResultSentence()
        {
            return GetResultSentence(0, 0);
        }

        /// <summary>
        ///     Returns the first sentence from the output from the ChatBot "n" steps ago
        /// </summary>
        /// <param name="n">the number of steps back to go</param>
        /// <returns>the first sentence from the output from the ChatBot "n" steps ago</returns>
        public string GetResultSentence(int n)
        {
            return GetResultSentence(n, 0);
        }

        /// <summary>
        ///     Returns the identified sentence number from the output from the ChatBot "n" steps ago
        /// </summary>
        /// <param name="n">the number of steps back to go</param>
        /// <param name="sentence">the sentence number to return</param>
        /// <returns>the identified sentence number from the output from the ChatBot "n" steps ago</returns>
        public string GetResultSentence(int n, int sentence)
        {
            if ((n >= 0) & (n < _results.Count))
            {
                var historicResult = _results[n];
                if ((sentence >= 0) & (sentence < historicResult.InputSentences.Count))
                {
                    return historicResult.InputSentences[sentence];
                }
            }
            return string.Empty;
        }

        /// <summary>
        ///     Adds the latest result from the ChatBot to the Results collection
        /// </summary>
        /// <param name="latestResult">the latest result from the ChatBot</param>
        public void AddResult(Result latestResult)
        {
            _results.Insert(0, latestResult);
        }
    }
}