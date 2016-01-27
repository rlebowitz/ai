using System.Xml;
using AIMLbot.AIMLTagHandlers;
using AIMLbot.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AIMLbot.UnitTest.TagTests
{
    [TestClass]
    public class ThatTagTests
    {
        private ChatBot _chatBot;
        private User _user;
        private Request _request;
        private SubQuery _query;
        private That _botTagHandler;

        [TestInitialize]
        public void Setup()
        {
            _chatBot = new ChatBot();
            _user = new User("1", _chatBot);
            _request = new Request("This is a test", _user, _chatBot);
            _query = new SubQuery();
            _query.InputStar.Insert(0, "first star");
            _query.InputStar.Insert(0, "second star");
            //this.mockResult = new Result(this._user, this._chatBot, this._request);
        }

        [TestMethod]
        public void TestResultHandlers()
        {
            XmlNode testNode = StaticHelpers.GetNode("<that/>");
            Result mockResult = new Result(_user, _chatBot, _request);
            _botTagHandler = new That(_chatBot, _user, _query, _request, mockResult, testNode);
            Assert.AreEqual("", _botTagHandler.Transform());
            _request = new Request("Sentence 1. Sentence 2", _user, _chatBot);
            mockResult.OutputSentences.Add("Result 1");
            mockResult.OutputSentences.Add("Result 2");
            _user.AddResult(mockResult);
            Result mockResult2 = new Result(_user, _chatBot, _request);
            mockResult2.OutputSentences.Add("Result 3");
            mockResult2.OutputSentences.Add("Result 4");
            _user.AddResult(mockResult2);

            Assert.AreEqual("Result 3", _botTagHandler.Transform());

            testNode = StaticHelpers.GetNode("<that index=\"1\"/>");
            _botTagHandler = new That(_chatBot, _user, _query, _request, mockResult, testNode);
            Assert.AreEqual("Result 3", _botTagHandler.Transform());

            testNode = StaticHelpers.GetNode("<that index=\"2,1\"/>");
            _botTagHandler = new That(_chatBot, _user, _query, _request, mockResult, testNode);
            Assert.AreEqual("Result 1", _botTagHandler.Transform());

            testNode = StaticHelpers.GetNode("<that index=\"1,2\"/>");
            _botTagHandler = new That(_chatBot, _user, _query, _request, mockResult, testNode);
            Assert.AreEqual("Result 4", _botTagHandler.Transform());

            testNode = StaticHelpers.GetNode("<that index=\"2,2\"/>");
            _botTagHandler = new That(_chatBot, _user, _query, _request, mockResult, testNode);
            Assert.AreEqual("Result 2", _botTagHandler.Transform());

            testNode = StaticHelpers.GetNode("<that index=\"1,3\"/>");
            _botTagHandler = new That(_chatBot, _user, _query, _request, mockResult, testNode);
            Assert.AreEqual("", _botTagHandler.Transform());

            testNode = StaticHelpers.GetNode("<that index=\"3\"/>");
            _botTagHandler = new That(_chatBot, _user, _query, _request, mockResult, testNode);
            Assert.AreEqual("", _botTagHandler.Transform());
        }
    }
}