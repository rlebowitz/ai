using System.Xml;
using AIMLbot.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AIMLbot.UnitTest
{
    [TestClass]
    public class AIMLLoaderTests
    {
        private AIMLLoader _loader;

        [TestInitialize]
        public void Setup()
        {
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