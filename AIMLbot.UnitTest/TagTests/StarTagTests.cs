using AIMLbot.AIMLTagHandlers;
using AIMLbot.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AIMLbot.UnitTest.TagTests
{
    [TestClass]
    public class StarTagTests
    {
        private Star _tagHandler;
        private SubQuery _query;
        private Request _request;

        [TestInitialize]
        public void Setup()
        {
            _request = new Request("This is a test", new User());
            _query = new SubQuery();
            _query.InputStar.Insert(0, "first star");
            _query.InputStar.Insert(0, "second star");
        }

        [TestMethod]
        public void TestBadInputAttributeName()
        {
            var testNode = StaticHelpers.GetNode("<star indox=\"3\"/>");
            _tagHandler = new Star(_query, _request, testNode);
            Assert.AreEqual("", _tagHandler.ProcessChange());
        }

        [TestMethod]
        public void TestBadInputAttributeValue()
        {
            var testNode = StaticHelpers.GetNode("<star index=\"one\"/>");
            _tagHandler = new Star(_query, _request, testNode);
            Assert.AreEqual("", _tagHandler.ProcessChange());
        }

        [TestMethod]
        public void TestBadInputTagName()
        {
            var testNode = StaticHelpers.GetNode("<stor index=\"1\"/>");
            _tagHandler = new Star(_query, _request, testNode);
            Assert.AreEqual("", _tagHandler.ProcessChange());
        }

        [TestMethod]
        public void TestExpectedInput()
        {
            var testNode = StaticHelpers.GetNode("<star/>");
            _tagHandler = new Star(_query, _request, testNode);
            Assert.AreEqual("second star", _tagHandler.ProcessChange());
        }

        [TestMethod]
        public void TestExpectedInputIndex()
        {
            var testNode = StaticHelpers.GetNode("<star index=\"1\"/>");
            _tagHandler = new Star(_query, _request, testNode);
            Assert.AreEqual("second star", _tagHandler.ProcessChange());
        }

        [TestMethod]
        public void TestExpectedInputIndexOutOfBounds()
        {
            var testNode = StaticHelpers.GetNode("<star index=\"3\"/>");
            _tagHandler = new Star(_query, _request, testNode);
            Assert.AreEqual("", _tagHandler.ProcessChange());
        }

        [TestMethod]
        public void TestExpectedInputIndexSecond()
        {
            var testNode = StaticHelpers.GetNode("<star index=\"2\"/>");
            _tagHandler = new Star(_query, _request, testNode);
            Assert.AreEqual("first star", _tagHandler.ProcessChange());
        }
    }
}