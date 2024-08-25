using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Collections;
using Querier.SqlQuery;
using MySql.Data.MySqlClient;
using Querier.SqlQuery.Interfaces;
using System.Data.SqlClient;
using Dapper;

namespace Querier
{
    public class Query<TQuery> : IQuery where TQuery : IBaseQuery<TQuery>
    {
        private readonly List<QueryMeasure> _queryMeasures;
        private readonly List<QueryDimension> _queryDimension;
        private QueryTimeDimension _queryTimeDimension;


        private readonly TQuery _query;
        private readonly IQueryDbConnection _connection;

        public Query(TQuery query, IQueryDbConnection connection)
        {
            _query = query;
            _queryMeasures = new List<QueryMeasure>();
            _queryDimension = new List<QueryDimension>();
            _connection = connection;
        }

        public IQuery New()
        {
            return new Query<TQuery>(_query, _connection);
        }
        public IQuery From(string table)
        {
            _query.From(table);
            return this;
        }

        public IQuery MeasureCount(string property, string? propertyAs = null, string? orderBy = null)
        {
            _query.SelectCount(property, propertyAs);

            if (!string.IsNullOrWhiteSpace(orderBy))
            {
                _query.OrderBy(property, orderBy);
            }

            var measure = new QueryMeasure() { Property = property, OrderBy = orderBy };
            _queryMeasures.Add(measure);

            return this;
        }

        public IQuery MeasureSum(string property, string? propertyAs = null, string? orderBy = null)
        {
            _query.SelectSum(property, propertyAs);

            if (!string.IsNullOrWhiteSpace(orderBy))
            {
                _query.OrderBy(property, orderBy);
            }

            var measure = new QueryMeasure() { Property = property, OrderBy = orderBy };
            _queryMeasures.Add(measure);

            return this;
        }

        public IQuery MeasureAvg(string property, string? propertyAs = null, string? orderBy = null)
        {
            _query.SelectAvg(property, propertyAs);

            if (!string.IsNullOrWhiteSpace(orderBy))
            {
                _query.OrderBy(property, orderBy);
            }

            var measure = new QueryMeasure() { Property = property, OrderBy = orderBy };
            _queryMeasures.Add(measure);

            return this;
        }

        public IQuery MeasureMin(string property, string? propertyAs = null, string? orderBy = null)
        {
            _query.SelectMin(property, propertyAs);

            if (!string.IsNullOrWhiteSpace(orderBy))
            {
                _query.OrderBy(property, orderBy);
            }

            var measure = new QueryMeasure() { Property = property, OrderBy = orderBy };
            _queryMeasures.Add(measure);

            return this;
        }

        public IQuery MeasureMax(string property, string? propertyAs = null, string? orderBy = null)
        {
            _query.SelectMax(property, propertyAs);

            if (!string.IsNullOrWhiteSpace(orderBy))
            {
                _query.OrderBy(property, orderBy);
            }

            var measure = new QueryMeasure() { Property = property, OrderBy = orderBy };
            _queryMeasures.Add(measure);

            return this;
        }

        public IQuery Dimension(string property)
        {
            _query.Select(property).GroupBy(property);

            var dimension = new QueryDimension() { Property = property };
            _queryDimension.Add(dimension);

            return this;
        }
        public IQuery TimeDimension(string property)
        {
            _query.Select(property).GroupBy(property);

            _queryTimeDimension = new QueryTimeDimension() { Property = property };

            return this;
        }
        public IQuery Filter(Func<IQueryFilter, IQueryFilter> filter)
        {
            var queryFilter = new QueryFilter<TQuery>(_query);
            filter.Invoke(queryFilter);

            return this;
        }
        public IQuery TimeDimension(string property, string timeDimensionPart)
        {
            switch (timeDimensionPart)
            {
                case "date":
                    _query.SelectSecond(property).GroupBy(e => e.Date(property));
                    break;
                case "second":
                    _query.SelectSecond(property).GroupBy(e => e.Second(property));
                    break;
                case "minute":
                    _query.SelectMinute(property).GroupBy(e => e.Minute(property));
                    break;
                case "hour":
                    _query.SelectHour(property).GroupBy(e => e.Hour(property));
                    break;
                case "day":
                    _query.SelectDay(property).GroupBy(e => e.Day(property));
                    break;
                case "month":
                    _query.SelectMonth(property).GroupBy(e => e.Month(property));
                    break;
                case "year":
                    _query.SelectYear(property).GroupBy(e => e.Year(property));
                    break;
                default:
                    _query.Select(property).GroupBy(property);
                    break;
            }
            _queryTimeDimension = new QueryTimeDimension() { Property = property, TimeDimensionPart = timeDimensionPart };

            return this;
        }
        public IQuery OrderBy(string property, string direction)
        {
            _query.OrderBy(property, direction);
            _queryTimeDimension = new QueryTimeDimension() { Property = property };

            return this;
        }

        public IQuery Limit(int limit)
        {
            _query.Limit(limit);
            return this;
        }



        public List<QueryProperty> GetMeasures<TType>() => QueryHelper.GetMeasureProperties<TType>();
        public List<QueryProperty> GetMeasures(string queryKey) => QueryHelper.GetMeasureProperties(queryKey);
        public List<QueryProperty> GetDimensions<TType>() => QueryHelper.GetDimensionProperties<TType>();
        public List<QueryProperty> GetDimensions(string queryKey) => QueryHelper.GetMeasureProperties(queryKey);
        public List<QueryProperty> GetTimeDimensions<TType>() => QueryHelper.GetTimeDimensionProperties<TType>();
        public List<QueryProperty> GetTimeDimensions(string queryKey) => QueryHelper.GetMeasureProperties(queryKey);

        public QueryResult Execute()
        {
            var result = new QueryResult()
            {
                Measures = _queryMeasures.Select(e => new QueryProperty() { Key = e.Property, DisplayName = e.Property }).ToList(),
                Dimensions = _queryDimension.Select(e => new QueryProperty() { Key = e.Property, DisplayName = e.Property }).ToList(),
            };
            if (_queryTimeDimension != null)
            {
                result.TimeDimensions = new List<QueryProperty>() { new QueryProperty() { Key = _queryTimeDimension.Property, DisplayName = _queryTimeDimension.Property } };
            }

            var complie = _query.Compile();

            result.Data = _connection.Connection
                .Query(complie.CompiledSql, complie.SqlParameters)
                .Cast<IDictionary<string, object>>()
                .Select(e => e.ToDictionary(k => k.Key, v => v.Value));

            return result;
        }
    }
}
