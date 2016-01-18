using System.Xml;
using AIMLbot.AIMLTagHandlers;
using AIMLbot.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AIMLbot.UnitTest.TagTests
{
    [TestClass]
    public class StarTagTests
    {
        private ChatBot _chatBot;
        private User _user;
        private Request _request;
        private Result _result;
        private SubQuery _query;
        private Star _botTagHandler;

        [TestInitialize]
        public void Setup()
        {
            _chatBot = new ChatBot();
            _user = new User("1", _chatBot);
            _request = new Request("This is a test", _user, _chatBot);
            _query = new SubQuery("This is a test <that> * <topic> *");
            _query.InputStar.Insert(0, "first star");
            _query.InputStar.Insert(0, "second star");
            _result = new Result(_user, _chatBot, _request);
        }

        [TestMethod]
        public void TestBadInputAttributeName()
        {
            XmlNode testNode = StaticHelpers.getNode("<star indox=\"3\"/>");
            _botTagHandler = new Star(_chatBot, _user, _query, _request, _result, testNode);
            Assert.AreEqual("", _botTagHandler.Transform());
        }

        [TestMethod]
        public void TestBadInputAttributeValue()
        {
            XmlNode testNode = StaticHelpers.getNode("<star index=\"one\"/>");
            _botTagHandler = new Star(_chatBot, _user, _query, _request, _result, testNode);
            Assert.AreEqual("", _botTagHandler.Transform());
        }

        [TestMethod]
        public void TestBadInputTagName()
        {
            XmlNode testNode = StaticHelpers.getNode("<stor index=\"1\"/>");
            _botTagHandler = new Star(_chatBot, _user, _query, _request, _result, testNode);
            Assert.AreEqual("", _botTagHandler.Transform());
        }

        [TestMethod]
        public void TestExpectedInput()
        {
            XmlNode testNode = StaticHelpers.getNode("<star/>");
            _botTagHandler = new Star(_chatBot, _user, _query, _request, _result, testNode);
            Assert.AreEqual("second star", _botTagHandler.Transform());
        }

        [TestMethod]
        public void TestExpectedInputIndex()
        {
            XmlNode testNode = StaticHelpers.getNode("<star index=\"1\"/>");
            _botTagHandler = new Star(_chatBot, _user, _query, _request, _result, testNode);
            Assert.AreEqual("second star", _botTagHandler.Transform());
        }

        [TestMethod]
        public void TestExpectedInputIndexOutOfBounds()
        {
            XmlNode testNode = StaticHelpers.getNode("<star index=\"3\"/>");
            _botTagHandler = new Star(_chatBot, _user, _query, _request, _result, testNode);
            Assert.AreEqual("", _botTagHandler.Transform());
        }

        [TestMethod]
        public void TestExpectedInputIndexSecond()
        {
            XmlNode testNode = StaticHelpers.getNode("<star index=\"2\"/>");
            _botTagHandler = new Star(_chatBot, _user, _query, _request, _result, testNode);
            Assert.AreEqual("first star", _botTagHandler.Transform());
        }
    }
}