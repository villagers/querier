using Querier.Interfaces;
using Querier.Options;
using Querier.Schema;
using Querier.SqlQuery;
using Querier.SqlQuery.Models;

namespace Querier
{
    public class QueryFiller
    {
        public SqlQueryResult Result { get; set; }
        public QuerySchema Schema { get; set; }
        public List<QueryMeasure> QueryMeasures { get; set; }
        public List<QueryDimension> QueryDimensions { get; set; }
        public QueryTimeDimension QueryTimeDimension { get; set; }
        

        private readonly IDuckDBQueryBuilder _builder;
        private readonly Dictionary<string, string> _orderBys;
        private readonly Dictionary<string, List<Func<IQueryFilter, IQueryFilter>>> _filters;

        public QueryFiller(IDuckDBQueryBuilder builder)
        {
            _builder = builder;
            QueryMeasures = new List<QueryMeasure>();
            QueryDimensions = new List<QueryDimension>();
            _orderBys = new Dictionary<string, string>();
            _filters = new Dictionary<string, List<Func<IQueryFilter, IQueryFilter>>>();
        }

        public void AddOrderBy(string column, string direction)
        {
            if (!_orderBys.ContainsKey(column))
            {
                _orderBys.Add(column, direction);
                return;
            }
            _orderBys[column] = direction;
        }
        public void AddFilter(string column, Func<IQueryFilter, IQueryFilter> filter)
        {
            if (!_filters.TryGetValue(column, out List<Func<IQueryFilter, IQueryFilter>>? value))
            {
                value = new List<Func<IQueryFilter, IQueryFilter>>();
                _filters.Add(column, value);
            }
            value.Add(filter);
        }

        public void Build(DateTime fromDate, DateTime toDate, FillMissingOption? options = null)
        {
            if (options == null) options = new FillMissingOption();

            var table = Schema.Table;

            var timeDimension = QueryTimeDimension;
            var timeDimensionProperty = timeDimension.Property;

            var measures = QueryMeasures.ToList();
            var dimension = QueryDimensions.ToList();

            var fromDateStr = fromDate.ToString("yyyy-MM-dd");
            var toDateStr = toDate.ToString("yyyy-MM-dd");

            var measureProperties = measures.Select(e => e.Property).ToList();
            var dimensionProperties = dimension.Select(e => e.Property).ToList();

            var query = _builder.New();

            // CTE StartDate
            query.WithRecursive("StartDate", e => e.SelectCoalesceRaw(e => e.SelectMax(timeDimensionProperty).From(table).WhereRaw($"{timeDimensionProperty} <= cast('{fromDateStr}' as date)"), $"cast('{fromDateStr}' as date)", "date"));

            // CTE DateRange
            query.With("DateRange", e => e.Select("date").From("StartDate").UnionAll(e => e.SelectRaw($"date_add(DateRange.date, interval {options.Interval} {options.Unit})").From("DateRange").WhereRaw($"date < cast('{toDateStr}' as date)")));

            // CTE Pairs
            //var pairCommand = columnValues.Select(e => $"{e.Key} in ({string.Join(",", e.Value)})");

            var queryFilter = new QueryFilter(_builder.New());
            foreach (var filter in _filters)
            {
                if (!dimension.Any(e => e.Property == filter.Key)) continue;
                queryFilter.Column(filter.Key);
                foreach (var func in filter.Value)
                {
                    func.Invoke(queryFilter);
                }
            }

            var pairCommand = queryFilter.GetQuery();
            foreach (var column in dimensionProperties)
            {
                pairCommand.Select(column);
            }
            pairCommand.Distinct().From(table);
            //pairCommand.SelectRaw(string.Join(",", pairKeys)).Distinct().From(table.Table);

            query.With("Pairs", e => pairCommand);
            //query.With("Pairs", e => e.SelectRaw(string.Join(",", columns)).Distinct().From(table.Table).WhereRaw(string.Join(" and ", pairCommand)));

            // CTE Cartesian
            query.With("Cartesian", e => e.Select("date").SelectRaw(string.Join(", ", dimensionProperties)).From("DateRange").CrossJoin("Pairs"));

            var cartesionColumns = dimensionProperties.Select(e => $"Cartesian.{e}");
            var cartecianMetricColumns = measureProperties.Select(e => $"{table}.{e}");
            var lastValueColumns = dimensionProperties.Select(e =>
                $"last_value({table}.{e} IGNORE NULLS) OVER (ORDER BY {string.Join(", ", cartesionColumns)}, Cartesian.date) AS previous_{e}");
            var lastValueMetricColumns = measureProperties.Select(e =>
                $"last_value({table}.{e} IGNORE NULLS) OVER (ORDER BY {string.Join(", ", cartesionColumns)}, Cartesian.date) AS previous_{e}");
            var leftJoinOperators = dimensionProperties.Select(e => $"{table}.{e} = Cartesian.{e}");

            // CTE Cartesian Table
            var cartesianTable = query.New().Select("date")
                .SelectRaw(string.Join(",", cartesionColumns))
                .SelectRaw(string.Join(",", cartecianMetricColumns))
                .SelectRaw(string.Join(",", lastValueColumns))
                .SelectRaw(string.Join(",", lastValueMetricColumns))
                .SelectRaw($"{table}.{timeDimensionProperty}")
                .From("Cartesian")
                .JoinRaw(table, $"left join {table} on strftime({table}.{timeDimensionProperty}, '%Y-%m-%d') = strftime(Cartesian.date, '%Y-%m-%d') ");
            foreach (var column in dimensionProperties)
            {
                cartesianTable.AndOn(column, column);
            }

            // CTE Mixed Data
            query.With("MixedData", e => cartesianTable);

            // Main Query
            var selectDimensions = dimensionProperties.Select(e => $"COALESCE({e}, previous_{e}) AS {e}");
            var selectMeasures = measureProperties.Select(e => $"COALESCE({e}, previous_{e}) AS {e}");
            var orderByDimensions = dimensionProperties.Select(e => $"MixedData.{e}");

            query
                .Select("date", timeDimensionProperty)
                .SelectRaw(string.Join(", ", selectDimensions))
                .SelectRaw(string.Join(", ", selectMeasures))
                .From("MixedData")
                .WhereRaw($"date >= cast('{fromDateStr}' as date)")
                .WhereRaw($"date <= cast('{toDateStr}' as date)")
                .OrderBy("date", "desc");

            foreach (var orderBy in _orderBys)
            {
                query.OrderBy(orderBy.Key, orderBy.Value);
            }

            Result = query.Compile();
        }
    }
}
