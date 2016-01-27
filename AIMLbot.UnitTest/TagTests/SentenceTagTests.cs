using System.Xml;
using AIMLbot.AIMLTagHandlers;
using AIMLbot.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AIMLbot.UnitTest.TagTests
{
    [TestClass]
    public class SentenceTagTests
    {
        private ChatBot _chatBot;
        private User _user;
        private Request _request;
        private Result _result;
        private SubQuery _query;
        private Sentence _botTagHandler;

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
        public void TestAtomic()
        {
            XmlNode testNode = StaticHelpers.GetNode("<sentence/>");
            _botTagHandler = new Sentence(_chatBot, _user, _query, _request, _result, testNode);
            _query.InputStar.Insert(0, "THIS IS. A TEST TO? SEE IF THIS; WORKS! OK");
            Assert.AreEqual("This is. A test to? See if this; Works! Ok", _botTagHandler.Transform());
        }

        [TestMethod]
        public void TestEmptyInput()
        {
            XmlNode testNode = StaticHelpers.GetNode("<sentence/>");
            _botTagHandler = new Sentence(_chatBot, _user, _query, _request, _result, testNode);
            _query.InputStar.Clear();
            Assert.AreEqual("", _botTagHandler.Transform());
        }

        [TestMethod]
        public void TestNonAtomicLower()
        {
            XmlNode testNode = StaticHelpers.GetNode("<sentence>this is. a test to? see if this; works! ok</sentence>");
            _botTagHandler = new Sentence(_chatBot, _user, _query, _request, _result, testNode);
            Assert.AreEqual("This is. A test to? See if this; Works! Ok", _botTagHandler.Transform());
        }

        [TestMethod]
        public void TestNonAtomicUpper()
        {
            XmlNode testNode = StaticHelpers.GetNode("<sentence>THIS IS. A TEST TO? SEE IF THIS; WORKS! OK</sentence>");
            _botTagHandler = new Sentence(_chatBot, _user, _query, _request, _result, testNode);
            Assert.AreEqual("This is. A test to? See if this; Works! Ok", _botTagHandler.Transform());
        }
    }
}