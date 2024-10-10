using Querier.SqlQuery.Extensions;
using Querier.SqlQuery.Interfaces;
using Querier.SqlQuery.Tokenizers;

namespace Querier.SqlQuery.Models
{
    public class SqlJoinOn : ISqlJoinOn
    {
        public required string Column { get; set; }
        public required string ReferenceTable { get; set; }
        public required string ReferenceColumn { get; set; }

        public SqlQueryResult Compile(ISqlTable table)
        {
            var result = new SqlQueryResult();
            result.NameParameters.Add("@refTable", ReferenceTable);
            result.NameParameters.Add("@column", Column);
            result.NameParameters.Add("@table", table.TableOrAlias);

            var tz = new SqlTokenizer();
            result.NameParameters.Add("@refColumn", ReferenceColumn);

            tz.AddToken(e => e.AddToken("@table").AddToken(".").AddToken("@column"), "")
                .AddToken("=")
                .AddToken(e => e.AddToken("@refTable").AddToken(".").AddToken("@refColumn"), "");

            result.Sql = tz.Build();
            return result.Enumerate();
        }
    }
}
