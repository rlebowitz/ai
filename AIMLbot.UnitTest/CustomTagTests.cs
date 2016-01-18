using System.Reflection;
using AIMLbot.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AIMLbot.UnitTest
{
    [TestClass]
    public class CustomTagTests
    {
        private static ChatBot _chatBot;

        private static AIMLLoader _loader;

        [TestInitialize]
        public void Initialize()
        {
            _chatBot = new ChatBot();
            _loader = new AIMLLoader(_chatBot);
            var assembly = Assembly.GetExecutingAssembly();
            using (var stream = assembly.GetManifestResourceStream("AIMLbot.UnitTest.AIML.Salutations.aiml"))
            {
                _loader.LoadAIML(stream);
            }
        }

        [TestMethod]
        public void TestCustomTagAccessToRSSFeed()
        {
            //var fi = new FileInfo(pathToCustomTagDll);
            //Assert.AreEqual(true, fi.Exists);
            //_chatBot.LoadCustomTagHandlers(pathToCustomTagDll);
            //var output = _chatBot.Chat("Test news tag", "1");
            //var result = output.RawOutput.Replace("[[BBC News]]", "");
            //Assert.IsTrue(result.Length > 0);
        }

        [TestMethod]
        public void TestCustomTagAccessToRssFeedWithArguments()
        {
            //var fi = new FileInfo(pathToCustomTagDll);
            //Assert.AreEqual(true, fi.Exists);
            //_chatBot.LoadCustomTagHandlers(pathToCustomTagDll);
            //var output = _chatBot.Chat("Test news tag with descriptions", "1");
            //var result = output.RawOutput.Replace("[[BBC News]]", "");
            //Assert.IsTrue(result.Length > 0);
        }

        [TestMethod]
        public void TestCustomTagAccessToWebService()
        {
            //var fi = new FileInfo(pathToCustomTagDll);
            //Assert.AreEqual(true, fi.Exists);
            //_chatBot.LoadCustomTagHandlers(pathToCustomTagDll);
            //var output = _chatBot.Chat("what is my fortune", "1");
            //Assert.IsTrue(output.RawOutput.Length > 0);
        }

        [TestMethod]
        public void TestCustomTagGoodData()
        {
            //var fi = new FileInfo(pathToCustomTagDll);
            //Assert.AreEqual(true, fi.Exists);
            //_chatBot.LoadCustomTagHandlers(pathToCustomTagDll);
            //var output = _chatBot.Chat("test custom tag", "1");
            //Assert.AreEqual("Test tag works! inner text is here.", output.RawOutput);
        }

        [TestMethod]
        public void TestCustomTagPigLatin()
        {
            //var fi = new FileInfo(pathToCustomTagDll);
            //Assert.AreEqual(true, fi.Exists);
            //_chatBot.LoadCustomTagHandlers(pathToCustomTagDll);
            //var output = _chatBot.Chat("Test pig latin", "1");
            //Assert.AreEqual("(Allway ethay orldway isway away agestay!).", output.Output);
        }
    }
}