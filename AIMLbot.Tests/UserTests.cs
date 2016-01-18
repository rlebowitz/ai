using System;
using System.Collections.Generic;
using System.Configuration;
using NUnit.Framework;

namespace AIMLbot.Tests
{
    [TestFixture]
    public class UserTests
    {
        private ChatBot _chatBot;
        private User _user;

        [OneTimeSetUp]
        public void SetupMockObjects()
        {
            _chatBot = new ChatBot();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _chatBot = null;
        }

        [Test]
        public void TestBadConstructor()
        {
            _user = new User("", _chatBot);
            Assert.That(() => new User("", _chatBot), Throws.TypeOf<Exception>());
        }

        [Test]
        public void TestConstructorPopulatesUserObject()
        {
            _user = new User("1", _chatBot);
            Assert.AreEqual("*", _user.Topic);
            Assert.AreEqual("1", _user.UserId);
            // the +1 in the following assert is the creation of the default topic predicate
            var predicates = ConfigurationManager.GetSection("Predicates") as Dictionary<string, string>;
            Assert.IsNotNull(predicates);
            Assert.AreEqual(predicates.Count + 1, _user.Predicates.Count);
        } 

        [Test]
        public void TestResultHandlers()
        {
            _user = new User("1", _chatBot);
            Assert.AreEqual("", _user.getResultSentence());
            var mockRequest = new Request("Sentence 1. Sentence 2", _user, _chatBot);
            var mockResult = new Result(_user, _chatBot, mockRequest);
            mockResult.InputSentences.Add("Result 1");
            mockResult.InputSentences.Add("Result 2");
            mockResult.OutputSentences.Add("Result 1");
            mockResult.OutputSentences.Add("Result 2");
            _user.AddResult(mockResult);
            var mockResult2 = new Result(_user, _chatBot, mockRequest);
            mockResult2.InputSentences.Add("Result 3");
            mockResult2.InputSentences.Add("Result 4");
            mockResult2.OutputSentences.Add("Result 3");
            mockResult2.OutputSentences.Add("Result 4");
            _user.AddResult(mockResult2);
            Assert.AreEqual("Result 3", _user.getResultSentence());
            Assert.AreEqual("Result 3", _user.getResultSentence(0));
            Assert.AreEqual("Result 1", _user.getResultSentence(1));
            Assert.AreEqual("Result 4", _user.getResultSentence(0, 1));
            Assert.AreEqual("Result 2", _user.getResultSentence(1, 1));
            Assert.AreEqual("", _user.getResultSentence(0, 2));
            Assert.AreEqual("", _user.getResultSentence(2, 0));
            Assert.AreEqual("Result 3", _user.getThat());
            Assert.AreEqual("Result 3", _user.getThat(0));
            Assert.AreEqual("Result 1", _user.getThat(1));
            Assert.AreEqual("Result 4", _user.getThat(0, 1));
            Assert.AreEqual("Result 2", _user.getThat(1, 1));
            Assert.AreEqual("", _user.getThat(0, 2));
            Assert.AreEqual("", _user.getThat(2, 0));
        }
    }
}