using Querier.SqlQuery.Functions;
using Querier.SqlQuery.Models;
using Querier.SqlQuery.Operators;

namespace Querier.SqlQuery.Interfaces
{
    public interface IWhereSqlQuery<TQuery>
    {
        TQuery Where<T>(string column, T value);
        TQuery WhereOperator(string column, AbstractOperator @operator);
        TQuery WhereEqual<T>(string column, T value);
        TQuery WhereEqual<T>(Func<IFunction, IFunction> function, T value);
        TQuery WhereNotEqual<T>(string column, T value);
        TQuery WhereGreater<T>(string column, T value);
        TQuery WhereGreater<T>(Func<IFunction, IFunction> function, T value);
        TQuery WhereNotGreater<T>(string column, T value);
        TQuery WhereGreaterOrEqual<T>(string column, T value);
        TQuery WhereGreaterOrEqual<T>(Func<IFunction, IFunction> function, T value);
        TQuery WhereNotGreaterOrEqual<T>(string column, T value);
        TQuery WhereLess<T>(string column, T value);
        TQuery WhereLess<T>(Func<IFunction, IFunction> function, T value);
        TQuery WhereNotLess<T>(string column, T value);
        TQuery WhereLessOrEqual<T>(string column, T value);
        TQuery WhereLessOrEqual<T>(Func<IFunction, IFunction> function, T value);
        TQuery WhereNotLessOrEqual<T>(string column, T value);
        TQuery WhereLike<T>(string column, T value);
        TQuery WhereLike<T>(Func<IFunction, IFunction> function, T value);
        TQuery WhereNotLike<T>(string column, T value);
        TQuery WhereStarts<T>(string column, T value);
        TQuery WhereStarts<T>(Func<IFunction, IFunction> function, T value);
        TQuery WhereNotStarts<T>(string column, T value);
        TQuery WhereEnds<T>(string column, T value);
        TQuery WhereEnds<T>(Func<IFunction, IFunction> function, T value);
        TQuery WhereNotEnds<T>(string column, T value);
        TQuery WhereBetween<T>(string column, T value, T secondValue);
        TQuery WhereNotBetween<T>(string column, T value, T secondValue);
        TQuery WhereNull(string column);
        TQuery WhereNotNull(string column);
        TQuery WhereIn<T>(string column, IEnumerable<T> value);
        TQuery WhereIn<T>(Func<IFunction, IFunction> function, IEnumerable<T> value);
        TQuery WhereNotIn<T>(string column, IEnumerable<T> value);
        TQuery WhereRaw(string sql);
    }
}