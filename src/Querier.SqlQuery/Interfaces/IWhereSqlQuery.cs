using Querier.SqlQuery.Functions;
using Querier.SqlQuery.Models;

namespace Querier.SqlQuery.Interfaces
{
    public interface IWhereSqlQuery<TQuery>
    {
        TQuery Where(string column);
        TQuery Where<T>(string column, T value);
        TQuery Where(Func<IFunction, IFunction> function);
        TQuery Equal<T>(T value);
        TQuery WhereEqual<T>(string column, T value);
        TQuery NotEqual<T>(T value);
        TQuery WhereNotEqual<T>(string column, T value);
        TQuery Greater<T>(T value);
        TQuery WhereGreater<T>(string column, T value);
        TQuery NotGreater<T>(T value);
        TQuery WhereNotGreater<T>(string column, T value);
        TQuery GreaterOrEqual<T>(T value);
        TQuery WhereGreaterOrEqual<T>(string column, T value);
        TQuery NotGreaterOrEqual<T>(T value);
        TQuery WhereNotGreaterOrEqual<T>(string column, T value);
        TQuery Less<T>(T value);
        TQuery WhereLess<T>(string column, T value);
        TQuery NotLess<T>(T value);
        TQuery WhereNotLess<T>(string column, T value);
        TQuery LessOrEqual<T>(T value);
        TQuery WhereLessOrEqual<T>(string column, T value);
        TQuery NotLessOrEqual<T>(T value);
        TQuery WhereNotLessOrEqual<T>(string column, T value);
        TQuery Like<T>(T value);
        TQuery WhereLike<T>(string column, T value);
        TQuery NotLike<T>(T value);
        TQuery WhereNotLike<T>(string column, T value);
        TQuery Starts<T>(T value);
        TQuery WhereStarts<T>(string column, T value);
        TQuery NotStarts<T>(T value);
        TQuery WhereNotStarts<T>(string column, T value);
        TQuery Ends<T>(T value);
        TQuery WhereEnds<T>(string column, T value);
        TQuery NotEnds<T>(T value);
        TQuery WhereNotEnds<T>(string column, T value);
        TQuery Between<T>(T value, T secondValue);
        TQuery WhereBetween<T>(string column, T value, T secondValue);
        TQuery NotBetween<T>(T value, T secondValue);
        TQuery WhereNotBetween<T>(string column, T value, T secondValue);
        TQuery Null();
        TQuery WhereNull(string column);
        TQuery NotNull();
        TQuery WhereNotNull(string column);
        TQuery In<T>(IEnumerable<T> value);
        TQuery WhereIn<T>(string column, IEnumerable<T> value);
        TQuery NotIn<T>(IEnumerable<T> value);
        TQuery WhereNotIn<T>(string column, IEnumerable<T> value);
        TQuery All(string @operator, Func<TQuery, TQuery> query);
        TQuery WhereAll(string column, string @operator, Func<TQuery, TQuery> query);
        TQuery Any(string @operator, Func<TQuery, TQuery> query);
        TQuery WhereAny(string column, string @operator, Func<TQuery, TQuery> query);
        TQuery WhereExists(Func<TQuery, TQuery> query);
        TQuery WhereNotExists(Func<TQuery, TQuery> query);

        TQuery Or();
        TQuery Or<T>(T value);
        TQuery Or<T>(T value, T? secondValue);
        TQuery And();
        TQuery And<T>(T value);
        TQuery And<T>(T value, T? secondValue);
    }
}