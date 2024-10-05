using Querier.SqlQuery.Extensions;
using Querier.SqlQuery.Interfaces;
using Querier.SqlQuery.Models;
using Querier.SqlQuery.Tokenizers;

namespace Querier.SqlQuery.Operators
{
    public class BetweenOperator : AbstractLogicalOperator
    {
        public required object Value { get; set; }
        public required object SecondValue { get; set; }

        public override SqlQueryResult Compile(ISqlTable table)
        {
            var column = Column.Compile(table);

            var sqlTz = new SqlTokenizer()
                .AddToken(AndOrOperator)
                .AddToken(column.Sql)
                .AddToken(NotOperator)
                .AddToken("between")
                .AddToken("@value")
                .AddToken("and")
                .AddToken("@secondValue")
                .Build();

            var result = new SqlQueryResult()
            {
                Sql = sqlTz,
                NameParameters = column.NameParameters,
                SqlParameters = new Dictionary<string, object>()
                {
                    { "@value", Value },
                    { "@secondValue", SecondValue }
                }
            };

            return result.Enumerate();
        }
    }
}
