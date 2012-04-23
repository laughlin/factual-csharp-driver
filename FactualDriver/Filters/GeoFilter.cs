using FactualDriver.JsonConverters;
using Newtonsoft.Json;

namespace FactualDriver.Filters
{
    [JsonConverter(typeof(GeoFilterConverter))]
    public class GeoFilter : IFilter
    {
        private string _parameterName = "geo";

        private string _shape = "$circle";
        private string _target = "$center";
        private string _distanceUnits = "$meters";

        public string Shape
        {
            get { return _shape; }
            set { _shape = value; }
        }

        public string Target
        {
            get { return _target; }
            set { _target = value; }
        }

        
        public string DistanceUnits
        {
            get { return _distanceUnits; }
            set { _distanceUnits = value; }
        }

        public bool IsText { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public int Distance { get; set; }

        public string ParameterName
        {
            get { return _parameterName; }
            set { _parameterName = value; }
        }

        /// <summary>
        /// GEO filter which sets geographical condiitons on the factual api call
        /// </summary>
        /// <param name="latitude">Latitude of the starting point</param>
        /// <param name="longitude">Longitude of the starting point</param>
        /// <param name="distance">Radius distance from the starting point</param>
        public GeoFilter(decimal latitude, decimal longitude, int distance)
        {
            Latitude = latitude;
            Longitude = longitude;
            Distance = distance;
        }
    }
}