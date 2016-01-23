using System.Collections;
using AIMLbot.AIMLTagHandlers;
using AIMLbot.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AIMLbot.UnitTest.TagTests
{
    [TestClass]
    public class RandomTagTests
    {
        private Random _botTagHandler;
        private ChatBot _chatBot;
        private ArrayList _possibleResults;
        private SubQuery _query;
        private Request _request;
        private Result _result;
        private User _user;

        [TestInitialize]
        public void Setup()
        {
            _chatBot = new ChatBot();
            _user = new User("1", _chatBot);
            _request = new Request("This is a test", _user, _chatBot);
            _query = new SubQuery("This is a test <that> * <topic> *");
            _result = new Result(_user, _chatBot, _request);
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
            _botTagHandler = new Random(_chatBot, _user, _query, _request, _result, testNode);
            Assert.IsTrue(_possibleResults.Contains(_botTagHandler.Transform()));
        }

        [TestMethod]
        public void TestWithNoListItems()
        {
            var testNode = StaticHelpers.GetNode("<random/>");
            _botTagHandler = new Random(_chatBot, _user, _query, _request, _result, testNode);
            Assert.AreEqual("", _botTagHandler.Transform());
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
            _botTagHandler = new Random(_chatBot, _user, _query, _request, _result, testNode);
            Assert.IsTrue(_possibleResults.Contains(_botTagHandler.Transform()));
        }
    }
}