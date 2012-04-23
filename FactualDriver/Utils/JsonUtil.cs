using System.Collections.Generic;
using System.Text;
using FactualDriver.Filters;
using Newtonsoft.Json;

namespace FactualDriver.Utils
{
    public class JsonUtil
    {
        public static string ToQueryString(IFilter filter)
        {
            return string.Format("{0}={1}", filter.ParameterName, filter.IsText ? filter.ToString() : JsonConvert.SerializeObject(filter));
        }

         public static string ToQueryString(IEnumerable<IFilter> filters)
         {
             var parameters = new List<string>();
             foreach (var filter in filters)
             {

                 parameters.Add(string.Format("{0}={1}", filter.ParameterName, filter.IsText ? filter.ToString() : JsonConvert.SerializeObject(filter)));
             }
             return string.Join("&", parameters);
         }

       
    }
}