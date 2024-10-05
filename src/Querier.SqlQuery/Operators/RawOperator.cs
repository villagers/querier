using Querier.SqlQuery.Interfaces;
using Querier.SqlQuery.Models;
using Querier.SqlQuery.Tokenizers;

namespace Querier.SqlQuery.Operators
{
    public class RawOperator : AbstractOperator
    {
        public required string RawSql { get; set; }
        public override SqlQueryResult Compile(ISqlTable table)
        {
            var sqlTz = new SqlTokenizer()
                .AddToken(AndOrOperator)
                .AddToken(RawSql)
                .Build();
            var result = new SqlQueryResult()
            {
                Sql = sqlTz,
            };
            return result;
        }
    }
}