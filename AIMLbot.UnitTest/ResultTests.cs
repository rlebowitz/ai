using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AIMLbot.UnitTest
{
    [TestClass]
    public class ResultTests
    {
        private ChatBot _chatBot;

        private User _user;

        private Request _request;

        private Result _result;

        [TestInitialize]
        public void Setup()
        {
            _chatBot = new ChatBot();
            _user = new User("1", _chatBot);
            _request = new Request("This is a test", _user, _chatBot);
        }

        [TestMethod]
        public void TestConstructor()
        {
            _result = new Result(_user, _chatBot, _request);
            Assert.AreEqual("This is a test", _result.RawInput);
        }
    }
}