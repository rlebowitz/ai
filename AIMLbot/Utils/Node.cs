using System;
using System.Collections.Generic;

namespace AIMLbot.Utils
{
    /// <summary>
    /// Encapsulates a node in the graphmaster tree structure
    /// </summary>
    [Serializable]
    public class Node
    {
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
    }
}