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

        public override SqlQueryResult Compile()
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

            var result = new SqlQueryResult()
            {
                Sql = queryTz,
                NameParameters = compiled.NameParameters ?? new Dictionary<string, string>(),
                SqlParameters = compiled?.SqlParameters ?? new Dictionary<string, object>()
            };
            result.NameParameters = result.NameParameters.Merge(column.NameParameters);
            return result;
        }
    }
}
