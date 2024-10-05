using Querier.SqlQuery.Extensions;
using Querier.SqlQuery.Interfaces;

namespace Querier.SqlQuery.Models
{
    public class SqlSelectAggregation : ISqlSelect
    {
        public required SqlColumnAggregation SqlColumnAggregation {  get; set; }

        public SqlQueryResult Compile(ISqlTable table) => SqlColumnAggregation.Compile(table).Enumerate();
    }
}
