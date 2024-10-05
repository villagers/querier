using Querier.SqlQuery.Extensions;
using Querier.SqlQuery.Interfaces;
using Querier.SqlQuery.Models;
using Querier.SqlQuery.Tokenizers;

namespace Querier.SqlQuery.Operators
{
    public class NotEqualRawOperator : AbstractComparisonOperator<string>
    {
        public required string RawSql { get; set; }
        public override SqlQueryResult Compile(ISqlTable table)
        {
            var column = Column.Compile(table);

            var sqlTz = new SqlTokenizer()
                .AddToken(AndOrOperator)
                .AddToken(column.Sql)
                .AddToken("!=")
                .AddToken(RawSql)
                .Build();

            var result = new SqlQueryResult()
            {
                Sql = sqlTz,
                NameParameters = column.NameParameters,
                SqlParameters = new Dictionary<string, object>()
            };
            return result.Enumerate();
        }
    }
}
