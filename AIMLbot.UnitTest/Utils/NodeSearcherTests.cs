using System.Text;
using System.Threading;
using System.Xml;
using AIMLbot.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AIMLbot.UnitTest.Utils
{
    [TestClass]
    public class NodeSearcherTests
    {
        private User _user;

        [TestInitialize]
        public void Setup()
        {
            _user = new User();
        }

        [TestMethod]
        public void TestAddCategoryAsLeafNode()
        {
            var node = new Node();
            var path = "";
            var template = "<srai>TEST</srai>";
            node.AddCategory(path, template);
            node.Word = "*";

            Assert.AreEqual(0, node.NumberOfChildNodes);
            Assert.AreEqual(template, node.Template);
            Assert.AreEqual("*", node.Word);
        }

        [TestMethod]
        [ExpectedException(typeof (XmlException))]
        public void TestAddCategoryWithEmptyTemplate()
        {
            var node = new Node();
            var path = "Test 1 <that> * <topic> *";
            node.AddCategory(path, string.Empty);
        }

        [TestMethod]
        public void TestAddCategoryWithGoodData()
        {
            var node = new Node();
            var path = "Test 1 <that> * <topic> *";
            var template = "<srai>Test</srai>";
            node.AddCategory(path, template);

            Assert.AreEqual(1, node.NumberOfChildNodes);
            Assert.AreEqual(string.Empty, node.Template);
            Assert.AreEqual(string.Empty, node.Word);
        }

        //[TestMethod]
        //public void TestEvaluateTimeOut()
        //{
        //    var node = new Node();
        //    var path = "Test 1 <that> that <topic> topic";
        //    var template = "<srai>TEST</srai>";
        //    node.AddCategory(path, template);

        //    var pathAlt = "Alt Test <that> that <topic> topic";
        //    var templateAlt = "<srai>TEST ALT</srai>";
        //    node.AddCategory(pathAlt, templateAlt);

        //    ChatBot.TimeOut = 10;
        //    var request = new Request("Test 1", _user);
        //    var searcher = new NodeSearcher();
        //    Thread.Sleep(20);
        //    var result =
        //        searcher.Evaluate(node, "Test 1 <that> that <topic> topic", MatchState.UserInput,
        //            new StringBuilder());
        //    Assert.AreEqual(string.Empty, result);
        //}

        [TestMethod]
        public void TestEvaluateWith_WildCardThat()
        {
            var path = "Test 1 <that> _ <topic> topic";
            var template = "<srai>TEST</srai>";
            var node = new Node();
            node.AddCategory(path, template);

            var pathAlt = "Alt Test <that> that <topic> topic";
            var templateAlt = "<srai>TEST ALT</srai>";
            node.AddCategory(pathAlt, templateAlt);

            var request = new Request("Test 1", _user);
            var searcher = new NodeSearcher();
            var result = searcher.Evaluate(node, "Test 1 <that> WILDCARD WORDS <topic> topic", MatchState.UserInput,
                new StringBuilder());

            Assert.AreEqual("<srai>TEST</srai>", result);
            Assert.AreEqual("WILDCARD WORDS", searcher.Query.ThatStar[0]);
        }

        [TestMethod]
        public void TestEvaluateWith_WildCardThatNotMatched()
        {
            var node = new Node();
            var path = "Test 1 <that> _ <topic> topic";
            var template = "<srai>TEST</srai>";
            node.AddCategory(path, template);

            var pathAlt = "Alt Test <that> that <topic> topic";
            var templateAlt = "<srai>TEST ALT</srai>";
            node.AddCategory(pathAlt, templateAlt);

            var request = new Request("Test 1", _user);
            var searcher = new NodeSearcher();
            var result = searcher.Evaluate(node, "Alt Test <that> that <topic> topic", MatchState.UserInput,
                new StringBuilder());
            Assert.AreEqual("<srai>TEST ALT</srai>", result);
        }

        [TestMethod]
        public void TestEvaluateWith_WildCardTopic()
        {
            var node = new Node();

            var path = "Test 1 <that> that <topic> _";
            var template = "<srai>TEST</srai>";
            node.AddCategory(path, template);

            var pathAlt = "Alt Test <that> that <topic> topic";
            var templateAlt = "<srai>TEST ALT</srai>";
            node.AddCategory(pathAlt, templateAlt);

            var request = new Request("Test 1", _user);
            var searcher = new NodeSearcher();
            var result = searcher.Evaluate(node, "Test 1 <that> that <topic> WILDCARD WORDS", MatchState.UserInput,
                new StringBuilder());
            Assert.AreEqual("<srai>TEST</srai>", result);
            Assert.AreEqual("WILDCARD WORDS", searcher.Query.TopicStar[0]);
        }

        [TestMethod]
        public void TestEvaluateWith_WildCardTopicNotMatched()
        {
            var node = new Node();

            var path = "Test 1 <that> that <topic> _";
            var template = "<srai>TEST</srai>";
            node.AddCategory(path, template);

            var pathAlt = "Alt Test <that> that <topic> topic";
            var templateAlt = "<srai>TEST ALT</srai>";
            node.AddCategory(pathAlt, templateAlt);

            var request = new Request("Test 1", _user);
            var searcher = new NodeSearcher();
            var result = searcher.Evaluate(node, "Alt Test <that> that <topic> topic", MatchState.UserInput,
                new StringBuilder());
            Assert.AreEqual("<srai>TEST ALT</srai>", result);
        }

        [TestMethod]
        public void TestEvaluateWith_WildCardUserInput()
        {
            var node = new Node();

            var path = "_ Test 1 <that> that <topic> topic";
            var template = "<srai>TEST</srai>";
            node.AddCategory(path, template);

            var pathAlt = "Alt Test <that> that <topic> topic";
            var templateAlt = "<srai>TEST ALT</srai>";
            node.AddCategory(pathAlt, templateAlt);

            var request = new Request("Test 1", _user);
            var searcher = new NodeSearcher();
            var result = searcher.Evaluate(node, "WILDCARD WORDS Test 1 <that> that <topic> topic", MatchState.UserInput,
                new StringBuilder());
            Assert.AreEqual("<srai>TEST</srai>", result);
            Assert.AreEqual("WILDCARD WORDS", searcher.Query.InputStar[0]);
        }

        [TestMethod]
        public void TestEvaluateWith_WildCardUserInputNotMatched()
        {
            var node = new Node();

            var path = "_ Test 1 <that> that <topic> topic";
            var template = "<srai>TEST</srai>";
            node.AddCategory(path, template);

            var pathAlt = "Alt Test <that> that <topic> topic";
            var templateAlt = "<srai>TEST ALT</srai>";
            node.AddCategory(pathAlt, templateAlt);

            var request = new Request("Test 1", _user);
            var searcher = new NodeSearcher();
            var result = searcher.Evaluate(node, "Alt Test <that> that <topic> topic", MatchState.UserInput,
                new StringBuilder());
            Assert.AreEqual("<srai>TEST ALT</srai>", result);
        }

        [TestMethod]
        public void TestEvaluateWithEmptyNode()
        {
            var node = new Node();
            var request = new Request("Test 1", _user);
            var searcher = new NodeSearcher();
            var result = searcher.Evaluate(node, "Test 1 <that> that <topic> topic", MatchState.UserInput,
                new StringBuilder());
            Assert.AreEqual(string.Empty, result);
        }

        [TestMethod]
        public void TestEvaluateWithInternationalCharset()
        {
            var node = new Node();

            var path = "中 文 <that> * <topic> *";
            var template = "中文 (Chinese)";
            node.AddCategory(path, template);

            var path2 = "日 本 語 <that> * <topic> *";
            var template2 = "日 本 語 (Japanese)";
            node.AddCategory(path2, template2);

            var path3 = "Русский язык <that> * <topic> *";
            var template3 = "Русский язык (Russian)";
            node.AddCategory(path3, template3);

            var request = new Request("中 文", _user);
            var searcher = new NodeSearcher();
            var result = searcher.Evaluate(node, "中 文 <that> * <topic> *", MatchState.UserInput, new StringBuilder());
            Assert.AreEqual("中文 (Chinese)", result);

            request = new Request("日 本 語", _user);
            searcher = new NodeSearcher();
            result = searcher.Evaluate(node, "日 本 語 <that> * <topic> *", MatchState.UserInput, new StringBuilder());
            Assert.AreEqual("日 本 語 (Japanese)", result);

            request = new Request("Русский язык", _user);
            searcher = new NodeSearcher();
            result = searcher.Evaluate(node, "Русский язык <that> * <topic> *", MatchState.UserInput,
                new StringBuilder());
            Assert.AreEqual("Русский язык (Russian)", result);
        }

        [TestMethod]
        public void TestEvaluateWithMultipleWildcards()
        {
            var node = new Node();

            var path = "Test _ 1 * <that> Test _ 1 * <topic> Test * 1 _";
            var template = "<srai>TEST</srai>";
            node.AddCategory(path, template);

            var pathAlt = "Alt Test <that> that <topic> topic";
            var templateAlt = "<srai>TEST ALT</srai>";
            node.AddCategory(pathAlt, templateAlt);

            var request = new Request("Test 1", _user);
            var searcher = new NodeSearcher();
            var result = searcher.Evaluate(node,
                "Test FIRST USER 1 SECOND USER <that> Test FIRST THAT 1 SECOND THAT <topic> Test FIRST TOPIC 1 SECOND TOPIC",
                MatchState.UserInput, new StringBuilder());
            Assert.AreEqual("<srai>TEST</srai>", result);
            Assert.AreEqual(2, searcher.Query.InputStar.Count);
            Assert.AreEqual("SECOND USER", searcher.Query.InputStar[0]);
            Assert.AreEqual("FIRST USER", searcher.Query.InputStar[1]);
            Assert.AreEqual(2, searcher.Query.ThatStar.Count);
            Assert.AreEqual("SECOND THAT", searcher.Query.ThatStar[0]);
            Assert.AreEqual("FIRST THAT", searcher.Query.ThatStar[1]);
            Assert.AreEqual(2, searcher.Query.TopicStar.Count);
            Assert.AreEqual("SECOND TOPIC", searcher.Query.TopicStar[0]);
            Assert.AreEqual("FIRST TOPIC", searcher.Query.TopicStar[1]);
        }

        [TestMethod]
        public void TestEvaluateWithMultipleWildcardsSwitched()
        {
            var node = new Node();
            var path = "Test * 1 _ <that> Test * 1 _ <topic> Test _ 1 *";
            var template = "<srai>TEST</srai>";
            node.AddCategory(path, template);

            var pathAlt = "Alt Test <that> that <topic> topic";
            var templateAlt = "<srai>TEST ALT</srai>";
            node.AddCategory(pathAlt, templateAlt);

            var request = new Request("Test 1", _user);
            var searcher = new NodeSearcher();
            var result = searcher.Evaluate(node,
                "Test FIRST USER 1 SECOND USER <that> Test FIRST THAT 1 SECOND THAT <topic> Test FIRST TOPIC 1 SECOND TOPIC",
                MatchState.UserInput, new StringBuilder());
            Assert.AreEqual("<srai>TEST</srai>", result);
            Assert.AreEqual(2, searcher.Query.InputStar.Count);
            Assert.AreEqual("SECOND USER", searcher.Query.InputStar[0]);
            Assert.AreEqual("FIRST USER", searcher.Query.InputStar[1]);
            Assert.AreEqual(2, searcher.Query.ThatStar.Count);
            Assert.AreEqual("SECOND THAT", searcher.Query.ThatStar[0]);
            Assert.AreEqual("FIRST THAT", searcher.Query.ThatStar[1]);
            Assert.AreEqual(2, searcher.Query.TopicStar.Count);
            Assert.AreEqual("SECOND TOPIC", searcher.Query.TopicStar[0]);
            Assert.AreEqual("FIRST TOPIC", searcher.Query.TopicStar[1]);
        }

        [TestMethod]
        public void TestEvaluateWithNoWildCards()
        {
            var node = new Node();

            var path = "Test 1 <that> that <topic> topic";
            var template = "<srai>TEST</srai>";
            node.AddCategory(path, template);

            var request = new Request("Test 1", _user);
            var searcher = new NodeSearcher();
            var result = searcher.Evaluate(node, "Test 1 <that> that <topic> topic", MatchState.UserInput,
                new StringBuilder());
            Assert.AreEqual("<srai>TEST</srai>", result);
        }

        [TestMethod]
        public void TestEvaluateWithStarWildCardThat()
        {
            var node = new Node();

            var path = "Test 1 <that> Test * 1 <topic> topic";
            var template = "<srai>TEST</srai>";
            node.AddCategory(path, template);

            var pathAlt = "Alt Test <that> that <topic> topic";
            var templateAlt = "<srai>TEST ALT</srai>";
            node.AddCategory(pathAlt, templateAlt);

            var request = new Request("Test 1", _user);
            var searcher = new NodeSearcher();
            var result = searcher.Evaluate(node, "Test 1 <that> Test WILDCARD WORDS 1 <topic> topic",
                MatchState.UserInput, new StringBuilder());
            Assert.AreEqual("<srai>TEST</srai>", result);
            Assert.AreEqual("WILDCARD WORDS", searcher.Query.ThatStar[0]);
        }

        [TestMethod]
        public void TestEvaluateWithStarWildCardThatNotMatched()
        {
            var node = new Node();

            var path = "Test 1 <that> Test * 1 <topic> topic";
            var template = "<srai>TEST</srai>";
            node.AddCategory(path, template);

            var pathAlt = "Alt Test <that> that <topic> topic";
            var templateAlt = "<srai>TEST ALT</srai>";
            node.AddCategory(pathAlt, templateAlt);

            var request = new Request("Test 1", _user);
            var searcher = new NodeSearcher();
            var result = searcher.Evaluate(node, "Alt Test <that> that <topic> topic",
                MatchState.UserInput, new StringBuilder());
            Assert.AreEqual("<srai>TEST ALT</srai>", result);
        }

        [TestMethod]
        public void TestEvaluateWithStarWildCardTopic()
        {
            var node = new Node();
            var path = "Test 1 <that> that <topic> Test * 1";
            var template = "<srai>TEST</srai>";
            node.AddCategory(path, template);

            var pathAlt = "Alt Test <that> that <topic> topic";
            var templateAlt = "<srai>TEST ALT</srai>";
            node.AddCategory(pathAlt, templateAlt);

            var request = new Request("Test 1", _user);
            var searcher = new NodeSearcher();

            var result = searcher.Evaluate(node, "Test 1 <that> that <topic> Test WILDCARD WORDS 1",
                MatchState.UserInput, new StringBuilder());
            Assert.AreEqual("<srai>TEST</srai>", result);
            Assert.AreEqual("WILDCARD WORDS", searcher.Query.TopicStar[0]);
        }

        [TestMethod]
        public void TestEvaluateWithStarWildCardTopicNotMatched()
        {
            var node = new Node();
            var path = "Test 1 <that> that <topic> Test * 1";
            var template = "<srai>TEST</srai>";
            node.AddCategory(path, template);

            var pathAlt = "Alt Test <that> that <topic> topic";
            var templateAlt = "<srai>TEST ALT</srai>";
            node.AddCategory(pathAlt, templateAlt);

            var request = new Request("Test 1", _user);
            var searcher = new NodeSearcher();

            var result = searcher.Evaluate(node, "Alt Test <that> that <topic> topic", MatchState.UserInput,
                new StringBuilder());
            Assert.AreEqual("<srai>TEST ALT</srai>", result);
        }

        [TestMethod]
        public void TestEvaluateWithStarWildCardUserInput()
        {
            var node = new Node();
            var path = "Test * 1 <that> that <topic> topic";
            var template = "<srai>TEST</srai>";
            node.AddCategory(path, template);

            var pathAlt = "Alt Test <that> that <topic> topic";
            var templateAlt = "<srai>TEST ALT</srai>";
            node.AddCategory(pathAlt, templateAlt);

            var request = new Request("Test 1", _user);
            var searcher = new NodeSearcher();
            var result = searcher.Evaluate(node, "Test WILDCARD WORDS 1 <that> that <topic> topic", MatchState.UserInput,
                new StringBuilder());
            Assert.AreEqual("<srai>TEST</srai>", result);
            Assert.AreEqual("WILDCARD WORDS", searcher.Query.InputStar[0]);
        }

        [TestMethod]
        public void TestEvaluateWithStarWildCardUserInputNotMatched()
        {
            var node = new Node();

            var path = "Test * 1 <that> that <topic> topic>";
            var template = "<srai>TEST</srai>";
            node.AddCategory(path, template);

            var pathAlt = "Alt Test <that> that <topic> topic";
            var templateAlt = "<srai>TEST ALT</srai>";
            node.AddCategory(pathAlt, templateAlt);

            var request = new Request("Test 1", _user);
            var searcher = new NodeSearcher();
            var result = searcher.Evaluate(node, "Alt Test <that> that <topic> topic", MatchState.UserInput,
                new StringBuilder());
            Assert.AreEqual("<srai>TEST ALT</srai>", result);
        }

        [TestMethod]
        public void TestEvaluateWithWildcardsInDifferentPartsOfPath()
        {
            var node = new Node();
            var path = "Test * 1 <that> Test * 1 <topic> Test * 1";
            var template = "<srai>TEST</srai>";
            node.AddCategory(path, template);

            var pathAlt = "Alt Test <that> that <topic> topic";
            var templateAlt = "<srai>TEST ALT</srai>";
            node.AddCategory(pathAlt, templateAlt);

            var request = new Request("Test 1", _user);
            var searcher = new NodeSearcher();
            var result = searcher.Evaluate(node,
                "Test WILDCARD USER WORDS 1 <that> Test WILDCARD THAT WORDS 1 <topic> Test WILDCARD TOPIC WORDS 1",
                MatchState.UserInput, new StringBuilder());
            Assert.AreEqual("<srai>TEST</srai>", result);
            Assert.AreEqual("WILDCARD USER WORDS", searcher.Query.InputStar[0]);
            Assert.AreEqual("WILDCARD THAT WORDS", searcher.Query.ThatStar[0]);
            Assert.AreEqual("WILDCARD TOPIC WORDS", searcher.Query.TopicStar[0]);
        }
    }
}