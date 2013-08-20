using System;

namespace FactualDriver.Exceptions
{
    /// <summary>
    /// A generic Factual exception class
    /// </summary>
    [Serializable]
    public class FactualException : Exception
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message"></param>
        public FactualException(string message) : base(message) {}
    }
}