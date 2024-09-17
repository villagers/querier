using Querier.SqlQuery.Extensions;
using Querier.SqlQuery.Functions;
using Querier.SqlQuery.Interfaces;
using Querier.SqlQuery.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querier.SqlQuery.Models
{
    public class SqlWhereFunction : SqlWhere, ISqlQueryCompile<SqlQueryResult>
    {
        public required IFunction Function;
        public SqlWhereFunction() : base() { }

        public override SqlQueryResult Compile(ISqlTable table) => Function.Compile(table).Enumerate();
    }
}