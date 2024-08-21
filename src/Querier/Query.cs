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

        private List<QueryFilter> _queryFilters;


        private readonly TQuery _query;
        private readonly IQueryDbConnection _connection;

        public Query(TQuery query, IQueryDbConnection connection)
        {
            _query = query;
            _queryMeasures = new List<QueryMeasure>();
            _queryDimension = new List<QueryDimension>();
            _queryFilters = new List<QueryFilter>();
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
        public IQuery TimeDimension(string property, TimeDimensionPart timeDimensionPart)
        {
            _query.Select(property).GroupBy(property);
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
