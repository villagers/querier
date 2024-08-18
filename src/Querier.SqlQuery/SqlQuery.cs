using Querier.SqlQuery.Extensions;
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
    public class SqlQuery : BaseQuery<SqlQuery>, IBaseQuery<SqlQuery>, ISqlQuery
    {
        public override SqlQueryResult Compile()
        {
            var result = base.Compile();

            foreach (var param in SqlParameters)
            {
                result.CompiledSql = result.CompiledSql.Replace(param.Key, param.Value.ToString());
            }
            
            return result;
        }
    }
}
