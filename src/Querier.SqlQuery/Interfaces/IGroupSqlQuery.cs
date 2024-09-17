using Querier.SqlQuery.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querier.SqlQuery.Interfaces
{
    public interface IGroupSqlQuery<TQuery>
    {
        TQuery GroupBy(string column);
        TQuery GroupBy(Func<IFunction, IFunction> function);

        TQuery GroupByRaw(string sql);
    }
}
