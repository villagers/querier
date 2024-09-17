using Querier.SqlQuery.Extensions;
using Querier.SqlQuery.Functions;
using Querier.SqlQuery.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querier.SqlQuery.Models
{
    public class SqlColumnFunction<TQuery> : ISqlColumnFunction where TQuery : IBaseQuery<TQuery>
    {
        public required TQuery Query { get; set; }
        public required IFunction Function { get; set; }
        public SqlQueryResult Compile(ISqlTable table) => Function.Compile(table).Enumerate();
    }
}
