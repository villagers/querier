using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querier.Descriptors
{
    public interface IAggregationDescriptor
    {
        string? Aggregation { get; set; }
    }
}
