using System.Xml;
using AIMLbot.AIMLTagHandlers;
using AIMLbot.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AIMLbot.UnitTest.TagTests
{
    [TestClass]
    public class InputTagTests
    {
        private ChatBot _chat;
        private User _user;
        private Request _request;
        private SubQuery _query;
        private Input _botTagHandler;

        [TestInitialize]
        public void Setup()
        {
            _chat = new ChatBot();
            _user = new User("1", _chat);
            _request = new Request("This is a test", _user, _chat);
            _query = new SubQuery("This is a test <that> * <topic> *");
            _query.InputStar.Insert(0, "first star");
            _query.InputStar.Insert(0, "second star");
            //this.mockResult = new Result(this._user, this._chat, this._request);
        }

        [TestMethod]
        public void testResultHandlers()
        {
            XmlNode testNode = StaticHelpers.getNode("<input/>");
            Result mockResult = new Result(_user, _chat, _request);
            _botTagHandler = new Input(_chat, _user, _query, _request, mockResult, testNode);
            Assert.AreEqual("", _botTagHandler.Transform());
            _request = new Request("Sentence 1. Sentence 2", _user, _chat);
            mockResult.InputSentences.Add("Result 1");
            mockResult.InputSentences.Add("Result 2");
            _user.AddResult(mockResult);
            Result mockResult2 = new Result(_user, _chat, _request);
            mockResult2.InputSentences.Add("Result 3");
            mockResult2.InputSentences.Add("Result 4");
            _user.AddResult(mockResult2);

            Assert.AreEqual("Result 3", _botTagHandler.Transform());

            testNode = StaticHelpers.getNode("<input index=\"1\"/>");
            _botTagHandler = new Input(_chat, _user, _query, _request, mockResult, testNode);
            Assert.AreEqual("Result 3", _botTagHandler.Transform());

            testNode = StaticHelpers.getNode("<input index=\"2,1\"/>");
            _botTagHandler = new Input(_chat, _user, _query, _request, mockResult, testNode);
            Assert.AreEqual("Result 1", _botTagHandler.Transform());

            testNode = StaticHelpers.getNode("<input index=\"1,2\"/>");
            _botTagHandler = new Input(_chat, _user, _query, _request, mockResult, testNode);
            Assert.AreEqual("Result 4", _botTagHandler.Transform());

            testNode = StaticHelpers.getNode("<input index=\"2,2\"/>");
            _botTagHandler = new Input(_chat, _user, _query, _request, mockResult, testNode);
            Assert.AreEqual("Result 2", _botTagHandler.Transform());

            testNode = StaticHelpers.getNode("<input index=\"1,3\"/>");
            _botTagHandler = new Input(_chat, _user, _query, _request, mockResult, testNode);
            Assert.AreEqual("", _botTagHandler.Transform());

            testNode = StaticHelpers.getNode("<input index=\"3\"/>");
            _botTagHandler = new Input(_chat, _user, _query, _request, mockResult, testNode);
            Assert.AreEqual("", _botTagHandler.Transform());
        }
    }
}