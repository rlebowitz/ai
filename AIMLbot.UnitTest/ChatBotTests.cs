using System.Configuration;
using System.IO;
using System.Reflection;
using AIMLbot.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AIMLbot.UnitTest
{
    [TestClass]
    public class ChatBotTests
    {
        private static ChatBot _chatBot;

        private static AIMLLoader _loader;

        [TestInitialize]
        public void Initialize()
        {
            _chatBot = new ChatBot();
            _loader = new AIMLLoader(_chatBot);
            var path = @"AIML\ChatBotTests.aiml";
            {
                _loader.LoadAIML(path);
            }
        }

        [TestMethod]
        public void TestChatRepsonseWhenNotAcceptingInput()
        {
            _chatBot.IsAcceptingUserInput = false;
            var output = _chatBot.Chat("Hi", "1");
            Assert.AreEqual("This ChatBot is currently set to not accept user input.", output.RawOutput);
        }

        //[TestMethod]
        //public void TestLoadFromAIML()
        //{
        //    _chatBot = new ChatBot();
        //    _chatBot.LoadAIMLFromFiles();
        //    Assert.AreEqual(sizeTagTests.Size, _chatBot.Size);
        //}

        [TestMethod]
        public void TestLoadFromBinary()
        {
            _chatBot = new ChatBot();
            _chatBot.LoadFromBinaryFile();
            var output = _chatBot.Chat("bye", "1");
            Assert.AreEqual("Cheerio.", output.RawOutput);
        }

        [TestMethod]
        public void TestSaveSerialization()
        {
            var path = ConfigurationManager.AppSettings.Get("graphMasterFile", "GraphMaster.dat");
            _chatBot.SaveToBinaryFile();
            var fileInfo = new FileInfo(path);
            Assert.AreEqual(true, fileInfo.Exists);
        }

        [TestMethod]
        public void TestSimpleChat()
        {
            Assert.AreNotEqual(0, _chatBot.Size);
            var output = _chatBot.Chat("bye", "1");
            Assert.AreEqual("Cheerio.", output.Output);
        }

        [TestMethod]
        public void TestTimeOutChatWorks()
        {
            var output = _chatBot.Chat("infiniteloop1", "1");
            Assert.AreEqual(true, output.Request.HasTimedOut);
            Assert.AreEqual("ERROR: The request has timed out.", output.Output);
        }

        [TestMethod]
        public void TestWildCardsDontMixBetweenSentences()
        {
            var output = _chatBot.Chat("My name is FIRST. My name is SECOND.", "1");
            Assert.AreEqual("Hello FIRST! Hello SECOND!", output.Output);
        }
    }
}