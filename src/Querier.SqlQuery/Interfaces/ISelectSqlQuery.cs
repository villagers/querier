using Querier.SqlQuery.Operators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querier.SqlQuery.Interfaces
{
    public interface ISelectSqlQuery<TQuery>
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
        TQuery SelectCase<T>(string column, T value, object thenValue, object? elseValue = null);

        TQuery SelectSecond(string column, string? columnAs = null);
        TQuery SelectMinute(string column, string? columnAs = null);
        TQuery SelectHour(string column, string? columnAs = null);
        TQuery SelectDay(string column, string? columnAs = null);
        TQuery SelectDate(string column, string? columnAs = null);
        TQuery SelectMonth(string column, string? columnAs = null);
        TQuery SelectYear(string column, string? columnAs = null);

        TQuery SelectRaw(string sql);

        TQuery Distinct();
    }
}
