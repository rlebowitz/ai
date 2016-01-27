using System;
using System.Xml;
using AIMLbot.AIMLTagHandlers;
using AIMLbot.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AIMLbot.UnitTest.TagTests
{
    [TestClass]
    public class SraiTagTests
    {
        private ChatBot _chatBot;
        private User _user;
        private Request _request;
        private Result _result;
        private SubQuery _query;
        private Srai _botTagHandler;

        [TestInitialize]
        public void Setup()
        {
            _chatBot = new ChatBot();
            var filePath = $@"{Environment.CurrentDirectory}\AIML\Srai.aiml";
            _chatBot.LoadAIML(filePath);
            _user = new User("1", _chatBot);
            _request = new Request("This is a test", _user, _chatBot);
            _query = new SubQuery();
            _query.InputStar.Insert(0, "first star");
            _query.InputStar.Insert(0, "second star");
            _result = new Result(_user, _chatBot, _request);
        }

        [TestMethod]
        public void TestSRAIBad()
        {
            XmlNode testNode = StaticHelpers.GetNode("<srui>srainested</srui>");
            _botTagHandler = new Srai(_chatBot, _user, _query, _request, _result, testNode);
            Assert.AreEqual("", _botTagHandler.Transform());
        }

        [TestMethod]
        public void TestSRAIEmpty()
        {
            XmlNode testNode = StaticHelpers.GetNode("<srai/>");
            _botTagHandler = new Srai(_chatBot, _user, _query, _request, _result, testNode);
            Assert.AreEqual("", _botTagHandler.Transform());
        }

        [TestMethod]
        public void TestSRAIRecursion()
        {
            XmlNode testNode = StaticHelpers.GetNode("<srai>srainested</srai>");
            _botTagHandler = new Srai(_chatBot, _user, _query, _request, _result, testNode);
            Assert.AreEqual("Test passed.", _botTagHandler.Transform());
        }

        [TestMethod]
        public void TestSRAIWithValidInput()
        {
            XmlNode testNode = StaticHelpers.GetNode("<srai>sraisucceeded</srai>");
            _botTagHandler = new Srai(_chatBot, _user, _query, _request, _result, testNode);
            Assert.AreEqual("Test passed.", _botTagHandler.Transform());
        }
    }
}