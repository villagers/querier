using Querier.SqlQuery.Functions;

namespace Querier.Interfaces
{
    public interface IQueryFilter
    {
        IQueryFilter New();
        IQueryFilter In<T>(IEnumerable<T> value);
        IQueryFilter Equal<T>(T value);
        IQueryFilter Equal(string rawSql);
        IQueryFilter Contains<T>(T value);
        IQueryFilter Contains(string rawSql);
        IQueryFilter StartsWith<T>(T value);
        IQueryFilter StartsWith(string rawSql);
        IQueryFilter EndsWith<T>(T value);
        IQueryFilter EndsWith(string rawSql);
        IQueryFilter Greater<T>(T value);
        IQueryFilter Greater(string rawSql);
        IQueryFilter GreaterOrEqual<T>(T value);
        IQueryFilter GreaterOrEqual(string rawSql);
        IQueryFilter Less<T>(T value);
        IQueryFilter Less(string rawSql);
        IQueryFilter LessOrEqual<T>(T value);
        IQueryFilter LessOrEqual(string rawSql);
        IQueryFilter And();
        IQueryFilter And<T>(T value);
        IQueryFilter Or();
        IQueryFilter Or<T>(T value);
    }
}
