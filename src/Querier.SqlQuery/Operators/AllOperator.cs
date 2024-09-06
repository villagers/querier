using Querier.SqlQuery.Extensions;
using Querier.SqlQuery.Functions;
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
    public class AllOperator<TQuery> : AbstractLogicalOperator where TQuery : IBaseQuery<TQuery>
    {
        public required string Operator { get; set; }
        public required TQuery Query { get; set; }

        public override SqlOperatorResult Compile()
        {

            var column = Column.Compile();

            var sqlTz = new SqlTokenizer()
                .AddToken(AndOrOperator)
                .AddToken(column.Sql)
                .AddToken(Operator)
                .AddToken("all")
                .Build();

            var compiled = Query.Compile();
            var queryTz = new SqlTokenizer().AddToken(sqlTz).AddToken($"({compiled.Sql})").Build();

            var result = new SqlOperatorResult()
            {
                Sql = queryTz,
                NameParameters = compiled.NameParameters ?? new Dictionary<string, string>(),
                SqlParameters = compiled?.SqlParameters ?? new Dictionary<string, object>()
            };


            column.NameParameters = column.NameParameters.Select((e, i) =>
            {
                result.Sql = result.Sql.ReplaceExact(e.Key, $"@name{i}");
                return new KeyValuePair<string, string>($"@name{i}", e.Value);
            }).ToDictionary();
            result.NameParameters = result.NameParameters.Select((e, i) =>
            {
                result.Sql = result.Sql.ReplaceExact(e.Key, $"@name{i}");
                return new KeyValuePair<string, string>($"@name{i}", e.Value);
            }).ToDictionary();
            result.NameParameters = result.NameParameters.Merge("@name", column.NameParameters);

            return result;
        }
    }
}
