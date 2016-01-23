using System;
using System.Collections.Generic;
using System.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AIMLbot.UnitTest
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class UserTests

    {

        private static ChatBot _chatBot;
        private User _user;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        [ClassInitialize()]
        public static void ClassInitialize(TestContext testContext)
        {
            _chatBot = new ChatBot();

        }

        //  Use ClassCleanup to run code after all tests in a class have run
        [ClassCleanup()]
        public static void ClassCleanup()
        {
            
        }
         #endregion


        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void TestBadConstructor()
        {
            _user = new User("", _chatBot);
        }

        [TestMethod]
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

        [TestMethod]
        public void TestResultHandlers()
        {
            _user = new User("1", _chatBot);
            Assert.AreEqual("", _user.GetResultSentence());
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
            Assert.AreEqual("Result 3", _user.GetResultSentence());
            Assert.AreEqual("Result 3", _user.GetResultSentence(0));
            Assert.AreEqual("Result 1", _user.GetResultSentence(1));
            Assert.AreEqual("Result 4", _user.GetResultSentence(0, 1));
            Assert.AreEqual("Result 2", _user.GetResultSentence(1, 1));
            Assert.AreEqual("", _user.GetResultSentence(0, 2));
            Assert.AreEqual("", _user.GetResultSentence(2, 0));
            Assert.AreEqual("Result 3", _user.GetThat());
            Assert.AreEqual("Result 3", _user.GetThat(0));
            Assert.AreEqual("Result 1", _user.GetThat(1));
            Assert.AreEqual("Result 4", _user.GetThat(0, 1));
            Assert.AreEqual("Result 2", _user.GetThat(1, 1));
            Assert.AreEqual("", _user.GetThat(0, 2));
            Assert.AreEqual("", _user.GetThat(2, 0));
        }
    }
}
