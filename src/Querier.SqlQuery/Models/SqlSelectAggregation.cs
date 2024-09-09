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
    public class SqlSelectAggregation : ISqlSelect
    {
        public required SqlColumnAggregation SqlColumnAggregation {  get; set; }

        public SqlQueryResult Compile() => SqlColumnAggregation.Compile().Enumerate();
    }
}
