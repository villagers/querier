using Querier.SqlQuery.Extensions;
using Querier.SqlQuery.Interfaces;
using Querier.SqlQuery.Tokenizers;

namespace Querier.SqlQuery.Models
{
    public class SqlCte<TQuery> : ISqlQueryCompile<SqlQueryResult> where TQuery : IBaseQuery<TQuery>
    {
        public required string Name { get; set; }
        public required TQuery? Query { get; set; }
        public required bool Recursive { get; set; }

        public SqlQueryResult Compile(ISqlTable table)
        {
            var result = Query?.Compile();
            var queryTz = new SqlTokenizer()
                .AddToken("@name")
                .AddToken("as")
                .AddToken(e => e.AddToken("(").AddToken(result.Sql).AddToken(")"), "")
                .Build();

            result?.NameParameters.Add("@name", Name);
            result.Sql = queryTz;
            return result.Enumerate();
        }
    }
}
