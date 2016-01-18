using System.Xml;
using AIMLbot.AIMLTagHandlers;
using AIMLbot.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AIMLbot.UnitTest.TagTests
{
    [TestClass]
    public class BotTagTests
    {
        private ChatBot _chatBot;
        private User _user;
        private Request _request;
        private Result _result;
        private SubQuery _query;
        private Bot _botTagHandler;

        [TestInitialize]
        public void Setup()
        {
            _chatBot = new ChatBot();
            _user = new User("1", _chatBot);
            _request = new Request("This is a test", _user, _chatBot);
            _query = new SubQuery("This is a test <that> * <topic> *");
            _result = new Result(_user, _chatBot, _request);
        }

        [TestMethod]
        public void TestBadAttribute()
        {
            XmlNode testNode = StaticHelpers.getNode("<ChatBot value=\"name\"/>");
            _botTagHandler = new Bot(_chatBot, _user, _query, _request, _result, testNode);
            Assert.AreEqual("", _botTagHandler.Transform());
        }

        [TestMethod]
        public void TestBadNodeName()
        {
            XmlNode testNode = StaticHelpers.getNode("<bad value=\"name\"/>");
            _botTagHandler = new Bot(_chatBot, _user, _query, _request, _result, testNode);
            Assert.AreEqual("", _botTagHandler.Transform());
        }

        [TestMethod]
        public void TestExpectedInput()
        {
            XmlNode testNode = StaticHelpers.getNode("<ChatBot name= \"name\"/>");
            _botTagHandler = new Bot(_chatBot, _user, _query, _request, _result, testNode);
            Assert.AreEqual("Unknown", _botTagHandler.Transform());
        }

        [TestMethod]
        public void TestNonExistentPredicate()
        {
            XmlNode testNode = StaticHelpers.getNode("<ChatBot name=\"nonexistent\"/>");
            _botTagHandler = new Bot(_chatBot, _user, _query, _request, _result, testNode);
            Assert.AreEqual("", _botTagHandler.Transform());
        }

        [TestMethod]
        public void TestStandardPredicateCollection()
        {
            string[] predicates = {
                                      "name", "birthday", "birthplace", "boyfriend", "favoriteband", "favoritebook",
                                      "favoritecolor", "favoritefood", "favoritesong", "favoritemovie", "forfun", "friends"
                                      , "Gender", "girlfriend", "kindmusic", "location", "looklike", "master", "question",
                                      "sign", "talkabout", "wear"
                                  };
            foreach (string predicate in predicates)
            {
                XmlNode testNode = StaticHelpers.getNode("<ChatBot name=\"" + predicate + "\"/>");
                _botTagHandler = new Bot(_chatBot, _user, _query, _request, _result, testNode);
                Assert.AreNotEqual(string.Empty, _botTagHandler.Transform());
            }
        }

        [TestMethod]
        public void TestTooManyAttributes()
        {
            XmlNode testNode = StaticHelpers.getNode("<ChatBot name=\"name\" value=\"bad\"/>");
            _botTagHandler = new Bot(_chatBot, _user, _query, _request, _result, testNode);
            Assert.AreEqual("", _botTagHandler.Transform());
        }
    }
}