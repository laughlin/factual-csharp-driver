using FactualDriver.JsonConverters;
using FactualDriver.Utils;
using Newtonsoft.Json;

namespace FactualDriver.Filters
{
    /// <summary>
    /// Point filter class, used to serilize geo point data for GeoFilters. 
    /// </summary>
    [JsonConverter(typeof(PointConverter))]
    public class Point : IFilter
    {
        /// <summary>
        /// Point's latitude.
        /// </summary>
        /// 
        public double Latitude { get; set; }
        /// <summary>
        /// Point's longitude.
        /// </summary>
        public double Longitude { get; set; }

        private string _name = Constants.FILTER_GEO;

        private string _pointKey = Constants.POINT;

        /// <summary>
        /// Point key name, defaults to $point.
        /// </summary>
        public string PointKey { get { return _pointKey; } set { _pointKey = value; } }

        /// <summary>
        /// Constructs a geographic PointKey representation.
        /// </summary>
        /// <param name="latitude">the latitude of the point.</param>
        /// <param name="longitude">the longitude of the point.</param>
        public Point(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// Converts point into uri encoded query.
        /// </summary>
        /// <returns></returns>
        public string ToUrlQuery()
        {
            return JsonUtil.ToQueryString(this);
        }
    }
}