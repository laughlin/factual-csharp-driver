using FactualDriver.JsonConverters;
using FactualDriver.Utils;
using Newtonsoft.Json;

namespace FactualDriver.Filters
{
    [JsonConverter(typeof(PointConverter))]
    public class Point : IFilter
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        private string _name = Constants.FILTER_GEO;

        private string _pointKey = Constants.POINT;

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

        public string ToUrlQuery()
        {
            return JsonUtil.ToQueryString(this);
        }
    }
}