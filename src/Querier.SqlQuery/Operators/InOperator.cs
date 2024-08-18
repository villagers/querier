using Querier.SqlQuery.Models;
using Querier.SqlQuery.Tokenizers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querier.SqlQuery.Operators
{
    public class InOperator : AbstractLogicalOperator
    {
        public required string Column { get; set; }
        public required object Value { get; set; }

        public override SqlOperatorResult Compile()
        {
            var sqlTz = new SqlTokenizer()
                .AddToken(AndOrOperator)
                .AddToken("@column")
                .AddToken(NotOperator)
                .AddToken("in")
                .AddToken("@value")
                .Build();

            var result = new SqlOperatorResult()
            {
                Sql = sqlTz,
                NameParameters = new Dictionary<string, string>() { { "@column", Column } },
                SqlParameters = new Dictionary<string, object>()
                {
                    { "@value", Value }
                }
            };
            result.NameParameters = result.NameParameters.Select((e, i) =>
            {
                result.Sql = result.Sql.Replace(e.Key, $"@name{i}");
                return new KeyValuePair<string, string>($"@name{i}", e.Value);
            }).ToDictionary();
            return result;
        }
    }
}
