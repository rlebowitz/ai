using System.Xml;
using AIMLbot.AIMLTagHandlers;
using AIMLbot.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AIMLbot.UnitTest.TagTests
{
    [TestClass]
    public class VersionTagTests
    {
        private ChatBot _chatBot;
        private User _user;
        private Request _request;
        private Result _result;
        private SubQuery _query;
        private Version _botTagHandler;

        [TestInitialize]
        public void Setup()
        {
            _chatBot = new ChatBot();
            _user = new User("1", _chatBot);
            _request = new Request("This is a test", _user, _chatBot);
            _result = new Result(_user, _chatBot, _request);
            _query = new SubQuery("This is a test <that> * <topic> *");
        }

        [TestMethod]
        public void TestBadInput()
        {
            XmlNode testNode = StaticHelpers.GetNode("<vorsion/>");
            _botTagHandler = new Version(_chatBot, _user, _query, _request, _result, testNode);
            Assert.AreEqual("", _botTagHandler.Transform());
        }

        [TestMethod]
        public void TestExpectedInput()
        {
            XmlNode testNode = StaticHelpers.GetNode("<version/>");
            _botTagHandler = new Version(_chatBot, _user, _query, _request, _result, testNode);
            Assert.AreEqual("unknown", _botTagHandler.Transform());
        }
    }
}