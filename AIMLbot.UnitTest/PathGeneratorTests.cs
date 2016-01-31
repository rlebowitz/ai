using System.Linq;
using System.Xml.Linq;
using AIMLbot.Normalize;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AIMLbot.UnitTest
{
    [TestClass]
    public class PathGeneratorTests
    {
        private PathGenerator _pathGenerator;

        [TestInitialize]
        public void Setup()
        {
            _pathGenerator = new PathGenerator();
        }

        [TestMethod]
        public void TestGeneratePathWorksAsUserInput()
        {
            var result =
                _pathGenerator.Generate("This * is _ a pattern", "This * is _ a that", "This * is _ a topic", true);
            var expected = "This is a pattern <that> This is a that <topic> This is a topic";
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestGeneratePathWorksWithGoodData()
        {
            const string path = @"AIML\TestThat.aiml";
            var element = XDocument.Load(path);
            var aiml = element.Descendants("aiml").FirstOrDefault();
            var result = _pathGenerator.Generate(aiml, "testing topic 123", false);
            var expected = "test 1 <that> testing that 123 <topic> testing topic 123";
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestGeneratePathWorksWithGoodDataWithWildcards()
        {
            var path = @"AIML\TestWildcards.aiml";
            var doc = XDocument.Load(path);
            var aiml = doc.Descendants("aiml").FirstOrDefault();
            var result = _pathGenerator.Generate(aiml, "testing _ 123 *", false);
            var expected = "test * 1 _ <that> testing * that _ 123 <topic> testing _ 123 *";
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestGeneratePathWorksWithNoThatTag()
        {
            var path = @"AIML\TestNoThat.aiml";
            var doc = XDocument.Load(path);
            var aiml = doc.Descendants("aiml").FirstOrDefault();
            var result = _pathGenerator.Generate(aiml, "*", false);
            var expected = "test 1 <that> * <topic> *";
            Assert.AreEqual(expected, result);
        }
    }
}