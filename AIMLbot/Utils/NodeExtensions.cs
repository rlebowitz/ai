using System.Xml;

namespace AIMLbot.Utils
{
    public static class NodeExtensions
    {
        /// <summary>
        ///     Adds a category to the node
        /// </summary>
        /// <param name="node">the node to which the category will be added</param>
        /// <param name="path">the path for the category</param>
        /// <param name="template">the template to find at the end of the path</param>
        public static void AddCategory(this Node node, string path, string template)
        {
            while (true)
            {
                if (template.Length == 0)
                {
                    var message = $"Category {path} has an empty template tag.";
                    throw new XmlException(message);
                }
                // check we're not at the leaf node
                if (path.Trim().Length == 0)
                {
                    node.Template = template;
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
                if (node.Children.ContainsKey(firstWord))
                {
                    var childNode = node.Children[firstWord];
                    node = childNode;
                    path = newPath;
                    continue;
                }
                else
                {
                    var childNode = new Node {Word = firstWord};
                    childNode.AddCategory(newPath, template);
                    node.Children.Add(childNode.Word, childNode);
                }
                break;
            }
        }
    }
}