using System.Collections;
using AIMLbot.AIMLTagHandlers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AIMLbot.UnitTest.TagTests
{
    [TestClass]
    public class RandomTagTests
    {
        private Random _tagHandler;
        private ArrayList _possibleResults;

        [TestInitialize]
        public void Setup()
        {
            _possibleResults = new ArrayList {"random 1", "random 2", "random 3", "random 4", "random 5"};
        }

        [TestMethod]
        public void TestWithBadListItems()
        {
            var testNode =
                StaticHelpers.GetNode(
                    @"<random>
    <li>random 1</li>
    <bad>bad 1</bad>
    <li>random 2</li>
    <bad>bad 2</bad>
    <li>random 3</li>
    <bad>bad 3</bad>
    <li>random 4</li>
    <bad>bad 4</bad>
    <li>random 5</li>
    <bad>bad 5</bad>
</random>");
            _tagHandler = new Random(testNode);
            Assert.IsTrue(_possibleResults.Contains(_tagHandler.ProcessChange()));
        }

        [TestMethod]
        public void TestWithNoListItems()
        {
            var testNode = StaticHelpers.GetNode("<random/>");
            _tagHandler = new Random(testNode);
            Assert.AreEqual("", _tagHandler.ProcessChange());
        }

        [TestMethod]
        public void TestWithValidData()
        {
            var testNode =
                StaticHelpers.GetNode(
                    @"<random>
    <li>random 1</li>
    <li>random 2</li>
    <li>random 3</li>
    <li>random 4</li>
    <li>random 5</li>
</random>");
            _tagHandler = new Random(testNode);
            Assert.IsTrue(_possibleResults.Contains(_tagHandler.ProcessChange()));
        }
    }
}