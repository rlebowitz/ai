using NUnit.Framework;

namespace AIMLbot.Tests.Normalize
{
    [TestFixture]
    public class MakeCaseInsensitiveTests
    {
        private ChatBot _mockChatBot;

        private MakeCaseInsensitive mockCaseInsensitive;

        [OneTimeSetUp]
        public void SetupMockBot()
        {
            _mockChatBot = new ChatBot();
        }

        [Test]
        public void testNormalizedToUpper()
        {
            string testInput = "abcdefghijklmnopqrstuvwxyz1234567890";
            mockCaseInsensitive = new MakeCaseInsensitive(_mockChatBot, testInput);
            Assert.AreEqual("ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890", mockCaseInsensitive.Transform());
        }
    }
}