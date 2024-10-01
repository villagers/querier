using Coravel.Invocable;
using Microsoft.Extensions.Logging;
using Querier.Schema;

namespace Querier.Schedules
{
    public class QuerySchemaScheduler : IInvocable
    {
        private readonly QuerySchema _schema;
        private readonly QuerySchemaExecutor _schemaExecutor;
        private readonly ILogger<QuerySchemaScheduler> _logger;

        public QuerySchemaScheduler(QuerySchemaExecutor schemaExecutor, QuerySchema schema, ILogger<QuerySchemaScheduler> logger)
        {
            _schema = schema;
            _schemaExecutor = schemaExecutor;
            _logger = logger;
        }

        public async Task Invoke()
        {
            try
            {
                await _schemaExecutor.Invoke(_schema);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Fail to execute scheduler: {ex.Message}");
            }  
        }
    }
}
