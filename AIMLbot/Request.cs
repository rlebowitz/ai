using System;

namespace AIMLbot
{
    /// <summary>
    /// Encapsulates all sorts of information about a request to the ChatBot for processing
    /// </summary>
    public class Request
    {
        #region Attributes

        /// <summary>
        /// The raw input from the user
        /// </summary>
        public string RawInput;

        /// <summary>
        /// The final result produced by this request
        /// </summary>
        public Result Result;

        /// <summary>
        /// The time at which this request was created within the system
        /// </summary>
        public DateTime StartedOn;

        /// <summary>
        /// The user who made this request
        /// </summary>
        public User User;

        #endregion

        /// <summary>
        /// Creates an instance of the Request object.
        /// </summary>
        /// <param name="rawInput">The raw input from the user</param>
        /// <param name="user">The user who made the request</param>
        public Request(string rawInput, User user)
        {
            RawInput = rawInput;
            User = user;
            StartedOn = DateTime.Now;
        }
    }
}