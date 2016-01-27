using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AIMLbot.UnitTest
{
    [TestClass]
    public class ResultTests
    {
        private User _user;

        private Request _request;

        private Result _result;

        [TestInitialize]
        public void Setup()
        {
            _user = new User();
            _request = new Request("This is a test", _user);
        }

        [TestMethod]
        public void TestConstructor()
        {
            _result = new Result(_user, _request);
            Assert.AreEqual("This is a test", _result.RawInput);
        }
    }
}