using System.Xml;
using AIMLbot.AIMLTagHandlers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AIMLbot.UnitTest.TagTests
{
    [TestClass]
    public class VersionTagTests
    {
        private Version _tagHandler;

        [TestInitialize]
        public void Setup()
        {
        }

        [TestMethod]
        public void TestBadInput()
        {
            XmlNode testNode = StaticHelpers.GetNode("<vorsion/>");
            _tagHandler = new Version(testNode);
            Assert.AreEqual("", _tagHandler.ProcessChange());
        }

        [TestMethod]
        public void TestExpectedInput()
        {
            XmlNode testNode = StaticHelpers.GetNode("<version/>");
            _tagHandler = new Version(testNode);
            Assert.AreEqual("unknown", _tagHandler.ProcessChange());
        }
    }
}