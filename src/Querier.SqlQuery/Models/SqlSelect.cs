using Querier.SqlQuery.Extensions;
using Querier.SqlQuery.Interfaces;

namespace Querier.SqlQuery.Models
{
    public class SqlSelect : ISqlSelect
    {
        public required SqlColumn SqlColumn { get; set; }
        public virtual SqlQueryResult Compile(ISqlTable table) => SqlColumn.Compile(table).Enumerate();
    }
}
