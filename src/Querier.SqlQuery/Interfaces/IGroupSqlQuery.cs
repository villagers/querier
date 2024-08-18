using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querier.SqlQuery.Interfaces
{
    public interface IGroupSqlQuery<TQuery> where TQuery : IBaseQuery<TQuery>, new()
    {
        TQuery GroupBy(string column);
    }
}
