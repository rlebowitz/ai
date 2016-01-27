using System;
using System.Collections.Generic;
using System.Xml;

namespace AIMLbot.Utils
{
    /// <summary>
    ///     Encapsulates a node in the graphmaster tree structure
    /// </summary>
    [Serializable]
    public class Node
    {

        #region Add category

        /// <summary>
        ///     Adds a category to the node
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
            var words = path.Trim().Split(" ".ToCharArray());

            // get the first word (to form the key for the child nodemapper)
            var firstWord = words[0].ToUpper();

            // concatenate the rest of the sentence into a suffix (to act as the
            // path argument in the child nodemapper)
            // ToDo: should just join array minus first word.
            var newPath = path.Substring(firstWord.Length, path.Length - firstWord.Length).Trim();

            // o.k. check we don't already have a child with the key from this sentence
            // if we do then pass the handling of this sentence down the branch to the 
            // child nodemapper otherwise the child nodemapper doesn't yet exist, so create a new one
            if (Children.ContainsKey(firstWord))
            {
                var childNode = Children[firstWord];
                childNode.AddCategory(newPath, template);
            }
            else
            {
                var childNode = new Node {Word = firstWord};
                childNode.AddCategory(newPath, template);
                Children.Add(childNode.Word, childNode);
            }
        }

        #endregion

        #region Attributes

        /// <summary>
        ///     Contains the child nodes of this node
        /// </summary>
        public Dictionary<string, Node> Children = new Dictionary<string, Node>();

        /// <summary>
        ///     The template (if any) associated with this node
        /// </summary>
        public string Template = string.Empty;

        /// <summary>
        ///     The word that identifies this node to it's parent node
        /// </summary>
        public string Word = string.Empty;

        /// <summary>
        ///     The number of direct children (non-recursive) of this node
        /// </summary>
        public int NumberOfChildNodes => Children.Count;

        #endregion
    }
}