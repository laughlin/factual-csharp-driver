namespace FactualDriver.Filters
{
    public interface IFilter
    {
        string ParameterName { get; set; }
        bool IsText { get; set; }
    }
}