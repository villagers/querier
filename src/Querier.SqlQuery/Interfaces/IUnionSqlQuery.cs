using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querier.SqlQuery.Interfaces
{
    public interface IUnionSqlQuery<TQuery>
    {
        TQuery Union(TQuery query);
        TQuery UnionAll(TQuery query);
    }
}
