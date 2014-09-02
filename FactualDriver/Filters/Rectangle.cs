using FactualDriver.JsonConverters;
using FactualDriver.Utils;
using Newtonsoft.Json;

namespace FactualDriver.Filters
{
    /// <summary>
    /// Rectangle filter class, used to serilize geo envelope data for GeoFilters.
    /// </summary>
    [JsonConverter(typeof(RectangleConverter))]
    public class Rectangle : IFilter
    {
        /// <summary>
        /// Rectangle's top left latitude.
        /// </summary>
        public double TopLeftLatitude { get; set; }

        /// <summary>
        /// Rectangle's top left longitude.
        /// </summary>
        public double TopLeftLLongitude { get; set; }

        /// <summary>
        /// Rectangle's bottom right latitude.
        /// </summary>
        public double BottonRightLatitude { get; set; }

        /// <summary>
        /// Rectangle's bottom right longitude.
        /// </summary>
        public double BottonRightLongitude { get; set; }

        private string _name = Constants.FILTER_GEO;

        private string _rectKey = Constants.RECT;

        /// <summary>
        /// Rect key name, defaults to $rect.
        /// </summary>
        public string RectKey { get { return _rectKey; } set { _rectKey = value; } }

        /// <summary>
        /// Constructs a geographic Rectangle representation.
        /// </summary>
        /// <param name="topLeftLat">Rectangle's top left latitude.</param>
        /// <param name="topLeftLong">Rectangle's top left longitude.</param>
        /// <param name="bottomRightLat">Rectangle's bottom right latitude.</param>
        /// <param name="bottomRightLong">Rectangle's bottom right longitude.</param>
        public Rectangle(double topLeftLat, double topLeftLong, double bottomRightLat, double bottomRightLong)
        {
            TopLeftLatitude = topLeftLat;
            TopLeftLLongitude = topLeftLong;
            BottonRightLatitude = bottomRightLat;
            BottonRightLongitude = bottomRightLong;
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

    }
}
