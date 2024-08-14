using Querier.SqlQuery.Tokenizers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querier.SqlQuery.Models
{
    public class SqlSelectAggregation : SqlSelect
    {
        public required string Aggregation { get; set; }

        public override SqlQueryResult Compile()
        {
            var result = new SqlQueryResult();
            result.NameParameters.Add("@column", Column);

            var aggrTz = new SqlTokenizer()
                .AddToken(Aggregation)
                .AddToken("(").AddToken("@column").AddToken(")").Build("");

            var selectTz = new SqlTokenizer().AddToken(aggrTz);

            if (!string.IsNullOrEmpty(ColumnAs))
            {
                result.NameParameters.Add("@as", ColumnAs);
                selectTz.AddToken("as").AddToken("@as");
            }

            result.Sql = selectTz.Build();
            result.NameParameters = result.NameParameters.Select((e, i) =>
            {
                result.Sql = result.Sql.Replace(e.Key, $"@name{i}");
                return new KeyValuePair<string, string>($"@name{i}", e.Value);
            }).ToDictionary();
            return result;
        }
    }
}
