using System.Xml;
using AIMLbot.AIMLTagHandlers;
using AIMLbot.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AIMLbot.UnitTest.TagTests
{
    [TestClass]
    public class PersonTagTests
    {
        private Request _request;
        private SubQuery _query;
        private Person _tagHandler;

        [TestInitialize]
        public void Setup()
        {
            _request = new Request("This is a test", new User());
            _query = new SubQuery();
        }

        [TestMethod]
        public void TestAtomic()
        {
            XmlNode testNode = StaticHelpers.GetNode("<person/>");
            _tagHandler = new Person(_query, _request, testNode);
            _query.InputStar.Insert(0, " I WAS HE WAS SHE WAS I AM I ME MY MYSELF ");
            Assert.AreEqual(" he or she was I was I was he or she is he or she him or her his or her him or herself ",
                            _tagHandler.ProcessChange());
        }

        [TestMethod]
        public void TestEmptyInput()
        {
            XmlNode testNode = StaticHelpers.GetNode("<person/>");
            _tagHandler = new Person(_query, _request, testNode);
            _query.InputStar.Clear();
            Assert.AreEqual("", _tagHandler.ProcessChange());
        }

        [TestMethod]
        public void TestNoMatches()
        {
            XmlNode testNode = StaticHelpers.GetNode("<person>THE QUICK BROWN FOX JUMPED OVER THE LAZY DOGS</person>");
            _tagHandler = new Person(_query, _request, testNode);
            Assert.AreEqual("THE QUICK BROWN FOX JUMPED OVER THE LAZY DOGS", _tagHandler.ProcessChange());
        }

        [TestMethod]
        public void TestNonAtomic()
        {
            XmlNode testNode = StaticHelpers.GetNode("<person> I WAS HE WAS SHE WAS I AM I ME MY MYSELF </person>");
            _tagHandler = new Person(_query, _request, testNode);
            Assert.AreEqual(" he or she was I was I was he or she is he or she him or her his or her him or herself ",
                            _tagHandler.ProcessChange());
        }
    }
}