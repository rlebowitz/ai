using AIMLbot.Utils;
using NUnit.Framework;

namespace AIMLbot.Tests.Utils
{
    public class mockTextTransformer : TextTransformer
    {
        public mockTextTransformer(ChatBot chatBot, string inputString) : base(chatBot, inputString)
        {
        }

        public mockTextTransformer(ChatBot chatBot) : base(chatBot)
        {
        }

        protected override string ProcessChange()
        {
            return inputString.ToUpper(ChatBot.Locale);
        }
    }

    [TestFixture]
    public class TextTransformerTests
    {
        public ChatBot _mockChatBot;
        public mockTextTransformer mockObject;
        public string inputString = "Hello World!";
        public string outputString = "HELLO WORLD!";

        [OneTimeSetUp]
        public void Setup()
        {
            _mockChatBot = new ChatBot();
        }

        [Test]
        public void testInputAttributeChangesProperly()
        {
            mockObject = new mockTextTransformer(_mockChatBot, inputString);
            mockObject.InputString = "Testing123";
            Assert.AreEqual("Testing123", mockObject.InputString);
        }

        [Test]
        public void testOutputViaOutputString()
        {
            mockObject = new mockTextTransformer(_mockChatBot, inputString);
            Assert.AreEqual(outputString, mockObject.OutputString);
        }

        [Test]
        public void testOutputViaOutputStringWithNoInputString()
        {
            mockObject = new mockTextTransformer(_mockChatBot);
            Assert.AreEqual(string.Empty, mockObject.OutputString);
        }

        [Test]
        public void testOutputViaTransformNoArgs()
        {
            mockObject = new mockTextTransformer(_mockChatBot, inputString);
            Assert.AreEqual(outputString, mockObject.Transform());
        }

        [Test]
        public void testOutputViaTransformNoArgsWithNoInputString()
        {
            mockObject = new mockTextTransformer(_mockChatBot);
            Assert.AreEqual(string.Empty, mockObject.Transform());
        }

        [Test]
        public void testOutputViaTransformWithArgs()
        {
            mockObject = new mockTextTransformer(_mockChatBot);
            Assert.AreEqual(outputString, mockObject.Transform(inputString));
        }

        [Test]
        public void testOutputViaTransformWithArgsEmptyInput()
        {
            mockObject = new mockTextTransformer(_mockChatBot);
            Assert.AreEqual(string.Empty, mockObject.Transform(""));
        }

        [Test]
        public void testTextTransformerWithDefaultCtor()
        {
            mockObject = new mockTextTransformer(_mockChatBot);
            Assert.AreEqual(string.Empty, mockObject.InputString);
        }

        [Test]
        public void testTextTransformerWithInputPassedToCtor()
        {
            mockObject = new mockTextTransformer(_mockChatBot, inputString);
            Assert.AreEqual(inputString, mockObject.InputString);
        }
    }
}