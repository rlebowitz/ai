using AIMLbot.Utils;

namespace AIMLbot.Normalize
{
    /// <summary>
    /// Normalizes the input text into upper case
    /// </summary>
    public class MakeCaseInsensitive : TextTransformer
    {
        public MakeCaseInsensitive(ChatBot chatBot, string inputString) : base(chatBot, inputString)
        {
        }

        public MakeCaseInsensitive(ChatBot chatBot) : base(chatBot)
        {
        }

        protected override string ProcessChange()
        {
            return inputString.ToUpper();
        }

        /// <summary>
        /// An ease-of-use static method that re-produces the instance transformation methods
        /// </summary>
        /// <param name="input">The string to transform</param>
        /// <returns>The resulting string</returns>
        public static string TransformInput(string input)
        {
            return input.ToUpper();
        }
    }
}