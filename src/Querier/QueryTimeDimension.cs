using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querier
{
    public class QueryTimeDimension
    {
        public required string Property { get; set; }
        public string? OrderBy { get; set; }
        public string? TimeDimensionPart { get; set; }
    }
}
