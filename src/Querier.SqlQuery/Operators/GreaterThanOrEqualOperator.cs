using Querier.SqlQuery.Extensions;
using Querier.SqlQuery.Interfaces;
using Querier.SqlQuery.Models;
using Querier.SqlQuery.Tokenizers;

namespace Querier.SqlQuery.Operators
{
    public class GreaterThanOrEqualOperator<T> : AbstractComparisonOperator<T>
    {
        public override SqlQueryResult Compile(ISqlTable table)
        {
            var column = Column.Compile(table);

            var sqlTz = new SqlTokenizer()
                .AddToken(AndOrOperator)
                .AddToken(NotOperator)
                .AddToken(column.Sql)
                .AddToken(">=")
                .AddToken("@value")
                .Build();

            var result = new SqlQueryResult()
            {
                Sql = sqlTz,
                NameParameters = column.NameParameters,
                SqlParameters = new Dictionary<string, object>()
                {
                    { "@value", Value }
                }
            };

            return result.Enumerate();
        }
    }
}
