using Coravel.Invocable;
using Querier.Schema;

namespace Querier.Schedules
{
    public class QuerySchemaScheduler : IInvocable
    {
        private readonly QuerySchema _schema;
        private readonly QuerySchemaExecutor _executor;

        public QuerySchemaScheduler(QuerySchemaExecutor executor, QuerySchema schema)
        {
            _schema = schema;
            _executor = executor;
        }

        public async Task Invoke()
        {
            await _executor.Invoke(_schema);
        }
    }      
}
