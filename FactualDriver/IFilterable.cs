using FactualDriver.Filters;

namespace FactualDriver
{
    public interface IFilterable
    {
        void AddRowFilter(IFilter filter);
    }
}