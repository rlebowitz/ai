using System.Xml;
using AIMLbot.AIMLTagHandlers;
using AIMLbot.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AIMLbot.UnitTest.TagTests
{
    [TestClass]
    public class InputTagTests
    {
        private User _user;
        private Request _request;
        private SubQuery _query;
        private Input _botTagHandler;

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
        public void testResultHandlers()
        {
            XmlNode testNode = StaticHelpers.GetNode("<input/>");
            Result mockResult = new Result(_user, _request);
            _botTagHandler = new Input(_user, _request, testNode);
            Assert.AreEqual("", _botTagHandler.ProcessChange());
            _request = new Request("Sentence 1. Sentence 2", _user);
            mockResult.InputSentences.Add("Result 1");
            mockResult.InputSentences.Add("Result 2");
            _user.AddResult(mockResult);
            Result mockResult2 = new Result(_user, _request);
            mockResult2.InputSentences.Add("Result 3");
            mockResult2.InputSentences.Add("Result 4");
            _user.AddResult(mockResult2);

            Assert.AreEqual("Result 3", _botTagHandler.ProcessChange());

            testNode = StaticHelpers.GetNode("<input index=\"1\"/>");
            _botTagHandler = new Input(_user, _request, testNode);
            Assert.AreEqual("Result 3", _botTagHandler.ProcessChange());

            testNode = StaticHelpers.GetNode("<input index=\"2,1\"/>");
            _botTagHandler = new Input(_user, _request, testNode);
            Assert.AreEqual("Result 1", _botTagHandler.ProcessChange());

            testNode = StaticHelpers.GetNode("<input index=\"1,2\"/>");
            _botTagHandler = new Input(_user, _request, testNode);
            Assert.AreEqual("Result 4", _botTagHandler.ProcessChange());

            testNode = StaticHelpers.GetNode("<input index=\"2,2\"/>");
            _botTagHandler = new Input(_user, _request, testNode);
            Assert.AreEqual("Result 2", _botTagHandler.ProcessChange());

            testNode = StaticHelpers.GetNode("<input index=\"1,3\"/>");
            _botTagHandler = new Input(_user, _request, testNode);
            Assert.AreEqual("", _botTagHandler.ProcessChange());

            testNode = StaticHelpers.GetNode("<input index=\"3\"/>");
            _botTagHandler = new Input(_user, _request, testNode);
            Assert.AreEqual("", _botTagHandler.ProcessChange());
        }
    }
}