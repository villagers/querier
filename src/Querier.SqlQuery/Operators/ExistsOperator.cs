using Querier.SqlQuery.Interfaces;
using Querier.SqlQuery.Models;
using Querier.SqlQuery.Tokenizers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querier.SqlQuery.Operators
{
    public class ExistsOperator<TQuery> : AbstractLogicalOperator where TQuery : IQuery<TQuery>, new()
    {
        public required string Column { get; set; }
        public required TQuery Query { get; set; }

        public override SqlOperatorResult Compile()
        {
            var sqlTz = new SqlTokenizer()
                .AddToken(AndOrOperator)
                .AddToken(NotOperator)
                .AddToken("exists")
                .AddToken("@value")
                .Build();

            var rawSql = Query.Compile().Sql;
            var result = new SqlOperatorResult()
            {
                Sql = sqlTz,
                SqlParameters = new Dictionary<string, object>()
                {
                    { "@value", rawSql }
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
