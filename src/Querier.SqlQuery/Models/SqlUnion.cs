using Querier.SqlQuery.Extensions;
using Querier.SqlQuery.Interfaces;
using Querier.SqlQuery.Tokenizers;

namespace Querier.SqlQuery.Models
{
    public class SqlUnion<TQuery> : ISqlQueryCompile<SqlQueryResult> where TQuery : IBaseQuery<TQuery>
    {
        public required TQuery Query { get; set; }
        public required bool All { get; set; } = false;

        public SqlQueryResult Compile(ISqlTable table)
        {
            var compiledQuery = Query.Compile();

            var tz = new SqlTokenizer()
                .AddToken(All ? "union all" : "union")
                .AddToken(compiledQuery.Sql)
                .Build();

            var result = new SqlQueryResult()
            {
                Sql = tz,
                NameParameters = compiledQuery.NameParameters,
                SqlParameters = compiledQuery.SqlParameters,
                
            };
            return result.Enumerate();
        }
    }
}
