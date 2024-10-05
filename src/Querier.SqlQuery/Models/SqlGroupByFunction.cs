using Querier.SqlQuery.Extensions;
using Querier.SqlQuery.Functions;
using Querier.SqlQuery.Interfaces;

namespace Querier.SqlQuery.Models
{
    public class SqlGroupByFunction : SqlGroupBy
    {
        public required IFunction Function { get; set; }

        public override SqlQueryResult Compile(ISqlTable table) => Function.Compile(table).Enumerate();
    }
}
