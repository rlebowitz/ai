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
        private User _user;
        private Request _request;
        private Result _result;
        private SubQuery _query;
        private Learn _botTagHandler;

        [TestInitialize]
        public void Setup()
        {
            _chatBot = new ChatBot();
            _user = new User("1", _chatBot);
            _request = new Request("This is a test", _user, _chatBot);
            _query = new SubQuery();
            _query.InputStar.Insert(0, "first star");
            _query.InputStar.Insert(0, "second star");
            _result = new Result(_user, _chatBot, _request);
        }

        [TestMethod]
        public void TestWithBadInput()
        {
            Assert.AreEqual(0, _chatBot.Size);
            XmlNode testNode = StaticHelpers.GetNode("<learn>./nonexistent/Salutations.aiml</learn>");
            _botTagHandler = new Learn(_chatBot, _user, _query, _request, _result, testNode);
            Assert.AreEqual("", _botTagHandler.Transform());
            Assert.AreEqual(0, _chatBot.Size);
        }

        [TestMethod]
        public void TestWithEmptyInput()
        {
            Assert.AreEqual(0, _chatBot.Size);
            XmlNode testNode = StaticHelpers.GetNode("<learn/>");
            _botTagHandler = new Learn(_chatBot, _user, _query, _request, _result, testNode);
            Assert.AreEqual("", _botTagHandler.Transform());
            Assert.AreEqual(0, _chatBot.Size);
        }

        [TestMethod]
        public void TestWithValidInput()
        {
            Assert.AreEqual(0, _chatBot.Size);
            var path = Environment.CurrentDirectory + @"\AIML\Salutations.aiml";
            XmlNode testNode = StaticHelpers.GetNode($"<learn>{path}</learn>");
            _botTagHandler = new Learn(_chatBot, _user, _query, _request, _result, testNode);
            Assert.AreEqual("", _botTagHandler.Transform());
            Assert.AreEqual(16, _chatBot.Size);
        }
    }
}