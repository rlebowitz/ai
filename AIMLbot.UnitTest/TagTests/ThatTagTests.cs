using AIMLbot.AIMLTagHandlers;
using AIMLbot.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AIMLbot.UnitTest.TagTests
{
    [TestClass]
    public class ThatTagTests
    {
        private That _tagHandler;
        private SubQuery _query;
        private Request _request;
        private User _user;

        [TestInitialize]
        public void Setup()
        {
            _user = new User();
            _request = new Request("This is a test", _user);
            _query = new SubQuery();
            _query.InputStar.Insert(0, "first star");
            _query.InputStar.Insert(0, "second star");
        }

        [TestMethod]
        public void TestResultHandlers()
        {
            var testNode = StaticHelpers.GetNode("<that/>");
            var mockResult = new Result(_user, _request);
            _tagHandler = new That(_user, _request, testNode);
            Assert.AreEqual("", _tagHandler.ProcessChange());
            _request = new Request("Sentence 1. Sentence 2", _user);
            mockResult.OutputSentences.Add("Result 1");
            mockResult.OutputSentences.Add("Result 2");
            _user.AddResult(mockResult);
            var mockResult2 = new Result(_user, _request);
            mockResult2.OutputSentences.Add("Result 3");
            mockResult2.OutputSentences.Add("Result 4");
            _user.AddResult(mockResult2);

            Assert.AreEqual("Result 3", _tagHandler.ProcessChange());

            testNode = StaticHelpers.GetNode("<that index=\"1\"/>");
            _tagHandler = new That(_user, _request, testNode);
            Assert.AreEqual("Result 3", _tagHandler.ProcessChange());

            testNode = StaticHelpers.GetNode("<that index=\"2,1\"/>");
            _tagHandler = new That(_user, _request, testNode);
            Assert.AreEqual("Result 1", _tagHandler.ProcessChange());

            testNode = StaticHelpers.GetNode("<that index=\"1,2\"/>");
            _tagHandler = new That(_user, _request, testNode);
            Assert.AreEqual("Result 4", _tagHandler.ProcessChange());

            testNode = StaticHelpers.GetNode("<that index=\"2,2\"/>");
            _tagHandler = new That(_user, _request, testNode);
            Assert.AreEqual("Result 2", _tagHandler.ProcessChange());

            testNode = StaticHelpers.GetNode("<that index=\"1,3\"/>");
            _tagHandler = new That(_user, _request, testNode);
            Assert.AreEqual("", _tagHandler.ProcessChange());

            testNode = StaticHelpers.GetNode("<that index=\"3\"/>");
            _tagHandler = new That(_user, _request, testNode);
            Assert.AreEqual("", _tagHandler.ProcessChange());
        }
    }
}