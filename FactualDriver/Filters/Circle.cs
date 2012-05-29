namespace FactualDriver.Filters
{
    public class Circle
    {
        private readonly IFilter _filter;
        
        public Circle(double centerLat, double centerLong, int meters)
        {
            _filter = new GeoFilter(centerLat,centerLong,meters);     
        }

        public IFilter GetFilter()
        {
            return _filter;
        }
    }
}