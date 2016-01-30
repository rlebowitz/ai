using AIMLbot.AIMLTagHandlers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AIMLbot.UnitTest.TagTests
{
    [TestClass]
    public class UppercaseTagTests
    {
        private Uppercase _tagHandler;

        [TestInitialize]
        public void Setup()
        {
        }

        [TestMethod]
        public void TestEmptyInput()
        {
            var testNode = StaticHelpers.GetNode("<uppercase/>");
            _tagHandler = new Uppercase(testNode);
            Assert.AreEqual("", _tagHandler.ProcessChange());
        }

        [TestMethod]
        public void TestExpectedInput()
        {
            var testNode = StaticHelpers.GetNode("<uppercase>this is a test</uppercase>");
            _tagHandler = new Uppercase(testNode);
            Assert.AreEqual("THIS IS A TEST", _tagHandler.ProcessChange());
        }
    }
}