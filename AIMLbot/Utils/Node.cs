using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Xml;
using log4net;

namespace AIMLbot.Utils
{
    /// <summary>
    /// Encapsulates a node in the graphmaster tree structure
    /// </summary>
    [Serializable]
    public class Node
    {

        private static readonly ILog Log = LogManager.GetLogger(typeof(Node));

        #region Attributes

        /// <summary>
        /// Contains the child nodes of this node
        /// </summary>
        private readonly Dictionary<string, Node> _children = new Dictionary<string, Node>();

        /// <summary>
        /// The template (if any) associated with this node
        /// </summary>
        public string Template = string.Empty;

        /// <summary>
        /// The word that identifies this node to it's parent node
        /// </summary>
        public string Word = string.Empty;

        /// <summary>
        /// The number of direct children (non-recursive) of this node
        /// </summary>
        public int NumberOfChildNodes => _children.Count;

        #endregion

        #region Methods

        #region Add category

        /// <summary>
        /// Adds a category to the node
        /// </summary>
        /// <param name="path">the path for the category</param>
        /// <param name="template">the template to find at the end of the path</param>
        public void AddCategory(string path, string template)
        {
            if (template.Length == 0)
            {
                var message = $"Category {path} has an empty template tag.";
                throw new XmlException(message);
            }
            // check we're not at the leaf node
            if (path.Trim().Length == 0)
            {
                Template = template;
                return;
            }

            // otherwise, this sentence requires further child nodemappers in order to
            // be fully mapped within the GraphMaster structure.

            // split the input into its component words
            string[] words = path.Trim().Split(" ".ToCharArray());

            // get the first word (to form the key for the child nodemapper)
            string firstWord = words[0].ToUpper();

            // concatenate the rest of the sentence into a suffix (to act as the
            // path argument in the child nodemapper)
            string newPath = path.Substring(firstWord.Length, path.Length - firstWord.Length).Trim();

            // o.k. check we don't already have a child with the key from this sentence
            // if we do then pass the handling of this sentence down the branch to the 
            // child nodemapper otherwise the child nodemapper doesn't yet exist, so create a new one
            if (_children.ContainsKey(firstWord))
            {
                Node childNode = _children[firstWord];
                childNode.AddCategory(newPath, template);
            }
            else
            {
                var childNode = new Node {Word = firstWord};
                childNode.AddCategory(newPath, template);
                _children.Add(childNode.Word, childNode);
            }
        }

        #endregion

        #region Evaluate Node

        /// <summary>
        /// Navigates this node (and recusively into child nodes) for a match to the path passed as an argument
        /// whilst processing the referenced request
        /// </summary>
        /// <param name="path">The normalized path derived from the user's input</param>
        /// <param name="query">The query that this search is for</param>
        /// <param name="request">An encapsulation of the request from the user</param>
        /// <param name="matchstate">The part of the input path the node represents</param>
        /// <param name="wildcard">The contents of the user input absorbed by the AIML wildcards "_" and "*"</param>
        /// <returns>The template to process to generate the output</returns>
        public string Evaluate(string path, SubQuery query, Request request, MatchState matchstate, StringBuilder wildcard)
        {
            var time = ConfigurationManager.AppSettings["timeoutMax"];
            var timeout = Convert.ToInt32(time);
            while (true)
            {
                // check for timeout
                if (request.StartedOn.AddMilliseconds(timeout) < DateTime.Now)
                {
                    Log.Error("WARNING! Request timeout. User: " + request.User.UserId + " raw input: \"" + request.RawInput + "\"");
                    request.HasTimedOut = true;
                    return string.Empty;
                }

                // so we still have time!
                path = path.Trim();

                // check if this is the end of a branch in the GraphMaster 
                // return the cCategory for this node
                if (_children.Count == 0)
                {
                    if (path.Length > 0)
                    {
                        // if we get here it means that there is a wildcard in the user input part of the
                        // path.
                        StoreWildCard(path, wildcard);
                    }
                    return Template;
                }

                // if we've matched all the words in the input sentence and this is the end
                // of the line then return the cCategory for this node
                if (path.Length == 0)
                {
                    return Template;
                }

                // otherwise split the input into it's component words
                string[] splitPath = path.Split(" \r\n\t".ToCharArray());

                // get the first word of the sentence
                string firstWord = splitPath[0].ToUpper();

                // and concatenate the rest of the input into a new path for child nodes
                string newPath = path.Substring(firstWord.Length, path.Length - firstWord.Length);

                // first option is to see if this node has a child denoted by the "_" 
                // wildcard. "_" comes first in precedence in the AIML alphabet
                if (_children.ContainsKey("_"))
                {
                    Node childNode = _children["_"];

                    // add the next word to the wildcard match 
                    StringBuilder newWildcard = new StringBuilder();
                    StoreWildCard(splitPath[0], newWildcard);

                    // move down into the identified branch of the GraphMaster structure
                    string result = childNode.Evaluate(newPath, query, request, matchstate, newWildcard);

                    // and if we get a result from the branch process the wildcard matches and return 
                    // the result
                    if (result.Length > 0)
                    {
                        if (newWildcard.Length > 0)
                        {
                            // capture and push the star content appropriate to the current matchstate
                            switch (matchstate)
                            {
                                case MatchState.UserInput:
                                    query.InputStar.Add(newWildcard.ToString());
                                    // added due to this match being the end of the line
                                    newWildcard.Remove(0, newWildcard.Length);
                                    break;
                                case MatchState.That:
                                    query.ThatStar.Add(newWildcard.ToString());
                                    break;
                                case MatchState.Topic:
                                    query.TopicStar.Add(newWildcard.ToString());
                                    break;
                            }
                        }
                        return result;
                    }
                }

                // second option - the nodemapper may have contained a "_" child, but led to no match
                // or it didn't contain a "_" child at all. So get the child nodemapper from this 
                // nodemapper that matches the first word of the input sentence.
                if (_children.ContainsKey(firstWord))
                {
                    // process the matchstate - this might not make sense but the matchstate is working
                    // with a "backwards" path: "topic <topic> that <that> user input"
                    // the "classic" path looks like this: "user input <that> that <topic> topic"
                    // but having it backwards is more efficient for searching purposes
                    MatchState newMatchstate = matchstate;
                    if (firstWord == "<THAT>")
                    {
                        newMatchstate = MatchState.That;
                    }
                    else if (firstWord == "<TOPIC>")
                    {
                        newMatchstate = MatchState.Topic;
                    }

                    Node childNode = _children[firstWord];
                    // move down into the identified branch of the GraphMaster structure using the new
                    // matchstate
                    StringBuilder newWildcard = new StringBuilder();
                    string result = childNode.Evaluate(newPath, query, request, newMatchstate, newWildcard);
                    // and if we get a result from the child return it
                    if (result.Length > 0)
                    {
                        if (newWildcard.Length > 0)
                        {
                            // capture and push the star content appropriate to the matchstate if it exists
                            // and then clear it for subsequent wildcards
                            switch (matchstate)
                            {
                                case MatchState.UserInput:
                                    query.InputStar.Add(newWildcard.ToString());
                                    newWildcard.Remove(0, newWildcard.Length);
                                    break;
                                case MatchState.That:
                                    query.ThatStar.Add(newWildcard.ToString());
                                    newWildcard.Remove(0, newWildcard.Length);
                                    break;
                                case MatchState.Topic:
                                    query.TopicStar.Add(newWildcard.ToString());
                                    newWildcard.Remove(0, newWildcard.Length);
                                    break;
                            }
                        }
                        return result;
                    }
                }

                // third option - the input part of the path might have been matched so far but hasn't
                // returned a match, so check to see it contains the "*" wildcard. "*" comes last in
                // precedence in the AIML alphabet.
                if (_children.ContainsKey("*"))
                {
                    // o.k. look for the path in the child node denoted by "*"
                    Node childNode = _children["*"];

                    // add the next word to the wildcard match 
                    StringBuilder newWildcard = new StringBuilder();
                    StoreWildCard(splitPath[0], newWildcard);

                    string result = childNode.Evaluate(newPath, query, request, matchstate, newWildcard);
                    // and if we get a result from the branch process and return it
                    if (result.Length > 0)
                    {
                        if (newWildcard.Length > 0)
                        {
                            // capture and push the star content appropriate to the current matchstate
                            switch (matchstate)
                            {
                                case MatchState.UserInput:
                                    query.InputStar.Add(newWildcard.ToString());
                                    // added due to this match being the end of the line
                                    newWildcard.Remove(0, newWildcard.Length);
                                    break;
                                case MatchState.That:
                                    query.ThatStar.Add(newWildcard.ToString());
                                    break;
                                case MatchState.Topic:
                                    query.TopicStar.Add(newWildcard.ToString());
                                    break;
                            }
                        }
                        return result;
                    }
                }

                // o.k. if the nodemapper has failed to match at all: the input contains neither 
                // a "_", the sFirstWord text, or "*" as a means of denoting a child node. However, 
                // if this node is itself representing a wildcard then the search continues to be
                // valid if we proceed with the tail.
                if ((Word == "_") || (Word == "*"))
                {
                    StoreWildCard(splitPath[0], wildcard);
                    path = newPath;
                    continue;
                }

                // If we get here then we're at a dead end so return an empty string. Hopefully, if the
                // AIML files have been set up to include a "* <that> * <topic> *" catch-all this
                // state won't be reached. Remember to empty the surplus to requirements wildcard matches
                wildcard.Clear();
                return string.Empty;
            }
        }

        /// <summary>
        /// Correctly stores a word in the wildcard slot
        /// </summary>
        /// <param name="word">The word matched by the wildcard</param>
        /// <param name="wildcard">The contents of the user input absorbed by the AIML wildcards "_" and "*"</param>
        private void StoreWildCard(string word, StringBuilder wildcard)
        {
            if (wildcard.Length > 0)
            {
                wildcard.Append(" ");
            }
            wildcard.Append(word);
        }

        #endregion

        #endregion
    }
}