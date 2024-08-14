using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querier.SqlQuery.Interfaces
{
    public interface IOrderQuery<TQuery> where TQuery : IQuery<TQuery>, new()
    {
        TQuery OrderBy(string column, string? order = "asc");
    }
}
