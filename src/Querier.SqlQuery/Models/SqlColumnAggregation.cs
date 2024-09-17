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
