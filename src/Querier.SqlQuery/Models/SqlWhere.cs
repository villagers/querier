using Querier.SqlQuery.Extensions;
using Querier.SqlQuery.Interfaces;
using Querier.SqlQuery.Models;
using Querier.SqlQuery.Operators;

namespace Querier.SqlQuery
{
    public class SqlWhere : ISqlQueryCompile<SqlQueryResult>
    {
        public AbstractOperator Operator;
        

        public SqlWhere() { }
        public SqlWhere(AbstractOperator @operator)
        {
            Operator = @operator;
        }
        public SqlWhere Clone()
        {
            return new SqlWhere((AbstractOperator)Operator.Clone());
        }

        public virtual SqlQueryResult Compile(ISqlTable table) => Operator.Compile(table).Enumerate();
    }
}
