using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AIMLbot.UnitTest.Normalize
{
    [TestClass]
    public class MakeCaseInsensitiveTests
    {
 
        [TestMethod]
        public void TestNormalizedToUpper()
        {
            const string testInput = "abcdefghijklmnopqrstuvwxyz1234567890";
            Assert.AreEqual("ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890", testInput.ToUpper());
        }
    }
}