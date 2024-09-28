using Coravel.Invocable;
using Dapper;
using DuckDB.NET.Data;
using Querier.Interfaces;
using Querier.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querier.Schedules
{
    public class QuerySchemaInitiator
    {
        private readonly SchemaStore _schemaStore;
        private readonly IQueryDbConnection _dbConnection;

        public QuerySchemaInitiator(SchemaStore schemaStore, IQueryDbConnection dbConnection)
        {
            _schemaStore = schemaStore;
            _dbConnection = dbConnection;
        }

        public async Task InitiateAsync()
        {
            var schemas = _schemaStore.Schemas;
            foreach (var schema in schemas)
            {
                var command = schema.SchemaCommand;

                if (!string.IsNullOrWhiteSpace(_schemaStore.LocalStoragePath))
                {
                    if (!Directory.Exists(_schemaStore.LocalStoragePath))
                    {
                        Directory.CreateDirectory(_schemaStore.LocalStoragePath);
                    }
                }

                var datasource = _schemaStore.DataSource(schema);
                using (var duckDBConnection = new DuckDBConnection(datasource))
                {
                    await duckDBConnection.OpenAsync();

                    var connection = _dbConnection.Connection;
                    var columnType = new List<Dictionary<string, string>>();
                    var columnNames = new List<string>();

                    schema.SchemaCommandDuckDbConfigTable = string.Format(schema.SchemaCommandDuckDbConfigTable, schema.Table.ToLowerInvariant());
                    await duckDBConnection.ExecuteAsync(schema.SchemaCommandDuckDbConfigTable);

                    await duckDBConnection.CloseAsync();
                }
            }
        }
    }
}
