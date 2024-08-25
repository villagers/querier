using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querier.SqlQuery.Operators
{
    public abstract class AbstractComparisonOperator<T> : AbstractOperator
    {
        public T Value { get; set; }
    }
}
