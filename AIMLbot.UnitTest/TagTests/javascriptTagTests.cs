using System.Xml;
using AIMLbot.AIMLTagHandlers;
using AIMLbot.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AIMLbot.UnitTest.TagTests
{
    [TestClass]
    public class JavascriptTagTests
    {
        private ChatBot _chatBot;
        private User _user;
        private Request _request;
        private Result _result;
        private SubQuery _query;
        private Javascript _botTagHandler;

        [TestInitialize]
        public void Setup()
        {
            _chatBot = new ChatBot();
            _user = new User();
            _request = new Request("This is a test", _user);
            _query = new SubQuery();
            _result = new Result(_user, _request);
        }

        [TestMethod]
        public void TestJavascriptIsNotImplemented()
        {
            XmlNode testNode = StaticHelpers.GetNode("<javascript/>");
            _botTagHandler = new Javascript(_chatBot, _user, _query, _request, _result, testNode);
            Assert.AreEqual("", _botTagHandler.Transform());
        }
    }
}