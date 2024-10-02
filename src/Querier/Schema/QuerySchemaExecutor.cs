using Dapper;
using DuckDB.NET.Data;
using Microsoft.Extensions.Logging;
using Querier.Extensions;
using Querier.Interfaces;
using System.Text;

namespace Querier.Schema
{
    public class QuerySchemaExecutor
    {
        private readonly SchemaStore _schemaStore;
        private readonly IQueryDbConnection _dbConnection;
        private readonly QuerySchemaDatabase _schemaDatabase;
        private readonly ILogger<QuerySchemaExecutor> _logger;


        public QuerySchemaExecutor(SchemaStore schemaStore, IQueryDbConnection dbConnection, QuerySchemaDatabase schemaDatabase, ILogger<QuerySchemaExecutor> logger)
        {
            _schemaStore = schemaStore;
            _dbConnection = dbConnection;
            _schemaDatabase = schemaDatabase;
            _logger = logger;
        }

        public async Task Invoke(QuerySchema schema)
        {
            try
            {
                var command = schema.SchemaCommand;

                var datasource = _schemaStore.DataSource(schema);
                using (var duckDBConnection = new DuckDBConnection(datasource))
                {
                    await duckDBConnection.OpenAsync();

                    var connection = _dbConnection.Connection;
                    var columnType = new List<Dictionary<string, string>>();

                    if (!string.IsNullOrWhiteSpace(schema.RefreshSql))
                    {
                        var result = await connection.ExecuteScalarAsync<string>(schema.RefreshSql);
                        if (!string.IsNullOrWhiteSpace(result))
                        {
                            var refreshKey = Convert.ToBase64String(Encoding.UTF8.GetBytes(result));
                            var existingRefreshKey = await duckDBConnection.ExecuteScalarAsync<string>(_schemaDatabase.TableConfigSelectSql, new { query = schema.Table.ToLowerInvariant() });
                            if (existingRefreshKey != null && refreshKey == existingRefreshKey) return;
                            await duckDBConnection.QueryAsync(_schemaDatabase.TableConfigInsertSql, new { query = schema.Table.ToLowerInvariant(), key = refreshKey });
                        }
                    }


                    using (var reader = await connection.ExecuteReaderAsync(command.Sql, command.Parameters))
                    {
                        for (var i = 0; i < reader.FieldCount; i++)
                        {
                            var columnTypeItem = new Dictionary<string, string>
                        {
                            { "name", reader.GetName(i) },
                            { "type", reader.GetDataTypeName(i) }
                        };
                            columnType.Add(columnTypeItem);
                        }
                        var tableSql = string.Join(",", columnType.Select(e => $"{e["name"]} {e["type"]}"));
                        await duckDBConnection.ExecuteAsync(string.Format(_schemaDatabase.TableCreateSql, schema.Table, tableSql));

                        using (var appender = duckDBConnection.CreateAppender(schema.Table))
                        {
                            while (reader.Read())
                            {
                                var row = appender.CreateRow();
                                for (var i = 0; i < reader.FieldCount; i++)
                                {
                                    var value = reader.GetValue(i);
                                    var type = reader.GetDataTypeName(i);
                                    row.AppendValue(value);
                                }
                                row.EndRow();
                            }
                        }
                        await duckDBConnection.CloseAsync();
                    }
                }

                _dbConnection.Connection.Close();
            }
            catch (Exception ex)
            {
                
                _logger.LogError(ex, $"Fail to run executor: {ex.Message}");
            }
        }
    }
}
