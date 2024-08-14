using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using Querier.SqlQuery;
using MySql.Data.MySqlClient;
using SqlKata.Compilers;
using SqlKata.Execution;

namespace Querier
{
    public class Query : IQuery
    {
        private readonly List<QueryMeasure> _queryMeasures;
        private readonly List<QueryDimension> _queryDimension;
        private QueryTimeDimension _queryTimeDimension;

        private int _limit = 50000;
        private List<QueryFilter> _queryFilters;


        private readonly SqlQuery.SqlQuery _sqlQuery;

        public Query(string table)
        {
            _sqlQuery = new SqlQuery.SqlQuery(table);


            //var db = new QueryFactory(null, null);
            //db.Query("table").AddComponent

            _queryMeasures = new List<QueryMeasure>();
            _queryDimension = new List<QueryDimension>();
            _queryFilters = new List<QueryFilter>();
        }

        public IQuery Create(string table)
        {
            return new Query(table);
        }
        public IQuery Filter(string property, string op, object? args)
        {
            _sqlQuery.WhereEqual(property, args);
            _queryFilters.Add(new QueryFilter()
            {
                Property = property,
                Operator = op,
                Args = args
            });
            return this;
        }
        public IQuery AndFilter(string property, string op, object? args)
        {
            _sqlQuery.WhereEqual(property, args);
            _queryFilters.Add(new QueryFilter()
            {
                Property = property,
                Operator = op,
                Args = args
            });
            return this;
        }
        public IQuery OrFilter(string property, string op, object? args)
        {
            _sqlQuery.WhereEqual(property, args);
            _queryFilters.Add(new QueryFilter()
            {
                Property = property,
                Operator = op,
                Args = args
            });
            return this;
        }
        public IQuery Measure(string aggregation, string property, string? orderBy = null)
        {
            _sqlQuery.Select(aggregation, property);

            if (!string.IsNullOrWhiteSpace(orderBy))
            {
                _sqlQuery.OrderBy(property, orderBy);
            }

            var measure = new QueryMeasure() { Property = property, OrderBy = orderBy };
            _queryMeasures.Add(measure);

            return this;
        }

        public IQuery MeasureCount(string property, string? orderBy = null)
        {
            _sqlQuery.SelectCount(property);

            if (!string.IsNullOrWhiteSpace(orderBy))
            {
                _sqlQuery.OrderBy(property, orderBy);
            }

            var measure = new QueryMeasure() { Property = property, OrderBy = orderBy };
            _queryMeasures.Add(measure);

            return this;
        }

        public IQuery MeasureSum(string property, string? orderBy = null)
        {
            _sqlQuery.SelectSum(property);

            if (!string.IsNullOrWhiteSpace(orderBy))
            {
                _sqlQuery.OrderBy(property, orderBy);
            }

            var measure = new QueryMeasure() { Property = property, OrderBy = orderBy };
            _queryMeasures.Add(measure);

            return this;
        }

        public IQuery MeasureAvg(string property, string? orderBy = null)
        {
            _sqlQuery.SelectAvg(property);

            if (!string.IsNullOrWhiteSpace(orderBy))
            {
                _sqlQuery.OrderBy(property, orderBy);
            }

            var measure = new QueryMeasure() { Property = property, OrderBy = orderBy };
            _queryMeasures.Add(measure);

            return this;
        }

        public IQuery MeasureMin(string property, string? orderBy = null)
        {
            _sqlQuery.SelectMin(property);

            if (!string.IsNullOrWhiteSpace(orderBy))
            {
                _sqlQuery.OrderBy(property, orderBy);
            }

            var measure = new QueryMeasure() { Property = property, OrderBy = orderBy };
            _queryMeasures.Add(measure);

            return this;
        }

        public IQuery MeasureMax(string property, string? orderBy = null)
        {
            _sqlQuery.SelectMax(property);

            if (!string.IsNullOrWhiteSpace(orderBy))
            {
                _sqlQuery.OrderBy(property, orderBy);
            }

            var measure = new QueryMeasure() { Property = property, OrderBy = orderBy };
            _queryMeasures.Add(measure);

            return this;
        }

        public IQuery Dimension(string property)
        {
            _sqlQuery.Select(property).GroupBy(property);

            var dimension = new QueryDimension() { Property = property };
            _queryDimension.Add(dimension);

            return this;
        }
        public IQuery TimeDimension(string property)
        {
            _sqlQuery.Select(property).GroupBy(property);

            _queryTimeDimension = new QueryTimeDimension() { Property = property };

            return this;
        }
        public IQuery TimeDimension(string property, TimeDimensionPart timeDimensionPart)
        {
            _sqlQuery.Select(property).GroupBy(property);
            _queryTimeDimension = new QueryTimeDimension() { Property = property, TimeDimensionPart = timeDimensionPart };

            return this;
        }
        public IQuery OrderBy(string property, string direction)
        {
            _sqlQuery.OrderBy(property, direction);
            _queryTimeDimension = new QueryTimeDimension() { Property = property };

            return this;
        }

        public IQuery Limit(int limit)
        {
            _sqlQuery.Limit(limit);
            _limit = limit;

            return this;
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

        public QueryResult Execute()
        {

            var complie = _sqlQuery.Compile();

            var sasd = new List<object>().AsQueryable().GroupBy("").ToQueryString();


            return new QueryResult();

            //var selectors = new List<string>();
            //selectors.AddRange(_queryMeasures.Select(e => e.Property));
            //selectors.AddRange(_queryDimension.Select(e => e.Property));
            //if (_queryTimeDimension != null)
            //{
            //    selectors.Add(_queryTimeDimension.Property);
            //    if (_queryTimeDimension.TimeDimensionPart.HasValue)
            //    {
            //        var part = _queryTimeDimension.TimeDimensionPart.ToString();
            //        selectors.Add($"{_queryTimeDimension.Property}.{part} as {_queryTimeDimension.Property}{part}");
            //    }
            //}
            //var selectExpression = "new {" + string.Join(",", selectors) + "}";

            //var groupSelectors = new List<string>();
            //groupSelectors.AddRange(_queryDimension.Select(e => e.Property));
            //if (_queryTimeDimension != null)
            //{
            //    groupSelectors.Add($"{_queryTimeDimension.Property} as {_queryTimeDimension.Property}");
            //    if (_queryTimeDimension.TimeDimensionPart.HasValue)
            //    {
            //        var part = _queryTimeDimension.TimeDimensionPart.ToString();
            //        groupSelectors.Add($"{_queryTimeDimension.Property}.{part} as {_queryTimeDimension.Property}{part}");
            //    }
            //}
            //var groupExpression = $"GROUP BY {groupSelectors.Count}";
            //if (groupSelectors.Count > 0)
            //{
            //    groupExpression = $"new ({string.Join(",", groupSelectors)})";
            //}

            //var orderByExpression = $"";
            //var orderSelectors = new List<string>();
            //orderSelectors.AddRange(_queryMeasures.Where(e => !string.IsNullOrWhiteSpace(e.OrderBy)).Select(e => $"{e.Property} {e.OrderBy}"));
            //orderSelectors.AddRange(_queryDimension.Where(e => !string.IsNullOrWhiteSpace(e.OrderBy)).Select(e => $"{e.Property} {e.OrderBy}"));
            //if (_queryTimeDimension != null && !string.IsNullOrWhiteSpace(_queryTimeDimension.OrderBy))
            //{
            //    orderSelectors.Add(_queryTimeDimension.OrderBy);
            //}
            //if (orderSelectors.Count != 0)
            //{
            //    orderByExpression = $"{string.Join(",", orderSelectors)}";
            //}


            //var newSelectors = new List<string>();
            //newSelectors.AddRange(_queryMeasures.Select(e => $"SUM({e.Property}) as {e.Property}"));
            //newSelectors.AddRange(_queryDimension.Select(m => $"Key.{m.Property}"));
            //if (_queryTimeDimension != null)
            //{
            //    newSelectors.Add($"Key.{_queryTimeDimension.Property} as {_queryTimeDimension.Property}");
            //    if (_queryTimeDimension.TimeDimensionPart.HasValue)
            //    {
            //        var part = _queryTimeDimension.TimeDimensionPart.ToString();
            //        newSelectors.Add($"Key.{_queryTimeDimension.Property}{part} as {_queryTimeDimension.Property}{part}");
            //    }
            //}
            //var newSelectExpression = "new (" + string.Join(",", newSelectors) + ")";

            //var queryBuilder = _queryContext.GetContext().Set<TType>().AsNoTracking().Select(selectExpression);
            //queryBuilder = queryBuilder.GroupBy(groupExpression, "it");
            //queryBuilder = queryBuilder.Select(newSelectExpression);

            //foreach (var filter in _queryFilters)
            //{
            //    switch (filter.Operator)
            //    {
            //        case "=":
            //        case ">=":
            //        case "<=":
            //            queryBuilder = queryBuilder.Where($"{filter.Property} {filter.Operator} @0", filter.Args);
            //            break;
            //        case "in":
            //            queryBuilder = queryBuilder.Where($"@0.Contains({filter.Property})", filter.Args);
            //            break;
            //    }

            //}

            //if (!string.IsNullOrWhiteSpace(orderByExpression))
            //{
            //    queryBuilder = queryBuilder.OrderBy(orderByExpression);
            //}
            //queryBuilder.Take(_limit);

            //var data = queryBuilder.ToDynamicList();

            //var result = new QueryResult()
            //{
            //    Data = data,
            //    Filters = _queryFilters,
            //    Measures = _queryMeasures.Select(e => QueryHelper.GetMeasureProperty<TType>(e.Property)).ToList(),
            //    Dimensions = _queryDimension.Select(e => QueryHelper.GetDimensionProperty<TType>(e.Property)).ToList(),
            //    TimeDimensions = [QueryHelper.GetTimeDimensionProperty<TType>(_queryTimeDimension.Property)],
            //};



            //return result;
        }
    }
}
