using Querier.SqlQuery.Functions;
using Querier.SqlQuery.Interfaces;
using Querier.SqlQuery.Models;
using Querier.SqlQuery.Operators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querier.SqlQuery
{
    public class SqlWhere : ISqlQueryCompile<SqlOperatorResult>
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

        public virtual SqlOperatorResult Compile()
        {
            return Operator.Compile();
        }
    }
}
