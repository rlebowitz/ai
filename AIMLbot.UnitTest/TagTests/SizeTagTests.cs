using System;
using System.IO;
using System.Xml;
using AIMLbot.AIMLTagHandlers;
using AIMLbot.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AIMLbot.UnitTest.TagTests
{
    [TestClass]
    public class SizeTagTests
    {
        private ChatBot _chatBot;
        private User _user;
        private Request _request;
        private Result _result;
        private SubQuery _query;
        private Size _botTagHandler;
        private AIMLLoader _loader;

        public static int Size = 16;

        [TestInitialize]
        public void Setup()
        {
            _chatBot = new ChatBot();
            _loader = new AIMLLoader(_chatBot);
            _user = new User("1", _chatBot);
            _request = new Request("This is a test", _user, _chatBot);
            _query = new SubQuery();
            _result = new Result(_user, _chatBot, _request);
            var path = $@"{Environment.CurrentDirectory}\AIML\Salutations.aiml";
            _loader.LoadAIML(path);
        }

        [TestMethod]
        public void TestWithBadXml()
        {
            XmlNode testNode = StaticHelpers.GetNode("<soze/>");
            _botTagHandler = new Size(_chatBot, _user, _query, _request, _result, testNode);
            Assert.AreEqual("", _botTagHandler.Transform());
        }

        [TestMethod]
        public void TestWithValidData()
        {
            XmlNode testNode = StaticHelpers.GetNode("<size/>");
            _botTagHandler = new Size(_chatBot, _user, _query, _request, _result, testNode);
            Assert.AreEqual(Convert.ToString(Size), _botTagHandler.Transform());
        }
    }
}