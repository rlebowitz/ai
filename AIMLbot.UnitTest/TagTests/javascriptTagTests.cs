using System.Xml;
using AIMLbot.AIMLTagHandlers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AIMLbot.UnitTest.TagTests
{
    [TestClass]
    public class JavascriptTagTests
    {
        private Javascript _tagHandler;

        [TestInitialize]
        public void Setup()
        {
        }

        [TestMethod]
        public void TestJavascriptIsNotImplemented()
        {
            XmlNode testNode = StaticHelpers.GetNode("<javascript/>");
            _tagHandler = new Javascript(testNode);
            Assert.AreEqual("", _tagHandler.ProcessChange());
        }
    }
}