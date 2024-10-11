using Querier.SqlQuery.Extensions;
using Querier.SqlQuery.Functions;
using Querier.SqlQuery.Models;

namespace Querier.SqlQuery
{
    public class DuckDBQueryBuilder : BaseQuery<IDuckDBQueryBuilder>, IDuckDBQueryBuilder
    {
        protected override string NameParameterOpening => "";
        protected override string NameParameterClosing => "";
        protected override string NameParameterPlaceholder { get; set; } = "@n";
        protected override string SqlParameterPlaceholder { get; set; } = "$p";
        public DuckDBQueryBuilder(IFunction functionFactory) : base(functionFactory)
        {
        }

        public override IDuckDBQueryBuilder New()
        {
            return new DuckDBQueryBuilder(_functionFactory);
        }

        public override Dictionary<string, object> CompileSqlParameters(SqlQueryResult result)
        {
            return SqlParameters
                .Select((e, i) =>
                {
                    result.Sql = result.Sql.ReplaceExact(e.Key, $"{SqlParameterPlaceholder}{i}");
                    return new KeyValuePair<string, object>($"{SqlParameterPlaceholder}{i}", e.Value);
                }).ToDictionary();
        }

        public override SqlQueryResult CompileSql(SqlQueryResult result)
        {
            result = base.CompileSql(result);
            var sqlParameters = result.SqlParameters.ToList();
            foreach (var parameter in sqlParameters)
            {
                var item = parameter;
                result.SqlParameters.Remove(item.Key);
                result.SqlParameters.Add(item.Key.Replace(SqlParameterPlaceholder, "p"), item.Value);
            }

            return result;
        }
    }
}
