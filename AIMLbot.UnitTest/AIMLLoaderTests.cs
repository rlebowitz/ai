using System.Linq;
using System.Xml;
using System.Xml.Linq;
using AIMLbot.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AIMLbot.UnitTest
{
    [TestClass]
    public class AIMLLoaderTests
    {
        private AIMLLoader _loader;

        //http://imaso.googlecode.com/svn/trunk/200811-Aiml/Tests/Tests/Settings/aiml/

        [TestInitialize]
        public void Setup()
        {
        }

        [TestMethod]
        public void TestGeneratePathWorksAsUserInput()
        {
            _loader = new AIMLLoader();
            var result =
                _loader.GeneratePath("This * is _ a pattern", "This * is _ a that", "This * is _ a topic", true);
            var expected = "This is a pattern <that> This is a that <topic> This is a topic";
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestGeneratePathWorksWithGoodData()
        {
            const string path = @"AIML\TestThat.aiml";
            var element = XDocument.Load(path);
            var aiml = element.Descendants("aiml").FirstOrDefault();
            _loader = new AIMLLoader();
            var result = _loader.GeneratePath(aiml, "testing topic 123", false);
            var expected = "test 1 <that> testing that 123 <topic> testing topic 123";
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestGeneratePathWorksWithGoodDataWithWildcards()
        {
            var path = @"AIML\TestWildcards.aiml";
            var doc = XDocument.Load(path);
            var aiml = doc.Descendants("aiml").FirstOrDefault();
            _loader = new AIMLLoader();
            var result = _loader.GeneratePath(aiml, "testing _ 123 *", false);
            var expected = "test * 1 _ <that> testing * that _ 123 <topic> testing _ 123 *";
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestGeneratePathWorksWithNoThatTag()
        {
            var path = @"AIML\TestNoThat.aiml";
            var doc = XDocument.Load(path);
            var aiml = doc.Descendants("aiml").FirstOrDefault();
            _loader = new AIMLLoader();
            var result = _loader.GeneratePath(aiml, "*", false);
            var expected = "test 1 <that> * <topic> *";
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        [ExpectedException(typeof (XmlException))]
        public void TestLoadAIMLFileWithBadXML()
        {
            var path = @"AIML\BadlyFormed.aiml";
            _loader = new AIMLLoader();
            _loader.LoadAIML(path);
        }

        [TestMethod]
        [ExpectedException(typeof (XmlException))]
        public void TestLoadAIMLFileWithValidXMLButMissingPattern()
        {
            var path = @"AIML\MissingPattern.aiml";
            _loader = new AIMLLoader();
            _loader.LoadAIML(path);
        }

        [TestMethod]
        [ExpectedException(typeof (XmlException))]
        public void TestLoadAIMLFileWithValidXMLButMissingTemplate()
        {
            var path = @"AIML\MissingPattern.aiml";
            _loader = new AIMLLoader();
            _loader.LoadAIML(path);
        }
    }
}