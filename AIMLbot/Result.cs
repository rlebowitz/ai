using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIMLbot.Utils;
using log4net;

namespace AIMLbot
{
    /// <summary>
    /// Encapsulates information about the result of a request to the ChatBot
    /// </summary>
    public class Result
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(Result));

        private readonly string[] _splitters;

        /// <summary>
        /// The ChatBot that is providing the answer
        /// </summary>
        public ChatBot ChatBot;

        /// <summary>
        /// The amount of time the request took to process
        /// </summary>
        public TimeSpan Duration;

        /// <summary>
        /// The individual sentences that constitute the raw input from the user
        /// </summary>
        public List<string> InputSentences = new List<string>();

        /// <summary>
        /// The normalized sentence(s) (paths) fed into the graphmaster
        /// </summary>
        public List<string> NormalizedPaths = new List<string>();

        /// <summary>
        /// The individual sentences produced by the ChatBot that form the complete response
        /// </summary>
        public List<string> OutputSentences = new List<string>();

        /// <summary>
        /// The request from the user
        /// </summary>
        public Request Request;

        /// <summary>
        /// The subQueries processed by the ChatBot's graphmaster that contain the templates that 
        /// are to be converted into the collection of Sentences
        /// </summary>
        public List<SubQuery> SubQueries = new List<SubQuery>();

        /// <summary>
        /// The user for whom this is a result
        /// </summary>
        public User User;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="user">The user for whom this is a result</param>
        /// <param name="chatBot">The ChatBot providing the result</param>
        /// <param name="request">The request that originated this result</param>
        public Result(User user, ChatBot chatBot, Request request)
        {
            User = user;
            ChatBot = chatBot;
            Request = request;
            Request.Result = this;
            _splitters = ChatBot.Splitters.ToArray();
        }

        /// <summary>
        /// The raw input from the user
        /// </summary>
        public string RawInput => Request.RawInput;

        /// <summary>
        /// The result from the ChatBot with logging and checking
        /// </summary>
        public string Output
        {
            get
            {
                if (OutputSentences.Count > 0)
                {
                    return RawOutput;
                }
                else
                {
                    if (Request.HasTimedOut)
                    {
                        return ChatBot.TimeOutMessage;
                    }
                    StringBuilder paths = new StringBuilder();
                    foreach (string pattern in NormalizedPaths)
                    {
                        paths.Append(pattern + Environment.NewLine);
                    }
                    Log.Error("The ChatBot could not find any response for the input: " + RawInput +
                              " with the path(s): " + Environment.NewLine + paths +
                              " from the user with an id: " + User.UserId);
                    return string.Empty;
                }
            }
        }

        /// <summary>
        /// Returns the raw sentences without any logging 
        /// </summary>
        public string RawOutput
        {
            get
            {
                StringBuilder result = new StringBuilder();
                foreach (string sentence in OutputSentences)
                {
                    string sentenceForOutput = sentence.Trim();
                    if (!CheckEndsAsSentence(sentenceForOutput))
                    {
                        sentenceForOutput += ".";
                    }
                    result.Append(sentenceForOutput + " ");
                }
                return result.ToString().Trim();
            }
        }

        /// <summary>
        /// Returns the raw output from the ChatBot
        /// </summary>
        /// <returns>The raw output from the ChatBot</returns>
        public override string ToString()
        {
            return Output;
        }

        /// <summary>
        /// Checks that the provided sentence ends with a sentence splitter
        /// </summary>
        /// <param name="sentence">the sentence to check</param>
        /// <returns>True if ends with an appropriate sentence splitter</returns>
        private bool CheckEndsAsSentence(string sentence)
        {
            return _splitters.Any(splitter => sentence.Trim().EndsWith(splitter));
        }
    }
}