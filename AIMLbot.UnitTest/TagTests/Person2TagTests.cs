using System.Xml;
using AIMLbot.AIMLTagHandlers;
using AIMLbot.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AIMLbot.UnitTest.TagTests
{
    [TestClass]
    public class Person2TagTests
    {
        private ChatBot _bot;
        private User _user;
        private Request _request;
        private Result _result;
        private SubQuery _query;
        private Person2 _tagHandler;

        [TestInitialize]
        public void Setup()
        {
            _bot = new ChatBot();
            _user = new User();
            _request = new Request("This is a test", _user);
            _query = new SubQuery();
            _result = new Result(_user, _request);
        }

        [TestMethod]
        public void TestAtomic()
        {
            XmlNode testNode = StaticHelpers.GetNode("<person2/>");
            _tagHandler = new Person2(_bot, _user, _query, _request, _result, testNode);
            _query.InputStar.Insert(0, " WITH YOU TO YOU ME MY YOUR ");
            Assert.AreEqual(" with me to me you your my ", _tagHandler.Transform());
        }

        [TestMethod]
        public void TestEmptyInput()
        {
            XmlNode testNode = StaticHelpers.GetNode("<person2/>");
            _tagHandler = new Person2(_bot, _user, _query, _request, _result, testNode);
            _query.InputStar.Clear();
            Assert.AreEqual("", _tagHandler.Transform());
        }

        [TestMethod]
        public void TestNoMatches()
        {
            XmlNode testNode = StaticHelpers.GetNode("<person2>THE QUICK BROWN FOX JUMPED OVER THE LAZY DOGS</person2>");
            _tagHandler = new Person2(_bot, _user, _query, _request, _result, testNode);
            Assert.AreEqual("THE QUICK BROWN FOX JUMPED OVER THE LAZY DOGS", _tagHandler.Transform());
        }

        [TestMethod]
        public void TestNonAtomic()
        {
            XmlNode testNode = StaticHelpers.GetNode("<person2> WITH YOU TO YOU ME MY YOUR </person2>");
            _tagHandler = new Person2(_bot, _user, _query, _request, _result, testNode);
            Assert.AreEqual(" with me to me you your my ", _tagHandler.Transform());
        }
    }
}