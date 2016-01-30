using AIMLbot.AIMLTagHandlers;
using AIMLbot.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AIMLbot.UnitTest.TagTests
{
    [TestClass]
    public class TopicStarTagTests
    {
        private Topicstar _tagHandler;
        private SubQuery _query;
        private Request _request;

        [TestInitialize]
        public void Setup()
        {
            _request = new Request("This is a test", new User());
            _query = new SubQuery();
            _query.TopicStar.Insert(0, "first star");
            _query.TopicStar.Insert(0, "second star");
        }

        [TestMethod]
        public void TestAtomic()
        {
            var testNode = StaticHelpers.GetNode("<topicstar/>");
            _tagHandler = new Topicstar(_query, _request, testNode);
            Assert.AreEqual("second star", _tagHandler.ProcessChange());
        }

        [TestMethod]
        public void TestBadIndex()
        {
            var testNode = StaticHelpers.GetNode("<topicstar index=\"two\"/>");
            _tagHandler = new Topicstar(_query, _request, testNode);
            Assert.AreEqual("", _tagHandler.ProcessChange());
        }

        [TestMethod]
        public void TestIndexed()
        {
            var testNode = StaticHelpers.GetNode("<topicstar index=\"1\"/>");
            _tagHandler = new Topicstar(_query, _request, testNode);
            Assert.AreEqual("second star", _tagHandler.ProcessChange());
        }

        [TestMethod]
        public void TestIndexed2()
        {
            var testNode = StaticHelpers.GetNode("<topicstar index=\"2\"/>");
            _tagHandler = new Topicstar(_query, _request, testNode);
            Assert.AreEqual("first star", _tagHandler.ProcessChange());
        }

        [TestMethod]
        public void TestOutOfBounds()
        {
            var testNode = StaticHelpers.GetNode("<topicstar index=\"3\"/>");
            _tagHandler = new Topicstar(_query, _request, testNode);
            Assert.AreEqual("", _tagHandler.ProcessChange());
        }
    }
}