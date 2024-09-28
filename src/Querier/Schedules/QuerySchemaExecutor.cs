using Coravel.Invocable;
using Dapper;
using DuckDB.NET.Data;
using Google.Protobuf.WellKnownTypes;
using Querier.Attributes;
using Querier.Extensions;
using Querier.Interfaces;
using Querier.Schema;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Numerics;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Querier.Schedules
{
    public class QuerySchemaExecutor : IInvocable
    {
        public required QuerySchema Payload { get; set; }

        private readonly SchemaStore _schemaStore;
        private readonly IQueryDbConnection _dbConnection;

        public QuerySchemaExecutor(SchemaStore schemaStore, IQueryDbConnection dbConnection, QuerySchema payload)
        {
            Payload = payload;

            _schemaStore = schemaStore;
            _dbConnection = dbConnection;
        }

        public async Task Invoke()
        {
            var command = Payload.SchemaCommand;

            var datasource = _schemaStore.DataSource(Payload);
            using (var duckDBConnection = new DuckDBConnection(datasource))
            {
                await duckDBConnection.OpenAsync();

                var connection = _dbConnection.Connection;
                var columnType = new List<Dictionary<string, string>>();
                var columnNames = new List<string>();

                if (!string.IsNullOrWhiteSpace(Payload.RefreshSql))
                {
                    var result = await connection.ExecuteScalarAsync(Payload.RefreshSql);
                    var refreshKey = Convert.ToBase64String(Encoding.UTF8.GetBytes(result.ToString()));
                    var existingRefreshKey = await duckDBConnection.ExecuteScalarAsync<string>($"SELECT refresh_key FROM config WHERE query = $query", new { query = Payload.Table.ToLowerInvariant() });

                    if (existingRefreshKey != null && refreshKey == existingRefreshKey)
                    {
                        return;
                    }
                    var insert = $"INSERT INTO config VALUES ($query, $key)";
                    await duckDBConnection.QueryAsync(insert, new { query = Payload.Table.ToLowerInvariant(), key = refreshKey });


                }


                using (var reader = await connection.ExecuteReaderAsync(command.Sql, command.Parameters))
                {
                    for (var i = 0; i < reader.FieldCount; i++)
                    {
                        columnNames.Add(reader.GetName(i));
                        var columnTypeItem = new Dictionary<string, string>
                        {
                            { "name", reader.GetName(i) },
                            { "type", reader.GetDataTypeName(i) }
                        };
                        columnType.Add(columnTypeItem);
                    }
                    var tableSql = string.Join(",", columnType.Select(e => $"{e["name"]} {e["type"]}"));
                    Payload.SchemaCommandDuckDbTable = string.Format(Payload.SchemaCommandDuckDbTable, Payload.Table, tableSql);
                    await duckDBConnection.ExecuteAsync(Payload.SchemaCommandDuckDbTable);



                    using (var appender = duckDBConnection.CreateAppender(Payload.Table))
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
        }
    }
}
