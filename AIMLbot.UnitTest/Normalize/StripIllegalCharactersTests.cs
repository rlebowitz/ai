using AIMLbot.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AIMLbot.UnitTest.Normalize
{
    [TestClass]
    public class StripIllegalCharactersTests
    {
        [TestInitialize]
        public void Setup()
        {
        }

        [TestMethod]
        public void TestAlphaNumericCharactersWithSpaces()
        {
            const string testString = "01234567 ABCDEFGHIJKLMNOPQRSTUVWXYZ abcdefghijklmnopqrstuvwxyz";
            Assert.AreEqual(testString, testString.StripCharacters());
        }

        [TestMethod]
        public void TestNonAlphaNumericBecomeSpaces()
        {
            const string testString = "!\"£$%^&*()-+'";
            Assert.AreEqual("             ", testString.StripCharacters());
        }

        [TestMethod]
        public void TestWithEmptyString()
        {
            Assert.AreEqual("", "".StripCharacters());
        }
    }
}