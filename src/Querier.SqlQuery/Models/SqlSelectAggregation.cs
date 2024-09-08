using Querier.SqlQuery.Extensions;
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
        public required SqlColumnAggregation SqlColumnAggregation {  get; set; }

        public override SqlQueryResult Compile() => SqlColumnAggregation.Compile().Enumerate();
    }
}
