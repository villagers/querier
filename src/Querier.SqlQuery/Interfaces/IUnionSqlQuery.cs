namespace Querier.SqlQuery.Interfaces
{
    public interface IUnionSqlQuery<TQuery>
    {
        TQuery Union(TQuery query);
        TQuery UnionAll(TQuery query);
    }
}
