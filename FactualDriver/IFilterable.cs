using FactualDriver.Filters;

namespace FactualDriver
{
    public interface IFilterable
    {
        void Add(IFilter filter);
    }
}