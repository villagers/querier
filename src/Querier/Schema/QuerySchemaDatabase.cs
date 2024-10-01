using Dapper;
using DuckDB.NET.Data;
using MySqlX.XDevAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querier.Schema
{
    public class QuerySchemaDatabase
    {
        public string TableConfig = "config";
        public string TableConfigInsertSql = "INSERT INTO config VALUES ($query, $key)";
        public string TableConfigSelectSql = "SELECT refresh_key FROM config WHERE query = $query";

        public string TableCreateSql = "DROP TABLE IF EXISTS {0}; CREATE TABLE {0} ({1})";

        private readonly SchemaStore _schemaStore;

        public QuerySchemaDatabase(SchemaStore schemaStore)
        {
            _schemaStore = schemaStore;
        }

        public async Task CreateTableConfig(QuerySchema schema)
        {
            var datasource = _schemaStore.DataSource(schema);
            using (var duckDBConnection = new DuckDBConnection(datasource))
            {
                await duckDBConnection.OpenAsync();
                await duckDBConnection.ExecuteAsync("DROP TABLE IF EXISTS config; CREATE TABLE config (query VARCHAR, refresh_key VARCHAR)");
                await duckDBConnection.CloseAsync();
            }
        }
    }
}
