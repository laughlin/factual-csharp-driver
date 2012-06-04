using System;
using System.Linq;
using System.Web;
using FactualDriver.Filters;
using Newtonsoft.Json;

namespace FactualDriver.Utils
{
    public class JsonUtil
    {
         public static string ToQueryString(params IFilter[] filters)
         {
             var parameters = filters.Select(filter => string.Format("{0}={1}", filter.Name, HttpUtility.UrlEncode(JsonConvert.SerializeObject(filter)))).ToList();
             return string.Join("&", parameters);
         }
    }
}