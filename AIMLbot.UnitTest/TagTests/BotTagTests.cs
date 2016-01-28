using System.Xml;
using AIMLbot.AIMLTagHandlers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AIMLbot.UnitTest.TagTests
{
    [TestClass]
    public class BotTagTests
    {
        private Bot _botTagHandler;

        [TestInitialize]
        public void Setup()
        {
        }

        [TestMethod]
        public void TestBadAttribute()
        {
            XmlNode testNode = StaticHelpers.GetNode("<bot value=\"name\"/>");
            _botTagHandler = new Bot(testNode);
            Assert.AreEqual("", _botTagHandler.ProcessChange());
        }

        [TestMethod]
        public void TestBadNodeName()
        {
            XmlNode testNode = StaticHelpers.GetNode("<bad value=\"name\"/>");
            _botTagHandler = new Bot(testNode);
            Assert.AreEqual("", _botTagHandler.ProcessChange());
        }

        [TestMethod]
        public void TestExpectedInput()
        {
            XmlNode testNode = StaticHelpers.GetNode("<bot name= \"name\"/>");
            _botTagHandler = new Bot(testNode);
            Assert.AreEqual("un-named user", _botTagHandler.ProcessChange());
        }

        [TestMethod]
        public void TestNonExistentPredicate()
        {
            XmlNode testNode = StaticHelpers.GetNode("<bot name=\"nonexistent\"/>");
            _botTagHandler = new Bot(testNode);
            Assert.AreEqual("", _botTagHandler.ProcessChange());
        }

        [TestMethod]
        public void TestStandardPredicateCollection()
        {
            string[] predicates = {
                                      "name", "birthday", "birthplace", "boyfriend", "favoriteband", "favoritebook",
                                      "favoritecolor", "favoritefood", "favoritesong", "favoritemovie", "forfun", "friends"
                                      , "gender", "girlfriend", "kindmusic", "location", "looklike", "master", "question",
                                      "sign", "talkabout", "wear"
                                  };
            foreach (string predicate in predicates)
            {
                var tag = $"<bot name=\"{predicate}\" />";
                var testNode = StaticHelpers.GetNode(tag);
                _botTagHandler = new Bot(testNode);
                var transform =_botTagHandler.ProcessChange();
                Assert.AreNotEqual(string.Empty, transform);
            }
        }

        [TestMethod]
        public void TestTooManyAttributes()
        {
            XmlNode testNode = StaticHelpers.GetNode("<bot name=\"name\" value=\"bad\"/>");
            _botTagHandler = new Bot(testNode);
            Assert.AreEqual("", _botTagHandler.ProcessChange());
        }
    }
}