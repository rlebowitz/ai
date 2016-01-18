using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using AIMLbot.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AIMLbot.UnitTest.Normalize
{
    [TestClass]
    public class ApplySubstitutionsTests
    {
        private Dictionary<string, string> _substitutions;

        [TestInitialize]
        public void Setup()
        {
            const string subs = "<?xml version=\"1.0\" encoding=\"utf-8\" ?><root><item name=\" this \" value=\" the \" /><item name=\" is \" value=\" test \" /><item name=\" a \" value=\" works \" /><item name=\" test \" value=\" great \" /><item name=\" fav \" value=\"favourite\" /><item name=\" a \" value=\" works \" /><item name=\" R U \" value=\" are you \" /><item name=\" a \" value=\" works \" /><item name=\" you r \" value=\" you are \" /></root>";
            var doc = new XmlDocument();
            doc.LoadXml(subs);
            var nodeList = doc.GetElementsByTagName("root");
            if (nodeList.Count > 0)
            {
                var node = nodeList[0];
                var handler = new AimlConfigurationHandler();
                _substitutions = handler.Create(null, null, node) as Dictionary<string, string>;
            }
        }

        [TestMethod]
        public void TestNoMatchData()
        {
            var text = "No substitutions here".Substitute(_substitutions);
            Assert.AreEqual("No substitutions here", text);
        }

        [TestMethod]
        public void TestPartialMatch()
        {
            var text = "My favourite things".Substitute(_substitutions);
            Assert.AreEqual("My favourite things", text);
        }

        [TestMethod]
        public void TestWithGoodData()
        {
            var text = " this is a test ".Substitute(_substitutions);
            Assert.AreEqual(" the test works great ", text);
        }

        [TestMethod]
        public void TestWithMultiWordSubstitutions()
        {
            var text = "How R U".Substitute(_substitutions);
            Assert.AreEqual("How are you", text);
        }

        [TestMethod]
        public void TestWithNonMatchingMultiWordSubstitutions()
        {
            var text = "you r sweet".Substitute(_substitutions);
            Assert.AreEqual("you are sweet", text);
            text = "your sweet".Substitute(_substitutions);
            Assert.AreEqual("your sweet", text);
        }
    }
}