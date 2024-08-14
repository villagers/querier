using Querier.SqlQuery.Models;

namespace Querier.SqlQuery.Interfaces
{
    public interface IWhereQuery<TQuery> where TQuery : IQuery<TQuery>, new()
    {
        TQuery Where(string column);
        TQuery Where(string column, object value);
        TQuery Equal(object value);
        TQuery WhereEqual(string column, object value);
        TQuery NotEqual(object value);
        TQuery WhereNotEqual(string column, object value);
        TQuery Greater(object value);
        TQuery WhereGreater(string column, object value);
        TQuery NotGreater(object value);
        TQuery WhereNotGreater(string column, object value);
        TQuery GreaterOrEqual(object value);
        TQuery WhereGreaterOrEqual(string column, object value);
        TQuery NotGreaterOrEqual(object value);
        TQuery WhereNotGreaterOrEqual(string column, object value);
        TQuery Less(object value);
        TQuery WhereLess(string column, object value);
        TQuery NotLess(object value);
        TQuery WhereNotLess(string column, object value);
        TQuery LessOrEqual(object value);
        TQuery WhereLessOrEqual(string column, object value);
        TQuery NotLessOrEqual(object value);
        TQuery WhereNotLessOrEqual(string column, object value);
        TQuery Like(object value);
        TQuery WhereLike(string column, object value);
        TQuery NotLike(object value);
        TQuery WhereNotLike(string column, object value);
        TQuery Starts(object value);
        TQuery WhereStarts(string column, object value);
        TQuery NotStarts(object value);
        TQuery WhereNotStarts(string column, object value);
        TQuery Ends(object value);
        TQuery WhereEnds(string column, object value);
        TQuery NotEnds(object value);
        TQuery WhereNotEnds(string column, object value);
        TQuery Between(object value, object secondValue);
        TQuery WhereBetween(string column, object value, object secondValue);
        TQuery NotBetween(object value, object secondValue);
        TQuery WhereNotBetween(string column, object value, object secondValue);
        TQuery Null();
        TQuery WhereNull(string column);
        TQuery NotNull();
        TQuery WhereNotNull(string column);
        TQuery In(object value);
        TQuery WhereIn(string column, object value);
        TQuery NotIn(object value);
        TQuery WhereNotIn(string column, object value);


        TQuery All(string column, string @operator, Func<TQuery, TQuery> query);
        TQuery WhereAll(string column, string @operator, Func<TQuery, TQuery> query);
        TQuery NotAll(string column, string @operator, Func<TQuery, TQuery> query);
        TQuery WhereNotAll(string column, string @operator, Func<TQuery, TQuery> query);
        TQuery WhereAny(string column, string @operator, TQuery query);
        TQuery WhereExists(string column, TQuery query);
        TQuery WhereNotAny(string column, string @operator, TQuery query);
        TQuery WhereNotExists(string column, TQuery query);
        TQuery WhereNotSome(string column, string @operator, TQuery query);
        TQuery WhereSome(string column, string @operator, TQuery query);
        TQuery Any(string column, string @operator, TQuery query);
        TQuery Exists(string column, TQuery query);
        TQuery NotAny(string column, string @operator, TQuery query);
        TQuery NotExists(string column, TQuery query);
        TQuery NotSome(string column, string @operator, TQuery query);
        TQuery Some(string column, string @operator, TQuery query);
        TQuery Or();
        TQuery Or(object value, object? secondValue = null);
        TQuery And();
        TQuery And(object value, object? secondValue = null);
    }
}