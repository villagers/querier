using Microsoft.Extensions.Logging;
using Querier.Interfaces;

namespace Querier.Schema
{
    public class QuerySchemaInitiator
    {
        private readonly SchemaStore _schemaStore;
        private readonly IQueryDbConnection _dbConnection;
        private readonly QuerySchemaDatabase _schemaDatabase;
        private readonly ILogger<QuerySchemaInitiator> _logger;

        public QuerySchemaInitiator(ILogger<QuerySchemaInitiator> logger, SchemaStore schemaStore, IQueryDbConnection dbConnection, QuerySchemaDatabase schemaDatabase)
        {
            _logger = logger;
            _schemaStore = schemaStore;
            _dbConnection = dbConnection;
            _schemaDatabase = schemaDatabase;
        }

        public async Task InitiateAsync()
        {
            var schemas = _schemaStore.Schemas;
            foreach (var schema in schemas)
            {
                var directory = CreateDirectory();
                var dbConfig = await CreateDatabaseConfigTableAsync(schema);

                if (!directory || !dbConfig)
                {
                    _logger.LogError($"Failed to initialize database for '{schema.Key}'");
                    return;
                }

                schema.Initialized = true;
            }
        }

        public bool CreateDirectory()
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(_schemaStore.LocalStoragePath))
                {
                    if (!Directory.Exists(_schemaStore.LocalStoragePath))
                    {
                        Directory.CreateDirectory(_schemaStore.LocalStoragePath);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to create directory for '{_schemaStore.LocalStoragePath}': {ex.Message}");
                return false;
            }
        }

        public async Task<bool> CreateDatabaseConfigTableAsync(QuerySchema schema)
        {
            try
            {
                await _schemaDatabase.CreateTableConfig(schema);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to create database table 'config': {ex.Message}");
                return false;
            }

        }
    }
}
