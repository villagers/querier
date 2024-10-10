using Querier.SqlQuery.Models;

namespace Querier.SqlQuery.Interfaces
{
    public interface ISqlJoin : ISqlQueryCompile<SqlQueryResult>
    {
        ISqlJoin On(string column, string referenceColumn);
        ISqlJoin On<T>(string table, string column, T columnValue);

    }
}
