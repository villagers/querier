using Dapper;
using Querier.Interfaces;
using Querier.Schema;
using Querier.SqlQuery;
using DuckDB.NET.Data;
using Querier.SqlQuery.Models;
using Querier.Options;

namespace Querier
{
    public class Query : IQuery
    {
        private readonly List<QueryMeasure> _queryMeasures;
        private readonly List<QueryDimension> _queryDimension;
        private QueryTimeDimension _queryTimeDimension;

        private readonly SchemaStore _schemaStore;
        private readonly IQueryFilter _queryFilter;
        private readonly IQueryDbConnection _dbConnection;
        private readonly IDuckDBQueryBuilder _duckDbQueryBuilder;

        private string _table;
        private int _numColumns = 0;

        private SchemaQueryCommand? _queryOverrideCommand;
        private SqlQueryResult SqlResult { get; set; }
        private QueryFiller _queryFiller;
        public Query(IDuckDBQueryBuilder duckDbQueryBuilder, IQueryDbConnection dbConnection, SchemaStore schemaStore)
        {
            _schemaStore = schemaStore;
            _duckDbQueryBuilder = duckDbQueryBuilder;
            _dbConnection = dbConnection;

            _queryMeasures = new List<QueryMeasure>();
            _queryDimension = new List<QueryDimension>();
            _queryFilter = new QueryFilter(duckDbQueryBuilder);
            _queryFiller = new QueryFiller(duckDbQueryBuilder);
        }

        public IQuery New()
        {
            return new Query(_duckDbQueryBuilder.New(), _dbConnection, _schemaStore);
        }
        public IQuery From(string table)
        {
            _table = table;
            _numColumns = 0;

            var schema = _schemaStore.Schemas.FirstOrDefault(e => e.Key == table);
            _queryFiller.Schema = schema;

            _duckDbQueryBuilder.From(schema?.Table ?? table);
            return this;
        }

        public IQuery Measure(string property, string? propertyAs = null)
        {
            _numColumns++;
            var table = _schemaStore.Schemas.First(e => e.Key == _table);
            var tableMeasure = table.Measures.Where(e => e.Key == property).FirstOrDefault();

            _duckDbQueryBuilder.Select(tableMeasure?.Column ?? property, propertyAs ?? property).GroupBy(_numColumns);

            AddMeasure(property, propertyAs);

            return this;
        }

        public IQuery MeasureCount(string property, string? propertyAs = null)
        {
            _numColumns++;
            var table = _schemaStore.Schemas.First(e => e.Key == _table);
            var tableMeasure = table.Measures.Where(e => e.Key == property).FirstOrDefault();

            _duckDbQueryBuilder.SelectCount(tableMeasure?.Column ?? property, propertyAs ?? property);

            AddMeasure(property, propertyAs);

            return this;
        }

        public IQuery MeasureSum(string property, string? propertyAs = null)
        {
            _numColumns++;
            var table = _schemaStore.Schemas.First(e => e.Key == _table);
            var tableMeasure = table.Measures.Where(e => e.Key == property).FirstOrDefault();

            _duckDbQueryBuilder.SelectSum(tableMeasure?.Column ?? property, propertyAs ?? property);

            AddMeasure(property, propertyAs);

            return this;
        }

        public IQuery MeasureAvg(string property, string? propertyAs = null)
        {
            _numColumns++;
            var table = _schemaStore.Schemas.First(e => e.Key == _table);
            var tableMeasure = table.Measures.Where(e => e.Key == property).FirstOrDefault();

            _duckDbQueryBuilder.SelectAvg(tableMeasure?.Column ?? property, propertyAs ?? property);

            AddMeasure(property, propertyAs);

            return this;
        }

        public IQuery MeasureMin(string property, string? propertyAs = null)
        {
            _numColumns++;
            var table = _schemaStore.Schemas.First(e => e.Key == _table);
            var tableMeasure = table.Measures.Where(e => e.Key == property).FirstOrDefault();

            _duckDbQueryBuilder.SelectMin(tableMeasure?.Column ?? property, propertyAs ?? property);

            AddMeasure(property, propertyAs);

            return this;
        }

        public IQuery MeasureMax(string property, string? propertyAs = null)
        {
            _numColumns++;
            var table = _schemaStore.Schemas.First(e => e.Key == _table);
            var tableMeasure = table.Measures.Where(e => e.Key == property).FirstOrDefault();

            _duckDbQueryBuilder.SelectMax(tableMeasure?.Column ?? property, propertyAs ?? property);

            AddMeasure(property, propertyAs);

            return this;
        }

        public IQuery Dimension(string property, string? propertyAs = null)
        {
            _numColumns++;
            var table = _schemaStore.Schemas.First(e => e.Key == _table);
            var tableDimension = table.Dimensions.Where(e => e.Key == property).FirstOrDefault();

            _duckDbQueryBuilder.Select(tableDimension?.Column ?? property, propertyAs ?? property).GroupBy(_numColumns);

            var dimension = new QueryDimension() { Property = property, PropertyAs = propertyAs };
            _queryDimension.Add(dimension);

            _queryFiller.QueryDimensions.Add(dimension);

            return this;
        }
        public IQuery TimeDimension(string property, string? propertyAs = null)
        {
            _numColumns++;
            var table = _schemaStore.Schemas.First(e => e.Key == _table);
            var tableDimension = table.Dimensions.Where(e => e.Key == property).FirstOrDefault();

            _duckDbQueryBuilder.Select(tableDimension?.Column ?? property, propertyAs ?? property).GroupBy(_numColumns);

            _queryTimeDimension = new QueryTimeDimension() { Property = property };

            _queryFiller.QueryTimeDimension = _queryTimeDimension;

            return this;
        }

        private void AddMeasure(string property, string? propertyAs = null)
        {
            var measure = new QueryMeasure() { Property = property, PropertyAs = propertyAs };

            _queryMeasures.Add(measure);
            _queryFiller.QueryMeasures.Add(measure);
        }

        public IQuery FilterRaw(string sql)
        {
            _duckDbQueryBuilder.WhereRaw(sql);
            return this;
        }
        public IQuery Filter(string column, Func<IQueryFilter, IQueryFilter> filter)
        {
            var newFilter = new QueryFilter(_duckDbQueryBuilder, column);
            filter.Invoke(newFilter);

            _queryFiller.AddFilter(column, filter);
            return this;

        }
        public IQuery TimeDimension(string property, string timeDimensionPart, string? propertyAs = null)
        {
            _numColumns++;
            var table = _schemaStore.Schemas.First(e => e.Key == _table);
            var tableTimeDimension = table.Dimensions.Where(e => e.Key == property).FirstOrDefault();

            var sqlColumn = propertyAs;

            switch (timeDimensionPart)
            {
                case "date":
                    _duckDbQueryBuilder.SelectDate(tableTimeDimension?.Column ?? property, propertyAs ?? property).GroupBy(_numColumns);
                    break;
                case "second":
                    _duckDbQueryBuilder.SelectSecond(tableTimeDimension?.Column ?? property, propertyAs ?? property).GroupBy(_numColumns);
                    break;
                case "minute":
                    _duckDbQueryBuilder.SelectMinute(tableTimeDimension?.Column ?? property, propertyAs ?? property).GroupBy(_numColumns);
                    break;
                case "hour":
                    _duckDbQueryBuilder.SelectHour(tableTimeDimension?.Column ?? property, propertyAs ?? property).GroupBy(_numColumns);
                    break;
                case "day":
                    _duckDbQueryBuilder.SelectDay(tableTimeDimension?.Column ?? property, propertyAs ?? property).GroupBy(_numColumns);
                    break;
                case "month":
                    _duckDbQueryBuilder.SelectMonth(tableTimeDimension?.Column ?? property, propertyAs ?? property).GroupBy(_numColumns);
                    break;
                case "year":
                    _duckDbQueryBuilder.SelectYear(tableTimeDimension?.Column ?? property, propertyAs ?? property).GroupBy(_numColumns);
                    break;
                default:
                    _duckDbQueryBuilder.Select(tableTimeDimension?.Column ?? property, propertyAs ?? property).GroupBy(_numColumns);
                    break;
            }
            _queryTimeDimension = new QueryTimeDimension() { Property = property, PropertyAs = propertyAs, TimeDimensionPart = timeDimensionPart };

            _queryFiller.QueryTimeDimension = _queryTimeDimension;

            return this;
        }
        public IQuery OrderBy(string property, string direction)
        {
            var table = _schemaStore.Schemas.First(e => e.Key == _table);
            var tableProperty =
                table.Measures.FirstOrDefault(e => e.Key == property)?.Column ??
                table.Dimensions.FirstOrDefault(e => e.Key == property)?.Column ??
                table.TimeDimensions.FirstOrDefault(e => e.Key == property)?.Column ??
                property;

            _duckDbQueryBuilder.OrderBy(tableProperty, direction);

            _queryFiller.AddOrderBy(tableProperty, direction);

            return this;
        }
        public IQuery Limit(int limit)
        {
            _duckDbQueryBuilder.Limit(limit);
            return this;
        }
        public IQuery Union(Func<IQuery, IQuery> query)
        {
            var newT = _duckDbQueryBuilder.New();
            var newQ = new Query(newT, _dbConnection, _schemaStore);

            var newQuery = query.Invoke(newQ);
            _duckDbQueryBuilder.UnionAll(newT);
            return this;
        }

        public IQuery FillMissingDates(DateTime fromDate, DateTime toDate, FillMissingOption? options = null)
        {
            var schema = _schemaStore.Schemas.First(e => e.Key == _table);
            _queryFiller.Build(fromDate, toDate, options);

            return this;
        }

        public HashSet<QueryMeasureSchema> GetMeasures<TType>() => _schemaStore.Schemas.FirstOrDefault(e => e.Type == typeof(TType))?.Measures ?? [];
        public HashSet<QueryMeasureSchema> GetMeasures(string queryKey) => _schemaStore.Schemas.FirstOrDefault(e => e.Key == queryKey)?.Measures ?? [];
        public HashSet<QueryDimensionSchema> GetDimensions<TType>() => _schemaStore.Schemas.FirstOrDefault(e => e.Type == typeof(TType))?.Dimensions ?? [];
        public HashSet<QueryDimensionSchema> GetDimensions(string queryKey) => _schemaStore.Schemas.FirstOrDefault(e => e.Key == queryKey)?.Dimensions ?? [];
        public HashSet<QueryTimeDimensionSchema> GetTimeDimensions<TType>() => _schemaStore.Schemas.FirstOrDefault(e => e.Type == typeof(TType))?.TimeDimensions ?? [];
        public HashSet<QueryTimeDimensionSchema> GetTimeDimensions(string queryKey) => _schemaStore.Schemas.FirstOrDefault(e => e.Key == queryKey)?.TimeDimensions ?? [];

        private SqlQueryResult PostCompile(SqlQueryResult result) => _duckDbQueryBuilder.PostCompile(result);
        private SchemaQueryCommand Compile()
        {
            if (_queryFiller != null)
            {
                SqlResult = PostCompile(_queryFiller.Result);
            } else
            {
                SqlResult = _duckDbQueryBuilder.Compile();
                SqlResult = PostCompile(SqlResult);
            }
            return new SchemaQueryCommand() { Sql = SqlResult.CompiledSql, Parameters = SqlResult.SqlParameters };
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

            var compiled = Compile();

            var schema = _schemaStore.Schemas.First(e => e.Key == _table);
            var datasource = _schemaStore.DataSource(schema);
            using (var duckDBConnection = new DuckDBConnection(datasource))
            {
                duckDBConnection.Open();
                result.Data = duckDBConnection
                    .Query(compiled.Sql, compiled.Parameters)
                    .Cast<IDictionary<string, object>>()
                    .Select(e => e.ToDictionary(k => k.Key, v => v.Value));
            }
            return result;
        }

        public IEnumerable<Dictionary<string, object>>? Get()
        {
            var complie = _duckDbQueryBuilder.Compile();

            return _dbConnection.Connection()
                .Query(complie.CompiledSql, complie.SqlParameters)
                .Cast<IDictionary<string, object>>()
                .Select(e => e.ToDictionary(k => k.Key, v => v.Value));
        }
    }
}
