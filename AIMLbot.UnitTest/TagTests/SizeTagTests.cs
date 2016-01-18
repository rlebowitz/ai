using System;
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

        public static int Size = 30;

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
        public void TestWithBadXml()
        {
            XmlNode testNode = StaticHelpers.getNode("<soze/>");
            _botTagHandler = new Size(_chatBot, _user, _query, _request, _result, testNode);
            Assert.AreEqual("", _botTagHandler.Transform());
        }

        [TestMethod]
        public void TestWithValidData()
        {
            XmlNode testNode = StaticHelpers.getNode("<size/>");
            _botTagHandler = new Size(_chatBot, _user, _query, _request, _result, testNode);
            Assert.AreEqual("0", _botTagHandler.Transform());
            AIMLLoader loader = new AIMLLoader(_chatBot);
            loader.LoadAIML();
            Assert.AreEqual(Convert.ToString(Size), _botTagHandler.Transform());
        }
    }
}