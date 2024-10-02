using Querier.SqlQuery.Models;

namespace Querier.SqlQuery.Interfaces
{
    public interface IBaseQuery<TQuery> :
        IFromSqlQuery<TQuery>, ISelectSqlQuery<TQuery>, IJoinSqlQuery<TQuery>, IWhereSqlQuery<TQuery>, IGroupSqlQuery<TQuery>, IUnionSqlQuery<TQuery>, IOrderSqlQuery<TQuery> where TQuery : IBaseQuery<TQuery>
    {
        TQuery New();
        SqlQueryResult Compile();
        SqlQueryResult CompileSql(SqlQueryResult result);
        TQuery Limit(int limit);
    }

}
