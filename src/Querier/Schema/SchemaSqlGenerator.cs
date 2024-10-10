using Querier.Interfaces;
using Querier.SqlQuery.Interfaces;

namespace Querier.Schema
{
    public class SchemaSqlGenerator<TQuery> : ISchemaSqlGenerator where TQuery : IBaseQuery<TQuery>
    {
        private readonly TQuery _query;
        private readonly SchemaStore _schemaStore;

        public SchemaSqlGenerator(TQuery query, SchemaStore schemaStore)
        {
            _query = query;
            _schemaStore = schemaStore;
        }

        public void Generate()
        {
            foreach (var schema in _schemaStore.Schemas)
            {
                var sqlQuery = _query.New();

                if (!string.IsNullOrWhiteSpace(schema.Sql))
                {
                    sqlQuery.FromRaw(schema.Sql);
                }
                else
                {
                    sqlQuery.From(schema.Table, schema.Alias);
                }

                if (schema.Joins.Any())
                {
                    foreach (var joins in schema.Joins)
                    {
                        sqlQuery.Join(joins.JoinColumn, joins.JoinRefTable, joins.JoinRefColumn);
                    }
                    
                }

                var columnIndex = 1;
                foreach (var measure in schema.Measures)
                {
                    if (!string.IsNullOrWhiteSpace(measure.Sql))
                    {
                        sqlQuery.SelectRaw(measure.Sql, measure.Alias ?? $"c{columnIndex}");
                    }
                    else
                    {
                        if (string.IsNullOrWhiteSpace(measure.Aggregation))
                        {
                            sqlQuery.Select(measure.Column, measure.Alias ?? measure.Column);
                        }
                        else
                        {
                            sqlQuery.Select(measure.Aggregation, measure.Column, measure.Alias ?? measure.Column);
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(measure.Order))
                    {
                        sqlQuery.OrderBy(columnIndex, measure.Order);
                    }
                    columnIndex++;
                }

                foreach (var dimension in schema.Dimensions)
                {
                    if (!string.IsNullOrWhiteSpace(dimension.Sql))
                    {
                        sqlQuery.SelectRaw(dimension.Sql, dimension.Alias ?? $"c{columnIndex}").GroupBy(columnIndex);
                    }
                    else
                    {
                        sqlQuery.Select(dimension.Column, dimension.Alias ?? dimension.Column).GroupBy(columnIndex);
                    }

                    if (!string.IsNullOrWhiteSpace(dimension.Order))
                    {
                        sqlQuery.OrderBy(columnIndex, dimension.Order);
                    }
                    columnIndex++;
                }

                foreach (var timeDimension in schema.TimeDimensions)
                {
                    if (!string.IsNullOrWhiteSpace(timeDimension.Sql))
                    {
                        sqlQuery.SelectRaw(timeDimension.Sql, timeDimension.Alias ?? $"c{columnIndex}").GroupBy(columnIndex);
                    }
                    else
                    {
                        switch (timeDimension.Granularity)
                        {
                            case "second":
                                sqlQuery.SelectSecond(timeDimension.Column, timeDimension.Alias ?? timeDimension.Column);
                                break;
                            case "minute":
                                sqlQuery.SelectMinute(timeDimension.Column, timeDimension.Alias ?? timeDimension.Column);
                                break;
                            case "hour":
                                sqlQuery.SelectHour(timeDimension.Column, timeDimension.Alias ?? timeDimension.Column);
                                break;
                            case "day":
                                sqlQuery.SelectDay(timeDimension.Column, timeDimension.Alias ?? timeDimension.Column);
                                break;
                            case "month":
                                sqlQuery.SelectMonth(timeDimension.Column, timeDimension.Alias ?? timeDimension.Column);
                                break;
                            case "year":
                                sqlQuery.SelectYear(timeDimension.Column, timeDimension.Alias ?? timeDimension.Column);
                                break;
                            default:
                                sqlQuery.Select(timeDimension.Column, timeDimension.Alias ?? timeDimension.Column);
                                break;
                        }
                        sqlQuery.GroupBy(columnIndex);
                    }

                    if (!string.IsNullOrWhiteSpace(timeDimension.Order))
                    {
                        sqlQuery.OrderBy(columnIndex, timeDimension.Order);
                    }
                    columnIndex++;
                }

                var compiled = sqlQuery.Compile();
                schema.SchemaCommand = new SchemaQueryCommand() { Sql = compiled.CompiledSql, Parameters = compiled.SqlParameters };
            }
        }

        public void GenerateFillMissingDates()
        {
            foreach (var schema in _schemaStore.Schemas)
            {
                var query = _query.New();

                var table = schema.Table;
                var timeDimensionColumn = schema.TimeDimensions.FirstOrDefault()?.Column;

                var fromDateStr = "|fromDate|";
                var toDateStr = "|toDate|";

                var measures = schema.Measures.Select(e => e.Column).ToList();
                var dimensions = schema.Dimensions.Select(e => e.Column).ToList();

                query
                    .AppendRaw(
                     $"with recursive StartDate as (select coalesce((select max({timeDimensionColumn}) FROM {table} WHERE {timeDimensionColumn} <= '{fromDateStr}'), '{fromDateStr}'::DATE) AS date),")
                    .AppendRaw(
                        "DateRange as (" +
                        "select StartDate.date from StartDate union all " +
                        $"select date_add(DateRange.date, interval 1 day) from DateRange where DateRange.date < '{toDateStr}'),")
                    .AppendRaw(
                        "CartesianProduct as (" +
                        $"select date, {string.Join(", ", dimensions)} from DateRange");


                query.AppendRaw("|crossjoins|");
                query.AppendRaw("),");

                var cartesionColumns = dimensions.Select(e => $"CartesianProduct.{e}");
                var cartecianMetricColumns = measures.Select(e => $"{table}.{e}");
                var lastValueMetricColumns = measures.Select(e =>
                    $"last_value({table}.{e} IGNORE NULLS) OVER (ORDER BY {string.Join(", ", cartesionColumns)}, CartesianProduct.date) AS previous_{e}");

                query.AppendRaw(
                        "FinancialWithDates AS (" +
                        $"SELECT CartesianProduct.date, {string.Join(", ", cartesionColumns)}, {string.Join($", ", cartecianMetricColumns)}, {table}.{timeDimensionColumn}," +
                        $"{string.Join(", ", lastValueMetricColumns)} " +
                        $"FROM CartesianProduct");

                var leftJoinOperators = dimensions.Select(e => $"{table}.{e} = CartesianProduct.{e}");

                query.AppendRaw(
                        $"LEFT JOIN {table} ON strftime({table}.{timeDimensionColumn}, '%Y-%m-%d') = strftime(CartesianProduct.date, '%Y-%m-%d') " +
                        $"AND {string.Join(" and ", leftJoinOperators)}) ");

                var selectDimensions = dimensions.Select(e => $"FinancialWithDates.{e}");
                var selectMeasures = measures.Select(e => $"FinancialWithDates.previous_{e}, COALESCE({e}, previous_{e}) AS {e}");
                var orderByDimensions = dimensions.Select(e => $"FinancialWithDates.{e} asc");
                query.AppendRaw(
                    "select " +
                    "FinancialWithDates.date," +
                    $"{string.Join(", ", selectDimensions)}," +
                    $"{string.Join(", ", selectMeasures)} " +
                    $"from FinancialWithDates as FinancialWithDates " +
                    $"where FinancialWithDates.date >= '{fromDateStr}' " +
                    $"and FinancialWithDates.date <= '{toDateStr}' " +
                    $"order by FinancialWithDates.date desc, {string.Join(", ", orderByDimensions)}");

                var compiled = query.Compile();
                query.CompileFull(compiled);

                schema.SchemaCommandFillMissingDates = new SchemaQueryCommand() { Sql = compiled.CompiledSql, Parameters = compiled.SqlParameters };
            }
        }
    }
}
