namespace Querier.SqlQuery.Interfaces
{
    public interface IJoinSqlQuery<TQuery>
    {
        TQuery Join(string table, string tableProperty, string property);
    }
}
