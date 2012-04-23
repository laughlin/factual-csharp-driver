using System.Collections.Generic;

namespace FactualDriver.Utils
{
    public class ParameterCollection
    {
        private readonly Dictionary<string, object> _parameters;

        internal ParameterCollection(Dictionary<string, object> parameters)
        {
            this._parameters = parameters;
        }

        public object this[string name]
        {
            get { return _parameters[name]; }
        }

        public int Count
        {
            get { return _parameters.Count; }
        }
    }