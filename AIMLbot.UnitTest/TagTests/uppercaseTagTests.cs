using System.Xml;
using AIMLbot.AIMLTagHandlers;
using AIMLbot.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AIMLbot.UnitTest.TagTests
{
    [TestClass]
    public class UppercaseTagTests
    {
        private ChatBot _chatBot;
        private User _user;
        private Request _request;
        private Result _result;
        private SubQuery _query;
        private Uppercase _botTagHandler;

        [TestInitialize]
        public void SetupMockObjects()
        {
            _chatBot = new ChatBot();
            _user = new User();
            _request = new Request("This is a test", _user);
            _result = new Result(_user, _request);
            _query = new SubQuery();
        }

        [TestMethod]
        public void TestEmptyInput()
        {
            XmlNode testNode = StaticHelpers.GetNode("<uppercase/>");
            _botTagHandler = new Uppercase(_chatBot, _user, _query, _request, _result, testNode);
            Assert.AreEqual("", _botTagHandler.Transform());
        }

        [TestMethod]
        public void TestExpectedInput()
        {
            XmlNode testNode = StaticHelpers.GetNode("<uppercase>this is a test</uppercase>");
            _botTagHandler = new Uppercase(_chatBot, _user, _query, _request, _result, testNode);
            Assert.AreEqual("THIS IS A TEST", _botTagHandler.Transform());
        }
    }
}