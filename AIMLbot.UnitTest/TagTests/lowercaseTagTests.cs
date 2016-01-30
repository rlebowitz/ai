using AIMLbot.AIMLTagHandlers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AIMLbot.UnitTest.TagTests
{
    [TestClass]
    public class LowercaseTagTests
    {
        private Lowercase _tagHandler;

        [TestInitialize]
        public void Setup()
        {
        }

        [TestMethod]
        public void TestEmptyInput()
        {
            var testNode = StaticHelpers.GetNode("<lowercase/>");
            _tagHandler = new Lowercase(testNode);
            Assert.AreEqual("", _tagHandler.ProcessChange());
        }

        [TestMethod]
        public void TestExpectedInput()
        {
            var testNode = StaticHelpers.GetNode("<lowercase>THIS IS A TEST</lowercase>");
            _tagHandler = new Lowercase(testNode);
            Assert.AreEqual("this is a test", _tagHandler.ProcessChange());
        }
    }
}