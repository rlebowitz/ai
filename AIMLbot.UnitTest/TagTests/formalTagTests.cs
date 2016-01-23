using AIMLbot.AIMLTagHandlers;
using AIMLbot.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AIMLbot.UnitTest.TagTests
{
    [TestClass]
    public class FormalTagTests
    {
        private Formal _botTagHandler;
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
        public void TestEmptyInput()
        {
            var testNode = StaticHelpers.GetNode("<formal/>");
            _botTagHandler = new Formal(_chatBot, _user, _query, _request, _result, testNode);
            Assert.AreEqual("", _botTagHandler.Transform());
        }

        [TestMethod]
        public void TestExpectedCapitalizedInput()
        {
            var testNode = StaticHelpers.GetNode("<formal>THIS IS A TEST</formal>");
            _botTagHandler = new Formal(_chatBot, _user, _query, _request, _result, testNode);
            Assert.AreEqual("This Is A Test", _botTagHandler.Transform());
        }

        [TestMethod]
        public void TestExpectedInput()
        {
            var testNode = StaticHelpers.GetNode("<formal>this is a test</formal>");
            _botTagHandler = new Formal(_chatBot, _user, _query, _request, _result, testNode);
            Assert.AreEqual("This Is A Test", _botTagHandler.Transform());
        }
    }
}