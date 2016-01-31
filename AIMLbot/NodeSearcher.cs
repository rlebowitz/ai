using System;
using System.Configuration;
using System.Text;
using AIMLbot.Utils;

namespace AIMLbot
{
    public class NodeSearcher
    {
        private static readonly string Time = ConfigurationManager.AppSettings["timeoutMax"];

        public SubQuery Query { get; }
        /// <summary>
        /// Used to set the number of milliseconds before the search times out.
        /// </summary>
        public int Timeout { get; set; }

        /// <summary>
        ///     Used to navigate the graph.
        /// </summary>
        public NodeSearcher()
        {
            Query = new SubQuery();
            Timeout = Convert.ToInt32(Time);
        }

        /// <summary>
        ///     Navigates this node (and recusively into child nodes) for a match to the path passed as an argument
        ///     whilst processing the referenced request
        /// </summary>
        /// <param name="node">The specified graph node to evaluat.</param>
        /// <param name="path">The normalized path derived from the user's input</param>
        /// <param name="matchstate">The part of the input path the node represents</param>
        /// <param name="wildcard">The contents of the user input absorbed by the AIML wildcards "_" and "*"</param>
        /// <returns>The template to process to generate the output</returns>
        public string Evaluate(Node node, string path, MatchState matchstate, StringBuilder wildcard)
        {
            while (true)
            {
                // so we still have time!
                path = path.Trim();

                // check if this is the end of a branch in the GraphMaster 
                // return the category for this node
                if (node.Children.Count == 0)
                {
                    if (path.Length > 0)
                    {
                        // if we get here it means that there is a wildcard in the user input part of the
                        // path.
                        StoreWildCard(path, wildcard);
                    }
                    Query.Template = node.Template;
                    return node.Template;
                }

                // if we've matched all the words in the input sentence and this is the end
                // of the line then return the category for this node
                if (path.Length == 0)
                {
                    Query.Template = node.Template;
                    return node.Template;
                }

                // otherwise split the input into it's component words
                var splitPath = path.Split(" \r\n\t".ToCharArray());

                // get the first word of the sentence
                var firstWord = splitPath[0].ToUpper();

                // and concatenate the rest of the input into a new path for child nodes
                var newPath = path.Substring(firstWord.Length, path.Length - firstWord.Length);

                // first option is to see if this node has a child denoted by the "_" 
                // wildcard. "_" comes first in precedence in the AIML alphabet
                if (node.Children.ContainsKey("_"))
                {
                    var childNode = node.Children["_"];

                    // add the next word to the wildcard match 
                    var newWildcard = new StringBuilder();
                    StoreWildCard(splitPath[0], newWildcard);

                    // move down into the identified branch of the GraphMaster structure
                    var result = Evaluate(childNode, newPath, matchstate, newWildcard);

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
                                    Query.InputStar.Add(newWildcard.ToString());
                                    // added due to this match being the end of the line
                                    newWildcard.Remove(0, newWildcard.Length);
                                    break;
                                case MatchState.That:
                                    Query.ThatStar.Add(newWildcard.ToString());
                                    break;
                                case MatchState.Topic:
                                    Query.TopicStar.Add(newWildcard.ToString());
                                    break;
                            }
                        }
                        Query.Template = result;
                        return result;
                    }
                }

                // second option - the nodemapper may have contained a "_" child, but led to no match
                // or it didn't contain a "_" child at all. So get the child nodemapper from this 
                // nodemapper that matches the first word of the input sentence.
                if (node.Children.ContainsKey(firstWord))
                {
                    // process the matchstate - this might not make sense but the matchstate is working
                    // with a "backwards" path: "topic <topic> that <that> user input"
                    // the "classic" path looks like this: "user input <that> that <topic> topic"
                    // but having it backwards is more efficient for searching purposes
                    var newMatchstate = matchstate;
                    if (firstWord == "<THAT>")
                    {
                        newMatchstate = MatchState.That;
                    }
                    else if (firstWord == "<TOPIC>")
                    {
                        newMatchstate = MatchState.Topic;
                    }

                    var childNode = node.Children[firstWord];
                    // move down into the identified branch of the GraphMaster structure using the new
                    // matchstate
                    var newWildcard = new StringBuilder();
                    var result = Evaluate(childNode, newPath, newMatchstate, newWildcard);
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
                                    Query.InputStar.Add(newWildcard.ToString());
                                    newWildcard.Remove(0, newWildcard.Length);
                                    break;
                                case MatchState.That:
                                    Query.ThatStar.Add(newWildcard.ToString());
                                    newWildcard.Remove(0, newWildcard.Length);
                                    break;
                                case MatchState.Topic:
                                    Query.TopicStar.Add(newWildcard.ToString());
                                    newWildcard.Remove(0, newWildcard.Length);
                                    break;
                            }
                        }
                        Query.Template = result;
                        return result;
                    }
                }

                // third option - the input part of the path might have been matched so far but hasn't
                // returned a match, so check to see it contains the "*" wildcard. "*" comes last in
                // precedence in the AIML alphabet.
                if (node.Children.ContainsKey("*"))
                {
                    // o.k. look for the path in the child node denoted by "*"
                    var childNode = node.Children["*"];
                    // add the next word to the wildcard match 
                    var newWildcard = new StringBuilder();
                    StoreWildCard(splitPath[0], newWildcard);
                    //
                    var result = Evaluate(childNode, newPath, matchstate, newWildcard);
                    // and if we get a result from the branch process and return it
                    if (result.Length > 0)
                    {
                        if (newWildcard.Length > 0)
                        {
                            // capture and push the star content appropriate to the current matchstate
                            switch (matchstate)
                            {
                                case MatchState.UserInput:
                                    Query.InputStar.Add(newWildcard.ToString());
                                    // added due to this match being the end of the line
                                    newWildcard.Remove(0, newWildcard.Length);
                                    break;
                                case MatchState.That:
                                    Query.ThatStar.Add(newWildcard.ToString());
                                    break;
                                case MatchState.Topic:
                                    Query.TopicStar.Add(newWildcard.ToString());
                                    break;
                            }
                        }
                        Query.Template = result;
                        return result;
                    }
                }

                // o.k. if the nodemapper has failed to match at all: the input contains neither 
                // a "_", the firstWord text, or "*" as a means of denoting a child node. However, 
                // if this node is itself representing a wildcard then the search continues to be
                // valid if we proceed with the tail.
                if ((node.Word == "_") || (node.Word == "*"))
                {
                    StoreWildCard(splitPath[0], wildcard);
                    path = newPath;
                    continue;
                }

                // If we get here then we're at a dead end so return an empty string. Hopefully, if the
                // AIML files have been set up to include a "* <that> * <topic> *" catch-all this
                // state won't be reached. Remember to empty the surplus to requirements wildcard matches
                wildcard.Clear();
                Query.Template = string.Empty;
                return string.Empty;
            }
        }

        /// <summary>
        ///     Correctly stores a word in the wildcard slot
        /// </summary>
        /// <param name="word">The word matched by the wildcard</param>
        /// <param name="wildcard">The contents of the user input absorbed by the AIML wildcards "_" and "*"</param>
        private static void StoreWildCard(string word, StringBuilder wildcard)
        {
            if (wildcard.Length > 0)
            {
                wildcard.Append(" ");
            }
            wildcard.Append(word);
        }
    }
}