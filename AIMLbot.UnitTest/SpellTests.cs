using System;
using System.Linq;
using AIMLbot.Spell;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AIMLbot.UnitTest
{
    /// <summary>
    ///     Summary description for SpellTests
    /// </summary>
    [TestClass]
    public class NorvigSpellTests
    {
        private readonly Spelling _spelling;
        private string _word;

        public NorvigSpellTests()
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
            Assert.AreEqual("corrected", _spelling.Correct(_word));
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
            char[] splitters = new[] {' '};
            var words = sentence.Split(splitters, StringSplitOptions.RemoveEmptyEntries);
            string[] result = new string[words.Length];
            for (var i = 0;i< words.Length;i++)
            {
                result[i] = _spelling.Correct(words[i]);
            }
            Assert.AreEqual("i have speed the word wrong", string.Join(" ", result));
        }

    }
}