using Querier.SqlQuery.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querier.SqlQuery.Interfaces
{
    public interface IQuery<TQuery> : IFromQuery<TQuery>, ISelectQuery<TQuery>, IWhereQuery<TQuery>, IGroupQuery<TQuery>, IOrderQuery<TQuery> where TQuery : IQuery<TQuery>, new()
    {
        TQuery New();
        TQuery Parent(TQuery parentQuery);
        SqlQueryResult Compile();
    }

}
