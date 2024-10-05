using Querier.SqlQuery.Models;

namespace Querier.SqlQuery.Interfaces
{
    public interface IWhereShorthandSqlQuery<TQuery>
    {
        TQuery Where(string column);
        TQuery Where(SqlColumn column);

        TQuery Equal<T>(T value);
        TQuery Equal(string sql);
        TQuery NotEqual<T>(T value);
        TQuery NotEqual(string sql);
        TQuery Greater<T>(T value);
        TQuery Greater(string sql);
        TQuery NotGreater<T>(T value);
        TQuery NotGreater(string sql);
        TQuery GreaterOrEqual<T>(T value);
        TQuery GreaterOrEqual(string sql);
        TQuery NotGreaterOrEqual<T>(T value);
        TQuery NotGreaterOrEqual(string sql);
        TQuery Less<T>(T value);
        TQuery Less(string sql);
        TQuery NotLess<T>(T value);
        TQuery NotLess(string sql);
        TQuery LessOrEqual<T>(T value);
        TQuery LessOrEqual(string sql);
        TQuery NotLessOrEqual<T>(T value);
        TQuery NotLessOrEqual(string sql);
        TQuery Like<T>(T value);
        TQuery Like(string sql);
        TQuery NotLike<T>(T value);
        TQuery NotLike(string sql);
        TQuery Starts<T>(T value);
        TQuery Starts(string sql);
        TQuery NotStarts<T>(T value);
        TQuery NotStarts(string sql);
        TQuery Ends<T>(T value);
        TQuery Ends(string sql);
        TQuery NotEnds<T>(T value);
        TQuery NotEnds(string sql);
        TQuery Between<T>(T value, T secondValue);
        TQuery Between(string sql);
        TQuery NotBetween<T>(T value, T secondValue);
        TQuery NotBetween(string sql);
        TQuery Null();
        TQuery NotNull();
        TQuery In<T>(IEnumerable<T> value);
        TQuery In(string sql);
        TQuery NotIn<T>(IEnumerable<T> value);
        TQuery NotIn(string sql);
        TQuery Or();
        TQuery Or<T>(T value);
        TQuery Or<T>(T value, T? secondValue);
        TQuery And();
        TQuery And<T>(T value);
        TQuery And<T>(T value, T? secondValue);
    }
}
