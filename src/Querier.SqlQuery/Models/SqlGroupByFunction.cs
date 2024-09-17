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
    public class SqlGroupByFunction : SqlGroupBy
    {
        public required IFunction Function { get; set; }

        public override SqlQueryResult Compile(ISqlTable table) => Function.Compile(table).Enumerate();
    }
}
