using Querier.SqlQuery.Functions;

namespace Querier.SqlQuery.Interfaces
{
    public interface IGroupSqlQuery<TQuery>
    {
        TQuery GroupBy();
        TQuery GroupBy(int orderId);
        TQuery GroupBy(string column);
        TQuery GroupBy(Func<IFunction, IFunction> function);

        TQuery GroupByRaw(string sql);
    }
}
