using AIMLbot.AIMLTagHandlers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AIMLbot.UnitTest.TagTests
{
    [TestClass]
    public class GetTagTests
    {
        private Get _tagHandler;
        private User _user;

        [TestInitialize]
        public void Setup()
        {
            _user = new User();
        }

        [TestMethod]
        public void TestWithBadAttribute()
        {
            var testNode = StaticHelpers.GetNode("<get nome=\"we\"/>");
            _tagHandler = new Get(_user, testNode);
            Assert.AreEqual("", _tagHandler.ProcessChange());
        }

        [TestMethod]
        public void TestWithBadNode()
        {
            var testNode = StaticHelpers.GetNode("<got name=\"we\"/>");
            _tagHandler = new Get(_user, testNode);
            Assert.AreEqual("", _tagHandler.ProcessChange());
        }

        [TestMethod]
        public void TestWithGoodData()
        {
            // first element
            var testNode = StaticHelpers.GetNode("<get name=\"name\"/>");
            _tagHandler = new Get(_user, testNode);
            Assert.AreEqual("un-named user", _tagHandler.ProcessChange());

            // last element
            testNode = StaticHelpers.GetNode("<get name=\"we\"/>");
            _tagHandler = new Get(_user, testNode);
            Assert.AreEqual("unknown", _tagHandler.ProcessChange());
        }

        [TestMethod]
        public void TestWithNoAttributes()
        {
            var testNode = StaticHelpers.GetNode("<get/>");
            _tagHandler = new Get(_user, testNode);
            Assert.AreEqual("", _tagHandler.ProcessChange());
        }

        [TestMethod]
        public void TestWithNonExistentPredicate()
        {
            var testNode = StaticHelpers.GetNode("<get name=\"nonexistent\"/>");
            _tagHandler = new Get(_user, testNode);
            Assert.AreEqual("", _tagHandler.ProcessChange());
        }

        [TestMethod]
        public void TestWithTooManyAttributes()
        {
            var testNode = StaticHelpers.GetNode("<get name=\"we\" value=\"value\" />");
            _tagHandler = new Get(_user, testNode);
            Assert.AreEqual("", _tagHandler.ProcessChange());
        }
    }
}