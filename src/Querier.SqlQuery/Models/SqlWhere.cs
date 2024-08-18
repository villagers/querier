using Querier.SqlQuery.Operators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querier.SqlQuery
{
    public class SqlWhere
    {
        public readonly string Column;
        public AbstractOperator Operator;
        public SqlWhere(string column, AbstractOperator @operator)
        {
            Column = column;
            Operator = @operator;
        }
        public SqlWhere Clone()
        {
            return new SqlWhere(Column, (AbstractOperator)Operator.Clone());
        }
    }
}
