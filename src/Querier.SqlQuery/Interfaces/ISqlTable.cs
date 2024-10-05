using Querier.SqlQuery.Models;

namespace Querier.SqlQuery.Interfaces
{
    public interface ISqlTable : ISqlQueryCompile<SqlQueryResult>
    {
        string TableOrAlias { get; }
    }
}
