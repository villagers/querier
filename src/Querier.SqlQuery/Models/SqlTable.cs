using Querier.SqlQuery.Interfaces;
using Querier.SqlQuery.Tokenizers;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querier.SqlQuery.Models
{
    public class SqlTable<TQuery> : ISqlQueryCompile<SqlQueryResult>
    {
        public string? Table { get; set; }
        public string? TableAs { get; set; }

        public TQuery? Query { get; set; }

        public virtual SqlQueryResult Compile()
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
            result.NameParameters = result.NameParameters.Select((e, i) =>
            {
                result.Sql = result.Sql.Replace(e.Key, $"@name{i}");
                return new KeyValuePair<string, string>($"@name{i}", e.Value);
            }).ToDictionary();
            return result;
        }
    }
}
