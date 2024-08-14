using Querier.SqlQuery.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querier.SqlQuery.Providers
{
    public class DefaultProvider<TQuery> : AbstractProvider<TQuery>, IDefaultProvider where TQuery : IQuery<TQuery>, new()
    {
        public override string Compile(TQuery query)
        {
            var result = query.Compile();

            foreach (var param in result.NameParameters)
            {
                result.Sql = result.Sql.Replace(param.Value, param.Key);
            }
            return result.Sql;
        }
    }
}
