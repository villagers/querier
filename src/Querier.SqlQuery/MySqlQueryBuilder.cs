using Querier.SqlQuery.Functions;
using Querier.SqlQuery.Models;
using Querier.SqlQuery.Tokenizers;

namespace Querier.SqlQuery
{
    public class MySqlQueryBuilder : BaseQuery<IMySqlQueryBuilder>, IMySqlQueryBuilder
    {
        protected override string NameParameterOpening => "`";
        protected override string NameParameterClosing => "`";

        public MySqlQueryBuilder(IFunction function) : base(function)
        {
        }

        public override IMySqlQueryBuilder New()
        {
            return new MySqlQueryBuilder(_functionFactory);
        }

        public override SqlQueryResult CompileSql(SqlQueryResult result)
        {
            return base.CompileSql(result);
        }
        public override SqlTokenizer CompileTokens(SqlQueryResult result)
        {
            var tokens = base.CompileTokens(result);
            if (_limit.HasValue)
            {
                tokens.AddToken("limit").AddToken($"{_limit}");
            }
            return tokens;
        }
    }
}
