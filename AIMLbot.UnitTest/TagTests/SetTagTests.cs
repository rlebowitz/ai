using System.Xml;
using AIMLbot.AIMLTagHandlers;
using AIMLbot.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AIMLbot.UnitTest.TagTests
{
    [TestClass]
    public class SetTagTests
    {
        private ChatBot _chatBot;
        private User _user;
        private Request _request;
        private Result _result;
        private SubQuery _query;
        private Set _botTagHandler;

        [TestInitialize]
        public void Initialize()
        {
            _chatBot = new ChatBot();
            _user = new User("1", _chatBot);
            _request = new Request("This is a test", _user, _chatBot);
            _query = new SubQuery("This is a test <that> * <topic> *");
            _result = new Result(_user, _chatBot, _request);
        }

        [TestMethod]
        public void TestAbilityToRemovePredicates()
        {
            XmlNode testNode = StaticHelpers.GetNode("<set name=\"test1\">content 1</set>");
            _botTagHandler = new Set(_chatBot, _user, _query, _request, _result, testNode);
            Assert.AreEqual("content 1", _botTagHandler.Transform());
            Assert.AreEqual(true, _user.Predicates.ContainsKey("test1"));
            XmlNode testNode2 = StaticHelpers.GetNode("<set name=\"test1\"/>");
            _botTagHandler = new Set(_chatBot, _user, _query, _request, _result, testNode2);
            Assert.AreEqual("", _botTagHandler.Transform());
            Assert.AreEqual(false, _user.Predicates.ContainsKey("test1"));
        }

        [TestMethod]
        public void TestWithBadAttribute()
        {
            XmlNode testNode = StaticHelpers.GetNode("<set nome=\"test 3\">content 3</set>");
            _botTagHandler = new Set(_chatBot, _user, _query, _request, _result, testNode);
            Assert.AreEqual("", _botTagHandler.Transform());
        }

        [TestMethod]
        public void TestWithBadNode()
        {
            XmlNode testNode = StaticHelpers.GetNode("<sot name=\"test2\">content 2</sot>");
            _botTagHandler = new Set(_chatBot, _user, _query, _request, _result, testNode);
            Assert.AreEqual("", _botTagHandler.Transform());
        }

        [TestMethod]
        public void TestWithGoodData()
        {
            XmlNode testNode = StaticHelpers.GetNode("<set name=\"test1\">content 1</set>");
            _botTagHandler = new Set(_chatBot, _user, _query, _request, _result, testNode);
            Assert.AreEqual("content 1", _botTagHandler.Transform());
            Assert.AreEqual(true, _user.Predicates.ContainsKey("test1"));
        }

        [TestMethod]
        public void TestWithNoAttributes()
        {
            XmlNode testNode = StaticHelpers.GetNode("<set/>");
            _botTagHandler = new Set(_chatBot, _user, _query, _request, _result, testNode);
            Assert.AreEqual("", _botTagHandler.Transform());
        }

        [TestMethod]
        public void TestWithTooManyAttributes()
        {
            XmlNode testNode = StaticHelpers.GetNode("<set name=\"test 4\" value=\"value\" >content 4</set>");
            _botTagHandler = new Set(_chatBot, _user, _query, _request, _result, testNode);
            Assert.AreEqual("", _botTagHandler.Transform());
        }
    }
}