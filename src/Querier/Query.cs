using Dapper;
using Querier.Interfaces;
using Querier.Schema;
using Querier.SqlQuery;
using DuckDB.NET.Data;
using Querier.SqlQuery.Models;

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
        private bool _fillMissingDates = false;
        private string? _fillMissingDatesCommand;

        private SqlQueryResult SqlResult { get; set; }

        public Query(IDuckDBQueryBuilder duckDbQueryBuilder, IQueryDbConnection dbConnection, SchemaStore schemaStore)
        {
            _schemaStore = schemaStore;
            _duckDbQueryBuilder = duckDbQueryBuilder;
            _dbConnection = dbConnection;

            _queryMeasures = new List<QueryMeasure>();
            _queryDimension = new List<QueryDimension>();
            _queryFilter = new QueryFilter(duckDbQueryBuilder);
        }

        public IQuery New()
        {
            return new Query(_duckDbQueryBuilder.New(), _dbConnection, _schemaStore);
        }
        public IQuery From(string table)
        {
            _table = table;
            _numColumns = 0;

            var tableKey = _schemaStore.Schemas.FirstOrDefault(e => e.Key == table);

            _duckDbQueryBuilder.From(tableKey?.Table ?? table);
            return this;
        }

        public IQuery Measure(string property, string? propertyAs = null)
        {
            _numColumns++;
            var table = _schemaStore.Schemas.First(e => e.Key == _table);
            var tableMeasure = table.Measures.Where(e => e.Key == property).FirstOrDefault();

            _duckDbQueryBuilder.Select(tableMeasure?.Column ?? property, propertyAs ?? property).GroupBy(_numColumns);

            var measure = new QueryMeasure() { Property = property, PropertyAs = propertyAs };
            _queryMeasures.Add(measure);

            return this;
        }

        public IQuery MeasureCount(string property, string? propertyAs = null)
        {
            _numColumns++;
            var table = _schemaStore.Schemas.First(e => e.Key == _table);
            var tableMeasure = table.Measures.Where(e => e.Key == property).FirstOrDefault();

            _duckDbQueryBuilder.SelectCount(tableMeasure?.Column ?? property, propertyAs ?? property);

            var measure = new QueryMeasure() { Property = property, PropertyAs = propertyAs };
            _queryMeasures.Add(measure);

            return this;
        }

        public IQuery MeasureSum(string property, string? propertyAs = null)
        {
            _numColumns++;
            var table = _schemaStore.Schemas.First(e => e.Key == _table);
            var tableMeasure = table.Measures.Where(e => e.Key == property).FirstOrDefault();

            _duckDbQueryBuilder.SelectSum(tableMeasure?.Column ?? property, propertyAs ?? property);

            var measure = new QueryMeasure() { Property = property, PropertyAs = propertyAs };
            _queryMeasures.Add(measure);

            return this;
        }

        public IQuery MeasureAvg(string property, string? propertyAs = null)
        {
            _numColumns++;
            var table = _schemaStore.Schemas.First(e => e.Key == _table);
            var tableMeasure = table.Measures.Where(e => e.Key == property).FirstOrDefault();

            _duckDbQueryBuilder.SelectAvg(tableMeasure?.Column ?? property, propertyAs ?? property);

            var measure = new QueryMeasure() { Property = property, PropertyAs = propertyAs };
            _queryMeasures.Add(measure);

            return this;
        }

        public IQuery MeasureMin(string property, string? propertyAs = null)
        {
            _numColumns++;
            var table = _schemaStore.Schemas.First(e => e.Key == _table);
            var tableMeasure = table.Measures.Where(e => e.Key == property).FirstOrDefault();

            _duckDbQueryBuilder.SelectMin(tableMeasure?.Column ?? property, propertyAs ?? property);

            var measure = new QueryMeasure() { Property = property, PropertyAs = propertyAs };
            _queryMeasures.Add(measure);

            return this;
        }

        public IQuery MeasureMax(string property, string? propertyAs = null)
        {
            _numColumns++;
            var table = _schemaStore.Schemas.First(e => e.Key == _table);
            var tableMeasure = table.Measures.Where(e => e.Key == property).FirstOrDefault();

            _duckDbQueryBuilder.SelectMax(tableMeasure?.Column ?? property, propertyAs ?? property);

            var measure = new QueryMeasure() { Property = property, PropertyAs = propertyAs };
            _queryMeasures.Add(measure);

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

            return this;
        }
        public IQuery TimeDimension(string property, string? propertyAs = null)
        {
            _numColumns++;
            var table = _schemaStore.Schemas.First(e => e.Key == _table);
            var tableDimension = table.Dimensions.Where(e => e.Key == property).FirstOrDefault();

            _duckDbQueryBuilder.Select(tableDimension?.Column ?? property, propertyAs ?? property).GroupBy(_numColumns);

            _queryTimeDimension = new QueryTimeDimension() { Property = property };

            return this;
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

        public IQuery FillMissingDates(DateTime fromDate, DateTime toDate, Dictionary<string, List<object>> columnValues)
        {
            _fillMissingDates = true;

            var table = _schemaStore.Schemas.First(e => e.Key == _table);

            var timeD = _queryTimeDimension.Property;
            var fromDateStr = fromDate.ToString("yyyy-MM-dd");
            var toDateStr = toDate.ToString("yyyy-MM-dd");

            var columns = _queryDimension.Select(e => e.Property).ToList();
            var metricColumns = _queryMeasures.Select(e => e.Property).ToList();

            var newQuery = _duckDbQueryBuilder.New()
                .AppendRaw(
                 $"with recursive StartDate as (select coalesce((select max({timeD}) FROM {table.Table} WHERE {timeD} <= '{fromDateStr}'), '{fromDateStr}'::DATE) AS date),")
                .AppendRaw(
                    "DateRange as (" +
                    "select StartDate.date from StartDate union all " +
                    $"select date_add(DateRange.date, interval 1 day) from DateRange where DateRange.date < '{toDateStr}'),")
                .AppendRaw(
                    "Pairs AS (" +
                    $"SELECT DISTINCT {string.Join(",", columns)} " +
                    $"FROM {table.Table} where");

            var pairCommand = columnValues.Select(e => $"{e.Key} in ({string.Join(",", e.Value)})");
            newQuery.AppendRaw(string.Join(" and ", pairCommand));
            newQuery.AppendRaw("),")
                .AppendRaw(
                    "CartesianProduct as (" +
                    $"select date, {string.Join(", ", columns)} from DateRange cross join Pairs),");

            var cartesionColumns = columns.Select(e => $"CartesianProduct.{e}");
            var cartecianMetricColumns = metricColumns.Select(e => $"{table.Table}.{e}");

            var lastValueColumns = columns.Select(e =>
                $"last_value({table.Table}.{e} IGNORE NULLS) OVER (ORDER BY {string.Join(", ", cartesionColumns)}, CartesianProduct.date) AS previous_{e}");
            var lastValueMetricColumns = metricColumns.Select(e =>
                $"last_value({table.Table}.{e} IGNORE NULLS) OVER (ORDER BY {string.Join(", ", cartesionColumns)}, CartesianProduct.date) AS previous_{e}");

            newQuery.AppendRaw(
                    "FinancialWithDates AS (" +
                    $"SELECT " +
                    $"CartesianProduct.date," +
                    $"{string.Join(",", cartesionColumns)}," +
                    $"{string.Join(",", cartecianMetricColumns)}," +
                    $"{string.Join(", ", lastValueColumns)}," +
                    $"{string.Join(", ", lastValueMetricColumns)}," +
                    $"{table.Table}.{timeD} " +
                    $"FROM CartesianProduct");

            var leftJoinOperators = columns.Select(e => $"{table.Table}.{e} = CartesianProduct.{e}");

            newQuery.AppendRaw(
                    $"LEFT JOIN {table.Table} ON strftime({table.Table}.{timeD}, '%Y-%m-%d') = strftime(CartesianProduct.date, '%Y-%m-%d') " +
                    $"AND {string.Join(" and ", leftJoinOperators)}) ");

            var selectDimensions = columns.Select(e => $"FinancialWithDates.previous_{e}, COALESCE({e}, previous_{e}) AS {e}");
            var selectMeasures = metricColumns.Select(e => $"FinancialWithDates.previous_{e}, COALESCE({e}, previous_{e}) AS {e}");
            var orderByDimensions = columns.Select(e => $"FinancialWithDates.{e} asc");
            newQuery.AppendRaw(
                "select " +
                $"FinancialWithDates.date as {timeD}," +
                $"{string.Join(", ", selectDimensions)}," +
                $"{string.Join(", ", selectMeasures)} " +
                $"from FinancialWithDates as FinancialWithDates " +
                $"where FinancialWithDates.date >= '{fromDateStr}' " +
                $"and FinancialWithDates.date <= '{toDateStr}' " +
                $"order by FinancialWithDates.date desc, {string.Join(", ", orderByDimensions)}");

            var qCompile = newQuery.Compile();
            newQuery.CompileFull(qCompile);


            _fillMissingDatesCommand = qCompile.CompiledSql;

            return this;
        }

        public HashSet<QueryMeasureSchema> GetMeasures<TType>() => _schemaStore.Schemas.FirstOrDefault(e => e.Type == typeof(TType))?.Measures ?? [];
        public HashSet<QueryMeasureSchema> GetMeasures(string queryKey) => _schemaStore.Schemas.FirstOrDefault(e => e.Key == queryKey)?.Measures ?? [];
        public HashSet<QueryDimensionSchema> GetDimensions<TType>() => _schemaStore.Schemas.FirstOrDefault(e => e.Type == typeof(TType))?.Dimensions ?? [];
        public HashSet<QueryDimensionSchema> GetDimensions(string queryKey) => _schemaStore.Schemas.FirstOrDefault(e => e.Key == queryKey)?.Dimensions ?? [];
        public HashSet<QueryTimeDimensionSchema> GetTimeDimensions<TType>() => _schemaStore.Schemas.FirstOrDefault(e => e.Type == typeof(TType))?.TimeDimensions ?? [];
        public HashSet<QueryTimeDimensionSchema> GetTimeDimensions(string queryKey) => _schemaStore.Schemas.FirstOrDefault(e => e.Key == queryKey)?.TimeDimensions ?? [];

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


            SqlResult = _duckDbQueryBuilder.Compile();
            var schema = _schemaStore.Schemas.First(e => e.Key == _table);

            _duckDbQueryBuilder.CompileFull(SqlResult);

            var query = _fillMissingDates ? _fillMissingDatesCommand : SqlResult.CompiledSql;
            var queryParameters = _fillMissingDates ? [] : SqlResult.SqlParameters;

            var datasource = _schemaStore.DataSource(schema);
            using (var duckDBConnection = new DuckDBConnection(datasource))
            {
                duckDBConnection.Open();
                result.Data = duckDBConnection
                    .Query(query, queryParameters)
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
