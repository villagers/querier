using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querier.SqlQuery.Interfaces
{
    public interface IFromSqlQuery<TQuery> where TQuery : IBaseQuery<TQuery>, new()
    {
        TQuery From(string table, string? tableAs = null);
        TQuery From(Func<TQuery, TQuery> query, string? tableAs = null);
    }
}
