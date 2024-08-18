using Querier.SqlQuery.Interfaces;
using Querier.SqlQuery.Tokenizers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querier.SqlQuery.Models
{
    public class SqlSelectQuery<TQuery> : SqlSelect where TQuery : IBaseQuery<TQuery>, new()
    {
        public required TQuery Query { get; set; }
        public string? QueryAs { get; set; }

        public override SqlQueryResult Compile()
        {
            var result = new SqlQueryResult();

            var query = Query.Compile();
            var queryTz = new SqlTokenizer().AddToken("(").AddToken(query.Sql).AddToken(")");

            result.SqlParameters = result.SqlParameters;
            result.NameParameters = result.NameParameters;
            result.Sql = queryTz.Build("");
            return result;
        }
    }
}
