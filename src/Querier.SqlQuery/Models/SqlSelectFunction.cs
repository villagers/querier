using Querier.SqlQuery.Extensions;
using Querier.SqlQuery.Functions;
using Querier.SqlQuery.Tokenizers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querier.SqlQuery.Models
{
    public class SqlSelectFunction : SqlSelect
    {
        public required IFunction Function { get; set; }
        public string? FunctionAs { get; set; }

        public override SqlQueryResult Compile()
        {
            var result = new SqlQueryResult();

            var compliledFunction = Function.Compile();
            result.NameParameters = compliledFunction.NameParameters;
            result.Sql = compliledFunction.Sql;
            return result.Enumerate();
        }
    }
}
