using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querier.SqlQuery.Interfaces
{
    public interface ISelectQuery<TQuery> where TQuery : IQuery<TQuery>, new()
    {
        TQuery Select();
        TQuery Select(string column, string? columnAs = null);
        TQuery Select(string aggregation, string column, string? columnAs = null);
        TQuery Select(Func<TQuery, TQuery> query, string? queryAs = null);
        TQuery SelectAvg(string column, string? columnAs = null);
        TQuery SelectCount(string column = "*", string? columnAs = null);
        TQuery SelectMax(string column, string? columnAs = null);
        TQuery SelectMin(string column, string? columnAs = null);
        TQuery SelectSum(string column, string? columnAs = null);

        TQuery Distinct();
    }
}
