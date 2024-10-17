using Querier.SqlQuery.Extensions;
using Querier.SqlQuery.Functions;
using Querier.SqlQuery.Models;

namespace Querier.SqlQuery
{
    public class DuckDBQueryBuilder : BaseQuery<IDuckDBQueryBuilder>, IDuckDBQueryBuilder
    {
        protected override string NameParameterOpening => "";
        protected override string NameParameterClosing => "";
        protected override string SqlParameterPlaceholder { get; set; } = "$p";

        
        public DuckDBQueryBuilder(IFunction functionFactory) : base(functionFactory)
        {
        }

        public override IDuckDBQueryBuilder New()
        {
            return new DuckDBQueryBuilder(_functionFactory);
        }

        public override SqlQueryResult PostCompile(SqlQueryResult result)
        {
            result.CompiledSql = result.CompiledSql.Replace("$", "$");
            foreach (var param in result.SqlParameters.ToList())
            {
                var item = param;
                result.SqlParameters.Remove(item.Key);
                result.SqlParameters.Add(item.Key.Replace("$", ""), item.Value);
            }
            return result;
        }
    }
}
