using System.Xml;
using AIMLbot.AIMLTagHandlers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AIMLbot.UnitTest.TagTests
{
    [TestClass]
    public class IdTagTests
    {
        private User _user;
        private Id _idTagHandler;

        [TestInitialize]
        public void Setup()
        {
            _user = new User();
        }

        [TestMethod]
        public void TestWithBadXml()
        {
            XmlNode testNode = StaticHelpers.GetNode("<od/>");
            _idTagHandler = new Id(_user, testNode);
            Assert.AreEqual("", _idTagHandler.ProcessChange());
        }

        [TestMethod]
        public void TestWithValidData()
        {
            XmlNode testNode = StaticHelpers.GetNode("<id/>");
            _idTagHandler = new Id(_user, testNode);
            Assert.AreEqual("1", _idTagHandler.ProcessChange());
        }
    }
}