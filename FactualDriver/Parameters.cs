using System;
using System.Collections.Generic;
using System.Linq;
using FactualDriver.Filters;

namespace FactualDriver
{
    public class Parameters
    {
        private List<IFilter> _filters = new List<IFilter>();
        public List<IFilter> Filters { get { return _filters; } set { _filters = value; } }


        public void Add(string filterName, object value)
        {
            _filters.Add(new Filter(filterName, value));
        }

        public void Add(IFilter filter)
        {
            if (filter is RowFilter || filter is FilterGroup)
            {
                AddRowFilter(filter);
                return;
            }

            _filters.Add(filter);
        }

        private void AddRowFilter(IFilter filter)
        {
            if (_filters.All(p => p.GetType() != typeof(FilterList)))
            {
                _filters.Add(new FilterList());
            }

            var list = GetRowFilterList();
            list.Add(filter);
        }

        public void AddCommaSeparatedFilter(string filterName, string value)
        {
            var filter = _filters.FirstOrDefault(p => p.Name == filterName);
            if (filter != null && filter is Filter)
            {
                var existingFilter = (Filter)filter;
                existingFilter.Value = string.Format("{0},{1}", existingFilter.Value, value);
            }
            else
            {
                _filters.Add(new Filter(filterName, value));
            }
        }

        public List<IFilter> GetRowFilterList()
        {
            return ((FilterList)_filters.Single(p => p.GetType() == typeof(FilterList))).Data;
        }

        public void PopRowFiltersIntoNewGroup(string operation, IQuery[] queries)
        {
            var filterCount = queries.Count();
            var filterGroup = new FilterGroup(operation);
            var filterList = GetRowFilterList();
            filterGroup.RowFilters = filterList.Skip(Math.Max(0, filterList.Count - filterCount)).Take(filterCount).ToList();
            filterList.RemoveRange(filterList.Count - filterCount, filterCount);

            Add(filterGroup);
        }

        public IFilter[] ToFilterArray()
        {
            return _filters.ToArray();
        }
    }
}