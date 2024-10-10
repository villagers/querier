using Querier.SqlQuery.Extensions;
using Querier.SqlQuery.Interfaces;
using Querier.SqlQuery.Tokenizers;

namespace Querier.SqlQuery.Models
{
    public class SqlSelectQuery<TQuery> : ISqlSelect where TQuery : IBaseQuery<TQuery>
    {
        public required TQuery Query { get; set; }
        public string? QueryAs { get; set; }
        public string? Function { get; set; }
        public List<object> FunctionParameters { get; set; } = new List<object>();

        public SqlQueryResult Compile(ISqlTable table)
        {
            var result = Query.Compile();

            var queryTz = new SqlTokenizer()
                .AddToken(e =>
                {
                    if (!string.IsNullOrWhiteSpace(Function))
                    {
                        e.AddToken(Function);
                        e.AddToken("(");
                        e.AddToken($"({result.Sql})");
                        
                        for (var i = 0; i < FunctionParameters.Count; i++)
                        {
                            var parameter = FunctionParameters[i];
                            e.AddToken($", @{i}");
                            result.SqlParameters.Add($"@{i}", parameter);
                        }
                        e.AddToken(")");
                    } else
                    {
                        e.AddToken("(");
                        e.AddToken(result.Sql);
                        e.AddToken(")");
                    }

                    return e;
                });

            if (!string.IsNullOrWhiteSpace(QueryAs))
            {
                queryTz.AddToken("as").AddToken("@as");
                result.NameParameters.Add("@as", QueryAs);
            }

            result.Sql = queryTz.Build();
            return result.Enumerate();
        }
    }
}
