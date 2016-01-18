using System.Xml;
using AIMLbot.AIMLTagHandlers;
using AIMLbot.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AIMLbot.UnitTest.TagTests
{
    [TestClass]
    public class PersonTagTests
    {
        private ChatBot _chatBot;
        private User _user;
        private Request _request;
        private Result _result;
        private SubQuery _query;
        private Person _botTagHandler;

        [TestInitialize]
        public void Setup()
        {
            _chatBot = new ChatBot();
            _user = new User("1", _chatBot);
            _request = new Request("This is a test", _user, _chatBot);
            _query = new SubQuery("This is a test <that> * <topic> *");
            _result = new Result(_user, _chatBot, _request);
        }

        [TestMethod]
        public void TestAtomic()
        {
            XmlNode testNode = StaticHelpers.getNode("<person/>");
            _botTagHandler = new Person(_chatBot, _user, _query, _request, _result, testNode);
            _query.InputStar.Insert(0, " I WAS HE WAS SHE WAS I AM I ME MY MYSELF ");
            Assert.AreEqual(" he or she was I was I was he or she is he or she him or her his or her him or herself ",
                            _botTagHandler.Transform());
        }

        [TestMethod]
        public void TestEmptyInput()
        {
            XmlNode testNode = StaticHelpers.getNode("<person/>");
            _botTagHandler = new Person(_chatBot, _user, _query, _request, _result, testNode);
            _query.InputStar.Clear();
            Assert.AreEqual("", _botTagHandler.Transform());
        }

        [TestMethod]
        public void TestNoMatches()
        {
            XmlNode testNode = StaticHelpers.getNode("<person>THE QUICK BROWN FOX JUMPED OVER THE LAZY DOGS</person>");
            _botTagHandler = new Person(_chatBot, _user, _query, _request, _result, testNode);
            Assert.AreEqual("THE QUICK BROWN FOX JUMPED OVER THE LAZY DOGS", _botTagHandler.Transform());
        }

        [TestMethod]
        public void TestNonAtomic()
        {
            XmlNode testNode = StaticHelpers.getNode("<person> I WAS HE WAS SHE WAS I AM I ME MY MYSELF </person>");
            _botTagHandler = new Person(_chatBot, _user, _query, _request, _result, testNode);
            Assert.AreEqual(" he or she was I was I was he or she is he or she him or her his or her him or herself ",
                            _botTagHandler.Transform());
        }
    }
}