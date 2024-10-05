using Querier.SqlQuery.Extensions;
using Querier.SqlQuery.Interfaces;
using Querier.SqlQuery.Tokenizers;

namespace Querier.SqlQuery.Models
{
    public class SqlColumnAggregation : SqlColumn
    {
        public required string Aggregation { get; set; }

        public override SqlQueryResult Compile(ISqlTable table)
        {
            var result = new SqlQueryResult();
            result.NameParameters.Add("@table", table.TableOrAlias);
            result.NameParameters.Add("@column", Column);

            var aggrTz = new SqlTokenizer()
                .AddToken(Aggregation)
                .AddToken("(")
                .AddToken(e => e.AddToken("@table").AddToken(".").AddToken("@column"), "")
                .AddToken(")").Build("");

            var selectTz = new SqlTokenizer().AddToken(aggrTz);

            if (!string.IsNullOrEmpty(ColumnAs))
            {
                result.NameParameters.Add("@as", ColumnAs);
                selectTz.AddToken("as").AddToken("@as");
            }

            result.Sql = selectTz.Build();
            return result.Enumerate();
        }
    }
}
