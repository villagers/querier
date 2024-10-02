using Querier.SqlQuery.Functions;

namespace Querier.Interfaces
{
    public interface IQueryFilter
    {
        IQueryFilter New();
        IQueryFilter In<T>(string column, IEnumerable<T> value);
        IQueryFilter In<T>(Func<IFunction, IFunction> function, IEnumerable<T> value);
        IQueryFilter Equal<T>(string column, T value);
        IQueryFilter Equal<T>(Func<IFunction, IFunction> function, T value);
        IQueryFilter Contains<T>(string column, T value);
        IQueryFilter Contains<T>(Func<IFunction, IFunction> function, T value);
        IQueryFilter StartsWith<T>(string column, T value);
        IQueryFilter StartsWith<T>(Func<IFunction, IFunction> function, T value);
        IQueryFilter EndsWith<T>(string column, T value);
        IQueryFilter EndsWith<T>(Func<IFunction, IFunction> function, T value);
        IQueryFilter Greater<T>(string column, T value);
        IQueryFilter Greater<T>(Func<IFunction, IFunction> function, T value);
        IQueryFilter GreaterOrEqual<T>(string column, T value);
        IQueryFilter GreaterOrEqual<T>(Func<IFunction, IFunction> function, T value);
        IQueryFilter Less<T>(string column, T value);
        IQueryFilter Less<T>(Func<IFunction, IFunction> function, T value);
        IQueryFilter LessOrEqual<T>(string column, T value);
        IQueryFilter LessOrEqual<T>(Func<IFunction, IFunction> function, T value);
        IQueryFilter And();
        IQueryFilter And<T>(T value);
        IQueryFilter Or();
        IQueryFilter Or<T>(T value);
    }
}
