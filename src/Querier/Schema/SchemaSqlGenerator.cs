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
    }
}
