using Querier.SqlQuery.Extensions;
using Querier.SqlQuery.Interfaces;
using Querier.SqlQuery.Tokenizers;

namespace Querier.SqlQuery.Models
{
    public class SqlJoinOnValue : ISqlJoinOn
    {
        public required string Table { get; set; }
        public required string Column { get; set; }
        public required object Value { get; set; }

        public SqlQueryResult Compile(ISqlTable table)
        {
            var result = new SqlQueryResult();
            result.SqlParameters.Add("@value", Value);
            result.NameParameters.Add("@column", Column);
            result.NameParameters.Add("@table", Table);

            var tz = new SqlTokenizer().AddToken(e => e.AddToken("@table").AddToken(".").AddToken("@column"), "").AddToken("=").AddToken("@value");

            result.Sql = tz.Build();
            return result.Enumerate();
        }
    }
}
