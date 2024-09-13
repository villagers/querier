using Querier.SqlQuery.Interfaces;
using Dapper;
using Querier.Interfaces;
using Querier.Attributes;
using Querier.SqlQuery.Extensions;

namespace Querier
{
    public class Query<TQuery> : IQuery where TQuery : IBaseQuery<TQuery>
    {
        private readonly List<QueryMeasure> _queryMeasures;
        private readonly List<QueryDimension> _queryDimension;
        private QueryTimeDimension _queryTimeDimension;

        private readonly List<IQuery> _combineQueries;

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

            _combineQueries = new List<IQuery>();

            _queryFilter = new QueryFilter<TQuery>(query);
            _propertyKeyMapper = propertyKeyMapper;
        }

        public IQuery New()
        {
            return new Query<TQuery>(_query.New(), _connection, _propertyKeyMapper);
        }
        public IQuery From(string table)
        {
            _from = table;
            _query.From(_propertyKeyMapper.GetTypeName(table) ?? table);
            return this;
        }

        public IQuery Measure(string property, string? propertyAs = null)
        {
            return MeasureSum(property, propertyAs);
        }

        public IQuery MeasureCount(string property, string? propertyAs = null)
        {
            var propertyName = _propertyKeyMapper.GetPropertyName(_from, property) ?? property;
            _query.SelectCount(propertyName, propertyAs ?? property);

            var measure = new QueryMeasure() { Property = property, PropertyAs = propertyAs };
            _queryMeasures.Add(measure);

            return this;
        }

        public IQuery MeasureSum(string property, string? propertyAs = null)
        {
            var propertyName = _propertyKeyMapper.GetPropertyName(_from, property) ?? property;
            _query.SelectSum(propertyName, propertyAs ?? property);

            var measure = new QueryMeasure() { Property = property, PropertyAs = propertyAs };
            _queryMeasures.Add(measure);

            return this;
        }

        public IQuery MeasureAvg(string property, string? propertyAs = null)
        {
            var propertyName = _propertyKeyMapper.GetPropertyName(_from, property) ?? property;
            _query.SelectAvg(propertyName, propertyAs ?? property);

            var measure = new QueryMeasure() { Property = property, PropertyAs = propertyAs };
            _queryMeasures.Add(measure);

            return this;
        }

        public IQuery MeasureMin(string property, string? propertyAs = null)
        {
            var propertyName = _propertyKeyMapper.GetPropertyName(_from, property) ?? property;
            _query.SelectMin(propertyName, propertyAs ?? property);

            var measure = new QueryMeasure() { Property = property, PropertyAs = propertyAs };
            _queryMeasures.Add(measure);

            return this;
        }

        public IQuery MeasureMax(string property, string? propertyAs = null)
        {
            var propertyName = _propertyKeyMapper.GetPropertyName(_from, property) ?? property;
            _query.SelectMax(propertyName, propertyAs ?? property);

            var measure = new QueryMeasure() { Property = property, PropertyAs = propertyAs };
            _queryMeasures.Add(measure);

            return this;
        }

        public IQuery Dimension(string property, string? propertyAs = null)
        {
            var propertyName = _propertyKeyMapper.GetPropertyName(_from, property) ?? property;
            _query.Select(propertyName, propertyAs ?? property).GroupBy(propertyName);

            var dimension = new QueryDimension() { Property = property, PropertyAs = propertyAs };
            _queryDimension.Add(dimension);

            return this;
        }
        public IQuery TimeDimension(string property, string? propertyAs = null)
        {
            var propertyName = _propertyKeyMapper.GetPropertyName(_from, property) ?? property;
            _query.Select(propertyName, propertyAs ?? property).GroupBy(propertyName);

            _queryTimeDimension = new QueryTimeDimension() { Property = property };

            return this;
        }
        public IQuery Filter(Func<IQueryFilter, IQueryFilter> filter)
        {
            filter.Invoke(_queryFilter);
            return this;
        }
        public IQuery TimeDimension(string property, string timeDimensionPart, string? propertyAs = null)
        {
            var propertyName = _propertyKeyMapper.GetPropertyName(_from, property) ?? property;

            var sqlColumn = propertyAs;

            switch (timeDimensionPart)
            {
                case "date":
                    _query.SelectDate(propertyName, propertyAs ?? property).GroupBy(e => e.Date(propertyName));
                    break;
                case "second":
                    _query.SelectSecond(propertyName, propertyAs ?? property).GroupBy(e => e.Second(propertyName));
                    break;
                case "minute":
                    _query.SelectMinute(propertyName, propertyAs ?? property).GroupBy(e => e.Minute(propertyName));
                    break;
                case "hour":
                    _query.SelectHour(propertyName, propertyAs ?? property).GroupBy(e => e.Hour(propertyName));
                    break;
                case "day":
                    _query.SelectDay(propertyName, propertyAs ?? property).GroupBy(e => e.Day(propertyName));
                    break;
                case "month":
                    _query.SelectMonth(propertyName, propertyAs ?? property).GroupBy(e => e.Month(propertyName));
                    break;
                case "year":
                    _query.SelectYear(propertyName, propertyAs ?? property).GroupBy(e => e.Year(propertyName));
                    break;
                default:
                    _query.Select(propertyName, propertyAs ?? property).GroupBy(propertyName);
                    break;
            }
            _queryTimeDimension = new QueryTimeDimension() { Property = property, PropertyAs = propertyAs, TimeDimensionPart = timeDimensionPart };

            return this;
        }
        public IQuery OrderBy(string property, string direction)
        {
            var propertyName = _propertyKeyMapper.GetPropertyName(_from, property) ?? property;
            _query.OrderBy(propertyName, direction);
            //_queryTimeDimension = new QueryTimeDimension() { Property = property };

            return this;
        }

        public IQuery Limit(int limit)
        {
            _query.Limit(limit);
            return this;
        }

        public IQuery Union(Func<IQuery, IQuery> query)
        {
            var newT = _query.New();
            var newQ = new Query<TQuery>(newT, _connection, _propertyKeyMapper);

            var newQuery = query.Invoke(newQ);
            _query.UnionAll(newT);
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

        public IEnumerable<Dictionary<string, object>>? Get()
        {
            var complie = _query.Compile();

            return _connection.Connection
                .Query(complie.CompiledSql, complie.SqlParameters)
                .Cast<IDictionary<string, object>>()
                .Select(e => e.ToDictionary(k => k.Key, v => v.Value));
        }

        public IEnumerable<T>? GetValues<T>(string property)
        {
            var complie = _query.Compile();
            return _connection.Connection
                .Query(complie.CompiledSql, complie.SqlParameters)
                .Cast<IDictionary<string, T>>()
                .Select(e => e.ToDictionary(k => k.Key, v => v.Value))
                .SelectMany(e => e.Values);
        }

        public IEnumerable<object>? GetValues(string property)
        {
            var complie = _query.Compile();
            return _connection.Connection
                .Query(complie.CompiledSql, complie.SqlParameters)
                .Cast<IDictionary<string, object>>()
                .Select(e => e.ToDictionary(k => k.Key, v => v.Value))
                .SelectMany(e => e.Values);
        }

        public T? GetScalar<T>()
        {
            var complie = _query.Compile();
            var result = _connection.Connection.ExecuteScalar<T>(complie.CompiledSql, complie.SqlParameters);
            if (result == null) return default;
            return result;
        }
        public async Task<T?> GetScalarAsync<T>()
        {
            var complie = _query.Compile();
            var result = await _connection.Connection.ExecuteScalarAsync<T>(complie.CompiledSql, complie.SqlParameters);
            if (result == null) return default;
            return result;
        }
        public object? GetScalar()
        {
            var complie = _query.Compile();
            var result = _connection.Connection.ExecuteScalar(complie.CompiledSql, complie.SqlParameters);
            if (result == null) return default;
            return result;
        }
        public async Task<object?> GetScalarAsync()
        {
            var complie = _query.Compile();
            var result = await _connection.Connection.ExecuteScalarAsync(complie.CompiledSql, complie.SqlParameters);
            if (result == null) return default;
            return result;
        }
        public IDictionary<string, object>? GetSingle()
        {
            var complie = _query.Compile();
            return _connection.Connection.QuerySingleOrDefault(complie.CompiledSql, complie.SqlParameters) as IDictionary<string, object>;
        }
        public async Task<IDictionary<string, object>?> GetSingleAsync()
        {
            var complie = _query.Compile();
            return await _connection.Connection.QuerySingleOrDefaultAsync(complie.CompiledSql, complie.SqlParameters) as IDictionary<string, object>;
        }

        public T? GetSingleValue<T>(string? property = null)
        {
            var result = GetSingle();
            if (result == null) return default;
            if (!string.IsNullOrWhiteSpace(property)) return (T)result[property];

            var key = _queryMeasures.FirstOrDefault()?.Property ?? _queryDimension.FirstOrDefault()?.Property ?? _queryTimeDimension.Property;
            return (T)result[key];
        }

        public async Task<T?> GetSingleValueAsync<T>(string? property = null)
        {
            var result = await GetSingleAsync();
            if (result == null) return default;
            if (!string.IsNullOrWhiteSpace(property)) return (T)result[property];

            var key = _queryMeasures.FirstOrDefault()?.Property ?? _queryDimension.FirstOrDefault()?.Property ?? _queryTimeDimension.Property;
            return (T)result[key];
        }

        public object? GetSingleValue(string? property = null)
        {
            var result = GetSingle();
            if (result == null) return default;
            if (!string.IsNullOrWhiteSpace(property)) return result[property];

            var key = _queryMeasures.FirstOrDefault()?.Property ?? _queryDimension.FirstOrDefault()?.Property ?? _queryTimeDimension.Property;
            return result[key];
        }

        public async Task<object?> GetSingleValueAsync(string? property = null)
        {
            var result = await GetSingleAsync();
            if (result == null) return default;
            if (!string.IsNullOrWhiteSpace(property)) return result[property];

            var key = _queryMeasures.FirstOrDefault()?.Property ?? _queryDimension.FirstOrDefault()?.Property ?? _queryTimeDimension.Property;
            return result[key];
        }

        public IDictionary<string, object>? GetFirst()
        {
            var complie = _query.Compile();
            return _connection.Connection.QueryFirstOrDefault(complie.CompiledSql, complie.SqlParameters) as IDictionary<string, object>;
        }

        public async Task<IDictionary<string, object>?> GetFirstAsync()
        {
            var complie = _query.Compile();
            return await _connection.Connection.QueryFirstOrDefaultAsync(complie.CompiledSql, complie.SqlParameters) as IDictionary<string, object>;
        }

        public T? GetFirstValue<T>(string? property = null)
        {
            var result = GetFirst();
            if (result == null) return default;
            if (!string.IsNullOrWhiteSpace(property)) return (T)result[property];

            var key = _queryMeasures.FirstOrDefault()?.SqlColumn ?? _queryDimension.FirstOrDefault()?.SqlColumn ?? _queryTimeDimension.SqlColumn;
            return (T)result[key];
        }

        public async Task<T?> GetFirstValueAsync<T>(string? property = null)
        {
            var result = await GetFirstAsync();
            if (result == null) return default;
            var ttt = result[property].GetType();
            if (!string.IsNullOrWhiteSpace(property)) return (T)(object)result[property];

            var key = _queryMeasures.FirstOrDefault()?.SqlColumn ?? _queryDimension.FirstOrDefault()?.SqlColumn ?? _queryTimeDimension.SqlColumn;
            return (T)result[key];
        }

        public object? GetFirstValue(string? property = null)
        {
            var result = GetFirst();
            if (result == null) return default;
            if (!string.IsNullOrWhiteSpace(property)) return result[property];

            var key = _queryMeasures.FirstOrDefault()?.SqlColumn ?? _queryDimension.FirstOrDefault()?.SqlColumn ?? _queryTimeDimension.SqlColumn;
            return result[key];
        }

        public async Task<object?> GetFirstValueAsync(string? property = null)
        {
            var result = await GetFirstAsync();
            if (result == null) return default;
            if (!string.IsNullOrWhiteSpace(property)) return result[property];

            var key = _queryMeasures.FirstOrDefault()?.Property ?? _queryDimension.FirstOrDefault()?.Property ?? _queryTimeDimension.Property;
            return result[key];
        }
    }
}
