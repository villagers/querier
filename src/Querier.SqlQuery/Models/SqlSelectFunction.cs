using Querier.SqlQuery.Extensions;
using Querier.SqlQuery.Functions;
using Querier.SqlQuery.Interfaces;

namespace Querier.SqlQuery.Models
{
    public class SqlSelectFunction : ISqlSelect
    {
        public required IFunction Function { get; set; }
        public string? FunctionAs { get; set; }

        public SqlQueryResult Compile(ISqlTable table)
        {
            var result = new SqlQueryResult();

            var compliledFunction = Function.Compile(table);
            result.NameParameters = compliledFunction.NameParameters;
            result.Sql = compliledFunction.Sql;
            return result.Enumerate();
        }
    }
}
