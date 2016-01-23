using System.Linq;
using System.Xml;
using AIMLbot.AIMLTagHandlers;
using AIMLbot.Utils;
using log4net.Appender;
using log4net.Config;
using log4net.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AIMLbot.UnitTest.TagTests
{
    [TestClass]
    public class GossipTagTests
    {
        private ChatBot _chatBot;
        private User _user;
        private Request _request;
        private Result _result;
        private SubQuery _query;
        private Gossip _botTagHandler;
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
        public void TestGossipWithEmpty()
        {
            XmlNode testNode = StaticHelpers.GetNode("<gossip/>");
            _botTagHandler = new Gossip(_chatBot, _user, _query, _request, _result, testNode);
            _botTagHandler.Transform();
            Assert.IsFalse(_appender.GetEvents().Any(le => le.Level == Level.Error),
            "Did not expect any error messages in the logs");
        }

        [TestMethod]
        public void TestGossipWithGoodData()
        {
            XmlNode testNode = StaticHelpers.GetNode("<gossip>this is gossip</gossip>");
            _botTagHandler = new Gossip(_chatBot, _user, _query, _request, _result, testNode);
            _botTagHandler.Transform();
            var last = _appender.GetEvents().Last();
            Assert.AreEqual("GOSSIP from user: 1, 'this is gossip'", last.RenderedMessage);
        }
    }
}