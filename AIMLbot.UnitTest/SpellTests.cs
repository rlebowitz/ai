using System.Linq;
using AIMLbot.Spell;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AIMLbot.UnitTest
{
    /// <summary>
    ///     Summary description for SpellTests
    /// </summary>
    [TestClass]
    public class SpellTests
    {
        private readonly Spelling _spelling;
        private string _word;

        public SpellTests()
        {
            _spelling = new Spelling();
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
            Assert.AreEqual("spelling", _spelling.Correct(_word));
        }

        [TestMethod]
        public void TestSpelling2()
        {
            _word = "korrecter"; // 'correcter' is not in the dictionary file so this doesn't work
            Assert.AreEqual("", _spelling.Correct(_word));
        }

        [TestMethod]
        public void TestSpelling3()
        {
            _word = "korrect";
            Assert.AreEqual("correct", _spelling.Correct(_word));
        }

        [TestMethod]
        public void TestSpelling4()
        {
            _word = "acess";
            Assert.AreEqual("access", _spelling.Correct(_word));
        }

        [TestMethod]
        public void TestSpelling5()
        {
            _word = "supposidly";
            Assert.AreEqual("supposedly", _spelling.Correct(_word));
        }

        [TestMethod]
        public void TestSentence()
        {
            // sees speed instead of spelled (see notes on norvig.com)
            const string sentence = "I havve speled thes woord wwrong";
            var correction = sentence.Split(' ')
                .Aggregate("", (current, item) => current + " " + _spelling.Correct(item));
            Assert.AreEqual("I have spelled this word wrong", correction);
        }

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