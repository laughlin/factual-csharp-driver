using FactualDriver.Filters;

namespace FactualDriver
{
    public interface IQuery
    {
        void Add(IFilter filter);

        string ToUrlQuery();
    }
}