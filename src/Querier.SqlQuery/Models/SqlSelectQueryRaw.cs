using Querier.SqlQuery.Extensions;
using Querier.SqlQuery.Interfaces;
using Querier.SqlQuery.Tokenizers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querier.SqlQuery.Models
{
    public class SqlSelectQueryRaw<TQuery> : ISqlSelect where TQuery : IBaseQuery<TQuery>
    {
        public required TQuery Query { get; set; }
        public string? QueryAs { get; set; }
        public string? Function { get; set; }
        public required string RawSql { get; set; }

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
                        e.AddToken($", {RawSql}");
                        e.AddToken(")");
                    }
                    else
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
