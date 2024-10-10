namespace Querier.SqlQuery.Interfaces
{
    public interface ICteSqlQuery<TQuery>
    {
        TQuery With(string name, Func<TQuery, TQuery> query);
        TQuery WithRecursive(string name, Func<TQuery, TQuery> query);
    }
}
