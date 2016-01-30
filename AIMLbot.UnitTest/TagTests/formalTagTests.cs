using AIMLbot.AIMLTagHandlers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AIMLbot.UnitTest.TagTests
{
    [TestClass]
    public class FormalTagTests
    {
        private Formal _tagHandler;

        [TestInitialize]
        public void Setup()
        {
        }

        [TestMethod]
        public void TestEmptyInput()
        {
            var testNode = StaticHelpers.GetNode("<formal/>");
            _tagHandler = new Formal(testNode);
            Assert.AreEqual("", _tagHandler.ProcessChange());
        }

        [TestMethod]
        public void TestExpectedCapitalizedInput()
        {
            var testNode = StaticHelpers.GetNode("<formal>THIS IS A TEST</formal>");
            _tagHandler = new Formal(testNode);
            Assert.AreEqual("This Is A Test", _tagHandler.ProcessChange());
        }

        [TestMethod]
        public void TestExpectedInput()
        {
            var testNode = StaticHelpers.GetNode("<formal>this is a test</formal>");
            _tagHandler = new Formal(testNode);
            Assert.AreEqual("This Is A Test", _tagHandler.ProcessChange());
        }
    }
}