using AIMLbot.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AIMLbot.UnitTest.Normalize
{
    [TestClass]
    public class SplitIntoSentencesTests
    {
        private readonly string[] _goodResult =
        {
            "This is sentence 1", "This is sentence 2", "This is sentence 3",
            "This is sentence 4"
        };

        private const string RawInput = "This is sentence 1. This is sentence 2! This is sentence 3; This is sentence 4?";

        [TestInitialize]
        public void Setup()
        {
        }

        [TestMethod]
        public void TestSplitterAllMadeFromWhiteSpace()
        {
            var result = "     ".SplitStrings();
            Assert.IsInstanceOfType(result, typeof(string[]));
            Assert.IsTrue(result.Length == 0);
        }

        [TestMethod]
        public void TestSplitterAllNoRawInput()
        {
            var result = "".SplitStrings();
            Assert.IsInstanceOfType(result, typeof(string[]));
            Assert.IsTrue(result.Length == 0);
        }

        [TestMethod]
        public void TestSplitterAllNoSentenceToSplit()
        {
            var result = "This is a sentence without splitters".SplitStrings();
            Assert.AreEqual("This is a sentence without splitters", result[0]);
        }

        [TestMethod]
        public void TestSplitterAllSentenceMadeOfSplitters()
        {
            var result = "!?.;".SplitStrings();
            Assert.IsInstanceOfType(result, typeof(string[]));
            Assert.IsTrue(result.Length == 0);
        }

        [TestMethod]
        public void TestSplitterAllSentenceWithSplitterAtEnd()
        {
            var result = "This is a sentence without splitters.".SplitStrings();
            Assert.AreEqual("This is a sentence without splitters", result[0]);
        }

        [TestMethod]
        public void TestSplitterAllSentenceWithSplitterAtStart()
        {
            var result = ".This is a sentence without splitters".SplitStrings();
            Assert.AreEqual("This is a sentence without splitters", result[0]);
        }

        [TestMethod]
        public void TestSplitterAllTokensPassedByMethod()
        {
            var result = RawInput.SplitStrings();
            for (var i = 0; i < _goodResult.Length; i++)
            {
                Assert.AreEqual(_goodResult[i], result[i]);
            }
        }
    }
}