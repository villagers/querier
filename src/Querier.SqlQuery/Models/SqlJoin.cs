using Querier.SqlQuery.Extensions;
using Querier.SqlQuery.Interfaces;
using Querier.SqlQuery.Tokenizers;

namespace Querier.SqlQuery.Models
{
    public class SqlJoin : ISqlQueryCompile<SqlQueryResult>
    {
        public required string RefenreceTable {  get; set; }
        public required string RefenreceColumn { get; set; }
        public required string Column { get; set; }

        public SqlQueryResult Compile(ISqlTable table)
        {
            var result = new SqlQueryResult();
            result.NameParameters.Add("@refTable", RefenreceTable);
            result.NameParameters.Add("@refColumn", RefenreceColumn);
            result.NameParameters.Add("@column", Column);
            result.NameParameters.Add("@table", table.TableOrAlias);

            var tz = new SqlTokenizer()
                .AddToken("inner join")
                .AddToken("@refTable")
                .AddToken("on")
                .AddToken(e => e.AddToken("@refTable").AddToken(".").AddToken("@refColumn"), "")
                .AddToken("=")
                .AddToken(e => e.AddToken("@table").AddToken(".").AddToken("@column"), "");

            result.Sql = tz.Build();
            return result.Enumerate();
        }
    }
}
