using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querier.SqlQuery.Interfaces
{
    public interface IOrderSqlQuery<TQuery> where TQuery : IBaseQuery<TQuery>, new()
    {
        TQuery OrderBy(string column, string? order = "asc");
    }
}
