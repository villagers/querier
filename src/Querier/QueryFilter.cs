using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querier
{
    public class QueryFilter
    {
        public required string Property { get; set; }
        public required string Operator { get; set; }
        public required object? Args { get; set; }
    }
}
