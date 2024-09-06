using Querier.SqlQuery.Functions;
using Querier.SqlQuery.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querier.SqlQuery.Models
{
    public class SqlColumnFunction : ISqlColumn
    {
        public required IFunction Function { get; set; }
        public SqlQueryResult Compile() => Function.Compile();
    }
}
