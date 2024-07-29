using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querier
{
    public class QueryResult
    {
        public object? Data { get; set; }
        public required List<QueryFilter> Filters { get; set; }
        public required List<QueryProperty> Measures { get; set; }
        public required List<QueryProperty> Dimensions { get; set; }
        public required List<QueryProperty> TimeDimensions { get; set; }
    }
}
