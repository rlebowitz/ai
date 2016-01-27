using System;
using System.Xml;
using AIMLbot.AIMLTagHandlers;
using AIMLbot.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AIMLbot.UnitTest.TagTests
{
    [TestClass]
    public class SrTagTests
    {
        private ChatBot _chatBot;
        private User _user;
        private Request _request;
        private Result _result;
        private SubQuery _query;
        private Sr _botTagHandler;

        [TestInitialize]
        public void Setup()
        {
            _chatBot = new ChatBot();
            var filePath = $@"{Environment.CurrentDirectory}\AIML\Srai.aiml";
            _chatBot.LoadAIML(filePath);
            _user = new User();
            _request = new Request("This is a test", _user);
            _query = new SubQuery();
            _query.InputStar.Insert(0, "first star");
            _query.InputStar.Insert(0, "second star");
            _result = new Result(_user, _request);
        }

        [TestMethod]
        public void TestSRAIBad()
        {
            XmlNode testNode = StaticHelpers.GetNode("<se/>");
            _botTagHandler = new Sr(_chatBot, _user, _query, _request, _result, testNode);
            Assert.AreEqual("", _botTagHandler.Transform());
        }

        [TestMethod]
        public void TestSRAIEmpty()
        {
            XmlNode testNode = StaticHelpers.GetNode("<sr/>");
            _botTagHandler = new Sr(_chatBot, _user, _query, _request, _result, testNode);
            Assert.AreEqual("", _botTagHandler.Transform());
        }

        [TestMethod]
        public void TestSRAIRecursion()
        {
            XmlNode testNode = StaticHelpers.GetNode("<sr/>");
            _query.InputStar.Insert(0, "srainested");
            _botTagHandler = new Sr(_chatBot, _user, _query, _request, _result, testNode);
            Assert.AreEqual("Test passed.", _botTagHandler.Transform());
        }

        [TestMethod]
        public void TestSRAIWithValidInput()
        {
            XmlNode testNode = StaticHelpers.GetNode("<sr/>");
            _query.InputStar.Insert(0, "sraisucceeded");
            _botTagHandler = new Sr(_chatBot, _user, _query, _request, _result, testNode);
            Assert.AreEqual("Test passed.", _botTagHandler.Transform());
        }
    }
}