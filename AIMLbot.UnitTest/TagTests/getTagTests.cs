using AIMLbot.AIMLTagHandlers;
using AIMLbot.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AIMLbot.UnitTest.TagTests
{
    [TestClass]
    public class GetTagTests
    {
        private Get _botTagHandler;
        private ChatBot _chatBot;
        private SubQuery _query;
        private Request _request;
        private Result _result;
        private User _user;

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
        public void TestWithBadAttribute()
        {
            var testNode = StaticHelpers.GetNode("<get nome=\"we\"/>");
            _botTagHandler = new Get(_chatBot, _user, _query, _request, _result, testNode);
            Assert.AreEqual("", _botTagHandler.Transform());
        }

        [TestMethod]
        public void TestWithBadNode()
        {
            var testNode = StaticHelpers.GetNode("<got name=\"we\"/>");
            _botTagHandler = new Get(_chatBot, _user, _query, _request, _result, testNode);
            Assert.AreEqual("", _botTagHandler.Transform());
        }

        [TestMethod]
        public void TestWithGoodData()
        {
            // first element
            var testNode = StaticHelpers.GetNode("<get name=\"name\"/>");
            _botTagHandler = new Get(_chatBot, _user, _query, _request, _result, testNode);
            Assert.AreEqual("un-named user", _botTagHandler.Transform());

            // last element
            testNode = StaticHelpers.GetNode("<get name=\"we\"/>");
            _botTagHandler = new Get(_chatBot, _user, _query, _request, _result, testNode);
            Assert.AreEqual("unknown", _botTagHandler.Transform());
        }

        [TestMethod]
        public void TestWithNoAttributes()
        {
            var testNode = StaticHelpers.GetNode("<get/>");
            _botTagHandler = new Get(_chatBot, _user, _query, _request, _result, testNode);
            Assert.AreEqual("", _botTagHandler.Transform());
        }

        [TestMethod]
        public void TestWithNonExistentPredicate()
        {
            var testNode = StaticHelpers.GetNode("<get name=\"nonexistent\"/>");
            _botTagHandler = new Get(_chatBot, _user, _query, _request, _result, testNode);
            Assert.AreEqual("", _botTagHandler.Transform());
        }

        [TestMethod]
        public void TestWithTooManyAttributes()
        {
            var testNode = StaticHelpers.GetNode("<get name=\"we\" value=\"value\" />");
            _botTagHandler = new Get(_chatBot, _user, _query, _request, _result, testNode);
            Assert.AreEqual("", _botTagHandler.Transform());
        }
    }
}