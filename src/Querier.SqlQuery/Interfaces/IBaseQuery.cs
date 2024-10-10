using Querier.SqlQuery.Models;

namespace Querier.SqlQuery.Interfaces
{
    public interface IBaseQuery<TQuery> :
        IFromSqlQuery<TQuery>,
        ICteSqlQuery<TQuery>,
        ISelectSqlQuery<TQuery>,
        IJoinSqlQuery<TQuery>,
        IWhereSqlQuery<TQuery>,
        IWhereShorthandSqlQuery<TQuery>,
        IGroupSqlQuery<TQuery>,
        IUnionSqlQuery<TQuery>,
        IOrderSqlQuery<TQuery>,
        IRawSqlQuery<TQuery> where TQuery : IBaseQuery<TQuery>
    {
        TQuery New();
        SqlQueryResult Compile();
        TQuery CompileFull(SqlQueryResult result);
        SqlQueryResult CompileSql(SqlQueryResult result);
        TQuery Limit(int limit);
    }

}
