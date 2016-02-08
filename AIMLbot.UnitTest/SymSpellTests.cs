using System.Linq;
using AIMLbot.Spell;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AIMLbot.UnitTest
{
    /// <summary>
    ///     Summary description for SpellTests
    /// </summary>
    [TestClass]
    public class SymSpellTests
    {
        private readonly SymSpell _spelling;
        private string _word;

        public SymSpellTests()
        {
            _spelling = new SymSpell();
            _spelling.CreateDictionary("big.txt", "");
        }

        /// <summary>
        ///     Gets or sets the test context which provides
        ///     information about and functionality for the current test run.
        /// </summary>
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void TestSpelling1()
        {
            _word = "speling";
           var item =  _spelling.Correct(_word, "en");
            Assert.AreEqual("spelling", item.Term);
        }

        [TestMethod]
        public void TestSpelling2()
        {
            _word = "korrecter"; // 'correcter' is not in the dictionary file so this doesn't work
            var item = _spelling.Correct(_word, "en");
            Assert.AreEqual("corrected", item.Term);
        }

        [TestMethod]
        public void TestSpelling3()
        {
            _word = "korrect";
            var item = _spelling.Correct(_word, "en");
            Assert.AreEqual("correct", item.Term);
        }

        [TestMethod]
        public void TestSpelling4()
        {
            _word = "acess";
            var item = _spelling.Correct(_word, "en");
            Assert.AreEqual("access", item.Term);
        }

        [TestMethod]
        public void TestSpelling5()
        {
            _word = "supposidly";
            var item = _spelling.Correct(_word, "en");
            Assert.AreEqual("supposedly", item.Term);
        }

        //[TestMethod]
        //public void TestSentence()
        //{
        //    // sees speed instead of spelled (see notes on norvig.com)
        //    const string sentence = "I havve speled thes woord wwrong";
        //    var correction = sentence.Split(' ')
        //        .Aggregate("", (current, item) => current + " " + _spelling.Correct(item));
        //    Assert.AreEqual("I have speed the word wrong", correction);
        //}

        #region Additional test attributes

        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize]
        //public static void MyClassInitialize(TestContext testContext) {
        //}
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //

        #endregion
    }
}