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
using Querier.Interfaces;
using Querier.Attributes;
using Querier.Helpers;

namespace Querier
{
    public class Query<TQuery> : IQuery where TQuery : IBaseQuery<TQuery>
    {
        private readonly List<QueryMeasure> _queryMeasures;
        private readonly List<QueryDimension> _queryDimension;
        private QueryTimeDimension _queryTimeDimension;

        private readonly IQueryFilter _queryFilter;

        private readonly TQuery _query;
        private readonly IQueryDbConnection _connection;

        private readonly IPropertyMapper _propertyKeyMapper;

        private string _from;
        public Query(TQuery query, IQueryDbConnection connection, IPropertyMapper propertyKeyMapper)
        {
            _query = query;
            _queryMeasures = new List<QueryMeasure>();
            _queryDimension = new List<QueryDimension>();
            _connection = connection;

            _queryFilter = new QueryFilter<TQuery>(query);
            _propertyKeyMapper = propertyKeyMapper;
        }

        public IQuery New()
        {
            return new Query<TQuery>(_query, _connection, _propertyKeyMapper);
        }
        public IQuery From(string table)
        {
            _from = table;
            _query.From(_propertyKeyMapper.GetTypeName(table) ?? table);
            return this;
        }

        public IQuery MeasureCount(string property, string? propertyAs = null, string? orderBy = null)
        {
            var propertyName = _propertyKeyMapper.GetPropertyName(_from, property) ?? property;
            _query.SelectCount(propertyName, propertyAs);

            if (!string.IsNullOrWhiteSpace(orderBy))
            {
                _query.OrderBy(propertyName, orderBy);
            }

            var measure = new QueryMeasure() { Property = property, OrderBy = orderBy };
            _queryMeasures.Add(measure);

            return this;
        }

        public IQuery MeasureSum(string property, string? propertyAs = null, string? orderBy = null)
        {
            var propertyName = _propertyKeyMapper.GetPropertyName(_from, property) ?? property;
            _query.SelectSum(propertyName, propertyAs);

            if (!string.IsNullOrWhiteSpace(orderBy))
            {
                _query.OrderBy(propertyName, orderBy);
            }

            var measure = new QueryMeasure() { Property = property, OrderBy = orderBy };
            _queryMeasures.Add(measure);

            return this;
        }

        public IQuery MeasureAvg(string property, string? propertyAs = null, string? orderBy = null)
        {
            var propertyName = _propertyKeyMapper.GetPropertyName(_from, property) ?? property;
            _query.SelectAvg(propertyName, propertyAs);

            if (!string.IsNullOrWhiteSpace(orderBy))
            {
                _query.OrderBy(propertyName, orderBy);
            }

            var measure = new QueryMeasure() { Property = property, OrderBy = orderBy };
            _queryMeasures.Add(measure);

            return this;
        }

        public IQuery MeasureMin(string property, string? propertyAs = null, string? orderBy = null)
        {
            var propertyName = _propertyKeyMapper.GetPropertyName(_from, property) ?? property;
            _query.SelectMin(propertyName, propertyAs);

            if (!string.IsNullOrWhiteSpace(orderBy))
            {
                _query.OrderBy(propertyName, orderBy);
            }

            var measure = new QueryMeasure() { Property = property, OrderBy = orderBy };
            _queryMeasures.Add(measure);

            return this;
        }

        public IQuery MeasureMax(string property, string? propertyAs = null, string? orderBy = null)
        {
            var propertyName = _propertyKeyMapper.GetPropertyName(_from, property) ?? property;
            _query.SelectMax(propertyName, propertyAs);

            if (!string.IsNullOrWhiteSpace(orderBy))
            {
                _query.OrderBy(propertyName, orderBy);
            }

            var measure = new QueryMeasure() { Property = property, OrderBy = orderBy };
            _queryMeasures.Add(measure);

            return this;
        }

        public IQuery Dimension(string property)
        {
            var propertyName = _propertyKeyMapper.GetPropertyName(_from, property) ?? property;
            _query.Select(propertyName).GroupBy(propertyName);

            var dimension = new QueryDimension() { Property = property };
            _queryDimension.Add(dimension);

            return this;
        }
        public IQuery TimeDimension(string property)
        {
            var propertyName = _propertyKeyMapper.GetPropertyName(_from, property) ?? property;
            _query.Select(propertyName).GroupBy(propertyName);

            _queryTimeDimension = new QueryTimeDimension() { Property = property };

            return this;
        }
        public IQuery Filter(Func<IQueryFilter, IQueryFilter> filter)
        {
            filter.Invoke(_queryFilter);
            return this;
        }
        public IQuery TimeDimension(string property, string timeDimensionPart)
        {
            var propertyName = _propertyKeyMapper.GetPropertyName(_from, property) ?? property;
            switch (timeDimensionPart)
            {
                case "date":
                    _query.SelectDate(propertyName).GroupBy(e => e.Date(propertyName));
                    break;
                case "second":
                    _query.SelectSecond(propertyName).GroupBy(e => e.Second(propertyName));
                    break;
                case "minute":
                    _query.SelectMinute(propertyName).GroupBy(e => e.Minute(propertyName));
                    break;
                case "hour":
                    _query.SelectHour(propertyName).GroupBy(e => e.Hour(propertyName));
                    break;
                case "day":
                    _query.SelectDay(propertyName).GroupBy(e => e.Day(propertyName));
                    break;
                case "month":
                    _query.SelectMonth(propertyName).GroupBy(e => e.Month(propertyName));
                    break;
                case "year":
                    _query.SelectYear(propertyName).GroupBy(e => e.Year(propertyName));
                    break;
                default:
                    _query.Select(propertyName).GroupBy(propertyName);
                    break;
            }
            _queryTimeDimension = new QueryTimeDimension() { Property = property, TimeDimensionPart = timeDimensionPart };

            return this;
        }
        public IQuery OrderBy(string property, string direction)
        {
            var propertyName = _propertyKeyMapper.GetPropertyName(_from, property) ?? property;
            _query.OrderBy(propertyName, direction);
            _queryTimeDimension = new QueryTimeDimension() { Property = property };

            return this;
        }

        public IQuery Limit(int limit)
        {
            _query.Limit(limit);
            return this;
        }



        public List<Dictionary<string, string>> GetMeasures<TType>() => _propertyKeyMapper.GetAttributeProperties<TType, QueryMeasureAttribute>();
        public List<Dictionary<string, string>> GetMeasures(string queryKey) => _propertyKeyMapper.GetAttributeProperties<QueryMeasureAttribute>(queryKey);
        public List<Dictionary<string, string>> GetDimensions<TType>() => _propertyKeyMapper.GetAttributeProperties<TType, QueryDimensionAttribute>();
        public List<Dictionary<string, string>> GetDimensions(string queryKey) => _propertyKeyMapper.GetAttributeProperties<QueryDimensionAttribute>(queryKey);
        public List<Dictionary<string, string>> GetTimeDimensions<TType>() => _propertyKeyMapper.GetAttributeProperties<TType, QueryTimeDimensionAttribute>();
        public List<Dictionary<string, string>> GetTimeDimensions(string queryKey) => _propertyKeyMapper.GetAttributeProperties<QueryTimeDimensionAttribute>(queryKey);

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
