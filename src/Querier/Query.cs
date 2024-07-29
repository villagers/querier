using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using System.Collections;

namespace Querier
{
    public class Query<TContext> : IQuery<TContext> where TContext : DbContext
    {
        private readonly IQueryContext<TContext> _queryContext;

        public Query(IQueryContext<TContext> queryContext)
        {
            _queryContext = queryContext;
        }

        public IQuery<TType, TContext> Create<TType>() where TType : class
        {
            return new Query<TType, TContext>(_queryContext);
        }

        public List<QueryProperty> ListMeasures<TType>()
        {
            return QueryHelper.ListMeasureProperties<TType>();
        }
        public List<QueryProperty> ListMeasures(Type type)
        {
            return QueryHelper.ListMeasureProperties(type);
        }
        public List<QueryProperty> ListMeasures(string queryKey)
        {
            return QueryHelper.ListMeasureProperties(queryKey);
        }

        public List<QueryProperty> ListDimensions<TType>()
        {
            return QueryHelper.ListDimensionProperties<TType>();
        }
        public List<QueryProperty> ListDimensions(Type type)
        {
            return QueryHelper.ListDimensionProperties(type);
        }
        public List<QueryProperty> ListDimensions(string queryKey)
        {
            return QueryHelper.ListDimensionProperties(queryKey);
        }

        public List<QueryProperty> ListTimeDimensions<TType>()
        {
            return QueryHelper.ListTimeDimensionProperties<TType>();
        }
        public List<QueryProperty> ListTimeDimensions(Type type)
        {
            return QueryHelper.ListTimeDimensionProperties(type);
        }
        public List<QueryProperty> ListTimeDimensions(string queryKey)
        {
            return QueryHelper.ListTimeDimensionProperties(queryKey);
        }
    }
    public class Query<TType, TContext> : IQuery<TType, TContext>
        where TContext : DbContext
        where TType : class
    {
        private readonly Type _type;
        private readonly TContext _dbContext;

        private readonly List<QueryMeasure<TType>> _queryMeasures;
        private readonly List<QueryDimension<TType>> _queryDimension;
        private QueryTimeDimension<TType> _queryTimeDimension;

        private int _limit = 50000;
        private List<QueryFilter> _queryFilters;

        private readonly IQueryContext<TContext> _queryContext;


        public Query(IQueryContext<TContext> queryContext)
        {
            _type = typeof(TType);
            _queryMeasures = new List<QueryMeasure<TType>>();
            _queryDimension = new List<QueryDimension<TType>>();
            _queryFilters = new List<QueryFilter>();
            _queryContext = queryContext;
        }

        public IQuery<TType, TContext> Filter(string property, string op, object? args)
        {
            _queryFilters.Add(new QueryFilter()
            {
                Property = property,
                Operator = op,
                Args = args
            });
            return this;
        }

        public IQuery<TType, TContext> Measure(string property, string? orderBy = null)
        {
            var measure = new QueryMeasure<TType>() { Property = property, OrderBy = orderBy };
            _queryMeasures.Add(measure);

            return this;
        }

        public IQuery<TType, TContext> Dimension(string property)
        {
            var dimension = new QueryDimension<TType>() { Property = property };
            _queryDimension.Add(dimension);

            return this;
        }

        public IQuery<TType, TContext> TimeDimension(string property)
        {
            _queryTimeDimension = new QueryTimeDimension<TType>() { Property = property };

            return this;
        }
        public IQuery<TType, TContext> TimeDimension(string property, TimeDimensionPart timeDimensionPart)
        {
            _queryTimeDimension = new QueryTimeDimension<TType>() { Property = property, TimeDimensionPart = timeDimensionPart };

            return this;
        }

        public IQuery<TType, TContext> OrderBy(string property, string direction)
        {
            _queryTimeDimension = new QueryTimeDimension<TType>() { Property = property };

            return this;
        }

        public IQuery<TType, TContext> Limit(int limit)
        {
            _limit = limit;

            return this;
        }

        public QueryResult Execute()
        {
            var selectors = new List<string>();
            selectors.AddRange(_queryMeasures.Select(e => e.Property));
            selectors.AddRange(_queryDimension.Select(e => e.Property));
            if (_queryTimeDimension != null)
            {
                selectors.Add(_queryTimeDimension.Property);
                if (_queryTimeDimension.TimeDimensionPart.HasValue)
                {
                    var part = _queryTimeDimension.TimeDimensionPart.ToString();
                    selectors.Add($"{_queryTimeDimension.Property}.{part} as {_queryTimeDimension.Property}{part}");
                }
            }
            var selectExpression = "new {" + string.Join(",", selectors) + "}";

            var groupSelectors = new List<string>();
            groupSelectors.AddRange(_queryDimension.Select(e => e.Property));
            if (_queryTimeDimension != null)
            {
                groupSelectors.Add($"{_queryTimeDimension.Property} as {_queryTimeDimension.Property}");
                if (_queryTimeDimension.TimeDimensionPart.HasValue)
                {
                    var part = _queryTimeDimension.TimeDimensionPart.ToString();
                    groupSelectors.Add($"{_queryTimeDimension.Property}.{part} as {_queryTimeDimension.Property}{part}");
                }
            }
            var groupExpression = $"GROUP BY {groupSelectors.Count}";
            if (groupSelectors.Count > 0)
            {
                groupExpression = $"new ({string.Join(",", groupSelectors)})";
            }

            var orderByExpression = $"";
            var orderSelectors = new List<string>();
            orderSelectors.AddRange(_queryMeasures.Where(e => !string.IsNullOrWhiteSpace(e.OrderBy)).Select(e => $"{e.Property} {e.OrderBy}"));
            orderSelectors.AddRange(_queryDimension.Where(e => !string.IsNullOrWhiteSpace(e.OrderBy)).Select(e => $"{e.Property} {e.OrderBy}"));
            if (_queryTimeDimension != null && !string.IsNullOrWhiteSpace(_queryTimeDimension.OrderBy))
            {
                orderSelectors.Add(_queryTimeDimension.OrderBy);
            }
            if (orderSelectors.Count != 0)
            {
                orderByExpression = $"{string.Join(",", orderSelectors)}";
            }


            var newSelectors = new List<string>();
            newSelectors.AddRange(_queryMeasures.Select(e => $"SUM({e.Property}) as {e.Property}"));
            newSelectors.AddRange(_queryDimension.Select(m => $"Key.{m.Property}"));
            if (_queryTimeDimension != null)
            {
                newSelectors.Add($"Key.{_queryTimeDimension.Property} as {_queryTimeDimension.Property}");
                if (_queryTimeDimension.TimeDimensionPart.HasValue)
                {
                    var part = _queryTimeDimension.TimeDimensionPart.ToString();
                    newSelectors.Add($"Key.{_queryTimeDimension.Property}{part} as {_queryTimeDimension.Property}{part}");
                }
            }
            var newSelectExpression = "new (" + string.Join(",", newSelectors) + ")";

            var queryBuilder = _queryContext.GetContext().Set<TType>().AsNoTracking().Select(selectExpression);
            queryBuilder = queryBuilder.GroupBy(groupExpression, "it");
            queryBuilder = queryBuilder.Select(newSelectExpression);

            foreach (var filter in _queryFilters)
            {
                switch (filter.Operator)
                {
                    case "=":
                    case ">=":
                    case "<=":
                        queryBuilder = queryBuilder.Where($"{filter.Property} {filter.Operator} @0", filter.Args);
                        break;
                    case "in":
                        queryBuilder = queryBuilder.Where($"@0.Contains({filter.Property})", filter.Args);
                        break;
                }

            }

            if (!string.IsNullOrWhiteSpace(orderByExpression))
            {
                queryBuilder = queryBuilder.OrderBy(orderByExpression);
            }
            queryBuilder.Take(_limit);

            var data = queryBuilder.ToDynamicList();

            var result = new QueryResult()
            {
                Data = data,
                Filters = _queryFilters,
                Measures = _queryMeasures.Select(e => QueryHelper.GetMeasureProperty<TType>(e.Property)).ToList(),
                Dimensions = _queryDimension.Select(e => QueryHelper.GetDimensionProperty<TType>(e.Property)).ToList(),
                TimeDimensions = [QueryHelper.GetTimeDimensionProperty<TType>(_queryTimeDimension.Property)],
            };



            return result;
        }
    }
}
