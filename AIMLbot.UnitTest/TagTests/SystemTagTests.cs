using AIMLbot.AIMLTagHandlers;
using AIMLbot.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AIMLbot.UnitTest.TagTests
{
    [TestClass]
    public class SystemTagTests
    {
        private SystemTag _botTagHandler;
        private ChatBot _chatBot;
        private SubQuery _query;
        private Request _request;
        private Result _result;
        private User _user;

        [TestInitialize]
        public void Setup()
        {
            _chatBot = new ChatBot();
            _user = new User();
            _request = new Request("This is a test", _user);
            _query = new SubQuery();
            _result = new Result(_user, _request);
        }

        [TestMethod]
        public void TestSystemIsNotImplemented()
        {
            var testNode = StaticHelpers.GetNode("<system/>");
            _botTagHandler = new SystemTag(_chatBot, _user, _query, _request, _result, testNode);
            Assert.AreEqual("", _botTagHandler.Transform());
        }
    }
}