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

        public virtual SqlQueryResult Compile()
        {
            var result = new SqlQueryResult();
            var selectTz = new SqlTokenizer();

            if (Column == "*")
            {
                selectTz.AddToken("*");
                result.Sql = selectTz.Build(" ");
                return result;
            }

            result.NameParameters.Add("@column", Column);
            selectTz.AddToken("@column");

            if (!string.IsNullOrEmpty(ColumnAs))
            {
                result.NameParameters.Add("@as", ColumnAs);
                selectTz.AddToken("as").AddToken("@as");
            }

            result.Sql = selectTz.Build(" ");
            result.NameParameters = result.NameParameters.Select((e, i) =>
            {
                result.Sql = result.Sql.ReplaceExact(e.Key, $"@name{i}");
                return new KeyValuePair<string, string>($"@name{i}", e.Value);
            }).ToDictionary();

            return result;
        }
    }
}
