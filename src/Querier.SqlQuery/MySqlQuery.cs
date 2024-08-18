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
    public class MySqlQuery : BaseQuery<MySqlQuery>, IBaseQuery<MySqlQuery>, IMySqlQuery
    {
        protected override string NameParameterOpening => "`";
        protected override string NameParameterClosing => "`";
        public MySqlQuery()
        {
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
