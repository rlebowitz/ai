using System;
using System.Collections.Generic;
using System.Reflection;

namespace AIMLbot.Utils
{
    /// <summary>
    /// Encapsulates information about a custom tag class
    /// </summary>
    public class TagHandler
    {
        /// <summary>
        /// The assembly this class is found in
        /// </summary>
        public string AssemblyName;

        /// <summary>
        /// The class name for the assembly
        /// </summary>
        public string ClassName;

        /// <summary>
        /// The name of the tag this class will deal with
        /// </summary>
        public string TagName;

        /// <summary>
        /// Provides an instantiation of the class represented by this tag-handler
        /// </summary>
        /// <param name="assemblies">All the assemblies the ChatBot knows about</param>
        /// <returns>The instantiated class</returns>
        public IAIMLTagHandler Instantiate(Dictionary<string, Assembly> assemblies)
        {
            if (assemblies.ContainsKey(AssemblyName))
            {
                Assembly tagDLL = (Assembly) assemblies[AssemblyName];
                Type[] tagDLLTypes = tagDLL.GetTypes();
                return (IAIMLTagHandler) tagDLL.CreateInstance(ClassName);
            }
            else
            {
                return null;
            }
        }
    }
}