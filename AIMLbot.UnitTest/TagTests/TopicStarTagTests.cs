using System.Xml;
using AIMLbot.AIMLTagHandlers;
using AIMLbot.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AIMLbot.UnitTest.TagTests
{
    [TestClass]
    public class TopicStarTagTests
    {
        private ChatBot _chatBot;
        private User _user;
        private Request _request;
        private Result _result;
        private SubQuery _query;
        private Topicstar _botTagHandler;

        [TestInitialize]
        public void Setup()
        {
            _chatBot = new ChatBot();
            _user = new User("1", _chatBot);
            _request = new Request("This is a test", _user, _chatBot);
            _query = new SubQuery("This is a test <that> * <topic> *");
            _query.TopicStar.Insert(0, "first star");
            _query.TopicStar.Insert(0, "second star");
            _result = new Result(_user, _chatBot, _request);
        }

        [TestMethod]
        public void TestAtomic()
        {
            XmlNode testNode = StaticHelpers.GetNode("<topicstar/>");
            _botTagHandler = new Topicstar(_chatBot, _user, _query, _request, _result, testNode);
            Assert.AreEqual("second star", _botTagHandler.Transform());
        }

        [TestMethod]
        public void TestBadIndex()
        {
            XmlNode testNode = StaticHelpers.GetNode("<topicstar index=\"two\"/>");
            _botTagHandler = new Topicstar(_chatBot, _user, _query, _request, _result, testNode);
            Assert.AreEqual("", _botTagHandler.Transform());
        }

        [TestMethod]
        public void TestIndexed()
        {
            XmlNode testNode = StaticHelpers.GetNode("<topicstar index=\"1\"/>");
            _botTagHandler = new Topicstar(_chatBot, _user, _query, _request, _result, testNode);
            Assert.AreEqual("second star", _botTagHandler.Transform());
        }

        [TestMethod]
        public void TestIndexed2()
        {
            XmlNode testNode = StaticHelpers.GetNode("<topicstar index=\"2\"/>");
            _botTagHandler = new Topicstar(_chatBot, _user, _query, _request, _result, testNode);
            Assert.AreEqual("first star", _botTagHandler.Transform());
        }

        [TestMethod]
        public void TestOutOfBounds()
        {
            XmlNode testNode = StaticHelpers.GetNode("<topicstar index=\"3\"/>");
            _botTagHandler = new Topicstar(_chatBot, _user, _query, _request, _result, testNode);
            Assert.AreEqual("", _botTagHandler.Transform());
        }
    }
}