using AIMLbot.AIMLTagHandlers;
using AIMLbot.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AIMLbot.UnitTest.TagTests
{
    [TestClass]
    public class Person2TagTests
    {
        private SubQuery _query;
        private Request _request;
        private Person2 _tagHandler;

        [TestInitialize]
        public void Setup()
        {
            var user = new User();
            _request = new Request("This is a test", user);
            _query = new SubQuery();
        }

        [TestMethod]
        public void TestAtomic()
        {
            var testNode = StaticHelpers.GetNode("<person2/>");
            _tagHandler = new Person2(_query, _request, testNode);
            _query.InputStar.Insert(0, " WITH YOU TO YOU ME MY YOUR ");
            Assert.AreEqual(" with me to me you your my ", _tagHandler.ProcessChange());
        }

        [TestMethod]
        public void TestEmptyInput()
        {
            var testNode = StaticHelpers.GetNode("<person2/>");
            _tagHandler = new Person2(_query, _request, testNode);
            _query.InputStar.Clear();
            Assert.AreEqual("", _tagHandler.ProcessChange());
        }

        [TestMethod]
        public void TestNoMatches()
        {
            var testNode = StaticHelpers.GetNode("<person2>THE QUICK BROWN FOX JUMPED OVER THE LAZY DOGS</person2>");
            _tagHandler = new Person2(_query, _request, testNode);
            Assert.AreEqual("THE QUICK BROWN FOX JUMPED OVER THE LAZY DOGS", _tagHandler.ProcessChange());
        }

        [TestMethod]
        public void TestNonAtomic()
        {
            var testNode = StaticHelpers.GetNode("<person2> WITH YOU TO YOU ME MY YOUR </person2>");
            _tagHandler = new Person2(_query, _request, testNode);
            Assert.AreEqual(" with me to me you your my ", _tagHandler.ProcessChange());
        }
    }
}