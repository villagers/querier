using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querier.SqlQuery.Interfaces
{
    public interface IFromQuery<TQuery> where TQuery : IQuery<TQuery>, new()
    {
        TQuery From(string table, string? tableAs = null);
        TQuery From(Func<TQuery, TQuery> query, string? tableAs = null);
    }
}
