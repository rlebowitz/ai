using System;
using System.Linq;
using AIMLbot.AIMLTagHandlers;
using log4net.Appender;
using log4net.Config;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AIMLbot.UnitTest.TagTests
{
    [TestClass]
    public class ThinkTagTests
    {
        private MemoryAppender _appender;
        private Think _botTagHandler;
        private ChatBot _chatBot;

        [TestInitialize]
        public void Setup()
        {
            _appender = new MemoryAppender();
            BasicConfigurator.Configure(_appender);
            _chatBot = new ChatBot();
            var filePath = $@"{Environment.CurrentDirectory}\AIML\ChatBotTests.aiml";
            _chatBot.LoadAIML(filePath);
        }

        [TestMethod]
        public void TestAsPartOfLargerTemplate()
        {
            var newResult = _chatBot.Chat("test think", "1");
            Assert.AreEqual("You should see this.", newResult.RawOutput);
        }

        [TestMethod]
        public void TestExpectedInput()
        {
            var testNode = StaticHelpers.GetNode("<think>This is a test</think>");
            _botTagHandler = new Think(testNode);
            Assert.AreEqual("", _botTagHandler.ProcessChange());
        }

        [TestMethod]
        public void TestWithChildNodes()
        {
            var newResult = _chatBot.Chat("test child think", "1");
            Assert.AreEqual("You should see this.", newResult.RawOutput);
            var last = _appender.GetEvents().Last();
            Assert.AreEqual("GOSSIP from user: 1, 'some gossip'", last.RenderedMessage);
        }
    }
}