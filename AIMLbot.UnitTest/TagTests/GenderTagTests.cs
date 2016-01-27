using AIMLbot.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Gender = AIMLbot.AIMLTagHandlers.Gender;

namespace AIMLbot.UnitTest.TagTests
{
    [TestClass]
    public class GenderTagTests
    {
        private Gender _botTagHandler;
        private ChatBot _chatBot;
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
            _query = new SubQuery();
            _result = new Result(_user, _chatBot, _request);
        }

        [TestMethod]
        public void TestAtomic()
        {
            var testNode = StaticHelpers.GetNode("<gender/>");
            _botTagHandler = new Gender(_chatBot, _user, _query, _request, _result, testNode);
            _query.InputStar.Insert(0, " HE SHE TO HIM TO HER HIS HER HIM ");
            Assert.AreEqual(" she he to her to him her his her ", _botTagHandler.Transform());
        }

        [TestMethod]
        public void TestEmptyInput()
        {
            var testNode = StaticHelpers.GetNode("<Gender/>");
            _botTagHandler = new Gender(_chatBot, _user, _query, _request, _result, testNode);
            _query.InputStar.Clear();
            Assert.AreEqual("", _botTagHandler.Transform());
        }

        [TestMethod]
        public void TestNoMatches()
        {
            var testNode = StaticHelpers.GetNode("<gender>THE QUICK BROWN FOX JUMPED OVER THE LAZY DOGS</gender>");
            _botTagHandler = new Gender(_chatBot, _user, _query, _request, _result, testNode);
            Assert.AreEqual("THE QUICK BROWN FOX JUMPED OVER THE LAZY DOGS", _botTagHandler.Transform());
        }

        [TestMethod]
        public void TestNonAtomic()
        {
            var testNode =
                StaticHelpers.GetNode(
                    "<gender> HE SHE TO HIM FOR HIM WITH HIM ON HIM IN HIM TO HER FOR HER WITH HER ON HER IN HER HIS HER HIM </gender>");
            _botTagHandler = new Gender(_chatBot, _user, _query, _request, _result, testNode);
            Assert.AreEqual(
                " she he to her for her with her on her in her to him for him with him on him in him her his her ",
                _botTagHandler.Transform());
        }
    }
}