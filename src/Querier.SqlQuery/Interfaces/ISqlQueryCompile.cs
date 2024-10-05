namespace Querier.SqlQuery.Interfaces
{
    public interface ISqlQueryCompile<TResult> where TResult : class
    {
        TResult Compile(ISqlTable table);
    }
}
