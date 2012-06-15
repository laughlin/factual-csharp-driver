using System;

namespace FactualDriver.Exceptions
{
    /// <summary>
    /// A generic Factual exception class
    /// </summary>
    public class FactualException : Exception
    {
        public FactualException(string message) : base(message) {}
    }
}