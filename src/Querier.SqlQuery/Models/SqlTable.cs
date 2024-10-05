using Querier.SqlQuery.Extensions;
using Querier.SqlQuery.Interfaces;
using Querier.SqlQuery.Tokenizers;

namespace Querier.SqlQuery.Models
{
    public class SqlTable<TQuery> : ISqlTable
    {
        public required string Table { get; set; }
        public string? TableAs { get; set; }
        public string TableOrAlias => TableAs ?? Table;

        public virtual SqlQueryResult Compile(ISqlTable table)
        {
            var result = new SqlQueryResult();
            result.NameParameters.Add("@table", Table);

            var selectTz = new SqlTokenizer().AddToken("@table");

            if (!string.IsNullOrEmpty(TableAs))
            {
                result.NameParameters.Add("@as", TableAs);
                selectTz.AddToken("as").AddToken("@as");
            }

            result.Sql = selectTz.Build(" ");
            return result.Enumerate();
        }
    }
}
