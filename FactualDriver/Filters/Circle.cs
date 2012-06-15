namespace FactualDriver.Filters
{
    /// <summary>
    /// Represents a geographic sub query confining results to a circle.
    /// </summary>
    public class Circle
    {
        private readonly IFilter _filter;
        
        /// <summary>
        /// Constructs a geographic Circle representation.
        /// </summary>
        /// <param name="centerLat">the latitude of the center of this Circle.</param>
        /// <param name="centerLong">the longitude of the center of this Circle.</param>
        /// <param name="meters">the radius, in meters, of this Circle.</param>
        public Circle(double centerLat, double centerLong, int meters)
        {
            _filter = new GeoFilter(centerLat,centerLong,meters);     
        }

        /// <summary>
        /// Get filter created from circle
        /// </summary>
        /// <returns>Returns IFilter object</returns>
        public IFilter GetFilter()
        {
            return _filter;
        }
    }
}