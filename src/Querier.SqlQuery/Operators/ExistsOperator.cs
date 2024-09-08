using Querier.SqlQuery.Extensions;
using Querier.SqlQuery.Functions;
using Querier.SqlQuery.Interfaces;
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
    public class ExistsOperator<TQuery> : AbstractLogicalOperator where TQuery : IBaseQuery<TQuery>
    {
        public required TQuery Query { get; set; }

        public override SqlQueryResult Compile()
        {
            var sqlTz = new SqlTokenizer()
                .AddToken(AndOrOperator)
                .AddToken(NotOperator)
                .AddToken("exists")
                .Build();

            var compiled = Query.Compile();
            var queryTz = new SqlTokenizer().AddToken(sqlTz).AddToken($"({compiled.Sql})").Build();

            var result = new SqlQueryResult()
            {
                Sql = queryTz,
                NameParameters = compiled.NameParameters ?? new Dictionary<string, string>(),
                SqlParameters = compiled?.SqlParameters ?? new Dictionary<string, object>()
            };
            return result.Enumerate();
        }
    }
}
