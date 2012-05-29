using FactualDriver.JsonConverters;
using Newtonsoft.Json;

namespace FactualDriver.Filters
{
    [JsonConverter(typeof(GeoFilterConverter))]
    public class GeoFilter : IFilter
    {
        private string _name = Constants.FILTER_GEO;

        private string _shape = Constants.CIRCLE;
        private string _target = Constants.CENTER;
        private string _distanceUnits = Constants.METERS;

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

        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int Distance { get; set; }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// GEO filter which sets geographical condiitons on the factual api call
        /// </summary>
        /// <param name="latitude">Latitude of the starting point</param>
        /// <param name="longitude">Longitude of the starting point</param>
        /// <param name="distance">Radius distance from the starting point</param>
        public GeoFilter(double latitude, double longitude, int distance)
        {
            Latitude = latitude;
            Longitude = longitude;
            Distance = distance;
        }
    }
}