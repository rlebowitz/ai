using AIMLbot.Utils;

namespace AIMLbot.Normalize
{
    /// <summary>
    /// Strips any illegal characters found within the input string. Illegal characters are referenced from
    /// the ChatBot's Strippers regex that is defined in the setup XML file.
    /// </summary>
    public class StripIllegalCharacters : TextTransformer
    {
        public StripIllegalCharacters(ChatBot chatBot, string inputString) : base(chatBot, inputString)
        {
        }

        public StripIllegalCharacters(ChatBot chatBot)
            : base(chatBot)
        {
        }

        protected override string ProcessChange()
        {
            return ChatBot.Strippers.Replace(inputString, " ");
        }
    }
}