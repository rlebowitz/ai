using System;
using System.Xml;
using AIMLbot.AIMLTagHandlers;
using AIMLbot.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AIMLbot.UnitTest.TagTests
{
    [TestClass]
    public class LearnTagTests
    {
        private ChatBot _chatBot;
        private SubQuery _query;
        private Learn _tagHandler;

        [TestInitialize]
        public void Setup()
        {
            _chatBot = new ChatBot();
            _query = new SubQuery();
            _query.InputStar.Insert(0, "first star");
            _query.InputStar.Insert(0, "second star");
        }

        [TestMethod]
        public void TestWithBadInput()
        {
            Assert.AreEqual(0, ChatBot.Size);
            XmlNode testNode = StaticHelpers.GetNode("<learn>./nonexistent/Salutations.aiml</learn>");
            _tagHandler = new Learn(testNode);
            Assert.AreEqual("", _tagHandler.ProcessChange());
            Assert.AreEqual(0, ChatBot.Size);
        }

        [TestMethod]
        public void TestWithEmptyInput()
        {
            Assert.AreEqual(0, ChatBot.Size);
            XmlNode testNode = StaticHelpers.GetNode("<learn/>");
            _tagHandler = new Learn(testNode);
            Assert.AreEqual("", _tagHandler.ProcessChange());
            Assert.AreEqual(0, ChatBot.Size);
        }

        [TestMethod]
        public void TestWithValidInput()
        {
            Assert.AreEqual(0, ChatBot.Size);
            var path = Environment.CurrentDirectory + @"\AIML\Salutations.aiml";
            XmlNode testNode = StaticHelpers.GetNode($"<learn>{path}</learn>");
            _tagHandler = new Learn(testNode);
            Assert.AreEqual("", _tagHandler.ProcessChange());
            Assert.AreEqual(16, ChatBot.Size);
        }
    }
}