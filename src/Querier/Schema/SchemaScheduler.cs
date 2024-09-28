using Coravel.Scheduling.Schedule.Interfaces;
using Querier.Schedules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querier.Schema
{
    public class SchemaScheduler
    {
        private readonly IScheduler _scheduler;
        private readonly SchemaStore _schemaStore;

        public SchemaScheduler(IScheduler scheduler, SchemaStore schemaStore)
        {
            _scheduler = scheduler;
            _schemaStore = schemaStore;
        }

        public void Schedule()
        {
            var query = _schemaStore.Schemas.FirstOrDefault();
            _scheduler.ScheduleWithParams<QuerySchemaExecutor>(query).EverySeconds(10);
        }
    }
}
