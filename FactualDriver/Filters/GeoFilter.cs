using FactualDriver.JsonConverters;
using Newtonsoft.Json;

namespace FactualDriver.Filters
{
    /// <summary>
    /// GeoFilter class.
    /// </summary>
    [JsonConverter(typeof(GeoFilterConverter))]
    public class GeoFilter : IFilter
    {
        private string _name = Constants.FILTER_GEO;

        private string _shape = Constants.CIRCLE;
        private string _target = Constants.CENTER;
        private string _distanceUnits = Constants.METERS;

        /// <summary>
        /// Shape key, defaults to circle.
        /// </summary>
        public string Shape
        {
            get { return _shape; }
            set { _shape = value; }
        }

        /// <summary>
        /// Target key, default to center.
        /// </summary>
        public string Target
        {
            get { return _target; }
            set { _target = value; }
        }

        /// <summary>
        /// Distance units, default is meters.
        /// </summary>
        public string DistanceUnits
        {
            get { return _distanceUnits; }
            set { _distanceUnits = value; }
        }

        /// <summary>
        /// Latitude of the filter.
        /// </summary>
        public double Latitude { get; set; }
        
        /// <summary>
        /// Longitutde of the filter. 
        /// </summary>
        public double Longitude { get; set; }

        /// <summary>
        /// Distance of the filter.
        /// </summary>
        public int Distance { get; set; }

        /// <summary>
        /// Filter key, defaults to geo.
        /// </summary>
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