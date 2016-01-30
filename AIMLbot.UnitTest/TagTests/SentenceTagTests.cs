using System.Xml;
using AIMLbot.AIMLTagHandlers;
using AIMLbot.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AIMLbot.UnitTest.TagTests
{
    [TestClass]
    public class SentenceTagTests
    {
        private Request _request;
        private SubQuery _query;
        private Sentence _botTagHandler;

        [TestInitialize]
        public void Setup()
        {
            _request = new Request("This is a test", new User());
            _query = new SubQuery();
        }

        [TestMethod]
        public void TestAtomic()
        {
            XmlNode testNode = StaticHelpers.GetNode("<sentence/>");
            _botTagHandler = new Sentence(_query, _request, testNode);
            _query.InputStar.Insert(0, "THIS IS. A TEST TO? SEE IF THIS; WORKS! OK");
            Assert.AreEqual("This is. A test to? See if this; Works! Ok", _botTagHandler.ProcessChange());
        }

        [TestMethod]
        public void TestEmptyInput()
        {
            XmlNode testNode = StaticHelpers.GetNode("<sentence/>");
            _botTagHandler = new Sentence(_query, _request, testNode);
            _query.InputStar.Clear();
            Assert.AreEqual("", _botTagHandler.ProcessChange());
        }

        [TestMethod]
        public void TestNonAtomicLower()
        {
            XmlNode testNode = StaticHelpers.GetNode("<sentence>this is. a test to? see if this; works! ok</sentence>");
            _botTagHandler = new Sentence(_query, _request, testNode);
            Assert.AreEqual("This is. A test to? See if this; Works! Ok", _botTagHandler.ProcessChange());
        }

        [TestMethod]
        public void TestNonAtomicUpper()
        {
            XmlNode testNode = StaticHelpers.GetNode("<sentence>THIS IS. A TEST TO? SEE IF THIS; WORKS! OK</sentence>");
            _botTagHandler = new Sentence(_query, _request, testNode);
            Assert.AreEqual("This is. A test to? See if this; Works! Ok", _botTagHandler.ProcessChange());
        }
    }
}