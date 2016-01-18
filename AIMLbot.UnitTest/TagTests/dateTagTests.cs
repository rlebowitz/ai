using System;
using System.Xml;
using AIMLbot.AIMLTagHandlers;
using AIMLbot.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AIMLbot.UnitTest.TagTests
{
    [TestClass]
    public class DateTagTests
    {
        private ChatBot _chatBot;
        private User _user;
        private Request _request;
        private Result _result;
        private SubQuery _query;
        private Date _botTagHandler;

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
        public void TestBadInput()
        {
            XmlNode testNode = StaticHelpers.getNode("<dote/>");
            _botTagHandler = new Date(_chatBot, _user, _query, _request, _result, testNode);
            Assert.AreEqual("", _botTagHandler.Transform());
        }

        [TestMethod]
        public void TestExpectedInput()
        {
            XmlNode testNode = StaticHelpers.getNode("<date/>");
            _botTagHandler = new Date(_chatBot, _user, _query, _request, _result, testNode);
            DateTime now = DateTime.Now;
            DateTime expected = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
            Assert.AreEqual(expected.ToString(_chatBot.Locale), _botTagHandler.Transform());
        }
    }
}