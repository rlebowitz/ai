using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AIMLbot.UnitTest
{
    [TestClass]
    public class RequestTests
    {
        private ChatBot _chatBot;

        private User _user;

        private Request _request;

        [TestInitialize]
        public void Setup()
        {
            _chatBot = new ChatBot();
            _user = new User("1", _chatBot);
        }

        [TestMethod]
        public void TestRequestConstructor()
        {
            _request = new Request("This is a test", _user, _chatBot);
            Assert.AreEqual("This is a test", _request.RawInput);
        }
    }
}