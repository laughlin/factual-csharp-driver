using System;
using System.Collections.Generic;

namespace FactualDriver.Filters
{

    /// <summary>
    /// A generic filter which serialises results into json key value pairs
    /// </summary>
    public class KeyValueFilter : IFilter
    {
        /// <summary>
        /// Filter key.
        /// </summary>
        public string Name { get; set; }

        public Dictionary<string,object> Pairs { get; set; }

        public KeyValueFilter(string name, Dictionary<string,object> pairs)
        {
            Name = name;
            Pairs = pairs;
        }
    }
}