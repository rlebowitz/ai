using System;
using AIMLbot.AIMLTagHandlers;
using AIMLbot.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AIMLbot.UnitTest.TagTests
{
    [TestClass]
    public class SizeTagTests
    {
        private ChatBot _chatBot;
        public static int Size = 16;
        private AIMLLoader _loader;
        private Size _tagHandler;

        [TestInitialize]
        public void Setup()
        {
            _chatBot = new ChatBot();
            ChatBot.Size = 0;
            _loader = new AIMLLoader();
            var path = $@"{Environment.CurrentDirectory}\AIML\Salutations.aiml";
            _loader.LoadAIML(path);
        }

        [TestMethod]
        public void TestWithBadXml()
        {
            var testNode = StaticHelpers.GetNode("<soze/>");
            _tagHandler = new Size(testNode);
            Assert.AreEqual("", _tagHandler.ProcessChange());
        }

        [TestMethod]
        public void TestWithValidData()
        {
            var testNode = StaticHelpers.GetNode("<size/>");
            _tagHandler = new Size(testNode);
            Assert.AreEqual(Convert.ToString(Size), _tagHandler.ProcessChange());
        }
    }
}