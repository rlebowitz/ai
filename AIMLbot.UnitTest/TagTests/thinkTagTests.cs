using System.Linq;
using System.Xml;
using AIMLbot.AIMLTagHandlers;
using AIMLbot.Utils;
using log4net.Appender;
using log4net.Config;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AIMLbot.UnitTest.TagTests
{
    [TestClass]
    public class ThinkTagTests
    {
        private ChatBot _chatBot;
        private User _user;
        private Request _request;
        private Result _result;
        private SubQuery _query;
        private Think _botTagHandler;
        private MemoryAppender _appender;

        [TestInitialize]
        public void Setup()
        {
            _appender = new MemoryAppender();
            BasicConfigurator.Configure(_appender);

            _chatBot = new ChatBot();
            _user = new User("1", _chatBot);
            _request = new Request("This is a test", _user, _chatBot);
            _query = new SubQuery("This is a test <that> * <topic> *");
            _result = new Result(_user, _chatBot, _request);
        }

        [TestMethod]
        public void TestAsPartOfLargerTemplate()
        {
            _chatBot.LoadAIMLFromFiles();
            Result newResult = _chatBot.Chat("test think", "1");
            Assert.AreEqual("You should see this.", newResult.RawOutput);
        }

        [TestMethod]
        public void TestExpectedInput()
        {
            XmlNode testNode = StaticHelpers.getNode("<think>This is a test</think>");
            _botTagHandler = new Think(_chatBot, _user, _query, _request, _result, testNode);
            Assert.AreEqual("", _botTagHandler.Transform());
        }

        [TestMethod]
        public void TestWithChildNodes()
        {
            _chatBot.LoadAIMLFromFiles();
            Result newResult = _chatBot.Chat("test child think", "1");
            Assert.AreEqual("You should see this.", newResult.RawOutput);
            var last = _appender.GetEvents().Last();
            Assert.AreEqual("GOSSIP from user: 1, 'some gossip'", last.RenderedMessage);
        }
    }
}