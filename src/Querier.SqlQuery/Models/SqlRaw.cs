using Querier.SqlQuery.Interfaces;
using Querier.SqlQuery.Tokenizers;

namespace Querier.SqlQuery.Models
{
    public class SqlRaw : ISqlRaw
    {
        public required string RawSql { get; set; }

        public SqlQueryResult Compile(ISqlTable table)
        {
            var tz = new SqlTokenizer(RawSql);
            var result = new SqlQueryResult()
            {
                Sql = tz.Build()
            };
            return result;
        }
    }
}
