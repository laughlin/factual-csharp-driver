namespace FactualDriver.Filters
{
    /// <summary>
    /// Filter inferface.
    /// </summary>
    public interface IFilter
    {
        /// <summary>
        /// Name or key of the filter is required.
        /// </summary>
        string Name { get; set; }
    }
}