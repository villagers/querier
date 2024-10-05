using Querier.SqlQuery.Interfaces;

namespace Querier.SqlQuery.Models
{
    public class SqlGroupByRaw : ISqlGroupBy
    {
        public required string RawSql { get; set; }
        public SqlQueryResult Compile(ISqlTable table)
        {
            return new SqlQueryResult()
            {
                Sql = RawSql
            };
        }
    }
}
