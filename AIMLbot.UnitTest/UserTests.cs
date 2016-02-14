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

        private User _user;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void TestConstructorPopulatesUserObject()
        {
            _user = new User("1");
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
            _user = new User();
            Assert.AreEqual("", _user.GetResultSentence());
            var mockRequest = new Request("Sentence 1. Sentence 2", _user);
            var mockResult = new Result(_user, mockRequest);
            mockResult.InputSentences.Add("Result 1");
            mockResult.InputSentences.Add("Result 2");
            mockResult.OutputSentences.Add("Result 1");
            mockResult.OutputSentences.Add("Result 2");
            _user.AddResult(mockResult);
            var mockResult2 = new Result(_user, mockRequest);
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
