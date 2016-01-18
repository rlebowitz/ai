using System.Xml;
using AIMLbot.AIMLTagHandlers;
using AIMLbot.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AIMLbot.UnitTest.TagTests
{
    [TestClass]
    public class Person2TagTests
    {
        private ChatBot _chatBot;
        private User _user;
        private Request _request;
        private Result _result;
        private SubQuery _query;
        private Person2 _botTagHandler;

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
        public void TestAtomic()
        {
            XmlNode testNode = StaticHelpers.getNode("<person2/>");
            _botTagHandler = new Person2(_chatBot, _user, _query, _request, _result, testNode);
            _query.InputStar.Insert(0, " WITH YOU TO YOU ME MY YOUR ");
            Assert.AreEqual(" with me to me you your my ", _botTagHandler.Transform());
        }

        [TestMethod]
        public void TestEmptyInput()
        {
            XmlNode testNode = StaticHelpers.getNode("<person2/>");
            _botTagHandler = new Person2(_chatBot, _user, _query, _request, _result, testNode);
            _query.InputStar.Clear();
            Assert.AreEqual("", _botTagHandler.Transform());
        }

        [TestMethod]
        public void TestNoMatches()
        {
            XmlNode testNode = StaticHelpers.getNode("<person2>THE QUICK BROWN FOX JUMPED OVER THE LAZY DOGS</person2>");
            _botTagHandler = new Person2(_chatBot, _user, _query, _request, _result, testNode);
            Assert.AreEqual("THE QUICK BROWN FOX JUMPED OVER THE LAZY DOGS", _botTagHandler.Transform());
        }

        [TestMethod]
        public void TestNonAtomic()
        {
            XmlNode testNode = StaticHelpers.getNode("<person2> WITH YOU TO YOU ME MY YOUR </person2>");
            _botTagHandler = new Person2(_chatBot, _user, _query, _request, _result, testNode);
            Assert.AreEqual(" with me to me you your my ", _botTagHandler.Transform());
        }
    }
}