using Querier.SqlQuery.Interfaces;
using Querier.SqlQuery.Models;

namespace Querier.SqlQuery.Functions
{
    public interface IFunction : IDateFunction, ISqlQueryCompile<SqlQueryResult>
    {
        IFunction New();
        SqlQueryResult Compile(ISqlTable table);
    }
}
