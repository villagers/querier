using Querier.SqlQuery.Extensions;
using Querier.SqlQuery.Interfaces;

namespace Querier.SqlQuery.Models
{
    public class SqlSelectCase : ISqlSelect
    {
        public required SqlCase SqlCase { get; set; }
        public SqlQueryResult Compile(ISqlTable table) => SqlCase.Compile(table).Enumerate();
    }
}
