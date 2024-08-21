using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querier.SqlQuery.Operators
{
    public abstract class AbstractComparisonOperator : AbstractOperator
    {
        public required string Column { get; set; }
        public required object Value { get; set; }

    }
}
