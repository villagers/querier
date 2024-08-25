using Querier.SqlQuery.Models;
using Querier.SqlQuery.Tokenizers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querier.SqlQuery.Operators
{
    public class LessThanOrEqualOperator<T> : AbstractComparisonOperator<T>
    {
        public override SqlOperatorResult Compile()
        {
            var column = Column.Compile();

            var sqlTz = new SqlTokenizer()
                .AddToken(AndOrOperator)
                .AddToken(NotOperator)
                .AddToken(column.Sql)
                .AddToken("<=")
                .AddToken("@value")
                .Build();

            var result = new SqlOperatorResult()
            {
                Sql = sqlTz,
                NameParameters = column.NameParameters,
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
