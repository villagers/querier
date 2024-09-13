using Querier.SqlQuery.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querier.SqlQuery.Interfaces
{
    public interface IBaseQuery<TQuery> : IFromSqlQuery<TQuery>, ISelectSqlQuery<TQuery>, IWhereSqlQuery<TQuery>, IGroupSqlQuery<TQuery>, IUnionSqlQuery<TQuery>, IOrderSqlQuery<TQuery> where TQuery : IBaseQuery<TQuery>
    {
        TQuery New();
        SqlQueryResult Compile();
        TQuery Limit(int limit);
    }

}
