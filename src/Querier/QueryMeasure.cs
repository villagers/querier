using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querier
{
    public class QueryMeasure<TEntity>
    {
        public required string Property { get; set; }
        public string? OrderBy { get; set; }
    }
}
