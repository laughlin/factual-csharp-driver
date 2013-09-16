using System;
using System.Collections.Generic;
using FactualDriver.JsonConverters;
using Newtonsoft.Json;

namespace FactualDriver.Filters
{

    /// <summary>
    /// A generic filter which serialises results into json key value pairs
    /// </summary>
    [JsonConverter(typeof(KeyValueFilterConverter))]
    public class KeyValueFilter : IFilter
    {
        /// <summary>
        /// Filter key.
        /// </summary>
        public string Name { get; set; }

        public Dictionary<string,object> Pairs { get; private set; }

        public KeyValueFilter(string name, Dictionary<string,object> pairs)
        {
            Name = name;
            Pairs = pairs;
        }
    }
}