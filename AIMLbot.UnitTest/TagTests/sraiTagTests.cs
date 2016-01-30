using System;
using AIMLbot.AIMLTagHandlers;
using AIMLbot.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AIMLbot.UnitTest.TagTests
{
    [TestClass]
    public class SraiTagTests
    {
        private Srai _tagHandler;
        private ChatBot _chatBot;
        private SubQuery _query;
        private Request _request;
        private User _user;

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
        }

        [TestMethod]
        public void TestSRAIBad()
        {
            var testNode = StaticHelpers.GetNode("<srui>srainested</srui>");
            _tagHandler = new Srai(_chatBot, _user, _request, testNode);
            Assert.AreEqual("", _tagHandler.ProcessChange());
        }

        [TestMethod]
        public void TestSRAIEmpty()
        {
            var testNode = StaticHelpers.GetNode("<srai/>");
            _tagHandler = new Srai(_chatBot, _user, _request, testNode);
            Assert.AreEqual("", _tagHandler.ProcessChange());
        }

        [TestMethod]
        public void TestSRAIRecursion()
        {
            var testNode = StaticHelpers.GetNode("<srai>srainested</srai>");
            _tagHandler = new Srai(_chatBot, _user, _request, testNode);
            Assert.AreEqual("Test passed.", _tagHandler.ProcessChange());
        }

        [TestMethod]
        public void TestSRAIWithValidInput()
        {
            var testNode = StaticHelpers.GetNode("<srai>sraisucceeded</srai>");
            _tagHandler = new Srai(_chatBot, _user, _request, testNode);
            Assert.AreEqual("Test passed.", _tagHandler.ProcessChange());
        }
    }
}