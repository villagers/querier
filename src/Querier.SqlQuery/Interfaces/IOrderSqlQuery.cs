namespace Querier.SqlQuery.Interfaces
{
    public interface IOrderSqlQuery<TQuery>
    {
        TQuery OrderBy(int orderId, string? order = "asc");
        TQuery OrderBy(string column, string? order = "asc");
    }
}
