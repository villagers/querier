using Querier.SqlQuery.Extensions;
using Querier.SqlQuery.Models;
using Querier.SqlQuery.Tokenizers;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querier.SqlQuery.Operators
{
    public class BetweenOperator : AbstractLogicalOperator
    {
        public required object Value { get; set; }
        public required object SecondValue { get; set; }

        public override SqlOperatorResult Compile()
        {
            var column = Column.Compile();

            var sqlTz = new SqlTokenizer()
                .AddToken(AndOrOperator)
                .AddToken(column.Sql)
                .AddToken(NotOperator)
                .AddToken("between")
                .AddToken("@value")
                .AddToken("and")
                .AddToken("@secondValue")
                .Build();

            var result = new SqlOperatorResult()
            {
                Sql = sqlTz,
                NameParameters = column.NameParameters,
                SqlParameters = new Dictionary<string, object>()
                {
                    { "@value", Value },
                    { "@secondValue", SecondValue }
                }
            };

            result.NameParameters = result.NameParameters.Select((e, i) =>
            {
                result.Sql = result.Sql.ReplaceExact(e.Key, $"@name{i}");
                return new KeyValuePair<string, string>($"@name{i}", e.Value);
            }).ToDictionary();
            return result;
        }
    }
}
