using AIMLbot.AIMLTagHandlers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AIMLbot.UnitTest.TagTests
{
    [TestClass]
    public class GetTagTests
    {
        private Get _getTagHandler;
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
            _getTagHandler = new Get(_user, testNode);
            Assert.AreEqual("", _getTagHandler.ProcessChange());
        }

        [TestMethod]
        public void TestWithBadNode()
        {
            var testNode = StaticHelpers.GetNode("<got name=\"we\"/>");
            _getTagHandler = new Get(_user, testNode);
            Assert.AreEqual("", _getTagHandler.ProcessChange());
        }

        [TestMethod]
        public void TestWithGoodData()
        {
            // first element
            var testNode = StaticHelpers.GetNode("<get name=\"name\"/>");
            _getTagHandler = new Get(_user, testNode);
            Assert.AreEqual("un-named user", _getTagHandler.ProcessChange());

            // last element
            testNode = StaticHelpers.GetNode("<get name=\"we\"/>");
            _getTagHandler = new Get(_user, testNode);
            Assert.AreEqual("unknown", _getTagHandler.ProcessChange());
        }

        [TestMethod]
        public void TestWithNoAttributes()
        {
            var testNode = StaticHelpers.GetNode("<get/>");
            _getTagHandler = new Get(_user, testNode);
            Assert.AreEqual("", _getTagHandler.ProcessChange());
        }

        [TestMethod]
        public void TestWithNonExistentPredicate()
        {
            var testNode = StaticHelpers.GetNode("<get name=\"nonexistent\"/>");
            _getTagHandler = new Get(_user, testNode);
            Assert.AreEqual("", _getTagHandler.ProcessChange());
        }

        [TestMethod]
        public void TestWithTooManyAttributes()
        {
            var testNode = StaticHelpers.GetNode("<get name=\"we\" value=\"value\" />");
            _getTagHandler = new Get(_user, testNode);
            Assert.AreEqual("", _getTagHandler.ProcessChange());
        }
    }
}