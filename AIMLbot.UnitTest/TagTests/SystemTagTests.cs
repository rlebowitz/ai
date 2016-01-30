using AIMLbot.AIMLTagHandlers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AIMLbot.UnitTest.TagTests
{
    [TestClass]
    public class SystemTagTests
    {
        private SystemTag _tagHandler;

        [TestInitialize]
        public void Setup()
        {
        }

        [TestMethod]
        public void TestSystemIsNotImplemented()
        {
            var testNode = StaticHelpers.GetNode("<system/>");
            _tagHandler = new SystemTag(testNode);
            Assert.AreEqual("", _tagHandler.ProcessChange());
        }
    }
}