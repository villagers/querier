using Querier.SqlQuery.Extensions;
using Querier.SqlQuery.Functions;
using Querier.SqlQuery.Interfaces;
using Querier.SqlQuery.Tokenizers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querier.SqlQuery.Models
{
    public class SqlColumn : ISqlColumn
    {
        public required string Column {  get; set; }
        public string? ColumnAs { get; set; }

        public virtual SqlQueryResult Compile(ISqlTable table)
        {
            var result = new SqlQueryResult();

            result.NameParameters.Add("@table", table.TableOrAlias);
            result.NameParameters.Add("@column", Column);
            result.SqlTokenizer
                .AddToken(e => Column == "*" ? e.AddToken("*") : e.AddToken("@table").AddToken(".").AddToken("@column"), "");

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
