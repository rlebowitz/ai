using System;
using System.IO;
using AIMLbot.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AIMLbot.UnitTest.TagTests
{
    [TestClass]
    public class ConditionTagTests
    {
        private ChatBot _chatBot;
        private User _user;
        private Request _request;
        private Result _result;

        [TestInitialize]
        public void Setup()
        {
            var path = @"AIML\ChatBotTests.aiml";
            _chatBot = new ChatBot();
            _chatBot.LoadAIML(path);
            _user = new User("1", _chatBot);
        }

        [TestMethod]
        public void TestMultiCondition()
        {
            _request = new Request("test multi condition", _user, _chatBot);
            _user.Predicates.AddOrReplace("test1", "match1");
            _result = _chatBot.Chat(_request);
            Assert.AreEqual("test 1 match 1 found.", _result.RawOutput);
            _request = new Request("test multi condition", _user, _chatBot);
            _user.Predicates.AddOrReplace("test1", "match2");
            _result = _chatBot.Chat(_request);
            Assert.AreEqual("test 1 match 2 found.", _result.RawOutput);
            _user.Predicates.AddOrReplace("test1", "");
            _request = new Request("test multi condition", _user, _chatBot);
            _user.Predicates.AddOrReplace("test2", "match1");
            _result = _chatBot.Chat(_request);
            Assert.AreEqual("test 2 match 1 found.", _result.RawOutput);
            _request = new Request("test multi condition", _user, _chatBot);
            _user.Predicates.AddOrReplace("test2", "match2");
            _result = _chatBot.Chat(_request);
            Assert.AreEqual("test 2 match 2 found.", _result.RawOutput);
            _user.Predicates.AddOrReplace("test2", "");
            _request = new Request("test multi condition", _user, _chatBot);
            _user.Predicates.AddOrReplace("test3", "match test the star works");
            _result = _chatBot.Chat(_request);
            Assert.AreEqual("match * found.", _result.RawOutput);
            _user.Predicates.AddOrReplace("test3", "");
            _request = new Request("test multi condition", _user, _chatBot);
            _user.Predicates.AddOrReplace("test3", "match test the star won't match this");
            _result = _chatBot.Chat(_request);
            Assert.AreEqual("default match found.", _result.RawOutput);
            _request = new Request("test multi condition", _user, _chatBot);
            _user.Predicates.AddOrReplace("test", "match4");
            _result = _chatBot.Chat(_request);
            Assert.AreEqual("default match found.", _result.RawOutput);
        }

        [TestMethod]
        public void TestRecursiveBlockCondition()
        {
            _request = new Request("test block recursive call", _user, _chatBot);
            _result = _chatBot.Chat(_request);
            Assert.AreEqual("Test passed.", _result.RawOutput);
        }

        [TestMethod]
        public void TestSetAndCondition()
        {
            _request = new Request("TEST SET AND CONDITION", _user, _chatBot);
            _result = _chatBot.Chat(_request);
            Assert.AreEqual("End value: 1.", _result.RawOutput);
        }

        [TestMethod]
        public void TestSimpleBlockCondition()
        {
            _request = new Request("test block condition", _user, _chatBot);
            _result = _chatBot.Chat(_request);
            Assert.AreEqual("Test passed.", _result.RawOutput);
        }

        [TestMethod]
        public void TestSingleCondition()
        {
            _request = new Request("test single condition", _user, _chatBot);
            _user.Predicates.AddOrReplace("test", "match1");
            _result = _chatBot.Chat(_request);
            Assert.AreEqual("match 1 found.", _result.RawOutput);
            _request = new Request("test single condition", _user, _chatBot);
            _user.Predicates.AddOrReplace("test", "match2");
            _result = _chatBot.Chat(_request);
            Assert.AreEqual("match 2 found.", _result.RawOutput);
            _request = new Request("test single condition", _user, _chatBot);
            _user.Predicates.AddOrReplace("test", "match test the star works");
            _result = _chatBot.Chat(_request);
            Assert.AreEqual("match * found.", _result.RawOutput);
            _request = new Request("test single condition", _user, _chatBot);
            _user.Predicates.AddOrReplace("test", "match test the star won't match this");
            _result = _chatBot.Chat(_request);
            Assert.AreEqual("default match found.", _result.RawOutput);
            _request = new Request("test single condition", _user, _chatBot);
            _user.Predicates.AddOrReplace("test", "match4");
            _result = _chatBot.Chat(_request);
            Assert.AreEqual("default match found.", _result.RawOutput);
        }
    }
}