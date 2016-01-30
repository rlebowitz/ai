using System.Linq;
using System.Xml;
using AIMLbot.AIMLTagHandlers;
using log4net.Appender;
using log4net.Config;
using log4net.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AIMLbot.UnitTest.TagTests
{
    [TestClass]
    public class GossipTagTests
    {
        private User _user;
        private Gossip _tagHandler;
        private MemoryAppender _appender;

        [TestInitialize]
        public void Setup()
        {
            _appender = new MemoryAppender();
            BasicConfigurator.Configure(_appender);
            _user = new User("1");
        }

        [TestMethod]
        public void TestGossipWithEmpty()
        {
            XmlNode testNode = StaticHelpers.GetNode("<gossip/>");
            _tagHandler = new Gossip(_user, testNode);
            _tagHandler.ProcessChange();
            Assert.IsFalse(_appender.GetEvents().Any(le => le.Level == Level.Error),
                "Did not expect any error messages in the logs");
        }

        [TestMethod]
        public void TestGossipWithGoodData()
        {
            XmlNode testNode = StaticHelpers.GetNode("<gossip>this is gossip</gossip>");
            _tagHandler = new Gossip(_user, testNode);
            _tagHandler.ProcessChange();
            var last = _appender.GetEvents().Last();
            Assert.AreEqual("GOSSIP from user: 1, 'this is gossip'", last.RenderedMessage);
        }
    }
}