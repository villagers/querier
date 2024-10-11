using Querier.SqlQuery.Extensions;
using Querier.SqlQuery.Interfaces;

namespace Querier.SqlQuery.Models
{
    public class SqlColumn : ISqlColumn
    {
        public required string Column {  get; set; }
        public string? ColumnAs { get; set; }

        public virtual SqlQueryResult Compile(ISqlTable table)
        {
            var result = new SqlQueryResult();

            var token = table != null ? "@table.@column" : "@column";
            if (table != null)
            {
                result.NameParameters.Add("@table", table.TableOrAlias);
            }
            result.NameParameters.Add("@column", Column);
            result.SqlTokenizer
                .AddToken(e => Column == "*" ? e.AddToken("*") : e.AddToken(token), "");

            if (!string.IsNullOrEmpty(ColumnAs))
            {
                result.NameParameters.Add("@as", ColumnAs);
                result.SqlTokenizer.AddToken("as").AddToken("@as");
            }

            result.Sql = result.SqlTokenizer.Build();
            return result.Enumerate();
        }
    }
}
