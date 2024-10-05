using Querier.SqlQuery.Extensions;
using Querier.SqlQuery.Interfaces;
using Querier.SqlQuery.Tokenizers;

namespace Querier.SqlQuery.Models
{
    public class SqlTableQuery<TQuery> : ISqlTable where TQuery : IBaseQuery<TQuery>
    {
        public required TQuery? Query { get; set; }
        public required string TableAs { get; set; }
        public string TableOrAlias => TableAs;

        public SqlQueryResult Compile(ISqlTable table)
        {
            var result = Query?.Compile();
            var queryTz = new SqlTokenizer().AddToken("(").AddToken(result?.Sql).AddToken(")").Build("");
            var tableTz = new SqlTokenizer().AddToken(queryTz);

            if (!string.IsNullOrEmpty(TableAs))
            {
                result.NameParameters.Add("@as", TableAs);
                tableTz.AddToken("as").AddToken("@as");
            }

            result.Sql = tableTz.Build();
            return result.Enumerate();
        }
    }
}
