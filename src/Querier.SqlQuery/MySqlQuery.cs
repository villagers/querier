using Querier.SqlQuery.Functions;
using Querier.SqlQuery.Interfaces;
using Querier.SqlQuery.Models;
using Querier.SqlQuery.Tokenizers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querier.SqlQuery
{
    public class MySqlQuery : BaseQuery<IMySqlQuery>, IMySqlQuery
    {
        protected override string NameParameterOpening => "`";
        protected override string NameParameterClosing => "`";

        public MySqlQuery(IFunction function) : base(function)
        {
        }

        public override IMySqlQuery New()
        {
            return new MySqlQuery(_functionFactory);
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

        public IMySqlQuery TestMethod()
        {
            return this;
        }
    }
}
