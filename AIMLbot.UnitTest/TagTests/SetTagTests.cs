using AIMLbot.AIMLTagHandlers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AIMLbot.UnitTest.TagTests
{
    [TestClass]
    public class SetTagTests
    {
        private Set _tagHandler;
        private User _user;

        [TestInitialize]
        public void Initialize()
        {
            _user = new User();
        }

        [TestMethod]
        public void TestAbilityToRemovePredicates()
        {
            var testNode = StaticHelpers.GetNode("<set name=\"test1\">content 1</set>");
            _tagHandler = new Set(_user, testNode);
            Assert.AreEqual("content 1", _tagHandler.ProcessChange());
            Assert.AreEqual(true, _user.Predicates.ContainsKey("test1"));
            var testNode2 = StaticHelpers.GetNode("<set name=\"test1\"/>");
            _tagHandler = new Set(_user, testNode2);
            Assert.AreEqual("", _tagHandler.ProcessChange());
            Assert.AreEqual(false, _user.Predicates.ContainsKey("test1"));
        }

        [TestMethod]
        public void TestWithBadAttribute()
        {
            var testNode = StaticHelpers.GetNode("<set nome=\"test 3\">content 3</set>");
            _tagHandler = new Set(_user, testNode);
            Assert.AreEqual("", _tagHandler.ProcessChange());
        }

        [TestMethod]
        public void TestWithBadNode()
        {
            var testNode = StaticHelpers.GetNode("<sot name=\"test2\">content 2</sot>");
            _tagHandler = new Set(_user, testNode);
            Assert.AreEqual("", _tagHandler.ProcessChange());
        }

        [TestMethod]
        public void TestWithGoodData()
        {
            var testNode = StaticHelpers.GetNode("<set name=\"test1\">content 1</set>");
            _tagHandler = new Set(_user, testNode);
            Assert.AreEqual("content 1", _tagHandler.ProcessChange());
            Assert.AreEqual(true, _user.Predicates.ContainsKey("test1"));
        }

        [TestMethod]
        public void TestWithNoAttributes()
        {
            var testNode = StaticHelpers.GetNode("<set/>");
            _tagHandler = new Set(_user, testNode);
            Assert.AreEqual("", _tagHandler.ProcessChange());
        }

        [TestMethod]
        public void TestWithTooManyAttributes()
        {
            var testNode = StaticHelpers.GetNode("<set name=\"test 4\" value=\"value\" >content 4</set>");
            _tagHandler = new Set(_user, testNode);
            Assert.AreEqual("", _tagHandler.ProcessChange());
        }
    }
}