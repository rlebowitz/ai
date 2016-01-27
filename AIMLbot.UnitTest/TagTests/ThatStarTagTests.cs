using System.Xml;
using AIMLbot.AIMLTagHandlers;
using AIMLbot.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AIMLbot.UnitTest.TagTests
{
    [TestClass]
    public class ThatStarTagTests
    {
        private ChatBot _chatBot;
        private User _user;
        private Request _request;
        private Result _result;
        private SubQuery _query;
        private ThatStar _botTagHandler;

        [TestInitialize]
        public void Setup()
        {
            _chatBot = new ChatBot();
            _user = new User();
            _request = new Request("This is a test", _user);
            _query = new SubQuery();
            _query.ThatStar.Insert(0, "first star");
            _query.ThatStar.Insert(0, "second star");
            _result = new Result(_user, _request);
        }

        [TestMethod]
        public void TestAtomic()
        {
            XmlNode testNode = StaticHelpers.GetNode("<thatstar/>");
            _botTagHandler = new ThatStar(_chatBot, _user, _query, _request, _result, testNode);
            Assert.AreEqual("second star", _botTagHandler.Transform());
        }

        [TestMethod]
        public void TestBadIndex()
        {
            XmlNode testNode = StaticHelpers.GetNode("<thatstar index=\"two\"/>");
            _botTagHandler = new ThatStar(_chatBot, _user, _query, _request, _result, testNode);
            Assert.AreEqual("", _botTagHandler.Transform());
        }

        [TestMethod]
        public void TestIndexed()
        {
            XmlNode testNode = StaticHelpers.GetNode("<thatstar index=\"1\"/>");
            _botTagHandler = new ThatStar(_chatBot, _user, _query, _request, _result, testNode);
            Assert.AreEqual("second star", _botTagHandler.Transform());
        }

        [TestMethod]
        public void TestIndexed2()
        {
            XmlNode testNode = StaticHelpers.GetNode("<thatstar index=\"2\"/>");
            _botTagHandler = new ThatStar(_chatBot, _user, _query, _request, _result, testNode);
            Assert.AreEqual("first star", _botTagHandler.Transform());
        }

        [TestMethod]
        public void TestOutOfBounds()
        {
            XmlNode testNode = StaticHelpers.GetNode("<thatstar index=\"3\"/>");
            _botTagHandler = new ThatStar(_chatBot, _user, _query, _request, _result, testNode);
            Assert.AreEqual("", _botTagHandler.Transform());
        }
    }
}